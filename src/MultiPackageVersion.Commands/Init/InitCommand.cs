using System.IO;
using MultiPackageVersion.Config;
using MultiPackageVersion.Core;

namespace MultiPackageVersion.Commands.Init
{
    public class InitCommand : ICommand<Void, (bool, string)>
    {
        public (bool, string) Execute(Void t = default(Void))
        {
            const string FILENAME = "mpv.config";
            if (File.Exists(FILENAME))
            {
                return (false, $"File {FILENAME} already exists.");
            }

            var config = new Configuration();
            config.Save(FILENAME);

            return (true, $"File {FILENAME} was created.");
        }
    }
}
