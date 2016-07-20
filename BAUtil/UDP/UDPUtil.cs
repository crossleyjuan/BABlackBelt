using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace BAUtil.UDP
{
    public class UDPUtil
    {
        private UdpClient _client;

        private int _port;
        private static UDPUtil _instance;

        public class UDPState
        {
            public UdpClient client;
            public IPEndPoint endPoint;
        }

        private UDPUtil(int port)
        {
            _port = port;
        }

        public static void Send(Commands.CommandFactory.COMMAND cmd)
        {
            if (_instance != null)
            {

                _instance.SendMessage(Commands.CommandFactory.CreateCommand(cmd));
            }
        }

        public static void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                UdpClient u = (UdpClient)((UDPState)(ar.AsyncState)).client;
                IPEndPoint e = (IPEndPoint)((UDPState)(ar.AsyncState)).endPoint;

                Byte[] receiveBytes = u.EndReceive(ar, ref e);
                string receiveString = Encoding.ASCII.GetString(receiveBytes);

                Console.WriteLine("Received: {0}", receiveString);
            }
            catch
            {
            }
        }

        private void StartSocket() {
            _client = new UdpClient(_port);
            IPEndPoint e = new IPEndPoint(IPAddress.Any, _port);
            _client.Connect("127.0.0.1", 19000);

            UDPState state = new UDPState()
            {
                client = _client,
                endPoint = e
            };

            _client.BeginReceive(new AsyncCallback(ReceiveCallback), state);
        }

        private void StopSocket()
        {
            _client.Close();
        }

        public static void Stop()
        {
            _instance.StopSocket();
        }

        public static void Start(int port)
        {
            if (_instance != null)
            {
                _instance.StopSocket();
            }
            _instance = new UDPUtil(port);
            _instance.StartSocket();
        }

        public void SendMessage(IMessageSerializable message)
        {
            byte[] data = message.Serialize();
            _client.Send(data, data.Length);
        }
    }
}
