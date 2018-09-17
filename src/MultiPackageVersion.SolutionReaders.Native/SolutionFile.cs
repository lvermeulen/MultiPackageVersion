using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MultiPackageVersion.SolutionReaders.Native
{
    public class SolutionFile
    {
        public IEnumerable<Project> Projects { get; private set; }

        public static SolutionFile Parse(string fileName)
        {
            string solutionFolder = Path.GetDirectoryName(fileName);
            var projects = new List<Project>();

#pragma warning disable SCS0018 // Path traversal: injection possible in {1} argument passed to '{0}'
            var lines = File.ReadAllLines(fileName);
#pragma warning restore SCS0018 // Path traversal: injection possible in {1} argument passed to '{0}'
            foreach (string line in lines)
            {
                if (!line.Trim().StartsWith("Project", StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }

                var pieces = line.Split(new[] { "=" }, 2, StringSplitOptions.None);
                string secondPiece = pieces.Skip(1).SingleOrDefault();
                pieces = secondPiece?.Split(new[] { "," }, StringSplitOptions.None).Select(x => x.Trim().Trim('"')).ToArray();
                if (pieces?.Length == 3)
                {
                    projects.Add(new Project
                    {
                        ProjectName = pieces[0],
                        // ReSharper disable once AssignNullToNotNullAttribute
                        AbsolutePath = Path.Combine(solutionFolder, pieces[1]),
                        Guid = Guid.Parse(pieces[2])
                    });
                }
            }

            return new SolutionFile { Projects = projects };
        }
    }
}
