using MultiPackageVersion.Core;
using MultiPackageVersion.DirectoryReaders.Git;
using Xunit;

namespace MultiPackageVersion.Hashers.Sha256.Tests
{
    public class Sha256HasherShould
    {
        private readonly IHasher _hasher = new Sha256Hasher(new GitDirectoryReader(@"C:\Program Files\Git\bin\git.exe"));

        [Theory]
        [InlineData(@"C:\bbrepo\Framework\dbinfogenerator", "ecc6d4eace129cd9e3a9899eab88b71dd4e699f1a6b587955d7cba9bfcbbf091")]
        public void Hash(string folderName, string expectedHash)
        {
            string hash = _hasher.Hash(folderName);
            Assert.Equal(expectedHash, hash);
        }
    }
}
