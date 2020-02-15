using Backupper.Logger;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Backupper
{
    static class Backupper
    {
        private static Stopwatch Watch { get; set; }

        private static object Locker { get; }

        private static int BufferSize { get; }

        private static byte[] Buffer { get; } 

        static Backupper()
        {
            Locker = new object();

            BufferSize = 1024 * 512;
            Buffer = new byte[BufferSize];
        }

        public static void DoWork(ILogger logger, string dirFrom, string dirTo, bool continueOnError = true, bool overwriteFiles = false)
        {
            logger.Status("Процесс копирования начался.");

            if (!CheckDirectories(logger, dirFrom, dirTo))
            {
                logger.Status("Отмена операции.");
                return;
            }

            logger.Status("Копирование директорий и файлов.");

            Watch = new Stopwatch();
            Watch.Start();

            RecursiveСopying(logger, dirFrom, dirTo, continueOnError, overwriteFiles);

            Watch.Stop();

            logger.Status("Процесс копирования завершен.");
            logger.Info($"Время копирования {Watch.Elapsed.Minutes:00}:{Watch.Elapsed.Seconds:00}");
        }

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

            return true;
        }

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
            catch(DirectoryNotFoundException)
            {
                logger.Error($"Директорию {dir} не удалось создать. Недопустимый путь.");
            }
            catch (Exception e)
            {
                logger.Error($"Директорию {dir} не удалось создать. Необработанное исключение. {e.Message}");
            }
            return false;
        }

        private static bool CopyFile(ILogger logger, string fileFrom, string fileTo, bool overwriteFiles)
        {
            lock(Locker)
            {
                logger.Debug($"Сохранение файла {fileTo}.");

                if (!overwriteFiles && File.Exists(fileTo))
                {
                    logger.Info($"Файл {fileTo} уже существует.");
                    return true;
                }

                FileStream fileFromStream = null, 
                           fileToStream = null;
                BinaryReader reader = null;
                BinaryWriter writer = null;

                try
                {
                    fileFromStream = new FileStream(fileTo, FileMode.Create, FileAccess.Write);
                    fileToStream   = new FileStream(fileFrom, FileMode.Open, FileAccess.Read);

                    reader = new BinaryReader(fileToStream);
                    writer = new BinaryWriter(fileFromStream);

                    fileFromStream.SetLength(fileToStream.Length);
                    int bytesRead = -1;
                    double bytesReadSum = 0;

                    while ((bytesRead = reader.Read(Buffer, 0, BufferSize)) > 0)
                    {
                        writer.Write(Buffer, 0, bytesRead);

                        bytesReadSum += bytesRead;
                        logger.Info($"Копирование {fileFrom} выполнено на {(bytesReadSum / fileToStream.Length):P1}.");
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    logger.Error($"Файл {fileFrom} не удалось скопировать. Отсутствует разрешение.");
                }
                catch (Exception e)
                {
                    logger.Error($"Файл {fileFrom} не удалось скопировать. {e.Message}");
                }
                finally
                {
                    fileFromStream?.Dispose();
                    fileToStream?.Dispose();
                    reader?.Dispose();
                    writer?.Dispose();

                    logger.Debug($"Созданные потоки закрыты");
                }

                return true;
            }
        }

        private static bool RecursiveСopying(ILogger logger, string dirFrom, string dirTo, bool continueOnError, bool overwriteFiles)
        {
            string[] subDirsFrom, filesFrom;
            bool dirCreated, subDirCreated, fileCopied;
            string subDirTo, fileTo;

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

                subDirCreated = RecursiveСopying(logger, subDirFrom, subDirTo, continueOnError, overwriteFiles);
                if (!subDirCreated && !continueOnError)
                    return false;
            }
            return true;
        }
    }
}
