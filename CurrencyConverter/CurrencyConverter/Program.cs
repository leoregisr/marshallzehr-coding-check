using System.Globalization;
using Microsoft.Extensions.DependencyInjection;

namespace CurrencyConverter;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("--- Welcome to the Canadian Currency Converter ---");

        var converter = GetCurrencyConverter();

        while (true)
        {
            var conversionInput = GetUserInput();

            var result = await converter.ConvertCurrency(conversionInput);

            OutputResult(result);

            Console.WriteLine("--- Press ESC to exit the application or press any other key to continue ---");

            var keyInfo = Console.ReadKey();

            if (keyInfo.Key == ConsoleKey.Escape)
                break;
        }        
    }

    private static ConversionInput GetUserInput()
    {
        var inputModel = new ConversionInput();

        var convertedCurrency = GetConvertedCurrency();
        var isConvertingToCAD = GetConversionDirection(convertedCurrency);

        if (isConvertingToCAD)
        {
            inputModel.CurrencyFrom = convertedCurrency;
            inputModel.CurrencyTo = CurrencyCode.CAD;
        }
        else
        {
            inputModel.CurrencyFrom = CurrencyCode.CAD;
            inputModel.CurrencyTo = convertedCurrency;
        }

        inputModel.Value = GetValueToBeConverted();

        inputModel.ExchangeDate = GetExchangeDate();

        return inputModel;
    }

    private static CurrencyCode GetConvertedCurrency()
    {
        Console.WriteLine();
        Console.WriteLine("What currency do you want to convert?");

        var currencyCodes = Enum.GetValues<CurrencyCode>();

        //ignore CAD because it has to be chosen based on conversion direction
        for (int i = 1; i < currencyCodes.Length; i++)
        {
            Console.WriteLine($"{i} - {currencyCodes[i]}");
        }

        Console.Write("Type the selected option number: ");

        var selectedOptionText = Console.ReadLine();

        //validate user input
        if (string.IsNullOrEmpty(selectedOptionText) ||
            !int.TryParse(selectedOptionText, out int selectedCurrency) ||
            selectedCurrency < 1 ||
            selectedCurrency > 10)
        {
            Console.WriteLine("The selected option is not valid.");
            return GetConvertedCurrency();
        }

        return currencyCodes[selectedCurrency];
    }

    private static bool GetConversionDirection(CurrencyCode currencyCode)
    {
        Console.WriteLine();
        Console.WriteLine($"Do you want to convert TO Canadian dollars or FROM Canadian dollars?");        

        Console.WriteLine($"1 - {currencyCode} to CAD");
        Console.WriteLine($"2 - From CAD to {currencyCode}");

        Console.Write("Type the selected option number: ");

        var selectedOptionText = Console.ReadLine();

        //validate user input
        if (string.IsNullOrEmpty(selectedOptionText) ||
            !int.TryParse(selectedOptionText, out int selectedOption) ||
            selectedOption < 1 ||
            selectedOption > 2)
        {
            Console.WriteLine("The selected option is not valid.");
            return GetConversionDirection(currencyCode);
        }

        return selectedOption == 1;
    }

    private static decimal GetValueToBeConverted()
    {
        Console.WriteLine();
        Console.WriteLine("How much do you want to convert?");
        Console.Write("Type the value: ");

        var userInput = Console.ReadLine();

        if (string.IsNullOrEmpty(userInput))
            return GetValueToBeConverted();

        if (!decimal.TryParse(userInput, out decimal result))
        {
            Console.WriteLine("The value inserted is not a valid currency value.");
            return GetValueToBeConverted();
        }

        return result;
    }

    private static DateTime? GetExchangeDate()
    {
        Console.WriteLine();
        Console.Write("Do you want to specify an exchange date? If not, the most recent published rate will be used.");
        Console.WriteLine("(Bank of Canada publishes daily rates once each business day by 16:30 ET.)");

        Console.WriteLine("1 - Yes");
        Console.WriteLine("2 - No");

        Console.Write("Type the selected option number: ");

        var selectedOptionText = Console.ReadLine();

        //validate user input
        if (string.IsNullOrEmpty(selectedOptionText) ||
            !int.TryParse(selectedOptionText, out int selectedOption) ||
            selectedOption < 1 ||
            selectedOption > 2)
        {
            Console.WriteLine("The selected option is not valid.");
            return GetExchangeDate();
        }

        if (selectedOption == 2)
            return null;

        return GetExchangeDateUserValue();
    }

    private static DateTime? GetExchangeDateUserValue()
    {
        Console.Write("Type the exchange date (format: MM/DD/YYYY): ");

        var dateText = Console.ReadLine();

        //validate user input
        if (string.IsNullOrEmpty(dateText) ||
            !DateTime.TryParseExact(dateText, "MM/dd/yyyy", CultureInfo.InvariantCulture,
                                 DateTimeStyles.None, out DateTime exchangeDate))
        {
            Console.WriteLine("The inserted date is not valid.");
            return GetExchangeDateUserValue();
        }

        return exchangeDate;
    }

    private static void OutputResult(ConversionResult result)
    {
        Console.WriteLine();
        Console.WriteLine("--- Conversion Results ---");
        Console.WriteLine($"Original Value: {result.OriginalValue:F4} {result.CurrencyFrom}");
        Console.WriteLine($"Converted Value: {result.ConvertedValue:F4} {result.CurrencyTo}");
        Console.WriteLine($"Exchange Date: {result.ExchangeDate:MM/dd/yyyy}");
        Console.WriteLine($"Exchange Rate: {result.ExchangeRate:F4}");
        Console.WriteLine("--------------------------");        
    }

    #region DI

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

    #endregion    
}
