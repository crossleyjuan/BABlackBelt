using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BAUtil.UDP
{
    public abstract class IMessageSerializable
    {
        internal abstract byte[] InternalSerialize();

        /// <summary>
        /// This will return a unique code that identifies the message, each implementation
        /// will have the Guid hardcoded.
        /// </summary>
        /// <returns></returns>
        internal abstract Guid GetCommandUID();

        public byte[] Serialize()
        {
            MemoryStream ms = new MemoryStream();
            byte[] uid = GetCommandUID().ToByteArray();
            ms.Write(uid, 0, uid.Length);

            byte[] data = InternalSerialize();
            ms.Write(data, 0, data.Length);

            ms.Flush();

            return ms.ToArray();
        }

        public static IMessageSerializable Deserialize(byte[] data)
        {
            byte[] bguid = new byte[16];
            Array.Copy(data, bguid, 16);

            Guid guid = new Guid(bguid);

            if (guid == Commands.IISRestart.MessageID)
            {
                Commands.IISRestart restart = Commands.IISRestart.Deserialize(data, 17);
                return restart;
            }
            else
            {
                throw new ApplicationException("Unknown Command: " + guid.ToString());
            }
        }
    }
}
