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
        public AsyncCommand Instance { get; set; }

        public CustomTimer(AsyncCommand command, double interval)
            : base(interval)
        {
            this.Instance = command;
        }
    }

    public class AsyncCommand
    {
        private ChatClient Client { get; set; }
        private ChatServer Server { get; set; }
        private Command Command { get; set; }
        private string _key;

        private bool _running;

        CustomTimer _timer;

        public delegate void CommandCompleted(string key);
        public event CommandCompleted OnCommandCompleted;

        public delegate void CommandCancelled(string key);
        public event CommandCancelled OnCommandCancelled;

        public AsyncCommand(string key, Command cmd)
        {
            Client = cmd.Client;
            Server = cmd.Server;
            _key = key;
            Command = cmd;
            _running = false;
        }

        static void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            CustomTimer timer = (CustomTimer)sender;
            ServerLib.ChatClient.Message restartBroadCast = new ServerLib.ChatClient.Message();
            restartBroadCast.From = timer.Instance.Client.Id;
            restartBroadCast.Date = DateTime.Now.ToString();
            restartBroadCast.Content = "[02:" + timer.Instance.Command.Key + "] " + timer.Instance.Command.Text + " requested by " + timer.Instance.Client.Id + " starting now";
            
            timer.Instance.Server.BroadCastMessage(restartBroadCast);
            timer.Enabled = false;
            timer.Stop();

            timer.Instance.Execute(timer.Instance.Server, timer.Instance.Client);
        }

        private void Execute(ChatServer server, ChatClient client)
        {
            _running = true;

            try
            {
                ProcessStartInfo progInfo = new ProcessStartInfo();
                progInfo.CreateNoWindow = true;
                progInfo.RedirectStandardError = true;
                progInfo.RedirectStandardOutput = true;

                string script = this.Command.Script;

                if (string.IsNullOrEmpty(script))
                {
                    script = @"c:\Windows\System32\iisreset.exe";
                }

                progInfo.FileName = script;

                Process process = new Process();
                if (this.Command.Arguments != null)
                {
                    progInfo.Arguments = this.Command.Arguments;
                }
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
                restartBroadCast.Content = "[03:" + this.Command.Key + "] " + this.Command.Text + " requested by " + client.Id + " completed";
                server.BroadCastMessage(restartBroadCast);
            }
            catch (Exception e)
            {
                ServerLib.ChatClient.Message restartBroadCast = new ServerLib.ChatClient.Message();
                restartBroadCast.From = client.Id;
                restartBroadCast.Date = DateTime.Now.ToString();
                restartBroadCast.Content = "[03:" + this.Command.Key + "] " + this.Command.Text + " requested by " + client.Id + " has failed with Error: " + e.Message;
                server.BroadCastMessage(restartBroadCast);
            }

            OnCommandCompleted(this._key);
            _running = false;
        }

        public void Execute()
        {
            _timer = new CustomTimer(this, 30000);
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
            ServerLib.ChatClient.Message restartBroadCast = new ServerLib.ChatClient.Message();
            restartBroadCast.From = client.Id;
            restartBroadCast.Date = DateTime.Now.ToString();
            restartBroadCast.Content = "[04:" + Command.Key + "] " + Command.Text + " has been cancelled by " + client.Id;
            this.Server.BroadCastMessage(restartBroadCast);

            if (OnCommandCancelled != null) OnCommandCancelled(this._key);
        }

    }
}
