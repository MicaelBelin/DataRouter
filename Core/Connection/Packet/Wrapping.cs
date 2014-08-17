using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Core.Connection.Packet
{
    public class Wrapping
    {
        public string Type { get; private set; }
        public byte[] Content { get; private set; }

    
        public Wrapping(string type, byte[] content)
        {
            Type = type;
            Content = content;
        }

        public static Wrapping FromArray(byte[] array)
        {
            using (var ms = new MemoryStream(array))
            using (var reader = new BinaryReader(ms))
            {
                var type = reader.ReadString();
                var contentlength = reader.ReadInt32();
                var content = reader.ReadBytes(contentlength);
                return new Wrapping(type, content);
            }
        }

        public byte[] Serialized
        {
            get
            {
                using (var ms = new MemoryStream())
                using (var writer = new BinaryWriter(ms))
                {
                    writer.Write(Type);
                    writer.Write(Content.Length);
                    writer.Write(Content);
                    return ms.ToArray();
                }
            }
        }

    }
}
