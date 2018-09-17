using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Build.Construction;
using Microsoft.Build.Definition;
using Microsoft.Build.Evaluation;
using MultiPackageVersion.Core;

namespace MultiPackageVersion.SolutionReaders.Build
{
    public class BuildSolutionReader : ISolutionReader
    {
        public IEnumerable<string> ReadFileNames(string fileName)
        {
            var results = new List<string>();

            var solutionFile = SolutionFile.Parse(fileName);
            var projects = solutionFile
                .ProjectsInOrder
                .Where(x => x.ProjectType == SolutionProjectType.KnownToBeMSBuildFormat
                    || x.ProjectType == SolutionProjectType.SharedProject
                    || x.ProjectType == SolutionProjectType.WebDeploymentProject
                    || x.ProjectType == SolutionProjectType.WebProject);

            foreach (var projectFile in projects)
            {
                var project = Project.FromFile(projectFile.AbsolutePath, new ProjectOptions { LoadSettings = ProjectLoadSettings.IgnoreEmptyImports | ProjectLoadSettings.IgnoreInvalidImports | ProjectLoadSettings.IgnoreMissingImports });
                if (project == null)
                {
                    continue;
                }

                var items = project.Items
                    .Where(x => x.ItemType == "None" || x.ItemType == "Content" || x.ItemType == "Compile")
                    .Select(x => x.EvaluatedInclude)
                    // ReSharper disable once AssignNullToNotNullAttribute
                    .Select(x => Path.Combine(Path.GetDirectoryName(projectFile.AbsolutePath), x));

                results.AddRange(items);
            }

            return results.Distinct();
        }
    }
}
