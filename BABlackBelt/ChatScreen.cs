﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerLib;

namespace BABlackBelt
{
    public partial class ChatScreen : Form
    {
        private ChatClient _client;
        private bool _ClosingChat;
        private static ChatScreen _chat;

        private ChatScreen(ChatClient client)
        {
            InitializeComponent();
            _client = client;
            _ClosingChat = false;
        }

        delegate void AsyncCallback(object o);

        delegate void Callback();

        public static void MessageReceived(ChatClient client, ServerLib.ChatClient.Message message)
        {
            if (!message.Content.StartsWith("[08]"))
            {
                _chat.AddMessage(message);
            }
        }

        public void AddText(object o)
        {
            if (this.txtChat.InvokeRequired)
            {
                AsyncCallback d = new AsyncCallback(AddText);
                this.Invoke(d, new object[] { o });
            }
            else
            {
                if (!_ClosingChat)
                {
                    txtChat.AppendText((string)o);
                    ShowScreen();
                }
            }
        }

        public void AddMessage(ServerLib.ChatClient.Message message)
        {
            AddText(string.Format("[{0}]: {1}\r\n", message.From, message.Content));
            StartFlash();
        }

        public void ShowScreen()
        {
            if (this.InvokeRequired)
            {
                Callback d = new Callback(ShowScreen);
                this.Invoke(d, new object[] { });
            }
            else
            {
                this.Show();
            }
        }

        private void ProcessCommand(string text, ref bool Handled)
        {
            if (text == "/clear")
            {
                txtMessage.Text = "";
                txtChat.Text = "";
                Handled = true;
                return;
            }
            Handled = false;
        }

        private void StartFlash()
        {
            if (this.InvokeRequired)
            {
                Callback d = new Callback(StartFlash);
                this.Invoke(d, new object[] { });
            }
            else
            {
                if (!FlashWindow.WindowHasFocus(this))
                {
                    FlashWindow.Start(this);
                }
            }
        }

        private void StopFlash()
        {
            if (this.InvokeRequired)
            {
                Callback d = new Callback(StopFlash);
                this.Invoke(d, new object[] { });
            }
            else
            {
                FlashWindow.Stop(this);
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            bool Handled = false;
            ProcessCommand(txtMessage.Text, ref Handled);
            if (Handled)
            {
                return;
            }
            ServerLib.ChatClient.Message message = new ChatClient.Message();
            if (!string.IsNullOrEmpty(UserWorkspace.Workspace().ChatUser))
            {
                message.From = UserWorkspace.Workspace().ChatUser;
            }
            else
            {
                message.From = _client.Id;
            }
            message.Date = DateTime.Now.ToString();
            message.Content = txtMessage.Text;
            Sender s = new Sender()
            {
                Name = "Me"
            };
            _client.SendMessage(s, message);
            if (!txtMessage.Text.StartsWith("/"))
            {
                AddMessage(message);
            }
            else
            {
                if (txtMessage.Text.StartsWith("/hello "))
                {
                    UserWorkspace.Workspace().ChatUser = txtMessage.Text.Substring(7).Trim();
                    UserWorkspace.Workspace().SaveWorkspace();
                }
            }
            txtMessage.Text = "";
        }

        private void ChatScreen_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_ClosingChat && e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
            else
            {
                _client.Close();
            }
        }

        private void txtMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSend_Click(sender, e);
                e.Handled = true;
            }
        }

        private void ChatScreen_Load(object sender, EventArgs e)
        {
            txtChat.Text = "/help to list the commands available\r\n";
            _client.CloseHandler += new ChatClient.ClientCloseHandler(Client_CloseHandler);
            _client.ConnectHandler += Client_ConnectHandler;
            _client.ReceiveHandler += MessageReceived;
        }

        void Client_ConnectHandler(ChatClient client)
        {
            AddText(string.Format("Connected to {0}\r\n", client._server));
            EnableControls(true);
        }

        void Client_CloseHandler(ChatClient client)
        {
            AddText("Connection to the server lost, retrying in 30 secs\r\n");
            EnableControls(false);
        }

        public void EnableControls(object o)
        {
            if (this.txtChat.InvokeRequired)
            {
                AsyncCallback d = new AsyncCallback(EnableControls);
                this.Invoke(d, new object[] { o });
            }
            else
            {
                txtMessage.Enabled = (bool)o;
                btnSend.Enabled = (bool)o;
                txtChat.Enabled = (bool)o;
            }
        }

        internal void CloseForm()
        {
            if (this.InvokeRequired)
            {
                Callback d = new Callback(CloseForm);
                this.Invoke(d, new object[] { });
            }
            else
            {
                _ClosingChat = true;
                Close();
            }
        }

        internal static ChatScreen Create()
        {
            _chat = new ChatScreen(UserWorkspace.Workspace().Client);
            return _chat;
        }

        private void ChatScreen_Activated(object sender, EventArgs e)
        {
            if (FlashWindow.WindowHasFocus(this))
            {
                StopFlash();
            }
        }
    }
}
