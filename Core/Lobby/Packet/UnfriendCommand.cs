using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Core.Lobby.Packet
{
    class UnfriendCommand : Connection.ICommand
    {
        public IAgent Agent { get; private set; }

        public UnfriendCommand(IAgent agent)
        {
            Agent = agent;
        }

        public Connection.Packet.Wrapping Wrapped
        {
            get
            {
                using (var stream = new MemoryStream())
                using (var writer = new BinaryWriter(stream))
                {
                    writer.Write(Agent.ToArray());
                    return new Connection.Packet.Wrapping(typeof(UnfriendCommand).Name, stream.ToArray());
                }
            }
        }


        public class Factory : Connection.Packet.IFactory
        {
            public string Type
            {
                get { return typeof(UnfriendCommand).Name; }
            }

            public Connection.IPacket Create(byte[] data)
            {
                using (var stream = new MemoryStream(data))
                {
                    return new UnfriendCommand(Core.Agent.Implementation.FromStream(stream));
                }
            }
        }
    }
}
