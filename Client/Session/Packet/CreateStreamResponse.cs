﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Client.Session.Packet
{
    [Core.Connection.Packet.AutoGenerateFactory]
    class CreateStreamResponse : Core.Connection.IResponse
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


        public static Core.Connection.IPacket FromByteArray(byte[] data)
        {
            using (var stream = new MemoryStream(data))
            using (var reader = new BinaryReader(stream))
            {
                return new CreateStreamResponse(reader.ReadInt64());
            }
        }

    }
}
