using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Core.Session.Packet
{
    [Connection.Packet.AutoGenerateFactory]
    public class StreamRequestDeniedException : Connection.Packet.Exception
    {
        public StreamRequestDeniedException()
            : base()
        {
        }

        public StreamRequestDeniedException(string msg)
            : base(msg)
        {
        }

        public static new Connection.IPacket FromByteArray(byte[] data)
        {
            return Connection.Packet.Exception.FromByteArray(data);
        }

    }



}
