using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Client.Session
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>To deny request, simply dispose of the object.</remarks>
    public interface IStreamRequest : IDisposable
    {
        IAgent Agent { get; }
        Stream Accept();
        DateTime Expiry { get; }
    }
}
