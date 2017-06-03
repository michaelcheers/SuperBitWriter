using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Numerics;

namespace SuperBitWriter
{
    public class BitWriter : IDisposable
    {
        public Stream BaseStream;
        BigInteger _currentStream;
        public void Write(byte value, byte max = byte.MaxValue) => Write((BigInteger)value, max);
        public void Write(byte value, byte min, byte max) => Write((BigInteger)value, min, max);
        public void Write(sbyte value, sbyte max = sbyte.MinValue) => Write(unchecked((byte)value), unchecked((byte)max));
        public void Write(sbyte value, sbyte min, sbyte max) => Write(unchecked((byte)(value - min)), unchecked((byte)(max - min)));
        public void Write(uint value, uint max = uint.MaxValue) => Write(value, 0, max);
        public void Write(uint value, uint min, uint max) => Write((BigInteger)(value - min), max - min);
        public void Write(int value, int max = int.MinValue) => Write(unchecked((uint)value), unchecked((uint)max));
        public void Write(int value, int min, int max) => Write(unchecked((uint)value), unchecked((uint)min), unchecked((uint)max));
        public void Write(BigInteger value, BigInteger min, BigInteger max) => Write(value - min, max - min);
        public void Write(BigInteger value, BigInteger max)
        {
            if (value > max || value < 0)
                throw new ArithmeticException($"{nameof(value)}({value}) cannot be greater than {nameof(max)}({max}) or less than 0.");
            _currentStream *= max + 1;
            _currentStream += value;
        }
        public void Flush ()
        {
            var byteArray = _currentStream.ToByteArray();
            BaseStream.Write(byteArray, 0, byteArray.Length);
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

        public BitWriter (Stream baseStream, bool leaveOpen = false)
        {
            BaseStream = baseStream;
            LeaveOpen = leaveOpen;
        }
    }
}
