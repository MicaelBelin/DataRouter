using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Core
{
    public interface ISession : IDisposable
    {
        Task<IAgent> HandShake(string passphrase, TimeSpan timeout);

        Task<Session.IFriendRequestListener> OpenFriendRequestListener(string channelname);
        Task<IAgent> SendFriendRequest(string channelname, TimeSpan timeout);


        Task<IEnumerable<IAgent>> Friends { get; }
        Task Unfriend(IAgent agent);


        Task<Stream> CreateStreamAsync(IAgent endpoint, TimeSpan timeout);
        event Action<Session.IStreamRequest> OnStreamRequest;


        Task<Forum.Interface.IAdmin> Create(string label);
        Task<IEnumerable<Forum.Interface.IAdmin>> OwnedForums { get; }

        Task<IEnumerable<Forum.Interface.IUser>> Forums { get; }
        event Action<Forum.Interface.IUser> OnInvited;
        Task<Forum.Interface.IUser> RequestInvitation(string label, TimeSpan timeout);

        

        

    }
}
