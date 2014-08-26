using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Client.Session.Packet
{
    /*
        class Template : Connection.IRequest
        {
            public Connection.Packet.Wrapping Wrapped
            {
                get {
                using (var stream = new MemoryStream())
                using (var writer = new BinaryWriter(stream))
                {
                    return new Connection.Packet.Wrapping(typeof(Template).Name, stream.ToArray() );
                }
                }
            }


            public class Factory : Connection.Packet.IFactory
            {
                public string Type
                {
                    get { return typeof(Template).Name; }
                }

                public Connection.IPacket Create(byte[] data)
                {
                    using (var stream = new MemoryStream(data))
                    using (var reader = new BinaryReader(stream))
                    {
                        return new Template();
                    }
                }
            }
        }
  
 
    */
}
