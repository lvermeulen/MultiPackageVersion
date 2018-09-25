using MultiPackageVersion.Commands.Build;
using MultiPackageVersion.Config;
using MultiPackageVersion.Core;
using MultiPackageVersion.Differs.Git;
using MultiPackageVersion.SolutionReaders.Native;
using Xunit;

namespace MultiPackageVersion.Commands.Tests
{
    public class BuildCommandShould
    {
        [Theory]
        [InlineData(@"C:\gitrepo\Eps\xpsdev")]
        public void Execute(string folderName)
        {
            using (new WithCurrentDirectory(folderName))
            {
                var command = new BuildCommand(new NativeSolutionReader(), new GitStatusDiffer(@"C:\Program Files\Git\bin\git.exe"));
                var configuration = Configuration.Load("mpv.config");
                (bool success, BuildContext result) = command.Execute(configuration);
                Assert.True(success);
                Assert.NotNull(result);
                Assert.NotEmpty(result.Differences);
                Assert.NotEmpty(result.VersionConfigurationEntries);
            }
        }
    }
}
