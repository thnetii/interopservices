using System;

namespace THNETII.InteropServices.NativeMemory
{
    /// <summary>
    /// Provides common extension methods for implementations of <see cref="IIntPtr"/> and <see cref="IIntPtr{T}"/>.
    /// </summary>
    public static class IIntPtrExtensions
    {
        /// <summary>
        /// Returns a value indicating whether the instance is a null-reference or not.
        /// </summary>
        /// <returns><c>true</c> if the underlying pointer is <see cref="IntPtr.Zero"/>; otherwise, <c>false</c>.</returns>
        public static bool IsNull<TPtr>(this TPtr ptr) where TPtr : struct, IIntPtr =>
            ptr.Pointer == IntPtr.Zero;

        /// <summary>
        /// Returns a reference to struct value pointed to by a pointer.
        /// </summary>
        /// <typeparam name="T">The type of the structure that is pointed to.</typeparam>
        /// <param name="ptr">The pointer to dereference.</param>
        /// <returns>A reference to the memory pointed to by <paramref name="ptr"/> interpreted as a structure of type <typeparamref name="T"/>.</returns>
        public static ref T AsRefStruct<TPtr, T>(this TPtr ptr) where TPtr : struct, IIntPtr<T> where T : struct =>
            ref ptr.Pointer.AsRefStruct<T>();

        /// <summary>
        /// Determines whether two pointers are equal.
        /// </summary>
        /// <param name="ptr">A pointer value.</param>
        /// <param name="otherPtr">The other pointer to compare against <paramref name="ptr"/>.</param>
        /// <returns>
        /// <c>true</c> if the <see cref="IIntPtr.Pointer"/> property of both arguments is equal;<br/>
        /// otherwise, <c>false</c>.
        /// </returns>
        public static bool Equals(this IIntPtr ptr, IIntPtr otherPtr) =>
            (ptr?.Pointer).GetValueOrDefault() == (otherPtr?.Pointer).GetValueOrDefault();

        /// <summary>
        /// Determines whether two pointers are equal.
        /// </summary>
        /// <param name="ptr">A pointer value.</param>
        /// <param name="otherPtr">The other pointer to compare against <paramref name="ptr"/>.</param>
        /// <returns>
        /// <c>true</c> if the <see cref="IIntPtr.Pointer"/> property of <paramref name="ptr"/> is equal to <paramref name="otherPtr"/>;<br/>
        /// otherwise, <c>false</c>.
        /// </returns>
        public static bool Equals(this IIntPtr ptr, IntPtr otherPtr) =>
            (ptr?.Pointer).GetValueOrDefault() == otherPtr;
    }
}
