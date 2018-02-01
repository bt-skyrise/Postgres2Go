using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Postgres2Go.Helper.Net
{
    public abstract class BasePortWatcher : IPortWatcher
    {
        public int FindOpenPort(int startPort)
        {
            var rnd = new Random();
            int port = 0;
            int numberOfAttempts = 0;

            do
            {
                port = rnd.Next(startPort, 65000);

            } while (!IsPortAvailable(port) && numberOfAttempts < 100);

            if(port == 0)
                throw new NoFreePortFoundException();

            return port;
        }

        protected virtual bool IsPortAvailable(int portNumber)
        {
            return IPGlobalProperties
                    .GetIPGlobalProperties()
                    .GetActiveTcpListeners()
                    .Where(ep => ep.AddressFamily == AddressFamily.InterNetwork)
                    .All(ep => ep.Port != portNumber)
                    ;

        }

    }
    
}
