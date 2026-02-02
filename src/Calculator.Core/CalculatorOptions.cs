namespace Calculator.Core;

/// <summary>
/// Configuration options for calculator behavior.
/// </summary>
public class CalculatorOptions
{
    /// <summary>
    /// Alternate delimiter used in step #3 (defaults to newline).
    /// </summary>
    public string AlternateDelimiter { get; set; } = "\n";

    /// <summary>
    /// Whether negative numbers should be denied (default: true).
    /// </summary>
    public bool DenyNegatives { get; set; } = true;

    /// <summary>
    /// Upper bound for valid numbers (default: 1000).
    /// </summary>
    public int UpperBound { get; set; } = 1000;
}
