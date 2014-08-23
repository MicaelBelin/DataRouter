using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Core.Session.Packet
{

    [Connection.Packet.AutoGenerateFactory]
    class CreateStreamRequest : Connection.IRequest
    {
        public IAgent Agent { get; private set; }
        public TimeSpan Timeout { get; private set; }

        public CreateStreamRequest(IAgent agent, TimeSpan timeout)
        {
            Agent = agent;
            Timeout = timeout;
        }

        public byte[] ToByteArray()
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write(Agent.ToArray());
                writer.Write(Timeout.Ticks);
                return stream.ToArray();
            }
        }

        public static Connection.IPacket FromByteArray(byte[] data)
        {
            using (var stream = new MemoryStream(data))
            using (var reader = new BinaryReader(stream))
            {
                return new CreateStreamRequest(Core.Agent.Implementation.FromStream(stream), TimeSpan.FromTicks(reader.ReadInt64()));
            }
        }

    }
}
