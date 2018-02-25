using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Postgres2Go.Helper.Postgres
{
    internal static class PostgresLibLinksFixerProcess
    {
        internal static void Exec(string binariesDirectory)
        {
            string pgLibLinksFixingScriptPath = Path.Combine(binariesDirectory, "./../scripts/") + "fix_linux_pg_dist_lib_links.sh";

            FileSystem.FileSystem
                .GrantExecutablePermission(pgLibLinksFixingScriptPath);

            string bashExecutable = "/bin/bash";
            string arguments = $"-c \"{pgLibLinksFixingScriptPath} {binariesDirectory}\"";
            
            System.Diagnostics.Process serverInitializatorProcess = Process.ProcessController
                .CreateProcess(bashExecutable, arguments);

            Process.ProcessOutput output = Process.ProcessController
                .StartAndWaitForExit(serverInitializatorProcess, $"Execute fix_linux_pg_dist_lib_links.sh");

            if(output.ExitCode != 0)
                throw new PostgresProcessFinishedWithErrorsException("Cannot prepare Pg lib symbolic links." + output.ToString());
                
            Console.Write(output.ToString());
        }
    }
}
