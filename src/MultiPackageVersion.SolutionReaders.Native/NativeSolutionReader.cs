using System.Collections.Generic;
using System.IO;
using System.Linq;
using MultiPackageVersion.Core;

namespace MultiPackageVersion.SolutionReaders.Native
{
    public class NativeSolutionReader : ISolutionReader
    {
        public IEnumerable<string> ReadFileNames(string fileName)
        {
            var results = new List<string>();

            var solutionFile = SolutionFile.Parse(fileName);

            foreach (var projectFile in solutionFile.Projects)
            {
                var project = Project.FromFile(projectFile.AbsolutePath);
                if (project == null)
                {
                    continue;
                }

                var items = project.Items
                    .Where(x => x.ItemType == "None" || x.ItemType == "Content" || x.ItemType == "Compile")
                    .Select(x => x.Include)
                    // ReSharper disable once AssignNullToNotNullAttribute
                    .Select(x => Path.Combine(Path.GetDirectoryName(projectFile.AbsolutePath), x));

                results.AddRange(items);
            }

            return results.Distinct();
        }
    }
}
