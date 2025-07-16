namespace RobloxDumper
{
    /// <summary>
    /// Main program entry point
    /// </summary>
    class Program
    {
        static void Main()
        {
            ConsoleHelper.PrintHeader("Roblox Function Dumper");

            ConsoleHelper.PrintSearch("Searching for function...");
            var offset = MemoryScanner.FindFunctionOffset("Current identity is %d");

            Console.WriteLine();
            if (offset.HasValue)
            {
                ConsoleHelper.PrintSuccess("Function search completed successfully!");
                ConsoleHelper.PrintAddress("Function offset", offset.Value);
            }
            else
            {
                ConsoleHelper.PrintError("Function not found!");
            }

            Console.WriteLine();
            ConsoleHelper.PrintInfo("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
