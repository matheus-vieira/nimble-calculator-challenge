namespace Calculator.Core.Services.Operations;

using Calculator.Core.Interfaces;

/// <summary>
/// Abstract base class for calculator operations.
/// Provides common guard clause logic to avoid duplication.
/// </summary>
/// <param name="validationService">The validation service for input validation.</param>
public abstract class CalculatorOperationBase(ValidationService validationService) : ICalculatorOperation
{
    /// <summary>
    /// Gets the validation service.
    /// </summary>
    protected ValidationService ValidationService { get; } = validationService;

    /// <summary>
    /// Executes the calculator operation with common guard clauses.
    /// </summary>
    /// <param name="input">The input string containing numbers.</param>
    /// <returns>The result of the operation.</returns>
    public int Execute(string input)
    {
        var numbers = ValidationService.GetValidatedNumbers(input);

        // Common guard clauses
        if (numbers.Count == 0) return 0;
        if (numbers.Count == 1) return numbers[0];

        // Delegate to specific operation logic
        return ExecuteOperation(numbers);
    }

    /// <summary>
    /// Executes the specific operation logic for multiple numbers.
    /// </summary>
    /// <param name="numbers">The validated numbers (guaranteed to have at least 2 elements).</param>
    /// <returns>The result of the operation.</returns>
    protected abstract int ExecuteOperation(List<int> numbers);
}
