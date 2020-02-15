using System;


namespace Backupper.Logger
{
    class ConsoleLogger : ILogger
    {
        private ConsoleColor StatusColor { get; }

        private ConsoleColor InfoColor { get; }

        private ConsoleColor ErrorColor { get; }

        private ConsoleColor DebugColor { get; }

        public LogLevel Level { get; set; }

        public ConsoleLogger(LogLevel level = LogLevel.Status)
        {
            StatusColor = ConsoleColor.Gray;
            ErrorColor = ConsoleColor.DarkRed;
            DebugColor = ConsoleColor.DarkMagenta;
            InfoColor = ConsoleColor.DarkGreen;

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

        public void Info(string message)
        {
            if (Level.HasFlag(LogLevel.Info))
            {
                Console.ForegroundColor = InfoColor;
                Console.WriteLine($"[Info]  : {message}");
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

        public void Status(string message)
        {
            if(Level.HasFlag(LogLevel.Status))
            {
                Console.ForegroundColor = StatusColor;
                Console.WriteLine($"[Status]  : {message}");
            }
        }
    }
}
