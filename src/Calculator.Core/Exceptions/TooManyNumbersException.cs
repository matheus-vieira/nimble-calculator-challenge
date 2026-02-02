namespace Calculator.Core.Exceptions;

/// <summary>
/// Exception thrown when more than the maximum allowed numbers are provided (Step 1).
/// </summary>
/// <param name="count">The actual count of numbers provided.</param>
/// <param name="maxAllowed">The maximum allowed numbers (default is 2).</param>
public class TooManyNumbersException(int count, int maxAllowed = 2) 
    : Exception($"Expected at most {maxAllowed} numbers, but got {count}.");
