using System.Linq;

namespace Postgres2Go.Helper.Process
{
    internal class ProcessWatcher
    {
        internal bool IsProcessRunning(string processName) 
            => System.Diagnostics.Process
                    .GetProcessesByName(processName)
                    .Any();
    }
}
