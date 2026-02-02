namespace Calculator.Core.Interfaces;

/// <summary>
/// Interface for calculator operations.
/// </summary>
public interface ICalculator
{
    /// <summary>
    /// Adds numbers from the input string.
    /// </summary>
    /// <param name="input">The input string containing numbers to sum.</param>
    /// <returns>The sum of valid numbers.</returns>
    /// <exception cref="Core.Exceptions.TooManyNumbersException">When more than 2 numbers are provided (Step 1).</exception>
    /// <exception cref="Core.Exceptions.NegativeNumbersException">When negative numbers are encountered.</exception>
    int Add(string input);
}
