using System;

namespace Backupper.Logger
{
    class ConsoleLogger : ILogger
    {
        private ConsoleColor DefaultColor { get; }

        private ConsoleColor ErrorColor { get; }

        private ConsoleColor DebugColor { get; }

        public LogLevel Level { get; set; }

        public ConsoleLogger(LogLevel level = LogLevel.Info)
        {
            DefaultColor = ConsoleColor.White;
            ErrorColor = ConsoleColor.Red;
            DebugColor = ConsoleColor.Blue;

            Level = level;
        }

        public void Debug(string message)
        {
            if(Level.HasFlag(LogLevel.Debug))
            {
                Console.ForegroundColor = DebugColor;
                Console.WriteLine($"[Debug] : {message}");
            }
        }

        public void Error(string message)
        {
            if (Level.HasFlag(LogLevel.Error))
            {
                Console.ForegroundColor = ErrorColor;
                Console.WriteLine($"[Error] : {message}");
            }
        }

        public void Info(string message)
        {
            if(Level.HasFlag(LogLevel.Info))
            {
                Console.ForegroundColor = DefaultColor;
                Console.WriteLine($"[Info]  : {message}");
            }
        }
    }
}
