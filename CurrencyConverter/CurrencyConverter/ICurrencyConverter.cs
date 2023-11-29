
namespace CurrencyConverter
{
	public interface ICurrencyConverter
	{
		Task<ConversionModel> ConvertCurrency(ConversionModel model);
	}
}

