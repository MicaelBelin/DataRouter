using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Core.Lobby.Packet
{
    class CreateStreamRequest : Connection.IRequest
    {
        public IAgent Agent { get; private set; }
        public TimeSpan Timeout { get; private set; }

        public CreateStreamRequest(IAgent agent, TimeSpan timeout)
        {
            Agent = agent;
            Timeout = timeout;
        }

        public Connection.Packet.Wrapping Wrapped
        {
            get
            {
                using (var stream = new MemoryStream())
                using (var writer = new BinaryWriter(stream))
                {
                    writer.Write(Agent.ToArray());
                    writer.Write(Timeout.Ticks);
                    return new Connection.Packet.Wrapping(typeof(CreateStreamRequest).Name, stream.ToArray());
                }
            }
        }


        public class Factory : Connection.Packet.IFactory
        {
            public string Type
            {
                get { return typeof(CreateStreamRequest).Name; }
            }

            public Connection.IPacket Create(byte[] data)
            {
                using (var stream = new MemoryStream(data))
                using (var reader = new BinaryReader(stream))
                {                    
                    return new CreateStreamRequest(Core.Agent.Implementation.FromStream(stream),TimeSpan.FromTicks(reader.ReadInt64()));
                }
            }
        }
    }
}
