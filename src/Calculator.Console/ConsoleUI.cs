namespace Calculator.Console;

using Calculator.Core.Exceptions;
using Calculator.Core.Interfaces;

/// <summary>
/// Handles console UI interaction and command execution.
/// Single Responsibility: User input and output handling.
/// </summary>
internal class ConsoleUI
{
    private readonly ICalculator _calculator;
    private bool _showFormula;

    internal ConsoleUI(ICalculator calculator)
    {
        _calculator = calculator;
        _showFormula = true; // Show formula by default
    }

    internal void Run()
    {
        DisplayWelcome();

        while (true)
        {
            Console.Write("Enter command: ");
            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                continue;
            }

            if (HandleSpecialCommands(input))
            {
                break;
            }

            ExecuteCalculation(input);
        }
    }

    private void DisplayWelcome()
    {
        Console.WriteLine("=== Nimble Calculator ===");
        Console.WriteLine("Enter numbers separated by commas or newlines.");
        Console.WriteLine("Commands:");
        Console.WriteLine("  add [input]  - Add numbers (default)");
        Console.WriteLine("  sub [input]  - Subtract numbers");
        Console.WriteLine("  mul [input]  - Multiply numbers");
        Console.WriteLine("  div [input]  - Divide numbers");
        Console.WriteLine("  formula      - Toggle formula display");
        Console.WriteLine("  exit         - Quit\n");
    }

    private bool HandleSpecialCommands(string input)
    {
        if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("Goodbye!");
            return true;
        }

        if (input.Equals("formula", StringComparison.OrdinalIgnoreCase))
        {
            _showFormula = !_showFormula;
            Console.WriteLine($"Formula display: {(_showFormula ? "ON" : "OFF")}\n");
            return false;
        }

        return false;
    }

    private void ExecuteCalculation(string input)
    {
        try
        {
            string operation = "add";
            string numbers = input;

            // Parse operation and numbers
            string[] parts = input.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 2)
            {
                string cmd = parts[0].ToLowerInvariant();
                if (cmd is "add" or "sub" or "mul" or "div")
                {
                    operation = cmd;
                    numbers = parts[1];
                }
            }

            // Execute operation
            ExecuteOperation(operation, numbers);
        }
        catch (NegativeNumbersException ex)
        {
            Console.WriteLine($"Error: {ex.Message}\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}. Please check your input format.\n");
        }
    }

    private void ExecuteOperation(string operation, string numbers)
    {
        switch (operation)
        {
            case "add":
                ExecuteAdd(numbers);
                break;

            case "sub":
                int subResult = _calculator.Subtract(numbers);
                Console.WriteLine($"Result: {subResult}\n");
                break;

            case "mul":
                int mulResult = _calculator.Multiply(numbers);
                Console.WriteLine($"Result: {mulResult}\n");
                break;

            case "div":
                int divResult = _calculator.Divide(numbers);
                Console.WriteLine($"Result: {divResult}\n");
                break;
        }
    }

    private void ExecuteAdd(string numbers)
    {
        if (_showFormula)
        {
            var (result, formula) = _calculator.AddWithFormula(numbers);
            Console.WriteLine($"Formula: {formula}");
            Console.WriteLine($"Result: {result}\n");
        }
        else
        {
            int result = _calculator.Add(numbers);
            Console.WriteLine($"Result: {result}\n");
        }
    }
}
