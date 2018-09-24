using System;
using System.Linq;
using MultiPackageVersion.Commands.Run;
using MultiPackageVersion.Config;
using MultiPackageVersion.Core;
using MultiPackageVersion.Differs.Git;
using MultiPackageVersion.SolutionReaders.Native;
using Xunit;
using Xunit.Abstractions;

namespace MultiPackageVersion.Commands.Tests
{
    public class RunCommandShould
    {
        private readonly ICommand<Configuration, (bool, RunContext)> _command = new RunCommand(
            new NativeSolutionReader(),
            new GitStatusDiffer(@"C:\Program Files\Git\bin\git.exe")
        );

        private readonly ITestOutputHelper _testOutputHelper;

        public RunCommandShould(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Theory]
        [InlineData(@"C:\gitrepo\Eps\xpsdev")]
        public void Execute(string folderName)
        {
            using (new WithCurrentDirectory(folderName))
            {
                var configuration = Configuration.Load("mpv.config");
                (bool success, var results) = _command.Execute(configuration);
                var updatedFiles = results
                    .UpdatedFiles
                    .ToList();
                Assert.True(success);
                Assert.NotEmpty(updatedFiles);
                _testOutputHelper.WriteLine(string.Join(Environment.NewLine, updatedFiles));
            }
        }
    }
}
