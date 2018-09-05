﻿using System;
using Xunit;

namespace THNETII.InteropServices.Bitwise.Test
{
    public class Bitmask64BitsTest
    {
        [Theory]
        [InlineData(00, 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000UL)]
        [InlineData(01, 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0001UL)]
        [InlineData(02, 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0011UL)]
        [InlineData(03, 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0111UL)]
        [InlineData(04, 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_1111UL)]
        [InlineData(05, 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0001_1111UL)]
        [InlineData(06, 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0011_1111UL)]
        [InlineData(07, 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0111_1111UL)]
        [InlineData(08, 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_1111_1111UL)]
        [InlineData(09, 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0001_1111_1111UL)]
        [InlineData(10, 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0011_1111_1111UL)]
        [InlineData(11, 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0111_1111_1111UL)]
        [InlineData(12, 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_1111_1111_1111UL)]
        [InlineData(13, 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0001_1111_1111_1111UL)]
        [InlineData(14, 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0011_1111_1111_1111UL)]
        [InlineData(15, 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0111_1111_1111_1111UL)]
        [InlineData(16, 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_1111_1111_1111_1111UL)]
        [InlineData(17, 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0001_1111_1111_1111_1111UL)]
        [InlineData(18, 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0011_1111_1111_1111_1111UL)]
        [InlineData(19, 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0111_1111_1111_1111_1111UL)]
        [InlineData(20, 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_1111_1111_1111_1111_1111UL)]
        [InlineData(21, 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0001_1111_1111_1111_1111_1111UL)]
        [InlineData(22, 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0011_1111_1111_1111_1111_1111UL)]
        [InlineData(23, 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0111_1111_1111_1111_1111_1111UL)]
        [InlineData(24, 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_1111_1111_1111_1111_1111_1111UL)]
        [InlineData(25, 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0001_1111_1111_1111_1111_1111_1111UL)]
        [InlineData(26, 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0011_1111_1111_1111_1111_1111_1111UL)]
        [InlineData(27, 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0111_1111_1111_1111_1111_1111_1111UL)]
        [InlineData(28, 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_1111_1111_1111_1111_1111_1111_1111UL)]
        [InlineData(29, 0b0000_0000_0000_0000_0000_0000_0000_0000_0001_1111_1111_1111_1111_1111_1111_1111UL)]
        [InlineData(30, 0b0000_0000_0000_0000_0000_0000_0000_0000_0011_1111_1111_1111_1111_1111_1111_1111UL)]
        [InlineData(31, 0b0000_0000_0000_0000_0000_0000_0000_0000_0111_1111_1111_1111_1111_1111_1111_1111UL)]
        [InlineData(32, 0b0000_0000_0000_0000_0000_0000_0000_0000_1111_1111_1111_1111_1111_1111_1111_1111UL)]
        [InlineData(33, 0b0000_0000_0000_0000_0000_0000_0000_0001_1111_1111_1111_1111_1111_1111_1111_1111UL)]
        [InlineData(34, 0b0000_0000_0000_0000_0000_0000_0000_0011_1111_1111_1111_1111_1111_1111_1111_1111UL)]
        [InlineData(35, 0b0000_0000_0000_0000_0000_0000_0000_0111_1111_1111_1111_1111_1111_1111_1111_1111UL)]
        [InlineData(36, 0b0000_0000_0000_0000_0000_0000_0000_1111_1111_1111_1111_1111_1111_1111_1111_1111UL)]
        [InlineData(37, 0b0000_0000_0000_0000_0000_0000_0001_1111_1111_1111_1111_1111_1111_1111_1111_1111UL)]
        [InlineData(38, 0b0000_0000_0000_0000_0000_0000_0011_1111_1111_1111_1111_1111_1111_1111_1111_1111UL)]
        [InlineData(39, 0b0000_0000_0000_0000_0000_0000_0111_1111_1111_1111_1111_1111_1111_1111_1111_1111UL)]
        [InlineData(40, 0b0000_0000_0000_0000_0000_0000_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111UL)]
        [InlineData(41, 0b0000_0000_0000_0000_0000_0001_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111UL)]
        [InlineData(42, 0b0000_0000_0000_0000_0000_0011_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111UL)]
        [InlineData(43, 0b0000_0000_0000_0000_0000_0111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111UL)]
        [InlineData(44, 0b0000_0000_0000_0000_0000_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111UL)]
        [InlineData(45, 0b0000_0000_0000_0000_0001_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111UL)]
        [InlineData(46, 0b0000_0000_0000_0000_0011_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111UL)]
        [InlineData(47, 0b0000_0000_0000_0000_0111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111UL)]
        [InlineData(48, 0b0000_0000_0000_0000_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111UL)]
        [InlineData(49, 0b0000_0000_0000_0001_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111UL)]
        [InlineData(50, 0b0000_0000_0000_0011_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111UL)]
        [InlineData(51, 0b0000_0000_0000_0111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111UL)]
        [InlineData(52, 0b0000_0000_0000_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111UL)]
        [InlineData(53, 0b0000_0000_0001_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111UL)]
        [InlineData(54, 0b0000_0000_0011_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111UL)]
        [InlineData(55, 0b0000_0000_0111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111UL)]
        [InlineData(56, 0b0000_0000_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111UL)]
        [InlineData(57, 0b0000_0001_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111UL)]
        [InlineData(58, 0b0000_0011_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111UL)]
        [InlineData(59, 0b0000_0111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111UL)]
        [InlineData(60, 0b0000_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111UL)]
        [InlineData(61, 0b0001_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111UL)]
        [InlineData(62, 0b0011_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111UL)]
        [InlineData(63, 0b0111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111UL)]
        [InlineData(64, 0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111UL)]
        public void LowerBitsUInt64(int count, ulong expected)
        {
            Assert.Equal(expected, Bitmask.LowerBitsUInt64(count));
        }

        [Fact]
        public void LowerBitsUInt64ThrowsWithNegativeCount()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Bitmask.LowerBitsUInt64(-1);
            });
        }

        [Fact]
        public void LowerBitsUInt64ThrowsWithTooBigCount()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Bitmask.LowerBitsUInt64(sizeof(ulong) * 8 + 1);
            });
        }

        [Theory]
        [InlineData(00, 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000L)]
        [InlineData(01, 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0001L)]
        [InlineData(02, 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0011L)]
        [InlineData(03, 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0111L)]
        [InlineData(04, 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_1111L)]
        [InlineData(05, 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0001_1111L)]
        [InlineData(06, 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0011_1111L)]
        [InlineData(07, 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0111_1111L)]
        [InlineData(08, 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_1111_1111L)]
        [InlineData(09, 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0001_1111_1111L)]
        [InlineData(10, 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0011_1111_1111L)]
        [InlineData(11, 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0111_1111_1111L)]
        [InlineData(12, 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_1111_1111_1111L)]
        [InlineData(13, 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0001_1111_1111_1111L)]
        [InlineData(14, 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0011_1111_1111_1111L)]
        [InlineData(15, 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0111_1111_1111_1111L)]
        [InlineData(16, 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_1111_1111_1111_1111L)]
        [InlineData(17, 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0001_1111_1111_1111_1111L)]
        [InlineData(18, 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0011_1111_1111_1111_1111L)]
        [InlineData(19, 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0111_1111_1111_1111_1111L)]
        [InlineData(20, 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_1111_1111_1111_1111_1111L)]
        [InlineData(21, 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0001_1111_1111_1111_1111_1111L)]
        [InlineData(22, 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0011_1111_1111_1111_1111_1111L)]
        [InlineData(23, 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0111_1111_1111_1111_1111_1111L)]
        [InlineData(24, 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_1111_1111_1111_1111_1111_1111L)]
        [InlineData(25, 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0001_1111_1111_1111_1111_1111_1111L)]
        [InlineData(26, 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0011_1111_1111_1111_1111_1111_1111L)]
        [InlineData(27, 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0111_1111_1111_1111_1111_1111_1111L)]
        [InlineData(28, 0b0000_0000_0000_0000_0000_0000_0000_0000_0000_1111_1111_1111_1111_1111_1111_1111L)]
        [InlineData(29, 0b0000_0000_0000_0000_0000_0000_0000_0000_0001_1111_1111_1111_1111_1111_1111_1111L)]
        [InlineData(30, 0b0000_0000_0000_0000_0000_0000_0000_0000_0011_1111_1111_1111_1111_1111_1111_1111L)]
        [InlineData(31, 0b0000_0000_0000_0000_0000_0000_0000_0000_0111_1111_1111_1111_1111_1111_1111_1111L)]
        [InlineData(32, 0b0000_0000_0000_0000_0000_0000_0000_0000_1111_1111_1111_1111_1111_1111_1111_1111L)]
        [InlineData(33, 0b0000_0000_0000_0000_0000_0000_0000_0001_1111_1111_1111_1111_1111_1111_1111_1111L)]
        [InlineData(34, 0b0000_0000_0000_0000_0000_0000_0000_0011_1111_1111_1111_1111_1111_1111_1111_1111L)]
        [InlineData(35, 0b0000_0000_0000_0000_0000_0000_0000_0111_1111_1111_1111_1111_1111_1111_1111_1111L)]
        [InlineData(36, 0b0000_0000_0000_0000_0000_0000_0000_1111_1111_1111_1111_1111_1111_1111_1111_1111L)]
        [InlineData(37, 0b0000_0000_0000_0000_0000_0000_0001_1111_1111_1111_1111_1111_1111_1111_1111_1111L)]
        [InlineData(38, 0b0000_0000_0000_0000_0000_0000_0011_1111_1111_1111_1111_1111_1111_1111_1111_1111L)]
        [InlineData(39, 0b0000_0000_0000_0000_0000_0000_0111_1111_1111_1111_1111_1111_1111_1111_1111_1111L)]
        [InlineData(40, 0b0000_0000_0000_0000_0000_0000_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111L)]
        [InlineData(41, 0b0000_0000_0000_0000_0000_0001_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111L)]
        [InlineData(42, 0b0000_0000_0000_0000_0000_0011_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111L)]
        [InlineData(43, 0b0000_0000_0000_0000_0000_0111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111L)]
        [InlineData(44, 0b0000_0000_0000_0000_0000_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111L)]
        [InlineData(45, 0b0000_0000_0000_0000_0001_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111L)]
        [InlineData(46, 0b0000_0000_0000_0000_0011_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111L)]
        [InlineData(47, 0b0000_0000_0000_0000_0111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111L)]
        [InlineData(48, 0b0000_0000_0000_0000_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111L)]
        [InlineData(49, 0b0000_0000_0000_0001_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111L)]
        [InlineData(50, 0b0000_0000_0000_0011_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111L)]
        [InlineData(51, 0b0000_0000_0000_0111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111L)]
        [InlineData(52, 0b0000_0000_0000_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111L)]
        [InlineData(53, 0b0000_0000_0001_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111L)]
        [InlineData(54, 0b0000_0000_0011_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111L)]
        [InlineData(55, 0b0000_0000_0111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111L)]
        [InlineData(56, 0b0000_0000_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111L)]
        [InlineData(57, 0b0000_0001_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111L)]
        [InlineData(58, 0b0000_0011_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111L)]
        [InlineData(59, 0b0000_0111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111L)]
        [InlineData(60, 0b0000_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111L)]
        [InlineData(61, 0b0001_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111L)]
        [InlineData(62, 0b0011_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111L)]
        [InlineData(63, 0b0111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111L)]
        [InlineData(64, unchecked((long)0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111L))]
        public void LowerBitsInt64(int count, long expected)
        {
            Assert.Equal(expected, Bitmask.LowerBitsInt64(count));
        }

        [Fact]
        public void LowerBitsInt64ThrowsWithNegativeCount()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Bitmask.LowerBitsInt64(-1);
            });
        }

        [Fact]
        public void LowerBitsInt64ThrowsWithTooBigCount()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Bitmask.LowerBitsInt64(sizeof(long) * 8 + 1);
            });
        }

        [Theory]
        [InlineData(00, unchecked((ulong)0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000UL))]
        [InlineData(01, unchecked((ulong)0b1000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000UL))]
        [InlineData(02, unchecked((ulong)0b1100_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000UL))]
        [InlineData(03, unchecked((ulong)0b1110_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000UL))]
        [InlineData(04, unchecked((ulong)0b1111_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000UL))]
        [InlineData(05, unchecked((ulong)0b1111_1000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000UL))]
        [InlineData(06, unchecked((ulong)0b1111_1100_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000UL))]
        [InlineData(07, unchecked((ulong)0b1111_1110_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000UL))]
        [InlineData(08, unchecked((ulong)0b1111_1111_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000UL))]
        [InlineData(09, unchecked((ulong)0b1111_1111_1000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000UL))]
        [InlineData(10, unchecked((ulong)0b1111_1111_1100_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000UL))]
        [InlineData(11, unchecked((ulong)0b1111_1111_1110_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000UL))]
        [InlineData(12, unchecked((ulong)0b1111_1111_1111_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000UL))]
        [InlineData(13, unchecked((ulong)0b1111_1111_1111_1000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000UL))]
        [InlineData(14, unchecked((ulong)0b1111_1111_1111_1100_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000UL))]
        [InlineData(15, unchecked((ulong)0b1111_1111_1111_1110_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000UL))]
        [InlineData(16, unchecked((ulong)0b1111_1111_1111_1111_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000UL))]
        [InlineData(17, unchecked((ulong)0b1111_1111_1111_1111_1000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000UL))]
        [InlineData(18, unchecked((ulong)0b1111_1111_1111_1111_1100_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000UL))]
        [InlineData(19, unchecked((ulong)0b1111_1111_1111_1111_1110_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000UL))]
        [InlineData(20, unchecked((ulong)0b1111_1111_1111_1111_1111_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000UL))]
        [InlineData(21, unchecked((ulong)0b1111_1111_1111_1111_1111_1000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000UL))]
        [InlineData(22, unchecked((ulong)0b1111_1111_1111_1111_1111_1100_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000UL))]
        [InlineData(23, unchecked((ulong)0b1111_1111_1111_1111_1111_1110_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000UL))]
        [InlineData(24, unchecked((ulong)0b1111_1111_1111_1111_1111_1111_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000UL))]
        [InlineData(25, unchecked((ulong)0b1111_1111_1111_1111_1111_1111_1000_0000_0000_0000_0000_0000_0000_0000_0000_0000UL))]
        [InlineData(26, unchecked((ulong)0b1111_1111_1111_1111_1111_1111_1100_0000_0000_0000_0000_0000_0000_0000_0000_0000UL))]
        [InlineData(27, unchecked((ulong)0b1111_1111_1111_1111_1111_1111_1110_0000_0000_0000_0000_0000_0000_0000_0000_0000UL))]
        [InlineData(28, unchecked((ulong)0b1111_1111_1111_1111_1111_1111_1111_0000_0000_0000_0000_0000_0000_0000_0000_0000UL))]
        [InlineData(29, unchecked((ulong)0b1111_1111_1111_1111_1111_1111_1111_1000_0000_0000_0000_0000_0000_0000_0000_0000UL))]
        [InlineData(30, unchecked((ulong)0b1111_1111_1111_1111_1111_1111_1111_1100_0000_0000_0000_0000_0000_0000_0000_0000UL))]
        [InlineData(31, unchecked((ulong)0b1111_1111_1111_1111_1111_1111_1111_1110_0000_0000_0000_0000_0000_0000_0000_0000UL))]
        [InlineData(32, unchecked((ulong)0b1111_1111_1111_1111_1111_1111_1111_1111_0000_0000_0000_0000_0000_0000_0000_0000UL))]
        [InlineData(33, unchecked((ulong)0b1111_1111_1111_1111_1111_1111_1111_1111_1000_0000_0000_0000_0000_0000_0000_0000UL))]
        [InlineData(34, unchecked((ulong)0b1111_1111_1111_1111_1111_1111_1111_1111_1100_0000_0000_0000_0000_0000_0000_0000UL))]
        [InlineData(35, unchecked((ulong)0b1111_1111_1111_1111_1111_1111_1111_1111_1110_0000_0000_0000_0000_0000_0000_0000UL))]
        [InlineData(36, unchecked((ulong)0b1111_1111_1111_1111_1111_1111_1111_1111_1111_0000_0000_0000_0000_0000_0000_0000UL))]
        [InlineData(37, unchecked((ulong)0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1000_0000_0000_0000_0000_0000_0000UL))]
        [InlineData(38, unchecked((ulong)0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1100_0000_0000_0000_0000_0000_0000UL))]
        [InlineData(39, unchecked((ulong)0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1110_0000_0000_0000_0000_0000_0000UL))]
        [InlineData(40, unchecked((ulong)0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_0000_0000_0000_0000_0000_0000UL))]
        [InlineData(41, unchecked((ulong)0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1000_0000_0000_0000_0000_0000UL))]
        [InlineData(42, unchecked((ulong)0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1100_0000_0000_0000_0000_0000UL))]
        [InlineData(43, unchecked((ulong)0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1110_0000_0000_0000_0000_0000UL))]
        [InlineData(44, unchecked((ulong)0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_0000_0000_0000_0000_0000UL))]
        [InlineData(45, unchecked((ulong)0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1000_0000_0000_0000_0000UL))]
        [InlineData(46, unchecked((ulong)0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1100_0000_0000_0000_0000UL))]
        [InlineData(47, unchecked((ulong)0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1110_0000_0000_0000_0000UL))]
        [InlineData(48, unchecked((ulong)0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_0000_0000_0000_0000UL))]
        [InlineData(49, unchecked((ulong)0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1000_0000_0000_0000UL))]
        [InlineData(50, unchecked((ulong)0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1100_0000_0000_0000UL))]
        [InlineData(51, unchecked((ulong)0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1110_0000_0000_0000UL))]
        [InlineData(52, unchecked((ulong)0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_0000_0000_0000UL))]
        [InlineData(53, unchecked((ulong)0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1000_0000_0000UL))]
        [InlineData(54, unchecked((ulong)0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1100_0000_0000UL))]
        [InlineData(55, unchecked((ulong)0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1110_0000_0000UL))]
        [InlineData(56, unchecked((ulong)0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_0000_0000UL))]
        [InlineData(57, unchecked((ulong)0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1000_0000UL))]
        [InlineData(58, unchecked((ulong)0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1100_0000UL))]
        [InlineData(59, unchecked((ulong)0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1110_0000UL))]
        [InlineData(60, unchecked((ulong)0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_0000UL))]
        [InlineData(61, unchecked((ulong)0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1000UL))]
        [InlineData(62, unchecked((ulong)0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1100UL))]
        [InlineData(63, unchecked((ulong)0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1110UL))]
        [InlineData(64, unchecked((ulong)0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111UL))]
        public void HigherBitsUInt64(int count, ulong expected)
        {
            Assert.Equal(expected, Bitmask.HigherBitsUInt64(count));
        }

        [Fact]
        public void HigherBitsUInt64ThrowsWithNegativeCount()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Bitmask.HigherBitsUInt64(-1);
            });
        }

        [Fact]
        public void HigherBitsUInt64ThrowsWithTooBigCount()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Bitmask.HigherBitsUInt64(sizeof(ulong) * 8 + 1);
            });
        }

        [Theory]
        [InlineData(00, unchecked((long)0b0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000L))]
        [InlineData(01, unchecked((long)0b1000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000L))]
        [InlineData(02, unchecked((long)0b1100_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000L))]
        [InlineData(03, unchecked((long)0b1110_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000L))]
        [InlineData(04, unchecked((long)0b1111_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000L))]
        [InlineData(05, unchecked((long)0b1111_1000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000L))]
        [InlineData(06, unchecked((long)0b1111_1100_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000L))]
        [InlineData(07, unchecked((long)0b1111_1110_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000L))]
        [InlineData(08, unchecked((long)0b1111_1111_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000L))]
        [InlineData(09, unchecked((long)0b1111_1111_1000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000L))]
        [InlineData(10, unchecked((long)0b1111_1111_1100_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000L))]
        [InlineData(11, unchecked((long)0b1111_1111_1110_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000L))]
        [InlineData(12, unchecked((long)0b1111_1111_1111_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000L))]
        [InlineData(13, unchecked((long)0b1111_1111_1111_1000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000L))]
        [InlineData(14, unchecked((long)0b1111_1111_1111_1100_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000L))]
        [InlineData(15, unchecked((long)0b1111_1111_1111_1110_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000L))]
        [InlineData(16, unchecked((long)0b1111_1111_1111_1111_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000L))]
        [InlineData(17, unchecked((long)0b1111_1111_1111_1111_1000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000L))]
        [InlineData(18, unchecked((long)0b1111_1111_1111_1111_1100_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000L))]
        [InlineData(19, unchecked((long)0b1111_1111_1111_1111_1110_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000L))]
        [InlineData(20, unchecked((long)0b1111_1111_1111_1111_1111_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000L))]
        [InlineData(21, unchecked((long)0b1111_1111_1111_1111_1111_1000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000L))]
        [InlineData(22, unchecked((long)0b1111_1111_1111_1111_1111_1100_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000L))]
        [InlineData(23, unchecked((long)0b1111_1111_1111_1111_1111_1110_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000L))]
        [InlineData(24, unchecked((long)0b1111_1111_1111_1111_1111_1111_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000L))]
        [InlineData(25, unchecked((long)0b1111_1111_1111_1111_1111_1111_1000_0000_0000_0000_0000_0000_0000_0000_0000_0000L))]
        [InlineData(26, unchecked((long)0b1111_1111_1111_1111_1111_1111_1100_0000_0000_0000_0000_0000_0000_0000_0000_0000L))]
        [InlineData(27, unchecked((long)0b1111_1111_1111_1111_1111_1111_1110_0000_0000_0000_0000_0000_0000_0000_0000_0000L))]
        [InlineData(28, unchecked((long)0b1111_1111_1111_1111_1111_1111_1111_0000_0000_0000_0000_0000_0000_0000_0000_0000L))]
        [InlineData(29, unchecked((long)0b1111_1111_1111_1111_1111_1111_1111_1000_0000_0000_0000_0000_0000_0000_0000_0000L))]
        [InlineData(30, unchecked((long)0b1111_1111_1111_1111_1111_1111_1111_1100_0000_0000_0000_0000_0000_0000_0000_0000L))]
        [InlineData(31, unchecked((long)0b1111_1111_1111_1111_1111_1111_1111_1110_0000_0000_0000_0000_0000_0000_0000_0000L))]
        [InlineData(32, unchecked((long)0b1111_1111_1111_1111_1111_1111_1111_1111_0000_0000_0000_0000_0000_0000_0000_0000L))]
        [InlineData(33, unchecked((long)0b1111_1111_1111_1111_1111_1111_1111_1111_1000_0000_0000_0000_0000_0000_0000_0000L))]
        [InlineData(34, unchecked((long)0b1111_1111_1111_1111_1111_1111_1111_1111_1100_0000_0000_0000_0000_0000_0000_0000L))]
        [InlineData(35, unchecked((long)0b1111_1111_1111_1111_1111_1111_1111_1111_1110_0000_0000_0000_0000_0000_0000_0000L))]
        [InlineData(36, unchecked((long)0b1111_1111_1111_1111_1111_1111_1111_1111_1111_0000_0000_0000_0000_0000_0000_0000L))]
        [InlineData(37, unchecked((long)0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1000_0000_0000_0000_0000_0000_0000L))]
        [InlineData(38, unchecked((long)0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1100_0000_0000_0000_0000_0000_0000L))]
        [InlineData(39, unchecked((long)0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1110_0000_0000_0000_0000_0000_0000L))]
        [InlineData(40, unchecked((long)0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_0000_0000_0000_0000_0000_0000L))]
        [InlineData(41, unchecked((long)0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1000_0000_0000_0000_0000_0000L))]
        [InlineData(42, unchecked((long)0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1100_0000_0000_0000_0000_0000L))]
        [InlineData(43, unchecked((long)0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1110_0000_0000_0000_0000_0000L))]
        [InlineData(44, unchecked((long)0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_0000_0000_0000_0000_0000L))]
        [InlineData(45, unchecked((long)0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1000_0000_0000_0000_0000L))]
        [InlineData(46, unchecked((long)0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1100_0000_0000_0000_0000L))]
        [InlineData(47, unchecked((long)0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1110_0000_0000_0000_0000L))]
        [InlineData(48, unchecked((long)0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_0000_0000_0000_0000L))]
        [InlineData(49, unchecked((long)0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1000_0000_0000_0000L))]
        [InlineData(50, unchecked((long)0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1100_0000_0000_0000L))]
        [InlineData(51, unchecked((long)0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1110_0000_0000_0000L))]
        [InlineData(52, unchecked((long)0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_0000_0000_0000L))]
        [InlineData(53, unchecked((long)0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1000_0000_0000L))]
        [InlineData(54, unchecked((long)0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1100_0000_0000L))]
        [InlineData(55, unchecked((long)0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1110_0000_0000L))]
        [InlineData(56, unchecked((long)0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_0000_0000L))]
        [InlineData(57, unchecked((long)0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1000_0000L))]
        [InlineData(58, unchecked((long)0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1100_0000L))]
        [InlineData(59, unchecked((long)0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1110_0000L))]
        [InlineData(60, unchecked((long)0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_0000L))]
        [InlineData(61, unchecked((long)0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1000L))]
        [InlineData(62, unchecked((long)0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1100L))]
        [InlineData(63, unchecked((long)0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1110L))]
        [InlineData(64, unchecked((long)0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111L))]
        public void HigherBitsInt64(int count, long expected)
        {
            Assert.Equal(expected, Bitmask.HigherBitsInt64(count));
        }

        [Fact]
        public void HigherBitsInt64ThrowsWithNegativeCount()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Bitmask.HigherBitsInt64(-1);
            });
        }

        [Fact]
        public void HigherBitsInt64ThrowsWithTooBigCount()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Bitmask.HigherBitsInt64(sizeof(long) * 8 + 1);
            });
        }
    }
}