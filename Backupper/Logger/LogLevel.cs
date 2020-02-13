using System;

namespace Backupper.Logger
{
    [Flags]
    enum LogLevel
    {
        None = 0,
        Info = 1,
        Error = 2,
        Debug = 4,
        All = Info | Error | Debug
    }
}
