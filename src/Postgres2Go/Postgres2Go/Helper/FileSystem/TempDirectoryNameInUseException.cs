using System;

namespace Postgres2Go.Helper.FileSystem
{
    public class TemporaryDirectoryNameInUseException: Exception
    {
        public TemporaryDirectoryNameInUseException() { }
        public TemporaryDirectoryNameInUseException(string message) : base(message) { }
        public TemporaryDirectoryNameInUseException(string message, Exception inner) : base(message, inner) { }
    }
}
