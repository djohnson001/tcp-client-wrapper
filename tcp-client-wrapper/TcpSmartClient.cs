using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using wait_for_it_lib;

namespace tcp_client_wrapper
{
    public class TcpSmartClient : TcpConnection, ITcpSmartClient
    {
        private readonly TcpClient _tcpClient;
        private readonly NetworkStream _stream;
        private bool _hasSentOutConnectedMessage = false;

        public event Action OnConnected;

        private TcpSmartClient(IPEndPoint endpoint)
        {
            _tcpClient = new TcpClient();
            _tcpClient.Connect(endpoint);
            _stream = _tcpClient.GetStream();

            StartWorker();
        }

        private TcpSmartClient(string hostname, int port)
        {
            _tcpClient = new TcpClient();
            _tcpClient.Connect(hostname, port);
            _stream = _tcpClient.GetStream();

            StartWorker();
        }

        public override void SendMessage(string text)
        {
            new WaitForIt().Wait(() => _tcpClient.Connected, TimeSpan.FromSeconds(2));

            var requestBytes = Encoding.ASCII.GetBytes(text);
            _stream.Write(requestBytes, 0, requestBytes.Length);
        }

        protected override void WorkerThread()
        {
            while (_shouldKeepWorking)
            {
                if (_tcpClient.Connected && !_hasSentOutConnectedMessage)
                {
                    _hasSentOutConnectedMessage = true;
                    OnConnected?.Invoke();                    
                }

                Iterate(_tcpClient, _stream);
                Thread.Sleep(25);
            }
        }        

        protected override TcpClient GetTcpClient()
        {
            return _tcpClient;
        }

        public bool Connected
        {
            get { return _tcpClient.Connected; }
        }      

        public static TcpSmartClient Connect(int port)
        {
            return Connect(LoopBack, port);
        }

        public static TcpSmartClient Connect(string hostName, int port)
        {
            return new TcpSmartClient(hostName, port);
        }

        public static TcpSmartClient Connect(byte[] address, int port)
        {
            return Connect(new IPAddress(address), port);
        }

        public static TcpSmartClient Connect(IPAddress address, int port)
        {
            return Connect(new IPEndPoint(address, port));
        }

        public static TcpSmartClient Connect(IPEndPoint endpoint)
        {
            return new TcpSmartClient(endpoint);
        }
    }
}
