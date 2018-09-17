using System;
using McMaster.Extensions.CommandLineUtils;
using MultiPackageVersion.Mpv.Commands;

namespace MultiPackageVersion.Mpv.CommandConfiguration
{
    public static class RootCommandConfiguration
    {
        public static void Configure(CommandLineApplication app, CommandLineOptions options, IServiceProvider serviceProvider)
        {
            app.Command("init", c => InitCommandConfiguration.Configure(c, options));
            app.Command("build", c => BuildCommandConfiguration.Configure(c, options, serviceProvider));
            app.Command("run", c => RunCommandConfiguration.Configure(c, options, serviceProvider));
            
            app.OnExecute(() =>
            {
                options.Command = new RootCommand(app);

                return 0;
            });
        }
    }
}