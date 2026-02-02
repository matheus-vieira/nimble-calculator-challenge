namespace Calculator.Core.Interfaces;

using Calculator.Core.Models;

/// <summary>
/// Interface for parsing calculator input strings.
/// </summary>
public interface INumberParser
{
    /// <summary>
    /// Parses the input string according to calculator rules.
    /// </summary>
    /// <param name="input">The input string to parse.</param>
    /// <returns>A ParsedInput containing valid numbers, negative numbers, and invalid tokens.</returns>
    ParsedInput Parse(string input);
}
