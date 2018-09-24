using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MultiPackageVersion.Commands.Build;
using MultiPackageVersion.Config;
using MultiPackageVersion.Core;
using MultiPackageVersion.Dependencies.NuGet;
using Version = SemVer.Version;

namespace MultiPackageVersion.Commands.Run
{
    public class RunCommand : ICommand<IConfiguration, (bool, RunContext)>
    {
        private readonly ISolutionReader _solutionReader;
        private readonly IDiffer _differ;
        private readonly RunContext _context;

        public RunCommand(ISolutionReader solutionReader, IDiffer differ)
        {
            _solutionReader = solutionReader;
            _differ = differ;
            _context = new RunContext { FolderName = Environment.CurrentDirectory };
        }

        private IDictionary<string, VersionIncrementType> GetNuspecVersionIncrementMap(IConfiguration configuration)
        {
            var result = configuration
                .Entries
                .Values
                .Select(x => new
                {
                    NuSpecFileName = x.VersionConfigurationEntries.Select(entry => entry.NuspecFileName).FirstOrDefault(),
                    x.VersionIncrement
                })
                .ToDictionary(x => x.NuSpecFileName, x => x.VersionIncrement);

            return result;
        }

        private string FindNuspecFileName(string consumingFileName)
        {
            string folderName = Path.GetDirectoryName(consumingFileName);
            string pathRoot;
            do
            {
                pathRoot = Path.GetPathRoot(folderName);
                // ReSharper disable once AssignNullToNotNullAttribute
                string result = Directory.GetFiles(folderName, "*.nuspec").FirstOrDefault();
                if (result != null)
                {
                    return result;
                }

                folderName = new DirectoryInfo(folderName).Parent?.FullName;
            } while (folderName != null && pathRoot != _context.FolderName);

            return null;
        }

        private IList<KeyValuePair<string, IEnumerable<Dependency>>> FindConsumingFilesWithDependencies()
        {
            var nugetReader = new NuGetReader(_context.FolderName);
            var fileDependencies = nugetReader.Read();
            return fileDependencies
                .Select(x => new KeyValuePair<string, IEnumerable<Dependency>>(x.FileName, x.Dependencies))
                .ToList();
        }

        private IList<string> FindConsumingFilesUsing(string packageId, IEnumerable<KeyValuePair<string, IEnumerable<Dependency>>> allConsumingFiles)
        {
            return allConsumingFiles
                .Where(x => x.Value.Any(dependency => dependency.Id.Equals(packageId, StringComparison.OrdinalIgnoreCase)))
                .Select(x => x.Key)
                .ToList();
        }

        private List<string> GetConsumingFiles(IEnumerable<string> nuspecFileNames, IEnumerable<KeyValuePair<string, IEnumerable<Dependency>>> allConsumingFiles)
        {
            var nuspecFileNamesList = nuspecFileNames.ToList();
            var consumingFiles = new List<string>();
            var results = new List<string>();
            do
            {
                nuspecFileNamesList
                    .ForEach(x =>
                    {
                        results.Add(x);
                        var nuspecFile = new NuspecFile(x);
                        consumingFiles = FindConsumingFilesUsing(nuspecFile.Id, allConsumingFiles)
                            .Select(FindNuspecFileName)
                            .ToList();

                        results.AddRange(consumingFiles);
                    });
            } while (consumingFiles.Any());

            return results
                .Distinct()
                .ToList();
        }

        private IEnumerable<KeyValuePair<string, string>> UpdateConsumingFiles(IEnumerable<string> nuspecFileNames, IEnumerable<KeyValuePair<string, IEnumerable<Dependency>>> allConsumingFiles, VersionIncrementType defaultVersionIncrement)
        {
            var dependencies = GetConsumingFiles(nuspecFileNames, allConsumingFiles);

            var results = new List<KeyValuePair<string, string>>();
            var nuGetWriter = new NuGetWriter();
            dependencies
                .ForEach(x =>
                {
                    // update nuspec file
                    var nuspecFile = new NuspecFile(x);
                    var versionIncrement = _context.NuspecVersionIncrementMap.ContainsKey(x)
                        ? _context.NuspecVersionIncrementMap[x]
                        : defaultVersionIncrement;
                    string newVersion = new Version(nuspecFile.Version, true)
                        .Increment(versionIncrement)
                        .ToString();
                    nuGetWriter.Write(x, nuspecFile.Id, newVersion);
                    results.Add(new KeyValuePair<string, string>(x, newVersion));
                });

            return results;
        }

        public (bool, RunContext) Execute(IConfiguration t = default(Configuration))
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

            // run build command
            var buildCommand = new BuildCommand(_solutionReader, _differ, _context);
            (bool success, var buildContext) = buildCommand.Execute(_context.Configuration);
            if (!success)
            {
                _context.Message = "Build command failed";
                return (false, _context);
            }

            // find dependencies
            var affectedNuspecFiles = buildContext.VersionConfigurationEntries
                .Select(x => x.NuspecFileName)
                .ToList();

            _context.NuspecVersionIncrementMap = GetNuspecVersionIncrementMap(t);
            var allConsumingFiles = FindConsumingFilesWithDependencies();

            // get all consumers using each nuspecFile
            var consumers = new List<string>();
            foreach (string nuspecFileName in affectedNuspecFiles)
            {
                consumers.Add(nuspecFileName);
                var nuspecFile = new NuspecFile(nuspecFileName);
                string nuspecPackageId = nuspecFile.Id;
                var consumingFiles = FindConsumingFilesUsing(nuspecPackageId, allConsumingFiles);

                // get all nuspec files for each consuming file
                var nuspecFileNames = consumingFiles.Select(FindNuspecFileName);
                consumers.AddRange(nuspecFileNames);
            }

            consumers = consumers
                .Where(x => x != null)
                .Distinct()
                .ToList();

            // update all nuspec files
            var updatedFiles = UpdateConsumingFiles(consumers, allConsumingFiles, t.DefaultVersionIncrement);

            return (true, new RunContext
            {
                UpdatedFiles = updatedFiles
            });
        }
    }
}
