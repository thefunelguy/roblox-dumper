using System;

namespace RobloxDumper
{
    /// <summary>
    /// Helper class for beautiful and colored console output
    /// </summary>
    public static class ConsoleHelper
    {
        // Icons for different message types
        private const string SUCCESS_ICON = "✓";
        private const string ERROR_ICON = "✗";
        private const string INFO_ICON = "ℹ";
        private const string SEARCH_ICON = "🔍";
        private const string WARNING_ICON = "⚠";
        private const string ARROW_ICON = "→";

        /// <summary>
        /// Prints a beautiful header with borders
        /// </summary>
        public static void PrintHeader(string title)
        {
            var border = new string('═', title.Length + 4);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"╔{border}╗");
            Console.WriteLine($"║  {title}  ║");
            Console.WriteLine($"╚{border}╝");
            Console.ResetColor();
            Console.WriteLine();
        }

        /// <summary>
        /// Prints a success message in green with checkmark
        /// </summary>
        public static void PrintSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{SUCCESS_ICON} ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        /// <summary>
        /// Prints an error message in red with X mark
        /// </summary>
        public static void PrintError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"{ERROR_ICON} ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        /// <summary>
        /// Prints an info message in blue with info icon
        /// </summary>
        public static void PrintInfo(string message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"{INFO_ICON} ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        /// <summary>
        /// Prints a search/progress message in yellow with search icon
        /// </summary>
        public static void PrintSearch(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"{SEARCH_ICON} ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        /// <summary>
        /// Prints a warning message in yellow with warning icon
        /// </summary>
        public static void PrintWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"{WARNING_ICON} ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        /// <summary>
        /// Prints an address or value in highlighted format
        /// </summary>
        public static void PrintValue(string label, string value)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write($"{ARROW_ICON} {label}: ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(value);
            Console.ResetColor();
        }

        /// <summary>
        /// Prints a hex address in a highlighted format
        /// </summary>
        public static void PrintAddress(string label, long address)
        {
            PrintValue(label, $"0x{address:X}");
        }

        /// <summary>
        /// Prints a section separator
        /// </summary>
        public static void PrintSeparator()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(new string('─', 50));
            Console.ResetColor();
        }

        /// <summary>
        /// Prints a phase header for multi-step processes
        /// </summary>
        public static void PrintPhase(int phase, string description)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"Phase {phase}: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(description);
            Console.ResetColor();
        }
    }
}
