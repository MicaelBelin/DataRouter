using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Core.Lobby.Packet
{
    class ListFriendsResponse : Connection.IResponse
    {
        public IEnumerable<IAgent> Friends { get; private set; }



        public Connection.Packet.Wrapping Wrapped
        {
            get
            {
                using (var stream = new MemoryStream())
                using (var writer = new BinaryWriter(stream))
                {
                    return new Connection.Packet.Wrapping(typeof(ListFriendsResponse).Name, stream.ToArray() );
                }
            }
        }


        public class Factory : Connection.Packet.IFactory
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

    }
}
