using System;
using System.Collections.Generic;
using System.Text;

namespace firefly.core.Domain
{
    static class Logger
    {
        public static void Message(string Message, LogSeverity Severity = LogSeverity.Information)
        {
            switch (Severity)
            {
                case LogSeverity.Warning:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine(Message);
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogSeverity.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(Message);
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogSeverity.Information:
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(Message);
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
        }
    }

    public enum LogSeverity
    {
        Information,
        Warning,
        Error
    }
}
