using System;
using System.IO;
using Postgres2Go.Helper.Net;
using Postgres2Go.Helper.Postgres;
using Postgres2Go.Helper.FileSystem;

namespace Postgres2Go
{
    public partial class PostgresRunner
    {
        private string _dataDirectory;
        private int _port;
        private PostgresBinaryLocator _pgBin;

        /// <summary>
        /// State of the current Postgres instance
        /// </summary>
        public State State { get; private set; }

        /// <summary>
        /// Connections string that should be used to establish a connection the Postgres instance
        /// </summary>
        public string ConnectionString { get; private set; }

        /// <summary>
        /// Starts Multiple Postgres instances with each call
        /// On dispose: kills them and deletes their data directory
        /// </summary>
        /// <remarks>Should be used for integration tests</remarks>
        public static PostgresRunner Start(string dataDirectory = null, string searchPatternOverride = null)
        {
            if (dataDirectory == null)
            {
                dataDirectory = TempDirectory.Create();
            }

            // this is required to support multiple instances to run in parallel
            var instanceDataDirectory = Path.Combine(dataDirectory, GetUniqueHash());

            return new PostgresRunner(
                PortPool.GetInstance,
                new PostgresBinaryLocator(searchPatternOverride),
                instanceDataDirectory
            );
        }

        public PostgresRunner()
        {
        }

        /// <summary>
        /// usage: integration tests
        /// </summary>
        private PostgresRunner(
            IPortPool portPool,
            PostgresBinaryLocator pgBin,
            string dataDirectory = null
            )
        {
            try
            {
                Run(portPool, pgBin, dataDirectory);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Dispose(true);
            }
        }

        private void Run(
            IPortPool portPool,
            PostgresBinaryLocator pgBin,
            string dataDirectory
            )
        {
            _port = portPool.GetNextOpenPort();
            _pgBin = pgBin;

            if (dataDirectory == null)
            {
                dataDirectory = TempDirectory.Create();
            }
            else
            {
                FileSystem.CreateFolder(dataDirectory);
            }

            _dataDirectory = dataDirectory;

            PostgresBinaries
                .AssertCanExecute(_pgBin.Directory);

            PostgresInitializatorProcess
                .Exec(_pgBin.Directory, _dataDirectory, PostgresDefaults.User);

            PostgresStarterProcess
                .Exec(_pgBin.Directory, _dataDirectory, _port);

            ConnectionString = $"Server=localhost;Port={_port};User Id={PostgresDefaults.User};Database=postgres";
            State = State.Running;
        }


        private static string GetUniqueHash() => Guid.NewGuid().ToString().GetHashCode().ToString("x");

    }
}
