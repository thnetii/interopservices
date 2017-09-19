﻿using System;
using System.Linq;
using System.Runtime.InteropServices;
using Xunit;

namespace THNETII.InteropServices.StructMarshal.Test
{
    public class ICustomMarshalerBasicTest
    {
        [StructLayout(LayoutKind.Sequential)]
        class DummyStruct { public int field; }

        [Fact]
        public void ProxyMarshalerGetInstanceReturnsCustomMarshaler()
        {
            var marshaler = ProxyMarshaler<DummyStruct>.GetInstance(null);
            Assert.IsType<ProxyMarshaler<DummyStruct>>(marshaler);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("test")]
        public void ProxyMarshalerGetInstanceTakesAnyStringValue(string cookie)
        {
            var marshaler = ProxyMarshaler<DummyStruct>.GetInstance(cookie);
        }

        [Fact]
        public void ProxyMarshalerGetNativeDataSizeReturnsNegativeOne()
        {
            ICustomMarshaler marshaler = ProxyMarshaler<DummyStruct>.GetInstance();
            Assert.Equal(-1, marshaler.GetNativeDataSize());
        }

        [Fact]
        public void ProxyMarshalerZeroPointerMarshalsToNull()
        {
            var marshaler = ProxyMarshaler<DummyStruct>.GetInstance();
            Assert.Null(marshaler.MarshalNativeToManaged(IntPtr.Zero));
        }

        [Fact]
        public void ProxyMarshalerNullMarshalsToZeroPointer()
        {
            var marshaler = ProxyMarshaler<DummyStruct>.GetInstance();
            Assert.Equal(IntPtr.Zero, marshaler.MarshalManagedToNative(null));
        }

        [Fact]
        public void ProxyMarshalerMarshalsDummyStructToNative()
        {
            const int expectedFieldValue = 42;
            var marshaler = ProxyMarshaler<DummyStruct>.GetInstance();
            var instance = new DummyStruct { field = expectedFieldValue };
            var ptr = marshaler.MarshalManagedToNative(instance);
            Assert.NotEqual(IntPtr.Zero, ptr);
            try
            {
                //  field in DummyStruct is at offset 0 (it's the first field)
                var fieldValue = Marshal.ReadInt32(ptr, ofs: 0);
                Assert.Equal(expectedFieldValue, fieldValue);
            }
            finally { marshaler.CleanUpNativeData(ptr); }
        }

        [Fact]
        public void ProxyMarshalerMarshalsDummyStructFromNative()
        {
            const int expectedFieldValue = 42;
            var ptr = Marshal.AllocCoTaskMem(Marshal.SizeOf<DummyStruct>());
            try
            {
                //  field in DummyStruct is at offset 0 (it's the first field)
                Marshal.WriteInt32(ptr, ofs: 0, val: expectedFieldValue);
                var marshaler = ProxyMarshaler<DummyStruct>.GetInstance();
                var instance = marshaler.MarshalNativeToManaged(ptr) as DummyStruct;
                Assert.NotNull(instance);
                Assert.Equal(expectedFieldValue, instance.field);
            }
            finally { Marshal.FreeCoTaskMem(ptr); }
        }

        [Fact]
        public void ProxyMarshalerProxySizeOfEqualToDummyStructSizeOf()
        {
            var expected = Marshal.SizeOf<DummyStruct>();
            Assert.Equal(expected, ProxyMarshaler<DummyStruct>.ProxySizeOf);
        }
    }
}