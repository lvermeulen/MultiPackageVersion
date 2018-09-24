using System.Collections.Generic;

namespace MultiPackageVersion.Core
{
    public interface IConfiguration
    {
        VersionIncrementType DefaultVersionIncrement { get; }
        IDictionary<string, VersionConfiguration> Entries { get; }
    }
}
