using MultiPackageVersion.Core;
using Xunit;
using static Bullseye.Targets;

namespace MultiPackageVersion.Mpv.Tests
{
    public class ProgramShould
    {
        [Theory]
        [InlineData(@"C:\gitrepo\Eps\xpsdev")]
        public void Init(string folderName)
        {
            var ex = Record.Exception(() =>            
            {
                using (new WithCurrentDirectory(folderName))
                {
                    Target("default", () => Program.Main(new[] { "init" }));
                    RunTargets();
                }
            });

            Assert.Null(ex);
        }

        [Theory]
        [InlineData(@"C:\gitrepo\Eps\xpsdev")]
        public void Build(string folderName)
        {
            var ex = Record.Exception(() =>
            {
                using (new WithCurrentDirectory(folderName))
                {
                    Target("default", () => Program.Main(new[] { "build" }));
                    RunTargets();
                }
            });

            Assert.Null(ex);
        }

        [Theory]
        [InlineData(@"C:\gitrepo\Eps\xpsdev")]
        public void Run(string folderName)
        {
            var ex = Record.Exception(() =>
            {
                using (new WithCurrentDirectory(folderName))
                {
                    Target("default", () => Program.Main(new[] { "run" }));
                    RunTargets();
                }
            });

            Assert.Null(ex);
        }
}
}
