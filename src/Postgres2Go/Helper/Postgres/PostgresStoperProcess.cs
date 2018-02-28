using Postgres2Go.Helper.Process;
using System;
using System.IO;

namespace Postgres2Go.Helper.Postgres
{
    internal class PostgresStoperProcess
    {
        private const int ProcessTimeoutInSeconds = 10;
        private const string ProcessIdentifier = nameof(PostgresStoperProcess);

        internal static void Exec(string binariesDirectory, string dataDirectory)
        {
            string pgControllerExecutablePath = $"{binariesDirectory}{Path.DirectorySeparatorChar}{PostgresDefaults.ServerControllerExecutable}";
            string arguments = $"stop -D \"{dataDirectory}\" ";

            System.Diagnostics.Process serverStopperProcess = Process.ProcessController
                .CreateProcess(pgControllerExecutablePath, arguments);

            ProcessOutput output = ProcessController
                .StartAndWaitForReady(serverStopperProcess, ProcessTimeoutInSeconds, ProcessIdentifier, "Postgres stopping");
        }
    }
}
