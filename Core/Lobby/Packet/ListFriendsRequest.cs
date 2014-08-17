using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Core.Lobby.Packet
{
    class ListFriendsRequest : Connection.IRequest
    {
        public Connection.Packet.Wrapping Wrapped
        {
            get {
                return new Connection.Packet.Wrapping(typeof(ListFriendsRequest).Name, new byte[] { });
            }
        }


        public class Factory : Connection.Packet.IFactory
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
    }
}
