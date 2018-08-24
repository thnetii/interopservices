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
    [SuppressMessage("Design", "CA1000:Do not declare static members on generic types")]
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates")]
    public struct IntPtr<T> : IEquatable<IntPtr<T>>, IEquatable<IntPtr>
        where T : struct
    {
        /// <summary>
        /// A read-only field that represent the zero- or null-pointer for pointers to <typeparamref name="T" />.
        /// </summary>
        public static readonly IntPtr<T> Null = new IntPtr<T>(IntPtr.Zero);

        /// <summary>
        /// Initializes a new typed pointer with the specified pointer to an unspecified type.
        /// </summary>
        /// <param name="ptr">A pointer to an unspecified type.</param>
        public IntPtr(IntPtr ptr) => Pointer = ptr;

        /// <summary>
        /// Gets a value indicating whether the instance is a null-reference or not.
        /// </summary>
        /// <value><c>true</c> if the underlying pointer is <see cref="IntPtr.Zero"/>; otherwise, <c>false</c>.</value>
        public bool IsNull => Pointer == IntPtr.Zero;

        /// <summary>
        /// Gets a read/writable reference to the struct value pointed to by the current instance.
        /// </summary>
        /// <value>A reference to a struct value of type <typeparamref name="T"/>.</value>
        /// <exception cref="NullReferenceException">The current instance represents a null-pointer and cannot be dereferenced.</exception>
        public ref T Struct => ref Pointer.AsRefStruct<T>();

        /// <summary>
        /// Gets the underlying pointer value.
        /// </summary>
        /// <value>A managed <see cref="IntPtr"/> value.</value>
        public IntPtr Pointer { get; }

        /// <inheritdoc />
        public override int GetHashCode() => Pointer.GetHashCode();

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            switch (obj)
            {
                case null:
                    return Pointer.Equals(null);
                case IntPtr<T> other:
                    return Pointer.Equals(other.Pointer);
                case IntPtr ptr:
                    return Pointer == ptr;
                default:
                    return false;
            }
        }

        /// <inheritdoc />
        public bool Equals(IntPtr<T> other) => Equals(other.Pointer);

        /// <inheritdoc />
        public bool Equals(IntPtr otherPtr) => Pointer == otherPtr;

        /// <inheritdoc />
        public static bool operator ==(IntPtr<T> left, IntPtr<T> right) =>
            left.Equals(right);

        /// <inheritdoc />
        public static bool operator !=(IntPtr<T> left, IntPtr<T> right) =>
            !left.Equals(right);

        /// <inheritdoc />
        public static bool operator ==(IntPtr<T> left, IntPtr right) =>
            left.Equals(right);

        /// <inheritdoc />
        public static bool operator !=(IntPtr<T> left, IntPtr right) =>
            !left.Equals(right);

        /// <inheritdoc />
        public static bool operator ==(IntPtr left, IntPtr<T> right) =>
            left.Equals(right);

        /// <inheritdoc />
        public static bool operator !=(IntPtr left, IntPtr<T> right) =>
            !right.Equals(left);

        /// <summary>
        /// Increments the pointer by the specified number of structs.
        /// </summary>
        /// <param name="left">The pointer to increment.</param>
        /// <param name="count">The number of structs to increment by.</param>
        /// <returns>A new pointer pointing to the memory location that is located at <c><paramref name="count"/> * sizeof(<typeparamref name="T"/>)</c> positive offset from <paramref name="left"/>.</returns>
        /// <remarks>This operation behaves like pointer arithmatic of typed pointer in the C programming language.</remarks>
        public static IntPtr<T> operator +(IntPtr<T> left, int count) =>
            new IntPtr<T>(left.Pointer + count * SizeOf<T>.Bytes);

        /// <summary>
        /// Increments the pointer by the specified number of structs.
        /// </summary>
        /// <param name="count">The number of structs to increment by.</param>
        /// <returns>A new pointer pointing to the memory location that is located at <c><paramref name="count"/> * sizeof(<typeparamref name="T"/>)</c> positive offset from the current instance.</returns>
        /// <remarks>This operation behaves like pointer arithmatic of typed pointer in the C programming language.</remarks>
        public IntPtr<T> Add(int count) => this + count;

        /// <summary>
        /// Decrements the pointer by the specified number of structs.
        /// </summary>
        /// <param name="left">The pointer to increment.</param>
        /// <param name="count">The number of structs to decrement by.</param>
        /// <returns>A new pointer pointing to the memory location that is located at <c><paramref name="count"/> * sizeof(<typeparamref name="T"/>)</c> negative offset from <paramref name="left"/>.</returns>
        /// <remarks>This operation behaves like pointer arithmatic of typed pointer in the C programming language.</remarks>
        public static IntPtr<T> operator -(IntPtr<T> left, int count) =>
            new IntPtr<T>(left.Pointer - count * SizeOf<T>.Bytes);

        /// <summary>
        /// Decrements the pointer by the specified number of structs.
        /// </summary>
        /// <param name="count">The number of structs to decrement by.</param>
        /// <returns>A new pointer pointing to the memory location that is located at <c><paramref name="count"/> * sizeof(<typeparamref name="T"/>)</c> negative offset from the current instance.</returns>
        /// <remarks>This operation behaves like pointer arithmatic of typed pointer in the C programming language.</remarks>
        public IntPtr<T> Subtract(int count) => this - count;

        /// <summary>
        /// Casts the typed instance to a pointer value of unspecified type.
        /// </summary>
        /// <param name="typedPtr">The pointer to cast.</param>
        public static explicit operator IntPtr(IntPtr<T> typedPtr) =>
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
