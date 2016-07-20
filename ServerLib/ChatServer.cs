using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace ServerLib
{
    public class ChatServer
    {

        private TcpListener _listener;

        private bool _running;
        private int _port;

        private List<ChatClient> _clients;
        private static ChatServer _server;

        public static void StartListener(int port)
        {
            _server = new ChatServer(port);
            Thread thread = new Thread(InternalStart);
            thread.Start(_server);
        }

        private ChatServer(int port)
        {
            _port = port;
            _clients = new List<ChatClient>();
        }

        private static void InternalStart(object server)
        {
            ChatServer chat = (ChatServer)server;
            chat.Listen();
        }

        public List<ChatClient> GetClients()
        {
            return _clients;
        }

        private void Listen()
        {
            _listener = new TcpListener(IPAddress.Any, _port);
            _running = true;
            _listener.Start();
            while (_running)
            {
                TcpClient client = _listener.AcceptTcpClient();
                AddClient(client);
            }
        }

        public void Stop()
        {
            _running = false;
            _listener.Stop();
        }

        public static void StopListener()
        {
            _server.Stop();
        }

        private void MessageHandler(ChatClient client, ServerLib.ChatClient.Message message)
        {
            Sender sender = new Sender()
            {
                Name = client.Id
            };
            if (ProcessServerMessage(client, message))
            {
                List<ChatClient> clients = new List<ChatClient>();
                clients.AddRange(_clients);
                foreach (ChatClient c in clients)
                {
                    if (c != client)
                    {
                        try
                        {
                            c.SendMessage(sender, message);
                        }
                        catch
                        {
                            RemoveClient(c);
                        }
                    }
                }
            }
        }

        private void RemoveClient(ChatClient client)
        {
            ServerLib.ChatClient.Message restartBroadCast = new ServerLib.ChatClient.Message();
            restartBroadCast.Date = DateTime.Now.ToString();
            restartBroadCast.From = client.Id;
            restartBroadCast.Content = "[05] " + client.Id + " has left the chat room<EOF>";
            _clients.Remove(client);
            BroadCastMessage(restartBroadCast);
        }

        private void CheckConnections()
        {
            List<ChatClient> toRemove = new List<ChatClient>();

            foreach (ChatClient c in _clients)
            {
                if (!c.IsConnected())
                {
                    toRemove.Add(c);
                }
            }
            ServerLib.ChatClient.Message restartBroadCast = new ServerLib.ChatClient.Message();
            restartBroadCast.Date = DateTime.Now.ToString();
            foreach (ChatClient c in toRemove)
            {
                RemoveClient(c);
            }
        }
        public void BroadCastMessage(ServerLib.ChatClient.Message message, ChatClient excludeClient = null)
        {
            CheckConnections();

            List<ChatClient> clients = new List<ChatClient>();
            clients.AddRange(_clients);
            foreach (ChatClient c in clients)
            {
                Sender s = new Sender()
                {
                    Name = "Server"
                };
                try
                {
                    if (excludeClient != c)
                    {
                        c.SendMessage(s, message);
                    }
                }
                catch (Exception)
                {
                    RemoveClient(c);
                }
            }
        }

        private bool ProcessServerMessage(ChatClient client, ChatClient.Message message)
        {
            return ServerMessageHandler.ProcessMessage(this, client, message);
        }

        private void AddClient(TcpClient client)
        {
            ChatClient cclient = new ChatClient(client);

            cclient.ReceiveHandler += MessageHandler;
            cclient.CloseHandler += ClientCloseHandler;
            _clients.Add(cclient);
            cclient.WaitMessage();
        }

        public void ClientCloseHandler(ChatClient client)
        {
            _clients.Remove(client);
        }

    }
}
