using Calculator.Core.Models;
using Calculator.Core.Services;
using Xunit;

namespace Calculator.Tests;

public class TokenProcessorTests
{
    private readonly TokenProcessor _processor = new();

    [Fact]
    public void SplitByDelimiters_NoDelimiters_ReturnsInput()
    {
        // Arrange
        List<string> delimiters = [];

        // Act
        List<string> result = _processor.SplitByDelimiters("1,2,3", delimiters);

        // Assert
        Assert.Equal(new[] { "1,2,3" }, result);
    }

    [Fact]
    public void SplitByDelimiters_EmptyPattern_ReturnsInput()
    {
        // Arrange
        List<string> delimiters = [""]; // pattern becomes empty

        // Act
        List<string> result = _processor.SplitByDelimiters("1,2,3", delimiters);

        // Assert
        Assert.Equal(new[] { "1,2,3" }, result);
    }

    [Fact]
    public void ProcessToken_Whitespace_AddsInvalidAndNull()
    {
        // Arrange
        ParsedInput result = new();

        // Act
        _processor.ProcessToken("   ", result);

        // Assert
        Assert.Single(result.InvalidTokens);
        Assert.Single(result.TokenNumbers);
        Assert.Null(result.TokenNumbers[0]);
    }

    [Fact]
    public void ProcessToken_InvalidNumber_AddsInvalidAndNull()
    {
        // Arrange
        ParsedInput result = new();

        // Act
        _processor.ProcessToken("abc", result);

        // Assert
        Assert.Single(result.InvalidTokens);
        Assert.Single(result.TokenNumbers);
        Assert.Null(result.TokenNumbers[0]);
    }

    [Fact]
    public void ProcessToken_Negative_AddsNegativeNumber()
    {
        // Arrange
        ParsedInput result = new();

        // Act
        _processor.ProcessToken("-5", result);

        // Assert
        Assert.Contains(-5, result.NegativeNumbers);
        Assert.Single(result.TokenNumbers);
        Assert.Equal(-5, result.TokenNumbers[0]);
    }
}
