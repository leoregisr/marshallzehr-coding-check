using Microsoft.Extensions.DependencyInjection;

namespace CurrencyConverter;

class Program
{
    static async Task Main(string[] args)
    {
        var converter = GetCurrencyConverter();

        var conversionInput = ParseUserInput(args);

        var result = await converter.ConvertCurrency(conversionInput);

        OutputResult(result);

        Console.ReadKey();
    }

    private static ICurrencyConverter GetCurrencyConverter()
    {
        ServiceProvider serviceProvider = BuildServiceProvider();

        ICurrencyConverter? currencyConverter = serviceProvider?.GetService<ICurrencyConverter>();

        if (currencyConverter is null)
        {
            throw new Exception();
        }

        return currencyConverter;
    }

    private static ServiceProvider BuildServiceProvider()
    {
        return new ServiceCollection()
            .AddSingleton<IExchangeRateService, ExchangeRateService>()
            .AddSingleton<ICurrencyConverter, CurrencyConverter>()
            .BuildServiceProvider();
    }

    private static ConversionInput ParseUserInput(string[] args)
    {
        return new ConversionInput
        {
            ExchangeDate = DateTime.Now,
            Value = 1,
            CurrencyFrom = CurrencyCode.USD,
            CurrencyTo = CurrencyCode.CAD,

        };
    }

    private static void OutputResult(ConversionResult result)
    {
        Console.WriteLine($"Original Value: {result.OriginalValue} {result.CurrencyFrom}");
        Console.WriteLine($"Converted Value: {result.ConvertedValue} {result.CurrencyTo}");
        Console.WriteLine($"Exchange Date: {result.ExchangeDate}");
        Console.WriteLine($"Exchange Rate: {result.ExchangeRate}");
    }
}
