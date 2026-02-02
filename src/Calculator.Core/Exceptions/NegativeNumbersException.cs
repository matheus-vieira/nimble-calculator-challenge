namespace Calculator.Core.Exceptions;

/// <summary>
/// Exception thrown when negative numbers are encountered in the input.
/// </summary>
/// <param name="negatives">The collection of negative numbers encountered.</param>
public class NegativeNumbersException(IEnumerable<int> negatives) 
    : Exception($"Negatives not allowed: {string.Join(", ", negatives)}")
{
    /// <summary>
    /// Gets the list of negative numbers that caused the exception.
    /// </summary>
    public List<int> NegativeNumbers { get; } = negatives.ToList();
}
