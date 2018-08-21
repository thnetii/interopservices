using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.NativeMemory
{
    /// <summary>
    /// Statically stores the marshaled size of a type.
    /// </summary>
    /// <typeparam name="T">The type to marshal.</typeparam>
    [SuppressMessage(category: null, "CA1000")]
    public static class SizeOf<T>
    {
        /// <summary>
        /// Gets the number of bytes that the type will occupy in native memory
        /// when marshaled.
        /// </summary>
        public static int Bytes { get; } = Marshal.SizeOf<T>();

        /// <summary>
        /// Gets the number of bits that the type will occupy in native memory
        /// when marshaled.
        /// </summary>
        /// <remarks>One Byte is assumed to hold 8 bits.</remarks>
        public static int Bits { get; } = Bytes * 8;
    }
}
