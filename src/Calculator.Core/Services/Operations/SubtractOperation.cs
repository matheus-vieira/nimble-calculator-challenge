namespace Calculator.Core.Services.Operations;

using Calculator.Core.Interfaces;

/// <summary>
/// Implements subtraction operation.
/// Single Responsibility: Only handles subtraction logic.
/// </summary>
/// <param name="validationService">The validation service for input validation.</param>
public class SubtractOperation(ValidationService validationService) : ICalculatorOperation
{
    /// <summary>
    /// Subtracts numbers from the first number in the input string.
    /// </summary>
    /// <param name="input">The input string containing numbers.</param>
    /// <returns>The result of subtraction (first - second - third - ...).</returns>
    public int Execute(string input)
    {
        var numbers = validationService.GetValidatedNumbers(input);

        if (numbers.Count == 0) return 0;
        if (numbers.Count == 1) return numbers[0];

        int result = numbers[0];
        for (int i = 1; i < numbers.Count; i++)
        {
            result -= numbers[i];
        }

        return result;
    }
}
