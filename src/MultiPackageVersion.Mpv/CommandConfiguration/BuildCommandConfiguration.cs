using System;
using McMaster.Extensions.CommandLineUtils;
using MultiPackageVersion.Mpv.Commands;

namespace MultiPackageVersion.Mpv.CommandConfiguration
{
    public static class BuildCommandConfiguration
    {
        public static void Configure(CommandLineApplication command, CommandLineOptions options, IServiceProvider serviceProvider)
        {
            command.Description = "Lists build definition files affected";
            command.HelpOption("--help|-h|-?");

            var gitPath = command.Option("-g|--git", "Path to git.exe", CommandOptionType.SingleValue);

            command.OnExecute(() =>
            {
                options.Command = new BuildCommand(serviceProvider, gitPath);

                return 0;
            });
        }
    }
}