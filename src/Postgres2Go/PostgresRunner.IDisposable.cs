using System;
using System.Collections.Generic;
using System.Text;
using Postgres2Go.Helper.FileSystem;

namespace Postgres2Go
{
    public partial class PostgresRunner
    {
        ~PostgresRunner()
        {
            Dispose(false);
        }

        public bool Disposed { get; private set; }

        public void Dispose()
        {
            Dispose();
            GC.SuppressFinalize(this);
        }

        private void Dispose()
        {
            if (Disposed)
            {
                return;
            }

            if (State != State.Running)
            {
                return;
            }
			

            if (_pgProcess != null)
            {
                _pgProcess.Stop(_pgBin.Directory, _dataDirectory);
            }

            Helper.FileSystem.FileSystem
                .DeleteFolder(_dataDirectory);

            Disposed = true;
            State = State.Stopped;
        }
    }
}
