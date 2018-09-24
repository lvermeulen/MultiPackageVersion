using System;
using System.IO;
using MultiPackageVersion.Config;
using MultiPackageVersion.Core;
using Void = MultiPackageVersion.Core.Void;

namespace MultiPackageVersion.Commands.Init
{
    public class InitCommand : ICommand<Void, (bool, InitContext)>
    {
        public (bool, InitContext) Execute(Void t = default(Void))
        {
            const string FILENAME = "mpv.config";

            var result = new InitContext { FolderName = Environment.CurrentDirectory };
            if (File.Exists(FILENAME))
            {
                result.Message = $"File {FILENAME} already exists.";
                return (false, result);
            }

            var config = new Configuration();
            config.Save(FILENAME);

            result.Message = $"File {FILENAME} was created.";
            return (true, result);
        }
    }
}
