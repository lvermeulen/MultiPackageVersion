using System;
using System.Collections.Generic;
using System.Linq;
using MultiPackageVersion.Core;

namespace MultiPackageVersion.Formatters.Build.ConsoleBuildOutput
{
    public class ConsoleBuildOutputFormatter : IOutputFormatter<KeyValuePair<string, string>, string>
    {
        public string Format(IEnumerable<KeyValuePair<string, string>> input)
        {
            string result = string.Join(Environment.NewLine, input.Select(x => x.Key));

            return result;
        }
    }
}
