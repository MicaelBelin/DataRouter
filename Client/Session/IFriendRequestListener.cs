﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Client.Session
{
    public interface IFriendRequestListener : IDisposable
    {
        event Action<FriendRequestListener.IFriendRequest> OnFriendRequest;
    }
}
