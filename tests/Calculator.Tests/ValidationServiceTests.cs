using Xunit;
using Calculator.Core;
using Calculator.Core.Services;
using Calculator.Core.Exceptions;

namespace Calculator.Tests;

/// <summary>
/// Unit tests for ValidationService.
/// Focused tests for validation logic only.
/// </summary>
public class ValidationServiceTests
{
    private readonly ValidationService _validationService;

    public ValidationServiceTests()
    {
        var options = new CalculatorOptions();
        var numberParser = new NumberParser(options);
        _validationService = new ValidationService(numberParser, options);
    }

    [Fact]
    public void GetValidatedNumbers_EmptyString_ReturnsEmptyList()
    {
        // Act
        var result = _validationService.GetValidatedNumbers("");

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void GetValidatedNumbers_ValidNumbers_ReturnsNumbers()
    {
        // Act
        var result = _validationService.GetValidatedNumbers("1,2,3");

        // Assert
        Assert.Equal(new[] { 1, 2, 3 }, result);
    }

    [Fact]
    public void GetValidatedNumbers_ContainsNegative_ThrowsException()
    {
        // Act & Assert
        var ex = Assert.Throws<NegativeNumbersException>(() => 
            _validationService.GetValidatedNumbers("1,-2,3"));
        Assert.Contains("-2", ex.Message);
    }

    [Fact]
    public void GetValidatedNumbers_MultipleNegatives_ListsAllInException()
    {
        // Act & Assert
        var ex = Assert.Throws<NegativeNumbersException>(() => 
            _validationService.GetValidatedNumbers("1,-2,-3"));
        Assert.Contains("-2", ex.Message);
        Assert.Contains("-3", ex.Message);
    }

    [Fact]
    public void GetValidatedNumbers_NumbersAbove1000_IgnoresThem()
    {
        // Act
        var result = _validationService.GetValidatedNumbers("100,1001,200");

        // Assert
        Assert.Equal(new[] { 100, 200 }, result);
    }

    [Fact]
    public void GetValidatedNumbers_InvalidTokens_IgnoresThem()
    {
        // Act
        var result = _validationService.GetValidatedNumbers("1,abc,2");

        // Assert
        Assert.Equal(new[] { 1, 2 }, result);
    }

    [Fact]
    public void GetValidatedNumbers_NegativesAllowed_ReturnsNegatives()
    {
        // Arrange
        var options = new CalculatorOptions { DenyNegatives = false };
        var numberParser = new NumberParser(options);
        var validationService = new ValidationService(numberParser, options);

        // Act
        var result = validationService.GetValidatedNumbers("1,-2,3");

        // Assert
        Assert.Equal(new[] { 1, -2, 3 }, result);
    }

    [Fact]
    public void GetValidatedNumbers_UpperBoundOverride_IgnoresAboveCustomBound()
    {
        // Arrange
        var options = new CalculatorOptions { UpperBound = 10 };
        var numberParser = new NumberParser(options);
        var validationService = new ValidationService(numberParser, options);

        // Act
        var result = validationService.GetValidatedNumbers("5,11,10");

        // Assert
        Assert.Equal(new[] { 5, 10 }, result);
    }

    [Fact]
    public void GetValidatedNumbers_AlternateDelimiter_ReturnsNumbers()
    {
        // Arrange
        var options = new CalculatorOptions { AlternateDelimiter = ";" };
        var numberParser = new NumberParser(options);
        var validationService = new ValidationService(numberParser, options);

        // Act
        var result = validationService.GetValidatedNumbers("1;2;3");

        // Assert
        Assert.Equal(new[] { 1, 2, 3 }, result);
    }
}
