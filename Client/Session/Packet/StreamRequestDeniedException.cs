using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Client.Session.Packet
{
    [Core.Connection.Packet.AutoGenerateFactory]
    public class StreamRequestDeniedException : Core.Connection.Packet.Exception
    {
        public StreamRequestDeniedException()
            : base()
        {
        }

        public StreamRequestDeniedException(string msg)
            : base(msg)
        {
        }

        public static new Core.Connection.IPacket FromByteArray(byte[] data)
        {
            return Core.Connection.Packet.Exception.FromByteArray(data);
        }

    }



}
