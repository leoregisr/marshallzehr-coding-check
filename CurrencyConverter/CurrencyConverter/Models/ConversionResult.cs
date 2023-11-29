
namespace CurrencyConverter
{
	public class ConversionResult
	{
		public decimal OriginalValue  { get; set; }

        public decimal ConvertedValue { get; set; }

        public CurrencyCode CurrencyFrom { get; set; }

        public CurrencyCode CurrencyTo { get; set; }

		public DateTime ExchangeDate { get; set; }

		public decimal ExchangeRate { get; set; }

        public static ConversionResult FromInput(ConversionInput model)
        {
            return new ConversionResult()
            {
                OriginalValue = model.Value,
                CurrencyFrom = model.CurrencyFrom,
                CurrencyTo = model.CurrencyTo,
                ExchangeDate = model.ExchangeDate ?? DateTime.Now
            };
        }
    }
}