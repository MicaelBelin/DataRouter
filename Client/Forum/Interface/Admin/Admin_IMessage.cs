using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Client.Forum.Interface.Admin
{
    public interface IMessage : Forum.IMessage
    {
        void Remove();
    }
}
