using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Core
{
    public static class TaskExtensions
    {
        public static void FireAndForget(this Task t)
        {
        }

        public static void FireAndForget<T>(this Task<T> t)
        {
        }
    }
}
