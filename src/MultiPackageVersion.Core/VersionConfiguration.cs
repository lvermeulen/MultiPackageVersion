using System.Collections.Generic;
using Newtonsoft.Json;

namespace MultiPackageVersion.Core
{
    public class VersionConfiguration
    {
        [JsonConverter(typeof(VersionIncrementTypeConverter))]
        public VersionIncrementType VersionIncrement { get; set; }
        public List<VersionConfigurationEntry> VersionConfigurationEntries { get; set; }
    }
}
