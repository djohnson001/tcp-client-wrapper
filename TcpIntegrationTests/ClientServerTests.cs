using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using tcp_client_wrapper;
using Shouldly;
using wait_for_it_lib;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace TcpIntegrationTests
{
    [TestClass]
    public class ClientServerTests
    {
        private static byte[] LocalHostBytes = new byte[] { 127, 0, 0, 1 };
        private static string ServerAddress = "127.0.0.1";

        private readonly WaitForIt _waitForIt = new WaitForIt();

        private ManualResetEventSlim _slim = new ManualResetEventSlim();
        public void Setup()
        {
            _slim.Wait(TimeSpan.FromSeconds(60));
        }

        [TestCleanup]
        public void Teardown()
        {
            _slim.Reset();
        }

        [TestMethod]
        public void The_client_should_be_able_to_connect_to_the_server()
        {
            const int Port = 1234;

            var _server = TcpServer.Listen(LocalHostBytes, Port);

            var _tcpClient = TcpSmartClient.Connect(ServerAddress, Port);

            _tcpClient.Connected.ShouldBe(true);
        }

        [TestMethod]
        public void Client_to_server()
        {
            const int Port = 2345;

            var server = TcpServer.Listen(LocalHostBytes, Port);

            var messagesReceivedByServer = new List<string>();
            server.OnDataRececived += () =>
            {
                messagesReceivedByServer.Add(server.GetNextPendingMessage());
            };

            var messagesReceivedByClient = new List<string>();
            var client = TcpSmartClient.Connect(ServerAddress, Port);
            client.OnDataRececived += () =>
            {
                messagesReceivedByClient.Add(client.GetNextPendingMessage());
            };

            client.SendMessage("Hello from the client!");

            _waitForIt.Wait(() => messagesReceivedByServer.Any(), TimeSpan.FromSeconds(5))
                .ShouldBeTrue();

            string.Equals(messagesReceivedByServer.Single(), "Hello from the client!", StringComparison.Ordinal).ShouldBeTrue();
            messagesReceivedByClient.Any().ShouldBe(false);
        }

        [TestMethod]
        public void Server_to_client()
        {
            const int Port = 3456;

            var server = TcpServer.Listen(LocalHostBytes, Port);

            var messagesReceivedByServer = new List<string>();
            server.OnDataRececived += () =>
            {
                messagesReceivedByServer.Add(server.GetNextPendingMessage());
            };

            var messagesReceivedByClient = new List<string>();
            var client = TcpSmartClient.Connect(ServerAddress, Port);
            client.OnDataRececived += () =>
            {
                messagesReceivedByClient.Add(client.GetNextPendingMessage());
            };

            _waitForIt.Wait(() => client.Connected, TimeSpan.FromSeconds(2));

            server.SendMessage("Hello from the server!");

            _waitForIt.Wait(() => messagesReceivedByClient.Any(), TimeSpan.FromSeconds(5))
                .ShouldBeTrue();

            string.Equals(messagesReceivedByClient.Single(), "Hello from the server!", StringComparison.Ordinal).ShouldBeTrue();
            messagesReceivedByServer.Any().ShouldBe(false);
        }

        [TestMethod]
        public void Ping_pong()
        {
            const int Port = 4567;

            var server = TcpServer.Listen(LocalHostBytes, Port);
            
            var messagesReceivedByServer = new List<string>();            
            server.OnDataRececived += () =>
            {
                messagesReceivedByServer.Add(server.GetNextPendingMessage());
                server.SendMessage("Pong");
            };

            var messagesReceivedByClient = new List<string>();
            var client = TcpSmartClient.Connect(ServerAddress, Port);
            client.OnDataRececived += () =>
            {
                messagesReceivedByClient.Add(client.GetNextPendingMessage());
            };

            client.SendMessage("Ping");

            _waitForIt.Wait(() => messagesReceivedByServer.Any() && messagesReceivedByClient.Any(), 
                TimeSpan.FromSeconds(5))
                .ShouldBeTrue();

            string.Equals(messagesReceivedByServer.Single(), "Ping", StringComparison.Ordinal).ShouldBeTrue();
            string.Equals(messagesReceivedByClient.Single(), "Pong", StringComparison.Ordinal).ShouldBeTrue();
        }   
    }
}
