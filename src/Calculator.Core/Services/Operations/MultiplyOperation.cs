namespace Calculator.Core.Services.Operations;

/// <summary>
/// Implements multiplication operation.
/// Single Responsibility: Only handles multiplication logic.
/// </summary>
/// <param name="validationService">The validation service for input validation.</param>
public class MultiplyOperation(ValidationService validationService) : CalculatorOperationBase(validationService)
{
    /// <summary>
    /// Multiplies all numbers (guaranteed to have at least 2 elements).
    /// </summary>
    /// <param name="numbers">The validated numbers to multiply.</param>
    /// <returns>The product of all numbers.</returns>
    protected override int ExecuteOperation(List<int> numbers) =>
        numbers.Aggregate((acc, num) => acc * num);
}
