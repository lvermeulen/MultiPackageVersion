namespace MultiPackageVersion.Core
{
    public interface IIgnoreReader
    {
        (bool, string) Execute(string fileName);
    }
}
