namespace Calculator.Core.Services;

using Calculator.Core.Exceptions;
using Calculator.Core.Interfaces;
using Calculator.Core.Models;

/// <summary>
/// Parses input strings to extract and validate numbers according to calculator rules.
/// </summary>
public class NumberParser : INumberParser
{
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
        var defaultDelimiters = new List<string> { ",", "\n" };

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

        var delimiters = ParseDelimiters(delimiterPart);
        return (delimiters, numbersPart);
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
            int i = 0;
            while (i < delimiterPart.Length)
            {
                if (delimiterPart[i] == '[')
                {
                    int closeIndex = delimiterPart.IndexOf(']', i);
                    if (closeIndex != -1)
                    {
                        string delimiter = delimiterPart.Substring(i + 1, closeIndex - i - 1);
                        if (!string.IsNullOrEmpty(delimiter))
                        {
                            delimiters.Add(delimiter);
                        }
                        i = closeIndex + 1;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    i++;
                }
            }
        }
        else
        {
            // Single character delimiter
            delimiters.Add(delimiterPart);
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

        var tokens = new List<string> { input };

        // Sort delimiters by length (longest first) to handle overlapping patterns
        var sortedDelimiters = delimiters.OrderByDescending(d => d.Length).ToList();

        foreach (var delimiter in sortedDelimiters)
        {
            var newTokens = new List<string>();
            foreach (var token in tokens)
            {
                newTokens.AddRange(token.Split(new[] { delimiter }, StringSplitOptions.None));
            }
            tokens = newTokens;
        }

        return tokens;
    }

    /// <summary>
    /// Processes a single token and adds it to the appropriate collection in the result.
    /// </summary>
    private void ProcessToken(string token, ParsedInput result)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return;
        }

        if (int.TryParse(token, out int number))
        {
            if (number < 0)
            {
                result.NegativeNumbers.Add(number);
            }
            else if (number <= 1000)
            {
                result.ValidNumbers.Add(number);
            }
            // Numbers > 1000 are treated as invalid (0)
        }
        else
        {
            result.InvalidTokens.Add(token);
        }
    }
}
