using System;
using MultiPackageVersion.Commands.Init;
using MultiPackageVersion.Core;

namespace MultiPackageVersion.Mpv.Commands
{
    public class InitCommand : ICommand
    {
        public int Execute()
        {
            Console.WriteLine("Initializing:");
            var command = new MultiPackageVersion.Commands.Init.InitCommand();
            (bool success, InitContext initContext) = command.Execute();
            Console.WriteLine(initContext.Message);

            return success
                ? 0
                : 1;
        }
    }
}
