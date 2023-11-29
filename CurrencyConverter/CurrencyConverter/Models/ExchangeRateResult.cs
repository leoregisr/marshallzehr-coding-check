namespace CurrencyConverter.Models
{
	public class ExchangeRateResult
	{
		public ExchangeRateResult()
		{
			Rate = 1;//Starts with 1, to be used in case of fail
		}

		public DateTime Date { get; set; }

		public decimal Rate { get; set; }
	}
}

