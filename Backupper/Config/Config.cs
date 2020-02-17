using Backupper.Logger;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backupper.Сonfig
{
    /// <summary>
    /// Класс для десериализации конфига. 
    /// По умолчанию продолжать после ошибки, файлы не перезаписывать, уровень логирования Standart
    /// </summary>
    public class Config
    {
        [JsonProperty("overwrite_files")]
        public bool OverwriteFiles { get; set; } = false;

        [JsonProperty("continue_on_error")]
        public bool ContinueOnError { get; set; } = true;

        [JsonProperty("directories")]
        public IEnumerable<DirectoriesPaths> Directories { get; set; }

        [JsonProperty("log_level")]
        public LogLevel Level { get; set; } = LogLevel.Standart;
    }
}
