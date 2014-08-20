using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Core.Session.Packet
{
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



        public class FactoryImpl : Connection.Packet.IFactory
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
        static FactoryImpl factory = new FactoryImpl();
        public Connection.Packet.IFactory Factory { get { return factory; } }
    }
}
