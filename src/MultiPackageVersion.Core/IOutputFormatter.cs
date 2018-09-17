using System.Collections.Generic;

namespace MultiPackageVersion.Core
{
    public interface IOutputFormatter<in T, out TResult>
    {
        TResult Format(IEnumerable<T> input);
    }
}
