using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace MultiPackageVersion.SolutionReaders.Native
{
    public class Project
    {
        public string ProjectName { get; set; }
        public string AbsolutePath { get; set; }
        public Guid Guid { get; set; }
        public IEnumerable<ProjectItem> Items { get; private set; }

        public static Project FromFile(string fileName)
        {
            if (!File.Exists(fileName))
            {
                return null;
            }

            var xmldoc = XDocument.Load(fileName);
            XNamespace msbuild = "http://schemas.microsoft.com/developer/msbuild/2003";

            var items = xmldoc
                .Elements(msbuild + "Project")
                .Elements(msbuild + "ItemGroup")
                .Elements()
                .Select(x => new
                {
                    ItemType = x.Name.LocalName,
                    Include = x.Attributes("Include").FirstOrDefault()?.Value
                });

            return new Project
            {
                Items = new List<ProjectItem>(items.Select(x => new ProjectItem
                {
                    ItemType = x.ItemType,
                    Include = x.Include
                }))
            };
        }
    }
}