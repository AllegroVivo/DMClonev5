using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace DungeonMaker.Utilities;

[Flags]
public enum LogLevel
{
    None = 0,
    
    Debug = 1 << 0,
    Info = 1 << 1,
    Warning = 1 << 2,
    Error = 1 << 3,
    
    All = Debug | Info | Warning | Error
}

public static class Logger
{
    private static StreamWriter fileWriter;
    
    public static LogLevel MinimumLevel { get; set; } = LogLevel.All;
    public static String LogFilePath { get; set; } = "log.txt";
    
    public static void Initialize(Boolean writeToFile = false)
    {
        if (writeToFile)
        {
            fileWriter = new StreamWriter(LogFilePath, true)
            {
                AutoFlush = true
            };
            Info("Initialized.");
        }
        else
        {
            Info("Initialized without file stream.");
        }
    }

    public static void Shutdown() => fileWriter?.Close();
    
    private static void Log(LogLevel level, String message, String caller, String filePath, Int32 lineNumber)
    {
        if (!MinimumLevel.HasFlag(level)) 
            return;

        String logMessage = $"[{DateTime.Now:HH:mm:ss}] [{level}] {Path.GetFileName(filePath)}:{lineNumber} ({caller}) - {message}";
        ConsoleColor originalColor = Console.ForegroundColor;

        // Set color based on log level
        Console.ForegroundColor = level switch
        {
            LogLevel.Debug => ConsoleColor.Gray,
            LogLevel.Info => ConsoleColor.White,
            LogLevel.Warning => ConsoleColor.Yellow,
            LogLevel.Error => ConsoleColor.Red,
            _ => ConsoleColor.Magenta
        };

        Console.WriteLine(logMessage);
        Console.ForegroundColor = originalColor;

        // Always write plain text to file
        fileWriter?.WriteLine(logMessage);
    }

    public static void Debug(String message,
        [CallerMemberName] String caller = "",
        [CallerFilePath] String filePath = "",
        [CallerLineNumber] Int32 lineNumber = 0) => Log(LogLevel.Debug, message, caller, filePath, lineNumber);
    public static void Info(String message,
        [CallerMemberName] String caller = "",
        [CallerFilePath] String filePath = "",
        [CallerLineNumber] Int32 lineNumber = 0) => Log(LogLevel.Info, message, caller, filePath, lineNumber);
    public static void Warning(String message,
        [CallerMemberName] String caller = "",
        [CallerFilePath] String filePath = "",
        [CallerLineNumber] Int32 lineNumber = 0) => Log(LogLevel.Warning, message, caller, filePath, lineNumber);
    public static void Error(String message,
        [CallerMemberName] String caller = "",
        [CallerFilePath] String filePath = "",
        [CallerLineNumber] Int32 lineNumber = 0) => Log(LogLevel.Error, message, caller, filePath, lineNumber);
}