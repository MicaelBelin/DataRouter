using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Client.Forum.Interface.Admin
{
    public interface IRequest : IDownloadInterface
    {
        Task Respond(byte[] data, Action<int> progress = null);
        void Ignore();
    }
}
