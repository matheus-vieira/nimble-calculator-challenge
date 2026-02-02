using Calculator.Core;
using Calculator.Core;
using Calculator.Core.Exceptions;
using Calculator.Core.Interfaces;
using Calculator.Core.Services;
using Calculator.Core.Services.Operations;
using Microsoft.Extensions.DependencyInjection;

// Set up dependency injection with Named DI pattern
var services = new ServiceCollection();

// Core services (stateless -> singleton)
services.AddSingleton<INumberParser, NumberParser>();
services.AddSingleton<ValidationService>();

// Operation implementations (keyed services)
services.AddKeyedSingleton<ICalculatorOperation, AddOperation>(OperationType.Add);
services.AddKeyedSingleton<ICalculatorOperation, SubtractOperation>(OperationType.Subtract);
services.AddKeyedSingleton<ICalculatorOperation, MultiplyOperation>(OperationType.Multiply);
services.AddKeyedSingleton<ICalculatorOperation, DivideOperation>(OperationType.Divide);

// Calculator service
services.AddSingleton<ICalculator, CalculatorService>();

var serviceProvider = services.BuildServiceProvider();
var calculator = serviceProvider.GetRequiredService<ICalculator>();

Console.WriteLine("=== Nimble Calculator ===");
Console.WriteLine("Enter numbers separated by commas or newlines.");
Console.WriteLine("Commands:");
Console.WriteLine("  add [input]  - Add numbers (default)");
Console.WriteLine("  sub [input]  - Subtract numbers");
Console.WriteLine("  mul [input]  - Multiply numbers");
Console.WriteLine("  div [input]  - Divide numbers");
Console.WriteLine("  formula      - Toggle formula display");
Console.WriteLine("  exit         - Quit\n");

bool showFormula = true; // Show formula by default

while (true)
{
    Console.Write("Enter command: ");
    string? input = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(input))
    {
        continue;
    }

    if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
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
        string operation = "add";
        string numbers = input;

        // Parse operation and numbers
        var parts = input.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 2)
        {
            var cmd = parts[0].ToLowerInvariant();
            if (cmd is "add" or "sub" or "mul" or "div")
            {
                operation = cmd;
                numbers = parts[1];
            }
        }

        // Execute operation
        switch (operation)
        {
            case "add":
                if (showFormula)
                {
                    var (result, formula) = calculator.AddWithFormula(numbers);
                    Console.WriteLine($"Formula: {formula}");
                    Console.WriteLine($"Result: {result}\n");
                }
                else
                {
                    int result = calculator.Add(numbers);
                    Console.WriteLine($"Result: {result}\n");
                }
                break;

            case "sub":
                int subResult = calculator.Subtract(numbers);
                Console.WriteLine($"Result: {subResult}\n");
                break;

            case "mul":
                int mulResult = calculator.Multiply(numbers);
                Console.WriteLine($"Result: {mulResult}\n");
                break;

            case "div":
                int divResult = calculator.Divide(numbers);
                Console.WriteLine($"Result: {divResult}\n");
                break;
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
