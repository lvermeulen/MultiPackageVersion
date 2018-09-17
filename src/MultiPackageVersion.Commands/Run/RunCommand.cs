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
    public class RunCommand : ICommand<Configuration, (bool, IEnumerable<string>)>
    {
        private readonly ISolutionReader _solutionReader;
        private readonly IDiffer _differ;
        private readonly IOutputFormatter<KeyValuePair<string, string>, KeyValuePair<string, string>> _formatter;

        public RunCommand(ISolutionReader solutionReader, IDiffer differ, IOutputFormatter<KeyValuePair<string, string>, KeyValuePair<string, string>> formatter)
        {
            _solutionReader = solutionReader;
            _differ = differ;
            _formatter = formatter;
        }

        private IEnumerable<string> GetAffectedNuspecFiles(IEnumerable<KeyValuePair<string, string>> buildDefinitionNames) => 
            buildDefinitionNames.Select(x => x.Value);

        private IDictionary<string, VersionIncrementType> GetNuspecVersionIncrementMap(Configuration configuration)
        {
            var result = configuration
                .Entries
                .Values
                //.Where(x => nuspecFiles.Any(nuSpecFileName => x.VersionConfigurationEntries.Any(entry => entry.NuspecFileName.Equals(nuSpecFileName, StringComparison.OrdinalIgnoreCase))))
                .Select(x => new
                {
                    NuSpecFileName = x.VersionConfigurationEntries.Select(entry => entry.NuspecFileName).FirstOrDefault(),
                    x.VersionIncrement
                })
                .ToDictionary(x => x.NuSpecFileName, x => x.VersionIncrement);

            return result;
        }

        private string FindNuspecFileName(string consumingFileName, string rootFolderName)
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
            } while (folderName != null && pathRoot != rootFolderName);

            return null;
        }

        private IEnumerable<KeyValuePair<string, IEnumerable<Dependency>>> FindConsumingFiles(string rootFolderName)
        {
            var nugetReader = new NuGetReader(rootFolderName);
            var fileDependencies = nugetReader.Read();
            return fileDependencies.Select(x => new KeyValuePair<string, IEnumerable<Dependency>>(x.FileName, x.Dependencies));
        }

        private IEnumerable<string> FindConsumingFilesUsing(string packageId, IEnumerable<KeyValuePair<string, IEnumerable<Dependency>>> allConsumingFiles)
        {
            return allConsumingFiles
                .Where(x => x.Value.Any(dependency => dependency.Id.Equals(packageId, StringComparison.OrdinalIgnoreCase)))
                .Select(x => x.Key);
        }

        private IEnumerable<string> GetConsumingFiles(IEnumerable<string> nuspecFileNames , IEnumerable<KeyValuePair<string, IEnumerable<Dependency>>> allConsumingFiles, string folderName)
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
                            .Select(consumingFile => FindNuspecFileName(consumingFile, folderName))
                            .ToList();

                        results.AddRange(consumingFiles);
                    });
            } while (consumingFiles.Any());

            return results
                .Distinct();
        }

        private IEnumerable<KeyValuePair<string, string>> UpdateConsumingFiles(IEnumerable<string> nuspecFileNames, IDictionary<string, VersionIncrementType> nuspecVersionIncrementMap, IEnumerable<KeyValuePair<string, IEnumerable<Dependency>>> allConsumingFiles, string folderName, VersionIncrementType defaultVersionIncrement)
        {
            var dependencies = GetConsumingFiles(nuspecFileNames.ToList(), allConsumingFiles.ToList(), folderName);

            var results = new List<KeyValuePair<string, string>>();
            var nuGetWriter = new NuGetWriter();
            dependencies
                .ToList()
                .ForEach(x =>
                {
                    // update nuspec file
                    var nuspecFile = new NuspecFile(x);
                    var versionIncrement = nuspecVersionIncrementMap.ContainsKey(x)
                        ? nuspecVersionIncrementMap[x]
                        : defaultVersionIncrement;
                    string newVersion = new Version(nuspecFile.Version, true)
                        .Increment(versionIncrement)
                        .ToString();
                    nuGetWriter.Write(x, nuspecFile.Id, newVersion);
                    results.Add(new KeyValuePair<string, string>(x, newVersion));
                });

            return results;
        }

        public (bool, IEnumerable<string>) Execute(Configuration t = default(Configuration))
        {
            if (t == default(Configuration))
            {
                throw new ArgumentNullException(nameof(t));
            }

            if (!t.Entries.Any())
            {
                return (false, Enumerable.Repeat("No configuration entries", 1));
            }

            // run build command
            var buildCommand = new BuildCommand(_solutionReader, _differ, _formatter);
            (bool success, var buildOutputFiles) = buildCommand.Execute(t);
            if (!success)
            {
                return (false, Enumerable.Repeat("Build command failed", 1));
            }

            string folderName = Environment.CurrentDirectory;
            var allConsumingFiles = FindConsumingFiles(folderName)
                .ToList();

            // find dependencies
            var nuspecFiles = GetAffectedNuspecFiles(buildOutputFiles)
                .ToList();
            var nuspecVersionIncrementMap = GetNuspecVersionIncrementMap(t);

            // get all consumers using each nuspecFile
            var results = new List<string>();
            foreach (string nuspecFileName in nuspecFiles)
            {
                results.Add(nuspecFileName);
                var nuspecFile = new NuspecFile(nuspecFileName);
                string nuspecPackageId = nuspecFile.Id;
                var consumingFiles = FindConsumingFilesUsing(nuspecPackageId, allConsumingFiles);

                // get all nuspec files for each consuming file
                var nuspecFileNames = consumingFiles.Select(consumingFile => FindNuspecFileName(consumingFile, folderName));
                results.AddRange(nuspecFileNames);
            }

            results = results
                .Where(x => x != null)
                .Distinct()
                .ToList();

            // update all nuspec files
            var updatedFiles = UpdateConsumingFiles(results, nuspecVersionIncrementMap, allConsumingFiles, folderName, t.DefaultVersionIncrement);

            return (true, updatedFiles.Select(x => $"{x.Key}\t{x.Value}"));
        }
    }
}
