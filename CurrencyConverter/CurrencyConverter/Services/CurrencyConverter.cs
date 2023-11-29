namespace CurrencyConverter
{
	public class CurrencyConverter : ICurrencyConverter
	{
        private readonly IExchangeRateService _exchangeRateService;

        public CurrencyConverter(IExchangeRateService exchangeRateService) 
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