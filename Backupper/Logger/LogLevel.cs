using System;

namespace Backupper.Logger
{
    [Flags]
    enum LogLevel
    {
        None = 0,
        Status = 1,
        Info = 2,
        Error = 4,
        Debug = 8,
        All = Status | Info | Error | Debug,
        Standart = Status | Info | Error,
    }
}
