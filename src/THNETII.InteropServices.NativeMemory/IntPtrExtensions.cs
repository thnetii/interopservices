using System;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.NativeMemory
{
    public static class IntPtrExtensions
    {
        public static T[] MarshalAsValueArray<T>(this IntPtr ptr, int length)
        {
            if (ptr == IntPtr.Zero)
                return null;
            else if (length < 1)
                length = 0;
            var tSizeOf = Marshal.SizeOf<T>();
            var ptrCurrent = ptr;
            var array = new T[length];
            for (int i = 0; i < length; i++, ptrCurrent += tSizeOf)
                array[i] = Marshal.PtrToStructure<T>(ptrCurrent);
            return array;
        }

        public static string MarshalAsBSTR(this IntPtr ptr) => Marshal.PtrToStringBSTR(ptr);

        public static string MarshalAsAnsiString(this IntPtr ptr) => Marshal.PtrToStringAnsi(ptr);

        public static string MarshalAsAnsiString(this IntPtr ptr, int length) => Marshal.PtrToStringAnsi(ptr, length);

        public static string MarshalAsUnicodeString(this IntPtr ptr) => Marshal.PtrToStringUni(ptr);

        public static string MarshalAsUnicodeString(this IntPtr ptr, int length) => Marshal.PtrToStringUni(ptr, length);

        public static string MarshalAsAutoString(this IntPtr ptr)
        {
            switch (Marshal.SystemDefaultCharSize)
            {
                case 0: return null;
                case 1: return ptr.MarshalAsAnsiString();
                case 2: return ptr.MarshalAsUnicodeString();
                default: throw new PlatformNotSupportedException($"System Default Char size of {Marshal.SystemDefaultCharSize} bytes is not supported.");
            }
        }

        public static string MarshalAsAutoString(this IntPtr ptr, int length)
        {
            switch (Marshal.SystemDefaultCharSize)
            {
                case 0: return null;
                case 1: return ptr.MarshalAsAnsiString(length);
                case 2: return ptr.MarshalAsUnicodeString(length);
                default: throw new PlatformNotSupportedException($"System Default Char size of {Marshal.SystemDefaultCharSize} bytes is not supported.");
            }
        }
    }
}
