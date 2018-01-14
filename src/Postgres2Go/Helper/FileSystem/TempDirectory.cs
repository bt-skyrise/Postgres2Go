using System.IO;

namespace Postgres2Go.Helper.FileSystem
{
    internal class TempDirectory
    {
        internal static string Create()
        {
            var tempDirPath = GetUnusedPath();
            
            FileSystem
                .CreateFolder(tempDirPath);

            return tempDirPath;
        }

        internal static string GetUnusedPath()
        {
            bool alreadyExists = false;
            int attempts = 0;
            int maxAttempts = 10;
            string tempDirPath = null;

            do
            {
                tempDirPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                alreadyExists = Directory.Exists(tempDirPath);
                attempts++;
            }
            while(!alreadyExists || attempts <= maxAttempts);

            if(alreadyExists)
                throw new TemporaryDirectoryNameInUseException();

            return tempDirPath;
        }
    }
}
