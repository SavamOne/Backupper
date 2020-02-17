using Backupper.Logger;
using System;
using System.IO;

namespace Backupper.Worker
{
    public static partial class BackupWorker
    {
        /// <summary>
        /// Создание директории
        /// </summary>
        /// <param name="logger">Объект типа логгер для ведения процесса создания</param>
        /// <param name="dir">Путь к директории, которая должна создаться</param>
        /// <returns>true - если cоздание пройдено успешно.</returns>
        private static bool CreateDirectory(ILogger logger, string dir)
        {
            logger.Debug($"Создание директории {dir}.");

            try
            {
                Directory.CreateDirectory(dir);
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                logger.Error($"Директорию {dir} не удалось создать. Отсутствует разрешение.");
            }
            catch (DirectoryNotFoundException)
            {
                logger.Error($"Директорию {dir} не удалось создать. Недопустимый путь.");
            }
            catch (Exception e)
            {
                logger.Error($"Директорию {dir} не удалось создать. Необработанное исключение. {e.Message}");
            }
            return false;
        }
    }
}
