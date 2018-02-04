using System;
using System.IO;
using Postgres2Go.Helper.Net;
using Postgres2Go.Helper.Postgres;
using Postgres2Go.Helper.FileSystem;

namespace Postgres2Go
{
    public partial class PostgresRunner
    {
        private int _port;
        private readonly PostgresRunnerOptions _options;
        private string _instanceDirectory, _binDirectory;

        /// <summary>
        /// State of the current Postgres instance
        /// </summary>
        public State State { get; private set; }

        /// <summary>
        /// Starts a new Postgres instance with each call
        /// </summary>
        /// <param name="options">Runner options</param>
        /// <remarks>Should be used for integration tests</remarks>
        public static PostgresRunner Start(PostgresRunnerOptions options = null)
        {
            PostgresRunner pgRunner = null;

            try
            {
                pgRunner = new PostgresRunner(options).Run();

                return pgRunner;
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                pgRunner?.Dispose(true);
                throw;
            }
    
        }

        private PostgresRunner(PostgresRunnerOptions options) => _options = options ?? new PostgresRunnerOptions();
        
        private PostgresRunner Run()
        {
            _instanceDirectory = Path.Combine(_options.DataDirectory ?? TempDirectory.GetUnusedPath(), GetUniqueHash());
            FileSystem.CreateFolder(_instanceDirectory);
            
            _port = _options.Port ?? PortPool.GetInstance.GetNextOpenPort();
            _binDirectory = new PostgresBinaryLocator(_options.BinariesSearchPattern).Directory;

            
            PostgresBinaries
                .AssertCanExecute(_binDirectory);

            PostgresInitializatorProcess
                .Exec(_binDirectory, _instanceDirectory, PostgresDefaults.User);

            PostgresStarterProcess
                .Exec(_binDirectory, _instanceDirectory, _port);

            State = State.Running;

            return this;
        }

        private static string GetUniqueHash() => Guid.NewGuid().ToString().GetHashCode().ToString("x");

        public string GetConnectionString(string databaseName = "postgres") => 
            $"Server=localhost;Port={_port};User Id={PostgresDefaults.User};Database={databaseName}";
    }
    

}
