using CurrencyConverter.Models;

namespace CurrencyConverter
{
	public interface IExchangeRateService
	{
        Task<ExchangeRateResult> GetExchangeRate(CurrencyCode fromCurrencyCode, CurrencyCode toCurrencyCode, DateTime? date);
	}
}

