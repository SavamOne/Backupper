using Backupper.Logger;
using Backupper.Worker;
using Backupper.Сonfig;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Backupper
{
    class Program
    {
        static void Main(string[] args)
        {
            ///Если не удаcтся прочитать конфиг, вывести эту ошибки в консоли.
            ILogger logger = new ConsoleLogger(LogLevel.Error); 
            try
            {
                ///Чтение и десереализация конфига
                string json = File.ReadAllText("config.json");
                Config config = JsonConvert.DeserializeObject<Config>(json);

                logger = new FileLogger(config.Level);


                using(logger as IDisposable)
                {
                    ///Для каждого пути выполнить копирование
                    foreach (var directories in config.Directories)
                    {
                        BackupWorker.DoWork(logger,
                                            directories.DirectoryFrom,
                                            directories.DirectoryTo,
                                            continueOnError: config.ContinueOnError,
                                            overwriteFiles: config.OverwriteFiles);
                    }
                }
            }
            catch(UnauthorizedAccessException)
            {
                logger.Error("Не удалось открыть файл конфигурации. Нет разрешения.");
            }
            catch(FileNotFoundException)
            {
                logger.Error("Отсутствует файл конфигурации.");
            }
            catch(Exception e)
            {
                logger.Error($"Не удалось прочитать файл конфигурации. Необработанное исключение. {e.Message}");
            }

            Console.ReadKey();
        }
    }
}
