
namespace CurrencyConverter
{
	public class ConversionModel
	{
		public decimal OriginalValue  { get; set; }

        public decimal ConvertedValue { get; set; }

        public CurrencyCode CurrencyFrom { get; set; }

        public CurrencyCode CurrencyTo { get; set; }

		public DateTime? ConversionDate { get; set; }

		public decimal ExchangeRate { get; set; }
	}
}