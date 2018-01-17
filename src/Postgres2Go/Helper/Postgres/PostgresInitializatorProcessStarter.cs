using System.Linq;

namespace Postgres2Go.Helper.Postgres
{
    internal class PostgresInitializatorProcessStarter
    {
        private const int ProcessStartTimeout = 10;

        internal static void Init(string binariesDirectory, string dataDirectory, string user)
        {

            string pgControllerExecutablePath = $"{binariesDirectory}{System.IO.Path.DirectorySeparatorChar}{PostgresDefaults.ServerControllerExecutable}";
            //string arguments = $"init -D \"{dataDirectory}\" -o \"-U test\" ";
            string arguments = $"init -D \"{dataDirectory}\" -o \" -U {user} \" ";

            System.Diagnostics.Process serverInitializatorProcess = Process.ProcessController
                .CreateProcess(pgControllerExecutablePath, arguments);

            Process.ProcessOutput output = Process.ProcessController
                .StartAndWaitForExit(serverInitializatorProcess, $"postgres initializing ...");

            if(output.ExitCode != 0)
                throw new PostgresProcessFinishedWithErrorsException(System.String.Join("\n",output.ErrorOutput));
        }
    }
}
