using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Core.UnitTest
{
    class TestRequest : Core.Connection.IRequest
    {
        public string Message { get; private set; }

        public TestRequest(string msg)
        {
            this.Message = msg;
        }

        public Connection.Packet.IFactory Factory
        {
            get { return FactoryInstance; }
        }

        public byte[] ToByteArray()
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write(Message);
                return stream.ToArray();
            }
        }

        public class FactoryImpl : Core.Connection.Packet.IFactory
        {

            public string Name
            {
                get { return typeof(TestRequest).Name; }
            }

            public Connection.IPacket Create(byte[] data)
            {
                using (var stream = new MemoryStream(data))
                using (var reader = new BinaryReader(stream))
                {
                    return new TestRequest(reader.ReadString());
                }
            }

            public bool IsMine(Connection.IPacket packet)
            {
                return packet is TestRequest;
            }
        }
        public static FactoryImpl FactoryInstance = new FactoryImpl();



    }
}
