using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Core.SessionHeader
{
    public class Static : ISessionHeader
    {
        public string Name{get; set;}

        public IAgent Creator {get; set;}

        public DateTime Created { get; set;}


        public byte[] ToArray()
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write(Name);
                writer.Write(Creator.ToArray());
                writer.Write(Created.ToBinary());
                return stream.ToArray();
            }
        }

        public static Static FromArray(byte[] array)
        {
            using (var stream = new MemoryStream(array))
            {
                return FromStream(stream);
            }
        }

        public static Static FromStream(Stream s)
        {
            using (var reader = new BinaryReader(s, Encoding.UTF8, true))
            {
                var ret = new Static();
                ret.Name = reader.ReadString();
                ret.Creator = Agent.Implementation.FromStream(s);
                ret.Created = DateTime.FromBinary(reader.ReadInt64());
                return ret;
            }
        }

    }
}
