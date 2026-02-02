# Nimble Calculator Challenge

Console calculator built in C#/.NET with a strong focus on readability, testability, and separation of concerns. It implements all core requirements and stretch goals from the challenge, including custom delimiters, formula output, and additional operations.

## Features

- ✅ Add with all delimiter rules (comma, newline, custom single/multi delimiter, multiple delimiters)
- ✅ Handles invalid tokens and missing numbers as zero
- ✅ Ignores values greater than the upper bound (default: 1000)
- ✅ Negative numbers validation (default: denied)
- ✅ Formula output (e.g., `2+0+4+0+6 = 12`)
- ✅ Additional operations: Subtract, Multiply, Divide
- ✅ Continuous console mode (until Ctrl+C / `exit`)
- ✅ Configurable options via CLI flags

## Project Structure

- src/Calculator.Core — Core services, parsing, validation, operations
- src/Calculator.Console — CLI entry point and DI setup
- tests/Calculator.Tests — Unit tests for parsing, validation, and operations

## Getting Started

### Build

- `dotnet build`

### Run

- `dotnet run --project src/Calculator.Console`

### Test

- `dotnet test`

## Console Usage

Commands:

- `add [input]` — Add numbers (default)
- `sub [input]` — Subtract numbers
- `mul [input]` — Multiply numbers
- `div [input]` — Divide numbers
- `formula` — Toggle formula display
- `exit` — Quit

Examples:

- `add 1,2,3`
- `sub 10,3`
- `mul 2,3,4`
- `div 100,5,2`

## CLI Options

These flags configure the calculator at startup:

- `--alt-delim=;` or `--alt-delimiter=;` — Alternate delimiter for step #3 (default newline)
- `--allow-negatives` — Allows negative numbers
- `--deny-negatives` — Denies negative numbers (default)
- `--upper-bound=500` — Sets max valid number (default 1000)

Example:

- `dotnet run --project src/Calculator.Console -- --alt-delim=; --upper-bound=500 --allow-negatives`

## Architecture Notes

- **Parsing:** `NumberParser` supports all delimiter formats.
- **Validation:** `ValidationService` centralizes rules and exposes both valid and display numbers.
- **Operations:** Each operation is isolated (SRP) and resolved via keyed DI at runtime.
- **Options:** `CalculatorOptions` enables configuration without changing core logic.

## Stretch Goals Coverage

- Formula output ✅
- Continuous processing ✅
- CLI arguments for delimiter/negatives/upper bound ✅
- DI ✅
- Subtract / Multiply / Divide ✅

## License

MIT — see LICENSE for details.
