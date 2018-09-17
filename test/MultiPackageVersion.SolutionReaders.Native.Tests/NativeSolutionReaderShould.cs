using MultiPackageVersion.Core;
using Xunit;

namespace MultiPackageVersion.SolutionReaders.Native.Tests
{
    public class NativeSolutionReaderShould
    {
        private readonly ISolutionReader _reader = new NativeSolutionReader();

        [Fact]
        public void ReadFileNames()
        {
            var results = _reader.ReadFileNames(@"C:\bbrepo\Ess\workforce\WorkForce.sln");
            Assert.NotEmpty(results);
        }
    }
}
