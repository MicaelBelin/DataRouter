using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Client
{
    public interface IDownloadInterface
    {
        DateTime Expiry { get; }
        int Length { get; }
        Task<byte[]> Data { get; }
        Task<byte[]> Get(Action<int> progresstatus);
    }
}
