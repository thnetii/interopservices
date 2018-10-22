using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.NativeMemory
{
    /// <summary>
    /// Contract for a typed pointer to a contiguous sequence of <typeparamref name="T"/> values.
    /// </summary>
    /// <typeparam name="T">The type of the structures that is pointed to.</typeparam>
    public interface IArrayPtr<T> : IIntPtr<T> where T : struct { }

    /// <summary>
    /// Represents a typed pointer to a contiguous sequence of <typeparamref name="T"/> values.
    /// </summary>
    /// <typeparam name="T">The type of the structures that is pointed to.</typeparam>
    [StructLayout(LayoutKind.Sequential)]
    [SuppressMessage("Design", "CA1000: Do not declare static members on generic types")]
    [SuppressMessage("Usage", "CA2225: Operator overloads have named alternates")]
    [SuppressMessage("Performance", "CA1815: Override equals and operator equals on value types", Justification = nameof(IIntPtrExtensions))]
    public struct ArrayPtr<T> : IArrayPtr<T> where T : struct
    {
        /// <summary>
        /// Initializes a new typed pointer with the specified pointer to an unspecified type.
        /// </summary>
        /// <param name="ptr">A pointer to an unspecified type.</param>
        public ArrayPtr(IntPtr ptr) => Pointer = ptr;

        /// <inheritdoc />
        public IntPtr Pointer { get; }

        /// <summary>
        /// Returns a span over the array with the specified length.
        /// </summary>
        /// <param name="count">The number of struct values in the array.</param>
        /// <returns>
        /// An empty span if <paramref name="count"/> is <c>0</c> (zero) or less;<br/>
        /// otherwise a <see cref="Span{T}"/> that starts at <see cref="Pointer"/> and has a <see cref="Span{T}.Length"/> of <paramref name="count"/>.
        /// </returns>
        public Span<T> AsSpan(int count) => Pointer.AsRefStructSpan<T>(count);

        /// <summary>
        /// Converts the numeric value of the current pointer to its equivalent
        /// string representation.
        /// </summary>
        /// <returns>The string representation of the value of this instance.</returns>
        public override string ToString() => Pointer.ToString();

        /// <summary>
        /// Converts the numeric value of the current pointer to its equivalent
        /// string representation.
        /// </summary>
        /// <param name="format">A format specification that governs how the current pointer is converted.</param>
        /// <returns>The string representation of the value of this instance.</returns>
        public string ToString(string format) => Pointer.ToString(format);
    }

    /// <summary>
    /// Provides common extension methods for implementations of <see cref="IArrayPtr{T}"/>
    /// </summary>
    public static class ArrayPtrOfTExtensions
    {
        /// <summary>
        /// Returns a span over the array with the specified length.
        /// </summary>
        /// <param name="ptr">The array pointer to interpret.</param>
        /// <param name="count">The number of struct values in the array.</param>
        /// <typeparam name="T">The type of the structures that is pointed to.</typeparam>
        /// <returns>
        /// An empty span if <paramref name="count"/> is <c>0</c> (zero) or less;<br/>
        /// otherwise a <see cref="Span{T}"/> that starts at the <see cref="IIntPtr.Pointer"/> of <paramref name="ptr"/> and has a <see cref="Span{T}.Length"/> of <paramref name="count"/>.
        /// </returns>
        public static Span<T> AsSpan<T>(this IArrayPtr<T> ptr, int count) where T : struct =>
            (ptr?.Pointer).GetValueOrDefault().AsRefStructSpan<T>(count);
    }
}
