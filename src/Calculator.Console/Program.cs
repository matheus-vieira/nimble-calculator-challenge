using Calculator.Core.Exceptions;
using Calculator.Core.Interfaces;
using Calculator.Core.Services;
using Microsoft.Extensions.DependencyInjection;

// Set up dependency injection
var services = new ServiceCollection();
services.AddScoped<INumberParser, NumberParser>();
services.AddScoped<ICalculator>(sp => new CalculatorService(sp.GetRequiredService<INumberParser>()));

var serviceProvider = services.BuildServiceProvider();
var calculator = serviceProvider.GetRequiredService<ICalculator>();

Console.WriteLine("=== Nimble Calculator ===");
Console.WriteLine("Enter numbers separated by commas or newlines.");
Console.WriteLine("Type 'exit' to quit.\n");

while (true)
{
    Console.Write("Enter input: ");
    string? input = Console.ReadLine();

    if (input == null || input.Equals("exit", StringComparison.OrdinalIgnoreCase))
    {
        Console.WriteLine("Goodbye!");
        break;
    }

    try
    {
        int result = calculator.Add(input);
        Console.WriteLine($"Result: {result}\n");
    }
    catch (TooManyNumbersException ex)
    {
        Console.WriteLine($"Error: {ex.Message}\n");
    }
    catch (NegativeNumbersException ex)
    {
        Console.WriteLine($"Error: {ex.Message}\n");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Unexpected error: {ex.Message}\n");
    }
}
