using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Client
{
    public interface IAgent
    {
        long Id { get; }
        string Name { get; }

        byte[] ToByteArray();

    }
}
