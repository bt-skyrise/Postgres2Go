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
                    FileSystem.FileSystem.GrantExecutablePermission(Path.Combine(pgBinDirectoryPath, PostgresDefaults.ServerControllerExecutable));
                    FileSystem.FileSystem.GrantExecutablePermission(Path.Combine(pgBinDirectoryPath, PostgresDefaults.ServerExecutable));
                    FileSystem.FileSystem.GrantExecutablePermission(Path.Combine(pgBinDirectoryPath, PostgresDefaults.ServerInitializatorExecutable));
                    break;
                case RecognizedOSPlatformEnum.OSX:
                    throw new UnsupportedPlatformException($"Cannot grant executable right to Postgres binaries when running on OSX platform. OSX platform is not supported.");
                case RecognizedOSPlatformEnum.Windows:
                    default:
                    break;

                    
            }
        }
    }
}
