namespace Calculator.Core.Services;

using Calculator.Core.Exceptions;
using Calculator.Core.Interfaces;

/// <summary>
/// Provides centralized validation logic for calculator operations.
/// Applies DRY principle by sharing validation across all operations.
/// </summary>
/// <param name="numberParser">The number parser to use for input processing.</param>
public class ValidationService(INumberParser numberParser)
{
    /// <summary>
    /// Validates input and returns parsed valid numbers.
    /// </summary>
    /// <param name="input">The input string to parse and validate.</param>
    /// <returns>List of valid numbers from the input.</returns>
    /// <exception cref="NegativeNumbersException">When negative numbers are encountered.</exception>
    public List<int> GetValidatedNumbers(string input)
    {
        ArgumentNullException.ThrowIfNull(numberParser);
        var parsed = numberParser.Parse(input);

        // Check for negative numbers (throws exception with list)
        if (parsed.NegativeNumbers.Count > 0)
        {
            throw new NegativeNumbersException(parsed.NegativeNumbers);
        }

        return parsed.ValidNumbers;
    }
}
