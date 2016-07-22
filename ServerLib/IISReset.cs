using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Diagnostics;
using System.Configuration;

namespace ServerLib
{
    public class CustomTimer : Timer
    {
        public ChatClient Client
        {
            get;
            set;
        }

        public ChatServer Server { get; set; }

        public CustomTimer(ChatServer server, ChatClient client, double interval)
            : base(interval)
        {
            this.Server = server;
            this.Client = client;
        }
    }

    public class IISReset
    {
        private static IISReset _instance;

        private ChatClient _client;
        private ChatServer _server;

        private bool _runningIISRestart;

        CustomTimer _timer;

        public static void Initialize(ChatServer server)
        {
            if (_instance == null)
            {
                _instance = new IISReset(server);
            }
        }

        public static IISReset Instance()
        {
            if (_instance == null)
            {
                throw new ApplicationException("Please initialize the IISReset before calling it");
            }
            return _instance;
        }

        private IISReset(ChatServer server)
        {
            _client = null;
            _server = server;
            _runningIISRestart = false;
        }

        static void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            CustomTimer timer = (CustomTimer)sender;
            ServerLib.ChatClient.Message restartBroadCast = new ServerLib.ChatClient.Message();
            restartBroadCast.From = timer.Client.Id;
            restartBroadCast.Date = DateTime.Now.ToString();
            restartBroadCast.Content = "[02] Restart requested by " + timer.Client.Id + " starting now";
            timer.Server.BroadCastMessage(restartBroadCast);
            timer.Enabled = false;
            timer.Stop();

            Instance().ExecuteIISRestart(timer.Server, timer.Client);
        }

        private void ExecuteIISRestart(ChatServer server, ChatClient client)
        {
            _runningIISRestart = true;

            ProcessStartInfo progInfo = new ProcessStartInfo();
            progInfo.CreateNoWindow = true;
            progInfo.RedirectStandardError = true;
            progInfo.RedirectStandardOutput = true;

            string restartCommand = ConfigurationSettings.AppSettings["RestartCommand"];

            if (string.IsNullOrEmpty(restartCommand))
            {
                restartCommand = @"c:\Windows\System32\iisreset.exe";
            }

            progInfo.FileName = restartCommand;

            Process process = new Process();
            progInfo.Arguments = "/restart";
            //progInfo.WorkingDirectory = _folder;
            progInfo.UseShellExecute = false;

            process.StartInfo = progInfo;
            process.Start();

            string stdout_str = process.StandardOutput.ReadToEnd(); // pick up STDOUT
            string stderr_str = process.StandardError.ReadToEnd();  // pick up STDERR

            process.WaitForExit();
            process.Close();

            ServerLib.ChatClient.Message restartBroadCast = new ServerLib.ChatClient.Message();
            restartBroadCast.From = client.Id;
            restartBroadCast.Date = DateTime.Now.ToString();
            restartBroadCast.Content = "[03] Restart requested by " + client.Id + " completed";
            server.BroadCastMessage(restartBroadCast);

            _runningIISRestart = false;
        }

        public void Execute(ChatClient client)
        {
            _timer = new CustomTimer(_server, client, 30000);
            _timer.Elapsed += new ElapsedEventHandler(_timer_Elapsed);
            _timer.Enabled = true;
            _timer.Start();
        }

        public void Cancel(ChatClient client)
        {
            if (_timer != null)
            {
                _timer.Enabled = false;
                _timer.Stop();
            }
            _timer = null;
        }
    }
}
