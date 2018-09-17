using System;

namespace MultiPackageVersion.Core
{
    public interface ICommand
    {
        int Execute();
    }

    public interface ICommand<in T, out TResult>
    {
        TResult Execute(T t = default(T));
    }
}
