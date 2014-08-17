using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Core.Connection.Packet.Provider
{
    class Implementation : IProvider
    {

        Dictionary<string, IFactory> factories = new Dictionary<string, IFactory>();

        public void RegisterFactory(IFactory factory)
        {
            factories[factory.Type] = factory;
        }

        public IPacket Create(byte[] data)
        {
            var wrapping = Packet.Wrapping.FromArray(data);
            return factories[wrapping.Type].Create(wrapping.Content);
        }
    }
}
