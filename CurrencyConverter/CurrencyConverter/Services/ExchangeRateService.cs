using System.Text;
using CurrencyConverter.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


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

namespace CurrencyConverter
{
	public class ExchangeRateService : IExchangeRateService
	{
		private readonly HttpClient httpClient;

		private const string ExchangeRatePrefix = "FX";
        private const string ResultJsonExchangePath = "observations[0]";

        public ExchangeRateService()
		{
			httpClient = new HttpClient()
			{
				BaseAddress = new Uri("https://www.bankofcanada.ca/valet/observations/")
            };
		}

        public async Task<ExchangeRateResult> GetExchangeRate(CurrencyCode fromCurrencyCode, CurrencyCode toCurrencyCode, DateTime? date)
		{
			string exchangeSeries = $"{ExchangeRatePrefix}{fromCurrencyCode}{toCurrencyCode}";
            ExchangeRateResult result = new ExchangeRateResult();                

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

			if (response.IsSuccessStatusCode)
			{
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

                            if (exchangeDateToken != null)
                            {
                                result.Date = exchangeDateToken.ToObject<DateTime>();
                            }

                            if (exchangeRateToken != null)
							{
                                result.Rate = exchangeRateToken.ToObject<decimal>();
							}
						}
                    }
                }
            }

			return result;
		}

    }
}