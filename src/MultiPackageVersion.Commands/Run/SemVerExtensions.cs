using MultiPackageVersion.Core;
using SemVer;

namespace MultiPackageVersion.Commands.Run
{
    public static class SemVerExtensions
    {
        public static Version Increment(this Version semVer, VersionIncrementType versionIncrement)
        {
            switch (versionIncrement)
            {
                case VersionIncrementType.Major: return new Version(semVer.Major + 1, semVer.Minor, semVer.Patch);
                case VersionIncrementType.Minor: return new Version(semVer.Major, semVer.Minor + 1, semVer.Patch);
                default: return new Version(semVer.Major, semVer.Minor, semVer.Patch + 1);
            }
        }
    }
}
