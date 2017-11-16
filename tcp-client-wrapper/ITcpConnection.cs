using System;
using System.Collections.Generic;

namespace tcp_client_wrapper
{
    public interface ITcpConnection : IDisposable
    {
        event Action OnDataRececived;

        bool AreThereAnyPendingMessages();

        string GetNextPendingMessage();

        List<string> GetAllPendingMessagees();

        void SendMessage(string text);
    }
}
