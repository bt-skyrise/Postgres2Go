using System.IO;

namespace Postgres2Go.Helper.Postgres.Platform.Linux
{
    internal class PostgresLinuxSetup
    {
        internal void Start(string pgBinDirectoryPath)
        {
            GrantPostgresExecutablePermission(pgBinDirectoryPath);
            PreparePostgresLibrarySymbolicLinks(pgBinDirectoryPath);
        }

        private void GrantPostgresExecutablePermission(string pgBinDirectoryPath)
        {
            FileSystem.FileSystem.GrantExecutablePermission(Path.Combine(pgBinDirectoryPath, PostgresDefaults.ServerControllerExecutable));
            FileSystem.FileSystem.GrantExecutablePermission(Path.Combine(pgBinDirectoryPath, PostgresDefaults.ServerExecutable));
            FileSystem.FileSystem.GrantExecutablePermission(Path.Combine(pgBinDirectoryPath, PostgresDefaults.ServerInitializatorExecutable));
        }

        private void PreparePostgresLibrarySymbolicLinks(string pgBinDirectoryPath)
        {
            PostgresLibLinksFixerProcess.Exec(pgBinDirectoryPath);
        }
    }
}
