using System.Runtime.InteropServices;

namespace Postgres2Go.Common
{
    internal enum RecognizedOSPlatformEnum
    {
        Unknown = 0,
        Windows = 1,
        Linux = 2,
        OSX = 3
    }

    internal static class RecognizedOSPlatform
    {
        internal static RecognizedOSPlatformEnum Determine()
        {
#if NETSTANDARD2_0
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return RecognizedOSPlatformEnum.Windows;
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return RecognizedOSPlatformEnum.Linux;
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return RecognizedOSPlatformEnum.OSX;
            }

            return RecognizedOSPlatformEnum.Unknown;
#else
            return RecognizedOSPlatformEnum.Windows;
#endif
        }
    }
}
