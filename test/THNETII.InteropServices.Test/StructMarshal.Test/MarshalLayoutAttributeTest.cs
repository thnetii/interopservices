using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Xunit;

namespace THNETII.InteropServices.StructMarshal.Test
{
    public class MarshalLayoutAttributeTest
    {
        [Fact]
        public void CanConvertToStructLayoutAttributeInstance()
        {
            var mla = new MarshalLayoutAttribute(LayoutKind.Sequential)
            {
                CharSet = CharSet.Unicode,
                Pack = 32,
                Size = 128
            };

            var sla = (StructLayoutAttribute)mla;
            Assert.NotNull(sla);
            Assert.Equal(mla.LayoutKind, sla.Value);
            Assert.Equal(mla.CharSet, sla.CharSet);
            Assert.Equal(mla.Pack, sla.Pack);
            Assert.Equal(mla.Size, sla.Size);
        }

        [Fact]
        public void ConvertsNullMarshalLayoutAttributeToNull()
        {
            MarshalLayoutAttribute mla = null;
            var sla = (StructLayoutAttribute)mla;
            Assert.Null(sla);
        }

        [Fact]
        public void CanConvertFromStructLayoutAttributeInstance()
        {
            var sla = new StructLayoutAttribute(LayoutKind.Sequential)
            {
                CharSet = CharSet.Unicode,
                Pack = 32,
                Size = 128
            };

            var mla = (MarshalLayoutAttribute)sla;
            Assert.NotNull(mla);
            Assert.Equal(sla.Value, mla.LayoutKind);
            Assert.Equal(sla.CharSet, mla.CharSet);
            Assert.Equal(sla.Pack, mla.Pack);
            Assert.Equal(sla.Size, mla.Size);
        }

        [Fact]
        public void ConvertsNullStructLayoutAttributeToNull()
        {
            StructLayoutAttribute sla = null;
            var mla = (MarshalLayoutAttribute)sla;
            Assert.Null(mla);
        }

        class TestType1
        {
            public int field;
        }

        [Fact]
        public void NoMarshalLayoutAttributeResultsInAutoLayoutProxyMarshalerType()
        {
            var ti = ProxyMarshaler.CreateMarshalProxyType(typeof(TestType1).GetTypeInfo(), out var _);
            Assert.Equal(LayoutKind.Auto, ti.StructLayoutAttribute.Value);
        }


    }
}
