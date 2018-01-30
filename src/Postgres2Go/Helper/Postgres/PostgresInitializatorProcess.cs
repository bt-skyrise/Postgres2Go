using System.Linq;

namespace Postgres2Go.Helper.Postgres
{
    internal class PostgresInitializatorProcess
    {
        internal static void Exec(string binariesDirectory, string dataDirectory, string user)
        {

            string pgControllerExecutablePath = $"{binariesDirectory}{System.IO.Path.DirectorySeparatorChar}{PostgresDefaults.ServerControllerExecutable}";
            string arguments = $"init -D \"{dataDirectory}\" -o \" -U {user} \" ";

            System.Diagnostics.Process serverInitializatorProcess = Process.ProcessController
                .CreateProcess(pgControllerExecutablePath, arguments);

            Process.ProcessOutput output = Process.ProcessController
                .StartAndWaitForExit(serverInitializatorProcess, $"Postgres cluster initializing");

            if(output.ExitCode != 0)
                throw new PostgresProcessFinishedWithErrorsException("Cannot initialize Postgres cluster.\n" + System.String.Join("\n",output.ErrorOutput));
        }
    }
}
