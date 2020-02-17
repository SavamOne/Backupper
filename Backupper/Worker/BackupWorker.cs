using Backupper.Logger;
using System.Diagnostics;

namespace Backupper.Worker
{
    /// <summary>
    /// Выполняет копирование файлов и папок из одной директории в другую
    /// </summary>
    public static partial class BackupWorker
    {
        private static Stopwatch Watch { get; set; }

        private static object Locker { get; }

        private static int BufferSize { get; }

        private static byte[] Buffer { get; } 

        static BackupWorker()
        {
            Locker = new object();

            BufferSize = 1024 * 512;
            Buffer = new byte[BufferSize];
        }

        /// <summary>
        /// Главный метод по копированию. Выполняет проверку на корректность директорий, выполняет копирование, логгирует процесс, ведет время копирования
        /// </summary>
        /// <param name="logger">Объект типа логгер для ведения процесса копирования</param>
        /// <param name="dirFrom">Директория, которую надо скопировать</param>
        /// <param name="dirTo">Директория, в которую надо скопировать</param>
        /// <param name="continueOnError">Продолжать процесс, если произошла какая-либо ошибка</param>
        /// <param name="overwriteFiles">Перезаписывать файл, если существует</param>
        public static void DoWork(ILogger logger, string dirFrom, string dirTo, bool continueOnError = true, bool overwriteFiles = false)
        {
            logger.Info($"Процесс копирования {dirFrom} начался.");

            if (!CheckDirectories(logger, dirFrom, dirTo))
            {
                logger.Info("Отмена операции.");
                return;
            }

            Watch = new Stopwatch();
            Watch.Start();

            if (!RecursiveCopying(logger, dirFrom, dirTo, continueOnError, overwriteFiles))
            {
                logger.Info("Отмена операции.");
                Watch.Stop();
                return;
            }

            Watch.Stop();

            logger.Info($"Процесс копирования {dirFrom} завершен.");
            logger.Info($"Время копирования {Watch.Elapsed.Minutes:00}:{Watch.Elapsed.Seconds:00}.");
        }
    }
}
