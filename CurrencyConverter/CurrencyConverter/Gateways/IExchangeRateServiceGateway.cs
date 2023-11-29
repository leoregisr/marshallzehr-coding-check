using CurrencyConverter.Models;

namespace CurrencyConverter
{
	public interface IExchangeRateGateway
	{
        Task<ExchangeRateResult> GetExchangeRate(CurrencyCode fromCurrencyCode, CurrencyCode toCurrencyCode, DateTime? date);
	}
}

