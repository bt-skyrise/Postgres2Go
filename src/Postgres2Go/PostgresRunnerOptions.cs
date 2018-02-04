namespace Postgres2Go
{
    public class PostgresRunnerOptions
    {
        /// <summary>
        /// Working data directory where Postgres will be initialized
        /// </summary>
        public string DataDirectory { get; set; }

        /// <summary>
        /// Pattern of path where postgres binaries should be located
        /// </summary>
        public string BinariesSearchPattern { get; set; }

        /// <summary>
        /// Run on specified port
        /// </summary>
        public ushort? Port { get; set; }
    }
}