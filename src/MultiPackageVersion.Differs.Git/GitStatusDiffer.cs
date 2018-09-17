using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MultiPackageVersion.Core;

namespace MultiPackageVersion.Differs.Git
{
    public class GitStatusDiffer : IDiffer
    {
        private static readonly IDictionary<string, DifferenceType> s_differenceTypeByString = new Dictionary<string, DifferenceType>
        {
            ["?"] = DifferenceType.Untracked,
            ["!"] = DifferenceType.Ignored,
            [" "] = DifferenceType.Unmodified,
            ["M"] = DifferenceType.Modified,
            ["A"] = DifferenceType.Added,
            ["D"] = DifferenceType.Deleted,
            ["R"] = DifferenceType.Renamed,
            ["C"] = DifferenceType.Copied,
            ["U"] = DifferenceType.UpdatedButUnmerged
        };

        private readonly string _pathToGit;

        public GitStatusDiffer(string pathToGit)
        {
            _pathToGit = pathToGit;
        }

        private (bool success, string output) GitDiff(string folderName)
        {
            using (new WithCurrentDirectory(folderName))
            {
#pragma warning disable S1075 // URIs should not be hardcoded
                return new Runner(_pathToGit ?? @"C:\Program Files\Git\bin\git.exe", "status --short").Run();
#pragma warning restore S1075 // URIs should not be hardcoded
            }
        }

        public IEnumerable<DifferenceEntry> Diff(string folderName)
        {
            (bool success, string output) = GitDiff(folderName);
            if (!success)
            {
                return Enumerable.Empty<DifferenceEntry>();
            }

            var differences = output
                .Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Where(x => !(x == null || x.Length <= 3))
                .Where(x =>
                {
                    string right = x.Substring(1, 1);
                    return s_differenceTypeByString.ContainsKey(right);
                })
                .Select(x =>
                {
                    string right = x.Substring(1, 1);
                    string fileName = x.Substring(3).Replace("/", @"\");

                    return new DifferenceEntry
                    {
                        FileName = Path.Combine(folderName, fileName),
                        DifferenceType = s_differenceTypeByString[right]
                    };
                });

            return differences;
        }
    }
}
