using Xunit;
using Calculator.Core;
using Calculator.Core.Services;
using Calculator.Core.Services.Operations;
using Calculator.Core.Exceptions;

namespace Calculator.Tests.Operations;

/// <summary>
/// Unit tests for SubtractOperation.
/// Focused tests for subtraction logic only.
/// </summary>
public class SubtractOperationTests
{
    private readonly SubtractOperation _subtractOperation;

    public SubtractOperationTests()
    {
        var options = new CalculatorOptions();
        var numberParser = new NumberParser(options);
        var validationService = new ValidationService(numberParser, options);
        _subtractOperation = new SubtractOperation(validationService);
    }

    [Fact]
    public void Execute_EmptyString_ReturnsZero()
    {
        // Act
        int result = _subtractOperation.Execute("");

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void Execute_SingleNumber_ReturnsThatNumber()
    {
        // Act
        int result = _subtractOperation.Execute("15");

        // Assert
        Assert.Equal(15, result);
    }

    [Fact]
    public void Execute_TwoNumbers_ReturnsResult()
    {
        // Act
        int result = _subtractOperation.Execute("10,3");

        // Assert
        Assert.Equal(7, result);
    }

    [Fact]
    public void Execute_MultipleNumbers_ReturnsResult()
    {
        // Act
        int result = _subtractOperation.Execute("20,5,3");

        // Assert
        Assert.Equal(12, result);
    }

    [Fact]
    public void Execute_WithInvalidNumbers_IgnoresThem()
    {
        // Act
        int result = _subtractOperation.Execute("10,abc,3");

        // Assert
        Assert.Equal(7, result);
    }

    [Fact]
    public void Execute_ContainsNegativeNumber_ThrowsException()
    {
        // Act & Assert
        var ex = Assert.Throws<NegativeNumbersException>(() => _subtractOperation.Execute("10,-3,5"));
        Assert.Contains("-3", ex.Message);
    }

    [Fact]
    public void Execute_NumbersGreaterThan1000_IgnoresThem()
    {
        // Act
        int result = _subtractOperation.Execute("100,1001,10");

        // Assert
        Assert.Equal(90, result); // 100 - 10 (1001 ignored)
    }
}
