using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Core.Connection
{
    public interface ICommand : IPacket
    {
    }


    namespace Command
    {
        public enum FilterResult
        {
            Consume,
            PassOnToNext,
        }
    }
}
