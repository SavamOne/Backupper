using Backupper.Logger;

namespace Backupper
{
    class Program
    {
        static void Main(string[] args)
        {
            ILogger log = new ConsoleLogger(LogLevel.Info | LogLevel.Error);

            log.Info("Информация");
            log.Error("Ошибка");
            log.Debug("Дебаг");
        }
    }
}
