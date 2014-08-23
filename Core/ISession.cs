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
        /// <summary>
        /// Connects to and registers a friend by specifying a mutual handshake phrase. Two agents sending the same handshake during the same timespan will get connected.
        /// </summary>
        /// <param name="passphrase"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        Task<IAgent> HandShake(string passphrase, TimeSpan timeout);

        /// <summary>
        /// Opens a channel which others can connect to and send a friend request.
        /// </summary>
        /// <param name="channelname"></param>
        /// <returns></returns>
        Task<Session.IFriendRequestListener> OpenFriendRequestListener(string channelname);

        /// <summary>
        /// Sends a friend request to the specified channel.
        /// </summary>
        /// <param name="channelname"></param>
        /// <param name="timeout"></param>
        /// <returns>The new friend</returns>
        /// <exception cref="OperationCanceledException">Is thrown if target agent rejects the invitation</exception>
        Task<IAgent> SendFriendRequest(string channelname, TimeSpan timeout);

        /// <summary>
        /// Lists all friends
        /// </summary>
        Task<IEnumerable<IAgent>> Friends { get; }

        /// <summary>
        /// Unfriends the specified agent
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        Task Unfriend(IAgent agent);


        /// <summary>
        /// Sends a stream request to agent and returns a stream object to it
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="timeout"></param>
        /// <exception cref="OperationCanceledException">Is thrown if target agent rejects the stream connection</exception>
        /// <returns></returns>
        Task<Stream> CreateStreamAsync(IAgent endpoint, TimeSpan timeout);

        /// <summary>
        /// Is called if a stream request is made
        /// </summary>
        Action<Session.IStreamRequest> OnStreamRequest { get; set; }


        /// <summary>
        /// Creates a new forum, with self as owner.
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        Task<Forum.Interface.IAdmin> Create(string label);
        /// <summary>
        /// Lists all forums owned by self
        /// </summary>
        Task<IEnumerable<Forum.Interface.IAdmin>> OwnedForums { get; }

        /// <summary>
        /// Lists all forums self is member of
        /// </summary>
        Task<IEnumerable<Forum.Interface.IUser>> Forums { get; }
        /// <summary>
        /// Is called if self was invited to a forum.
        /// </summary>
        event Action<Forum.Interface.IUser> OnInvited;
        /// <summary>
        /// Sends an invitation request to the forum with the specified label
        /// </summary>
        /// <param name="label"></param>
        /// <param name="timeout"></param>
        /// <exception cref="TimoutException">Is thrown if request was not responded to in time</exception>
        /// <exception cref="OperationCanceledException">Is thrown if request was rejected.</exception>
        /// <returns></returns>
        Task<Forum.Interface.IUser> RequestInvitation(string label, TimeSpan timeout);

        

        

    }
}
