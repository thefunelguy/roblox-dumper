# Roblox Function Dumper

> **⚠️ PROOF OF CONCEPT** - Educational memory scanning and pattern recognition in Windows processes.

## Overview

A memory scanner that locates functions in the Roblox process by searching for string references and analyzing x64 assembly instructions. **Read-only operation - no modifications made.**

## Features

- String-based function discovery
- x64 assembly pattern recognition  
- Colored console output
- High-performance memory scanning
- Safe read-only operation

## How It Works

### Phase 1: String Location

1. Find Roblox process (`RobloxPlayerBeta.exe`)
2. Scan memory regions for target string
3. Record string address

### Phase 2: Assembly Analysis

1. Search for `LEA REG, [RIP+disp32]` instructions referencing the string
2. Find subsequent `CALL rel32` instruction  
3. Calculate function offset from CALL target

### Assembly Pattern

```assembly
lea rdx, [rip+0x12345678]  ; Load string address
call 0x12345678            ; Call target function
```

## Components

- `MemoryScanner.cs` - Core memory scanning and function discovery
- `PatternSearcher.cs` - Pattern matching algorithms  
- `ConsoleHelper.cs` - Colored console output
- `MemoryUtils.cs` - Memory operation utilities
- `WinAPI.cs` - Windows API bindings

## Building and Running

**Prerequisites:** .NET 9.0 SDK, Windows

```bash
dotnet build
dotnet run --project RobloxDumper
```

**Usage:** Run Roblox first, then execute the tool.

## Sample Output

```text
╔═══════════════════════════════╗
║  Roblox Function Dumper  ║
╚═══════════════════════════════╝

🔍 Searching for function...
✓ Found Roblox process with PID: 12345
✓ String found in memory at: 0x7FF1234567890
✓ Found LEA instruction at offset: 0x123456
✓ Found CALL instruction at offset: 0x12345D
✓ Function offset: 0x987654321

ℹ Press any key to exit...
```

## Technical Notes

- Uses unsafe code for direct memory access
- Scans only accessible memory regions
- Employs optimized pattern matching algorithms
- Read-only operations with proper error handling

**Dependencies:** Gee.External.Capstone, .NET 9.0

This is an educational proof-of-concept demonstrating memory analysis techniques.
