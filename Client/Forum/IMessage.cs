using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Client.Forum
{
    public interface IMessage : IDownloadInterface, IDisposable
    {
        DateTime Created { get; }
    }
}
