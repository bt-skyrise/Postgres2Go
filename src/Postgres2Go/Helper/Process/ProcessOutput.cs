using System.Collections.Generic;

namespace Postgres2Go.Helper.Process
{
    internal class ProcessOutput
    {
        internal ProcessOutput(IEnumerable<string> standardOutput, IEnumerable<string> errorOutput)
        {
            StandardOutput = standardOutput;
            ErrorOutput = errorOutput;
        }

        internal IEnumerable<string> StandardOutput { get; private set; }
        internal IEnumerable<string> ErrorOutput { get; private set; }
    }
}
