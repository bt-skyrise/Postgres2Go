using System.IO;
using System.Linq;
using System.Text;

namespace Postgres2Go.Helper.Postgres
{
    internal static class PostgresLibLinksFixerProcess
    {
        internal static void Exec(string binariesDirectory)
        {
            string pgLibLinksFixingScriptPath = Path.Combine(binariesDirectory, "/../scripts/", "fix_linux_pg_dist_lib_links.sh");

            FileSystem.FileSystem
                .GrantExecutablePermission(pgLibLinksFixingScriptPath);

            string bashExecutable = "/bin/bash";
            string arguments = $"-c {pgLibLinksFixingScriptPath}";
            
            System.Diagnostics.Process serverInitializatorProcess = Process.ProcessController
                .CreateProcess(bashExecutable, arguments);

            Process.ProcessOutput output = Process.ProcessController
                .StartAndWaitForExit(serverInitializatorProcess, $"Execute fix_linux_pg_dist_lib_links.sh");

            if(output.ExitCode != 0)
            {
                var sb = new StringBuilder();
                sb.AppendLine();
                output.StandardOutput.Select(sb.AppendLine);
                sb.AppendLine();
                output.ErrorOutput.Select(sb.AppendLine);

                throw new PostgresProcessFinishedWithErrorsException("Cannot prepare Pg lib symbolic links." + sb.ToString());
            }
                
        }
    }
}
