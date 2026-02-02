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
    /// Step 1: Enforces maximum of 2 numbers (configurable).
    /// Step 2+: Removes max constraint.
    /// Negative numbers throw an exception.
    /// Numbers > 1000 are treated as invalid (0).
    /// </summary>
    /// <param name="input">The input string containing numbers to sum.</param>
    /// <returns>The sum of valid numbers.</returns>
    /// <exception cref="TooManyNumbersException">When more than 2 numbers are provided (Step 1).</exception>
    /// <exception cref="NegativeNumbersException">When negative numbers are encountered.</exception>
    public int Add(string input)
    {
        var parsed = _numberParser.Parse(input);

        // Check for negative numbers (throws exception with list)
        if (parsed.NegativeNumbers.Count > 0)
        {
            throw new NegativeNumbersException(parsed.NegativeNumbers);
        }

        // Step 1: Enforce maximum of 2 numbers
        if (parsed.ValidNumbers.Count > 2)
        {
            throw new TooManyNumbersException(parsed.ValidNumbers.Count, maxAllowed: 2);
        }

        // Sum valid numbers
        return parsed.ValidNumbers.Sum();
    }
}
