using Xunit;

namespace Calculator.Tests;

public class AddBoundaryTests : CalculatorTestBase
{
    [Fact]
    public void Add_ZeroNumbers_ReturnsZero()
    {
        // Act
        int result = Calculator.Add("0,0,0");

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void Add_ZeroAndValidNumbers_ReturnsSum()
    {
        // Act
        int result = Calculator.Add("0,5,10");

        // Assert
        Assert.Equal(15, result);
    }

    [Fact]
    public void Add_WhitespaceOnlyInput_ReturnsZero()
    {
        // Act
        int result = Calculator.Add("   ");

        // Assert
        Assert.Equal(0, result);
    }
}
