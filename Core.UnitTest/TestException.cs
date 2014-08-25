using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Core.UnitTest
{
    [Connection.Packet.AutoGenerateFactory]
    public class TestException : Connection.Packet.Exception
    {
        public TestException()
            : base("This is a test exception!")
        {
        }

        public override byte[] ToByteArray()
        {
            return new byte[]{};
        }

        public static new Connection.IPacket FromByteArray(byte[] b)
        {
            return new TestException();
        }

    }
}
