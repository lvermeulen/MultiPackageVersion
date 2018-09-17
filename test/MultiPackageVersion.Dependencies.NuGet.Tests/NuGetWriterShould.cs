using Xunit;

namespace MultiPackageVersion.Dependencies.NuGet.Tests
{
    public class NuGetWriterShould
    {
        private readonly NuGetWriter _nuGetWriter = new NuGetWriter();

        [Theory]
        [InlineData(@"C:\gitrepo\Eps\xpsdev\Web\WorkForce.BusinessLogic\WorkForce.BusinessLogic.nuspec", "Workforce.BusinessLogic", "2.3.4")]
        [InlineData(@"C:\gitrepo\Eps\xpsdev\FrameWork\DescEnumGenerator\packages.config", "Saga.Terminal", "1.2.3")]
        [InlineData(@"C:\Users\luk.vermeulen\Documents\Visual Studio 2017\Projects\BitStuff\BitStuff\BitStuff.csproj", "NBitCoin", "1.4.5")]
        public void Write(string fileName, string packageId, string version)
        {
            _nuGetWriter.Write(fileName, packageId, version);
        }
    }
}
