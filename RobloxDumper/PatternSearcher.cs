using System.Runtime.CompilerServices;

namespace RobloxDumper
{
    /// <summary>
    /// Optimized pattern searching algorithms for memory analysis
    /// </summary>
    public static unsafe class PatternSearcher
    {
        /// <summary>
        /// Optimized pattern search using fast byte comparison
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long SearchPatternIndex(byte* data, long dataLength, byte* pattern, int patternLength)
        {
            if (patternLength == 0 || dataLength < patternLength) return -1;

            byte firstByte = pattern[0];
            long lastPossibleStart = dataLength - patternLength;

            for (long i = 0; i <= lastPossibleStart; i++)
            {
                // Fast first byte check
                if (data[i] != firstByte) continue;

                // Check remaining bytes
                bool match = true;
                for (int j = 1; j < patternLength; j++)
                {
                    if (data[i + j] != pattern[j])
                    {
                        match = false;
                        break;
                    }
                }
                if (match) return i;
            }
            return -1;
        }

        /// <summary>
        /// Optimized instruction pattern search for x64 LEA RIP-relative addressing
        /// Searches for LEA REG, [RIP+disp32] instructions that reference a target address
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long FindLeaRipPattern(byte* buffer, long bufferSize, long targetAddr, long baseAddr)
        {
            for (long i = 0; i <= bufferSize - 7; i++)
            {
                // LEA REG, [RIP+disp32] - 0x48 0x8D 0x15
                if (buffer[i] == 0x48 && buffer[i + 1] == 0x8D && buffer[i + 2] == 0x15)
                {
                    int disp32 = *(int*)(buffer + i + 3);
                    long instrAddr = baseAddr + i;
                    long nextInstr = instrAddr + 7;
                    long calculatedTarget = nextInstr + disp32;

                    if (calculatedTarget == targetAddr)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        /// <summary>
        /// Optimized search for CALL instructions (0xE8 opcode)
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long FindCallInstruction(byte* buffer, long bufferSize, long startOffset)
        {
            for (long i = startOffset; i <= bufferSize - 5; i++)
            {
                if (buffer[i] == 0xE8) // CALL rel32
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
