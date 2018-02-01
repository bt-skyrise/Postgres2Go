using System;

namespace Postgres2Go.Helper.Net
{
    public class NoFreePortFoundException : Exception
    {
        public NoFreePortFoundException() { }
        public NoFreePortFoundException(string message) : base(message) { }
        public NoFreePortFoundException(string message, Exception inner) : base(message, inner) { }
    }
}
