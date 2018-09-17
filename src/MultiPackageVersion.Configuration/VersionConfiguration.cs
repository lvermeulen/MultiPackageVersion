using System.Collections.Generic;
using Newtonsoft.Json;

namespace MultiPackageVersion.Config
{
    public class VersionConfiguration
    {
        public string Version { get; set; }
        [JsonConverter(typeof(VersionIncrementTypeConverter))]
        public VersionIncrementType VersionIncrement { get; set; }
        public List<VersionConfigurationEntry> VersionConfigurationEntries { get; set; }
    }
}
