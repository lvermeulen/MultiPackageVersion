using System.Collections.Generic;
using Xunit;

namespace MultiPackageVersion.Formatters.Build.JsonBuildOutput.Tests
{
    public class JsonBuildOutputFormatterShould
    {
        private readonly JsonBuildOutputFormatter _formatter = new JsonBuildOutputFormatter();

        [Fact]
        public void Format()
        {
            var input = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("some key", "some value"),
                new KeyValuePair<string, string>("another key", "another value")
            };
            string result = _formatter.Format(input);
            Assert.NotNull(result);
            Assert.Equal("[\"some key\",\"another key\"]", result);
        }
    }
}
