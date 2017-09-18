using System;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.StructMarshal
{
    public class ProxyMarshaler<T> : ICustomMarshaler where T : new()
    {
        static ProxyMarshaler()
        {

        }

        /// <inheritdoc />
        public void CleanUpManagedData(object ManagedObj)
        {
            if (ManagedObj is IDisposable disposable)
                disposable.Dispose();
        }

        public void CleanUpNativeData(IntPtr pNativeData)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public int GetNativeDataSize() => -1;

        public IntPtr MarshalManagedToNative(object ManagedObj)
        {
            throw new NotImplementedException();
        }

        public object MarshalNativeToManaged(IntPtr pNativeData)
        {
            throw new NotImplementedException();
        }

        public static ICustomMarshaler GetInstance(string cookie = null) => new ProxyMarshaler<T>();
    }
}