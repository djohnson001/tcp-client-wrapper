using System;

namespace tcp_client_wrapper
{
    public interface ITcpSmartClient : ITcpConnection
    {
        bool Connected { get; }

        event Action OnConnected;
    }
}
