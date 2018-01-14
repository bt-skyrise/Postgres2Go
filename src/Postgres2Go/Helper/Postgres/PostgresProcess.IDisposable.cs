using System;

namespace Postgres2Go.Helper.Postgres
{
    partial class PostgresProcess : IDisposable
    {

        ~PostgresProcess()
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
			
            if (_process == null)
            {
                return;
            }
			
            if (!_process.HasExited)
            {
                _process.Kill();
                _process.WaitForExit();
            }

            _process.Dispose();
            _process = null;

            Disposed = true;
        }
    }
}
