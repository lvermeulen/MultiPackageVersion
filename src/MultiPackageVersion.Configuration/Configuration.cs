using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace MultiPackageVersion.Config
{
    public class Configuration
    {
        public static Configuration Load(string fileName)
        {
#pragma warning disable SCS0018 // Path traversal: injection possible in {1} argument passed to '{0}'
            string json = File.ReadAllText(fileName);
#pragma warning restore SCS0018 // Path traversal: injection possible in {1} argument passed to '{0}'
            return Parse(json);
        }

        public static Configuration Parse(string json)
        {
            return JsonConvert.DeserializeObject<Configuration>(json);
        }

        public static void Save(Configuration configuration, string fileName)
        {
            configuration.Save(fileName);
        }

        [JsonConverter(typeof(VersionIncrementTypeConverter))]
        public VersionIncrementType DefaultVersionIncrement { get; set; }
        public IDictionary<string, VersionConfiguration> Entries { get; set; }

        public void Save(string fileName)
        {
            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            if (fileName != null)
            {
#pragma warning disable SCS0018 // Path traversal: injection possible in {1} argument passed to '{0}'
                File.WriteAllText(fileName, json);
#pragma warning restore SCS0018 // Path traversal: injection possible in {1} argument passed to '{0}'
            }
        }
    }
}