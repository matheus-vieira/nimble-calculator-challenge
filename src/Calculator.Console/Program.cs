using Calculator.Core;
using Calculator.Core.Exceptions;
using Calculator.Core.Interfaces;
using Calculator.Core.Services;
using Calculator.Core.Services.Operations;
using Microsoft.Extensions.DependencyInjection;

// Parse command-line options
var options = ParseOptions(args);

// Set up dependency injection
var services = new ServiceCollection();

// Core services (stateless -> singleton)
services.AddSingleton(options);
services.AddSingleton<INumberParser, NumberParser>();
services.AddSingleton<ValidationService>();

// Operation implementations
services.AddSingleton<AddOperation>();
services.AddSingleton<SubtractOperation>();
services.AddSingleton<MultiplyOperation>();
services.AddSingleton<DivideOperation>();

// Operation resolver with explicit dependencies
services.AddSingleton<ICalculatorOperationResolver>(sp =>
{
    var operations = new Dictionary<OperationType, ICalculatorOperation>
    {
        [OperationType.Add] = sp.GetRequiredService<AddOperation>(),
        [OperationType.Subtract] = sp.GetRequiredService<SubtractOperation>(),
        [OperationType.Multiply] = sp.GetRequiredService<MultiplyOperation>(),
        [OperationType.Divide] = sp.GetRequiredService<DivideOperation>()
    };
    return new CalculatorOperationResolver(operations);
});

// Calculator service
services.AddSingleton<ICalculator, CalculatorService>();

using var serviceProvider = services.BuildServiceProvider();
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
    catch (NegativeNumbersException ex)
    {
        Console.WriteLine($"Error: {ex.Message}\n");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Unexpected error: {ex.Message}\n");
    }
}

static CalculatorOptions ParseOptions(string[] args)
{
    var options = new CalculatorOptions();

    foreach (var arg in args)
    {
        if (arg.StartsWith("--alt-delim=", StringComparison.OrdinalIgnoreCase) ||
            arg.StartsWith("--alt-delimiter=", StringComparison.OrdinalIgnoreCase))
        {
            var value = arg.Split('=', 2)[1];
            if (!string.IsNullOrEmpty(value))
            {
                options.AlternateDelimiter = value;
            }
            continue;
        }

        if (arg.Equals("--allow-negatives", StringComparison.OrdinalIgnoreCase))
        {
            options.DenyNegatives = false;
            continue;
        }

        if (arg.Equals("--deny-negatives", StringComparison.OrdinalIgnoreCase))
        {
            options.DenyNegatives = true;
            continue;
        }

        if (arg.StartsWith("--upper-bound=", StringComparison.OrdinalIgnoreCase))
        {
            var value = arg.Split('=', 2)[1];
            if (int.TryParse(value, out int upperBound) && upperBound >= 0)
            {
                options.UpperBound = upperBound;
            }
        }
    }

    return options;
}
