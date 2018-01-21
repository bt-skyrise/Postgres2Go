using Postgres2Go.Common;

namespace Postgres2Go
{
    internal static class PostgresDefaults
    {
        internal static int TcpPort => 5433;
        internal static string User => "Test";

        internal static string ServerExecutable 
            => RecognizedOSPlatform.Determine() == RecognizedOSPlatformEnum.Windows ? "postgres.exe" : "postgres";
        internal static string ServerControllerExecutable
            => RecognizedOSPlatform.Determine() == RecognizedOSPlatformEnum.Windows ? "pg_ctl.exe" : "pg_ctl";
        internal static string ServerInitializatorExecutable
            => RecognizedOSPlatform.Determine() == RecognizedOSPlatformEnum.Windows ? "initdb.exe" : "initdb";
        
    }
}
