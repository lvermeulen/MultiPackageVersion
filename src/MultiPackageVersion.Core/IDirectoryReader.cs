using System.Collections.Generic;

namespace MultiPackageVersion.Core
{
    public interface IDirectoryReader
    {
        IEnumerable<string> GetFiles(string folderName);
    }
}
