namespace Calculator.Core.Services.Operations;

using Calculator.Core.Interfaces;

/// <summary>
/// Implements addition operation.
/// Single Responsibility: Only handles addition logic.
/// </summary>
/// <param name="validationService">The validation service for input validation.</param>
public class AddOperation(ValidationService validationService) : ICalculatorOperation
{
    /// <summary>
    /// Adds all numbers from the input string.
    /// </summary>
    /// <param name="input">The input string containing numbers to sum.</param>
    /// <returns>The sum of all valid numbers.</returns>
    public int Execute(string input)
    {
        var numbers = validationService.GetValidatedNumbers(input);
        return numbers.Sum();
    }
}
