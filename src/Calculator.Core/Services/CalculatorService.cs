namespace Calculator.Core.Services;

using Calculator.Core.Exceptions;
using Calculator.Core.Interfaces;

/// <summary>
/// Provides calculator operations with input validation and parsing.
/// </summary>
/// <param name="numberParser">The number parser to use for input processing.</param>
public class CalculatorService(INumberParser numberParser) : ICalculator
{
    /// <summary>
    /// Adds numbers from the input string.
    /// Step 2: Supports unlimited numbers (N numbers).
    /// Negative numbers throw an exception.
    /// Numbers > 1000 are treated as invalid (0).
    /// </summary>
    /// <param name="input">The input string containing numbers to sum.</param>
    /// <returns>The sum of valid numbers.</returns>
    /// <exception cref="NegativeNumbersException">When negative numbers are encountered.</exception>
    public int Add(string input)
    {
        ArgumentNullException.ThrowIfNull(numberParser);
        var parsed = numberParser.Parse(input);

        // Check for negative numbers (throws exception with list)
        if (parsed.NegativeNumbers.Count > 0)
        {
            throw new NegativeNumbersException(parsed.NegativeNumbers);
        }

        // Sum valid numbers (no max constraint - Step 2)
        return parsed.ValidNumbers.Sum();
    }

    /// <summary>
    /// Adds numbers from the input string and returns both the result and the formula used.
    /// Stretch Goal: Shows the formula (e.g., "2+0+4+0+0+6 = 12").
    /// </summary>
    /// <param name="input">The input string containing numbers to sum.</param>
    /// <returns>A tuple containing the sum and the formula string.</returns>
    /// <exception cref="NegativeNumbersException">When negative numbers are encountered.</exception>
    public (int result, string formula) AddWithFormula(string input)
    {
        ArgumentNullException.ThrowIfNull(numberParser);
        var parsed = numberParser.Parse(input);

        // Check for negative numbers (throws exception with list)
        if (parsed.NegativeNumbers.Count > 0)
        {
            throw new NegativeNumbersException(parsed.NegativeNumbers);
        }

        // Build formula and calculate sum
        var numbers = parsed.ValidNumbers.Count > 0 
            ? parsed.ValidNumbers 
            : new List<int> { 0 };

        int sum = numbers.Sum();
        string formula = $"{string.Join("+", numbers)} = {sum}";

        return (sum, formula);
    }

    /// <summary>
    /// Subtracts numbers from the first number in the input string.
    /// Stretch Goal: Additional operations.
    /// </summary>
    /// <param name="input">The input string containing numbers.</param>
    /// <returns>The result of subtraction (first - second - third - ...).</returns>
    /// <exception cref="NegativeNumbersException">When negative numbers are encountered in the input.</exception>
    public int Subtract(string input)
    {
        ArgumentNullException.ThrowIfNull(numberParser);
        var parsed = numberParser.Parse(input);

        // Check for negative numbers (throws exception with list)
        if (parsed.NegativeNumbers.Count > 0)
        {
            throw new NegativeNumbersException(parsed.NegativeNumbers);
        }

        var numbers = parsed.ValidNumbers.Count > 0 
            ? parsed.ValidNumbers 
            : new List<int> { 0 };

        if (numbers.Count == 0) return 0;
        if (numbers.Count == 1) return numbers[0];

        int result = numbers[0];
        for (int i = 1; i < numbers.Count; i++)
        {
            result -= numbers[i];
        }

        return result;
    }
}
