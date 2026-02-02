using Xunit;
using Calculator.Core.Services;
using Calculator.Core.Services.Operations;
using Calculator.Core.Exceptions;

namespace Calculator.Tests.Operations;

/// <summary>
/// Unit tests for AddOperation.
/// Focused tests for addition logic only.
/// </summary>
public class AddOperationTests
{
    private readonly AddOperation _addOperation;

    public AddOperationTests()
    {
        var numberParser = new NumberParser();
        var validationService = new ValidationService(numberParser);
        _addOperation = new AddOperation(validationService);
    }

    [Fact]
    public void Execute_EmptyString_ReturnsZero()
    {
        // Act
        int result = _addOperation.Execute("");

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void Execute_SingleNumber_ReturnsThatNumber()
    {
        // Act
        int result = _addOperation.Execute("5");

        // Assert
        Assert.Equal(5, result);
    }

    [Fact]
    public void Execute_TwoNumbers_ReturnsSum()
    {
        // Act
        int result = _addOperation.Execute("2,3");

        // Assert
        Assert.Equal(5, result);
    }

    [Fact]
    public void Execute_MultipleNumbers_ReturnsSum()
    {
        // Act
        int result = _addOperation.Execute("1,2,3,4,5");

        // Assert
        Assert.Equal(15, result);
    }

    [Fact]
    public void Execute_WithInvalidNumbers_IgnoresThem()
    {
        // Act
        int result = _addOperation.Execute("5,abc,10");

        // Assert
        Assert.Equal(15, result);
    }

    [Fact]
    public void Execute_ContainsNegativeNumber_ThrowsException()
    {
        // Act & Assert
        var ex = Assert.Throws<NegativeNumbersException>(() => _addOperation.Execute("5,-3,10"));
        Assert.Contains("-3", ex.Message);
    }

    [Fact]
    public void Execute_NumbersGreaterThan1000_IgnoresThem()
    {
        // Act
        int result = _addOperation.Execute("100,1001,50");

        // Assert
        Assert.Equal(150, result);
    }
}
