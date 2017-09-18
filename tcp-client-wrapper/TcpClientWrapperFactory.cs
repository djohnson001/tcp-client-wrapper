using System.Net;
using System.Net.Sockets;

namespace tcp_client_wrapper
{
    public class TcpClientWrapperFactory : ITcpClientWrapperFactory
    {
        public ITcpClientWrapper Create()
        {
            return new TcpClientWrapper();
        }

        public ITcpClientWrapper Create(TcpClient tcpClient)
        {
            return new TcpClientWrapper(tcpClient);
        }

        public ITcpClientWrapper Create(IPEndPoint localEP)
        {
            return new TcpClientWrapper(localEP);
        }

        public ITcpClientWrapper Create(AddressFamily family)
        {
            return new TcpClientWrapper(family);
        }

        public ITcpClientWrapper Create(string hostname, int port)
        {
            return new TcpClientWrapper(hostname, port);
        }
    }
}
