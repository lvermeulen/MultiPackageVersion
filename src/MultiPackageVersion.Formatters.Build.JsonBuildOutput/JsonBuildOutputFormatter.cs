using System.Collections.Generic;
using System.Linq;
using MultiPackageVersion.Core;

namespace MultiPackageVersion.Formatters.Build.JsonBuildOutput
{
    public class JsonBuildOutputFormatter : IOutputFormatter<KeyValuePair<string, string>, string>
    {
        public string Format(IEnumerable<KeyValuePair<string, string>> input)
        {
            string result = "[";
            result += string.Join(",", input.Select(x => x.Key.DoubleQuoted()));
            result += "]";

            return result;
        }
    }
}
