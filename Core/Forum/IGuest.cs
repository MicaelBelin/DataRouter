using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Core.Session
{
    public interface IGuest : IDisposable
    {
        void Leave();


        IEnumerable<Session.IPacketHeader> AvailablePackets { get; }
        event Action<Session.IPacketHeader> OnPacketAvailable;

        int MaximumRequestDataSize { get; }
        Task SendRequestAsync(string label, byte[] data, TimeSpan lifetime, Action<int> progresscallback = null);

    }
}
