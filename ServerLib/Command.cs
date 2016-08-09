using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerLib
{
    public class Command
    {
        public string Text { get; set; }
        public string Key { get; set; }
        public string Script { get; set; }
        public string Arguments { get; set; }

        public ChatClient Client { get; set; }
        public ChatServer Server { get; set; }

        public Command Clone()
        {
            return new Command()
            {
                Text = this.Text,
                Key = this.Key,
                Script = this.Script
            };
        }

        public static List<Command> ParseCommands(string messageAvailableCommands)
        {
            messageAvailableCommands = messageAvailableCommands.Substring("[09:".Length + 1);
            messageAvailableCommands = messageAvailableCommands.Substring(0, messageAvailableCommands.IndexOf("]"));

            string[] commands = messageAvailableCommands.Split(new char[] { '\r' });
            List<Command> result = new List<Command>();
            for (int x = 0; x < commands.Length; x++)
            {
                string[] values = commands[x].Split(new char[] { '·' });
                Command cmd = new Command()
                {
                    Key = values[0],
                    Text = values[1]
                };
                result.Add(cmd);
            }
            return result;
        }

        public static string Serialize(List<Command> commands)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Command c in commands)
            {
                sb.Append("\r" + c.Key + "·" + c.Text);
            }
            return sb.ToString();
        }
    }
}
