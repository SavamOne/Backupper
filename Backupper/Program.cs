using Backupper.Logger;

namespace Backupper
{
    class Program
    {
        static void Main(string[] args)
        {
            ILogger log = new ConsoleLogger(LogLevel.Standart);

            Backupper.DoWork(log, @"D:\c#_projects\Backupper\Backupper", @"C:\Users\anton\Desktop\test_papka", continueOnError: true, overwriteFiles: true);

            System.Console.ReadKey();
        }
    }
}
