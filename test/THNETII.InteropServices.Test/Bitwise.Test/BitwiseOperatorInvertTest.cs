using System;
using System.Linq;
using System.Runtime.InteropServices;
using Xunit;

namespace THNETII.InteropServices.Bitwise.Test
{
    public class BitwiseOperatorInvertTest
    {
        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        struct TestStruct24Bits
        {
            public short f1;
            public byte f2;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        struct TestStruct48Bits
        {
            public int f1;
            public short f2;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        struct TestStruct72Bits
        {
            public long f1;
            public byte f2;
        }

        [Fact]
        public void InvertDefaultInt64()
        {
            long test = default;
            var inverted = BitwiseOperator.Invert(test);
            Assert.Equal(~test, inverted);
        }

        [Fact]
        public void InvertDefaultUInt64()
        {
            ulong test = default;
            var inverted = BitwiseOperator.Invert(test);
            Assert.Equal(~test, inverted);
        }

        [Fact]
        public void InvertDefaultInt32()
        {
            int test = default;
            var inverted = BitwiseOperator.Invert(test);
            Assert.Equal(~test, inverted);
        }

        [Fact]
        public void InvertDefaultUInt32()
        {
            uint test = default;
            var inverted = BitwiseOperator.Invert(test);
            Assert.Equal(~test, inverted);
        }

        [Fact]
        public void InvertDefaultInt16()
        {
            short test = default;
            var inverted = BitwiseOperator.Invert(test);
            Assert.Equal((short)(~test), inverted);
        }

        [Fact]
        public void InvertDefaultUInt16()
        {
            ushort test = default;
            var inverted = BitwiseOperator.Invert(test);
            Assert.Equal((ushort)(~test), inverted);
        }

        [Fact]
        public void InvertDefaultInt8()
        {
            sbyte test = default;
            var inverted = BitwiseOperator.Invert(test);
            Assert.Equal((sbyte)(~test), inverted);
        }

        [Fact]
        public void InvertDefaultUInt8()
        {
            byte test = default;
            var inverted = BitwiseOperator.Invert(test);
            Assert.Equal((byte)(~test), inverted);
        }

        [Fact]
        public void InvertDefault24BitsStruct()
        {
            TestStruct24Bits test = default;
            var inverted = BitwiseOperator.Invert(test);

            Assert.Equal(unchecked((short)0xFFFF), inverted.f1);
            Assert.Equal(unchecked((byte)0xFF), inverted.f2);
        }

        [Fact]
        public void InvertDefault48BitsStruct()
        {
            TestStruct48Bits test = default;
            var inverted = BitwiseOperator.Invert(test);

            Assert.Equal(unchecked(-1), inverted.f1);
            Assert.Equal(unchecked((short)0xFFFF), inverted.f2);
        }

        [Fact]
        public void InvertDefault72BitsStruct()
        {
            TestStruct72Bits test = default;
            var inverted = BitwiseOperator.Invert(test);

            Assert.Equal(unchecked(-1L), inverted.f1);
            Assert.Equal(unchecked((byte)0xFF), inverted.f2);
        }
    }
}
