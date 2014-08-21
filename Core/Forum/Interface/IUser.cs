using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Core.Forum.Interface
{
    public interface IUser : IDisposable
    {
        void Leave();
        event Action Kicked;
        event Action Removed; //Is sent if forum was removed by admin.

        Task<IEnumerable<User.IMessage>> Messages { get; }
        Task<IEnumerable<User.IMessage>> UnreadMessages { get; }
        event Action<User.IMessage> OnMessageAvailable;

        Task<User.IRequest> CreateRequest(byte[] data, DateTime expiry, Action<int> progresscallback = null);
        Task<IEnumerable<User.IRequest>> Requests { get; }
    }
}
