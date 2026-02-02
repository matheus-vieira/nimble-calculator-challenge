namespace Calculator.Core.Models;

/// <summary>
/// Represents validated numbers and display numbers for formula rendering.
/// </summary>
/// <param name="ValidNumbers">Numbers used for calculation.</param>
/// <param name="DisplayNumbers">Numbers used for formula display.</param>
public record ValidationResult(List<int> ValidNumbers, List<int> DisplayNumbers);
