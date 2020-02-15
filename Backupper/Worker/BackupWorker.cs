using Backupper.Logger;
using System.Diagnostics;

namespace Backupper.Worker
{
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
