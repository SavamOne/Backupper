using System;

namespace Backupper.Logger
{
    /// <summary>
    /// Уровни логгирования. 
    /// </summary>
    [Flags]
    public enum LogLevel
    {
        /// <summary>
        /// None - ничего не писать.
        /// </summary>
        None = 0,
        /// <summary>
        /// Info - выводить только основую информацию (начало процесса, окончание процесса, копирование директории).
        /// </summary>
        Info = 1,
        /// <summary>
        /// Error - выводить только ошибки.
        /// </summary>
        Error = 2,
        /// <summary>
        /// Percents - выводить только информацию о скопированности файла.
        /// </summary>
        Percents = 4,
        /// <summary>
        /// Debug - выводить только отладочную информацию - создание файла, создание директории, начало этапа.
        /// </summary>
        Debug = 8,
        /// <summary>
        /// All - выводить все.
        /// </summary>
        All = Percents | Info | Error | Debug,
        /// <summary>
        /// Standart - выводить только Info и Error.
        /// </summary>
        Standart =  Info | Error
    }
}
