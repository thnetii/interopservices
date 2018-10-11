using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.NativeMemory
{
    /// <summary>
    /// Statically stores the marshaled size of a type.
    /// </summary>
    /// <typeparam name="T">The type to marshal.</typeparam>
    [SuppressMessage("Design", "CA1000: Do not declare static members on generic types")]
    public static class SizeOf<T>
    {
        /// <summary>
        /// Gets the number of bytes that the type will occupy in native memory
        /// when marshaled.
        /// </summary>
        /// <value>The cached result from calling the static <see cref="Marshal.SizeOf{T}()"/> method of the <see cref="Marshal"/> class.</value>
        public static int Bytes { get; } = Marshal.SizeOf<T>();

        /// <summary>
        /// Gets the number of bits that the type will occupy in native memory
        /// when marshaled.
        /// </summary>
        /// <value><c><see cref="Bytes"/> * 8</c></value>
        /// <remarks>One Byte is assumed to hold 8 bits.</remarks>
        public static int Bits { get; } = Bytes * 8;
    }
}
