using System.Collections.Generic;
using MultiPackageVersion.Core;

namespace MultiPackageVersion.Commands.Run
{
    public class RunContext : IContext
    {
        public string FolderName { get; set; }
        public IConfiguration Configuration { get; set; }
        public string Message { get; set; }
        public IDictionary<string, VersionIncrementType> NuspecVersionIncrementMap { get; set; }
        public IEnumerable<KeyValuePair<string, string>> UpdatedFiles { get; set; }
    }
}
