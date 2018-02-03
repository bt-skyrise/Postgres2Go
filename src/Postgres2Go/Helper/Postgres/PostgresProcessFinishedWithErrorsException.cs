using System;
using System.Collections.Generic;
using System.Text;

namespace Postgres2Go.Helper.Postgres
{
    public class PostgresProcessFinishedWithErrorsException : Exception
    {
        public PostgresProcessFinishedWithErrorsException() { }
        public PostgresProcessFinishedWithErrorsException(string message) : base(message) { }
        public PostgresProcessFinishedWithErrorsException(string message, Exception inner) : base(message, inner) { }
    }
}
