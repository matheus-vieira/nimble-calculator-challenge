using Calculator.Core;
using Calculator.Core.Interfaces;
using Calculator.Core.Services;
using Calculator.Core.Services.Operations;
using Microsoft.Extensions.DependencyInjection;

namespace Calculator.Tests;

public abstract class CalculatorTestBase
{
    protected readonly INumberParser NumberParser;
    protected readonly ICalculator Calculator;

    protected CalculatorTestBase()
    {
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
        NumberParser = serviceProvider.GetRequiredService<INumberParser>();
        Calculator = serviceProvider.GetRequiredService<ICalculator>();
    }
}
