using Backupper.Logger;
using System;
using System.IO;

namespace Backupper.Worker
{
    public static partial class BackupWorker
    {
        private static bool RecursiveCopying(ILogger logger, string dirFrom, string dirTo, bool continueOnError, bool overwriteFiles)
        {
            string[] subDirsFrom, filesFrom;
            bool dirCreated, subDirCreated, fileCopied;
            string subDirTo, fileTo;

            logger.Info($"Копирование {dirFrom}.");

            try
            {
                filesFrom = Directory.GetFiles(dirFrom);
                subDirsFrom = Directory.GetDirectories(dirFrom);
            }
            catch (UnauthorizedAccessException)
            {
                logger.Error($"Директорию {dirFrom} не удалось открыть. Отстутсвует разрешение.");
                return false;
            }
            catch (Exception e)
            {
                logger.Error($"Директорию {dirFrom} не удалось открыть. Необработанное исключение. {e.Message}");
                return false;
            }

            logger.Debug($"Копирование файлов в {dirFrom}.");

            foreach (string fileFrom in filesFrom)
            {
                fileTo = fileFrom.Replace(dirFrom, dirTo);

                fileCopied = CopyFile(logger, fileFrom, fileTo, overwriteFiles);
                if (!fileCopied && !continueOnError)
                    return false;
            }

            logger.Debug($"Копирование директорий в {dirFrom}.");

            foreach (string subDirFrom in subDirsFrom)
            {
                subDirTo = subDirFrom.Replace(dirFrom, dirTo);

                dirCreated = CreateDirectory(logger, subDirTo);
                if (!dirCreated && !continueOnError)
                    return false;

                if(dirCreated)
                {
                    subDirCreated = RecursiveCopying(logger, subDirFrom, subDirTo, continueOnError, overwriteFiles);
                    if (!subDirCreated && !continueOnError)
                        return false;
                }

            }
            return true;
        }
    }
}
