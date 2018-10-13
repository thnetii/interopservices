using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.NativeMemory
{
    /// <summary>
    /// Represents a typed pointer to a structure
    /// </summary>
    /// <typeparam name="T">The type of the structure that is pointed to.</typeparam>
    public interface IIntPtr<T> where T : struct
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
    [StructLayout(LayoutKind.Sequential)]
    [SuppressMessage("Design", "CA1000: Do not declare static members on generic types")]
    [SuppressMessage("Usage", "CA2225: Operator overloads have named alternates")]
    [SuppressMessage("Performance", "CA1815: Override equals and operator equals on value types", Justification = nameof(IntPtrOfTExtensions))]
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

    /// <summary>
    /// Provides common extension methods for implementations of <see cref="IIntPtr{T}"/>
    /// </summary>
    public static class IntPtrOfTExtensions
    {
        /// <summary>
        /// Returns a value indicating whether the instance is a null-reference or not.
        /// </summary>
        /// <returns><c>true</c> if the underlying pointer is <see cref="IntPtr.Zero"/>; otherwise, <c>false</c>.</returns>
        public static bool IsNull<T>(this IIntPtr<T> ptr) where T : struct =>
            (ptr?.Pointer).GetValueOrDefault() == IntPtr.Zero;

        /// <summary>
        /// Returns a reference to struct value pointed to by a pointer.
        /// </summary>
        /// <typeparam name="T">The type of the structure that is pointed to.</typeparam>
        /// <param name="ptr">The pointer to dereference.</param>
        /// <returns>A reference to the memory pointed to by <paramref name="ptr"/> interpreted as a structure of type <typeparamref name="T"/>.</returns>
        public static ref T AsRefStruct<T>(this IIntPtr<T> ptr) where T : struct =>
            ref (ptr?.Pointer).GetValueOrDefault().AsRefStruct<T>();

        /// <summary>
        /// Determines whether two pointers are equal.
        /// </summary>
        /// <typeparam name="T">The type of the structure that is pointed to.</typeparam>
        /// <param name="ptr">A pointer value to a structure of type <typeparamref name="T"/>.</param>
        /// <param name="otherPtr">The other pointer to compare against <paramref name="ptr"/>.</param>
        /// <returns>
        /// <c>true</c> if the <see cref="IIntPtr{T}.Pointer"/> property of both arguments is equal;<br/>
        /// otherwise, <c>false</c>.
        /// </returns>
        public static bool Equals<T>(this IIntPtr<T> ptr, IIntPtr<T> otherPtr) where T : struct =>
            (ptr?.Pointer).GetValueOrDefault() == (otherPtr?.Pointer).GetValueOrDefault();

        /// <summary>
        /// Determines whether two pointers are equal.
        /// </summary>
        /// <typeparam name="T">The type of the structure that is pointed to.</typeparam>
        /// <param name="ptr">A pointer value to a structure of type <typeparamref name="T"/>.</param>
        /// <param name="otherPtr">The other pointer to compare against <paramref name="ptr"/>.</param>
        /// <returns>
        /// <c>true</c> if the <see cref="IIntPtr{T}.Pointer"/> property of <paramref name="ptr"/> is equal to <paramref name="otherPtr"/>;<br/>
        /// otherwise, <c>false</c>.
        /// </returns>
        public static bool Equals<T>(this IIntPtr<T> ptr, IntPtr otherPtr) where T : struct =>
            (ptr?.Pointer).GetValueOrDefault() == otherPtr;
    }
}
