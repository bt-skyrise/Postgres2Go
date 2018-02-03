using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using Xunit;

namespace Postgres2Go.Samples
{
    public class PgFixture : IDisposable
    {
        private readonly PostgresRunner _pgRunner;

        public PgFixture()
        {
            _pgRunner = PostgresRunner
                .Start(new PostgresRunnerOptions{ BinariesSearchPattern = GetPgBinariesRelativePath()});
        }

        public void Dispose() => _pgRunner?.Dispose();

        public string ConnectionString => _pgRunner.GetConnectionString();

        private string GetPgBinariesRelativePath()
        {
            if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return "\\tools\\pgsql-10.1-windows64-binaries\\bin";
            else if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return "/tools/pgsql-10.1-linux-binaries/bin";
            else
                throw new NotSupportedException("OSX is not yet supported");
        }
    }
    
    public class when_using_as_class_fixture : IClassFixture<PgFixture>
    {
        private readonly PgFixture _fixture;

        public when_using_as_class_fixture(PgFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task should_create_new_database_and_allow_exec_query_1()
        {
            // PREPARE
            var dbName = "test_db_1";

            var cmdBuilder = new StringBuilder();
            cmdBuilder.AppendLine($"CREATE DATABASE {dbName}");
            cmdBuilder.AppendLine("CONNECTION LIMIT = -1");

            // RUN
            using (var conn = new Npgsql.NpgsqlConnection(_fixture.ConnectionString))
            {
                await conn.OpenAsync();
                var cmd = new NpgsqlCommand(cmdBuilder.ToString(), conn);
                cmd.ExecuteNonQuery();

                // ASSERT
                var dbExists = await new NpgsqlCommand($"SELECT datname FROM pg_catalog.pg_database WHERE datname = '{dbName}'",conn)
                    .ExecuteScalarAsync();

                Assert.NotNull(dbExists);
            }
        }

        [Fact]
        public async Task should_create_new_database_and_allow_exec_query_2()
        {
            // PREPARE
            var dbName = "test_db_2";

            var cmdBuilder = new StringBuilder();
            cmdBuilder.AppendLine($"CREATE DATABASE {dbName}");
            cmdBuilder.AppendLine("CONNECTION LIMIT = -1");

            // RUN
            using (var conn = new Npgsql.NpgsqlConnection(_fixture.ConnectionString))
            {
                await conn.OpenAsync();
                var cmd = new NpgsqlCommand(cmdBuilder.ToString(), conn);
                cmd.ExecuteNonQuery();

                // ASSERT
                var dbExists = await new NpgsqlCommand($"SELECT datname FROM pg_catalog.pg_database WHERE datname = '{dbName}'",conn)
                    .ExecuteScalarAsync();

                Assert.NotNull(dbExists);
            }
        }
    }
}
