
namespace CurrencyConverter
{
	public interface ICurrencyConverter
	{
		Task<ConversionResult> ConvertCurrency(ConversionInput model);
	}
}