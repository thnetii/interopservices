using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.NativeMemory
{
    /// <summary>
    /// A typed pointer to an array of <see cref="IntPtr"/> values.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    [SuppressMessage("Performance", "CA1815: Override equals and operator equals on value types", Justification = nameof(IntPtrOfTExtensions))]
    public struct IntPtrArrayPtr : IArrayPtr<IntPtr>
    {
        /// <summary>
        /// Initializes a new typed pointer with the specified pointer to an unspecified type.
        /// </summary>
        /// <param name="ptr">A pointer to an unspecified type.</param>
        public IntPtrArrayPtr(IntPtr ptr) => Pointer = ptr;

        /// <inheritdoc />
        public IntPtr Pointer { get; }
    }
}
