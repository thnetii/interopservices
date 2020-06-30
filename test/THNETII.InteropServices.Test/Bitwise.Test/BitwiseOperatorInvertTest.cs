using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        internal static void InvertDefaultValueGeneric<T>()
            where T : unmanaged
        {
            T zero = default;
            T expectedInverted = default;
            Span<byte> expectedBytes = MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref expectedInverted, 1));
            for (int i = 0; i < expectedBytes.Length; i++)
                expectedBytes[i] = 0b1111_1111;

            BitwiseOperator.Invert(zero, out T inverted);

            Assert.Equal(expectedInverted, inverted);
        }

        [Theory]
        [InlineData(typeof(byte)), InlineData(typeof(sbyte))]
        [InlineData(typeof(ushort)), InlineData(typeof(short))]
        [InlineData(typeof(uint)), InlineData(typeof(int))]
        [InlineData(typeof(ulong)), InlineData(typeof(long))]
        [InlineData(typeof(TestStruct24Bits))]
        [InlineData(typeof(TestStruct48Bits))]
        [InlineData(typeof(TestStruct72Bits))]
        public static void InvertDefaultValue(Type type)
        {
            MethodInfo definition = typeof(BitwiseOperatorInvertTest)
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
                .Where(mi => mi.Name == nameof(InvertDefaultValueGeneric))
                .Where(mi => mi.IsGenericMethodDefinition)
                .Where(mi => mi.GetGenericArguments().Length == 1)
                .Where(mi => (mi.GetParameters()?.Length ?? 0 )== 0)
                .First();
            var generic = definition.MakeGenericMethod(type);
            generic.Invoke(null, null);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(10)]
        [InlineData(11)]
        [InlineData(12)]
        [InlineData(13)]
        [InlineData(14)]
        [InlineData(15)]
        public void InvertDefaultByteArrayWithLength(int length)
        {
            ReadOnlySpan<byte> bytes = new byte[length];
            var inverted = new byte[length];
            BitwiseOperator.Invert(bytes, inverted);
            Assert.All(inverted, b => Assert.Equal<byte>(0xFF, b));
        }
    }
}
