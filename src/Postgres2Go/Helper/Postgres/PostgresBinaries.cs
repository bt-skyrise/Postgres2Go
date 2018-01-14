using System.IO;
using Postgres2Go.Common;

namespace Postgres2Go.Helper.Postgres
{
    internal static class PostgresBinaries
    {
        internal static void AssertCanExecute(string pgBinDirectoryPath)
        {
            switch (RecognizedOSPlatform.Determine())
            {
                case RecognizedOSPlatformEnum.Linux:
                case RecognizedOSPlatformEnum.OSX:
                    FileSystem.FileSystem.GrantExecutablePermission(Path.Combine(pgBinDirectoryPath, PostgresDefaults.ServerControllerExecutable));
                    FileSystem.FileSystem.GrantExecutablePermission(Path.Combine(pgBinDirectoryPath, PostgresDefaults.ServerExecutable));
                    FileSystem.FileSystem.GrantExecutablePermission(Path.Combine(pgBinDirectoryPath, PostgresDefaults.ServerInitializatorExecutable));
                    break;
                case RecognizedOSPlatformEnum.Windows:
                    default:
                    break;

                    
            }
        }
    }
}
