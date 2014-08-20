﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xintric.DataRouter.Core.Session.Packet
{
    public class HandshakeRequest : Connection.IRequest
    {
        public string Passphrase { get; private set; }
        public TimeSpan Timeout { get; private set; }

        public HandshakeRequest(string passphrase, TimeSpan timeout)
        {
            Passphrase = passphrase;
            Timeout = timeout;
        }

       

        public byte[] ToByteArray()
        {
            using (var ms = new MemoryStream())
            using (var writer = new BinaryWriter(ms))
            {
                writer.Write(Passphrase);
                writer.Write(Timeout.Ticks);
                return ms.ToArray();
            }
        }

        public class FactoryImpl : Connection.Packet.IFactory
        {
            public string Type
            {
                get { return typeof(HandshakeRequest).Name; }
            }

            public Connection.IPacket Create(byte[] data)
            {
                using (var ms = new MemoryStream(data))
                using (var reader = new BinaryReader(ms))
                {
                    return new HandshakeRequest(reader.ReadString(),TimeSpan.FromTicks(reader.ReadInt64()));
                }
            }
        }
        static FactoryImpl factory = new FactoryImpl();
        public Connection.Packet.IFactory Factory { get { return factory; } }


    }
}