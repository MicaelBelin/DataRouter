using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Client.Forum.Interface.User
{
    //A packet is available until expiry hits, it is downloaded or ignored.
    public interface IMessage : Forum.IMessage
    {
        bool IsRead { get; }
        Task SetReadFlagAsync(bool isread);

        /// <summary>
        /// Send a response on the packet. Only one response is allowed. If multiple responses are sent, an exception is thrown.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task RespondAsync(byte[] data);

    }
}
