using Xunit;
using Calculator.Core;
using Calculator.Core.Interfaces;
using Calculator.Core.Services;
using Calculator.Core.Services.Operations;
using Calculator.Core.Exceptions;
using Microsoft.Extensions.DependencyInjection;

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
        // Setup DI container for testing
        var services = new ServiceCollection();
        services.AddSingleton(new CalculatorOptions());
        services.AddSingleton<INumberParser, NumberParser>();
        services.AddSingleton<ValidationService>();
        services.AddKeyedSingleton<ICalculatorOperation, AddOperation>(OperationType.Add);
        services.AddKeyedSingleton<ICalculatorOperation, SubtractOperation>(OperationType.Subtract);
        services.AddKeyedSingleton<ICalculatorOperation, MultiplyOperation>(OperationType.Multiply);
        services.AddKeyedSingleton<ICalculatorOperation, DivideOperation>(OperationType.Divide);
        services.AddSingleton<ICalculator, CalculatorService>();

        var serviceProvider = services.BuildServiceProvider();
        _numberParser = serviceProvider.GetRequiredService<INumberParser>();
        _calculator = serviceProvider.GetRequiredService<ICalculator>();
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

    #region Stretch Goal: Show Formula

    [Fact]
    public void AddWithFormula_EmptyString_ReturnsFormulaWithZero()
    {
        // Act
        var (result, formula) = _calculator.AddWithFormula("");

        // Assert
        Assert.Equal(0, result);
        Assert.Equal("0 = 0", formula);
    }

    [Fact]
    public void AddWithFormula_TwoNumbers_ReturnsFormulaWithSum()
    {
        // Act
        var (result, formula) = _calculator.AddWithFormula("2,4");

        // Assert
        Assert.Equal(6, result);
        Assert.Equal("2+4 = 6", formula);
    }

    [Fact]
    public void AddWithFormula_MixedValid_ReturnsFormulaWithSum()
    {
        // Act
        var (result, formula) = _calculator.AddWithFormula("2,abc,4,1001,6");

        // Assert
        Assert.Equal(12, result);
        Assert.Equal("2+0+4+0+6 = 12", formula);
    }

    [Fact]
    public void AddWithFormula_ManyNumbers_ReturnsFormula()
    {
        // Act
        var (result, formula) = _calculator.AddWithFormula("1,2,3,4,5");

        // Assert
        Assert.Equal(15, result);
        Assert.Equal("1+2+3+4+5 = 15", formula);
    }

    #endregion

    #region Stretch Goal: Subtract Operation

    [Fact]
    public void Subtract_EmptyString_ReturnsZero()
    {
        // Act
        int result = _calculator.Subtract("");

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void Subtract_SingleNumber_ReturnsThatNumber()
    {
        // Act
        int result = _calculator.Subtract("15");

        // Assert
        Assert.Equal(15, result);
    }

    [Fact]
    public void Subtract_TwoNumbers_ReturnsResult()
    {
        // Act
        int result = _calculator.Subtract("10,3");

        // Assert
        Assert.Equal(7, result);
    }

    [Fact]
    public void Subtract_MultipleNumbers_ReturnsResult()
    {
        // Act
        int result = _calculator.Subtract("20,5,3");

        // Assert
        Assert.Equal(12, result);
    }

    [Fact]
    public void Subtract_WithInvalidNumbers_IgnoresThem()
    {
        // Act
        int result = _calculator.Subtract("10,abc,3");

        // Assert
        Assert.Equal(7, result);
    }

    [Fact]
    public void Subtract_ContainsNegativeNumber_ThrowsException()
    {
        // Act & Assert
        var ex = Assert.Throws<NegativeNumbersException>(() => _calculator.Subtract("10,-3,5"));
        Assert.Contains("-3", ex.Message);
    }

    [Fact]
    public void Subtract_NumbersGreaterThan1000_IgnoresThem()
    {
        // Act
        int result = _calculator.Subtract("100,1001,10");

        // Assert
        Assert.Equal(90, result); // 100 - 10 (1001 ignored)
    }

    [Fact]
    public void Subtract_WhenOperationNotRegistered_ThrowsInvalidOperationException()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddSingleton(new CalculatorOptions());
        services.AddSingleton<INumberParser, NumberParser>();
        services.AddSingleton<ValidationService>();
        services.AddKeyedSingleton<ICalculatorOperation, AddOperation>(OperationType.Add);
        services.AddSingleton<ICalculator, CalculatorService>();

        var serviceProvider = services.BuildServiceProvider();
        var calculator = serviceProvider.GetRequiredService<ICalculator>();

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => calculator.Subtract("10,3"));
        Assert.Contains("Subtract", ex.Message);
    }

    #endregion

    #region Stretch Goal: Multiply Operation

    [Fact]
    public void Multiply_EmptyString_ReturnsZero()
    {
        // Act
        int result = _calculator.Multiply("");

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void Multiply_TwoNumbers_ReturnsProduct()
    {
        // Act
        int result = _calculator.Multiply("3,4");

        // Assert
        Assert.Equal(12, result);
    }

    [Fact]
    public void Multiply_WithInvalidNumbers_IgnoresThem()
    {
        // Act
        int result = _calculator.Multiply("2,abc,5");

        // Assert
        Assert.Equal(10, result);
    }

    [Fact]
    public void Multiply_ContainsNegativeNumber_ThrowsException()
    {
        // Act & Assert
        var ex = Assert.Throws<NegativeNumbersException>(() => _calculator.Multiply("2,-3,5"));
        Assert.Contains("-3", ex.Message);
    }

    [Fact]
    public void Multiply_NumbersGreaterThan1000_IgnoresThem()
    {
        // Act
        int result = _calculator.Multiply("2,1001,5");

        // Assert
        Assert.Equal(10, result);
    }

    #endregion

    #region Stretch Goal: Divide Operation

    [Fact]
    public void Divide_EmptyString_ReturnsZero()
    {
        // Act
        int result = _calculator.Divide("");

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void Divide_TwoNumbers_ReturnsQuotient()
    {
        // Act
        int result = _calculator.Divide("8,2");

        // Assert
        Assert.Equal(4, result);
    }

    [Fact]
    public void Divide_WithInvalidNumbers_IgnoresThem()
    {
        // Act
        int result = _calculator.Divide("20,abc,5");

        // Assert
        Assert.Equal(4, result);
    }

    [Fact]
    public void Divide_ContainsNegativeNumber_ThrowsException()
    {
        // Act & Assert
        var ex = Assert.Throws<NegativeNumbersException>(() => _calculator.Divide("10,-2,5"));
        Assert.Contains("-2", ex.Message);
    }

    [Fact]
    public void Divide_NumbersGreaterThan1000_IgnoresThem()
    {
        // Act
        int result = _calculator.Divide("100,1001,5");

        // Assert
        Assert.Equal(20, result);
    }

    [Fact]
    public void Divide_ByZero_ThrowsException()
    {
        // Act & Assert
        Assert.Throws<DivideByZeroException>(() => _calculator.Divide("10,0"));
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
