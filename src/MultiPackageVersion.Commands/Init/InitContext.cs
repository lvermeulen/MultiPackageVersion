using MultiPackageVersion.Core;

namespace MultiPackageVersion.Commands.Init
{
    public class InitContext : IContext
    {
        public string FolderName { get; set; }
        public IConfiguration Configuration { get; set; }
        public string Message { get; set; }
    }
}
