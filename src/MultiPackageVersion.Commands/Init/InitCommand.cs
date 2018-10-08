using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MultiPackageVersion.Config;
using MultiPackageVersion.Core;
using Void = MultiPackageVersion.Core.Void;

namespace MultiPackageVersion.Commands.Init
{
    public class InitCommand : ICommand<Void, (bool, InitContext)>
    {
        private readonly InitContext _context;
        private readonly string _gitPath;

        public InitCommand(string gitPath)
        {
            _context = new InitContext { FolderName = Environment.CurrentDirectory };
            _gitPath = gitPath;
        }

        private string FindSolutionFileName(string nuspecFileName)
        {
            IList<string> FindSolutionFileNames(string folderName)
            {
                return Directory.GetFiles(folderName, "*.sln", SearchOption.AllDirectories);
            }

            string nuspecFileNameWithoutExtension = Path.GetFileNameWithoutExtension(nuspecFileName);
            string nuspecFolder = Path.GetDirectoryName(nuspecFileName);
            string lastPartOfNuspecFolder = Path.GetFileName(nuspecFolder);
            var solutionFileNames = FindSolutionFileNames(_context.FolderName);

            // find solution file with same filename
            // ReSharper disable once PossibleNullReferenceException
            string result = solutionFileNames.FirstOrDefault(x => Path.GetFileNameWithoutExtension(x).Equals(nuspecFileNameWithoutExtension, StringComparison.InvariantCultureIgnoreCase));
            if (result != null)
            {
                return result;
            }

            // find solution file containing filename
            // ReSharper disable once AssignNullToNotNullAttribute
            // ReSharper disable once PossibleNullReferenceException
            result = solutionFileNames.FirstOrDefault(x => Path.GetFileNameWithoutExtension(x).Contains(nuspecFileNameWithoutExtension));
            if (result != null)
            {
                return result;
            }

            // find first solution file from nuspec folder
            // ReSharper disable once AssignNullToNotNullAttribute
            // ReSharper disable once PossibleNullReferenceException
            result = FindSolutionFileNames(nuspecFolder).FirstOrDefault();
            if (result != null)
            {
                return result;
            }

            // find solution file from last part of nuspec folder
            // ReSharper disable once PossibleNullReferenceException
            result = solutionFileNames.FirstOrDefault(x => Path.GetFileNameWithoutExtension(x).Equals(lastPartOfNuspecFolder, StringComparison.InvariantCultureIgnoreCase));
            if (result != null)
            {
                return result;
            }

            // find solution file containing last part of nuspec folder
            // ReSharper disable once AssignNullToNotNullAttribute
            // ReSharper disable once PossibleNullReferenceException
            result = solutionFileNames.FirstOrDefault(x => Path.GetFileNameWithoutExtension(x).Contains(lastPartOfNuspecFolder));
            if (result != null)
            {
                return result;
            }

            return solutionFileNames.FirstOrDefault() ?? "";
        }

        private IDictionary<string, VersionConfiguration> FindConfigurations()
        {
            var result = new Dictionary<string, VersionConfiguration>();

            var nuspecFileNames = Directory.GetFiles(_context.FolderName, "*.nuspec", SearchOption.AllDirectories);
            foreach (string nuspecFileName in nuspecFileNames)
            {
                string glob = nuspecFileName.ToGlob(_context.FolderName);
                var versionConfiguration = result.ContainsKey(glob)
                    ? result[glob]
                    : result[glob] = new VersionConfiguration { VersionConfigurationEntries = new List<VersionConfigurationEntry>() };

                versionConfiguration.VersionIncrement = VersionIncrementType.Patch;
                versionConfiguration.VersionConfigurationEntries.Add(new VersionConfigurationEntry
                {
                    NuspecFileName = nuspecFileName,
                    SolutionFileName = FindSolutionFileName(nuspecFileName),
                    BuildDefinitionName = ""
                });
            }

            return result;
        }

        public (bool, InitContext) Execute(Void t = default(Void))
        {
            const string FILENAME = "mpv.config";

            if (File.Exists(FILENAME))
            {
                _context.Message = $"File {FILENAME} already exists.";
                return (false, _context);
            }

            var config = new Configuration { Entries = FindConfigurations() };
            config.Save(FILENAME);

            _context.Message = $"File {FILENAME} was created.";
            return (true, _context);
        }
    }
}
