namespace MultiPackageVersion.Core
{
    public class NuspecFileDependencies : FileDependencies
    {
        public NuspecFile NuspecFile { get; set; }
        public string PackageId { get; set; }

        public bool HasNuspecFile => NuspecFile != null;
    }
}
