namespace Calculator.Core.Services;

using Calculator.Core;
using Calculator.Core.Exceptions;
using Calculator.Core.Interfaces;

/// <summary>
/// Facade for calculator operations.
/// Uses explicit operation resolver for clean dependency management.
/// </summary>
/// <param name="validationService">The validation service for input validation.</param>
/// <param name="operationResolver">The resolver for calculator operations.</param>
public class CalculatorService(
    ValidationService validationService,
    ICalculatorOperationResolver operationResolver) : ICalculator
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
        var validation = validationService.Validate(input);
        var numbers = validation.ValidNumbers;
        var displayNumbers = validation.DisplayNumbers;
        
        // Build formula and calculate sum
        var display = displayNumbers.Count > 0 ? displayNumbers : new List<int> { 0 };
        int sum = numbers.Sum();
        string formula = $"{string.Join("+", display)} = {sum}";

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

    /// <summary>
    /// Multiplies numbers from the input string.
    /// Stretch Goal: Additional operations.
    /// </summary>
    /// <param name="input">The input string containing numbers.</param>
    /// <returns>The result of multiplication (first * second * third * ...).</returns>
    /// <exception cref="NegativeNumbersException">When negative numbers are encountered in the input.</exception>
    public int Multiply(string input) => ResolveOperation(OperationType.Multiply).Execute(input);

    /// <summary>
    /// Divides numbers from the input string.
    /// Stretch Goal: Additional operations.
    /// </summary>
    /// <param name="input">The input string containing numbers.</param>
    /// <returns>The result of division (first / second / third / ...).</returns>
    /// <exception cref="DivideByZeroException">When any divisor is zero.</exception>
    /// <exception cref="NegativeNumbersException">When negative numbers are encountered in the input.</exception>
    public int Divide(string input) => ResolveOperation(OperationType.Divide).Execute(input);

    private ICalculatorOperation ResolveOperation(OperationType operationType)
        => operationResolver.Resolve(operationType);
}
