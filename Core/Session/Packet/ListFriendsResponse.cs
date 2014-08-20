using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Core.Session.Packet
{
    class ListFriendsResponse : Connection.IResponse
    {
        public IEnumerable<IAgent> Friends { get; private set; }


        public byte[] ToByteArray()
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                return stream.ToArray();
            }
        }


        public class FactoryImpl : Connection.Packet.IFactory
        {
            public string Type
            {
                get { return typeof(ListFriendsResponse).Name; }
            }

            public Connection.IPacket Create(byte[] data)
            {
                using (var stream = new MemoryStream(data))
                using (var reader = new BinaryReader(stream))
                {
                    return new ListFriendsResponse();
                }
            }
        }
        static FactoryImpl factory = new FactoryImpl();
        public Connection.Packet.IFactory Factory { get { return factory; } }

    }
}
