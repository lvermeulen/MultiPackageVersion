using McMaster.Extensions.CommandLineUtils;
using MultiPackageVersion.Mpv.Commands;

namespace MultiPackageVersion.Mpv.CommandConfiguration
{
    public static class InitCommandConfiguration
    {
        public static void Configure(CommandLineApplication command, CommandLineOptions options)
        {
            command.Description = "Initializes the current directory";
            command.HelpOption("--help|-h|-?");

            var gitPath = command.Option("-g|--git", "Path to git.exe", CommandOptionType.SingleValue);

            command.OnExecute(() =>
            {
                options.Command = new InitCommand();

                return 0;
            });
        }
    }
}