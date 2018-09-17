using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using MultiPackageVersion.Core;

namespace MultiPackageVersion.Dependencies.NuGet
{
    public class NuGetReader
    {
        private readonly IEnumerable<FileInfo> _fileInfos;

        public NuGetReader(string path)
        {
            _fileInfos = new DirectoryInfo(path).EnumerateFiles("*.??proj", SearchOption.AllDirectories);
        }

        public IEnumerable<FileDependencies> Read()
        {
            var results = new List<FileDependencies>();

            foreach (var fileInfo in _fileInfos.Where(x => !x.Extension.EndsWith("vdproj", StringComparison.OrdinalIgnoreCase)))
            {
                string projectName = GetProjectName(fileInfo.FullName);
                if (projectName == null)
                {
                    return Enumerable.Empty<FileDependencies>();
                }

                results.Add(GetProjectDependencies(projectName, fileInfo.Directory));
            }

            return results.Where(x => x!= null);
        }

        private string GetProjectName(string fileName)
        {
            var doc = XDocument.Load(fileName);
            var xmlns = doc.Root?.Name.Namespace;
            string assemblyName = doc
                .Element(xmlns + "Project")
                ?.Elements(xmlns + "PropertyGroup")
                .Elements(xmlns + "AssemblyName")
                .FirstOrDefault(x => x != null)
                ?.Value;

            return assemblyName ?? Path.GetFileNameWithoutExtension(fileName);
        }

        private FileDependencies GetPackagesConfigDependencies(string projectName, DirectoryInfo directory)
        {
            string fileName = directory
                .EnumerateFiles("packages.config", SearchOption.TopDirectoryOnly)
                .FirstOrDefault()
                ?.FullName;
            if (fileName == null)
            {
                return null;
            }

            var doc = XDocument.Load(fileName);
            return new FileDependencies
            {
                FileName = fileName,
                Dependencies = doc
                    .Element("packages")
                    ?.Elements("package")
                    .Select(x => new Dependency
                    {
                        ProjectName = projectName,
                        Id = x?.Attribute("id")?.Value,
                        Version = x?.Attribute("version")?.Value
                    })
            };
        }

        private FileDependencies GetProjectDependencies(string projectName, DirectoryInfo directory)
        {
            string fileName = directory
                .EnumerateFileSystemInfos("*.??proj", SearchOption.TopDirectoryOnly)
                .FirstOrDefault(x => !x.Extension.EndsWith("vdproj", StringComparison.OrdinalIgnoreCase))
                ?.FullName;
            if (fileName == null)
            {
                return null;
            }

            var doc = XDocument.Load(fileName);
            var result = new FileDependencies
            {
                FileName = fileName,
                Dependencies = doc
                    .Element("Project")
                    ?.Elements("ItemGroup")
                    .Elements("PackageReference")
                    .Select(x => new Dependency
                    {
                        ProjectName = projectName,
                        Id = x?.Attribute("Include")?.Value,
                        Version = x?.Attribute("Version")?.Value
                    })
            };

            return result.Dependencies != null
                ? result
                : GetPackagesConfigDependencies(projectName, directory);
        }
    }
}
