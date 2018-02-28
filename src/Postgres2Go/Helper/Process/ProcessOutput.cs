using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        internal new string ToString()
        {
            var sb = new StringBuilder();
            
            sb.AppendLine();
            sb.AppendLine("===");
            
            StandardOutput.ToList().ForEach(str => sb.AppendLine(str));
            
            sb.AppendLine();
            
            ErrorOutput.ToList().ForEach(str => sb.AppendLine(str));

            return sb.ToString();
        }
    }
}
