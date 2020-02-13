﻿namespace Backupper.Logger
{
    interface ILogger
    {
        LogLevel Level { get; set; }

        void Info(string message);

        void Error(string message);

        void Debug(string message);
    }
}