using MultiPackageVersion.Core;
using Xunit;

namespace MultiPackageVersion.Differs.Git.Tests
{
    public class GitStatusDifferShould
    {
        private readonly IDiffer _differ = new GitStatusDiffer(@"C:\Program Files\Git\bin\git.exe");

        [Theory]
        [InlineData(@"C:\gitrepo\Eps\xpsdev")]
        public void Diff(string folderName)
        {
            var result = _differ.Diff(folderName);
            Assert.NotNull(result);
        }
    }
}
