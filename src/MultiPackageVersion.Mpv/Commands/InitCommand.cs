using System;
using McMaster.Extensions.CommandLineUtils;
using MultiPackageVersion.Commands.Init;
using MultiPackageVersion.Core;

namespace MultiPackageVersion.Mpv.Commands
{
    public class InitCommand : ICommand
    {
        private readonly string _gitPath;

        public InitCommand(CommandOption gitPath = null)
        {
            _gitPath = gitPath?.Value() ?? @"C:\Program Files\Git\bin\git.exe";
        }

        public int Execute()
        {
            Console.WriteLine("Initializing:");
            var command = new MultiPackageVersion.Commands.Init.InitCommand(_gitPath);
            (bool success, InitContext initContext) = command.Execute();
            Console.WriteLine(initContext.Message);

            return success
                ? 0
                : 1;
        }
    }
}
