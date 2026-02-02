namespace Calculator.Core.Services;

using Calculator.Core;
using Calculator.Core.Exceptions;
using Calculator.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Facade for calculator operations.
/// Uses keyed services for clean dependency management.
/// </summary>
/// <param name="validationService">The validation service for input validation.</param>
/// <param name="serviceProvider">The service provider used to resolve keyed operations.</param>
public class CalculatorService(
    ValidationService validationService,
    IServiceProvider serviceProvider) : ICalculator
{
    /// <summary>
    /// Adds numbers from the input string.
    /// Step 2: Supports unlimited numbers (N numbers).
    /// Negative numbers throw an exception.
    /// Numbers > 1000 are treated as invalid (0).
    /// </summary>
    /// <param name="input">The input string containing numbers to sum.</param>
    /// <returns>The sum of valid numbers.</returns>
    /// <exception cref="NegativeNumbersException">When negative numbers are encountered.</exception>
    public int Add(string input) => ResolveOperation(OperationType.Add).Execute(input);

    /// <summary>
    /// Adds numbers from the input string and returns both the result and the formula used.
    /// Stretch Goal: Shows the formula (e.g., "2+0+4+0+0+6 = 12").
    /// </summary>
    /// <param name="input">The input string containing numbers to sum.</param>
    /// <returns>A tuple containing the sum and the formula string.</returns>
    /// <exception cref="NegativeNumbersException">When negative numbers are encountered.</exception>
    public (int result, string formula) AddWithFormula(string input)
    {
        var numbers = validationService.GetValidatedNumbers(input);
        
        // Build formula and calculate sum
        var displayNumbers = numbers.Count > 0 ? numbers : new List<int> { 0 };
        int sum = numbers.Sum();
        string formula = $"{string.Join("+", displayNumbers)} = {sum}";

        return (sum, formula);
    }

    /// <summary>
    /// Subtracts numbers from the first number in the input string.
    /// Stretch Goal: Additional operations.
    /// </summary>
    /// <param name="input">The input string containing numbers.</param>
    /// <returns>The result of subtraction (first - second - third - ...).</returns>
    /// <exception cref="NegativeNumbersException">When negative numbers are encountered in the input.</exception>
    public int Subtract(string input) => ResolveOperation(OperationType.Subtract).Execute(input);

    private ICalculatorOperation ResolveOperation(OperationType operationType)
    {
        var operation = serviceProvider.GetKeyedService<ICalculatorOperation>(operationType);

        if (operation is not null)
        {
            return operation;
        }

        throw new NotImplementedException($"Operation '{operationType}' not yet implemented");
    }
}
