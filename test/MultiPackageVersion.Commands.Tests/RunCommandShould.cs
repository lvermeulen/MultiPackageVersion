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
                var command = new RunCommand(new NativeSolutionReader(), new GitStatusDiffer(@"C:\Program Files\Git\bin\git.exe"));
                var configuration = Configuration.Load("mpv.config");
                (bool success, var results) = command.Execute(configuration);
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
