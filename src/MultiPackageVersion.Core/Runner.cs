using System.Diagnostics;

namespace MultiPackageVersion.Core
{
    public class Runner : IRunner
    {
        private readonly string _executablePath;
        private readonly string _arguments;

        public Runner(string executablePath, string arguments = "")
        {
            _executablePath = executablePath;
            _arguments = arguments;
        }

        private (bool, string) RunInternal(string executablePath, string arguments)
        {
            using (var process = new Process())
            {
                process.StartInfo = new ProcessStartInfo
                {
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
#pragma warning disable SCS0001 // Possible command injection
                    FileName = executablePath,
                    Arguments = arguments,
#pragma warning restore SCS0001 // Possible command injection
                    CreateNoWindow = true
                };

                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                return (process.ExitCode == 0, output);
            }
        }

        public (bool, string) Run()
        {
            return RunInternal(_executablePath, _arguments);
        }

        public (bool, string) Run(string arguments)
        {
            return RunInternal(_executablePath, arguments);
        }

        public (bool, string) Run(string executablePath, string arguments)
        {
            return RunInternal(executablePath, arguments);
        }
    }
}
