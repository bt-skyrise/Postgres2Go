using System;
using System.Collections.Generic;
using System.Text;

namespace Postgres2Go.Helper.Postgres
{
    public class PostgresBinariesNotFoundException : Exception
    {
        public PostgresBinariesNotFoundException() { }
        public PostgresBinariesNotFoundException(string message) : base(message) { }
        public PostgresBinariesNotFoundException(string message, Exception inner) : base(message, inner) { }
    }
}
