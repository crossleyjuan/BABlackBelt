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
            if (!message.Content.StartsWith("[08]") && !message.Content.StartsWith("[09]"))
            {
                _chat.AddMessage(message);
            }
            _chat.EnableCommands(message.Content);
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
            string content = message.Content;
            if (content.StartsWith("["))
            {
                if (content.IndexOf("]") > -1)
                {
                    content = content.Substring(content.IndexOf("]") + 1);
                }
            }
            if (!string.IsNullOrEmpty(content))
            {
                AddText(string.Format("[{0} {2}]: {1}\r\n", message.From, content, message.Date.ToString("HH:MM:ss")));
                StartFlash();
            }
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
            UserWorkspace.Workspace().CloseHandler += Client_CloseHandler;
            UserWorkspace.Workspace().ConnectHandler += Client_ConnectHandler;
            UserWorkspace.Workspace().ReceiveHandler += MessageReceived;
        }

        private void LoadCommands(string messageAvailableCommands)
        {
            List<Command> cmds = Command.ParseCommands(messageAvailableCommands);
            ExecuteDropDown.DropDownItems.Clear();
            CancelDropDown.DropDownItems.Clear();

            foreach (Command cmd in cmds) { 
                ToolStripMenuItem item = new ToolStripMenuItem("Execute " + cmd.Text);
                item.Tag = cmd.Key;
                item.Click += OnExecuteCommand_Click;
                item.Enabled = true;
                ExecuteDropDown.DropDownItems.Add(item);
                ToolStripMenuItem itemCancel = new ToolStripMenuItem("Cancel " + cmd.Text);
                itemCancel.Tag = cmd.Key;
                itemCancel.Click += OnCancelCommand_Click;
                itemCancel.Enabled = false;
                CancelDropDown.DropDownItems.Add(itemCancel);
            }
        }

        private void OnCancelCommand_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            if (item != null)
            {
                string command = (string)item.Tag;
                UserWorkspace.Workspace().SendMessage("/cancel " + command);
            }
        }

        private void OnExecuteCommand_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            if (item != null)
            {
                string command = (string)item.Tag;
                UserWorkspace.Workspace().SendMessage("/command " + command);
            }
        }

        void Client_ConnectHandler(object client)
        {
            if (this.InvokeRequired)
            {
                AsyncCallback d = new AsyncCallback(Client_ConnectHandler);
                this.Invoke(d, new object[] { client });
            }
            else
            {
                AddText(string.Format("Connected to {0}\r\n", ((ChatClient)client).Server));
                EnableControls(true);
                UserWorkspace.Workspace().SendMessage("/commands porcelain");
            }
        }

        void Client_CloseHandler(object client)
        {
            if (this.InvokeRequired)
            {
                AsyncCallback d = new AsyncCallback(Client_CloseHandler);
                this.Invoke(d, new object[] { client });
            }
            else
            {
                AddText("Connection to the server lost, retrying in 30 secs\r\n");
                EnableControls(false);
            }
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
                if (((string)message).StartsWith("[01")) // Requested
                {
                    UpdateCommandStatus((string)message);
                }
                else if (((string)message).StartsWith("[02")) // Started
                {
                    UpdateCommandStatus((string)message);
                }
                else if (((string)message).StartsWith("[03")) // Completed
                {
                    UpdateCommandStatus((string)message);
                }
                else if (((string)message).StartsWith("[04")) // Cancelled
                {
                    UpdateCommandStatus((string)message);
                }
                else if (((string)message).StartsWith("[09")) // Available Commands
                {
                    LoadCommands((string)message);
                }
            }
        }

        private void UpdateCommandStatus(string message)
        {
            string ev = message.Substring(1, message.IndexOf(":") - 1);
            string command = message.Substring(message.IndexOf(":") + 1);
            command = command.Substring(0, command.IndexOf("]"));

            ToolStripMenuItem executeItem = null;
            ToolStripMenuItem cancelItem = null;
            foreach (ToolStripMenuItem item in ExecuteDropDown.DropDownItems)
            {
                if (item.Tag.ToString() == command)
                {
                    executeItem = item;
                    break;
                }
            }
            foreach (ToolStripMenuItem item in CancelDropDown.DropDownItems)
            {
                if (item.Tag.ToString() == command)
                {
                    cancelItem = item;
                    break;
                }
            }
            if ((cancelItem != null) && (executeItem != null))
            {
                switch (ev)
                {
                    case "01": // requested
                        cancelItem.Enabled = true;
                        executeItem.Enabled = false;
                        break;
                    case "02": // Started
                        cancelItem.Enabled = false;
                        executeItem.Enabled = false;
                        break;
                    case "03": // Completed
                    case "04": // Cancelled
                        cancelItem.Enabled = false;
                        executeItem.Enabled = true;
                        break;
                    default:
                        cancelItem.Enabled = false;
                        executeItem.Enabled = true;
                        break;
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
