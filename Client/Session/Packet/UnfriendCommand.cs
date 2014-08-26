using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Client.Session.Packet
{
    [Core.Connection.Packet.AutoGenerateFactory]
    class UnfriendCommand : Core.Connection.ICommand
    {
        public IAgent Agent { get; private set; }

        public UnfriendCommand(IAgent agent)
        {
            Agent = agent;
        }

        public byte[] ToByteArray()
        {
            return Agent.ToByteArray();
        }



        public static Core.Connection.IPacket FromByteArray(byte[] data)
        {
            using (var stream = new MemoryStream(data))
            {
                return new UnfriendCommand(Client.Agent.Implementation.FromStream(stream));
            }
        }
    }
}
