using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Net;

namespace ServerLib
{
    public class ChatClient
    {

        public delegate void MessageReceivedHandler(ChatClient client, Message message);
        public delegate void ClientCloseHandler(ChatClient client);
        public delegate void ClientConnectedHandler(ChatClient client);

        public event MessageReceivedHandler ReceiveHandler;
        public event ClientCloseHandler CloseHandler;
        public event ClientConnectedHandler ConnectHandler;

        private TcpClient _client;
        private Timer _pingTimer;
        private PINGSTATUS _pingStatus;
        private bool _Closing;
        private enum PINGSTATUS
        {
            NONE,
            SENT,
            RECEIVED
        }

        public struct Message
        {
            public string From;
            public string Date;
            public string Content;
        }

        public string Id
        {
            get;
            set;
        }

        public ChatClient(string server, int port)
        {
            _client = null;
            _server = server;
            _port = port;
        }

        internal ChatClient(TcpClient client)
        {
            this._client = client;
        }

        public static void PingTimerCallback(object state)
        {
            ChatClient client = (ChatClient)state;
            if (client._pingStatus == PINGSTATUS.NONE)
            {
                try
                {
                    Message msg = client.CreateMessage("/ping");
                    client._pingStatus = PINGSTATUS.SENT;
                    client.SendMessage(new Sender() { Name = "Me" }, msg);
                }
                catch 
                {
                    try
                    {
                        client.Connect();
                    }
                    catch
                    {
                        client._pingStatus = PINGSTATUS.NONE;
                    }
                }
            }
            else if (client._pingStatus == PINGSTATUS.SENT)
            {
                // the reply has never been received, then try again in the next run
                client._pingStatus = PINGSTATUS.NONE;
            }
            else
            {
                // Reset and wait for the next ping
                client._pingStatus = PINGSTATUS.NONE;
            }
        }

        public void Connect()
        {
            _client = new TcpClient();
            IPAddress ipAddress = null;
            IPAddress[] addresses = Dns.GetHostEntry(_server).AddressList;
            for (var x = 0; x < addresses.Length; x++)
            {
                if (!addresses[x].IsIPv6LinkLocal && (addresses[x].AddressFamily == AddressFamily.InterNetwork))
                {
                    ipAddress = addresses[x];
                    break;
                }
            }
            if (ipAddress != null)
            {
                _pingStatus = PINGSTATUS.NONE;

                if (_pingTimer != null)
                {
                    _pingTimer.Dispose();
                    _pingTimer = null;
                }
                _pingTimer = new Timer(PingTimerCallback, this, 30000, 30000);
                _client.Connect(ipAddress, _port);
                Id = _client.Client.RemoteEndPoint.ToString();
                WaitMessage();

                if (ConnectHandler != null) ConnectHandler(this);
            }
            else
            {
                throw new ApplicationException("The IPv4 is not supported");
            }
        }

        private Message CreateMessage(string content)
        {
            ServerLib.ChatClient.Message message = new ChatClient.Message();
            message.From = this.Id;
            message.Date = DateTime.Now.ToString();
            message.Content = content;
            return message;
        }
        public void ChangeNick(string nick)
        {
            ServerLib.ChatClient.Message message = new ChatClient.Message();
            message.From = this.Id;
            message.Date = DateTime.Now.ToString();
            message.Content = "/hello " + nick;
            Sender s = new Sender()
            {
                Name = "Me"
            };
            SendMessage(s, message);
            this.Id = nick;
        }

        public void SendMessage(Sender sender, Message message)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("user:{0}\n", sender.Name);
            sb.AppendFormat("date:{0}\n", message.Date);
            sb.AppendFormat("message:{0}<EOF>", message.Content);

            byte[] data = UTF8Encoding.UTF8.GetBytes(sb.ToString());
            _client.GetStream().Write(data, 0, data.Length);
        }

        public static void StartWait(object o)
        {
            ChatClient client = (ChatClient)o;
            client.InternalWait();
        }

        public void InternalWait()
        {
            NetworkStream stream = _client.GetStream();

            StringBuilder sbMessage = new StringBuilder();
            byte[] buffer = new byte[1024];
            int read;
            try
            {
                while ((read = stream.Read(buffer, 0, 1024)) > 0)
                {
                    sbMessage.Append(UTF8Encoding.UTF8.GetString(buffer, 0, read));
                    if (sbMessage.ToString().EndsWith("<EOF>"))
                    {
                        if (ReceiveHandler != null)
                        {
                            Message message = ParseMessage(sbMessage.ToString());

                            if (!message.Content.StartsWith("[08]"))
                            {
                                ReceiveHandler(this, message);
                            }
                            else
                            {
                                _pingStatus = PINGSTATUS.RECEIVED;
                            }
                            sbMessage.Clear();
                        }
                    }
                }
            }
            catch
            {
                if (!_Closing && (CloseHandler != null))
                {
                    CloseHandler(this);
                }
            }
        }

        private void RemoveHandlers()
        {
            CloseHandler = null;
            ReceiveHandler = null;
            ConnectHandler = null;
        }

        private Message ParseMessage(string p)
        {
            Message m = new Message();
            string[] lines = p.Split('\n');
            m.From = lines[0].Split(':')[1];
            m.Date = lines[1].Split(':')[1];
            m.Content = lines[2].Substring(lines[2].IndexOf(':') + 1);
            for (int x = 3; x < lines.Length; x++)
            {
                if (m.Content.Length > 0)
                {
                    m.Content += "\r\n";
                }
                m.Content += lines[x];
            }
            if (m.Content.EndsWith("<EOF>"))
            {
                m.Content = m.Content.Substring(0, m.Content.Length - 5);
            }

            return m;
        }

        public void WaitMessage()
        {
            Thread thread = new Thread(StartWait);
            thread.Start(this);
        }

        public void Close()
        {
            _Closing = true;
            _client.Close();
            _Closing = false;
        }

        public bool IsConnected()
        {
            return _client.Connected;
        }

        public string _server { get; set; }

        public int _port { get; set; }
    }
}
