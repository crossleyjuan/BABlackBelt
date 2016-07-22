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
        private bool _ClosingChat;
        private static ChatScreen _chat;

        private ChatScreen()
        {
            InitializeComponent();
            _ClosingChat = false;
        }

        delegate void AsyncCallback(object o);

        delegate void Callback();

        public static void MessageReceived(ChatClient client, ServerLib.ChatClient.Message message)
        {
            if (!message.Content.StartsWith("[08]"))
            {
                _chat.AddMessage(message);
                _chat.EnableCommands(message.Content);
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
            string sMessage = txtMessage.Text;

            ServerLib.ChatClient.Message message = UserWorkspace.Workspace().SendMessage(sMessage);

            if (!txtMessage.Text.StartsWith("/"))
            {
                AddMessage(message);
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
        }

        private void txtMessage_KeyDown(object sender, KeyEventArgs e)
        {
            /*
            if (e.KeyCode == Keys.Enter)
            {
                btnSend_Click(sender, e);
                e.Handled = true;
            }
             * */
        }

        private void ChatScreen_Load(object sender, EventArgs e)
        {
            txtChat.Text = "/help to list the commands available\r\n";
            btnRestart.Enabled = false;
            btnCancelRestart.Enabled = false;
            UserWorkspace.Workspace().CloseHandler += Client_CloseHandler;
            UserWorkspace.Workspace().ConnectHandler += Client_ConnectHandler;
            UserWorkspace.Workspace().ReceiveHandler += MessageReceived;
        }

        void Client_ConnectHandler(object client)
        {
            AddText(string.Format("Connected to {0}\r\n", ((ChatClient)client).Server));
            EnableControls(true);
            btnRestart.Enabled = true;
            btnCancelRestart.Enabled = false;
        }

        void Client_CloseHandler(object client)
        {
            AddText("Connection to the server lost, retrying in 30 secs\r\n");
            EnableControls(false);
            btnRestart.Enabled = false;
            btnCancelRestart.Enabled = true;
        }

        public void EnableControls(object o)
        {
            if (this.InvokeRequired)
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

        public void EnableCommands(object message)
        {
            if (this.InvokeRequired)
            {
                AsyncCallback d = new AsyncCallback(EnableCommands);
                this.Invoke(d, new object[] { message });
            }
            else
            {
                if (((string)message).StartsWith("[01]")) // Requested
                {
                    btnCancelRestart.Enabled = true;
                    btnRestart.Enabled = false;
                }
                else if (((string)message).StartsWith("[02]")) // Started
                {
                    btnCancelRestart.Enabled = false;
                    btnRestart.Enabled = false;
                }
                else if (((string)message).StartsWith("[03]")) // Completed
                {
                    btnCancelRestart.Enabled = false;
                    btnRestart.Enabled = true;
                }
                else if (((string)message).StartsWith("[04]")) // Cancelled
                {
                    btnCancelRestart.Enabled = false;
                    btnRestart.Enabled = true;
                }
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
                UserWorkspace.Workspace().CloseHandler -= Client_CloseHandler;
                UserWorkspace.Workspace().ConnectHandler -= Client_ConnectHandler;
                UserWorkspace.Workspace().ReceiveHandler -= MessageReceived;
                Close();
            }
        }

        internal static ChatScreen Create()
        {
            _chat = new ChatScreen();
            return _chat;
        }

        private void ChatScreen_Activated(object sender, EventArgs e)
        {
            if (FlashWindow.WindowHasFocus(this))
            {
                StopFlash();
            }
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {

        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            UserWorkspace.Workspace().SendMessage("/restart");
        }

        private void btnCancelRestart_Click(object sender, EventArgs e)
        {
            UserWorkspace.Workspace().SendMessage("/cancelrestart");
        }

        private void txtMessage_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
