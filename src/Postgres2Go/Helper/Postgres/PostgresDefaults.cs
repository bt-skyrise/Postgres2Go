namespace Postgres2Go
{
    internal static class PostgresDefaults
    {
        internal static int TcpPort => 5433;
        internal static string ServerExecutable => "postgres.exe";
        internal static string ServerControllerExecutable => "pg_ctl.exe";
        internal static string ServerInitializatorExecutable => "initdb.exe";
    }
}
