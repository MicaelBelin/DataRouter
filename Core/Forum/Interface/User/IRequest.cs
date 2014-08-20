﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Core.Forum.Interface.User
{
    public interface IRequest :IDisposable
    {
        IDownloadInterface Response { get; }
        event Action<IDownloadInterface> OnResponse;
    }
}