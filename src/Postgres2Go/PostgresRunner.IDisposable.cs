using System;
using System.Collections.Generic;
using System.Text;
using Postgres2Go.Helper.FileSystem;
using Postgres2Go.Helper.Postgres;

namespace Postgres2Go
{
    partial class PostgresRunner : IDisposable
    {
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
            //if (Disposed)
            //{
            //    return;
            //}

            //if (State != State.Running)
            //{
            //    return;
            //}
			
            //if (_pgStarterProcess != null)
            //{
            //    _pgStarterProcess.Dispose();
            //}

            //if(_pgBin != null)
            //{
            //    PostgresProcessStarter
            //        .Stop(_pgBin.Directory, _dataDirectory);
            //}
            
            //Helper.FileSystem.FileSystem
            //    .DeleteFolder(_dataDirectory);

            //Disposed = true;
            //State = State.Stopped;
        }
    }
}
