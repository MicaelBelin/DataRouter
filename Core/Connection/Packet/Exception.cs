using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Core.Connection.Packet
{
    /// <summary>
    /// This class represents an exception packet. Objects of this class and any of its derivatives will be sent as the response should the response calculation throw an exception.
    /// </summary>
    [AutoGenerateFactory]
    public class Exception : System.Exception, IResponse
    {
        public Exception()
            : base()
        {
        }

        public Exception(string message)
            : base(message)
        {
        }

        public virtual byte[] ToByteArray()
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write(Message);
                return stream.ToArray();
            }
        }

        public static IPacket FromByteArray(byte[] data)
        {
            using (var stream = new MemoryStream(data))
            using (var reader = new BinaryReader(stream))
            {
                string msg = reader.ReadString();
                return new Exception(msg);
            }
        }
    }
}
