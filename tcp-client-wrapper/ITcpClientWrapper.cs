using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace tcp_client_wrapper
{
    /// <summary>
    /// Just a decorator style wrapper for the TcpClient class to make it unit testable..
    /// </summary>
    public interface ITcpClientWrapper : IDisposable
    {
        int SendTimeout { get; set; }
        int ReceiveTimeout { get; set; }
        int SendBufferSize { get; set; }
        int ReceiveBufferSize { get; set; }
        bool ExclusiveAddressUse { get; set; }
        bool Connected { get; }
        int Available { get; }
        Socket Client { get; set; }
        LingerOption LingerState { get; set; }
        bool NoDelay { get; set; }

        IAsyncResult BeginConnect(IPAddress[] addresses, int port, AsyncCallback requestCallback, object state);

        IAsyncResult BeginConnect(IPAddress address, int port, AsyncCallback requestCallback, object state);
    
        IAsyncResult BeginConnect(string host, int port, AsyncCallback requestCallback, object state);

        void Close();

        void Connect(IPAddress[] ipAddresses, int port);

        void Connect(IPEndPoint remoteEP);

        void Connect(IPAddress address, int port);

        void Connect(string hostname, int port);

        Task ConnectAsync(string host, int port);

        Task ConnectAsync(IPAddress address, int port);

        Task ConnectAsync(IPAddress[] addresses, int port);
       
        void EndConnect(IAsyncResult asyncResult);

        NetworkStream GetStream();
    }
}
