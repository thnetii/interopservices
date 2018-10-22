using System;

namespace THNETII.InteropServices.NativeMemory
{
    /// <summary>
    /// Wraps an <see cref="IntPtr"/> value.
    /// </summary>
    public interface IIntPtr
    {
        /// <summary>
        /// Gets the underlying pointer value.
        /// </summary>
        /// <value>An <see cref="IntPtr"/> value.</value>
        IntPtr Pointer { get; }
    }

    /// <summary>
    /// Represents a typed pointer to a structure
    /// </summary>
    /// <typeparam name="T">The type of the structure that is pointed to.</typeparam>
    public interface IIntPtr<T> : IIntPtr where T : struct { }
}
