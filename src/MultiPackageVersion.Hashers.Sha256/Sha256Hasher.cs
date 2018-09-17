using System.Security.Cryptography;
using MultiPackageVersion.Core;

namespace MultiPackageVersion.Hashers.Sha256
{
    public class Sha256Hasher : HasherBase
    {
        public Sha256Hasher(IDirectoryReader directoryReader) 
            : base(directoryReader)
        { }

        protected override HashAlgorithm HashAlgorithm { get; } = SHA256.Create();
    }
}
