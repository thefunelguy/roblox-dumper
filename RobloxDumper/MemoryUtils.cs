using static RobloxDumper.WinAPI;

namespace RobloxDumper
{
    /// <summary>
    /// Utility methods for memory operations
    /// </summary>
    public static class MemoryUtils
    {
        /// <summary>
        /// Checks if memory protection allows reading or execution
        /// </summary>
        public static bool IsReadableOrExecutable(MemoryProtection protect) =>
            protect == MemoryProtection.PAGE_READONLY ||
            protect == MemoryProtection.PAGE_READWRITE ||
            protect == MemoryProtection.PAGE_EXECUTE_READ ||
            protect == MemoryProtection.PAGE_EXECUTE_READWRITE ||
            protect == MemoryProtection.PAGE_EXECUTE;

        /// <summary>
        /// Checks if memory region is accessible (committed and not no-access)
        /// </summary>
        public static bool IsAccessibleMemory(MEMORY_BASIC_INFORMATION memInfo) =>
            (memInfo.State & MemoryState.MEM_COMMIT) != 0 &&
            memInfo.Protect != MemoryProtection.PAGE_NOACCESS &&
            memInfo.Protect != 0;
    }
}
