namespace Calculator.Core.Services;

using Calculator.Core;
using Calculator.Core.Interfaces;
using Calculator.Core.Models;

/// <summary>
/// Parses input strings to extract and validate numbers according to calculator rules.
/// Coordinates delimiter parsing and token processing.
/// </summary>
public class NumberParser(CalculatorOptions options) : INumberParser
{
    private readonly CalculatorOptions _options = options ?? new CalculatorOptions();
    private readonly DelimiterParser _delimiterParser = new();
    private readonly TokenProcessor _tokenProcessor = new();
    
    /// <summary>
    /// Parses the input string according to calculator rules.
    /// Supports comma and newline delimiters for step 1-5.
    /// </summary>
    /// <param name="input">The input string to parse.</param>
    /// <returns>A ParsedInput containing valid numbers, negative numbers, and invalid tokens.</returns>
    public ParsedInput Parse(string input)
    {
        ParsedInput result = new();

        if (string.IsNullOrWhiteSpace(input))
        {
            return result;
        }

        // Extract delimiters and numbers from the input
        string alternateDelimiter = string.IsNullOrEmpty(_options.AlternateDelimiter)
            ? "\n"
            : _options.AlternateDelimiter;
        List<string> defaultDelimiters = [",", alternateDelimiter];
        
        var (delimiters, numbers) = _delimiterParser.ExtractDelimitersAndNumbers(input, defaultDelimiters);

        // Split the numbers string by delimiters
        List<string> tokens = _tokenProcessor.SplitByDelimiters(numbers, delimiters);

        // Process each token
        foreach (string token in tokens)
        {
            _tokenProcessor.ProcessToken(token, result);
        }

        return result;
    }
}

