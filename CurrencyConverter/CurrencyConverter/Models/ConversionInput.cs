
namespace CurrencyConverter
{
	public class ConversionInput
	{
		public decimal Value  { get; set; }

        public CurrencyCode CurrencyFrom { get; set; }

        public CurrencyCode CurrencyTo { get; set; }

		public DateTime? ExchangeDate { get; set; }
	}
}