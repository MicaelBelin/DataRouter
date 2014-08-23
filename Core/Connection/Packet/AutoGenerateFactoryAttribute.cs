using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Core.Connection.Packet
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>class must implement a static method defined as: 
    /// public static IPacket FromByteArray(byte[] data)
    /// </remarks>
    public class AutoGenerateFactoryAttribute : Attribute
    {
    }
}
