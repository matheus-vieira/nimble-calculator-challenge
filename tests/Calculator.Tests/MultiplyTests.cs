using Calculator.Core.Exceptions;
using Xunit;

namespace Calculator.Tests;

public class MultiplyTests : CalculatorTestBase
{
    [Fact]
    public void Multiply_EmptyString_ReturnsZero()
    {
        // Act
        int result = Calculator.Multiply("");

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void Multiply_SingleNumber_ReturnsThatNumber()
    {
        // Act
        int result = Calculator.Multiply("7");

        // Assert
        Assert.Equal(7, result);
    }

    [Fact]
    public void Multiply_TwoNumbers_ReturnsProduct()
    {
        // Act
        int result = Calculator.Multiply("3,4");

        // Assert
        Assert.Equal(12, result);
    }

    [Fact]
    public void Multiply_WithInvalidNumbers_IgnoresThem()
    {
        // Act
        int result = Calculator.Multiply("2,abc,5");

        // Assert
        Assert.Equal(10, result);
    }

    [Fact]
    public void Multiply_ContainsNegativeNumber_ThrowsException()
    {
        // Act & Assert
        var ex = Assert.Throws<NegativeNumbersException>(() => Calculator.Multiply("2,-3,5"));
        Assert.Contains("-3", ex.Message);
    }

    [Fact]
    public void Multiply_NumbersGreaterThan1000_IgnoresThem()
    {
        // Act
        int result = Calculator.Multiply("2,1001,5");

        // Assert
        Assert.Equal(10, result);
    }

    [Fact]
    public void Multiply_WithZero_ReturnsZero()
    {
        // Act
        int result = Calculator.Multiply("5,0,10");

        // Assert
        Assert.Equal(0, result);
    }
}
