using System;
using System.Linq;
using System.Runtime.InteropServices;
using THNETII.InteropServices.NativeMemory;
using Xunit;

namespace THNETII.InteropServices.Test.NativeMemory
{
    public class ByValArrayIntPtrTest
    {
        [Fact]
        public void IntPtrZeroMarshalAsByValArrayReturnsNull()
        {
            Assert.Null(IntPtr.Zero.MarshalAsValueArray<TestStruct>(10));
        }

        [Fact]
        public void IntPtrMarshalAsByValArrayReturnsEmptyArrayWithZeroLength()
        {
            var ptr = Marshal.AllocCoTaskMem(10 * TestStruct.SizeOf);

            try { Assert.Empty(ptr.MarshalAsValueArray<TestStruct>(length: 0)); }

            finally { Marshal.FreeCoTaskMem(ptr); }
        }

        [Fact]
        public void IntPtrMarshalAsByValArrayReturnsArray()
        {
            const int cnt = 10;
            int testStructSize = TestStruct.SizeOf;
            var nativePtr = Marshal.AllocCoTaskMem(cnt * testStructSize);
            try
            {
                var ptrCurrent = nativePtr;
                for (int i = 0; i < cnt; i++, ptrCurrent += testStructSize)
                {
                    Marshal.StructureToPtr(new TestStruct
                    {
                        intField = i,
                        doubleField = i,
                        stringField = i.ToString()
                    }, ptrCurrent, fDeleteOld: false);
                }

                try
                {
                    var managedArray = nativePtr.MarshalAsValueArray<TestStruct>(cnt);
                    Assert.NotNull(managedArray);
                    Assert.Equal(managedArray.Length, cnt);
                    Assert.All(Enumerable.Range(0, managedArray.Length).Zip(managedArray, (i, v) => new { Index = i, Item = v }), elem =>
                    {
                        Assert.Equal(elem.Index, elem.Item.intField);
                        Assert.Equal(elem.Index, elem.Item.doubleField, precision: 2);
                        Assert.Equal(elem.Index, int.Parse(elem.Item.stringField));
                    });
                }
                finally
                {
                    ptrCurrent = nativePtr;
                    for (int i = 0; i < cnt; i++, ptrCurrent += testStructSize)
                        Marshal.DestroyStructure<TestStruct>(ptrCurrent);
                }
            }
            finally { Marshal.FreeCoTaskMem(nativePtr); }
        }
    }
}
