using System;
using System.Collections.Generic;

namespace Postgres2Go.Helper.Postgres
{
    internal partial class PostgresProcess
    {
        private System.Diagnostics.Process _process;

        public IEnumerable<string> ErrorOutput { get; set; }
        public IEnumerable<string> StandardOutput { get; set; }

        internal PostgresProcess(System.Diagnostics.Process process)
        {
            _process = process;
        }
        
    }
}
