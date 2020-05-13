using System;
using System.IO;
using Object = System.Object;

namespace Trudograd.NuclearEdition
{
    public abstract class ProxyStream : Stream
    {
        protected readonly Stream _stream;

        protected ProxyStream(Stream stream)
        {
            _stream = stream;
        }

        public override Int64 Length => _stream.Length;

        public override Boolean CanRead => _stream.CanRead;
        public override Boolean CanSeek => _stream.CanSeek;
        public override Boolean CanWrite => _stream.CanWrite;
        public override Boolean CanTimeout => _stream.CanTimeout;

        public override Int64 Position
        {
            get => _stream.Position;
            set => _stream.Position = value;
        }

        public override Int32 ReadTimeout
        {
            get => _stream.ReadTimeout;
            set => _stream.ReadTimeout = value;
        }

        public override Int32 WriteTimeout
        {
            get => _stream.WriteTimeout;
            set => _stream.WriteTimeout = value;
        }

        public override void Flush() => _stream.Flush();
        public override void Close() => _stream.Close();

        public override Int64 Seek(Int64 offset, SeekOrigin origin) => _stream.Seek(offset, origin);
        public override void SetLength(Int64 value) => _stream.SetLength(value);

        public override Int32 ReadByte() => _stream.ReadByte();
        public override Int32 Read(Byte[] buffer, Int32 offset, Int32 count) => _stream.Read(buffer, offset, count);
        public override IAsyncResult BeginRead(Byte[] buffer, Int32 offset, Int32 count, AsyncCallback callback, Object state) => _stream.BeginRead(buffer, offset, count, callback, state);
        public override Int32 EndRead(IAsyncResult asyncResult) => _stream.EndRead(asyncResult);

        public override void WriteByte(Byte value) => _stream.WriteByte(value);
        public override void Write(Byte[] buffer, Int32 offset, Int32 count) => _stream.Write(buffer, offset, count);
        public override IAsyncResult BeginWrite(Byte[] buffer, Int32 offset, Int32 count, AsyncCallback callback, Object state) => _stream.BeginWrite(buffer, offset, count, callback, state);
        public override void EndWrite(IAsyncResult asyncResult) => _stream.EndWrite(asyncResult);

        protected override void Dispose(Boolean disposing)
        {
            if (disposing)
                _stream.Dispose();
        }
    }
}