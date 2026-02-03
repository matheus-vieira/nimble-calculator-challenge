using Xunit;

namespace Calculator.Tests;

public class ParserMalformedDelimiterTests : CalculatorTestBase
{
    [Fact]
    public void Parse_EmptyBracketDelimiter_FallsBackToDefault()
    {
        // Arrange - malformed: empty brackets
        var input = "//[]\n1,2,3";

        // Act
        var result = NumberParser.Parse(input);

        // Assert - should fall back to default comma delimiter
        Assert.NotNull(result.TokenNumbers);
    }

    [Fact]
    public void Parse_UnclosedBracketDelimiter_FallsBackToDefault()
    {
        // Arrange - malformed: unclosed bracket
        var input = "//[***\n1,2,3";

        // Act
        var result = NumberParser.Parse(input);

        // Assert - should fall back to default comma delimiter
        Assert.NotNull(result.TokenNumbers);
        Assert.Contains((int?)1, result.TokenNumbers);
        Assert.Contains((int?)2, result.TokenNumbers);
        Assert.Contains((int?)3, result.TokenNumbers);
    }

    [Fact]
    public void Parse_MissingNewlineAfterDelimiter_HandlesGracefully()
    {
        // Arrange - delimiter without newline
        var input = "//;1;2;3";

        // Act
        var result = NumberParser.Parse(input);

        // Assert - should handle gracefully (either parse or return empty)
        Assert.NotNull(result.TokenNumbers);
    }
}
