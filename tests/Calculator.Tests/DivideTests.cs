using Calculator.Core.Exceptions;
using Xunit;

namespace Calculator.Tests;

public class DivideTests : CalculatorTestBase
{
    [Fact]
    public void Divide_EmptyString_ReturnsZero()
    {
        // Act
        int result = Calculator.Divide("");

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void Divide_TwoNumbers_ReturnsQuotient()
    {
        // Act
        int result = Calculator.Divide("8,2");

        // Assert
        Assert.Equal(4, result);
    }

    [Fact]
    public void Divide_WithInvalidNumbers_IgnoresThem()
    {
        // Act
        int result = Calculator.Divide("20,abc,5");

        // Assert
        Assert.Equal(4, result);
    }

    [Fact]
    public void Divide_ContainsNegativeNumber_ThrowsException()
    {
        // Act & Assert
        var ex = Assert.Throws<NegativeNumbersException>(() => Calculator.Divide("10,-2,5"));
        Assert.Contains("-2", ex.Message);
    }

    [Fact]
    public void Divide_NumbersGreaterThan1000_IgnoresThem()
    {
        // Act
        int result = Calculator.Divide("100,1001,5");

        // Assert
        Assert.Equal(20, result);
    }

    [Fact]
    public void Divide_ByZero_ThrowsException()
    {
        // Act & Assert
        Assert.Throws<DivideByZeroException>(() => Calculator.Divide("10,0"));
    }

    [Fact]
    public void Divide_WithRemainder_PerformsIntegerDivision()
    {
        // Act
        int result = Calculator.Divide("10,3");

        // Assert
        Assert.Equal(3, result); // 10 / 3 = 3 (integer division)
    }
}
