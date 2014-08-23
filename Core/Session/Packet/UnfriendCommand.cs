using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Core.Session.Packet
{
    [Connection.Packet.AutoGenerateFactory]
    class UnfriendCommand : Connection.ICommand
    {
        public IAgent Agent { get; private set; }

        public UnfriendCommand(IAgent agent)
        {
            Agent = agent;
        }

        public byte[] ToByteArray()
        {
            return Agent.ToArray();
        }



        public static Connection.IPacket FromByteArray(byte[] data)
        {
            using (var stream = new MemoryStream(data))
            {
                return new UnfriendCommand(Core.Agent.Implementation.FromStream(stream));
            }
        }
    }
}
