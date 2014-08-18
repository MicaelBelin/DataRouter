using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Core.Session
{
    //A packet is available until expiry hits, it is downloaded or ignored.
    public interface IPacketHeader
    {
        DateTime Expiry { get; }
        int Length { get; }
        bool IsRead { get;}
        Task<byte[]> GetAsync(Action<int> progresstatus);
        Task SetReadFlagAsync(bool isread);

        int ResponseQuota { get; }
        /// <summary>
        /// Send a response on the packet. Only one response is allowed. If multiple responses are sent, an exception is thrown.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task RespondAsync(byte[] data);

    }
}
