namespace Calculator.Core.Services;

using Calculator.Core.Exceptions;
using Calculator.Core.Interfaces;

/// <summary>
/// Provides calculator operations with input validation and parsing.
/// </summary>
public class CalculatorService : ICalculator
{
    private readonly INumberParser _numberParser;

    /// <summary>
    /// Initializes a new instance of the CalculatorService.
    /// </summary>
    /// <param name="numberParser">The number parser to use for input processing.</param>
    public CalculatorService(INumberParser numberParser)
    {
        _numberParser = numberParser ?? throw new ArgumentNullException(nameof(numberParser));
    }

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
        var parsed = _numberParser.Parse(input);

        // Check for negative numbers (throws exception with list)
        if (parsed.NegativeNumbers.Count > 0)
        {
            throw new NegativeNumbersException(parsed.NegativeNumbers);
        }

        // Sum valid numbers (no max constraint - Step 2)
        return parsed.ValidNumbers.Sum();
    }
}
