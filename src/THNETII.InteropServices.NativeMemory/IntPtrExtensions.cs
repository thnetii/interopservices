using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.NativeMemory
{
    /// <summary>
    /// Provides marshaling extension methods for the <see cref="IntPtr"/> type.
    /// </summary>
    [SuppressMessage(category: null, "CA1720", Scope = "Parameter")]
    public static class IntPtrExtensions
    {
        /// <summary>
        /// Marshals the pointer to an array of consecutive structured values in native memory.
        /// </summary>
        /// <typeparam name="T">The type each array element should be marshaled to.</typeparam>
        /// <param name="ptr">The pointer to marshal.</param>
        /// <param name="length">The number of items in the array.</param>
        /// <returns>
        /// <c>null</c> if <paramref name="ptr"/> is equal to <see cref="IntPtr.Zero"/>;
        /// otherwise an array of marshaled instances of type <typeparamref name="T"/>.
        /// </returns>
        public static T[] MarshalAsValueArray<T>(this IntPtr ptr, int length)
        {
            if (ptr == IntPtr.Zero)
                return null;
            else if (length < 1)
                length = 0;
            var tSizeOf = SizeOf<T>.Bytes;
            var ptrCurrent = ptr;
            var array = length > 0 ? new T[length] : Array.Empty<T>();
            for (int i = 0; i < length; i++, ptrCurrent += tSizeOf)
                array[i] = Marshal.PtrToStructure<T>(ptrCurrent);
            return array;
        }

        /// <summary>
        /// Marshals the pointer to an array of pointer values in native memory.
        /// </summary>
        /// <typeparam name="T">The type each array element should be marshaled to.</typeparam>
        /// <param name="ptr">The pointer to marshal.</param>
        /// <param name="length">The number of items in the array.</param>
        /// <returns>
        /// <c>null</c> if <paramref name="ptr"/> is equal to <see cref="IntPtr.Zero"/>;
        /// otherwise an array of marshaled instances of type <typeparamref name="T"/>.
        /// </returns>
        public static T[] MarshalAsReferenceArray<T>(this IntPtr ptr, int length)
        {
            if (ptr == IntPtr.Zero)
                return null;
            else if (length < 1)
                length = 0;
            var ptrCurrent = ptr;
            var array = length > 0 ? new T[length] : Array.Empty<T>();
            for (int i = 0; i < length; i++, ptrCurrent += IntPtr.Size)
            {
                var itemPtr = Marshal.ReadIntPtr(ptrCurrent);
                array[i] = itemPtr == IntPtr.Zero ? default
                    : Marshal.PtrToStructure<T>(itemPtr);
            }
            return array;
        }

        /// <summary>
        /// Marshals a BSTR pointer to a managed string value.
        /// </summary>
        /// <param name="ptr">The pointer to marshal.</param>
        /// <returns>
        /// <c>null</c> if <paramref name="ptr"/> is <see cref="IntPtr.Zero"/>; otherwise,
        /// A managed string instance containing a copy of the unmanaged string.
        /// </returns>
        public static string MarshalAsBSTR(this IntPtr ptr) => Marshal.PtrToStringBSTR(ptr);

        /// <summary>
        /// Marshals an ANSI string pointer (<c>PSTR</c>, <c>LPSTR</c> or <c>char</c> pointer in C) to a managed string value.
        /// <para>Copies and widens each character to UTF-16 up to the first null character.</para>
        /// </summary>
        /// <param name="ptr">The pointer to marshal.</param>
        /// <returns>
        /// <c>null</c> if <paramref name="ptr"/> is <see cref="IntPtr.Zero"/>; otherwise,
        /// A managed string instance containing a copy of the unmanaged string.
        /// </returns>
        public static string MarshalAsAnsiString(this IntPtr ptr) => Marshal.PtrToStringAnsi(ptr);

        /// <summary>
        /// Marshals an ANSI string pointer (<c>PSTR</c>, <c>LPSTR</c> or <c>char</c> pointer in C) with the specified length to a managed string value.
        /// </summary>
        /// <param name="ptr">The pointer to marshal.</param>
        /// <param name="length">The non-negative number of single-byte characters to copy.</param>
        /// <returns>
        /// <c>null</c> if <paramref name="ptr"/> is <see cref="IntPtr.Zero"/>; otherwise,
        /// A managed string instance containing a copy of the unmanaged string.
        /// </returns>
        /// <exception cref="ArgumentException"><paramref name="length"/> is negative.</exception>
        public static string MarshalAsAnsiString(this IntPtr ptr, int length) => Marshal.PtrToStringAnsi(ptr, length);

        /// <summary>
        /// Marshals a Unicode UTF-16 string pointer (<c>PWSTR</c>, <c>LPWSTR</c> or <c>wchar_t</c> pointer in C) to a managed string value.
        /// <para>Copies each character up to the first null character.</para>
        /// </summary>
        /// <param name="ptr">The pointer to marshal.</param>
        /// <returns>
        /// <c>null</c> if <paramref name="ptr"/> is <see cref="IntPtr.Zero"/>; otherwise,
        /// A managed string instance containing a copy of the unmanaged string.
        /// </returns>
        public static string MarshalAsUnicodeString(this IntPtr ptr) => Marshal.PtrToStringUni(ptr);

        /// <summary>
        /// Marshals a Unicode UTF-16 string pointer (<c>PWSTR</c>, <c>LPWSTR</c> or <c>wchar_t</c> pointer in C) with the specified length to a managed string value.
        /// </summary>
        /// <param name="ptr">The pointer to marshal.</param>
        /// <param name="length">The non-negative number of two-byte UTF-16 characters to copy.</param>
        /// <returns>
        /// <c>null</c> if <paramref name="ptr"/> is <see cref="IntPtr.Zero"/>; otherwise,
        /// A managed string instance containing a copy of the unmanaged string.
        /// </returns>
        public static string MarshalAsUnicodeString(this IntPtr ptr, int length) => Marshal.PtrToStringUni(ptr, length);

        /// <summary>
        /// Marshals a platfrom dependent string pointer (<c>PTSTR</c>, <c>LPTSTR</c> or <c>TCHAR</c> pointer in C) to a managed string value.
        /// <para>Copies each character up to the first null character.</para>
        /// </summary>
        /// <param name="ptr">The pointer to marshal.</param>
        /// <returns>
        /// <c>null</c> if <paramref name="ptr"/> is <see cref="IntPtr.Zero"/>; otherwise,
        /// A managed string instance containing a copy of the unmanaged string.
        /// </returns>
        /// <exception cref="PlatformNotSupportedException" />
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

        /// <summary>
        /// Marshals a platfrom dependent string pointer (<c>PTSTR</c>, <c>LPTSTR</c> or <c>TCHAR</c> pointer in C) to a managed string value.
        /// </summary>
        /// <param name="ptr">The pointer to marshal.</param>
        /// <param name="length">The non-negative number of characters to copy.</param>
        /// <returns>
        /// <c>null</c> if <paramref name="ptr"/> is <see cref="IntPtr.Zero"/>; otherwise,
        /// A managed string instance containing a copy of the unmanaged string.
        /// </returns>
        /// <exception cref="PlatformNotSupportedException" />
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
