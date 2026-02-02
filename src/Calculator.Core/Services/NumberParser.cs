namespace Calculator.Core.Services;

using Calculator.Core;
using Calculator.Core.Interfaces;
using Calculator.Core.Models;
using System.Text.RegularExpressions;

/// <summary>
/// Parses input strings to extract and validate numbers according to calculator rules.
/// </summary>
public class NumberParser(CalculatorOptions options) : INumberParser
{
    private readonly CalculatorOptions _options = options ?? new CalculatorOptions();
    private static readonly Regex BracketDelimiterRegex = new(@"\[([^\]]+)\]", RegexOptions.Compiled | RegexOptions.CultureInvariant);
    /// <summary>
    /// Parses the input string according to calculator rules.
    /// Supports comma and newline delimiters for step 1-5.
    /// </summary>
    /// <param name="input">The input string to parse.</param>
    /// <returns>A ParsedInput containing valid numbers, negative numbers, and invalid tokens.</returns>
    public ParsedInput Parse(string input)
    {
        var result = new ParsedInput();

        if (string.IsNullOrWhiteSpace(input))
        {
            return result;
        }

        // Extract delimiters and numbers from the input
        var (delimiters, numbers) = ExtractDelimitersAndNumbers(input);

        // Split the numbers string by delimiters
        var tokens = SplitByDelimiters(numbers, delimiters);

        // Process each token
        foreach (var token in tokens)
        {
            ProcessToken(token, result);
        }

        return result;
    }

    /// <summary>
    /// Extracts custom delimiters (if specified) and the numbers string.
    /// </summary>
    private (List<string> delimiters, string numbers) ExtractDelimitersAndNumbers(string input)
    {
        var alternateDelimiter = string.IsNullOrEmpty(_options.AlternateDelimiter)
            ? "\n"
            : _options.AlternateDelimiter;
        var defaultDelimiters = new List<string> { ",", alternateDelimiter };

        if (!input.StartsWith("//"))
        {
            return (defaultDelimiters, input);
        }

        var newlineIndex = input.IndexOf('\n');
        if (newlineIndex == -1)
        {
            return (defaultDelimiters, input);
        }

        var delimiterPart = input.Substring(2, newlineIndex - 2);
        var numbersPart = input.Substring(newlineIndex + 1);

        var customDelimiters = ParseDelimiters(delimiterPart);
        var delimiters = new HashSet<string>(StringComparer.Ordinal);

        foreach (var delimiter in defaultDelimiters)
        {
            if (!string.IsNullOrEmpty(delimiter))
            {
                delimiters.Add(delimiter);
            }
        }

        foreach (var delimiter in customDelimiters)
        {
            if (!string.IsNullOrEmpty(delimiter))
            {
                delimiters.Add(delimiter);
            }
        }

        return (delimiters.ToList(), numbersPart);
    }

    /// <summary>
    /// Parses delimiter specification from the input header.
    /// Supports formats like: "," (single char) or "[***]" (bracket format).
    /// </summary>
    private List<string> ParseDelimiters(string delimiterPart)
    {
        var delimiters = new List<string>();

        // Check if it contains bracket notation
        if (delimiterPart.StartsWith("[") && delimiterPart.Contains("]"))
        {
            // Parse bracket-delimited delimiters: [d1][d2]...
            foreach (Match match in BracketDelimiterRegex.Matches(delimiterPart))
            {
                var value = match.Groups[1].Value;
                if (!string.IsNullOrEmpty(value))
                {
                    delimiters.Add(value);
                }
            }
        }
        else
        {
            // Single character delimiter
            if (!string.IsNullOrEmpty(delimiterPart))
            {
                delimiters.Add(delimiterPart);
            }
        }

        return delimiters.Count > 0 ? delimiters : new List<string> { "," };
    }

    /// <summary>
    /// Splits the numbers string by any of the provided delimiters.
    /// </summary>
    private List<string> SplitByDelimiters(string input, List<string> delimiters)
    {
        if (delimiters.Count == 0)
        {
            return new List<string> { input };
        }

        // Sort delimiters by length (longest first) to handle overlapping patterns
        var sortedDelimiters = delimiters.OrderByDescending(d => d.Length).ToList();
        var pattern = string.Join("|", sortedDelimiters.Select(Regex.Escape));

        if (string.IsNullOrEmpty(pattern))
        {
            return new List<string> { input };
        }

        return Regex.Split(input, pattern, RegexOptions.CultureInvariant).ToList();
    }

    /// <summary>
    /// Processes a single token and adds it to the appropriate collection in the result.
    /// </summary>
    private void ProcessToken(string token, ParsedInput result)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            result.InvalidTokens.Add(token);
            result.TokenNumbers.Add(null);
            result.DisplayNumbers.Add(0);
            return;
        }

        if (int.TryParse(token, out int number))
        {
            result.TokenNumbers.Add(number);

            if (number < 0)
            {
                result.NegativeNumbers.Add(number);
                result.DisplayNumbers.Add(number);
                return;
            }
            else if (number <= _options.UpperBound)
            {
                result.ValidNumbers.Add(number);
                result.DisplayNumbers.Add(number);
            }
            else
            {
                result.DisplayNumbers.Add(0);
            }
        }
        else
        {
            result.InvalidTokens.Add(token);
            result.TokenNumbers.Add(null);
            result.DisplayNumbers.Add(0);
        }
    }
}
