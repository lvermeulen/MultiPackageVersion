using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MultiPackageVersion.Core;

namespace MultiPackageVersion.DirectoryReaders.Git
{
    public class GitDirectoryReader : IDirectoryReader
    {
        private readonly string _pathToGit;

        public GitDirectoryReader(string pathToGit)
        {
            _pathToGit = pathToGit;
        }

        private (bool success, string output) GitLsFiles(string folderName)
        {
            using (new WithCurrentDirectory(folderName))
            {
#pragma warning disable S1075 // URIs should not be hardcoded
                return new Runner(_pathToGit ?? @"C:\Program Files\Git\bin\git.exe", "ls-files").Run();
#pragma warning restore S1075 // URIs should not be hardcoded
            }
        }

        public IEnumerable<string> GetFiles(string folderName)
        {
            (bool success, string output) = GitLsFiles(folderName);
            if (!success)
            {
                return Enumerable.Empty<string>();
            }

            var fileNames = output
                .Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => Path.Combine(folderName, x));

            return new List<string>(fileNames);
        }
    }
}
