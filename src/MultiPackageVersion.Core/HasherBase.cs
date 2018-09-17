using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MultiPackageVersion.Core
{
    public abstract class HasherBase : IHasher
    {
        private readonly IDirectoryReader _directoryReader;

        protected abstract HashAlgorithm HashAlgorithm { get; }

        protected HasherBase(IDirectoryReader directoryReader)
        {
            _directoryReader = directoryReader;
        }

        public string Hash(string fileOrFolderName)
        {
            var files = _directoryReader
                .GetFiles(fileOrFolderName)
                .ToList();

            for (int i = 0; i < files.Count; i++)
            {
                string file = files[i];

                // hash path
                string relativePath = file.Substring(fileOrFolderName.Length + 1);
                var pathBytes = Encoding.UTF8.GetBytes(relativePath.ToLower());
                HashAlgorithm.TransformBlock(pathBytes, 0, pathBytes.Length, pathBytes, 0);

                // hash contents
                try
                {
                    var contentBytes = File.ReadAllBytes(file);
                    if (i == files.Count - 1)
                    {
                        HashAlgorithm.TransformFinalBlock(contentBytes, 0, contentBytes.Length);
                    }
                    else
                    {
                        HashAlgorithm.TransformBlock(contentBytes, 0, contentBytes.Length, contentBytes, 0);
                    }
                }
                catch
                {
                    // can't open file: continue
                }
            }

            return BitConverter
                .ToString(HashAlgorithm.Hash)
                .Replace("-", "")
                .ToLower();
        }
    }
}
