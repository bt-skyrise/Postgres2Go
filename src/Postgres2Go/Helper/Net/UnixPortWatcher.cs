using System;
using System.Net;
using System.Net.Sockets;

namespace Postgres2Go.Helper.Net
{
    public sealed class UnixPortWatcher : BasePortWatcher
    {
        protected override bool IsPortAvailable(int portNumber)
        {
            bool result = false;

            TcpListener tcpListener = new TcpListener(IPAddress.Loopback, portNumber);
            try
            {
                tcpListener.Start();
            }
            catch (SocketException)
            {
                result = false;
            }
            finally
            {
                tcpListener.Stop();
                result = true;
            }

            return result;
        }
    }
}
