namespace CurrencyConverter.Services
{
    public interface ICurrencyConverterService
    {
        Task<ConversionResult> ConvertCurrency(ConversionInput model);
    }
}