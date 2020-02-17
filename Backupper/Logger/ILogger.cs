namespace Backupper.Logger
{
    /// <summary>
    /// Интерфейс логгера - содержит свойство уровня логгирования, а так же методы в зависимости от уровня логгирования.
    /// </summary>
    public interface ILogger
    {
        LogLevel Level { get; set; }

        void Info(string message);

        void Percents(string message);

        void Error(string message);

        void Debug(string message);
    }
}