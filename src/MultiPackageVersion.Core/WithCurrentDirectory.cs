using System;

namespace MultiPackageVersion.Core
{
    public sealed class WithCurrentDirectory : IDisposable
    {
        private readonly string _currentDirectory;

        public WithCurrentDirectory(string folderName)
        {
            _currentDirectory = Environment.CurrentDirectory;
            Environment.CurrentDirectory = folderName;
        }

        public void Dispose()
        {
            Environment.CurrentDirectory = _currentDirectory;
        }
    }
}
