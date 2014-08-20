﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Core.Session.Packet
{
    class CreateStreamResponse : Connection.IResponse
    {
        public long StreamId { get; private set; }


        public CreateStreamResponse(long streamid)
        {
            StreamId = streamid;
        }

        public byte[] ToByteArray()
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write(StreamId);
                return stream.ToArray();
            }
        }


        public class FactoryImpl : Connection.Packet.IFactory
        {
            public string Type
            {
                get { return typeof(CreateStreamResponse).Name; }
            }

            public Connection.IPacket Create(byte[] data)
            {
                using (var stream = new MemoryStream(data))
                using (var reader = new BinaryReader(stream))
                {
                    return new CreateStreamResponse(reader.ReadInt64());
                }
            }
        }
        static FactoryImpl factory = new FactoryImpl();
        public Connection.Packet.IFactory Factory { get { return factory; } }

    }
}
