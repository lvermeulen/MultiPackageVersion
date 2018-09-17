using System;
using MultiPackageVersion.Core;

namespace MultiPackageVersion.Mpv.Commands
{
    public class InitCommand : ICommand
    {
        public int Execute()
        {
            Console.WriteLine("Initializing:");
            var command = new MultiPackageVersion.Commands.Init.InitCommand();
            (bool success, string output) = command.Execute();
            Console.WriteLine(output);

            return success
                ? 0
                : 1;
        }
    }
}
