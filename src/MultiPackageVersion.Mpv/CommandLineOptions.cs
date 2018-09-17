using System;
using McMaster.Extensions.CommandLineUtils;
using MultiPackageVersion.Core;
using MultiPackageVersion.Mpv.CommandConfiguration;

namespace MultiPackageVersion.Mpv
{
    public class CommandLineOptions
    {
        public ICommand Command { get; set; }
        public string GitPath { get; set; }

        public static CommandLineOptions Parse(IServiceProvider serviceProvider, string[] args)
        {
            var options = new CommandLineOptions();

            var app = new CommandLineApplication(throwOnUnexpectedArg: false)
            {
                Name = "mpv",
                FullName = "mpv - A tool to help with your multi-package .NET projects"
            };
            app.HelpOption("-?|-h|--help");

            RootCommandConfiguration.Configure(app, options, serviceProvider);

            int result = app.Execute(args);
            return result != 0
                ? null
                : options;
        }
    }
}
