using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace tcp_client_wrapper
{
    /// <summary>
    /// Just a decorator style wrapper for the TcpClient class to make it unit testable.
    /// </summary>
    public class TcpClientWrapper : ITcpClientWrapper
    {
        private readonly TcpClient _tcpClient;

        public TcpClientWrapper()
        {
            _tcpClient = new TcpClient();
        }

        public TcpClientWrapper(TcpClient tcpClient)
        {
            _tcpClient = tcpClient;
        }
        
        public TcpClientWrapper(IPEndPoint localEP)
        {
            _tcpClient = new TcpClient(localEP);
        }

        public TcpClientWrapper(AddressFamily family)
        {
            _tcpClient = new TcpClient(family);
        }

        public TcpClientWrapper(string hostname, int port)
        {
            _tcpClient = new TcpClient(hostname, port);
        }

        //
        // Summary:
        //     Frees resources used by the System.Net.Sockets.TcpClient class.
        ~TcpClientWrapper()
        {
            _tcpClient.Dispose();
        }

        public int SendTimeout { get { return _tcpClient.SendTimeout; } set { _tcpClient.SendTimeout = value; } }
        public int ReceiveTimeout { get { return _tcpClient.ReceiveTimeout; } set { _tcpClient.ReceiveTimeout = value; } }
        public int SendBufferSize { get { return _tcpClient.SendBufferSize; } set { _tcpClient.SendBufferSize = value; } }
        public int ReceiveBufferSize { get { return _tcpClient.ReceiveBufferSize; } set { _tcpClient.ReceiveBufferSize = value; } }
        public bool ExclusiveAddressUse { get { return _tcpClient.ExclusiveAddressUse; } set { _tcpClient.ExclusiveAddressUse = value; } }
        public bool Connected { get { return _tcpClient.Connected; } }
        public int Available { get { return _tcpClient.Available; } }
        public Socket Client { get { return _tcpClient.Client; } set { _tcpClient.Client = value; } }
        public LingerOption LingerState { get { return _tcpClient.LingerState; } set { _tcpClient.LingerState = value; } }
        public bool NoDelay { get { return _tcpClient.NoDelay; } set { _tcpClient.NoDelay = value; } }

        public IAsyncResult BeginConnect(IPAddress[] addresses, int port, AsyncCallback requestCallback, object state)
        {
            return null;
        }

        public IAsyncResult BeginConnect(IPAddress address, int port, AsyncCallback requestCallback, object state)
        {
            return _tcpClient.BeginConnect(address, port, requestCallback, state);
        }

        public IAsyncResult BeginConnect(string host, int port, AsyncCallback requestCallback, object state)
        {
            return _tcpClient.BeginConnect(host, port, requestCallback, state);
        }

        public void Close()
        {
            _tcpClient.Close();
        }

        public void Connect(IPAddress[] ipAddresses, int port)
        {
            _tcpClient.Connect(ipAddresses, port);
        }

        public void Connect(IPEndPoint remoteEP)
        {
            _tcpClient.Connect(remoteEP);
        }

        public void Connect(IPAddress address, int port)
        {
            _tcpClient.Connect(address, port);
        }

        public void Connect(string hostname, int port)
        {
            _tcpClient.Connect(hostname, port);
        }

        public Task ConnectAsync(string host, int port)
        {
            return _tcpClient.ConnectAsync(host, port);
        }

        public Task ConnectAsync(IPAddress address, int port)
        {
            return _tcpClient.ConnectAsync(address, port);
        }

        public Task ConnectAsync(IPAddress[] addresses, int port)
        {
            return _tcpClient.ConnectAsync(addresses, port);
        }

        public void Dispose()
        {
            _tcpClient.Dispose();
        }

        public void EndConnect(IAsyncResult asyncResult)
        {
            _tcpClient.EndConnect(asyncResult);
        }

        public NetworkStream GetStream()
        {
            return _tcpClient.GetStream();
        }
    }
}
