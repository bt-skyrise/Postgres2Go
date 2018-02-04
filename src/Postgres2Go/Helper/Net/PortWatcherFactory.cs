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
                    throw new UnsupportedPlatformException($"Cannot create {nameof(IPortWatcher)} when running on OSX platform. OSX platform is not supported.");
                case RecognizedOSPlatformEnum.Windows:
                default:
                    return (IPortWatcher) new WindowsPortWatcher();

            }
        }
    }
}
