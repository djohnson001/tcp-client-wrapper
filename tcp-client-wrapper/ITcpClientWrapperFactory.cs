using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace tcp_client_wrapper
{
    public interface ITcpClientWrapperFactory
    {
        ITcpClientWrapper Create();
        ITcpClientWrapper Create(TcpClient tcpClient);
        ITcpClientWrapper Create(IPEndPoint localEP);
        ITcpClientWrapper Create(AddressFamily family);
        ITcpClientWrapper Create(string hostname, int port);
    }
}
