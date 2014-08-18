using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Core.Session
{
    public interface IStreamRequest
    {
        IAgent Agent { get; }
        Stream Accept();
        void Ignore();
        DateTime Expiry { get; }
    }
}
