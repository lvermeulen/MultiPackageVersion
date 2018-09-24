namespace MultiPackageVersion.Core
{
    public interface IContext
    {
        string FolderName { get; }
        IConfiguration Configuration { get; }
        string Message { get; }
    }
}
