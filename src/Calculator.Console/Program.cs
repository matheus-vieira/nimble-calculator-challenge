using Calculator.Core;
using Calculator.Core.Exceptions;
using Calculator.Core.Interfaces;
using Calculator.Core.Services;
using Calculator.Core.Services.Operations;
using Calculator.UI.Console;
using Microsoft.Extensions.DependencyInjection;

// Parse command-line options
var options = OptionParser.ParseOptions(args);

// Set up dependency injection
var services = new ServiceCollection();

// Core services (stateless -> singleton)
services.AddSingleton(options);
services.AddSingleton<INumberParser, NumberParser>();
services.AddSingleton<ValidationService>();

// Operation implementations
services.AddSingleton<AddOperation>();
services.AddSingleton<SubtractOperation>();
services.AddSingleton<MultiplyOperation>();
services.AddSingleton<DivideOperation>();

// Operation resolver with explicit dependencies
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

// Calculator service
services.AddSingleton<ICalculator, CalculatorService>();

using var serviceProvider = services.BuildServiceProvider();
ICalculator calculator = serviceProvider.GetRequiredService<ICalculator>();

var ui = new ConsoleUI(calculator);
ui.Run();
