using Postgres2Go.Helper.FileSystem;
using Postgres2Go.Helper.Postgres;
using System;

namespace Postgres2Go
{
    partial class PostgresRunner : IDisposable
    {
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

            PostgresStoperProcess.Exec(_binDirectory, _instanceDirectory);

            FileSystem.DeleteFolder(_instanceDirectory);
            
            State = State.Stopped;

            Disposed = true;
        }
    }
}
