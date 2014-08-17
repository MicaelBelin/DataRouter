using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Core
{
    public interface ISessionHeader
    {
        string Name { get; }
        IAgent Creator { get; }
        DateTime Created { get; }

        byte[] ToArray();

    }
}
