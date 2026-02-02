namespace Calculator.Core.Services.Operations;

using Calculator.Core.Interfaces;

/// <summary>
/// Implements multiplication operation.
/// Single Responsibility: Only handles multiplication logic.
/// </summary>
/// <param name="validationService">The validation service for input validation.</param>
public class MultiplyOperation(ValidationService validationService) : ICalculatorOperation
{
    /// <summary>
    /// Multiplies all numbers from the input string.
    /// </summary>
    /// <param name="input">The input string containing numbers to multiply.</param>
    /// <returns>The product of all valid numbers.</returns>
    public int Execute(string input)
    {
        var numbers = validationService.GetValidatedNumbers(input);

        if (numbers.Count == 0) return 0;
        if (numbers.Count == 1) return numbers[0];

        int result = numbers[0];
        for (int i = 1; i < numbers.Count; i++)
        {
            result *= numbers[i];
        }

        return result;
    }
}
