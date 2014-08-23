using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Core.Session.Packet
{

    [Connection.Packet.AutoGenerateFactory]
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

        public static Connection.IPacket FromByteArray(byte[] data)
        {
            using (var stream = new MemoryStream())
            using (var reader = new BinaryReader(stream))
            {
                return new ListFriendsRequest();
            }
        }


    }
}
