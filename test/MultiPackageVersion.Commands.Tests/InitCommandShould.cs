using MultiPackageVersion.Commands.Init;
using MultiPackageVersion.Core;
using Xunit;

namespace MultiPackageVersion.Commands.Tests
{
    public class InitCommandShould
    {
        private readonly ICommand<Void, (bool, InitContext)> _command = new InitCommand();

        [Theory]
        [InlineData(@"C:\gitrepo\Eps\xpsdev")]
        public void Execute(string folderName)
        {
            using (new WithCurrentDirectory(folderName))
            {
                _command.Execute();
            }
        }
    }
}
