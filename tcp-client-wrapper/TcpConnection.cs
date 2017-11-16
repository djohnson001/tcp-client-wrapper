using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace tcp_client_wrapper
{
    public abstract class TcpConnection : ITcpConnection
    {
        public virtual event Action OnDataRececived;

        protected static byte[] LoopBack { get { return new byte[] { 127, 0, 0, 1 }; } }
        protected bool _shouldKeepWorking = true;

        private object MessageLocker = new object();
        private Queue<string> _messages = new Queue<string>();        

        private Task _workerTask;

        protected void StartWorker()
        {
            _workerTask = Task.Run(() => WorkerThread());
        }

        public void Dispose()
        {
            _shouldKeepWorking = false;
            _workerTask.Wait();
        }

        protected virtual void Iterate(TcpClient client, NetworkStream stream)
        {
            if (!_shouldKeepWorking || client == null || !client.Connected || stream == null)
            {
                return;
            }

            var receivedText = GetPendingString(stream);

            if (receivedText != null)
            {
                lock (MessageLocker)
                {
                    _messages.Enqueue(receivedText);
                }

                OnDataRececived?.Invoke();
            }
        }

        public bool AreThereAnyPendingMessages()
        {
            lock (MessageLocker)
            {
                return _messages.Count > 0;
            }
        }

        public string GetNextPendingMessage()
        {
            lock (MessageLocker)
            {
                return _messages.Count > 0 ? _messages.Dequeue() : null;
            }
        }

        public List<string> GetAllPendingMessagees()
        {
            lock (MessageLocker)
            {
                var dequeuedMessages = _messages.ToArray().ToList();
                _messages.Clear();

                return dequeuedMessages;
            }
        }

        public virtual void SendMessage(string text)
        {
            if (_currentStream == null)
            {
                if (_currentClient == null && (_currentClient = GetTcpClient()) == null)
                {
                    return;
                }

                if (!_currentClient.Connected)
                {
                    return;
                }

                if ((_currentStream = _currentClient.GetStream()) == null)
                {
                    return;
                }
            }
            
            var requestBytes = Encoding.ASCII.GetBytes(text);
            _currentStream.Write(requestBytes, 0, requestBytes.Length);
        }

        protected abstract TcpClient GetTcpClient();

        private TcpClient _currentClient = null;
        private NetworkStream _currentStream = null;

        protected virtual void WorkerThread()
        {
            while (_shouldKeepWorking)
            {
                if (_currentClient == null || !_currentClient.Connected)
                {
                    _currentStream = null;
                    _currentClient = GetTcpClient();
                    if (_currentClient != null && _currentClient.Connected)
                    {
                        _currentStream = _currentClient.GetStream();
                    }
                }

                if (_currentClient != null && _currentClient.Connected)
                {
                    if (_currentStream == null)
                    {
                        _currentStream = _currentClient.GetStream();
                    }

                    if (_currentStream != null)
                    {
                        Iterate(_currentClient, _currentStream);
                    }
                }

                Thread.Sleep(25);
            }
        }

        private string GetPendingString(NetworkStream stream)
        {
            var bytes = GetPendingBytes(stream);
            if (bytes == null) { return null; }

            return Encoding.ASCII.GetString(bytes);
        }

        private byte[] GetPendingBytes(NetworkStream stream)
        {
            if (!_shouldKeepWorking || stream == null) { return null; }

            var data = new List<byte>();
            int currentByte = -1;
            while (_shouldKeepWorking && stream.DataAvailable && (currentByte = stream.ReadByte()) != -1)
            {
                if (!_shouldKeepWorking) { return null; }
                data.Add((byte)currentByte);
            }

            if (!_shouldKeepWorking) { return null; }
            return !data.Any() ? null : data.ToArray();
        }

    }
}
