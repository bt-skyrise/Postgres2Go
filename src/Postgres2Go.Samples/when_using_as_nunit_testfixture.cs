using System;
using System.Collections.Generic;
using System.Text;
using Npgsql;
using NUnit.Framework;

namespace Postgres2Go.Samples
{
    [TestFixture]
    public class when_using_as_nunit_testfixture
    {
        private PostgresRunner _pgRunner;

        [OneTimeSetUp]
        public void Init()
        {
            _pgRunner = PostgresRunner.Start();
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            _pgRunner?.Dispose();
        }

        [Test]
        public async System.Threading.Tasks.Task should_create_new_database_and_allow_exec_query_1()
        {
            // PREPARE
            var dbName = "test_nunit_db_1";

            var cmdBuilder = new StringBuilder();
            cmdBuilder.AppendLine($"CREATE DATABASE {dbName}");
            cmdBuilder.AppendLine("CONNECTION LIMIT = -1");

            // RUN
            using (var conn = new Npgsql.NpgsqlConnection(_pgRunner.GetConnectionString()))
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

        [Test]
        public async System.Threading.Tasks.Task should_create_new_database_and_allow_exec_query_2()
        {
            // PREPARE
            var dbName = "test_nunit_db_2";

            var cmdBuilder = new StringBuilder();
            cmdBuilder.AppendLine($"CREATE DATABASE {dbName}");
            cmdBuilder.AppendLine("CONNECTION LIMIT = -1");

            // RUN
            using (var conn = new Npgsql.NpgsqlConnection(_pgRunner.GetConnectionString()))
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
