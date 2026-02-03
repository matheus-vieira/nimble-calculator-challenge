namespace Calculator.Core.Services.Operations;

/// <summary>
/// Implements division operation.
/// Single Responsibility: Only handles division logic.
/// </summary>
/// <param name="validationService">The validation service for input validation.</param>
public class DivideOperation(ValidationService validationService) : CalculatorOperationBase(validationService)
{
    /// <summary>
    /// Divides numbers sequentially (guaranteed to have at least 2 elements).
    /// </summary>
    /// <param name="numbers">The validated numbers.</param>
    /// <returns>The result of division (first / second / third / ...).</returns>
    /// <exception cref="DivideByZeroException">When any divisor is zero.</exception>
    protected override int ExecuteOperation(List<int> numbers)
    {
        int result = numbers[0];
        foreach (var number in numbers.Skip(1))
        {
            if (number == 0)
                throw new DivideByZeroException();
            
            result /= number;
        }
        return result;
    }
}
