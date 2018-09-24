using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Glob;
using MultiPackageVersion.Config;
using MultiPackageVersion.Core;

namespace MultiPackageVersion.Commands.Build
{
    public class BuildCommand : ICommand<IConfiguration, (bool, BuildContext)>
    {
        private readonly ISolutionReader _solutionReader;
        private readonly IDiffer _differ;
        private readonly BuildContext _context;

        public BuildCommand(ISolutionReader solutionReader, IDiffer differ, IContext context = null)
        {
            _solutionReader = solutionReader;
            _differ = differ;
            _context = context != null
                ? new BuildContext { FolderName = context.FolderName }
                : new BuildContext { FolderName = Environment.CurrentDirectory };
        }

        public (bool, BuildContext) Execute(IConfiguration t = default(Configuration))
        {
            if (t == default(Configuration))
            {
                throw new ArgumentNullException(nameof(t));
            }

            if (!t.Entries.Any())
            {
                _context.Message = "No configuration entries";
                return (false, _context);
            }

            _context.Configuration = t;
            var versionConfigurationEntries = new List<VersionConfigurationEntry>();

            // find file differences
            _context.Differences = _differ.Diff(_context.FolderName)
                .Select(x => x.FileName)
                .ToList();

            foreach (string globSpec in t.Entries.Keys)
            {
                // localize differences to current glob
                var globFolders = new DirectoryInfo(_context.FolderName)
                    .GlobDirectories(globSpec)
                    .Select(x => x.FullName)
                    .ToList();
                var localDifferences = _context.Differences
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
                    var filesInSolution = _solutionReader.ReadFileNames(Path.Combine(_context.FolderName, versionConfigurationEntry.SolutionFileName));
                    if (filesInSolution.Intersect(localDifferences).Any())
                    {
                        versionConfigurationEntries.Add(versionConfigurationEntry);
                    }
                }
            }

            _context.VersionConfigurationEntries = versionConfigurationEntries
                .Distinct()
                .ToList();

            return (true, _context);
        }
    }
}
