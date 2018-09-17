using System.Collections.Generic;

namespace MultiPackageVersion.Core
{
    public interface ISolutionReader
    {
        IEnumerable<string> ReadFileNames(string fileName);
    }
}
