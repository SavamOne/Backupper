using System;

namespace Backupper.Logger
{
    [Flags]
    public enum LogLevel
    {
        None = 0,
        Info = 1,
        Error = 2,
        Percents = 4,
        Debug = 8,
        All = Percents | Info | Error | Debug,
        Standart =  Info | Error,
    }
}
