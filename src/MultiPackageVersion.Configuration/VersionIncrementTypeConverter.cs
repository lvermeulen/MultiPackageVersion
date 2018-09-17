using System;
using System.Collections.Generic;
using System.Linq;

namespace MultiPackageVersion.Config
{
    public class VersionIncrementTypeConverter : JsonEnumConverter<VersionIncrementType>
    {
        private static readonly Dictionary<VersionIncrementType, string> s_stringByVersionIncrementType = new Dictionary<VersionIncrementType, string>
        {
            [VersionIncrementType.Major] = "major",
            [VersionIncrementType.Minor] = "minor",
            [VersionIncrementType.Patch] = "patch"
        };

        protected override string ConvertToString(VersionIncrementType value)
        {
            if (!s_stringByVersionIncrementType.TryGetValue(value, out string result))
            {
                result = "patch";
            }

            return result;
        }

        protected override VersionIncrementType ConvertFromString(string s)
        {
            var pair = s_stringByVersionIncrementType.FirstOrDefault(kvp => kvp.Value.Equals(s, StringComparison.OrdinalIgnoreCase));
            // ReSharper disable once SuspiciousTypeConversion.Global
            return EqualityComparer<KeyValuePair<VersionIncrementType, string>>.Default.Equals(pair)
                ? VersionIncrementType.Patch
                : pair.Key;
        }
    }
}
