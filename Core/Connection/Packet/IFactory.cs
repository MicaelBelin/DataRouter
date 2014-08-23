using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Core.Connection.Packet
{
    public interface IFactory
    {
        /// <summary>
        /// Returns true if specified packet is of my creation
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>
        bool IsMine(IPacket packet);
        string Name { get; }
        IPacket Create(byte[] data);
    }



}
