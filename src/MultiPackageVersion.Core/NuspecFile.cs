using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace MultiPackageVersion.Core
{
    public class NuspecFile
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

        public string Id { get; }
        public string Version { get; }
        public string FileName { get; }

        public NuspecFile(string fileName)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            if (!File.Exists(fileName))
            {
                throw new ArgumentException($"File does not exists: {fileName}", nameof(fileName));
            }

            var doc = XDocument.Load(fileName);
            foreach (string schemaVersion in s_schemaVersions)
            {
                XNamespace ns = schemaVersion;
                Id = doc.Descendants(ns + "id").FirstOrDefault()?.Value;
                Version = doc.Descendants(ns + "version").FirstOrDefault()?.Value;
                FileName = fileName;

                if (Id != null && Version != null)
                {
                    return;
                }
            }

            if (Id == null || Version == null)
            {
                Id = null;
                Version = null;
            }
        }
    }
}
