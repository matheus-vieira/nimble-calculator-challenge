namespace Calculator.Console;

using Calculator.Core;

/// <summary>
/// Parses command-line arguments to configure calculator options.
/// Single Responsibility: Command-line argument parsing only.
/// </summary>
internal static class OptionParser
{
    /// <summary>
    /// Parses command-line arguments and returns configured calculator options.
    /// </summary>
    /// <param name="args">Command-line arguments from Main.</param>
    /// <returns>CalculatorOptions configured from arguments.</returns>
    internal static CalculatorOptions ParseOptions(string[] args)
    {
        CalculatorOptions options = new();

        foreach (var arg in args)
        {
            if (arg.StartsWith("--alt-delim=", StringComparison.OrdinalIgnoreCase) ||
                arg.StartsWith("--alt-delimiter=", StringComparison.OrdinalIgnoreCase))
            {
                var value = arg.Split('=', 2)[^1];
                if (!string.IsNullOrEmpty(value))
                {
                    options.AlternateDelimiter = value;
                }
                continue;
            }

            if (arg.Equals("--allow-negatives", StringComparison.OrdinalIgnoreCase))
            {
                options.DenyNegatives = false;
                continue;
            }

            if (arg.Equals("--deny-negatives", StringComparison.OrdinalIgnoreCase))
            {
                options.DenyNegatives = true;
                continue;
            }

            if (arg.StartsWith("--upper-bound=", StringComparison.OrdinalIgnoreCase))
            {
                var value = arg.Split('=', 2)[^1];
                if (int.TryParse(value, out int upperBound) && upperBound >= 0)
                {
                    options.UpperBound = upperBound;
                }
            }
        }

        return options;
    }
}
