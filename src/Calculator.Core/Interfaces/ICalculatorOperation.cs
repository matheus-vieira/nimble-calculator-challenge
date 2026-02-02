namespace Calculator.Core.Interfaces;

/// <summary>
/// Base interface for calculator operations.
/// Enables extensibility following Open/Closed Principle.
/// </summary>
public interface ICalculatorOperation
{
    /// <summary>
    /// Executes the operation on the given input.
    /// </summary>
    /// <param name="input">The input string containing numbers.</param>
    /// <returns>The result of the operation.</returns>
    int Execute(string input);
}
