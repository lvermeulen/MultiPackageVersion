using MultiPackageVersion.Core;
using Xunit;

namespace MultiPackageVersion.DirectoryReaders.Git.Tests
{
    public class GitDirectoryReaderShould
    {
        private readonly IDirectoryReader _reader = new GitDirectoryReader(@"C:\Program Files\Git\bin\git.exe");

        [Fact]
        public void GetFiles()
        {
            var results = _reader.GetFiles(@"C:\gitrepo\DependencyTracker");
            Assert.NotEmpty(results);
        }
    }
}
