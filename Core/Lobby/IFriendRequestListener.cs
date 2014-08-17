﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Core.Lobby
{
    public interface IFriendRequestListener
    {
        event Action<FriendRequestListener.IFriendRequest> OnFriendRequest;
        void Close();
    }
}
