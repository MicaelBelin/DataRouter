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

        string GetNameOf(IPacket packet); //Returns the name of the specific packet (same as its factory.Name)

        IPacket Create(string type, byte[] data);
        IEnumerable<IFactory> Factories { get; }
    }
}
