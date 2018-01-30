using System;
using Postgres2Go.Helper.Process;
using System.IO;

namespace Postgres2Go.Helper.Postgres
{
    internal class PostgresStarterProcess
    {
        private const int ProcessTimeoutInSeconds = 10;
        private const string ProcessIdentifier = nameof(PostgresStarterProcess);

        internal static void Exec(string binariesDirectory, string dataDirectory, int port)
        {
            string pgControllerExecutablePath = $"{binariesDirectory}{Path.DirectorySeparatorChar}{PostgresDefaults.ServerControllerExecutable}";
            string arguments = $"start -D \"{dataDirectory}\" -s -o \"-i -p {port} -F \"";

            System.Diagnostics.Process serverStarterProcess = Process.ProcessController
                .CreateProcess(pgControllerExecutablePath, arguments);
            
            ProcessOutput output = Process.ProcessController
                .StartAndWaitForReady(serverStarterProcess, ProcessTimeoutInSeconds, ProcessIdentifier, $"Postgres starting | port: {port}");
            
            if (output.ExitCode != 0)
                throw new PostgresProcessFinishedWithErrorsException("Cannot start Postmaster.\n" + String.Join("\n", output.ErrorOutput));
        }
    }
}
