using Calculator.Core.Exceptions;
using Xunit;

namespace Calculator.Tests;

public class AddValidationTests : CalculatorTestBase
{
    [Fact]
    public void Add_ContainsNegativeNumber_ThrowsNegativeNumbersException()
    {
        // Act & Assert
        var ex = Assert.Throws<NegativeNumbersException>(() => Calculator.Add("2,-3"));
        Assert.Contains("Negatives not allowed", ex.Message);
        Assert.Contains("-3", ex.Message);
    }

    [Fact]
    public void Add_MultipleNegativeNumbers_ListsAllInException()
    {
        // Act & Assert
        var ex = Assert.Throws<NegativeNumbersException>(() => Calculator.Add("2,-3,-5"));
        Assert.Contains("-3", ex.Message);
        Assert.Contains("-5", ex.Message);
    }

    [Fact]
    public void Add_NumberGreaterThan1000_IgnoresIt()
    {
        // Act
        int result = Calculator.Add("2,1001");

        // Assert
        Assert.Equal(2, result);
    }

    [Fact]
    public void Add_NumberEqualTo1000_Includes()
    {
        // Act
        int result = Calculator.Add("1000,1");

        // Assert
        Assert.Equal(1001, result);
    }
}
