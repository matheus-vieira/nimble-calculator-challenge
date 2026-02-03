namespace Calculator.Core.Services;

using Calculator.Core.Models;

/// <summary>
/// Processes individual tokens and populates parsing results.
/// Single Responsibility: Token processing only.
/// </summary>
internal class TokenProcessor
{
    private const int MaxValidNumber = 1000;

    /// <summary>
    /// Processes a single token and adds it to the appropriate collection in the result.
    /// </summary>
    internal void ProcessToken(string token, ParsedInput result)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            result.InvalidTokens.Add(token);
            result.TokenNumbers.Add(null);
            return;
        }

        if (!int.TryParse(token, out int number))
        {
            result.InvalidTokens.Add(token);
            result.TokenNumbers.Add(null);
            return;
        }

        // Use pattern matching to categorize numbers
        result.TokenNumbers.Add(number);
        
        if (number < 0)
        {
            result.NegativeNumbers.Add(number);
        }
    }

    /// <summary>
    /// Splits the numbers string by any of the provided delimiters.
    /// </summary>
    internal List<string> SplitByDelimiters(string input, List<string> delimiters)
    {
        if (delimiters.Count == 0)
        {
            return [input];
        }

        // Sort delimiters by length (longest first) to handle overlapping patterns
        List<string> sortedDelimiters = delimiters.OrderByDescending(d => d.Length).ToList();
        string pattern = string.Join("|", sortedDelimiters.Select(System.Text.RegularExpressions.Regex.Escape));

        if (string.IsNullOrEmpty(pattern))
        {
            return [input];
        }

        return System.Text.RegularExpressions.Regex
            .Split(input, pattern, System.Text.RegularExpressions.RegexOptions.CultureInvariant)
            .ToList();
    }
}
