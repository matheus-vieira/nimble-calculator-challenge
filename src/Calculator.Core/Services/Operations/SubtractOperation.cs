namespace Calculator.Core.Services.Operations;

/// <summary>
/// Implements subtraction operation.
/// Single Responsibility: Only handles subtraction logic.
/// </summary>
/// <param name="validationService">The validation service for input validation.</param>
public class SubtractOperation(ValidationService validationService) : CalculatorOperationBase(validationService)
{
    /// <summary>
    /// Subtracts numbers from the first number (guaranteed to have at least 2 elements).
    /// </summary>
    /// <param name="numbers">The validated numbers.</param>
    /// <returns>The result of subtraction (first - second - third - ...).</returns>
    protected override int ExecuteOperation(List<int> numbers)
    {
        int result = numbers[0];
        foreach (var number in numbers.Skip(1))
        {
            result -= number;
        }
        return result;
    }
}
