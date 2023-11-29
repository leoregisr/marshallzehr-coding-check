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
            var exchangeDate = model.ExchangeDate.HasValue ?
                model.ExchangeDate.Value :
                DateTime.Now;

            var exchangeRate = await _exchangeRateService.GetExchangeRate(model.CurrencyFrom,
                model.CurrencyTo, exchangeDate);

            var result = ConversionResult.FromInput(model);

            result.ConvertedValue = model.Value * exchangeRate;
            result.ExchangeRate = exchangeRate;

            return result;
        }
    }
}