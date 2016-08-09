using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace ServerLib
{
    public class AsyncController
    {
        static Dictionary<string, AsyncCommand> _runningCommands = new Dictionary<string,AsyncCommand>();

        static Dictionary<string, Command> _commands = new Dictionary<string,Command>();

        private static DateTime _lastCheck;

        private static void ReadCommands()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load("Commands.xml");
                XmlNamespaceManager manager = new XmlNamespaceManager(doc.NameTable);

                _commands.Clear();

                XmlNodeList lst = doc.SelectNodes("//Command");
                foreach (XmlNode nod in lst)
                {
                    XmlNode text = nod.SelectSingleNode("text");
                    XmlNode key = nod.SelectSingleNode("key");
                    XmlNode script = nod.SelectSingleNode("script");
                    XmlNode arguments = nod.SelectSingleNode("arguments");
                    Command cmd = new Command()
                    {
                        Text = text.InnerText,
                        Key = key.InnerText.ToLower(),
                        Script = script.InnerText
                    };
                    if (arguments != null)
                    {
                        cmd.Arguments = arguments.InnerText;
                    }
                    _commands.Add(cmd.Key, cmd);
                }
            }
            catch (Exception e)
            {
            }
        }

        public static Dictionary<string, Command> Commands
        {
            get
            {
                FileInfo info = new FileInfo("Commands.xml");
                DateTime writeTime = info.LastWriteTime;
                if ((_lastCheck == null) || (_lastCheck != writeTime))
                {
                    ReadCommands();
                    _lastCheck = writeTime;
                }

                return _commands;
            }
        }

        public static Command ExecuteCommand(ChatServer server, ChatClient client, ServerLib.ChatClient.Message message) 
        {
            string sMessage = message.Content;

            string command = sMessage.Substring("/command".Length).Trim();
            // /restart is the same length as command therefore it will be empty
            if (command.Length == 0)
            {
                command = "restart";
            }
            command = command.ToLower();

            Command cmd = null;
            if (Commands.ContainsKey(command))
            {
                cmd = Commands[command].Clone();
                cmd.Server = server;
                cmd.Client = client;
                AsyncCommand acmd = new AsyncCommand(command, cmd);
                acmd.OnCommandCompleted += OnCommandCompleted;
                _runningCommands.Add(cmd.Key, acmd);
                acmd.Execute();

                ServerLib.ChatClient.Message restartBroadCast = new ServerLib.ChatClient.Message();
                restartBroadCast.From = client.Id;
                restartBroadCast.Date = DateTime.Now.ToString();
                restartBroadCast.Content = "[01:" + cmd.Key + "] " + cmd.Text + " requested by " + client.Id + ", to cancel send /cancel " + cmd.Key;
                server.BroadCastMessage(restartBroadCast);
            }
            else
            {
                ServerLib.ChatClient.Message msg = new ServerLib.ChatClient.Message();
                msg.From = client.Id;
                msg.Date = DateTime.Now.ToString();
                msg.Content = "Unknown command:" + command;

                Sender s = new Sender()
                {
                    Name = "Server"
                };
                client.SendMessage(s, msg);
            }
            return cmd;
        }

        private static void OnCommandCompleted(string key)
        {
            _runningCommands.Remove(key);
        }

        public static bool CancelCommand(string command, ChatClient client)
        {
            if (_runningCommands.ContainsKey(command))
            {
                AsyncCommand cmd = _runningCommands[command];
                cmd.Cancel(client);
                _runningCommands.Remove(command);
            }
            else
            {
                ServerLib.ChatClient.Message msg = new ServerLib.ChatClient.Message();
                msg.From = client.Id;
                msg.Date = DateTime.Now.ToString();
                msg.Content = "[10] Your command cannot be cancelled, it has already started or it's completed.";

                Sender s = new Sender()
                {
                    Name = "Server"
                };
                client.SendMessage(s, msg);
            }

            return false;
        }
    }
}
