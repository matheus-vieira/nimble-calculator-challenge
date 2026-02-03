namespace Calculator.Core.Services;

using System.Text.RegularExpressions;

/// <summary>
/// Parses delimiter specifications from calculator input headers.
/// Single Responsibility: Delimiter parsing only.
/// </summary>
internal class DelimiterParser
{
    private static readonly Regex BracketDelimiterRegex = 
        new(@"\[([^\]]+)\]", RegexOptions.Compiled | RegexOptions.CultureInvariant);

    /// <summary>
    /// Parses delimiter specification from the input header.
    /// Supports formats like: "," (single char) or "[***]" (bracket format).
    /// </summary>
    internal List<string> Parse(string delimiterPart)
    {
        List<string> delimiters = [];

        // Check if it contains bracket notation
        if (delimiterPart.StartsWith("[") && delimiterPart.Contains("]"))
        {
            // Parse bracket-delimited delimiters: [d1][d2]...
            foreach (Match match in BracketDelimiterRegex.Matches(delimiterPart))
            {
                string? value = match.Groups[1].Value;
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

        return delimiters.Count > 0 ? delimiters : [","];
    }

    /// <summary>
    /// Extracts delimiters and numbers from the input string.
    /// </summary>
    internal (List<string> delimiters, string numbers) ExtractDelimitersAndNumbers(
        string input,
        List<string> defaultDelimiters)
    {
        if (!input.StartsWith("//"))
        {
            return (defaultDelimiters, input);
        }

        int newlineIndex = input.IndexOf('\n');
        if (newlineIndex == -1)
        {
            return (defaultDelimiters, input);
        }

        string delimiterPart = input.Substring(2, newlineIndex - 2);
        string numbersPart = input.Substring(newlineIndex + 1);

        List<string> customDelimiters = Parse(delimiterPart);
        HashSet<string> combined = new(StringComparer.Ordinal);

        foreach (var delimiter in defaultDelimiters)
        {
            if (!string.IsNullOrEmpty(delimiter))
            {
                combined.Add(delimiter);
            }
        }

        foreach (var delimiter in customDelimiters)
        {
            if (!string.IsNullOrEmpty(delimiter))
            {
                combined.Add(delimiter);
            }
        }

        return (combined.ToList(), numbersPart);
    }
}
