using System.Collections.Generic;

namespace MultiPackageVersion.Core
{
    public class FileDependencies
    {
        public string Id { get; set; }
        public string FileName { get; set; }
        public IEnumerable<Dependency> Dependencies { get; set; }
    }
}
