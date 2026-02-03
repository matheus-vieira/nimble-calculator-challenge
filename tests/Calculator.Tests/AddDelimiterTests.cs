using Xunit;

namespace Calculator.Tests;

public class AddDelimiterTests : CalculatorTestBase
{
    [Fact]
    public void Add_NumbersWithNewlineDelimiter_ReturnsSum()
    {
        // Act
        int result = Calculator.Add("2\n3");

        // Assert
        Assert.Equal(5, result);
    }

    [Fact]
    public void Add_MixedCommaAndNewlineDelimiters_ReturnsSum()
    {
        // Act
        int result = Calculator.Add("2\n3");

        // Assert
        Assert.Equal(5, result);
    }

    [Fact]
    public void Add_CustomSingleCharDelimiter_ReturnsSum()
    {
        // Act
        int result = Calculator.Add("//;\n1;2;3");

        // Assert
        Assert.Equal(6, result);
    }

    [Fact]
    public void Add_CustomPipeDelimiter_ReturnsSum()
    {
        // Act
        int result = Calculator.Add("//|\n4|5|6");

        // Assert
        Assert.Equal(15, result);
    }

    [Fact]
    public void Add_CustomMultiCharDelimiter_ReturnsSum()
    {
        // Act
        int result = Calculator.Add("//[***]\n1***2***3");

        // Assert
        Assert.Equal(6, result);
    }

    [Fact]
    public void Add_CustomWordDelimiter_ReturnsSum()
    {
        // Act
        int result = Calculator.Add("//[and]\n10and20and30");

        // Assert
        Assert.Equal(60, result);
    }

    [Fact]
    public void Add_MultipleCustomDelimiters_ReturnsSum()
    {
        // Act
        int result = Calculator.Add("//[*][%]\n1*2%3");

        // Assert
        Assert.Equal(6, result);
    }

    [Fact]
    public void Add_MultipleLongDelimiters_ReturnsSum()
    {
        // Act
        int result = Calculator.Add("//[***][%%%]\n1***2%%%3");

        // Assert
        Assert.Equal(6, result);
    }

    [Fact]
    public void Add_ThreeCustomDelimiters_ReturnsSum()
    {
        // Act
        int result = Calculator.Add("//[*][%][#]\n1*2%3#4");

        // Assert
        Assert.Equal(10, result);
    }
}
