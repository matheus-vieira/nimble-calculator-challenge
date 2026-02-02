using Xunit;
using Calculator.Core.Services;
using Calculator.Core.Services.Operations;
using Calculator.Core.Exceptions;

namespace Calculator.Tests.Operations;

/// <summary>
/// Unit tests for DivideOperation.
/// Focused tests for division logic only.
/// </summary>
public class DivideOperationTests
{
    private readonly DivideOperation _divideOperation;

    public DivideOperationTests()
    {
        var numberParser = new NumberParser();
        var validationService = new ValidationService(numberParser);
        _divideOperation = new DivideOperation(validationService);
    }

    [Fact]
    public void Execute_EmptyString_ReturnsZero()
    {
        // Act
        int result = _divideOperation.Execute("");

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void Execute_SingleNumber_ReturnsThatNumber()
    {
        // Act
        int result = _divideOperation.Execute("8");

        // Assert
        Assert.Equal(8, result);
    }

    [Fact]
    public void Execute_TwoNumbers_ReturnsQuotient()
    {
        // Act
        int result = _divideOperation.Execute("8,2");

        // Assert
        Assert.Equal(4, result);
    }

    [Fact]
    public void Execute_MultipleNumbers_ReturnsQuotient()
    {
        // Act
        int result = _divideOperation.Execute("100,5,2");

        // Assert
        Assert.Equal(10, result);
    }

    [Fact]
    public void Execute_WithInvalidNumbers_IgnoresThem()
    {
        // Act
        int result = _divideOperation.Execute("20,abc,5");

        // Assert
        Assert.Equal(4, result);
    }

    [Fact]
    public void Execute_ContainsNegativeNumber_ThrowsException()
    {
        // Act & Assert
        var ex = Assert.Throws<NegativeNumbersException>(() => _divideOperation.Execute("10,-2,5"));
        Assert.Contains("-2", ex.Message);
    }

    [Fact]
    public void Execute_NumbersGreaterThan1000_IgnoresThem()
    {
        // Act
        int result = _divideOperation.Execute("100,1001,5");

        // Assert
        Assert.Equal(20, result);
    }

    [Fact]
    public void Execute_DivisionByZero_ThrowsException()
    {
        // Act & Assert
        Assert.Throws<DivideByZeroException>(() => _divideOperation.Execute("10,0"));
    }
}
