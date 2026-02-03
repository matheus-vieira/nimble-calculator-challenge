using Xunit;

namespace Calculator.Tests;

public class AddBasicTests : CalculatorTestBase
{
    [Fact]
    public void Add_EmptyString_ReturnsZero()
    {
        // Act
        int result = Calculator.Add("");

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void Add_SingleNumber_ReturnsThatNumber()
    {
        // Act
        int result = Calculator.Add("5");

        // Assert
        Assert.Equal(5, result);
    }

    [Fact]
    public void Add_TwoNumbers_ReturnsSum()
    {
        // Act
        int result = Calculator.Add("2,3");

        // Assert
        Assert.Equal(5, result);
    }

    [Fact]
    public void Add_InvalidNumber_TreatsAsZero()
    {
        // Act
        int result = Calculator.Add("2,abc");

        // Assert
        Assert.Equal(2, result);
    }

    [Fact]
    public void Add_MissingNumber_TreatsAsZero()
    {
        // Act
        int result = Calculator.Add("2,");

        // Assert
        Assert.Equal(2, result);
    }

    [Fact]
    public void Add_ThreeNumbers_ReturnsSum()
    {
        // Act
        int result = Calculator.Add("1,2,3");

        // Assert
        Assert.Equal(6, result);
    }

    [Fact]
    public void Add_ManyNumbers_ReturnsSum()
    {
        // Act
        int result = Calculator.Add("1,2,3,4,5,6,7,8,9,10");

        // Assert
        Assert.Equal(55, result);
    }
}
