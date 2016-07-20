using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAUtil.UDP.Commands
{
    public class CommandFactory
    {
        public enum COMMAND
        {
            IISRESTART
        };

        public static IMessageSerializable CreateCommand(COMMAND cmd)
        {
            switch (cmd)
            {
                case COMMAND.IISRESTART:
                    return new IISRestart();
            }
            return null;
        }
    }
}
