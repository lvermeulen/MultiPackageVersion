namespace MultiPackageVersion.Core
{
    public interface IRunner
    {
        (bool, string) Run();
        (bool, string) Run(string arguments);
        (bool, string) Run(string executablePath, string arguments);
    }
}