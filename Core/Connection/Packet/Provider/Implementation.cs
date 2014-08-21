using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Core.Connection.Packet.Provider
{
    public class Implementation : IProvider
    {

        Dictionary<string, IFactory> factories = new Dictionary<string, IFactory>();

        public void RegisterFactory(IFactory factory)
        {
            factories[factory.Type] = factory;
        }

        public IPacket Create(string type, byte[] data)
        {
            return factories[type].Create(data);
        }

        public void Populate()
        {
            RegisterFactory(Connection.Stream.DataPacket.FactoryInstance);
        }

    }
}
