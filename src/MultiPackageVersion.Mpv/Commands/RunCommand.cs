using System;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using MultiPackageVersion.Config;
using MultiPackageVersion.Core;
using MultiPackageVersion.Differs.Git;

namespace MultiPackageVersion.Mpv.Commands
{
    public class RunCommand : ICommand
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly string _gitPath;

        public RunCommand(IServiceProvider serviceProvider, CommandOption gitPath)
        {
            _serviceProvider = serviceProvider;
            _gitPath = gitPath.Value() ?? @"C:\Program Files\Git\bin\git.exe";
        }

        public int Execute()
        {
            var console = _serviceProvider.GetService<IConsole>();
            var solutionReader = _serviceProvider.GetService<ISolutionReader>();
            var differ = new GitStatusDiffer(_gitPath);

            console.WriteLine("Files modified:");
            var command = new MultiPackageVersion.Commands.Run.RunCommand(solutionReader, differ);
            (bool success, var results) = command.Execute(Configuration.Load("mpv.config"));
            results
                .UpdatedFiles
                .Select(x => $"{x.Key}\t{x.Value}")
                .ToList()
                .ForEach(x => console.WriteLine($"{x}"));

            return success
                ? 0
                : 1;
        }
    }
}
