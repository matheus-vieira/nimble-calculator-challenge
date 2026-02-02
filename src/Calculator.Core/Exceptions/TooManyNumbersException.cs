namespace Calculator.Core.Exceptions;

/// <summary>
/// Exception thrown when more than the maximum allowed numbers are provided (Step 1).
/// </summary>
public class TooManyNumbersException : Exception
{
    public TooManyNumbersException(int count, int maxAllowed = 2)
        : base($"Expected at most {maxAllowed} numbers, but got {count}.")
    {
    }
}
