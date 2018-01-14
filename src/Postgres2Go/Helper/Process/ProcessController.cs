using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Postgres2Go.Helper.Process
{
    internal class ProcessController
    {
        internal static System.Diagnostics.Process CreateProcess(string executivePath, string arguments)
        {
            return new System.Diagnostics.Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = executivePath,
                    Arguments = arguments,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                }
            };
        }

        internal static ProcessOutput StartAndWaitForExit(System.Diagnostics.Process process, string windowTitle)
        {
            List<string> errorOutput = new List<string>();
            List<string> standardOutput = new List<string>();

            process.ErrorDataReceived += (sender, args) => errorOutput.Add(args.Data);
            process.OutputDataReceived += (sender, args) => standardOutput.Add(args.Data);

            process.Start();

            process.BeginErrorReadLine();
            process.BeginOutputReadLine();

            process.WaitForExit();

            process.CancelErrorRead();
            process.CancelOutputRead();

            return new ProcessOutput(errorOutput, standardOutput);
        }

        internal static ProcessOutput StartAndWaitForReady(System.Diagnostics.Process process, int timeoutInSeconds, string processReadyIdentifier, string windowTitle)
        {
            if (timeoutInSeconds < 1 ||
                timeoutInSeconds > 10)
            {
                throw new ArgumentOutOfRangeException("timeoutInSeconds", "The amount in seconds should have a value between 1 and 10.");
            }

            List<string> errorOutput = new List<string>();
            List<string> standardOutput = new List<string>();
            bool processReady = false;


            process.ErrorDataReceived += (sender, args) => errorOutput.Add(args.Data);
            process.OutputDataReceived += (sender, args) =>
            {
                standardOutput.Add(args.Data);

                if (
                    !string.IsNullOrEmpty(args.Data) 
                    &&
                    args.Data.Contains(processReadyIdentifier)
                )
                {
                    processReady = true;
                }
            };

            process.Start();

            process.BeginErrorReadLine();
            process.BeginOutputReadLine();

            int lastResortCounter = 0;
            int timeOut = timeoutInSeconds * 10;
            while (!processReady)
            {
                System.Threading.Tasks.Task
                    .Delay(100)
                    .Wait();

                if (++lastResortCounter > timeOut)
                {
                    // we waited X seconds.
                    // for any reason the detection did not worked, eg. the identifier changed
                    // lets assume everything is still ok
                    break;
                }
            }

            process.CancelErrorRead();
            process.CancelOutputRead();

            return new ProcessOutput(errorOutput, standardOutput);
        }
    }
}
