using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Core.Session
{
    public interface IStreamRequest : IDisposable
    {
        IAgent Agent { get; }
        Stream Accept();
        DateTime Expiry { get; }
    }
}
