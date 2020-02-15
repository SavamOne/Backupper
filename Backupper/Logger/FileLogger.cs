using System;
using System.IO;

namespace Backupper.Logger
{
    public class FileLogger : ConsoleLogger, IDisposable
    {
        StreamWriter Writer { get; }

        private string LogName()
        {
            if(File.Exists("log.txt"))
            {
                int i = 0;

                while (File.Exists($"log{i}.txt"))
                    i++;
                return $"log{i}.txt";
            }
            return "log.txt";
        }

        public FileLogger(LogLevel level) : base(level)
        {
            var fileName = LogName();
            try
            {
                Writer = new StreamWriter(fileName, true);
            }
            catch
            {
                Writer = null;
            }
        }

        public override void Debug(string message)
        {
            base.Debug(message);
            if (Level.HasFlag(LogLevel.Debug))
            {
                Writer?.WriteLine($"[Debug] : {message}");
            }
        }

        public override void Percents(string message)
        {
            base.Percents(message);
            if (Level.HasFlag(LogLevel.Percents))
            {
                Writer?.WriteLine($"[Percents] : {message}");
            }
        }

        public override void Error(string message)
        {
            base.Error(message);
            if (Level.HasFlag(LogLevel.Error))
            {
                Writer?.WriteLine($"[Error] : {message}");
            }
        }

        public override void Info(string message)
        {
            base.Info(message);
            if (Level.HasFlag(LogLevel.Info))
            {
                Writer?.WriteLine($"[Info] : {message}");
            }
        }
   
        bool isDisposed = false;

        public void Dispose()
        {
            if(!isDisposed)
            {
                Writer?.Dispose();
                isDisposed = true;
            }
        }
    }
}
