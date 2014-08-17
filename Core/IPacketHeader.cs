using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Core
{
    //A packet is available until expiry hits, it is downloaded or ignored.
    public interface IPacketHeader
    {
        IAgent From { get; }
        DateTime Expiry { get; }
        int Length { get; }
        Task<byte[]> GetAsync(Action<int> progresstatus);
        Task<bool> Ignore();

    }
}
