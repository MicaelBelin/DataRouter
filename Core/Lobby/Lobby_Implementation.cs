using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Core.Lobby
{
    class Implementation : ILobby
    {
        public Implementation(IConnection connection)
        {
            Connection = connection;

            Connection.RegisterOnCommand<Packet.InvitedToSession>(invitation =>
                {
                    if (OnInvited != null) OnInvited(invitation.SessionHeader);
                    return CommandFilterResult.Consume;
                });
        }

        public IConnection Connection { get; private set; }

        public async Task<IAgent> HandShakeAsync(string passphrase, TimeSpan timeout)
        {
            var ret = await Connection.SendAsync(new Packet.HandshakeRequest(passphrase, timeout)) as Packet.HandshakeResponse;
            return ret.Agent;
        }




        public Task<IEnumerable<IAgent>> Friends
        {
            get 
            {
                return Task<IEnumerable<IAgent>>.Run(() =>
                    {
                        return (Connection.SendAsync(new Packet.ListFriendsRequest()).Result as Packet.ListFriendsResponse).Friends;
                    });
            }
        }

        public Task Unfriend(IAgent agent)
        {
            return Connection.SendAsync(new Packet.UnfriendCommand(agent));
        }

        public async Task<System.IO.Stream> CreateStream(IAgent endpoint, TimeSpan timeout)
        {
            var ret = await Connection.SendAsync(new Packet.CreateStreamRequest(endpoint, timeout)) as Packet.CreateStreamResponse;
            if (ret.StreamId == 0) return null;
            return new Connection.Stream(ret.StreamId, Connection);
        }

        public event Action<IStreamRequest> OnStreamRequest;



        public Task<IEnumerable<ISessionHeader>> Sessions
        {
            get { throw new NotImplementedException(); }
        }

        public Task<ISession> Create(string name)
        {
            throw new NotImplementedException();
        }

        public Task<ISession> Join(ISessionHeader session)
        {
            throw new NotImplementedException();
        }

        public event Action<ISessionHeader> OnInvited;




        public Task<IFriendRequestListener> OpenFriendRequestListener(string channelname)
        {
            throw new NotImplementedException();
        }

        public Task<IAgent> SendFriendRequest(string channelname)
        {
            throw new NotImplementedException();
        }
    }
}
