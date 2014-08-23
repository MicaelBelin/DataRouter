using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Core.UnitTest
{
    
        class TestResponse : Core.Connection.IResponse
        {
            public string Message {get; private set;}

            public TestResponse(string msg)
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
                    get { return typeof(TestResponse).Name; }
                }

                public Connection.IPacket Create(byte[] data)
                {
                    using (var stream = new MemoryStream(data))
                    using (var reader = new BinaryReader(stream))
                    {
                        return new TestResponse(reader.ReadString());
                    }
                }

                public bool IsMine(Connection.IPacket packet)
                {
                    return packet is TestResponse;
                }
            }
            public static FactoryImpl FactoryInstance = new FactoryImpl();


        }
     
}
