using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Core.Forum.Interface
{
    public interface IAdmin
    {
        string Label { get; }
        Task Remove();

        Task Invite(IAgent agent);
        Task Kick(IAgent agent);
        Task<IEnumerable<IAgent>> Agents { get; }

        Task<Admin.IMessage> CreateMessage(byte[] data, TimeSpan lifetime, Action<int> progresscallback = null);
        Task<IEnumerable<Admin.IMessage>> Messages { get; }

        Task<IEnumerable<Admin.IRequest>> Requests { get; }
        event Action<Admin.IRequest> OnRequest;

        Task<IEnumerable<Admin.IInvitationRequest>> InvitationRequests { get; }
        event Action<Admin.IInvitationRequest> OnInvitationRequest;
        


    }
}
