using System;
using Xunit;

namespace THNETII.InteropServices.Bitwise.Test
{
    public class Bitmask32BitsTest
    {
        [Theory]
        [InlineData(00, 0b0000_0000_0000_0000_0000_0000_0000_0000U)]
        [InlineData(01, 0b0000_0000_0000_0000_0000_0000_0000_0001U)]
        [InlineData(02, 0b0000_0000_0000_0000_0000_0000_0000_0011U)]
        [InlineData(03, 0b0000_0000_0000_0000_0000_0000_0000_0111U)]
        [InlineData(04, 0b0000_0000_0000_0000_0000_0000_0000_1111U)]
        [InlineData(05, 0b0000_0000_0000_0000_0000_0000_0001_1111U)]
        [InlineData(06, 0b0000_0000_0000_0000_0000_0000_0011_1111U)]
        [InlineData(07, 0b0000_0000_0000_0000_0000_0000_0111_1111U)]
        [InlineData(08, 0b0000_0000_0000_0000_0000_0000_1111_1111U)]
        [InlineData(09, 0b0000_0000_0000_0000_0000_0001_1111_1111U)]
        [InlineData(10, 0b0000_0000_0000_0000_0000_0011_1111_1111U)]
        [InlineData(11, 0b0000_0000_0000_0000_0000_0111_1111_1111U)]
        [InlineData(12, 0b0000_0000_0000_0000_0000_1111_1111_1111U)]
        [InlineData(13, 0b0000_0000_0000_0000_0001_1111_1111_1111U)]
        [InlineData(14, 0b0000_0000_0000_0000_0011_1111_1111_1111U)]
        [InlineData(15, 0b0000_0000_0000_0000_0111_1111_1111_1111U)]
        [InlineData(16, 0b0000_0000_0000_0000_1111_1111_1111_1111U)]
        [InlineData(17, 0b0000_0000_0000_0001_1111_1111_1111_1111U)]
        [InlineData(18, 0b0000_0000_0000_0011_1111_1111_1111_1111U)]
        [InlineData(19, 0b0000_0000_0000_0111_1111_1111_1111_1111U)]
        [InlineData(20, 0b0000_0000_0000_1111_1111_1111_1111_1111U)]
        [InlineData(21, 0b0000_0000_0001_1111_1111_1111_1111_1111U)]
        [InlineData(22, 0b0000_0000_0011_1111_1111_1111_1111_1111U)]
        [InlineData(23, 0b0000_0000_0111_1111_1111_1111_1111_1111U)]
        [InlineData(24, 0b0000_0000_1111_1111_1111_1111_1111_1111U)]
        [InlineData(25, 0b0000_0001_1111_1111_1111_1111_1111_1111U)]
        [InlineData(26, 0b0000_0011_1111_1111_1111_1111_1111_1111U)]
        [InlineData(27, 0b0000_0111_1111_1111_1111_1111_1111_1111U)]
        [InlineData(28, 0b0000_1111_1111_1111_1111_1111_1111_1111U)]
        [InlineData(29, 0b0001_1111_1111_1111_1111_1111_1111_1111U)]
        [InlineData(30, 0b0011_1111_1111_1111_1111_1111_1111_1111U)]
        [InlineData(31, 0b0111_1111_1111_1111_1111_1111_1111_1111U)]
        [InlineData(32, 0b1111_1111_1111_1111_1111_1111_1111_1111U)]
        public void LowerBitsUInt32(int count, uint expected)
        {
            Assert.Equal(expected, Bitmask.LowerBitsUInt32(count));
        }

        [Fact]
        public void LowerBitsUInt32ThrowsWithNegativeCount()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Bitmask.LowerBitsUInt32(-1);
            });
        }

        [Fact]
        public void LowerBitsUInt32ThrowsWithTooBigCount()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Bitmask.LowerBitsUInt32(sizeof(uint) * 8 + 1);
            });
        }

        [Theory]
        [InlineData(00, 0b0000_0000_0000_0000_0000_0000_0000_0000)]
        [InlineData(01, 0b0000_0000_0000_0000_0000_0000_0000_0001)]
        [InlineData(02, 0b0000_0000_0000_0000_0000_0000_0000_0011)]
        [InlineData(03, 0b0000_0000_0000_0000_0000_0000_0000_0111)]
        [InlineData(04, 0b0000_0000_0000_0000_0000_0000_0000_1111)]
        [InlineData(05, 0b0000_0000_0000_0000_0000_0000_0001_1111)]
        [InlineData(06, 0b0000_0000_0000_0000_0000_0000_0011_1111)]
        [InlineData(07, 0b0000_0000_0000_0000_0000_0000_0111_1111)]
        [InlineData(08, 0b0000_0000_0000_0000_0000_0000_1111_1111)]
        [InlineData(09, 0b0000_0000_0000_0000_0000_0001_1111_1111)]
        [InlineData(10, 0b0000_0000_0000_0000_0000_0011_1111_1111)]
        [InlineData(11, 0b0000_0000_0000_0000_0000_0111_1111_1111)]
        [InlineData(12, 0b0000_0000_0000_0000_0000_1111_1111_1111)]
        [InlineData(13, 0b0000_0000_0000_0000_0001_1111_1111_1111)]
        [InlineData(14, 0b0000_0000_0000_0000_0011_1111_1111_1111)]
        [InlineData(15, 0b0000_0000_0000_0000_0111_1111_1111_1111)]
        [InlineData(16, 0b0000_0000_0000_0000_1111_1111_1111_1111)]
        [InlineData(17, 0b0000_0000_0000_0001_1111_1111_1111_1111)]
        [InlineData(18, 0b0000_0000_0000_0011_1111_1111_1111_1111)]
        [InlineData(19, 0b0000_0000_0000_0111_1111_1111_1111_1111)]
        [InlineData(20, 0b0000_0000_0000_1111_1111_1111_1111_1111)]
        [InlineData(21, 0b0000_0000_0001_1111_1111_1111_1111_1111)]
        [InlineData(22, 0b0000_0000_0011_1111_1111_1111_1111_1111)]
        [InlineData(23, 0b0000_0000_0111_1111_1111_1111_1111_1111)]
        [InlineData(24, 0b0000_0000_1111_1111_1111_1111_1111_1111)]
        [InlineData(25, 0b0000_0001_1111_1111_1111_1111_1111_1111)]
        [InlineData(26, 0b0000_0011_1111_1111_1111_1111_1111_1111)]
        [InlineData(27, 0b0000_0111_1111_1111_1111_1111_1111_1111)]
        [InlineData(28, 0b0000_1111_1111_1111_1111_1111_1111_1111)]
        [InlineData(29, 0b0001_1111_1111_1111_1111_1111_1111_1111)]
        [InlineData(30, 0b0011_1111_1111_1111_1111_1111_1111_1111)]
        [InlineData(31, 0b0111_1111_1111_1111_1111_1111_1111_1111)]
        [InlineData(32, unchecked((int)0b1111_1111_1111_1111_1111_1111_1111_1111))]
        public void LowerBitsInt32(int count, int expected)
        {
            Assert.Equal(expected, Bitmask.LowerBitsInt32(count));
        }

        [Fact]
        public void LowerBitsInt32ThrowsWithNegativeCount()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Bitmask.LowerBitsInt32(-1);
            });
        }

        [Fact]
        public void LowerBitsInt32ThrowsWithTooBigCount()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Bitmask.LowerBitsInt32(sizeof(int) * 8 + 1);
            });
        }

        [Theory]
        [InlineData(00, unchecked((uint)0b0000_0000_0000_0000_0000_0000_0000_0000U))]
        [InlineData(01, unchecked((uint)0b1000_0000_0000_0000_0000_0000_0000_0000U))]
        [InlineData(02, unchecked((uint)0b1100_0000_0000_0000_0000_0000_0000_0000U))]
        [InlineData(03, unchecked((uint)0b1110_0000_0000_0000_0000_0000_0000_0000U))]
        [InlineData(04, unchecked((uint)0b1111_0000_0000_0000_0000_0000_0000_0000U))]
        [InlineData(05, unchecked((uint)0b1111_1000_0000_0000_0000_0000_0000_0000U))]
        [InlineData(06, unchecked((uint)0b1111_1100_0000_0000_0000_0000_0000_0000U))]
        [InlineData(07, unchecked((uint)0b1111_1110_0000_0000_0000_0000_0000_0000U))]
        [InlineData(08, unchecked((uint)0b1111_1111_0000_0000_0000_0000_0000_0000U))]
        [InlineData(09, unchecked((uint)0b1111_1111_1000_0000_0000_0000_0000_0000U))]
        [InlineData(10, unchecked((uint)0b1111_1111_1100_0000_0000_0000_0000_0000U))]
        [InlineData(11, unchecked((uint)0b1111_1111_1110_0000_0000_0000_0000_0000U))]
        [InlineData(12, unchecked((uint)0b1111_1111_1111_0000_0000_0000_0000_0000U))]
        [InlineData(13, unchecked((uint)0b1111_1111_1111_1000_0000_0000_0000_0000U))]
        [InlineData(14, unchecked((uint)0b1111_1111_1111_1100_0000_0000_0000_0000U))]
        [InlineData(15, unchecked((uint)0b1111_1111_1111_1110_0000_0000_0000_0000U))]
        [InlineData(16, unchecked((uint)0b1111_1111_1111_1111_0000_0000_0000_0000U))]
        [InlineData(17, unchecked((uint)0b1111_1111_1111_1111_1000_0000_0000_0000U))]
        [InlineData(18, unchecked((uint)0b1111_1111_1111_1111_1100_0000_0000_0000U))]
        [InlineData(19, unchecked((uint)0b1111_1111_1111_1111_1110_0000_0000_0000U))]
        [InlineData(20, unchecked((uint)0b1111_1111_1111_1111_1111_0000_0000_0000U))]
        [InlineData(21, unchecked((uint)0b1111_1111_1111_1111_1111_1000_0000_0000U))]
        [InlineData(22, unchecked((uint)0b1111_1111_1111_1111_1111_1100_0000_0000U))]
        [InlineData(23, unchecked((uint)0b1111_1111_1111_1111_1111_1110_0000_0000U))]
        [InlineData(24, unchecked((uint)0b1111_1111_1111_1111_1111_1111_0000_0000U))]
        [InlineData(25, unchecked((uint)0b1111_1111_1111_1111_1111_1111_1000_0000U))]
        [InlineData(26, unchecked((uint)0b1111_1111_1111_1111_1111_1111_1100_0000U))]
        [InlineData(27, unchecked((uint)0b1111_1111_1111_1111_1111_1111_1110_0000U))]
        [InlineData(28, unchecked((uint)0b1111_1111_1111_1111_1111_1111_1111_0000U))]
        [InlineData(29, unchecked((uint)0b1111_1111_1111_1111_1111_1111_1111_1000U))]
        [InlineData(30, unchecked((uint)0b1111_1111_1111_1111_1111_1111_1111_1100U))]
        [InlineData(31, unchecked((uint)0b1111_1111_1111_1111_1111_1111_1111_1110U))]
        [InlineData(32, unchecked((uint)0b1111_1111_1111_1111_1111_1111_1111_1111U))]
        public void HigherBitsUInt32(int count, uint expected)
        {
            Assert.Equal(expected, Bitmask.HigherBitsUInt32(count));
        }

        [Fact]
        public void HigherBitsUInt32ThrowsWithNegativeCount()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Bitmask.HigherBitsUInt32(-1);
            });
        }

        [Fact]
        public void HigherBitsUInt32ThrowsWithTooBigCount()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Bitmask.HigherBitsUInt32(sizeof(uint) * 8 + 1);
            });
        }

        [Theory]
        [InlineData(00, unchecked((int)0b0000_0000_0000_0000_0000_0000_0000_0000))]
        [InlineData(01, unchecked((int)0b1000_0000_0000_0000_0000_0000_0000_0000))]
        [InlineData(02, unchecked((int)0b1100_0000_0000_0000_0000_0000_0000_0000))]
        [InlineData(03, unchecked((int)0b1110_0000_0000_0000_0000_0000_0000_0000))]
        [InlineData(04, unchecked((int)0b1111_0000_0000_0000_0000_0000_0000_0000))]
        [InlineData(05, unchecked((int)0b1111_1000_0000_0000_0000_0000_0000_0000))]
        [InlineData(06, unchecked((int)0b1111_1100_0000_0000_0000_0000_0000_0000))]
        [InlineData(07, unchecked((int)0b1111_1110_0000_0000_0000_0000_0000_0000))]
        [InlineData(08, unchecked((int)0b1111_1111_0000_0000_0000_0000_0000_0000))]
        [InlineData(09, unchecked((int)0b1111_1111_1000_0000_0000_0000_0000_0000))]
        [InlineData(10, unchecked((int)0b1111_1111_1100_0000_0000_0000_0000_0000))]
        [InlineData(11, unchecked((int)0b1111_1111_1110_0000_0000_0000_0000_0000))]
        [InlineData(12, unchecked((int)0b1111_1111_1111_0000_0000_0000_0000_0000))]
        [InlineData(13, unchecked((int)0b1111_1111_1111_1000_0000_0000_0000_0000))]
        [InlineData(14, unchecked((int)0b1111_1111_1111_1100_0000_0000_0000_0000))]
        [InlineData(15, unchecked((int)0b1111_1111_1111_1110_0000_0000_0000_0000))]
        [InlineData(16, unchecked((int)0b1111_1111_1111_1111_0000_0000_0000_0000))]
        [InlineData(17, unchecked((int)0b1111_1111_1111_1111_1000_0000_0000_0000))]
        [InlineData(18, unchecked((int)0b1111_1111_1111_1111_1100_0000_0000_0000))]
        [InlineData(19, unchecked((int)0b1111_1111_1111_1111_1110_0000_0000_0000))]
        [InlineData(20, unchecked((int)0b1111_1111_1111_1111_1111_0000_0000_0000))]
        [InlineData(21, unchecked((int)0b1111_1111_1111_1111_1111_1000_0000_0000))]
        [InlineData(22, unchecked((int)0b1111_1111_1111_1111_1111_1100_0000_0000))]
        [InlineData(23, unchecked((int)0b1111_1111_1111_1111_1111_1110_0000_0000))]
        [InlineData(24, unchecked((int)0b1111_1111_1111_1111_1111_1111_0000_0000))]
        [InlineData(25, unchecked((int)0b1111_1111_1111_1111_1111_1111_1000_0000))]
        [InlineData(26, unchecked((int)0b1111_1111_1111_1111_1111_1111_1100_0000))]
        [InlineData(27, unchecked((int)0b1111_1111_1111_1111_1111_1111_1110_0000))]
        [InlineData(28, unchecked((int)0b1111_1111_1111_1111_1111_1111_1111_0000))]
        [InlineData(29, unchecked((int)0b1111_1111_1111_1111_1111_1111_1111_1000))]
        [InlineData(30, unchecked((int)0b1111_1111_1111_1111_1111_1111_1111_1100))]
        [InlineData(31, unchecked((int)0b1111_1111_1111_1111_1111_1111_1111_1110))]
        [InlineData(32, unchecked((int)0b1111_1111_1111_1111_1111_1111_1111_1111))]
        public void HigherBitsInt32(int count, int expected)
        {
            Assert.Equal(expected, Bitmask.HigherBitsInt32(count));
        }

        [Fact]
        public void HigherBitsInt32ThrowsWithNegativeCount()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Bitmask.HigherBitsInt32(-1);
            });
        }

        [Fact]
        public void HigherBitsInt32ThrowsWithTooBigCount()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Bitmask.HigherBitsInt32(sizeof(int) * 8 + 1);
            });
        }
    }
}
