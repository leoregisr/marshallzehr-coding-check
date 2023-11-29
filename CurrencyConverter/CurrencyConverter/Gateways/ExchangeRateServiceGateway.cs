using System.Text;
using CurrencyConverter.Exceptions;
using CurrencyConverter.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CurrencyConverter.Gateways
{
    public class ExchangeRateServiceGateway : IExchangeRateGateway
    {
        private readonly HttpClient httpClient;

        private const string ExchangeRatePrefix = "FX";
        private const string ResultJsonExchangePath = "observations[0]";

        public ExchangeRateServiceGateway()
        {
            httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://www.bankofcanada.ca/valet/observations/")
            };
        }

        public async Task<ExchangeRateResult> GetExchangeRate(CurrencyCode fromCurrencyCode, CurrencyCode toCurrencyCode, DateTime? date)
        {
            string exchangeSeries = $"{ExchangeRatePrefix}{fromCurrencyCode}{toCurrencyCode}";            

            var requestUriBuilder = new StringBuilder($"{exchangeSeries}/json");

            if (date.HasValue)
            {
                string dateFilter = date.Value.ToString("yyyy-MM-dd");
                requestUriBuilder.Append($"?start_date={dateFilter}&end_date={dateFilter}");
            }
            else
            {
                requestUriBuilder.Append("?recent=1");
            }

            var response = await httpClient.GetAsync(requestUriBuilder.ToString());

            if (!response.IsSuccessStatusCode)
                throw new ExchangeApiErrorException();

            try
            {
                var result = new ExchangeRateResult();

                //reading stream to improve memory using
                using (var streamReader = new StreamReader(response.Content.ReadAsStream()))
                using (var jsonTextReader = new JsonTextReader(streamReader))
                {
                    while (jsonTextReader.Read())
                    {
                        if (jsonTextReader.TokenType == JsonToken.StartObject)
                        {
                            var token = JToken.Load(jsonTextReader);

                            var exchangeDateToken = token.SelectToken($"{ResultJsonExchangePath}.d");
                            var exchangeRateToken = token.SelectToken($"{ResultJsonExchangePath}.{exchangeSeries}.v");

                            if (exchangeDateToken == null || exchangeRateToken == null)
                                throw new ExchangeRateNotFoundException();

                            result.Date = exchangeDateToken.ToObject<DateTime>();
                            result.Rate = exchangeRateToken.ToObject<decimal>();
                        }
                    }
                }

                return result;
            }
            catch (ExchangeRateNotFoundException exRateNotFound)
            {
                throw exRateNotFound;
            }
            catch (Exception)
            {
                throw new ExchangeApiErrorException();
            }
        }
    }
}

/*
Response Example from Bank Of Canada exchange API
{
    "terms": {
        "url": "https://www.bankofcanada.ca/terms/"
    },
    "seriesDetail": {
        "FXUSDCAD": {
                "label": "USD/CAD",
                "description": "US dollar to Canadian dollar daily exchange rate",
                "dimension": {
                    "key": "d",
                    "name": "Date"
            }
        }
    },
    "observations": [
        {
            "d": "2023-11-28",
            "FXUSDCAD": {
                "v": "1.3581"
            }
        }
    ]
*/