using System;
using System.Collections.Generic;
using Xunit;

namespace MultiPackageVersion.Formatters.Build.ConsoleBuildOutput.Tests
{
    public class ConsoleBuildOutputFormatterShould
    {
        private readonly ConsoleBuildOutputFormatter _formatter = new ConsoleBuildOutputFormatter();

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
            Assert.Equal($"some key{Environment.NewLine}another key", result);
        }
    }
}
