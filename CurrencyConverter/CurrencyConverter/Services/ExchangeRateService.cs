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

        public ExchangeRateService()
		{
			httpClient = new HttpClient()
			{
				BaseAddress = new Uri("https://www.bankofcanada.ca/valet/observations/")
            };
		}

        public async Task<decimal> GetExchangeRate(CurrencyCode fromCurrencyCode, CurrencyCode toCurrencyCode, DateTime date)
		{
			string exchangeSeries = $"{ExchangeRatePrefix}{fromCurrencyCode}{toCurrencyCode}";

			string dateFilter = date.ToString("yyyy-MM-dd");

			var response = await httpClient.GetAsync($"{exchangeSeries}/json?start_date={dateFilter}&end_date={dateFilter}");

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
							var exchangeRateToken = token.SelectToken($"observations[0].{exchangeSeries}.v");

                            if (exchangeRateToken != null)
							{
								return exchangeRateToken.ToObject<decimal>();
							}
						}
                    }
                }
            }

			return 1;
		}

    }
}