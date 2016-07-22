using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerLib
{
    public class ServerMessageHandler
    {
        public static bool ProcessMessage(ChatServer server, ChatClient client, ServerLib.ChatClient.Message message)
        {
            if (message.Content.StartsWith("/help"))
            {
                SendHelp(server, client);
                return false;
            }
            if (message.Content.StartsWith("/ping"))
            {
                SendPingBack(server, client);
                return false;
            }
            if (message.Content.StartsWith("/hello"))
            {
                handleHello(server, client, message);
                return false;
            }
            if (message.Content.StartsWith("/restart"))
            {
                ServerLib.ChatClient.Message restartBroadCast = new ServerLib.ChatClient.Message();
                restartBroadCast.From = client.Id;
                restartBroadCast.Date = DateTime.Now.ToString();
                restartBroadCast.Content = "[01] Restart requested by " + client.Id + ", to cancel send /cancelrestart";
                server.BroadCastMessage(restartBroadCast);
                IISReset.Initialize(server);
                IISReset.Instance().Execute(client);
                return false;
            }
            if (message.Content.StartsWith("/cancelrestart"))
            {
                ServerLib.ChatClient.Message restartBroadCast = new ServerLib.ChatClient.Message();
                restartBroadCast.From = client.Id;
                restartBroadCast.Date = DateTime.Now.ToString();
                restartBroadCast.Content = "[04] Restart has been cancelled by " + client.Id;
                server.BroadCastMessage(restartBroadCast);
                IISReset.Initialize(server);
                IISReset.Instance().Cancel(client);
                return false;
            }
            if (message.Content.StartsWith("/list"))
            {
                SendUsersList(server, client);
                return false;
            }
            if (message.Content.StartsWith("/ping"))
            {
                SendPingBack(server, client);
                return false;
            }
            return true;
        }

        private static void handleHello(ChatServer server, ChatClient client, ServerLib.ChatClient.Message message)
        {
            string name = message.Content.Substring(7).Trim();
            client.Id = name;

            ServerLib.ChatClient.Message broadcast = new ServerLib.ChatClient.Message();
            broadcast.Date = DateTime.Now.ToString();
            broadcast.From = client.Id;
            broadcast.Content = "[06] " + client.Id + " has joined the chat room";
            server.BroadCastMessage(broadcast, client);

            // Sends hello back!
            ServerLib.ChatClient.Message msg = new ServerLib.ChatClient.Message();
            msg.From = client.Id;
            msg.Date = DateTime.Now.ToString();
            msg.Content = "Hello " + client.Id + " good to see you!";

            Sender s = new Sender()
            {
                Name = "Server"
            };
            client.SendMessage(s, msg);
        }

        private static void SendUsersList(ChatServer server, ChatClient client)
        {
            ServerLib.ChatClient.Message msg = new ServerLib.ChatClient.Message();
            msg.From = client.Id;
            msg.Date = DateTime.Now.ToString();
            List<ChatClient> users = server.GetClients();
            msg.Content = "[07] Users list:";
            foreach (ChatClient c in users)
            {
                msg.Content += "\r\n" + c.Id;
            }

            Sender s = new Sender()
            {
                Name = "Server"
            };
            client.SendMessage(s, msg);
        }

        private static void SendHelp(ChatServer server, ChatClient client)
        {
            ServerLib.ChatClient.Message msg = new ServerLib.ChatClient.Message();
            msg.From = client.Id;
            msg.Date = DateTime.Now.ToString();

            StringBuilder sb = new StringBuilder();
            sb.Append("Commands help:\r\n");
            sb.Append("/hello username\tChanges nickname\r\n");
            sb.Append("/clear\t\tCleans the chat screen\r\n");
            sb.Append("/restart\t\tExecute the restart command\r\n");
            sb.Append("/cancelrestart\tCancels any restart requested\r\n");
            sb.Append("/list\t\tConnected users");

            msg.Content = sb.ToString();

            Sender s = new Sender()
            {
                Name = "Server"
            };
            client.SendMessage(s, msg);
        }

        private static void SendPingBack(ChatServer server, ChatClient client)
        {
            ServerLib.ChatClient.Message msg = new ServerLib.ChatClient.Message();
            msg.From = client.Id;
            msg.Date = DateTime.Now.ToString();
            StringBuilder sb = new StringBuilder();
            sb.Append("[08]");

            msg.Content = sb.ToString();

            Sender s = new Sender()
            {
                Name = "Server"
            };
            client.SendMessage(s, msg);
        }
    }
}
