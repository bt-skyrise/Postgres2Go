using System.Collections.Generic;

namespace Postgres2Go.Helper.Process
{
    internal class ProcessOutput
    {
        internal ProcessOutput(IEnumerable<string> standardOutput, IEnumerable<string> errorOutput, int exitCode)
        {
            StandardOutput = standardOutput;
            ErrorOutput = errorOutput;
            ExitCode = exitCode;
        }

        internal IEnumerable<string> StandardOutput { get; private set; }
        internal IEnumerable<string> ErrorOutput { get; private set; }
        internal int ExitCode{get;private set;}
    }
}
