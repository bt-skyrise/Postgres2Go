using Postgres2Go.Common;

namespace Postgres2Go.Helper.Net
{
    public class PortWatcherFactory
    {
        public static IPortWatcher CreatePortWatcher()
        {
            switch(RecognizedOSPlatform.Determine())
            {
                case RecognizedOSPlatformEnum.Linux:
                    return (IPortWatcher) new UnixPortWatcher();
                case RecognizedOSPlatformEnum.OSX:
                    return (IPortWatcher) new OsxPortWatcher();
                case RecognizedOSPlatformEnum.Windows:
                default:
                    return (IPortWatcher) new WindowsPortWatcher();

            }
        }
    }
}
