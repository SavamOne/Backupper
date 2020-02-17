using Backupper.Logger;
using System;
using System.IO;

namespace Backupper.Worker
{
    public static partial class BackupWorker
    {
        /// <summary>
        /// Выполняет копирование файла при помощи FileStream и BinaryReader/Writer.
        /// Если LogLevel логгера содержит флаг Percents, выводится отношение скопированности файла в процентах.
        /// </summary>
        /// <param name="logger">Объект типа логгер для ведения процесса копирования</param>
        /// <param name="fileFrom">Путь к файлу, который надо скопировать</param>
        /// <param name="fileTo">Пупть к файлу, в который надо скопировать</param>
        /// <param name="overwriteFiles">Перезаписывать файл, если существует</param>
        /// <returns>true - если копирование пройдено успешно</returns>
        private static bool CopyFile(ILogger logger, string fileFrom, string fileTo, bool overwriteFiles)
        {
            lock (Locker)
            {
                logger.Debug($"Сохранение файла {fileTo}.");

                if (!overwriteFiles && File.Exists(fileTo))
                {
                    logger.Debug($"Файл {fileTo} уже существует.");
                    return true;
                }
                
                bool isOk = true;

                FileStream fileFromStream = null,
                           fileToStream = null;
                BinaryReader reader = null;
                BinaryWriter writer = null;

                try
                {
                    fileFromStream = new FileStream(fileTo, FileMode.Create, FileAccess.Write);
                    fileToStream = new FileStream(fileFrom, FileMode.Open, FileAccess.Read);

                    reader = new BinaryReader(fileToStream);
                    writer = new BinaryWriter(fileFromStream);

                    fileFromStream.SetLength(fileToStream.Length);
                    int bytesRead = -1;
                    double bytesReadSum = 0;

                    while ((bytesRead = reader.Read(Buffer, 0, BufferSize)) > 0)
                    {
                        writer.Write(Buffer, 0, bytesRead);

                        bytesReadSum += bytesRead;
                        logger.Percents($"Копирование {fileFrom} выполнено на {(bytesReadSum / fileToStream.Length):P1}.");
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    logger.Error($"Файл {fileFrom} не удалось скопировать. Отсутствует разрешение.");
                    isOk = false;
                }
                catch (Exception e)
                {
                    logger.Error($"Файл {fileFrom} не удалось скопировать. {e.Message}");
                    isOk = false;
                }
                finally
                {
                    fileFromStream?.Dispose();
                    fileToStream?.Dispose();
                    reader?.Dispose();
                    writer?.Dispose();
                }

                return isOk;
            }
        }
    }
}
