using MultiPackageVersion.Core;
using MultiPackageVersion.DirectoryReaders.Git;
using Xunit;

namespace MultiPackageVersion.Hashers.Sha1.Tests
{
    public class Sha1HasherShould
    {
        private readonly IHasher _hasher = new Sha1Hasher(new GitDirectoryReader(@"C:\Program Files\Git\bin\git.exe"));

        [Theory]
        [InlineData(@"C:\bbrepo\Framework\dbinfogenerator", "5c88814e50239fd6fa9563d5b256f6a925558e14")]
        public void Hash(string folderName, string expectedHash)
        {
            string hash = _hasher.Hash(folderName);
            Assert.Equal(expectedHash, hash);
        }
    }
}
