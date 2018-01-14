using System;
using System.IO;
using Postgres2Go.Helper.Net;
using Postgres2Go.Helper.Postgres;
using Postgres2Go.Helper.FileSystem;

namespace Postgres2Go
{
    public partial class PostgresRunner : IDisposable
    {
        private readonly string _dataDirectory;
        private readonly int _port;
        private readonly PostgresBinaryLocator _pgBin;
        private readonly PostgresProcess _pgProcess;

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

        /// <summary>
        /// usage: integration tests
        /// </summary>
        private PostgresRunner(
            IPortPool portPool, 
            PostgresBinaryLocator pgBin, 
            string dataDirectory = null
            )
        {
            _port = portPool.GetNextOpenPort();
            _pgBin = pgBin;

            if (dataDirectory == null)
            {
                dataDirectory = TempDirectory.Create();
            }

            _dataDirectory = dataDirectory;

            PostgresBinaries
                .AssertCanExecute(_pgBin.Directory);

            ConnectionString = $"Server=localhost;Port:{_port};";
            
            _pgProcess = PostgresProcessStarter
                .Start(_pgBin.Directory, _dataDirectory, _port);

            State = State.Running;
        }


        private static string GetUniqueHash() => Guid.NewGuid().ToString().GetHashCode().ToString("x");
    }
}
