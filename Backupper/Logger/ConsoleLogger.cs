using System;


namespace Backupper.Logger
{
    public class ConsoleLogger : ILogger
    {
        private ConsoleColor InfoColor { get; }

        private ConsoleColor PercentsColor { get; }

        private ConsoleColor ErrorColor { get; }

        private ConsoleColor DebugColor { get; }

        public LogLevel Level { get; set; }

        public ConsoleLogger(LogLevel level = LogLevel.Percents)
        {
            InfoColor = ConsoleColor.Gray;
            ErrorColor = ConsoleColor.DarkRed;
            DebugColor = ConsoleColor.DarkMagenta;
            PercentsColor = ConsoleColor.DarkGreen;

            Level = level;
        }

        public virtual void Debug(string message)
        {
            if(Level.HasFlag(LogLevel.Debug))
            {
                Console.ForegroundColor = DebugColor;
                Console.WriteLine($"[Debug] : {message}");
            }
        }

        public virtual void Percents(string message)
        {
            if (Level.HasFlag(LogLevel.Percents))
            {
                Console.ForegroundColor = PercentsColor;
                Console.WriteLine($"[Percents] : {message}");
            }
        }

        public virtual void Error(string message)
        {
            if (Level.HasFlag(LogLevel.Error))
            {
                Console.ForegroundColor = ErrorColor;
                Console.WriteLine($"[Error] : {message}");
            }
        }

        public virtual void Info(string message)
        {
            if(Level.HasFlag(LogLevel.Info))
            {
                Console.ForegroundColor = InfoColor;
                Console.WriteLine($"[Info] : {message}");
            }
        }
    }
}
