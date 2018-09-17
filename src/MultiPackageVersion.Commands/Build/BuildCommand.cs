using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Glob;
using MultiPackageVersion.Config;
using MultiPackageVersion.Core;

namespace MultiPackageVersion.Commands.Build
{
    public class BuildCommand : ICommand<Configuration, (bool, IEnumerable<KeyValuePair<string, string>>)>
    {
        private readonly ISolutionReader _solutionReader;
        private readonly IDiffer _differ;
        private readonly IOutputFormatter<KeyValuePair<string, string>, KeyValuePair<string, string>> _formatter;

        public BuildCommand(ISolutionReader solutionReader, IDiffer differ, IOutputFormatter<KeyValuePair<string, string>, KeyValuePair<string, string>> formatter)
        {
            _solutionReader = solutionReader;
            _differ = differ;
            _formatter = formatter;
        }

        public (bool, IEnumerable<KeyValuePair<string, string>>) Execute(Configuration t = default(Configuration))
        {
            if (t == default(Configuration))
            {
                throw new ArgumentNullException(nameof(t));
            }

            if (!t.Entries.Any())
            {
                return (false, Enumerable.Repeat(new KeyValuePair<string, string>("", ""), 1));
            }

            var results = new List<KeyValuePair<string, string>>();
            string folderName = Environment.CurrentDirectory;

            // find file differences
            var differences = _differ.Diff(folderName)
                .Select(x => x.FileName)
                .ToList();

            foreach (string globSpec in t.Entries.Keys)
            {
                // localize differences to current glob
                var globFolders = new DirectoryInfo(folderName)
                    .GlobDirectories(globSpec)
                    .Select(x => x.FullName)
                    .ToList();
                var localDifferences = differences
                    .WhereSubPathOfAnyFolderName(globFolders.ToArray())
                    .ToList();
                if (!localDifferences.Any())
                {
                    continue;
                }

                // inspect nuspec level
                var versionConfiguration = t.Entries[globSpec];
                foreach (var versionConfigurationEntry in versionConfiguration.VersionConfigurationEntries)
                {
                    // read solution file
                    var filesInSolution = _solutionReader.ReadFileNames(Path.Combine(folderName, versionConfigurationEntry.SolutionFileName));
                    if (filesInSolution.Intersect(localDifferences).Any())
                    {
                        results.Add(new KeyValuePair<string, string>(versionConfigurationEntry.BuildDefinitionName, versionConfigurationEntry.NuspecFileName));
                    }
                }
            }

            results = results
                .Distinct()
                .ToList();

            return (true, results);
        }
    }
}
