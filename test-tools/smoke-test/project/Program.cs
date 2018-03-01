using System;
using System.Runtime.InteropServices;
using Npgsql;
using Postgres2Go;

namespace Postgres2GoSmokeTest
{
    public class Program
    {
        public static void Main(params string[] args) 
        {
            using(var runner = PostgresRunner.Start()) 
            using(var conn = new NpgsqlConnection(runner.GetConnectionString()))
            using(var cmd = new NpgsqlCommand("select version()", conn))
            {
                conn.Open();
                var version = cmd.ExecuteScalar() as string;

                Console.WriteLine($"PostgreSQL version: {version}");
            }
        }
    }
}
