namespace CurrencyConverter
{
	public interface IExchangeRateService
	{
        Task<decimal> GetExchangeRate(CurrencyCode fromCurrencyCode, CurrencyCode toCurrencyCode, DateTime date);
	}
}

