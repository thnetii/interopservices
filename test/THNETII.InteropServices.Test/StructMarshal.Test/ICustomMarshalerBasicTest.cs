using System;
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
            var marshaler = ProxyMarshaler<DummyStruct>.GetInstance();
            Assert.Equal(-1, marshaler.GetNativeDataSize());
        }
    }
}
