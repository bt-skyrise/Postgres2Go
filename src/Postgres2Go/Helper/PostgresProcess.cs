using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Postgres2Go.Helper
{
    public partial class PostgresProcess
    {
        private Process _process;

        public IEnumerable<string> ErrorOutput { get; set; }
        public IEnumerable<string> StandardOutput { get; set; }

        internal PostgresProcess(Process process)
        {
            _process = process;
        }

        public void Dispose()
        {
            if (this._process != null)
                this._process.Kill();
            GC.SuppressFinalize(this);
        }
    }
}
