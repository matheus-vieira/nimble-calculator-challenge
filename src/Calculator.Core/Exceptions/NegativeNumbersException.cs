namespace Calculator.Core.Exceptions;

/// <summary>
/// Exception thrown when negative numbers are encountered in the input.
/// </summary>
public class NegativeNumbersException : Exception
{
    public NegativeNumbersException(IEnumerable<int> negatives)
        : base($"Negatives not allowed: {string.Join(", ", negatives)}")
    {
        NegativeNumbers = negatives.ToList();
    }

    /// <summary>
    /// Gets the list of negative numbers that caused the exception.
    /// </summary>
    public List<int> NegativeNumbers { get; }
}
