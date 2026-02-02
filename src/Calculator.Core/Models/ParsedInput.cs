namespace Calculator.Core.Models;

/// <summary>
/// Represents the result of parsing a calculator input string.
/// </summary>
public class ParsedInput
{
    /// <summary>
    /// Gets the list of negative numbers found in the input.
    /// </summary>
    public List<int> NegativeNumbers { get; set; } = new();

    /// <summary>
    /// Gets the list of invalid tokens from the input.
    /// </summary>
    public List<string> InvalidTokens { get; set; } = new();

    /// <summary>
    /// Gets the list of parsed numbers in original order (null for invalid/missing tokens).
    /// </summary>
    public List<int?> TokenNumbers { get; set; } = new();
}
