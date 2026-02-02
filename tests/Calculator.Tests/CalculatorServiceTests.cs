using Xunit;
using Calculator.Core.Interfaces;
using Calculator.Core.Services;
using Calculator.Core.Exceptions;

namespace Calculator.Tests;

/// <summary>
/// Unit tests for the CalculatorService and NumberParser.
/// Tests are organized by requirement step.
/// </summary>
public class CalculatorServiceTests
{
    private readonly INumberParser _numberParser;
    private readonly ICalculator _calculator;

    public CalculatorServiceTests()
    {
        _numberParser = new NumberParser();
        _calculator = new CalculatorService(_numberParser);
    }

    #region Step 1: Basic Addition with Max 2 Numbers

    [Fact]
    public void Add_EmptyString_ReturnsZero()
    {
        // Act
        int result = _calculator.Add("");

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void Add_SingleNumber_ReturnsThatNumber()
    {
        // Act
        int result = _calculator.Add("5");

        // Assert
        Assert.Equal(5, result);
    }

    [Fact]
    public void Add_TwoNumbers_ReturnsSum()
    {
        // Act
        int result = _calculator.Add("2,3");

        // Assert
        Assert.Equal(5, result);
    }

    [Fact]
    public void Add_InvalidNumber_TreatsAsZero()
    {
        // Act
        int result = _calculator.Add("2,abc");

        // Assert
        Assert.Equal(2, result);
    }

    [Fact]
    public void Add_MissingNumber_TreatsAsZero()
    {
        // Act
        int result = _calculator.Add("2,");

        // Assert
        Assert.Equal(2, result);
    }

    #endregion

    #region Step 2: Remove Max Constraint (Support N Numbers)

    [Fact]
    public void Add_ThreeNumbers_ReturnsSum()
    {
        // Act
        int result = _calculator.Add("1,2,3");

        // Assert
        Assert.Equal(6, result);
    }

    [Fact]
    public void Add_ManyNumbers_ReturnsSum()
    {
        // Act
        int result = _calculator.Add("1,2,3,4,5,6,7,8,9,10");

        // Assert
        Assert.Equal(55, result);
    }

    #endregion

    #region Step 3: Newline as Delimiter

    [Fact]
    public void Add_NumbersWithNewlineDelimiter_ReturnsSum()
    {
        // Act
        int result = _calculator.Add("2\n3");

        // Assert
        Assert.Equal(5, result);
    }

    [Fact]
    public void Add_MixedCommaAndNewlineDelimiters_ReturnsSum()
    {
        // Act
        int result = _calculator.Add("2\n3");

        // Assert
        Assert.Equal(5, result);
    }

    #endregion

    #region Step 4: Deny Negatives

    [Fact]
    public void Add_ContainsNegativeNumber_ThrowsNegativeNumbersException()
    {
        // Act & Assert
        var ex = Assert.Throws<NegativeNumbersException>(() => _calculator.Add("2,-3"));
        Assert.Contains("Negatives not allowed", ex.Message);
        Assert.Contains("-3", ex.Message);
    }

    [Fact]
    public void Add_MultipleNegativeNumbers_ListsAllInException()
    {
        // Act & Assert
        var ex = Assert.Throws<NegativeNumbersException>(() => _calculator.Add("2,-3,-5"));
        Assert.Contains("-3", ex.Message);
        Assert.Contains("-5", ex.Message);
    }

    #endregion

    #region Step 5: Ignore Numbers > 1000

    [Fact]
    public void Add_NumberGreaterThan1000_IgnoresIt()
    {
        // Act
        int result = _calculator.Add("2,1001");

        // Assert
        Assert.Equal(2, result);
    }

    [Fact]
    public void Add_NumberEqualTo1000_Includes()
    {
        // Act
        int result = _calculator.Add("1000,1");

        // Assert
        Assert.Equal(1001, result);
    }

    #endregion

    #region Step 6: Custom Single-Char Delimiter

    [Fact]
    public void Add_CustomSingleCharDelimiter_ReturnsSum()
    {
        // Act
        int result = _calculator.Add("//;\n1;2;3");

        // Assert
        Assert.Equal(6, result);
    }

    [Fact]
    public void Add_CustomPipeDelimiter_ReturnsSum()
    {
        // Act
        int result = _calculator.Add("//|\n4|5|6");

        // Assert
        Assert.Equal(15, result);
    }

    #endregion

    #region Step 7: Custom Any-Length Delimiter

    [Fact]
    public void Add_CustomMultiCharDelimiter_ReturnsSum()
    {
        // Act
        int result = _calculator.Add("//[***]\n1***2***3");

        // Assert
        Assert.Equal(6, result);
    }

    [Fact]
    public void Add_CustomWordDelimiter_ReturnsSum()
    {
        // Act
        int result = _calculator.Add("//[and]\n10and20and30");

        // Assert
        Assert.Equal(60, result);
    }

    #endregion

    #region Step 8: Multiple Delimiters

    [Fact]
    public void Add_MultipleCustomDelimiters_ReturnsSum()
    {
        // Act
        int result = _calculator.Add("//[*][%]\n1*2%3");

        // Assert
        Assert.Equal(6, result);
    }

    [Fact]
    public void Add_MultipleLongDelimiters_ReturnsSum()
    {
        // Act
        int result = _calculator.Add("//[***][%%%]\n1***2%%%3");

        // Assert
        Assert.Equal(6, result);
    }

    [Fact]
    public void Add_ThreeCustomDelimiters_ReturnsSum()
    {
        // Act
        int result = _calculator.Add("//[*][%][#]\n1*2%3#4");

        // Assert
        Assert.Equal(10, result);
    }

    #endregion

    #region Parser Tests

    [Fact]
    public void Parse_EmptyString_ReturnsEmptyParsedInput()
    {
        // Act
        var result = _numberParser.Parse("");

        // Assert
        Assert.Empty(result.ValidNumbers);
        Assert.Empty(result.NegativeNumbers);
    }

    [Fact]
    public void Parse_CommaDelimitedNumbers_ExtractsAllNumbers()
    {
        // Act
        var result = _numberParser.Parse("1,2,3");

        // Assert
        Assert.Equal(new[] { 1, 2, 3 }, result.ValidNumbers);
    }

    [Fact]
    public void Parse_NewlineDelimitedNumbers_ExtractsAllNumbers()
    {
        // Act
        var result = _numberParser.Parse("1\n2\n3");

        // Assert
        Assert.Equal(new[] { 1, 2, 3 }, result.ValidNumbers);
    }

    [Fact]
    public void Parse_NegativeNumbers_PopulatesNegativesList()
    {
        // Act
        var result = _numberParser.Parse("1,-2,-3");

        // Assert
        Assert.Single(result.ValidNumbers);
        Assert.Equal(new[] { -2, -3 }, result.NegativeNumbers);
    }

    [Fact]
    public void Parse_NumbersAbove1000_IgnoredButNotInvalidTokens()
    {
        // Act
        var result = _numberParser.Parse("1,1001,2");

        // Assert
        Assert.Equal(new[] { 1, 2 }, result.ValidNumbers);
        Assert.DoesNotContain("1001", result.InvalidTokens);
    }

    #endregion
}
