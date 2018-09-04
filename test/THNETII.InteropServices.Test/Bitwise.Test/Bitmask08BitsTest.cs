using System;
using Xunit;

namespace THNETII.InteropServices.Bitwise.Test
{
    public class Bitmask08BitsTest
    {
        [Theory]
        [InlineData(0, 0b0000_0000)]
        [InlineData(1, 0b0000_0001)]
        [InlineData(2, 0b0000_0011)]
        [InlineData(3, 0b0000_0111)]
        [InlineData(4, 0b0000_1111)]
        [InlineData(5, 0b0001_1111)]
        [InlineData(6, 0b0011_1111)]
        [InlineData(7, 0b0111_1111)]
        [InlineData(8, 0b1111_1111)]
        public void LowerBitsUInt8(int count, byte expected)
        {
            Assert.Equal(expected, Bitmask.LowerBitsUInt8(count));
        }

        [Fact]
        public void LowerBitsUInt8ThrowsWithNegativeCount()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Bitmask.LowerBitsUInt8(-1);
            });
        }

        [Fact]
        public void LowerBitsUInt8ThrowsWithTooBigCount()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Bitmask.LowerBitsUInt8(sizeof(byte) * 8 + 1);
            });
        }

        [Theory]
        [InlineData(0, 0b0000_0000)]
        [InlineData(1, 0b0000_0001)]
        [InlineData(2, 0b0000_0011)]
        [InlineData(3, 0b0000_0111)]
        [InlineData(4, 0b0000_1111)]
        [InlineData(5, 0b0001_1111)]
        [InlineData(6, 0b0011_1111)]
        [InlineData(7, 0b0111_1111)]
        [InlineData(8, unchecked((sbyte)0b1111_1111))]
        public void LowerBitsInt8(int count, sbyte expected)
        {
            Assert.Equal(expected, Bitmask.LowerBitsInt8(count));
        }

        [Fact]
        public void LowerBitsInt8ThrowsWithNegativeCount()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Bitmask.LowerBitsInt8(-1);
            });
        }

        [Fact]
        public void LowerBitsInt8ThrowsWithTooBigCount()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Bitmask.LowerBitsInt8(sizeof(sbyte) * 8 + 1);
            });
        }

        [Theory]
        [InlineData(00, unchecked((byte)0b0000_0000))]
        [InlineData(01, unchecked((byte)0b1000_0000))]
        [InlineData(02, unchecked((byte)0b1100_0000))]
        [InlineData(03, unchecked((byte)0b1110_0000))]
        [InlineData(04, unchecked((byte)0b1111_0000))]
        [InlineData(05, unchecked((byte)0b1111_1000))]
        [InlineData(06, unchecked((byte)0b1111_1100))]
        [InlineData(07, unchecked((byte)0b1111_1110))]
        [InlineData(08, unchecked((byte)0b1111_1111))]
        public void HigherBitsUInt8(int count, byte expected)
        {
            Assert.Equal(expected, Bitmask.HigherBitsUInt8(count));
        }

        [Fact]
        public void HigherBitsUInt8ThrowsWithNegativeCount()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Bitmask.HigherBitsUInt8(-1);
            });
        }

        [Fact]
        public void HigherBitsUInt8ThrowsWithTooBigCount()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Bitmask.HigherBitsUInt8(sizeof(byte) * 8 + 1);
            });
        }

        [Theory]
        [InlineData(00, unchecked((sbyte)0b0000_0000))]
        [InlineData(01, unchecked((sbyte)0b1000_0000))]
        [InlineData(02, unchecked((sbyte)0b1100_0000))]
        [InlineData(03, unchecked((sbyte)0b1110_0000))]
        [InlineData(04, unchecked((sbyte)0b1111_0000))]
        [InlineData(05, unchecked((sbyte)0b1111_1000))]
        [InlineData(06, unchecked((sbyte)0b1111_1100))]
        [InlineData(07, unchecked((sbyte)0b1111_1110))]
        [InlineData(08, unchecked((sbyte)0b1111_1111))]
        public void HigherBitsInt8(int count, sbyte expected)
        {
            Assert.Equal(expected, Bitmask.HigherBitsInt8(count));
        }

        [Fact]
        public void HigherBitsInt8ThrowsWithNegativeCount()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Bitmask.HigherBitsInt8(-1);
            });
        }

        [Fact]
        public void HigherBitsInt8ThrowsWithTooBigCount()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Bitmask.HigherBitsInt8(sizeof(sbyte) * 8 + 1);
            });
        }
    }
}
