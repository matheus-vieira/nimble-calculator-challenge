using Calculator.Core;
using Calculator.Core.Interfaces;
using Calculator.Core.Services;
using Calculator.Core.Services.Operations;
using Calculator.Core.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Calculator.Tests;

public class SubtractTests : CalculatorTestBase
{
    [Fact]
    public void Subtract_EmptyString_ReturnsZero()
    {
        // Act
        int result = Calculator.Subtract("");

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void Subtract_SingleNumber_ReturnsThatNumber()
    {
        // Act
        int result = Calculator.Subtract("15");

        // Assert
        Assert.Equal(15, result);
    }

    [Fact]
    public void Subtract_TwoNumbers_ReturnsResult()
    {
        // Act
        int result = Calculator.Subtract("10,3");

        // Assert
        Assert.Equal(7, result);
    }

    [Fact]
    public void Subtract_MultipleNumbers_ReturnsResult()
    {
        // Act
        int result = Calculator.Subtract("20,5,3");

        // Assert
        Assert.Equal(12, result);
    }

    [Fact]
    public void Subtract_WithInvalidNumbers_IgnoresThem()
    {
        // Act
        int result = Calculator.Subtract("10,abc,3");

        // Assert
        Assert.Equal(7, result);
    }

    [Fact]
    public void Subtract_ContainsNegativeNumber_ThrowsException()
    {
        // Act & Assert
        var ex = Assert.Throws<NegativeNumbersException>(() => Calculator.Subtract("10,-3,5"));
        Assert.Contains("-3", ex.Message);
    }

    [Fact]
    public void Subtract_NumbersGreaterThan1000_IgnoresThem()
    {
        // Act
        int result = Calculator.Subtract("100,1001,10");

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
        services.AddSingleton<AddOperation>();
        services.AddSingleton<ICalculatorOperationResolver>(sp =>
        {
            Dictionary<OperationType, ICalculatorOperation> operations = new()
            {
                [OperationType.Add] = sp.GetRequiredService<AddOperation>()
            };
            return new CalculatorOperationResolver(operations);
        });
        services.AddSingleton<ICalculator, CalculatorService>();

        var serviceProvider = services.BuildServiceProvider();
        var calculator = serviceProvider.GetRequiredService<ICalculator>();

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => calculator.Subtract("10,3"));
        Assert.Contains("Subtract", ex.Message);
    }
}
