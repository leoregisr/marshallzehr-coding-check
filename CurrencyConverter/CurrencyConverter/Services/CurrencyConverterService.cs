using CurrencyConverter.Services;

namespace CurrencyConverter
{
    public class CurrencyConverterService : ICurrencyConverterService
    {
        private readonly IExchangeRateGateway _exchangeRateService;

        public CurrencyConverterService(IExchangeRateGateway exchangeRateService) 
		{
            _exchangeRateService = exchangeRateService;
        }

        public async Task<ConversionResult> ConvertCurrency(ConversionInput model)
        {
            var exchangeRate = await _exchangeRateService.GetExchangeRate(model.CurrencyFrom,
                model.CurrencyTo, model.ExchangeDate);

            var result = ConversionResult.FromInput(model);

            result.ConvertedValue = model.Value * exchangeRate.Rate;
            result.ExchangeRate = exchangeRate.Rate;
            result.ExchangeDate = exchangeRate.Date;

            return result;
        }
    }
}