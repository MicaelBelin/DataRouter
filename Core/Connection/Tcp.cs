using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Core.Connection
{
    public class Tcp : Abstract
    {
        public TcpClient Socket { get; private set; }

        public Tcp(TcpClient client, Packet.IProvider provider)
            : base(provider)
        {
            Socket = client;
        }

        protected override async Task SendDataAsync(byte[] data)
        {
            await Task.Run(() => Socket.Client.Send(data));
        }

        protected override byte[] ReceiveData(int bytestoreceive)
        {
            var ret = new byte[bytestoreceive];
            var bytesread = Socket.Client.Receive(ret);
            return ret.Take(bytesread).ToArray();
        }

        bool disposed = false;
        public override void Dispose()
        {
            if (disposed) return;
            disposed = true;
            Socket.Close();                        
        }

        ~Tcp()
        {
            Dispose();
        }
    }
}
