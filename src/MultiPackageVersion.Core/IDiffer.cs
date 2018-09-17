using System.Collections.Generic;

namespace MultiPackageVersion.Core
{
    public interface IDiffer
    {
        IEnumerable<DifferenceEntry> Diff(string folderName);
    }
}
