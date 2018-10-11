using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.NativeMemory
{
    /// <summary>
    /// Represents a typed pointer to a contiguous sequence of <typeparamref name="T"/> values.
    /// </summary>
    /// <typeparam name="T">The type of the structures that is pointed to.</typeparam>
    [StructLayout(LayoutKind.Sequential)]
    [SuppressMessage("Design", "CA1000: Do not declare static members on generic types")]
    [SuppressMessage("Usage", "CA2225: Operator overloads have named alternates")]
    public struct ArrayPtr<T> : IEquatable<ArrayPtr<T>>, IEquatable<IntPtr<T>>, IEquatable<IntPtr> where T : struct
    {
        /// <summary>
        /// A read-only field that represent the zero- or null-pointer for pointers to arrays of <typeparamref name="T" />.
        /// </summary>
        public static readonly ArrayPtr<T> Null = new ArrayPtr<T>(IntPtr.Zero);

        /// <summary>
        /// Initializes a new typed pointer with the specified pointer to an unspecified type.
        /// </summary>
        /// <param name="ptr">A pointer to an unspecified type.</param>
        public ArrayPtr(IntPtr ptr) => Pointer = ptr;

        /// <summary>
        /// Gets a value indicating whether the instance is a null-reference or not.
        /// </summary>
        /// <value><c>true</c> if the underlying pointer is <see cref="IntPtr.Zero"/>; otherwise, <c>false</c>.</value>
        public bool IsNull => Pointer == IntPtr.Zero;

        /// <summary>
        /// Gets the underlying pointer value.
        /// </summary>
        /// <value>A managed <see cref="IntPtr"/> value.</value>
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

        /// <inheritdoc />
        public override int GetHashCode() => Pointer.GetHashCode();

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            switch (obj)
            {
                case null:
                    return Pointer.Equals(null);
                case ArrayPtr<T> other:
                    return Pointer.Equals(other.Pointer);
                case IntPtr ptr:
                    return Pointer == ptr;
                default:
                    return false;
            }
        }

        /// <inheritdoc />
        public bool Equals(ArrayPtr<T> other) => Equals(other.Pointer);

        /// <inheritdoc />
        public bool Equals(IntPtr otherPtr) => Pointer == otherPtr;

        /// <inheritdoc />
        public bool Equals(IntPtr<T> otherPtr) => Pointer == otherPtr.Pointer;

        /// <inheritdoc />
        public static bool operator ==(ArrayPtr<T> left, ArrayPtr<T> right) =>
            left.Equals(right);

        /// <inheritdoc />
        public static bool operator !=(ArrayPtr<T> left, ArrayPtr<T> right) =>
            !left.Equals(right);

        /// <inheritdoc />
        public static bool operator ==(ArrayPtr<T> left, IntPtr right) =>
            left.Equals(right);

        /// <inheritdoc />
        public static bool operator !=(ArrayPtr<T> left, IntPtr right) =>
            !left.Equals(right);

        /// <inheritdoc />
        public static bool operator ==(ArrayPtr<T> left, IntPtr<T> right) =>
            left.Equals(right.Pointer);

        /// <inheritdoc />
        public static bool operator !=(ArrayPtr<T> left, IntPtr<T> right) =>
            !left.Equals(right);

        /// <inheritdoc />
        public static bool operator ==(IntPtr left, ArrayPtr<T> right) =>
            left.Equals(right);

        /// <inheritdoc />
        public static bool operator !=(IntPtr left, ArrayPtr<T> right) =>
            !right.Equals(left);

        /// <inheritdoc />
        public static bool operator ==(IntPtr<T> left, ArrayPtr<T> right) =>
            left.Equals(right);

        /// <inheritdoc />
        public static bool operator !=(IntPtr<T> left, ArrayPtr<T> right) =>
            !right.Equals(left);

        /// <summary>
        /// Increments the pointer by the specified number of structs.
        /// </summary>
        /// <param name="left">The pointer to increment.</param>
        /// <param name="count">The number of structs to increment by.</param>
        /// <returns>A new pointer pointing to the memory location that is located at <c><paramref name="count"/> * sizeof(<typeparamref name="T"/>)</c> positive offset from <paramref name="left"/>.</returns>
        /// <remarks>This operation behaves like pointer arithmatic of typed pointer in the C programming language.</remarks>
        public static ArrayPtr<T> operator +(ArrayPtr<T> left, int count) =>
            new ArrayPtr<T>(left.Pointer + count * SizeOf<T>.Bytes);

        /// <summary>
        /// Increments the pointer by the specified number of structs.
        /// </summary>
        /// <param name="count">The number of structs to increment by.</param>
        /// <returns>A new pointer pointing to the memory location that is located at <c><paramref name="count"/> * sizeof(<typeparamref name="T"/>)</c> positive offset from the current instance.</returns>
        /// <remarks>This operation behaves like pointer arithmatic of typed pointer in the C programming language.</remarks>
        public ArrayPtr<T> Add(int count) => this + count;

        /// <summary>
        /// Decrements the pointer by the specified number of structs.
        /// </summary>
        /// <param name="left">The pointer to increment.</param>
        /// <param name="count">The number of structs to decrement by.</param>
        /// <returns>A new pointer pointing to the memory location that is located at <c><paramref name="count"/> * sizeof(<typeparamref name="T"/>)</c> negative offset from <paramref name="left"/>.</returns>
        /// <remarks>This operation behaves like pointer arithmatic of typed pointer in the C programming language.</remarks>
        public static ArrayPtr<T> operator -(ArrayPtr<T> left, int count) =>
            new ArrayPtr<T>(left.Pointer - count * SizeOf<T>.Bytes);

        /// <summary>
        /// Decrements the pointer by the specified number of structs.
        /// </summary>
        /// <param name="count">The number of structs to decrement by.</param>
        /// <returns>A new pointer pointing to the memory location that is located at <c><paramref name="count"/> * sizeof(<typeparamref name="T"/>)</c> negative offset from the current instance.</returns>
        /// <remarks>This operation behaves like pointer arithmatic of typed pointer in the C programming language.</remarks>
        public ArrayPtr<T> Subtract(int count) => this - count;

        /// <summary>
        /// Casts the typed instance to a pointer value of unspecified type.
        /// </summary>
        /// <param name="typedPtr">The pointer to cast.</param>
        public static explicit operator IntPtr(ArrayPtr<T> typedPtr) =>
            typedPtr.Pointer;

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
}
