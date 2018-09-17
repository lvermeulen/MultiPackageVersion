using Xunit;

namespace MultiPackageVersion.Dependencies.NuGet.Tests
{
    public class NuGetReaderShould
    {
        private NuGetReader _nuGetReader;

        [Theory]
        [InlineData(@"C:\gitrepo\Eps\xpsdev")]
        public void Read(string path)
        {
            _nuGetReader = new NuGetReader(path);
            var results = _nuGetReader.Read();
            Assert.NotEmpty(results);
        }
    }
}
