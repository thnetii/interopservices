using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.NativeMemory
{
    /// <summary>
    /// A pointer to a seqence of wide UTF-16 characters representing a
    /// UTF-16 native string.
    /// </summary>
    /// <remarks>
    /// Pointer to wide-characters sequences are also known in C APIs as:
    /// <list type="bullet">
    /// <item><term><c>wchar_t*</c></term></item>
    /// <item><term><c>PWSTR</c></term></item>
    /// <item><term><c>LPWSTR</c></term></item>
    /// </list>
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    [SuppressMessage("Performance", "CA1815: Override equals and operator equals on value types", Justification = nameof(IntPtrOfTExtensions))]
    public struct WideStringPtr : IArrayPtr<char>
    {
        /// <summary>
        /// Initializes a new typed pointer with the specified pointer to an unspecified type.
        /// </summary>
        /// <param name="ptr">A pointer to an unspecified type.</param>
        public WideStringPtr(IntPtr ptr) => Pointer = ptr;

        /// <inheritdoc />
        public IntPtr Pointer { get; }
    }
}
