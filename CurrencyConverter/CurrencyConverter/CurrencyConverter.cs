namespace CurrencyConverter
{
	public class CurrencyConverter : ICurrencyConverter
	{
        private readonly IExchangeRateService _exchangeRateService;

        public CurrencyConverter(IExchangeRateService exchangeRateService) 
		{
            _exchangeRateService = exchangeRateService;
        }

        public async Task<ConversionModel> ConvertCurrency(ConversionModel model)
        {
            var conversionDate = model.ConversionDate.HasValue ?
                model.ConversionDate.Value :
                DateTime.Now;

            var exchangeRate = await _exchangeRateService.GetExchangeRate(model.CurrencyFrom,
                model.CurrencyTo, conversionDate);

            model.ConvertedValue = model.OriginalValue * exchangeRate;
            model.ExchangeRate = exchangeRate;

            return model;
        }
    }
}