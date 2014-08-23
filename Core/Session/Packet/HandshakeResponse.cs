using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Core.Session.Packet
{
    [Connection.Packet.AutoGenerateFactory]
    public class HandshakeResponse : Connection.IResponse
    {
        public string Passphrase {get; private set;}
        public IAgent Agent {get; private set;}

        public HandshakeResponse(string passphrase, IAgent agent)
        {
            Passphrase = passphrase;
            Agent = agent;
        }

        public byte[] ToByteArray()
        {
            using (var ms = new MemoryStream())
            using (var writer = new BinaryWriter(ms))
            {
                writer.Write(Passphrase);
                writer.Write(Agent.ToArray());
                return ms.ToArray();
            }
        }

        public static Connection.IPacket FromByteArray(byte[] data)
        {

            using (var ms = new MemoryStream(data))
            using (var reader = new BinaryReader(ms))
            {
                return new HandshakeResponse(reader.ReadString(), Core.Agent.Implementation.FromStream(ms));
            }
        }

    }
}
