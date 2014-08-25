using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Core.Connection.Packet
{
    /// <summary>
    /// Is sent to the client telling the response routine failed to deliver a response.
    /// </summary>
    [Packet.AutoGenerateFactory]
    public class ResponseException : Exception
    {
        public ResponseException()
            : base("Recipient failed to generate a response")
        {
        }

        public ResponseException(string msg)
            : base(msg)
        {
        }

        public static new IPacket FromByteArray(byte[] data)
        {
            using (var stream = new MemoryStream(data))
            using (var reader = new BinaryReader(stream))
            {
                string msg = reader.ReadString();
                return new ResponseException(msg);
            }
        }
    }
}
