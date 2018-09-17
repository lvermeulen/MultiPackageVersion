using System;
using McMaster.Extensions.CommandLineUtils;
using MultiPackageVersion.Core;

namespace MultiPackageVersion.Mpv.Commands
{
    public class RootCommand : ICommand
    {
        private readonly CommandLineApplication _app;

        public RootCommand(CommandLineApplication app)
        {
            _app = app;
        }

        public int Execute()
        {
            _app.ShowHelp();

            return 1;
        }
    }
}
