using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Core.Connection.Packet
{
    public interface IProvider
    {
        void RegisterFactory(Packet.IFactory factory);

        IPacket Create(byte[] data);

    }
}
