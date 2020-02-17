using Backupper.Logger;
using System.IO;

namespace Backupper.Worker
{
    public static partial class BackupWorker
    {
        private static bool CheckDirectories(ILogger logger, string dirFrom, string dirTo)
        {
            logger.Debug($"Проверка директорий.");


            if (dirFrom == null)
            {
                logger.Error($"Директория источника равна null.");
                return false;
            }

            if (dirTo == null)
            {
                logger.Error($"Директория назначения равна null.");
                return false;
            }

            if (!Directory.Exists(dirFrom))
            {
                logger.Error($"Директории {dirFrom} не существует.");
                return false;
            }

            if (!Directory.Exists(dirTo))
            {
                logger.Debug($"Директрии {dirTo} не существует.");

                if (!CreateDirectory(logger, dirTo))
                    return false;
            }

            dirFrom = Path.GetFullPath(dirFrom);
            dirTo = Path.GetFullPath(dirTo);
            if (dirTo.StartsWith($"{dirFrom}\\"))
            {
                logger.Error($"Директрия {dirTo} является поддиректорией {dirFrom}.");
                return false;
            }

            return true;
        }
    }
}
