﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Core.Connection
{
    public partial class Stream
    {
        [Connection.Packet.AutoGenerateFactory]
        public class EOFPacket : Connection.ICommand
        {
            public long Id { get; private set; }

            public EOFPacket(long id)
            {
                Id = id;
            }

            public byte[] ToByteArray()
            {
                using (var stream = new MemoryStream())
                using (var writer = new BinaryWriter(stream, Encoding.UTF8, true))
                {
                    writer.Write(Id);
                    return stream.ToArray();
                }
            }


            public static Connection.IPacket FromByteArray(byte[] data)
            {
                using (var reader = new BinaryReader(new MemoryStream(data)))
                {
                    var id = reader.ReadInt64();
                    return new EOFPacket(id);
                }
            }
        }

    }
}
