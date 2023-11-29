using Microsoft.Extensions.DependencyInjection;

namespace CurrencyConverter;

class Program
{
    static async Task Main(string[] args)
    {
        var service = GetCurrencyConverter();

        var result = await service.ConvertCurrency(new ConversionModel
        {
            ConversionDate = DateTime.Now,
            OriginalValue = 1,
            CurrencyFrom = CurrencyCode.USD,
            CurrencyTo = CurrencyCode.CAD,
            
        });

        Console.WriteLine($"Original Value: {result.OriginalValue} {result.CurrencyFrom}");
        Console.WriteLine($"Converted Value: {result.ConvertedValue} {result.CurrencyTo}");
        Console.WriteLine($"Exchange Date: {result.ConversionDate}");
        Console.WriteLine($"Exchange Rate: {result.ExchangeRate}");

        Console.ReadKey();
    }

    static ICurrencyConverter GetCurrencyConverter()
    {   
        ServiceProvider serviceProvider = new ServiceCollection()
            .AddSingleton<IExchangeRateService, ExchangeRateService>()
            .AddSingleton<ICurrencyConverter, CurrencyConverter>()
            .BuildServiceProvider();

        ICurrencyConverter? currencyConverter = serviceProvider?.GetService<ICurrencyConverter>();

        if (currencyConverter is null)
        {
            throw new Exception();
        }

        return currencyConverter;
    }
}

