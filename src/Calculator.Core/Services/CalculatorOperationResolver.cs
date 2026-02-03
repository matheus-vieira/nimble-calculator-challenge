namespace Calculator.Core.Services;

using Calculator.Core;
using Calculator.Core.Interfaces;

/// <summary>
/// Resolves calculator operations using a dictionary.
/// Provides explicit dependency over service locator pattern.
/// </summary>
public class CalculatorOperationResolver : ICalculatorOperationResolver
{
    private readonly Dictionary<OperationType, ICalculatorOperation> _operations;

    /// <summary>
    /// Initializes a new instance of the <see cref="CalculatorOperationResolver"/> class.
    /// </summary>
    /// <param name="operations">Dictionary mapping operation types to implementations.</param>
    public CalculatorOperationResolver(Dictionary<OperationType, ICalculatorOperation> operations)
    {
        _operations = operations ?? throw new ArgumentNullException(nameof(operations));
    }

    /// <summary>
    /// Resolves the calculator operation for the specified operation type.
    /// </summary>
    /// <param name="operationType">The type of operation to resolve.</param>
    /// <returns>The calculator operation implementation.</returns>
    /// <exception cref="InvalidOperationException">When the operation type is not registered.</exception>
    public ICalculatorOperation Resolve(OperationType operationType) =>
        _operations.TryGetValue(operationType, out var operation)
            ? operation
            : throw new InvalidOperationException($"Operation '{operationType}' is not registered");
}
