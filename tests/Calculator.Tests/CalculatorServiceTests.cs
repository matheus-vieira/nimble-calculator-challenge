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
        services.AddSingleton<AddOperation>();
        services.AddSingleton<SubtractOperation>();
        services.AddSingleton<MultiplyOperation>();
        services.AddSingleton<DivideOperation>();
        services.AddSingleton<ICalculatorOperationResolver>(sp =>
        {
            Dictionary<OperationType, ICalculatorOperation> operations = new()
            {
                [OperationType.Add] = sp.GetRequiredService<AddOperation>(),
                [OperationType.Subtract] = sp.GetRequiredService<SubtractOperation>(),
                [OperationType.Multiply] = sp.GetRequiredService<MultiplyOperation>(),
                [OperationType.Divide] = sp.GetRequiredService<DivideOperation>()
            };
            return new CalculatorOperationResolver(operations);
        });
        services.AddSingleton<ICalculator, CalculatorService>();

        var serviceProvider = services.BuildServiceProvider();
        _numberParser = serviceProvider.GetRequiredService<INumberParser>();
        _calculator = serviceProvider.GetRequiredService<ICalculator>();
    }

}
