using System;
using Postgres2Go.Helper.Process;
using System.IO;
using System.Linq;

namespace Postgres2Go.Helper.Postgres
{
    internal class PostgresProcessStarter
    {
        private const int ProcessStartTimeout = 10;
        private const string ProcessReadyIdentifier = "waiting for connection";

        internal static PostgresProcess Start(string binariesDirectory, string dataDirectory, int port)
        {
            string pgControllerExecutablePath = $"{binariesDirectory}{Path.DirectorySeparatorChar}{PostgresDefaults.ServerControllerExecutable}";
            string arguments = $"start -D \"{dataDirectory}\" -s -o \"-i -p {port} -F \"";

            System.Diagnostics.Process serverStarterProcess = Process.ProcessController
                .CreateProcess(pgControllerExecutablePath, arguments);
            
            ProcessOutput output = Process.ProcessController
                .StartAndWaitForReady(serverStarterProcess, ProcessStartTimeout, ProcessReadyIdentifier, $"postgres starting | port: {port}");

            return new PostgresProcess(serverStarterProcess)
            {
                ErrorOutput = output.ErrorOutput, 
                StandardOutput = output.StandardOutput
            };
        }

        internal static void Stop(string binariesDirectory, string dataDirectory)
        {

            string pgControllerExecutablePath = $"{binariesDirectory}{Path.DirectorySeparatorChar}{PostgresDefaults.ServerControllerExecutable}";
            string arguments = $"stop -D \"{dataDirectory}\" ";

            System.Diagnostics.Process serverStopperProcess = Process.ProcessController
                .CreateProcess(pgControllerExecutablePath, arguments);
            
            ProcessOutput output = Process.ProcessController
                .StartAndWaitForExit(serverStopperProcess, $"postgres stopping");

            if(output.ExitCode != 0)
                throw new PostgresProcessFinishedWithErrorsException(String.Join("\n",output.ErrorOutput));
        }
    }
}
