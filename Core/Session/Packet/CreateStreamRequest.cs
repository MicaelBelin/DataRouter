using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Core.Session.Packet
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


        public class FactoryImpl : Connection.Packet.IFactory
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
        static FactoryImpl factory = new FactoryImpl();
        public Connection.Packet.IFactory Factory { get { return factory; } }
    }
}
