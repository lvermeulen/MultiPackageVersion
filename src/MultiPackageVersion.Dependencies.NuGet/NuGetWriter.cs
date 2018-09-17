using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace MultiPackageVersion.Dependencies.NuGet
{
    public class NuGetWriter
    {
        private static readonly string[] s_schemaVersions =
        {
            "http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd",
            "http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd",
            "http://schemas.microsoft.com/packaging/2011/10/nuspec.xsd",
            "http://schemas.microsoft.com/packaging/2012/06/nuspec.xsd",
            "http://schemas.microsoft.com/packaging/2013/01/nuspec.xsd",
            "http://schemas.microsoft.com/packaging/2013/05/nuspec.xsd"
        };

        private void WriteNuspecInfo(string fileName, string packageId, string version)
        {
            var doc = XDocument.Load(fileName);
            foreach (string schemaVersion in s_schemaVersions)
            {
                XNamespace ns = schemaVersion;
                string id = doc.Descendants(ns + "id").FirstOrDefault()?.Value;
                if (id == null)
                {
                    continue;
                }

                if (!id.Equals(packageId, StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }

                doc.Descendants(ns + "version")
                    .FirstOrDefault()
                    ?.SetValue(version);

                break;
            }

            doc.Save(fileName);
        }

        private void WritePackagesConfigInfo(string fileName, string packageId, string version)
        {
            var doc = XDocument.Load(fileName);
            doc.Element("packages")
                ?.Elements("package")
                .FirstOrDefault(x => x.Attributes("id").Any(attribute => attribute.Value.Equals(packageId, StringComparison.OrdinalIgnoreCase)))
                ?.Attributes("version")
                .FirstOrDefault()
                ?.SetValue(version);

            doc.Save(fileName);
        }

        private void WriteProjectInfo(string fileName, string packageId, string version)
        {
            var doc = XDocument.Load(fileName);
            doc.Element("Project")
                ?.Elements("ItemGroup")
                .Elements("PackageReference")
                .FirstOrDefault(x => x.Attributes("Include").Any(attribute => attribute.Value.Equals(packageId, StringComparison.OrdinalIgnoreCase)))
                ?.Attributes("Version")
                .FirstOrDefault()
                ?.SetValue(version);

            doc.Save(fileName);
        }

        public void Write(string fileName, string packageId, string version)
        {
            var fileInfo = new FileInfo(fileName);

            if (fileInfo.Extension.EndsWith("nuspec", StringComparison.OrdinalIgnoreCase))
            {
                WriteNuspecInfo(fileName, packageId, version);
            }
            else if (fileInfo.Name.Equals("packages.config", StringComparison.OrdinalIgnoreCase))
            {
                WritePackagesConfigInfo(fileName, packageId, version);
            }
            else if (fileInfo.Extension.EndsWith("proj", StringComparison.OrdinalIgnoreCase))
            {
                WriteProjectInfo(fileName, packageId, version);
            }
            else
            {
                throw new InvalidOperationException($"Unknown file to write dependencies: {fileName}");
            }
        }
    }
}
