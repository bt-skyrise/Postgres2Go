using System.IO;

namespace Postgres2Go.Helper.FileSystem
{
    internal static class FileSystem
    {
        internal static void CreateFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        internal static void DeleteFolder(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }

        internal static void DeleteFile(string fullFileName)
        {
            if (File.Exists(fullFileName))
            {
                File.Delete(fullFileName);
            }
        }

        internal static void GrantExecutablePermission(string path)
        {
            var p = System.Diagnostics.Process.Start("chmod", $"+x {path}");
            p.WaitForExit();

            if (p.ExitCode != 0) 
            {
                throw new System.IO.IOException($"Could not set executable bit for {path}");
            }
        }

    }
}
