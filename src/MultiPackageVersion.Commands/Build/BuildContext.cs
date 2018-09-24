using System.Collections.Generic;
using MultiPackageVersion.Core;

namespace MultiPackageVersion.Commands.Build
{
    public class BuildContext : IContext
    {
        public string FolderName { get; set; }
        public IConfiguration Configuration { get; set; }
        public string Message { get; set; }
        public IEnumerable<string> Differences { get; set; }
        public IEnumerable<VersionConfigurationEntry> VersionConfigurationEntries { get; set; }
    }
}
