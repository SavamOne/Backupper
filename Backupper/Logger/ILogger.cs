namespace Backupper.Logger
{
    public interface ILogger
    {
        LogLevel Level { get; set; }

        void Info(string message);

        void Percents(string message);

        void Error(string message);

        void Debug(string message);
    }
}