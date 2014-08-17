using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Core.Connection
{
    public partial class Stream : System.IO.Stream
    {
        public long Id { get; private set; }
        public IConnection Connection { get; private set; }

        public Stream(long id, IConnection connection)
        {
            Id = id;
            Connection = connection;

            Connection.RegisterOnCommand<DataPacket>(cmd =>
                {
                    if (cmd.Id == Id)
                    {
                        AddToReadBuffer(cmd.Data);
                        return CommandFilterResult.Consume;
                    }
                    else return CommandFilterResult.PassOnToNext;
                });

        }


        object buffermutex = new object();
        byte[] buffer = new byte[]{};


        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override void Flush()
        {
        }

        public override long Length
        {
            get
            {
                throw new NotSupportedException("");
            }
        }

        public override long Position
        {
            get
            {
                throw new NotSupportedException("");
            }
            set
            {
                throw new NotSupportedException("");
            }
        }


        bool disposed = false;
        protected override void Dispose(bool disposing)
        {
            if (disposed) return;
            disposed = true;
            lock (buffermutex)
            {
                Monitor.PulseAll(buffermutex);
            }

            base.Dispose(disposing);
        }

        public override int Read(byte[] outbuf, int offset, int count)
        {
            lock(buffermutex)
            {
                while (buffer.Length == 0)
                {
                    if (disposed) return 0;
                    Monitor.Wait(buffermutex);
                }

                var bytestoread = Math.Min(count, buffer.Length);

                for (int num = 0; num < bytestoread; ++num)
                {
                    outbuf[offset + num] = buffer[num];
                }

                buffer = buffer.Skip(bytestoread).ToArray();

                return bytestoread;
            }
        }

        public void AddToReadBuffer(byte[] data)
        {
            lock (buffermutex)
            {
                buffer = buffer.Concat(data).ToArray();
                Monitor.Pulse(buffermutex);
            }
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            Connection.SendAsync(new DataPacket(Id,buffer.Skip(offset).Take(count).ToArray()));
        }

        public override long Seek(long offset, System.IO.SeekOrigin origin)
        {
            throw new NotSupportedException("");
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException("");
        }






    }
}
