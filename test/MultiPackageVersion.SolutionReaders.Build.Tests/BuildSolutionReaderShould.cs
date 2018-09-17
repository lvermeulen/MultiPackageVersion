using MultiPackageVersion.Core;
using Xunit;

namespace MultiPackageVersion.SolutionReaders.Build.Tests
{
    public class BuildSolutionReaderShould
    {
        private readonly ISolutionReader _reader = new BuildSolutionReader();

        [Fact]
        public void ReadFileNames()
        {
            var results = _reader.ReadFileNames(@"C:\bbrepo\Ess\workforce\WorkForce.sln");
            Assert.NotEmpty(results);
        }
    }
}
