using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Core
{
    public interface ILobby
    {
        Task<IAgent> HandShakeAsync(string passphrase, TimeSpan timeout);

        Task<Lobby.IFriendRequestListener> OpenFriendRequestListener(string channelname);
        Task<IAgent> SendFriendRequest(string channelname);


        Task<IEnumerable<IAgent>> Friends { get; }
        Task Unfriend(IAgent agent);

        Task<Stream> CreateStream(IAgent endpoint, TimeSpan timeout);
        event Action<Lobby.IStreamRequest> OnStreamRequest;


        Task<IEnumerable<ISessionHeader>> Sessions { get; }
        Task<ISession> Create(string name);


        event Action<ISessionHeader> OnInvited;

    }
}
