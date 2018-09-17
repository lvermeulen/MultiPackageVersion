using System.IO;
using Microsoft.Extensions.Configuration;
using MultiPackageVersion.Core;
using Xunit;

namespace MultiPackageVersion.Builders.Jenkins.Tests
{
    public class JenkinsBuilderShould
    {
        private readonly IBuilder _builder;

        public JenkinsBuilderShould()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            _builder = new JenkinsBuilder(configuration["url"], configuration["userName"], configuration["password"], configuration["accesstoken"]);
        }

        [Theory]
        [InlineData("Saga.SerialPorts-Release")]
        public void Build(string buildDefinition)
        {
            bool success = _builder.Build(buildDefinition);
            Assert.True(success);
        }
    }
}
