namespace MultiPackageVersion.Core
{
    public interface IBuilder
    {
        bool Build(params string[] buildDefinitions);
    }
}
