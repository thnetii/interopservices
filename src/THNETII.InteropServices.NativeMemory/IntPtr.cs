using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.NativeMemory
{
    /// <summary>
    /// Represents a typed pointer to a structure
    /// </summary>
    /// <typeparam name="T">The type of the structure that is pointed to.</typeparam>
    [StructLayout(LayoutKind.Sequential)]
    [SuppressMessage("Design", "CA1000: Do not declare static members on generic types")]
    [SuppressMessage("Usage", "CA2225: Operator overloads have named alternates")]
    [SuppressMessage("Performance", "CA1815: Override equals and operator equals on value types", Justification = nameof(IIntPtrExtensions))]
    public struct IntPtr<T> : IIntPtr<T>
        where T : struct
    {
        /// <summary>
        /// Initializes a new typed pointer with the specified pointer to an unspecified type.
        /// </summary>
        /// <param name="ptr">A pointer to an unspecified type.</param>
        public IntPtr(IntPtr ptr) => Pointer = ptr;

        /// <inheritdoc />
        public IntPtr Pointer { get; }
    }
}
