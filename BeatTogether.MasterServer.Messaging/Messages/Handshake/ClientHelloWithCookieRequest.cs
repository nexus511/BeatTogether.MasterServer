﻿using BeatTogether.Core.Messaging.Abstractions;
using Krypton.Buffers;

namespace BeatTogether.MasterServer.Messaging.Messages.Handshake
{
    public class ClientHelloWithCookieRequest : IMessage, IReliableRequest
    {
        public uint RequestId { get; set; }
        public uint CertificateResponseId { get; set; }
        public byte[] Random { get; set; }
        public byte[] Cookie { get; set; }

        public void WriteTo(ref GrowingSpanBuffer buffer)
        {
            buffer.WriteUInt32(CertificateResponseId);
            buffer.WriteBytes(Random);
            buffer.WriteBytes(Cookie);
        }

        public void ReadFrom(ref SpanBufferReader bufferReader)
        {
            CertificateResponseId = bufferReader.ReadUInt32();
            Random = bufferReader.ReadBytes(32).ToArray();
            Cookie = bufferReader.ReadBytes(32).ToArray();
        }
    }
}