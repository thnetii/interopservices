using System.Runtime.InteropServices;

namespace THNETII.InteropServices.NativeMemory
{
    /// <summary>
    /// Provides extension methods for Wide character UTF-16 string pointers.
    /// </summary>
    public static class WideStringPtrExtensions
    {
        /// <summary>
        /// Allocated a new managed <see cref="string"/> instance and copies
        /// the charaters from the pointer to the new <see cref="string"/> until
        /// the first null-character is found.
        /// </summary>
        /// <param name="stringPtr">The pointer to marshal.</param>
        /// <returns>
        /// A managed string that holds a copy of the unmanaged string if the value of the
        /// <paramref name="stringPtr"/> parameter is not <c>null</c> or a null-pointer;<br/>
        /// otherwise, this method returns <c>null</c>.
        /// </returns>
        /// <seealso cref="Marshal.PtrToStringUni(System.IntPtr)"/>
        public static string MarshalAsString(this IArrayPtr<char> stringPtr) =>
            Marshal.PtrToStringUni((stringPtr?.Pointer).GetValueOrDefault());

        /// <summary>
        /// Allocated a new manages <see cref="string"/> instance and copies
        /// the specified number of charaters from the pointer to the new <see cref="string"/>.
        /// </summary>
        /// <param name="stringPtr">The pointer to marshal.</param>
        /// <param name="length">The number of Unicode characters to copy.</param>
        /// <returns>
        /// A managed string that holds a copy of the unmanaged string if the value of the
        /// <paramref name="stringPtr"/> parameter is not <c>null</c> or a null-pointer;<br/>
        /// otherwise, this method returns <c>null</c>.
        /// </returns>
        /// <seealso cref="Marshal.PtrToStringUni(System.IntPtr, int)"/>
        public static string MarshalAsString(this IArrayPtr<char> stringPtr, int length) =>
            Marshal.PtrToStringUni((stringPtr?.Pointer).GetValueOrDefault(), length);
    }
}
