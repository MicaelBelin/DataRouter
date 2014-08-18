using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Core.Session
{
    public interface IOwner
    {
        void Invite(IAgent agent);
        Task<bool> SendPacketAsync(string label, byte[] data, TimeSpan lifetime, Action<int> progresscallback = null);



    }
}
