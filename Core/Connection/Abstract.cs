using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Core.Connection
{
    public abstract class Abstract : IConnection
    {
        protected abstract Task SendDataAsync(byte[] data);
        protected abstract byte[] ReceiveData(int bytestoreceive);
        public abstract void Dispose();


        protected async Task SendDataPacketAsync(byte[] data)
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write(data.Length);
                writer.Write(data);
                await SendDataAsync(stream.ToArray());
            }
        }

        public Connection.Packet.IProvider Provider { get; private set; }

        public Abstract(Connection.Packet.IProvider provider)
        {
            Provider = provider;
        }


        object idgeneratorlocker = new object();
        long lastid = 0;
        long GenerateId()
        {
            lock(idgeneratorlocker)
            {
                return ++lastid;
            }
        }

        public async Task SendAsync(ICommand command)
        {
            await SendDataPacketAsync(Wrap(command,GenerateId(),0));
        }

        public async Task<IResponse> SendAsync(IRequest packet, TimeSpan timeout)
        {
            var id = GenerateId();
            IResponse response = null;
            AutoResetEvent sync = new AutoResetEvent(false);
            lock (responsehandler)
            {
                responsehandler[id] = r =>
                    {
                        response = r;
                        sync.Set();
                    };
            }

            await SendDataPacketAsync(Wrap(packet, id, 0));

            bool gotresponse = sync.WaitOne(timeout);
            lock (responsehandler)
            {
                responsehandler.Remove(id);
            }
            if (gotresponse) return response;
            else
            {
                throw new TimeoutException();
            }
        }
        public Dictionary<long, Action<IResponse>> responsehandler = new Dictionary<long, Action<IResponse>>();


        public List<Func<ICommand, CommandFilterResult>> commandfilters = new List<Func<ICommand, CommandFilterResult>>();
        public void RegisterOnCommand(Func<ICommand, CommandFilterResult> cmd)
        {
            lock(commandfilters)
            {
                commandfilters.Insert(0, cmd);
            }
        }

        public void UnregisterOnCommand(Func<ICommand, CommandFilterResult> cmd)
        {
            lock (commandfilters)
            {
                commandfilters.Remove(cmd);
            }
        }






        public void RunCollector()
        {
            foreach(var packet in GetPacket())
            {
                Task.Run(() =>
                    {
                        if (packet.Packet is ICommand)
                        {
                            var command = packet.Packet as ICommand;
                            foreach (var filter in commandfilters)
                            {
                                try
                                {
                                    if (filter(command) == CommandFilterResult.Consume) break;
                                }
                                catch (Exception)
                                {
                                    //TODO: Log!
                                }
                            }
                        }
                        else if (packet.Packet is IRequest)
                        {
                            var request = packet.Packet as IRequest;
                            foreach (var filter in requestfilters)
                            {
                                try
                                {
                                    var ret = filter(request);
                                    if (ret != null)
                                    {
                                        SendDataPacketAsync(Wrap(ret, GenerateId(), packet.Id)).FireAndForget();
                                    }
                                }
                                catch (Exception)
                                {
                                    //TODO: Log!
                                }
                            }
                        }
                        else if (packet.Packet is IResponse)
                        {
                            var response = packet.Packet as IResponse;
                            lock(responsehandler)
                            {
                                if (!responsehandler.ContainsKey(packet.InResponseTo))
                                {
                                    //May happen if response took too long and request timed out. Simply ignore it.
                                    return;
                                }
                                responsehandler[packet.InResponseTo](response);
                            }
                        }
                    });
            }
        }

        IEnumerable<PacketInfo> GetPacket()
        {
            while (true)
            {
                var header = ReceiveData(sizeof(int));
                if (header == null || header.Length == 0) yield break;
                using (var headerstream = new MemoryStream(header))
                using (var headerreader = new BinaryReader(headerstream))
                {
                    var size = headerreader.ReadInt32();
                    var data = ReceiveData(size);
                    if (data == null || data.Length == 0) yield break;
                    yield return Unwrap(data);
                }
            }
        }

        /**
         * string type;
         * long id;
         * long inresponseto;
         * int datalength;
         * byte[datalength] data;
         */ 
        static byte[] Wrap(IPacket packet,long id, long inresponseto)
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write(packet.Factory.Type);
                var data = packet.ToByteArray();
                writer.Write(id);
                writer.Write(inresponseto);                
                writer.Write(data.Length);
                writer.Write(data);
                return stream.ToArray();
            }
        }

        /**
         * string type;
         * long id;
         * long inresponseto;
         * int datalength;
         * byte[datalength] data;
         */
        PacketInfo Unwrap(byte[] data)
        {
            using (var stream = new MemoryStream(data))
            using (var reader = new BinaryReader(stream))
            {
                var type = reader.ReadString();
                var id = reader.ReadInt64();
                var inresponseto = reader.ReadInt64();
                var size = reader.ReadInt32();
                var pdata = reader.ReadBytes(size);
                return new PacketInfo() { Packet = Provider.Create(type, pdata), Id = id, InResponseTo = inresponseto };
            }
        }

        class PacketInfo
        {
            public long Id { get; set; }
            public long InResponseTo { get; set; }
            public IPacket Packet { get; set; }
        }



        public List<Func<IRequest, IResponse>> requestfilters = new List<Func<IRequest, IResponse>>();
        public void RegisterOnRequest(Func<IRequest, IResponse> cmd)
        {
            lock (requestfilters)
            {
                requestfilters.Insert(0, cmd);
            }
        }

        public void UnregisterOnRequest(Func<IRequest, IResponse> cmd)
        {
            lock (requestfilters)
            {
                requestfilters.Remove(cmd);
            }
        }

    }
}
