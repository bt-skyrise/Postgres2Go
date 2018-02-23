using Postgres2Go.Common;
using Postgres2Go.Helper.Postgres.Platform.Linux;

namespace Postgres2Go.Helper.Postgres.Platform
{
    internal class PostgresPlatformSetup
    {
        internal static void Start(string pgBinDirectoryPath)
        {
            switch (RecognizedOSPlatform.Determine())
            {
                case RecognizedOSPlatformEnum.Linux:
                    new PostgresLinuxSetup().Start(pgBinDirectoryPath);
                    break;
                case RecognizedOSPlatformEnum.OSX:
                    throw new UnsupportedPlatformException($"Cannot grant executable right to Postgres binaries when running on OSX platform. OSX platform is not supported.");
                case RecognizedOSPlatformEnum.Windows:
                default:
                    break;

                    
            }
        }
        
    }
}