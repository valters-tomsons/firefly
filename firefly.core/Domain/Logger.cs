using System;

namespace firefly.core.Domain;

static class Logger
{
    public static void Message(string Message, LogSeverity Severity = LogSeverity.Information, bool KeepLine = false)
    {
        switch (Severity)
        {
            case LogSeverity.Warning:
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                break;
            case LogSeverity.Error:
                Console.ForegroundColor = ConsoleColor.Red;
                break;
            case LogSeverity.Information:
                Console.ForegroundColor = ConsoleColor.White;
                break;
        }

        if (KeepLine)
        {
            Console.Write(" | " + Message);
        }
        else
        {
            Console.WriteLine();
            Console.WriteLine(Message);
        }

        Console.ForegroundColor = ConsoleColor.White;
    }
}

public enum LogSeverity
{
    Information,
    Warning,
    Error
}
