using System.Security.Cryptography;
using MultiPackageVersion.Core;

namespace MultiPackageVersion.Hashers.Sha1
{
    public class Sha1Hasher : HasherBase
    {
        public Sha1Hasher(IDirectoryReader directoryReader) 
            : base(directoryReader)
        { }

#pragma warning disable SCS0006 // Weak hashing function
        protected override HashAlgorithm HashAlgorithm { get; } = SHA1.Create();
#pragma warning restore SCS0006 // Weak hashing function
    }
}
