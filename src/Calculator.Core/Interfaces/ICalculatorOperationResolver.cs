namespace Calculator.Core.Interfaces;

using Calculator.Core;

/// <summary>
/// Resolves calculator operations by type.
/// Replaces service locator pattern with explicit dependency.
/// </summary>
public interface ICalculatorOperationResolver
{
    /// <summary>
    /// Resolves the calculator operation for the specified operation type.
    /// </summary>
    /// <param name="operationType">The type of operation to resolve.</param>
    /// <returns>The calculator operation implementation.</returns>
    /// <exception cref="InvalidOperationException">When the operation type is not registered.</exception>
    ICalculatorOperation Resolve(OperationType operationType);
}
