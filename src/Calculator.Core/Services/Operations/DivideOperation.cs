namespace Calculator.Core.Services.Operations;

using Calculator.Core.Interfaces;

/// <summary>
/// Implements division operation.
/// Single Responsibility: Only handles division logic.
/// </summary>
/// <param name="validationService">The validation service for input validation.</param>
public class DivideOperation(ValidationService validationService) : ICalculatorOperation
{
    /// <summary>
    /// Divides numbers from the input string sequentially.
    /// </summary>
    /// <param name="input">The input string containing numbers.</param>
    /// <returns>The result of division (first / second / third / ...).</returns>
    /// <exception cref="DivideByZeroException">When any divisor is zero.</exception>
    public int Execute(string input)
    {
        var numbers = validationService.GetValidatedNumbers(input);

        if (numbers.Count == 0) return 0;
        if (numbers.Count == 1) return numbers[0];

        int result = numbers[0];
        for (int i = 1; i < numbers.Count; i++)
        {
            if (numbers[i] == 0)
            {
                throw new DivideByZeroException();
            }

            result /= numbers[i];
        }

        return result;
    }
}
