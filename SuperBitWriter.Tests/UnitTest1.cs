using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace SuperBitWriter.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (BitWriter writer = new BitWriter(memoryStream, true))
                {
                    writer.Write(77, 77, 77);
                    writer.Write((byte)9, (byte)100);
                    writer.Write((sbyte)-8, (sbyte)-10, (sbyte)0);
                    writer.Write(10u);
                    writer.Write(10, -8, 100);
                }
                memoryStream.Seek(0, SeekOrigin.Begin);
                using (BitReader reader = new BitReader(memoryStream))
                {
                    Assert.AreEqual(10, reader.ReadInt(-8, 100));
                    Assert.AreEqual(10u, reader.ReadUInt());
                    Assert.AreEqual<sbyte>(-8, reader.ReadSByte(-10, 0));
                    Assert.AreEqual<byte>(9, reader.ReadByte(100));
                    Assert.AreEqual(77, reader.ReadInt(77, 77));
                }
            }
        }
    }
}
