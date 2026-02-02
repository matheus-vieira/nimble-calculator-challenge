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

    /// <summary>
    /// Adds numbers from the input string and returns both the result and the formula used.
    /// </summary>
    /// <param name="input">The input string containing numbers to sum.</param>
    /// <returns>A tuple containing the sum and the formula string (e.g., "2+0+4+0+0+6 = 12").</returns>
    /// <exception cref="Core.Exceptions.TooManyNumbersException">When more than 2 numbers are provided (Step 1).</exception>
    /// <exception cref="Core.Exceptions.NegativeNumbersException">When negative numbers are encountered.</exception>
    (int result, string formula) AddWithFormula(string input);

    /// <summary>
    /// Subtracts numbers from the first number in the input string.
    /// </summary>
    /// <param name="input">The input string containing numbers.</param>
    /// <returns>The result of subtraction (first - second - third - ...).</returns>
    /// <exception cref="Core.Exceptions.NegativeNumbersException">When negative numbers are encountered in the input.</exception>
    int Subtract(string input);

    /// <summary>
    /// Multiplies numbers from the input string.
    /// </summary>
    /// <param name="input">The input string containing numbers.</param>
    /// <returns>The result of multiplication (first * second * third * ...).</returns>
    /// <exception cref="Core.Exceptions.NegativeNumbersException">When negative numbers are encountered in the input.</exception>
    int Multiply(string input);

    /// <summary>
    /// Divides numbers from the input string.
    /// </summary>
    /// <param name="input">The input string containing numbers.</param>
    /// <returns>The result of division (first / second / third / ...).</returns>
    /// <exception cref="DivideByZeroException">When any divisor is zero.</exception>
    /// <exception cref="Core.Exceptions.NegativeNumbersException">When negative numbers are encountered in the input.</exception>
    int Divide(string input);
}
