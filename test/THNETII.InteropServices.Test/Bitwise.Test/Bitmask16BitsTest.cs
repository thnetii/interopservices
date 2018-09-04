using System;
using Xunit;

namespace THNETII.InteropServices.Bitwise.Test
{
    public class Bitmask16BitsTest
    {
        [Theory]
        [InlineData(00, 0b0000_0000_0000_0000)]
        [InlineData(01, 0b0000_0000_0000_0001)]
        [InlineData(02, 0b0000_0000_0000_0011)]
        [InlineData(03, 0b0000_0000_0000_0111)]
        [InlineData(04, 0b0000_0000_0000_1111)]
        [InlineData(05, 0b0000_0000_0001_1111)]
        [InlineData(06, 0b0000_0000_0011_1111)]
        [InlineData(07, 0b0000_0000_0111_1111)]
        [InlineData(08, 0b0000_0000_1111_1111)]
        [InlineData(09, 0b0000_0001_1111_1111)]
        [InlineData(10, 0b0000_0011_1111_1111)]
        [InlineData(11, 0b0000_0111_1111_1111)]
        [InlineData(12, 0b0000_1111_1111_1111)]
        [InlineData(13, 0b0001_1111_1111_1111)]
        [InlineData(14, 0b0011_1111_1111_1111)]
        [InlineData(15, 0b0111_1111_1111_1111)]
        [InlineData(16, 0b1111_1111_1111_1111)]
        public void LowerBitsUInt16(int count, ushort expected)
        {
            Assert.Equal(expected, Bitmask.LowerBitsUInt16(count));
        }

        [Fact]
        public void LowerBitsUInt16ThrowsWithNegativeCount()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Bitmask.LowerBitsUInt16(-1);
            });
        }

        [Fact]
        public void LowerBitsUInt16ThrowsWithTooBigCount()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Bitmask.LowerBitsUInt16(sizeof(ushort) * 8 + 1);
            });
        }

        [Theory]
        [InlineData(00, 0b0000_0000_0000_0000)]
        [InlineData(01, 0b0000_0000_0000_0001)]
        [InlineData(02, 0b0000_0000_0000_0011)]
        [InlineData(03, 0b0000_0000_0000_0111)]
        [InlineData(04, 0b0000_0000_0000_1111)]
        [InlineData(05, 0b0000_0000_0001_1111)]
        [InlineData(06, 0b0000_0000_0011_1111)]
        [InlineData(07, 0b0000_0000_0111_1111)]
        [InlineData(08, 0b0000_0000_1111_1111)]
        [InlineData(09, 0b0000_0001_1111_1111)]
        [InlineData(10, 0b0000_0011_1111_1111)]
        [InlineData(11, 0b0000_0111_1111_1111)]
        [InlineData(12, 0b0000_1111_1111_1111)]
        [InlineData(13, 0b0001_1111_1111_1111)]
        [InlineData(14, 0b0011_1111_1111_1111)]
        [InlineData(15, 0b0111_1111_1111_1111)]
        [InlineData(16, unchecked((short)0b1111_1111_1111_1111))]
        public void LowerBitsInt16(int count, short expected)
        {
            Assert.Equal(expected, Bitmask.LowerBitsInt16(count));
        }

        [Fact]
        public void LowerBitsInt16ThrowsWithNegativeCount()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Bitmask.LowerBitsInt16(-1);
            });
        }

        [Fact]
        public void LowerBitsInt16ThrowsWithTooBigCount()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Bitmask.LowerBitsInt16(sizeof(short) * 8 + 1);
            });
        }

        [Theory]
        [InlineData(00, unchecked((ushort)0b0000_0000_0000_0000))]
        [InlineData(01, unchecked((ushort)0b1000_0000_0000_0000))]
        [InlineData(02, unchecked((ushort)0b1100_0000_0000_0000))]
        [InlineData(03, unchecked((ushort)0b1110_0000_0000_0000))]
        [InlineData(04, unchecked((ushort)0b1111_0000_0000_0000))]
        [InlineData(05, unchecked((ushort)0b1111_1000_0000_0000))]
        [InlineData(06, unchecked((ushort)0b1111_1100_0000_0000))]
        [InlineData(07, unchecked((ushort)0b1111_1110_0000_0000))]
        [InlineData(08, unchecked((ushort)0b1111_1111_0000_0000))]
        [InlineData(09, unchecked((ushort)0b1111_1111_1000_0000))]
        [InlineData(10, unchecked((ushort)0b1111_1111_1100_0000))]
        [InlineData(11, unchecked((ushort)0b1111_1111_1110_0000))]
        [InlineData(12, unchecked((ushort)0b1111_1111_1111_0000))]
        [InlineData(13, unchecked((ushort)0b1111_1111_1111_1000))]
        [InlineData(14, unchecked((ushort)0b1111_1111_1111_1100))]
        [InlineData(15, unchecked((ushort)0b1111_1111_1111_1110))]
        [InlineData(16, unchecked((ushort)0b1111_1111_1111_1111))]
        public void HigherBitsUInt16(int count, ushort expected)
        {
            Assert.Equal(expected, Bitmask.HigherBitsUInt16(count));
        }

        [Fact]
        public void HigherBitsUInt16ThrowsWithNegativeCount()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Bitmask.HigherBitsUInt16(-1);
            });
        }

        [Fact]
        public void HigherBitsUInt16ThrowsWithTooBigCount()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Bitmask.HigherBitsUInt16(sizeof(ushort) * 8 + 1);
            });
        }

        [Theory]
        [InlineData(00, unchecked((short)0b0000_0000_0000_0000))]
        [InlineData(01, unchecked((short)0b1000_0000_0000_0000))]
        [InlineData(02, unchecked((short)0b1100_0000_0000_0000))]
        [InlineData(03, unchecked((short)0b1110_0000_0000_0000))]
        [InlineData(04, unchecked((short)0b1111_0000_0000_0000))]
        [InlineData(05, unchecked((short)0b1111_1000_0000_0000))]
        [InlineData(06, unchecked((short)0b1111_1100_0000_0000))]
        [InlineData(07, unchecked((short)0b1111_1110_0000_0000))]
        [InlineData(08, unchecked((short)0b1111_1111_0000_0000))]
        [InlineData(09, unchecked((short)0b1111_1111_1000_0000))]
        [InlineData(10, unchecked((short)0b1111_1111_1100_0000))]
        [InlineData(11, unchecked((short)0b1111_1111_1110_0000))]
        [InlineData(12, unchecked((short)0b1111_1111_1111_0000))]
        [InlineData(13, unchecked((short)0b1111_1111_1111_1000))]
        [InlineData(14, unchecked((short)0b1111_1111_1111_1100))]
        [InlineData(15, unchecked((short)0b1111_1111_1111_1110))]
        [InlineData(16, unchecked((short)0b1111_1111_1111_1111))]
        public void HigherBitsInt16(int count, short expected)
        {
            Assert.Equal(expected, Bitmask.HigherBitsInt16(count));
        }

        [Fact]
        public void HigherBitsInt16ThrowsWithNegativeCount()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Bitmask.HigherBitsInt16(-1);
            });
        }

        [Fact]
        public void HigherBitsInt16ThrowsWithTooBigCount()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Bitmask.HigherBitsInt16(sizeof(short) * 8 + 1);
            });
        }
    }
}
