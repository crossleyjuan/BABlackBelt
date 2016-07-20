using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAUtil.UDP.Commands
{
    public class IISRestart: IMessageSerializable
    {
        public static Guid MessageID = new Guid("4cc48d76-3981-4c7e-9bde-d6c2460f4904");

        private Guid _internalID;

        internal IISRestart(Guid guid){
        }

        internal IISRestart()
        {
            _internalID = Guid.NewGuid();
        }

        internal override byte[] InternalSerialize()
        {
            // the first int is always the size of the bytes
            byte[] result = BitConverter.GetBytes(0);

            return result;
        }

        internal override Guid GetCommandUID()
        {
            return MessageID;
        }

        public static IISRestart Deserialize(byte[] data, int offset)
        {
            IISRestart restart = new IISRestart();

            return restart;
        }
    }
}
