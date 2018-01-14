namespace Postgres2Go
{
    internal static class PostgresDefaults
    {
        internal static int TcpPort => 5433;
        internal static string ServerExecutable => "postgres";
        internal static string ServerControllerExecutable => "pg_ctl";
        internal static string ServerInitializatorExecutable => "initdb";
    }
}
