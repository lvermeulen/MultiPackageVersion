using MultiPackageVersion.Commands.Init;
using Xunit;

namespace MultiPackageVersion.Commands.Tests
{
    public class StringExtensionsShould
    {
        [Theory]
        [InlineData(@"\\hello\hello.txt", null)]
        [InlineData(@"\\hello\1\hello.txt", @"\")]
        [InlineData(@"\\hello\1\2\3\hello.txt", @"\2\3")]
        [InlineData(@"\hello.txt", @"\")]
        [InlineData(@"C:\hello\1\2\3\hello.txt", @"\hello\1\2\3")]
        public void GetDirectoryNameWithoutDriveOrNetworkShare(string fileName, string expectedResult)
        {
            string result = fileName.GetDirectoryNameWithoutDriveOrNetworkShare();
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(@"\\hello\hello.txt", null)]
        [InlineData(@"\\hello\1\hello.txt", @"**\*")]
        [InlineData(@"\\hello\1\2\3\hello.txt", @"**\2\3\*")]
        [InlineData(@"\hello.txt", @"**\*")]
        [InlineData(@"C:\hello\1\2\3\hello.txt", @"**\hello\1\2\3\*")]
        public void ToGlob(string fileName, string expectedResult)
        {
            string result = fileName.ToGlob();
            Assert.Equal(expectedResult, result);
        }
    }
}
