using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using static RobloxDumper.WinAPI;

namespace RobloxDumper
{
    /// <summary>
    /// Main memory scanner for finding function offsets in Roblox process
    /// </summary>
    public static unsafe class MemoryScanner
    {
        /// <summary>
        /// Finds the offset of a function by searching for a string reference and analyzing assembly code
        /// </summary>
        /// <param name="searchString">The string to search for in memory</param>
        /// <returns>Function offset if found, null otherwise</returns>
        public static long? FindFunctionOffset(string searchString)
        {
            ConsoleHelper.PrintInfo($"Starting function search for string: '{searchString}'");

            var proc = Process.GetProcessesByName("RobloxPlayerBeta")
                .FirstOrDefault(p => p.MainWindowTitle == "Roblox");
            if (proc == null)
            {
                ConsoleHelper.PrintError("Roblox process not found!");
                return null;
            }

            ConsoleHelper.PrintSuccess($"Found Roblox process with PID: {proc.Id}");

            IntPtr hProc = OpenProcess(PROCESS_QUERY_INFORMATION | PROCESS_VM_READ, false, proc.Id);
            if (hProc == IntPtr.Zero)
            {
                ConsoleHelper.PrintError("Failed to open process handle!");
                return null;
            }

            ConsoleHelper.PrintSuccess("Process handle opened successfully");
            ConsoleHelper.PrintSeparator();

            try
            {
                byte[] searchBytes = Encoding.ASCII.GetBytes(searchString);
                IntPtr addr = IntPtr.Zero;
                IntPtr foundString = IntPtr.Zero;
                const long maxAddr = 0x7FFFFFFFFFFF;

                ConsoleHelper.PrintPhase(1, $"Searching for string '{searchString}' in memory...");

                // Phase 1: Find the string in memory (optimized)
                while ((ulong)addr.ToInt64() < (ulong)maxAddr)
                {
                    if (VirtualQueryEx(hProc, addr, out MEMORY_BASIC_INFORMATION memInfo, (UIntPtr)Marshal.SizeOf<MEMORY_BASIC_INFORMATION>()) == 0)
                        break;

                    if (MemoryUtils.IsAccessibleMemory(memInfo))
                    {
                        // Allocate unmanaged memory for better performance
                        IntPtr buffer = Marshal.AllocHGlobal(memInfo.RegionSize);
                        try
                        {
                            if (ReadProcessMemory(hProc, memInfo.BaseAddress, (void*)buffer, (UIntPtr)memInfo.RegionSize, out UIntPtr bytesRead))
                            {
                                fixed (byte* searchPattern = searchBytes)
                                {
                                    long idx = PatternSearcher.SearchPatternIndex((byte*)buffer, (long)bytesRead, searchPattern, searchBytes.Length);
                                    if (idx != -1)
                                    {
                                        foundString = IntPtr.Add(memInfo.BaseAddress, (int)idx);
                                        ConsoleHelper.PrintSuccess("String found in memory!");
                                        ConsoleHelper.PrintAddress("String address", foundString.ToInt64());
                                        Marshal.FreeHGlobal(buffer);
                                        break;
                                    }
                                }
                            }
                        }
                        finally
                        {
                            Marshal.FreeHGlobal(buffer);
                        }
                    }

                    long next = memInfo.BaseAddress.ToInt64() + memInfo.RegionSize.ToInt64();
                    if (next <= addr.ToInt64()) break;
                    addr = new IntPtr(next);
                }

                if (foundString == IntPtr.Zero)
                {
                    ConsoleHelper.PrintError("String not found in memory!");
                    return null;
                }

                Console.WriteLine();
                ConsoleHelper.PrintPhase(2, "Searching for LEA instruction referencing the string...");

                // Phase 2: Find LEA instruction referencing the string (optimized)
                addr = IntPtr.Zero;
                while ((ulong)addr.ToInt64() < (ulong)maxAddr)
                {
                    if (VirtualQueryEx(hProc, addr, out MEMORY_BASIC_INFORMATION memInfo, (UIntPtr)Marshal.SizeOf<MEMORY_BASIC_INFORMATION>()) == 0)
                        break;

                    if ((memInfo.State & MemoryState.MEM_COMMIT) != 0 && MemoryUtils.IsReadableOrExecutable(memInfo.Protect))
                    {
                        IntPtr buffer = Marshal.AllocHGlobal(memInfo.RegionSize);
                        try
                        {
                            if (ReadProcessMemory(hProc, memInfo.BaseAddress, (void*)buffer, (UIntPtr)memInfo.RegionSize, out UIntPtr bytesRead))
                            {
                                long baseAddr = memInfo.BaseAddress.ToInt64();
                                long targetAddr = foundString.ToInt64();

                                long leaOffset = PatternSearcher.FindLeaRipPattern((byte*)buffer, (long)bytesRead, targetAddr, baseAddr);
                                if (leaOffset != -1)
                                {
                                    ConsoleHelper.PrintSuccess("Found LEA instruction!");
                                    ConsoleHelper.PrintAddress("LEA instruction offset", leaOffset);

                                    // Found LEA instruction, now find the next CALL
                                    long callOffset = PatternSearcher.FindCallInstruction((byte*)buffer, (long)bytesRead, leaOffset + 7);
                                    if (callOffset != -1)
                                    {
                                        ConsoleHelper.PrintSuccess("Found CALL instruction!");
                                        ConsoleHelper.PrintAddress("CALL instruction offset", callOffset);

                                        long callInstr = baseAddr + callOffset;
                                        int rel32 = *(int*)((byte*)buffer + callOffset + 1);
                                        long target = callInstr + 5 + rel32;
                                        long offset = target - baseAddr;

                                        Marshal.FreeHGlobal(buffer);

                                        Console.WriteLine();
                                        ConsoleHelper.PrintSuccess("Function analysis completed!");
                                        ConsoleHelper.PrintAddress("Function offset", offset);
                                        ConsoleHelper.PrintAddress("Function address", target);
                                        return offset;
                                    }
                                    else
                                    {
                                        ConsoleHelper.PrintWarning("CALL instruction not found after LEA");
                                    }
                                }
                            }
                        }
                        finally
                        {
                            Marshal.FreeHGlobal(buffer);
                        }
                    }

                    long nextAddr = memInfo.BaseAddress.ToInt64() + memInfo.RegionSize.ToInt64();
                    if (nextAddr <= addr.ToInt64()) break;
                    addr = new IntPtr(nextAddr);
                }

                ConsoleHelper.PrintError("LEA instruction not found!");
                return null;
            }
            finally
            {
                CloseHandle(hProc);
                ConsoleHelper.PrintInfo("Process handle closed.");
            }
        }
    }
}
