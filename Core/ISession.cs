using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Core
{
    public interface ISession : ISessionHeader, IDisposable
    {

        IEnumerable<IAgent> Users { get; }
        void Invite(IAgent agent);
        void Leave();

        Task<bool> SendPacketAsync(string header, byte[] data, TimeSpan lifetime, Action<int> progresscallback = null);

        IEnumerable<IPacketHeader> AvailablePackets { get; }

        event Action<IPacketHeader> OnPacketAvailable;

    }
}
