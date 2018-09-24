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
        private readonly ICommand<Configuration, (bool, BuildContext)> _command = new BuildCommand(
            new NativeSolutionReader(),
            new GitStatusDiffer(@"C:\Program Files\Git\bin\git.exe")
        );

        [Theory]
        [InlineData(@"C:\gitrepo\Eps\xpsdev")]
        public void Execute(string folderName)
        {
            using (new WithCurrentDirectory(folderName))
            {
                var configuration = Configuration.Load("mpv.config");
                (bool success, BuildContext result) = _command.Execute(configuration);
                Assert.True(success);
                Assert.NotNull(result);
                Assert.NotEmpty(result.Differences);
                Assert.NotEmpty(result.VersionConfigurationEntries);
            }
        }
    }
}
