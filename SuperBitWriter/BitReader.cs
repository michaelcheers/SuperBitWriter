using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Numerics;

namespace SuperBitWriter
{
    public class BitReader : IDisposable
    {
        public Stream BaseStream;
        BigInteger _reading;
        public byte ReadByte(byte max = byte.MaxValue) => (byte)ReadBigInteger(max);
        public byte ReadByte(byte min, byte max) => (byte)ReadBigInteger(min, max);
        public sbyte ReadSByte(sbyte max = sbyte.MinValue) => unchecked((sbyte)ReadByte(unchecked((byte)max)));
        public sbyte ReadSByte(sbyte min, sbyte max) => unchecked((sbyte)(unchecked((sbyte)(ReadByte(unchecked((byte)(max - min))))) + min));
        public uint ReadUInt(uint max = uint.MaxValue) => ReadUInt(0, max);
        public uint ReadUInt(uint min, uint max) => (uint)ReadBigInteger(min, max);
        public int ReadInt(int max = int.MinValue) => unchecked((int)ReadUInt(unchecked((uint)max)));
        public int ReadInt(int min, int max) => unchecked((int)ReadUInt(unchecked((uint)(max - min)))) + min;
        public BigInteger ReadBigInteger(BigInteger min, BigInteger max) => ReadBigInteger(max - min) + min;
        public BigInteger ReadBigInteger(BigInteger max)
        {
            if (max == 0)
                return BigInteger.Zero;
            var readBigInteger = _reading % (max + 1);
            _reading /= max + 1;
            return readBigInteger;
        }
        public void Flush ()
        {
            BaseStream.Flush();
        }
        public void Close ()
        {
            Flush();
            if (!LeaveOpen)
                BaseStream.Close();
        }
        public void Dispose ()
        {
            Close();
            if (!LeaveOpen)
                BaseStream.Dispose();
        }

        public bool LeaveOpen;

        public BitReader (Stream baseStream, bool leaveOpen = false)
        {
            BaseStream = baseStream;
            LeaveOpen = leaveOpen;
            byte[] buffer = new byte[baseStream.Length];
            baseStream.Read(buffer, 0, buffer.Length);
            _reading = new BigInteger(buffer);
        }
    }
}
