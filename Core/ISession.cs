using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Core
{
    public interface ISession
    {
        Task<IAgent> HandShakeAsync(string passphrase, TimeSpan timeout);

        Task<Session.IFriendRequestListener> OpenFriendRequestListener(string channelname);
        Task<IAgent> SendFriendRequest(string channelname);


        Task<IEnumerable<IAgent>> Friends { get; }
        Task Unfriend(IAgent agent);

        Task<Stream> CreateStream(IAgent endpoint, TimeSpan timeout);
        event Action<Session.IStreamRequest> OnStreamRequest;



/*
        Task<IEnumerable<Session.IHeader>> Sessions { get; }
        event Action<Session.IHeader> OnInvitedToSession;


        Task<IEnumerable<OwnedSession.IHeader>> MySessions {get;}
        Task<IOwnedSession> Create(string name);
*/
        

    }
}
