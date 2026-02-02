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
Console.WriteLine("Type 'exit' to quit.");
Console.WriteLine("Type 'formula' to toggle formula display.\n");

bool showFormula = true; // Show formula by default

while (true)
{
    Console.Write("Enter input: ");
    string? input = Console.ReadLine();

    if (input == null || input.Equals("exit", StringComparison.OrdinalIgnoreCase))
    {
        Console.WriteLine("Goodbye!");
        break;
    }

    if (input.Equals("formula", StringComparison.OrdinalIgnoreCase))
    {
        showFormula = !showFormula;
        Console.WriteLine($"Formula display: {(showFormula ? "ON" : "OFF")}\n");
        continue;
    }

    try
    {
        if (showFormula)
        {
            var (result, formula) = calculator.AddWithFormula(input);
            Console.WriteLine($"Formula: {formula}");
            Console.WriteLine($"Result: {result}\n");
        }
        else
        {
            int result = calculator.Add(input);
            Console.WriteLine($"Result: {result}\n");
        }
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
