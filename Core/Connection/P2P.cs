using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Core.Connection
{
    public class P2P : Abstract
    {
        public P2P Link {get; set;}

        public P2P(Packet.IProvider p) : base(p)
        {
        }

        public static Tuple<P2P, P2P> GeneratePair(Packet.IProvider p)
        {
            var p1 = new P2P(p);
            var p2 = new P2P(p);
            p1.Link = p2;
            p2.Link = p1;
            return new Tuple<P2P, P2P>(p1, p2);
        }

        protected override async Task SendDataAsync(byte[] data)
        {
            await Task.Run(() =>
                {
                    lock (q)
                    {
                        foreach (var b in data) q.Enqueue(b);
                        Monitor.Pulse(q);
                    }
                });
        }

        Queue<byte> q = new Queue<byte>();
        protected override byte[] ReceiveData(int bytestoreceive)
        {
            if (bytestoreceive == 0) throw new ArgumentException("bytestorecieve cannot be 0","bytestorecieve");
            if (Link == null) return new byte[] { };
            lock (Link.q)
            {
                while (Link != null && Link.q != null && Link.q.Count == 0) Monitor.Wait(Link.q);
                if (Link == null || Link.q == null) return new byte[]{};
                var bytes = Math.Min(bytestoreceive, Link.q.Count);
                var ret = Enumerable.Range(0, bytes).Select(i => Link.q.Dequeue()).ToArray();
                return ret;
            }
        }

        bool disposed = false;
        public override void Dispose()
        {
            if (disposed) return;
            disposed = true;
            lock (q)
            {
                var tmp = q;
                q = null;
                if (Link != null) Link.Link = null;
                Link = null;
                Monitor.Pulse(tmp);
            }
        }

        ~P2P()
        {
            Dispose();
        }
    }
}
