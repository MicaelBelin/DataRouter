using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Core.Forum.Interface.Admin
{
    public interface IInvitationRequest
    {
        IAgent Agent { get; }
        void Accept();
        void Ignore();
    }
}
