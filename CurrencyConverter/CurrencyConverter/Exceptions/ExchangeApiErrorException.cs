namespace CurrencyConverter.Exceptions
{
	public class ExchangeApiErrorException : Exception
	{
		public ExchangeApiErrorException() : base("An unexpected error has occurred obtaining the exchange rate.")
		{
		}
	}
}

