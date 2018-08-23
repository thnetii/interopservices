using System;
using System.Runtime.InteropServices;
using Xunit;

namespace THNETII.InteropServices.NativeMemory.Test
{
    public class IntPtrMarshalRefStructTest
    {
        [Fact]
        public void MarshalRefStructFromNullPointerThrows()
        {
            Assert.Throws<NullReferenceException>(() =>
            {
                var ptr = IntPtr.Zero;
                var intValue = ptr.AsRefStruct<int>();
            });
        }

        [Fact]
        public void MarshalRefStructWriteToNullPointerThrows()
        {
            var ptr = IntPtr.Zero;
            Assert.Throws<NullReferenceException>(() =>
            {
                ref int value = ref ptr.AsRefStruct<int>();
                value = 42;
            });
        }

        [StructLayout(LayoutKind.Sequential)]
        struct PrimitiveValues
        {
            public int t1;
            public int t2;
        }

        [Fact]
        public void MarshalRefStructPrimitiveValues()
        {
            var ptr = Marshal.AllocCoTaskMem(SizeOf<PrimitiveValues>.Bytes);
            try
            {
                const int t1 = 42;
                const int t2 = 24;
                Marshal.WriteInt32(ptr, ofs: 0, t1);
                Marshal.WriteInt32(ptr, ofs: sizeof(int), t2);

                var @struct = ptr.AsRefStruct<PrimitiveValues>();

                Assert.Equal(t1, @struct.t1);
                Assert.Equal(t2, @struct.t2);
            }
            finally { Marshal.FreeCoTaskMem(ptr); }
        }

        [Fact]
        public void MarshalRefStructPrimitiveValuesWrite()
        {
            var ptr = Marshal.AllocCoTaskMem(SizeOf<PrimitiveValues>.Bytes);
            try
            {
                const int t1 = 42;
                const int t2 = 24;

                ref PrimitiveValues @struct = ref ptr.AsRefStruct<PrimitiveValues>();
                @struct.t1 = t1;
                @struct.t2 = t2;

                var m1 = Marshal.ReadInt32(ptr, ofs: 0);
                var m2 = Marshal.ReadInt32(ptr, ofs: sizeof(int));

                Assert.Equal(t1, m1);
                Assert.Equal(t2, m2);
            }
            finally { Marshal.FreeCoTaskMem(ptr); }
        }

        [StructLayout(LayoutKind.Sequential)]
        struct MarshaledValues
        {
            [MarshalAs(UnmanagedType.LPWStr)]
            public string s1;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string s2;
        }

        [Fact]
        public void MarshalRefStructMarshaledValuesThrows()
        {
            var ptr = Marshal.AllocCoTaskMem(SizeOf<MarshaledValues>.Bytes);
            try
            {
                const string s1 = nameof(s1);
                const string s2 = nameof(s2);
                var p1 = Marshal.StringToCoTaskMemUni(s1);
                var p2 = Marshal.StringToCoTaskMemUni(s2);

                try
                {
                    Marshal.WriteIntPtr(ptr, ofs: 0, p1);
                    Marshal.WriteIntPtr(ptr, ofs: IntPtr.Size, p2);

                    Assert.Throws<ArgumentException>(() => ptr.AsRefStruct<MarshaledValues>());
                }
                finally
                {
                    Marshal.FreeCoTaskMem(p1);
                    Marshal.FreeCoTaskMem(p2);
                }
            }
            finally { Marshal.FreeCoTaskMem(ptr); }
        }
    }
}
