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
        /// <param name="dataDirectory">Working directory where Postgres cluster will be initialized</param>
        /// <param name="postgresBinariesSearchPattern">Pattern of path where postgres binaries should be located</param>
        /// <param name="databaseName">Name of database used within of connection string</param>
        public static PostgresRunner Start(string dataDirectory = null, string postgresBinariesSearchPattern = null, string databaseName = "postgres")
        {
            if (dataDirectory == null)
            {
                dataDirectory = TempDirectory.Create();
            }

            // this is required to support multiple instances to run in parallel
            var instanceDataDirectory = Path.Combine(dataDirectory, GetUniqueHash());
            
            PostgresRunner pgRunner = null;

            try
            {
                pgRunner = new PostgresRunner().Run(
                    PortPool.GetInstance, 
                    new PostgresBinaryLocator(postgresBinariesSearchPattern), 
                    dataDirectory,
                    databaseName
                );

                return pgRunner;
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                pgRunner?.Dispose(true);
                throw;
            }
    
        }

        private PostgresRunner Run(
            IPortPool portPool,
            PostgresBinaryLocator pgBin,
            string dataDirectory,
            string databaseName
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

            ConnectionString = $"Server=localhost;Port={_port};User Id={PostgresDefaults.User};Database={databaseName}";
            State = State.Running;

            return this;
        }


        private static string GetUniqueHash() => Guid.NewGuid().ToString().GetHashCode().ToString("x");

    }
}
