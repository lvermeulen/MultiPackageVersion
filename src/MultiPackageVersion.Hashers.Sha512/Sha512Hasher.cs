using System.Security.Cryptography;
using MultiPackageVersion.Core;

namespace MultiPackageVersion.Hashers.Sha512
{
    public class Sha512Hasher : HasherBase
    {
        public Sha512Hasher(IDirectoryReader directoryReader) 
            : base(directoryReader)
        { }

        protected override HashAlgorithm HashAlgorithm { get; } = SHA512.Create();
    }
}
