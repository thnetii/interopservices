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
        private static Span<T> AsRefStructSpanUnsafe<T>(this IntPtr ptr, int count)
            where T : struct
        {
            try
            {
                unsafe { return new Span<T>(ptr.ToPointer(), count); }
            }
            catch (ArgumentException argExcept)
            {
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
                throw new ArgumentException(argExcept.Message, paramName: nameof(T), argExcept);
#pragma warning restore CA2208 // Instantiate argument exceptions correctly
            }
        }

        /// <summary>
        /// Interprets the pointer as a read/writable span over multiple structs of the same type.
        /// </summary>
        /// <typeparam name="T">The type of the structs. Must only contain value types fields.</typeparam>
        /// <param name="ptr">The pointer to marshal.</param>
        /// <param name="count">The number of struct values in the memory area pointed to by <paramref name="ptr"/>.</param>
        /// <returns>A writable reference to a <typeparamref name="T"/> value. Returns an empty Span if <paramref name="count"/> is less than <c>1</c>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="T"/> contains reference members or pointers.</exception>
        public static Span<T> AsRefStructSpan<T>(this IntPtr ptr, int count)
            where T : struct
        {
            if (count < 1)
                return Span<T>.Empty;
            return AsRefStructSpanUnsafe<T>(ptr, count);
        }

        /// <summary>
        /// Interprets the pointer as a read/writable reference to a struct value.
        /// </summary>
        /// <typeparam name="T">The type of the struct to marshal. Must only contain value types fields.</typeparam>
        /// <param name="ptr">The pointer to marshal.</param>
        /// <returns>A writable reference to a <typeparamref name="T"/> value.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="T"/> contains reference members or pointers.</exception>
        public static ref T AsRefStruct<T>(this IntPtr ptr) where T : struct =>
            ref AsRefStructSpanUnsafe<T>(ptr, 1)[0];

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

#if NETSTANDARD1_3 || NETSTANDARD1_6
        /// <exception cref="OverflowException"/>
        [SuppressMessage("Usage", "PC001: API not supported on all platforms", Justification = "https://github.com/dotnet/platform-compat/issues/123")]
        private static unsafe Span<T> ToDefaultDelimitedSpan<T>(this IntPtr ptr)
            where T : struct, IEquatable<T>
        {
            if (ptr == IntPtr.Zero)
                return Span<T>.Empty;
            int length;
            var maxSpan = new Span<T>(ptr.ToPointer(), int.MaxValue);
            for (length = 0; true; length = checked(length + 1))
            {
                if (maxSpan[length].Equals(default))
                    break;
            }
            return maxSpan.Slice(start: 0, length);
        }
#else // !NETSTANDARD1_3 && !NETSTANDARD1_6
        /// <exception cref="OverflowException"/>
        private static unsafe Span<T> ToDefaultDelimitedSpan<T>(this IntPtr ptr)
            where T : struct, IEquatable<T>
        {
            if (ptr == IntPtr.Zero)
                return Span<T>.Empty;
            IntPtr pagePtr = ptr;
            int totalLength = 0;
            int segmentLength = GetRemainingBytesInPage(pagePtr);
            do
            {
                Span<byte> byteSpan = new Span<byte>(pagePtr.ToPointer(), segmentLength);
                Span<T> pageSpan = MemoryMarshal.Cast<byte, T>(byteSpan);
                int endIdx = pageSpan.IndexOf(default(T));
                if (endIdx >= 0)
                {
                    checked { totalLength += endIdx; }
                    break;
                }
                checked { totalLength += pageSpan.Length; }
                pagePtr += (pageSpan.Length * SizeOf<T>.Bytes);
                segmentLength = GetRemainingBytesInPage(pagePtr) + Environment.SystemPageSize;
            } while (true);
            return new Span<T>(ptr.ToPointer(), totalLength);

            int GetRemainingBytesInPage(IntPtr pPageMem)
            {
                long addr = pPageMem.ToInt64();
                int pageOffset = (int)(addr % Environment.SystemPageSize);
                if (pageOffset == 0)
                    return 0;
                return Environment.SystemPageSize - pageOffset;
            }
        }
#endif // !NETSTANDARD1_3 && !NETSTANDARD1_6

        /// <summary>
        /// Interprets a pointer to a zero-terminated byte sequence as a span of
        /// <see cref="byte"/> values up to, but excluding the terminating null-byte.
        /// </summary>
        /// <param name="ptr">A pointer to null-byte terminated data.</param>
        /// <remarks>
        /// The returned span is limited to a maximum size of <see cref="int.MaxValue"/> bytes.
        /// Passing a pointer to a larger, non-terminated memory area will raise an <see cref="OverflowException"/>.
        /// </remarks>
        /// <exception cref="OverflowException">No null-terminating byte within the first (2^31 - 1) bytes found.</exception>
        public static Span<byte> ToZeroTerminatedByteSpan(this IntPtr ptr) =>
            ToDefaultDelimitedSpan<byte>(ptr);

        /// <summary>
        /// Interprets a Unicode UTF-16 string pointer (<c>PWSTR</c>, <c>LPWSTR</c> or <c>wchar_t</c> pointer in C) as
        /// a span of <see cref="char"/> values up to, but excluding the terminating null-character.
        /// </summary>
        /// <param name="ptr">A pointer to the start of a Unicode UTF-16 string.</param>
        /// <returns>
        /// A Span of <see cref="char"/> values starting at the memory address pointed to
        /// by <paramref name="ptr"/> and spanning up to, but excluding the first
        /// encountered null-character (<c>'\0'</c>). If <paramref name="ptr"/> is <see cref="IntPtr.Zero"/> an empty
        /// span is returned.
        /// </returns>
        /// <remarks>
        /// The returned span is limited to a maximum size of <see cref="int.MaxValue"/> characters.
        /// Passing a pointer to a larger, non-terminated memory area will raise an <see cref="OverflowException"/>.
        /// </remarks>
        /// <exception cref="OverflowException">No null-terminating character within the first (2^31 - 1) characters found.</exception>
        public static Span<char> ToZeroTerminatedUnicodeSpan(this IntPtr ptr) =>
            ToDefaultDelimitedSpan<char>(ptr);
    }
}
