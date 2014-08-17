using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Core.Lobby
{
    public interface IStreamRequest
    {
        IAgent Agent { get; }
        Stream Accept();
        void Ignore();
        TimeSpan EstimatedTimeout { get; }
    }
}
