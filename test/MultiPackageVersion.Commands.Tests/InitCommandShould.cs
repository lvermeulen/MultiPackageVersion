using MultiPackageVersion.Commands.Init;
using MultiPackageVersion.Core;
using Xunit;

namespace MultiPackageVersion.Commands.Tests
{
    public class InitCommandShould
    {
        [Theory]
        [InlineData(@"C:\gitrepo\Eps\xpsdev")]
        public void Execute(string folderName)
        {
            using (new WithCurrentDirectory(folderName))
            {
                var command = new InitCommand();
                command.Execute();
            }
        }
    }
}
