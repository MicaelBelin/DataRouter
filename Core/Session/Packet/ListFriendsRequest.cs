using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Core.Session.Packet
{
    class ListFriendsRequest : Connection.IRequest
    {

        public byte[] ToByteArray()
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream,Encoding.UTF8,true))
            {
                return stream.ToArray();
            }
        }            

        public class FactoryImpl : Connection.Packet.IFactory
        {
            public string Type
            {
                get { return typeof(ListFriendsRequest).Name; }
            }

            public Connection.IPacket Create(byte[] data)
            {
                using (var stream = new MemoryStream())
                using (var reader = new BinaryReader(stream))
                {
                    return new ListFriendsRequest();
                }
            }
        }
        static FactoryImpl factory = new FactoryImpl();
        public Connection.Packet.IFactory Factory { get { return factory; } }
    }
}
