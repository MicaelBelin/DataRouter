using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Core.Connection
{
    public partial class Stream
    {
        [Connection.Packet.AutoGenerateFactory]
        public class DataPacket : Connection.ICommand
        {
            public long Id { get; private set; }
            public byte[] Data { get; private set; }

            public DataPacket(long id, byte[] data)
            {
                Id = id;
                Data = data;
            }

            public byte[] ToByteArray()
            {
                using (var stream = new MemoryStream())
                using (var writer = new BinaryWriter(stream,Encoding.UTF8,true))
                {
                    writer.Write(Id);
                    writer.Write(Data.Length);
                    writer.Write(Data);
                    return stream.ToArray();
                }
            }


            public static Connection.IPacket FromByteArray(byte[] data)
            {
                using (var reader = new BinaryReader(new MemoryStream(data)))
                {
                    var id = reader.ReadInt64();
                    var length = reader.ReadInt32();
                    var bytes = reader.ReadBytes(length);
                    return new DataPacket(id, bytes);
                }
            }
        }
  
    }
}
