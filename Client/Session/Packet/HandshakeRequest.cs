using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Client.Session.Packet
{
    [Core.Connection.Packet.AutoGenerateFactory]
    public class HandshakeRequest : Core.Connection.IRequest
    {
        public string Passphrase { get; private set; }
        public TimeSpan Timeout { get; private set; }

        public HandshakeRequest(string passphrase, TimeSpan timeout)
        {
            Passphrase = passphrase;
            Timeout = timeout;
        }

       

        public byte[] ToByteArray()
        {
            using (var ms = new MemoryStream())
            using (var writer = new BinaryWriter(ms))
            {
                writer.Write(Passphrase);
                writer.Write(Timeout.Ticks);
                return ms.ToArray();
            }
        }

        public static Core.Connection.IPacket FromByteArray(byte[] data)
        {
            using (var ms = new MemoryStream(data))
            using (var reader = new BinaryReader(ms))
            {
                return new HandshakeRequest(reader.ReadString(), TimeSpan.FromTicks(reader.ReadInt64()));
            }
        }


    }
}
