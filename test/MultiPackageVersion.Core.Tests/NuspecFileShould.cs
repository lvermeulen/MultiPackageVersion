using Xunit;

namespace MultiPackageVersion.Core.Tests
{
    public class NuspecFileShould
    {
        private NuspecFile _nuspecFile;

        [Theory]
        [InlineData(@"C:\gitrepo\Eps\xpsdev\Libraries\Terminal\Terminal.nuspec", "Saga.Terminal")]
        [InlineData(@"C:\gitrepo\Eps\xpsdev\Framework\DbInfoGenerator\DbInfoGenerator.nuspec", "DbInfoGenerator")]
        public void Read(string fileName, string id)
        {
            _nuspecFile = new NuspecFile(fileName);
            Assert.Equal(id, _nuspecFile.Id);
        }
    }
}
