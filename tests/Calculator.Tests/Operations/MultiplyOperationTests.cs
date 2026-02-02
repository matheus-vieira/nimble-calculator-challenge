using Xunit;
using Calculator.Core;
using Calculator.Core.Services;
using Calculator.Core.Services.Operations;
using Calculator.Core.Exceptions;

namespace Calculator.Tests.Operations;

/// <summary>
/// Unit tests for MultiplyOperation.
/// Focused tests for multiplication logic only.
/// </summary>
public class MultiplyOperationTests
{
    private readonly MultiplyOperation _multiplyOperation;

    public MultiplyOperationTests()
    {
        var options = new CalculatorOptions();
        var numberParser = new NumberParser(options);
        var validationService = new ValidationService(numberParser, options);
        _multiplyOperation = new MultiplyOperation(validationService);
    }

    [Fact]
    public void Execute_EmptyString_ReturnsZero()
    {
        // Act
        int result = _multiplyOperation.Execute("");

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void Execute_SingleNumber_ReturnsThatNumber()
    {
        // Act
        int result = _multiplyOperation.Execute("5");

        // Assert
        Assert.Equal(5, result);
    }

    [Fact]
    public void Execute_TwoNumbers_ReturnsProduct()
    {
        // Act
        int result = _multiplyOperation.Execute("2,3");

        // Assert
        Assert.Equal(6, result);
    }

    [Fact]
    public void Execute_MultipleNumbers_ReturnsProduct()
    {
        // Act
        int result = _multiplyOperation.Execute("1,2,3,4");

        // Assert
        Assert.Equal(24, result);
    }

    [Fact]
    public void Execute_WithInvalidNumbers_IgnoresThem()
    {
        // Act
        int result = _multiplyOperation.Execute("2,abc,5");

        // Assert
        Assert.Equal(10, result);
    }

    [Fact]
    public void Execute_ContainsNegativeNumber_ThrowsException()
    {
        // Act & Assert
        var ex = Assert.Throws<NegativeNumbersException>(() => _multiplyOperation.Execute("2,-3,5"));
        Assert.Contains("-3", ex.Message);
    }

    [Fact]
    public void Execute_NumbersGreaterThan1000_IgnoresThem()
    {
        // Act
        int result = _multiplyOperation.Execute("2,1001,5");

        // Assert
        Assert.Equal(10, result);
    }
}
