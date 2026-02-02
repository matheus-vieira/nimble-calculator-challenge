namespace Calculator.Core.Services.Operations;

/// <summary>
/// Implements addition operation.
/// Single Responsibility: Only handles addition logic.
/// </summary>
/// <param name="validationService">The validation service for input validation.</param>
public class AddOperation(ValidationService validationService) : CalculatorOperationBase(validationService)
{
    /// <summary>
    /// Adds all numbers (guaranteed to have at least 2 elements).
    /// </summary>
    /// <param name="numbers">The validated numbers to sum.</param>
    /// <returns>The sum of all numbers.</returns>
    protected override int ExecuteOperation(List<int> numbers)
        => numbers.Sum();
}
