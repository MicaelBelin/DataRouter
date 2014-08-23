using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Core.Session.Packet
{
    [Connection.Packet.AutoGenerateFactory]
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


        public static Connection.IPacket FromByteArray(byte[] data)
        {
            using (var stream = new MemoryStream(data))
            using (var reader = new BinaryReader(stream))
            {
                return new ListFriendsResponse();
            }
        }

    }
}
