namespace Calculator.Core.Services;

using Calculator.Core;
using Calculator.Core.Exceptions;
using Calculator.Core.Interfaces;
using Calculator.Core.Models;

/// <summary>
/// Provides centralized validation logic for calculator operations.
/// Applies DRY principle by sharing validation across all operations.
/// </summary>
/// <param name="numberParser">The number parser to use for input processing.</param>
/// <param name="options">The calculator options for validation rules.</param>
public class ValidationService(INumberParser numberParser, CalculatorOptions options)
{
    private readonly CalculatorOptions _options = options ?? new CalculatorOptions();
    /// <summary>
    /// Validates input and returns parsed valid numbers.
    /// </summary>
    /// <param name="input">The input string to parse and validate.</param>
    /// <returns>List of valid numbers from the input.</returns>
    /// <exception cref="NegativeNumbersException">When negative numbers are encountered.</exception>
    public List<int> GetValidatedNumbers(string input)
    {
        return Validate(input).ValidNumbers;
    }

    /// <summary>
    /// Validates input and returns both valid numbers and display numbers.
    /// </summary>
    /// <param name="input">The input string to parse and validate.</param>
    /// <returns>A validation result with valid numbers and display numbers.</returns>
    /// <exception cref="NegativeNumbersException">When negative numbers are encountered.</exception>
    public ValidationResult Validate(string input)
    {
        ArgumentNullException.ThrowIfNull(numberParser);
        var parsed = numberParser.Parse(input);

        // Check for negative numbers (throws exception with list)
        if (_options.DenyNegatives && parsed.NegativeNumbers.Count > 0)
        {
            throw new NegativeNumbersException(parsed.NegativeNumbers);
        }

        var validNumbers = new List<int>();

        foreach (var token in parsed.TokenNumbers)
        {
            if (!token.HasValue)
            {
                continue;
            }

            int value = token.Value;

            if (value >= 0 && value > _options.UpperBound)
            {
                continue;
            }

            if (_options.DenyNegatives && value < 0)
            {
                continue;
            }

            validNumbers.Add(value);
        }

        return new ValidationResult(validNumbers, parsed.DisplayNumbers);
    }
}
