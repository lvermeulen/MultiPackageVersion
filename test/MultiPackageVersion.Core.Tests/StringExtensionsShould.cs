using Xunit;

namespace MultiPackageVersion.Core.Tests
{
    public class StringExtensionsShould
    {
        [Theory]
        [InlineData(@"c:\foo", @"c:", true)]
        [InlineData(@"c:\foo", @"c:\", true)]
        [InlineData(@"c:\foo", @"c:\foo", true)]
        [InlineData(@"c:\foo", @"c:\foo\", true)]
        [InlineData(@"c:\foo\", @"c:\foo", true)]
        [InlineData(@"c:\foo\bar\", @"c:\foo\", true)]
        [InlineData(@"c:\foo\bar", @"c:\foo\", true)]
        [InlineData(@"c:\foo\a.txt", @"c:\foo", true)]
        [InlineData(@"c:\FOO\a.txt", @"c:\foo", true)]
        [InlineData(@"c:/foo/a.txt", @"c:\foo", true)]
        [InlineData(@"c:\foobar", @"c:\foo", false)]
        [InlineData(@"c:\foobar\a.txt", @"c:\foo", false)]
        [InlineData(@"c:\foobar\a.txt", @"c:\foo\", false)]
        [InlineData(@"c:\foo\a.txt", @"c:\foobar", false)]
        [InlineData(@"c:\foo\a.txt", @"c:\foobar\", false)]
        [InlineData(@"c:\foo\..\bar\baz", @"c:\foo", false)]
        [InlineData(@"c:\foo\..\bar\baz", @"c:\bar", true)]
        [InlineData(@"c:\foo\..\bar\baz", @"c:\barr", false)]
        public void IsSubPathOf(string path, string baseDirPath, bool expectedResult)
        {
            Assert.Equal(expectedResult, path.IsSubPathOf(baseDirPath));
        }
    }
}
