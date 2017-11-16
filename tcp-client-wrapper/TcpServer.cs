using System.Net;
using System.Net.Sockets;

namespace tcp_client_wrapper
{
    public class TcpServer : TcpConnection
    {        
        private readonly TcpListener _tcpListener;        

        private TcpServer(IPEndPoint localEndPoint)
        {
            _tcpListener = new TcpListener(localEndPoint);
            _tcpListener.Start();

            StartWorker();
        }

        protected override TcpClient GetTcpClient()
        {
            return _tcpListener != null && _tcpListener.Pending() ? _tcpListener.AcceptTcpClient() : null;
        }

        public static TcpServer Listen(int port)
        {
            return Listen(LoopBack, port);
        }

        public static TcpServer Listen(byte[] address, int port)
        {
            return Listen(new IPAddress(address), port);
        }

        public static TcpServer Listen(IPAddress address, int port)
        {
            return Listen(new IPEndPoint(address, port));
        }

        public static TcpServer Listen(IPEndPoint endpoint)
        {
            return new TcpServer(endpoint);
        }
    }
}
