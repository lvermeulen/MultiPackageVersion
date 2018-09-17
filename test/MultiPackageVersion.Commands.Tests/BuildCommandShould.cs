using System.Collections.Generic;
using MultiPackageVersion.Commands.Build;
using MultiPackageVersion.Config;
using MultiPackageVersion.Core;
using MultiPackageVersion.Differs.Git;
using MultiPackageVersion.Formatters.Build.JsonBuildOutput;
using MultiPackageVersion.SolutionReaders.Native;
using Xunit;

namespace MultiPackageVersion.Commands.Tests
{
    public class BuildCommandShould
    {
        private readonly ICommand<Configuration, (bool, IEnumerable<KeyValuePair<string, string>>)> _command = new BuildCommand(
            new NativeSolutionReader(),
            new GitStatusDiffer(@"C:\Program Files\Git\bin\git.exe"),
            null
        );

        [Theory]
        [InlineData(@"C:\gitrepo\Eps\xpsdev")]
        public void Execute(string folderName)
        {
            using (new WithCurrentDirectory(folderName))
            {
                var configuration = Configuration.Load("mpv.config");
                (bool success, IEnumerable<KeyValuePair<string, string>> results) = _command.Execute(configuration);
                Assert.True(success);
                Assert.NotEmpty(results);
            }
        }
    }
}
