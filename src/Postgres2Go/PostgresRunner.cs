using System;
using System.IO;
using Postgres2Go.Helper.Net;
using Postgres2Go.Helper.Postgres;
using Postgres2Go.Helper.FileSystem;

namespace Postgres2Go
{
    public partial class PostgresRunner : IDisposable
    {
        private string _dataDirectory;
        private int _port;
        private PostgresBinaryLocator _pgBin;
        private PostgresProcess _pgStarterProcess;

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

            PostgresInitializatorProcessStarter
                .Init(_pgBin.Directory, _dataDirectory, PostgresDefaults.User);

            _pgStarterProcess = PostgresProcessStarter
                .Start(_pgBin.Directory, _dataDirectory, _port);
            
            ConnectionString = $"Server=localhost;Port={_port};User Id={PostgresDefaults.User};Database=postgres";
            State = State.Running;
        }


        private static string GetUniqueHash() => Guid.NewGuid().ToString().GetHashCode().ToString("x");

        ~PostgresRunner()
        {
            Dispose(false);
        }

        public bool Disposed { get; private set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (Disposed)
            {
                return;
            }

            if (State != State.Running)
            {
                return;
            }
            
            if (_pgBin != null)
            {
                try
                {
                    PostgresProcessStarter
                        .Stop(_pgBin.Directory, _dataDirectory);
                }
                catch (Exception)
                {
                    ;
                }
            }

            if (_pgStarterProcess != null)
            {
                _pgStarterProcess.Dispose();
            }


            FileSystem
                .DeleteFolder(_dataDirectory);

            Disposed = true;
            State = State.Stopped;
        }
    }
}
