using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Core.Agent
{
    class Implementation : IAgent
    {
        public string Id {get; private set;}
        public string Name {get; private set;}

        public static IAgent FromArray(byte[] array)
        {
            return FromStream(new MemoryStream(array));
        }

        public static IAgent FromStream(Stream s)
        {
            using (var reader = new BinaryReader(s,Encoding.UTF8,true))
            {
                return new Implementation()
                {
                    Id = reader.ReadString(),
                    Name = reader.ReadString(),
                };
            }
        }

        public byte[] ToArray()
        {
            using (var ms = new MemoryStream())
            using (var writer = new BinaryWriter(ms))
            {
                writer.Write(Id);
                writer.Write(Name);
                return ms.ToArray();
            }
        }
    }
}
