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
        /// Interprets the pointer as a read/writable reference to a struct value.
        /// </summary>
        /// <typeparam name="T">The type of the struct to marshal. Must only contain value types fields.</typeparam>
        /// <param name="ptr">The pointer to marshal.</param>
        /// <returns>A writable reference to a <typeparamref name="T"/> value.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="T"/> contains reference members or pointers.</exception>
        public static ref T AsRefStruct<T>(this IntPtr ptr) where T : struct
        {
            var cbT = SizeOf<T>.Bytes;
            Span<T> span;
            try
            {
                unsafe { span = new Span<T>(ptr.ToPointer(), cbT); }
            }
            catch (ArgumentException argExcept)
            {
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
                throw new ArgumentException(argExcept.Message, paramName: nameof(T), argExcept);
#pragma warning restore CA2208 // Instantiate argument exceptions correctly
            }
            return ref span[0];
        }

        /// <summary>
        /// Interprets the pointer as a pointer to null-terminated UTF-16 Unicode string
        /// (<c>PWSTR</c>, <c>LPWSTR</c> or <c>wchar_t*</c> in C) and returnes a character-span
        /// over the memory containing the string.
        /// </summary>
        /// <param name="ptr">The pointer to access.</param>
        /// <returns>
        /// A writable character span that spans from the memory location pointed to by <paramref name="ptr"/> and up to (but not including) the first encountered null character.
        /// </returns>
        /// <remarks>
        /// The returned span cannot span over more than 2^31-1 (<c><see cref="int"/>.<see cref="int.MaxValue"/></c>) bytes of memory.
        /// As a consequence the span will never be able to span over more than 2^30 characters, as each character
        /// occupies 2 bytes in memory.
        /// </remarks>
        public static Span<char> AsZeroTerminatedUnicodeSpan(this IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
                return Span<char>.Empty;
            Span<char> span;
            unsafe { span = new Span<char>(ptr.ToPointer(), int.MaxValue); }
            int nullIdx;
            const char nullChar = '\0';
            try { nullIdx = span.IndexOf(nullChar); }
#if !NETSTANDARD2_0
            catch (Exception)
#else // NETSTANDARD2_0
            catch (AccessViolationException)
#endif
            {
                for (nullIdx = 0; nullIdx < span.Length; nullIdx++)
                {
                    if (span[nullIdx] == nullChar)
                        break;
                }
            }
            if (nullIdx < 0)
                return span;
#pragma warning disable PC001 // API not supported on all platforms
            return span.Slice(start: 0, length: nullIdx);
#pragma warning restore PC001 // API not supported on all platforms
        }

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
