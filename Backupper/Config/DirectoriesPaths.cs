using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backupper.Сonfig
{
    public class DirectoriesPaths
    {
        [JsonProperty("from")]
        public string DirectoryFrom { get; set; }

        [JsonProperty("to")]
        public string DirectoryTo { get; set; }
    }
}
