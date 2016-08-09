using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerLib
{
    /// <summary>
    /// [01] /restart requested
    /// [02] /restart started
    /// [03] /restart completed
    /// [04] /restart cancelled
    /// [05] User left the chat
    /// [06] User joined the chat
    /// [07] Users List
    /// [08] Ping
    /// [09] Check Available commands (internal usage porcelain)
    /// [10] Error
    /// [11] Command executed
    /// [12] Check Available commands (user readable)
    /// </summary>
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
                AsyncController.ExecuteCommand(server, client, message);
                return false;
            }
            if (message.Content.StartsWith("/commands"))
            {
                handleCommands(server, client, message);
                return false;
            }
            if (message.Content.StartsWith("/cancelrestart"))
            {
                AsyncController.CancelCommand("restart", client);
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
            if (message.Content.StartsWith("/command"))
            {
                ProcessCommand(message, server, client);
                return false;
            }
            if (!message.Content.StartsWith("/cancelrestart") 
                && message.Content.StartsWith("/cancel"))
            {
                string cmdText = message.Content.Substring("/cancel".Length).Trim();
                AsyncController.CancelCommand(cmdText, client);
                return false;
            }
            return true;
        }

        private static void ProcessCommand(ChatClient.Message message, ChatServer server, ChatClient client)
        {
            AsyncController.ExecuteCommand(server, client, message);
        }

        private static void handleCommands(ChatServer server, ChatClient client, ChatClient.Message message)
        {
            ServerLib.ChatClient.Message msg = new ServerLib.ChatClient.Message();
            msg.From = client.Id;
            msg.Date = DateTime.Now.ToString();
            Dictionary<string, Command> commands = AsyncController.Commands;

            bool porcelain = (message.Content.IndexOf("porcelain") > -1);
            if (!porcelain)
            {
                msg.Content = "[12] Available Commands:";
                foreach (Command c in commands.Values)
                {
                    msg.Content += "\r\n" + c.Key + "\t\t" + c.Text;
                }
            }
            else
            {
                msg.Content = "[09:";
                msg.Content += Command.Serialize(commands.Values.ToList<Command>());
                msg.Content += "]";
            }
            Sender s = new Sender()
            {
                Name = "Server"
            };
            client.SendMessage(s, msg);
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
