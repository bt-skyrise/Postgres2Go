namespace Postgres2Go.Helper.FileSystem
{
    internal static class FileSystem
    {
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
