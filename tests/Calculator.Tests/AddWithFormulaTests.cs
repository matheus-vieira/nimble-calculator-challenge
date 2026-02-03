using Xunit;

namespace Calculator.Tests;

public class AddWithFormulaTests : CalculatorTestBase
{
    [Fact]
    public void AddWithFormula_EmptyString_ReturnsFormulaWithZero()
    {
        // Act
        var (result, formula) = Calculator.AddWithFormula("");

        // Assert
        Assert.Equal(0, result);
        Assert.Equal("0 = 0", formula);
    }

    [Fact]
    public void AddWithFormula_TwoNumbers_ReturnsFormulaWithSum()
    {
        // Act
        var (result, formula) = Calculator.AddWithFormula("2,4");

        // Assert
        Assert.Equal(6, result);
        Assert.Equal("2+4 = 6", formula);
    }

    [Fact]
    public void AddWithFormula_MixedValid_ReturnsFormulaWithSum()
    {
        // Act
        var (result, formula) = Calculator.AddWithFormula("2,abc,4,1001,6");

        // Assert
        Assert.Equal(12, result);
        Assert.Equal("2+0+4+0+6 = 12", formula);
    }

    [Fact]
    public void AddWithFormula_ManyNumbers_ReturnsFormula()
    {
        // Act
        var (result, formula) = Calculator.AddWithFormula("1,2,3,4,5");

        // Assert
        Assert.Equal(15, result);
        Assert.Equal("1+2+3+4+5 = 15", formula);
    }

    [Fact]
    public void AddWithFormula_CustomSingleCharDelimiter_ReturnsFormula()
    {
        // Act
        var (result, formula) = Calculator.AddWithFormula("//;\n2;4;6");

        // Assert
        Assert.Equal(12, result);
        Assert.Equal("2+4+6 = 12", formula);
    }

    [Fact]
    public void AddWithFormula_CustomMultiCharDelimiter_ReturnsFormula()
    {
        // Act
        var (result, formula) = Calculator.AddWithFormula("//[***]\n1***2***3");

        // Assert
        Assert.Equal(6, result);
        Assert.Equal("1+2+3 = 6", formula);
    }

    [Fact]
    public void AddWithFormula_MultipleCustomDelimiters_ReturnsFormula()
    {
        // Act
        var (result, formula) = Calculator.AddWithFormula("//[*][%]\n1*2%3");

        // Assert
        Assert.Equal(6, result);
        Assert.Equal("1+2+3 = 6", formula);
    }

    [Fact]
    public void AddWithFormula_CustomDelimiterWithInvalidNumbers_ShowsZeros()
    {
        // Act
        var (result, formula) = Calculator.AddWithFormula("//;\n2;abc;4;1001;6");

        // Assert
        Assert.Equal(12, result);
        Assert.Equal("2+0+4+0+6 = 12", formula);
    }

    [Fact]
    public void AddWithFormula_WithZeros_IncludesInFormula()
    {
        // Act
        var (result, formula) = Calculator.AddWithFormula("5,0,10");

        // Assert
        Assert.Equal(15, result);
        Assert.Equal("5+0+10 = 15", formula);
    }
}
