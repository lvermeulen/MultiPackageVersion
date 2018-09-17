using MultiPackageVersion.Core;
using MultiPackageVersion.DirectoryReaders.Git;
using Xunit;

namespace MultiPackageVersion.Hashers.Sha512.Tests
{
    public class Sha512HasherShould
    {
        private readonly IHasher _hasher = new Sha512Hasher(new GitDirectoryReader(@"C:\Program Files\Git\bin\git.exe"));

        [Theory]
        [InlineData(@"C:\bbrepo\Framework\dbinfogenerator", "3738090e90f0ddca3fbc535a7131edb52eda7eb280236f4540fb3621ee40a5e981cfb727c8382393a00675509b45879304ba69184ceb55174feca3dde07ec702")]
        public void Hash(string folderName, string expectedHash)
        {
            string hash = _hasher.Hash(folderName);
            Assert.Equal(expectedHash, hash);
        }
    }
}
