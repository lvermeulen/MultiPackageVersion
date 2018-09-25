using MultiPackageVersion.Commands.Run;
using MultiPackageVersion.Core;
using SemVer;
using Xunit;

namespace MultiPackageVersion.Commands.Tests
{
    public class SemVerExtensionsShould
    {
        [Theory]
        [InlineData("1.1.1", VersionIncrementType.Major, "2.1.1")]
        [InlineData("1.1.1", VersionIncrementType.Minor, "1.2.1")]
        [InlineData("1.1.1", VersionIncrementType.Patch, "1.1.2")]
        public void Increment(string version, VersionIncrementType versionIncrementType, string expectedResult)
        {
            string newVersion = new Version(version, true).Increment(versionIncrementType).ToString();
            Assert.Equal(expectedResult, newVersion);
        }
    }
}
