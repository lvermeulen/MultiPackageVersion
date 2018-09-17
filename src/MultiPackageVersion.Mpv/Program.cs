using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using MultiPackageVersion.Builders.Jenkins;
using MultiPackageVersion.Core;
using MultiPackageVersion.SolutionReaders.Native;

namespace MultiPackageVersion.Mpv
{
    public static class Program
    {
        private static ServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            services
                .AddSingleton(PhysicalConsole.Singleton)
                .AddSingleton<IBuilder, JenkinsBuilder>()
                .AddSingleton<ISolutionReader, NativeSolutionReader>();

            return services.BuildServiceProvider();
        }

        public static int Main(string[] args)
        {
            var serviceProvider = ConfigureServices();

            var options = CommandLineOptions.Parse(serviceProvider, args);
            return options?.Command?.Execute() ?? 1;
        }
    }
}
