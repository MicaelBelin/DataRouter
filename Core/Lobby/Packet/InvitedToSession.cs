using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Core.Lobby.Packet
{
    class InvitedToSession : Connection.ICommand
    {
        public ISessionHeader SessionHeader { get; private set; }

        public InvitedToSession(ISessionHeader sessionheader)
        {
            SessionHeader = SessionHeader;
        }


        public Connection.Packet.Wrapping Wrapped
        {
            get 
            {
                using (var ms = new MemoryStream())
                using (var writer = new BinaryWriter(ms))
                {
                    writer.Write(SessionHeader.ToArray());
                    return new Connection.Packet.Wrapping(typeof(HandshakeRequest).Name, ms.ToArray());
                }

            }
        }

        public class Factory : Connection.Packet.IFactory
        {
            public string Type
            {
                get { return typeof(Factory).Name; }
            }

            public Connection.IPacket Create(byte[] data)
            {
                return new InvitedToSession(Core.SessionHeader.Static.FromArray(data));
            }
        }

    }
}
