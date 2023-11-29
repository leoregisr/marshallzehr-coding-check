namespace CurrencyConverter.Exceptions
{
	public class ExchangeRateNotFoundException : Exception
	{
		public ExchangeRateNotFoundException() :
			base($"No exchange rate was found for the specified parameters.")
		{
		}
	}
}

