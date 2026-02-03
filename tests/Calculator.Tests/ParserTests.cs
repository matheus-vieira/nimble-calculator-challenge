using Xunit;

namespace Calculator.Tests;

public class ParserTests : CalculatorTestBase
{
    [Fact]
    public void Parse_EmptyString_ReturnsEmptyParsedInput()
    {
        // Act
        var result = NumberParser.Parse("");

        // Assert
        Assert.Empty(result.TokenNumbers);
        Assert.Empty(result.NegativeNumbers);
        Assert.Empty(result.InvalidTokens);
    }

    [Fact]
    public void Parse_CommaDelimitedNumbers_ExtractsAllNumbers()
    {
        // Act
        var result = NumberParser.Parse("1,2,3");

        // Assert
        Assert.Equal(new int?[] { 1, 2, 3 }, result.TokenNumbers);
    }

    [Fact]
    public void Parse_NewlineDelimitedNumbers_ExtractsAllNumbers()
    {
        // Act
        var result = NumberParser.Parse("1\n2\n3");

        // Assert
        Assert.Equal(new int?[] { 1, 2, 3 }, result.TokenNumbers);
    }

    [Fact]
    public void Parse_NegativeNumbers_PopulatesNegativesList()
    {
        // Act
        var result = NumberParser.Parse("1,-2,-3");

        // Assert
        Assert.Equal(new int?[] { 1, -2, -3 }, result.TokenNumbers);
        Assert.Equal(new[] { -2, -3 }, result.NegativeNumbers);
    }

    [Fact]
    public void Parse_NumbersAbove1000_ParsedButNotInvalidTokens()
    {
        // Act
        var result = NumberParser.Parse("1,1001,2");

        // Assert
        Assert.Equal(new int?[] { 1, 1001, 2 }, result.TokenNumbers);
        Assert.DoesNotContain("1001", result.InvalidTokens);
    }
}
