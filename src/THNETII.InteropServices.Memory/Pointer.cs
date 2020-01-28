using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.Memory
{
    /// <summary>
    /// Base interface for specific Pointer struct implementations.
    /// </summary>
    public interface IPointer
    {
        /// <summary>Gets the underlying <see cref="IntPtr"/> value.</summary>
        IntPtr Pointer { get; }
    }

    /// <summary>
    /// Interface for a pointer to a single value of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type to which the pointer points to. Can be any structural type.</typeparam>
    public interface IPointer<T> : IPointer where T : struct { }

    /// <summary>
    /// Interface for a pointer to a single read-only value of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type to which the pointer points to. Can be any structural type.</typeparam>
    public interface IConstPointer<T> : IPointer where T : struct { }

    /// <summary>
    /// Interface for a pointer to a contiguous sequence of values of type <typeparamref name="TItem"/>.
    /// </summary>
    /// <typeparam name="TItem">The type of values to which the pointer points to.</typeparam>
    public interface IArrayPointer<TItem> : IPointer { }

    /// <summary>
    /// Interface for a pointer to a contiguous read-only sequence of values of type <typeparamref name="TItem"/>.
    /// </summary>
    /// <typeparam name="TItem">The type of values to which the pointer points to.</typeparam>
    public interface IConstArrayPointer<TItem> : IPointer { }

    /// <summary>
    /// Interface for a ANSI-string pointer with a context-defined length.
    /// </summary>
    public interface IAnsiStringPointer : IArrayPointer<byte> { }

    /// <summary>
    /// Interface for a read-only ANSI-string pointer with a context-defined length.
    /// </summary>
    public interface IConstAnsiStringPointer : IConstArrayPointer<byte> { }

    /// <summary>
    /// Interface for a wide-character UTF-16-string pointer with a context-defined length.
    /// </summary>
    public interface IUnicodeStringPointer : IArrayPointer<char> { }

    /// <summary>
    /// Interface for a read-only wide-character UTF-16-string pointer with a context-defined length.
    /// </summary>
    public interface IConstUnicodeStringPointer : IConstArrayPointer<char> { }

    /// <summary>
    /// Interface for a platform-dependant string pointer with a context-defined length.
    /// </summary>
    public interface IAutoStringPointer : IArrayPointer<byte> { }

    /// <summary>
    /// Interface for a read-only platform-dependant string pointer with a context-defined length.
    /// </summary>
    public interface IConstAutoStringPointer : IConstArrayPointer<byte> { }

    /// <summary>
    /// Interface for a null-byte terminated ANSI string-pointer.
    /// </summary>
    public interface ITerminatedAnsiStringPointer : IAnsiStringPointer { }

    /// <summary>
    /// Interface for a read-only null-byte terminated ANSI string-pointer.
    /// </summary>
    public interface IConstTerminatedAnsiStringPointer : IConstAnsiStringPointer { }

    /// <summary>
    /// Interface for a null-character (<c>'\0'</c>) terminated wide-character UTF-16 string pointer.
    /// </summary>
    public interface ITerminatedUnicodeStringPointer : IUnicodeStringPointer { }

    /// <summary>
    /// Interface for a read-only null-character (<c>'\0'</c>) terminated wide-character UTF-16 string pointer.
    /// </summary>
    public interface IConstTerminatedUnicodeStringPointer : IConstUnicodeStringPointer { }

    /// <summary>
    /// Interface for a platform-dependant terminated string pointer.
    /// </summary>
    public interface ITerminatedAutoStringPointer : IAutoStringPointer { }

    /// <summary>
    /// Interface for a read-only platform-dependant terminated string pointer.
    /// </summary>
    public interface IConstTerminatedAutoStringPointer : IConstAutoStringPointer { }

    /// <summary>
    /// Interface for storing marshalled function pointers.
    /// </summary>
    /// <typeparam name="T">An unmanaged delegate type, typically attributed with the <see cref="UnmanagedFunctionPointerAttribute"/>.</typeparam>
    public interface IFunctionPointer<T> : IPointer where T : Delegate { }

    /// <summary>
    /// Static helper class that provides extension methods for the default pointer interface types.
    /// </summary>
    public static class Pointer
    {
        [StructLayout(LayoutKind.Explicit)]
        private struct PointerBlit<T> where T : unmanaged, IPointer
        {
            [FieldOffset(0)]
            public IntPtr ptr;
            [FieldOffset(0)]
            public T @struct;
        }

        /// <summary>
        /// Create a pointer structure of type <typeparamref name="T"/> from the
        /// specified <see cref="IntPtr"/> value.
        /// </summary>
        /// <typeparam name="T">
        /// A blittable structural type implementing <see cref="IPointer"/>.
        /// </typeparam>
        /// <param name="ptr">The pointer to initialize the new structure with.</param>
        /// <returns>
        /// A structure value with the value of <paramref name="ptr"/> blitted
        /// to the first bytes of the structure.
        /// </returns>
        public static T Create<T>(IntPtr ptr) where T : struct, IPointer
        {
            var @struct = default(T);
            var ptrSpan = MemoryMarshal.Cast<T, IntPtr>(SpanOverRef.GetSpan(ref @struct));
            ptrSpan[0] = ptr;

            return @struct;
        }

        /// <inheritdoc cref="Create{T}(IntPtr)"/>
        public static unsafe T Create<T>(void* ptr) where T : struct, IPointer =>
            Create<T>(new IntPtr(ptr));

        /// <summary>
        /// Checks whether a pointer represents <see langword="null"/>.
        /// </summary>
        /// <typeparam name="T">
        /// A structurual type implementing the <see cref="IPointer"/> interface.
        /// </typeparam>
        /// <param name="ptr">The pointer value to check.</param>
        /// <returns>
        /// <see langword="true"/> if the <see cref="IPointer.Pointer"/> property
        /// of <paramref name="ptr"/> is not equal to <see cref="IntPtr.Zero"/>;
        /// otrherwise, <see langword="false"/>
        /// </returns>
        public static bool HasValue<T>(this T ptr) where T : struct, IPointer =>
            ptr.Pointer != IntPtr.Zero;

        /// <summary>
        /// Gets a reference to the structural value that a pointer points to.
        /// </summary>
        /// <typeparam name="TPtr">The structural type of the pointer implementing the <see cref="IPointer"/> interface.</typeparam>
        /// <typeparam name="TValue">The type of the value that the pointer points to.</typeparam>
        /// <param name="ptr">The pointer to dereference.</param>
        /// <returns>A reference to the dereferenced value of type <typeparamref name="TValue"/>.</returns>
        public static ref TValue GetReference<TPtr, TValue>(this TPtr ptr)
            where TPtr : struct, IPointer<TValue> where TValue : struct =>
            ref ptr.Pointer.AsRefStruct<TValue>();

        /// <summary>
        /// Gets a read-only reference to the structural value that a const-pointer points to.
        /// </summary>
        /// <typeparam name="TPtr">The structural type of the pointer implementing the <see cref="IConstPointer{TValue}"/> interface.</typeparam>
        /// <typeparam name="TValue">The type of the value that the pointer points to.</typeparam>
        /// <param name="ptr">The pointer to dereference.</param>
        /// <returns>A read-only reference to the dereferenced value of type <typeparamref name="TValue"/>.</returns>
        public static ref readonly TValue GetReadOnlyReference<TPtr, TValue>(this TPtr ptr)
            where TPtr : struct, IConstPointer<TValue> where TValue : struct =>
            ref ptr.Pointer.AsRefStruct<TValue>();

        /// <summary>
        /// Reinterprets the pointer as a <see cref="Span{TItem}"/> with the
        /// specified length.
        /// </summary>
        /// <typeparam name="TPtr">The structural type implementing the <see cref="IArrayPointer{TItem}"/> interface.</typeparam>
        /// <typeparam name="TItem">The type of the sequence of values that the pointer points to.</typeparam>
        /// <param name="ptr">The pointer to interpret.</param>
        /// <param name="count">The number of items placed sequentially at the referenced address.</param>
        /// <returns>A <see cref="Span{TItem}"/> value with a length of <paramref name="count"/>.</returns>
        public static Span<TItem> GetSpan<TPtr, TItem>(this TPtr ptr,
            int count)
            where TPtr : struct, IArrayPointer<TItem>
            where TItem : struct =>
            ptr.Pointer.AsRefStructSpan<TItem>(count);

        /// <summary>
        /// Reinterprets the pointer as a <see cref="ReadOnlySpan{TItem}"/> with the
        /// specified length.
        /// </summary>
        /// <typeparam name="TPtr">The structural type implementing the <see cref="IConstArrayPointer{TItem}"/> interface.</typeparam>
        /// <typeparam name="TItem">The type of the sequence of values that the pointer points to.</typeparam>
        /// <param name="ptr">The pointer to interpret.</param>
        /// <param name="count">The number of items placed sequentially at the referenced address.</param>
        /// <returns>A <see cref="ReadOnlySpan{TItem}"/> value with a length of <paramref name="count"/>.</returns>
        public static ReadOnlySpan<TItem> GetReadOnlySpan<TPtr, TItem>(
            this TPtr ptr, int count)
            where TPtr : struct, IConstArrayPointer<TItem>
            where TItem : struct =>
            ptr.Pointer.AsRefStructSpan<TItem>(count);
    }

    /// <summary>
    /// Provides extension methods for ANSI string pointer types.
    /// </summary>
    public static class AnsiStringPointer
    {
        /// <summary>
        /// Marshals the pointer and copies the referenced string to .NET
        /// <see cref="string"/> value.
        /// </summary>
        /// <typeparam name="TPtr">The structural type implementing the <see cref="IAnsiStringPointer"/> interface.</typeparam>
        /// <param name="ptr">The pointer to marshal.</param>
        /// <param name="length">The number of ANSI characters to copy.</param>
        /// <returns>A <see cref="string"/> copy of the buffer represented by <paramref name="ptr"/>, or <see langword="null"/> if <paramref name="ptr"/> references <see cref="IntPtr.Zero"/>.</returns>
        public static string MarshalToString<TPtr>(this TPtr ptr, int length)
            where TPtr : struct, IAnsiStringPointer =>
            ptr.Pointer.MarshalAsAnsiString(length);

        /// <summary>
        /// Marshals the pointer and copies the referenced string to .NET
        /// <see cref="string"/> value. 
        /// </summary>
        /// <typeparam name="TPtr">The structural type implementing the <see cref="ITerminatedAnsiStringPointer"/> interface.</typeparam>
        /// <param name="ptr">The pointer to marshal.</param>
        /// <returns>A <see cref="string"/> copy of the buffer represented by <paramref name="ptr"/>, or <see langword="null"/> if <paramref name="ptr"/> references <see cref="IntPtr.Zero"/>.</returns>
        public static string MarshalToString<TPtr>(this TPtr ptr)
            where TPtr : struct, ITerminatedAnsiStringPointer =>
            ptr.Pointer.MarshalAsAnsiString();

        /// <summary>
        /// Determines the length of the references data by search for the first
        /// ANSI null-character byte, and returns a <see cref="Span{Byte}"/> up
        /// to that position.
        /// </summary>
        /// <typeparam name="TPtr">The structural type implementing the <see cref="ITerminatedAnsiStringPointer"/> interface.</typeparam>
        /// <param name="ptr">The pointer to interpret</param>
        /// <returns>
        /// A <see cref="Span{Byte}"/> up to but excluding the first ANSI
        /// null-character byte.
        /// </returns>
        public static Span<byte> GetTerminatedSpan<TPtr>(this TPtr ptr)
            where TPtr : struct, ITerminatedAnsiStringPointer =>
            ptr.Pointer.ToZeroTerminatedByteSpan();
    }

    /// <summary>
    /// Provides extension methods for read-only ANSI string pointer types.
    /// </summary>
    public static class ConstAnsiStringPointer
    {
        /// <summary>
        /// Marshals the pointer and copies the referenced string to .NET
        /// <see cref="string"/> value.
        /// </summary>
        /// <typeparam name="TPtr">The structural type implementing the <see cref="IConstAnsiStringPointer"/> interface.</typeparam>
        /// <param name="ptr">The pointer to marshal.</param>
        /// <param name="length">The number of ANSI characters to copy.</param>
        /// <returns>A <see cref="string"/> copy of the buffer represented by <paramref name="ptr"/>, or <see langword="null"/> if <paramref name="ptr"/> references <see cref="IntPtr.Zero"/>.</returns>
        public static string MarshalToString<TPtr>(this TPtr ptr, int length)
            where TPtr : struct, IConstAnsiStringPointer =>
            ptr.Pointer.MarshalAsAnsiString(length);

        /// <summary>
        /// Marshals the pointer and copies the referenced string to .NET
        /// <see cref="string"/> value.
        /// </summary>
        /// <typeparam name="TPtr">The structural type implementing the <see cref="IConstTerminatedAnsiStringPointer"/> interface.</typeparam>
        /// <param name="ptr">The pointer to marshal.</param>
        /// <returns>A <see cref="string"/> copy of the buffer represented by <paramref name="ptr"/>, or <see langword="null"/> if <paramref name="ptr"/> references <see cref="IntPtr.Zero"/>.</returns>
        public static string MarshalToString<TPtr>(this TPtr ptr)
            where TPtr : struct, IConstTerminatedAnsiStringPointer =>
            ptr.Pointer.MarshalAsAnsiString();

        /// <summary>
        /// Determines the length of the references data by search for the first
        /// ANSI null-character byte, and returns a
        /// <see cref="ReadOnlySpan{Byte}"/> up to that position.
        /// </summary>
        /// <typeparam name="TPtr">The structural type implementing the <see cref="IConstTerminatedAnsiStringPointer"/> interface.</typeparam>
        /// <param name="ptr">The pointer to interpret</param>
        /// <returns>
        /// A <see cref="ReadOnlySpan{Byte}"/> up to but excluding the first
        /// ANSI null-character byte.
        /// </returns>
        public static ReadOnlySpan<byte> GetTerminatedSpan<TPtr>(this TPtr ptr)
            where TPtr : struct, IConstTerminatedAnsiStringPointer =>
            ptr.Pointer.ToZeroTerminatedByteSpan();
    }

    /// <summary>
    /// Provides extension methods for wide-character UTF-16 string pointer types.
    /// </summary>
    public static class UnicodeStringPointer
    {
        /// <summary>
        /// Marshals the pointer and copies the referenced string to .NET
        /// <see cref="string"/> value.
        /// </summary>
        /// <typeparam name="TPtr">The structural type implementing the <see cref="IUnicodeStringPointer"/> interface.</typeparam>
        /// <param name="ptr">The pointer to marshal.</param>
        /// <param name="length">The number of characters to copy.</param>
        /// <returns>A <see cref="string"/> copy of the buffer represented by <paramref name="ptr"/>, or <see langword="null"/> if <paramref name="ptr"/> references <see cref="IntPtr.Zero"/>.</returns>
        public static string MarshalToString<TPtr>(this TPtr ptr, int length)
            where TPtr : struct, IUnicodeStringPointer =>
            ptr.Pointer.MarshalAsUnicodeString(length);

        /// <summary>
        /// Marshals the pointer and copies the referenced string to .NET
        /// <see cref="string"/> value.
        /// </summary>
        /// <typeparam name="TPtr">The structural type implementing the <see cref="ITerminatedUnicodeStringPointer"/> interface.</typeparam>
        /// <param name="ptr">The pointer to marshal.</param>
        /// <returns>A <see cref="string"/> copy of the buffer represented by <paramref name="ptr"/>, or <see langword="null"/> if <paramref name="ptr"/> references <see cref="IntPtr.Zero"/>.</returns>
        public static string MarshalToString<TPtr>(this TPtr ptr)
            where TPtr : struct, ITerminatedUnicodeStringPointer =>
            ptr.Pointer.MarshalAsUnicodeString();

        /// <summary>
        /// Determines the length of the references data by search for the first
        /// null-character, and returns a <see cref="Span{Char}"/> up to that
        /// position.
        /// </summary>
        /// <typeparam name="TPtr">The structural type implementing the <see cref="ITerminatedUnicodeStringPointer"/> interface.</typeparam>
        /// <param name="ptr">The pointer to interpret</param>
        /// <returns>
        /// A <see cref="Span{Char}"/> up to but excluding the first
        /// null-character byte.
        /// </returns>
        public static Span<char> GetTerminatedSpan<TPtr>(this TPtr ptr)
            where TPtr : struct, ITerminatedUnicodeStringPointer =>
            ptr.Pointer.ToZeroTerminatedUnicodeSpan();
    }

    /// <summary>
    /// Provides extension methods for read-only wide-character UTF-16 string pointer types.
    /// </summary>
    public static class ConstUnicodePointer
    {
        /// <summary>
        /// Marshals the pointer and copies the referenced string to .NET
        /// <see cref="string"/> value.
        /// </summary>
        /// <typeparam name="TPtr">The structural type implementing the <see cref="IConstUnicodeStringPointer"/> interface.</typeparam>
        /// <param name="ptr">The pointer to marshal.</param>
        /// <param name="length">The number of characters to copy.</param>
        /// <returns>A <see cref="string"/> copy of the buffer represented by <paramref name="ptr"/>, or <see langword="null"/> if <paramref name="ptr"/> references <see cref="IntPtr.Zero"/>.</returns>
        public static string MarshalToString<TPtr>(this TPtr ptr, int length)
            where TPtr : struct, IConstUnicodeStringPointer =>
            ptr.Pointer.MarshalAsUnicodeString(length);

        /// <summary>
        /// Marshals the pointer and copies the referenced string to .NET
        /// <see cref="string"/> value.
        /// </summary>
        /// <typeparam name="TPtr">The structural type implementing the <see cref="IConstTerminatedUnicodeStringPointer"/> interface.</typeparam>
        /// <param name="ptr">The pointer to marshal.</param>
        /// <returns>A <see cref="string"/> copy of the buffer represented by <paramref name="ptr"/>, or <see langword="null"/> if <paramref name="ptr"/> references <see cref="IntPtr.Zero"/>.</returns>
        public static string MarshalToString<TPtr>(this TPtr ptr)
            where TPtr : struct, IConstTerminatedUnicodeStringPointer =>
            ptr.Pointer.MarshalAsUnicodeString();

        /// <summary>
        /// Determines the length of the references data by search for the first
        /// null-character, and returns a <see cref="ReadOnlySpan{Char}"/> up to
        /// that position.
        /// </summary>
        /// <typeparam name="TPtr">The structural type implementing the <see cref="IConstTerminatedUnicodeStringPointer"/> interface.</typeparam>
        /// <param name="ptr">The pointer to interpret</param>
        /// <returns>
        /// A <see cref="ReadOnlySpan{Char}"/> up to but excluding the first
        /// null-character byte.
        /// </returns>
        public static ReadOnlySpan<char> GetTerminatedSpan<TPtr>(this TPtr ptr)
            where TPtr : struct, IConstTerminatedUnicodeStringPointer =>
            ptr.Pointer.ToZeroTerminatedUnicodeSpan();
    }

    /// <summary>
    /// Provides extension methods for platform-dependant string pointer types.
    /// </summary>
    public static class AutoStringPointer
    {
        /// <summary>
        /// Marshals the pointer and copies the referenced string to .NET
        /// <see cref="string"/> value.
        /// </summary>
        /// <typeparam name="TPtr">The structural type implementing the <see cref="IAutoStringPointer"/> interface.</typeparam>
        /// <param name="ptr">The pointer to marshal.</param>
        /// <param name="length">The number of characters to copy.</param>
        /// <returns>A <see cref="string"/> copy of the buffer represented by <paramref name="ptr"/>, or <see langword="null"/> if <paramref name="ptr"/> references <see cref="IntPtr.Zero"/>.</returns>
        public static string MarshalToString<TPtr>(this TPtr ptr, int length)
            where TPtr : struct, IAutoStringPointer =>
            ptr.Pointer.MarshalAsAutoString(length);

        /// <summary>
        /// Marshals the pointer and copies the referenced string to .NET
        /// <see cref="string"/> value.
        /// </summary>
        /// <typeparam name="TPtr">The structural type implementing the <see cref="IAutoStringPointer"/> interface.</typeparam>
        /// <param name="ptr">The pointer to marshal.</param>
        /// <returns>A <see cref="string"/> copy of the buffer represented by <paramref name="ptr"/>, or <see langword="null"/> if <paramref name="ptr"/> references <see cref="IntPtr.Zero"/>.</returns>
        public static string MarshalToString<TPtr>(this TPtr ptr)
            where TPtr : struct, ITerminatedAutoStringPointer =>
            ptr.Pointer.MarshalAsAutoString();
    }

    /// <summary>
    /// Provides extension methods for read-only platform-dependant string pointer types.
    /// </summary>
    public static class ConstAutoStringPointer
    {
        /// <summary>
        /// Marshals the pointer and copies the referenced string to .NET
        /// <see cref="string"/> value.
        /// </summary>
        /// <typeparam name="TPtr">The structural type implementing the <see cref="IConstAutoStringPointer"/> interface.</typeparam>
        /// <param name="ptr">The pointer to marshal.</param>
        /// <param name="length">The number of characters to copy.</param>
        /// <returns>A <see cref="string"/> copy of the buffer represented by <paramref name="ptr"/>, or <see langword="null"/> if <paramref name="ptr"/> references <see cref="IntPtr.Zero"/>.</returns>
        public static string MarshalToString<TPtr>(this TPtr ptr, int length)
            where TPtr : struct, IConstAutoStringPointer =>
            ptr.Pointer.MarshalAsAutoString(length);

        /// <summary>
        /// Marshals the pointer and copies the referenced string to .NET
        /// <see cref="string"/> value.
        /// </summary>
        /// <typeparam name="TPtr">The structural type implementing the <see cref="IConstTerminatedAutoStringPointer"/> interface.</typeparam>
        /// <param name="ptr">The pointer to marshal.</param>
        /// <returns>A <see cref="string"/> copy of the buffer represented by <paramref name="ptr"/>, or <see langword="null"/> if <paramref name="ptr"/> references <see cref="IntPtr.Zero"/>.</returns>
        public static string MarshalToString<TPtr>(this TPtr ptr)
            where TPtr : struct, IConstTerminatedAutoStringPointer =>
            ptr.Pointer.MarshalAsAutoString();
    }

    /// <summary>
    /// Static helper class that provides extension methods for function pointer interface types.
    /// </summary>
    public static class FunctionPointer
    {
        /// <summary>
        /// Creates a function pointer from the specified delegate value.
        /// </summary>
        /// <typeparam name="TPtr">The structural type to store the marshalled function pointer.</typeparam>
        /// <typeparam name="TProc">An unmanaged delegate type, typically attributed with the <see cref="UnmanagedFunctionPointerAttribute"/>.</typeparam>
        /// <param name="proc">The delegate to be marshalled to a function pointer.</param>
        /// <returns>A pointer structure storing the value of the marshalled pointer.</returns>
        /// <remarks>If <paramref name="proc"/> is <see langword="null"/>, the returned pointer has a value of <see cref="IntPtr.Zero"/>.</remarks>
        public static TPtr Create<TPtr, TProc>(TProc proc)
            where TProc : Delegate
            where TPtr : struct, IFunctionPointer<TProc> =>
            Pointer.Create<TPtr>(proc is null ? IntPtr.Zero : Marshal.GetFunctionPointerForDelegate(proc));

        /// <summary>
        /// Gets a managed invocable delegate from a previously marshalled unmanaged function pointer.
        /// </summary>
        /// <typeparam name="TPtr">The structural type to store the marshalled function pointer.</typeparam>
        /// <typeparam name="TProc">An unmanaged delegate type, typically attributed with the <see cref="UnmanagedFunctionPointerAttribute"/>.</typeparam>
        /// <param name="ptr">The function pointer structure storing the unmanaged function pointer.</param>
        /// <returns>A <typeparamref name="TProc"/> delegate instance or <see langword="null"/> if <paramref name="ptr"/> has a <see cref="IntPtr.Zero"/> value.</returns>
        public static TProc GetDelegate<TPtr, TProc>(this TPtr ptr)
            where TProc : Delegate
            where TPtr : struct, IFunctionPointer<TProc> =>
            ptr.HasValue() ? Marshal.GetDelegateForFunctionPointer<TProc>(ptr.Pointer) : null;
    }

    /// <summary>
    /// Provides a default pointer value comparer, to compare two pointers of
    /// type <typeparamref name="TPtr"/>.
    /// </summary>
    /// <typeparam name="TPtr">The structural type of the pointers to compare.</typeparam>
    public sealed class PointerComparer<TPtr> : IComparer<TPtr>, IEqualityComparer<TPtr>
        where TPtr : struct, IPointer
    {
        /// <summary>
        /// Gets a static singleton instance for the <see cref="PointerComparer{TPtr}"/>.
        /// </summary>
        [SuppressMessage("Design", "CA1000: Do not declare static members on generic types")]
        public static PointerComparer<TPtr> Instance { get; } =
            new PointerComparer<TPtr>();

        private PointerComparer() : base() { }

        /// <inheritdoc cref="IComparer{TPtr}.Compare(TPtr, TPtr)"/>
        public int Compare(TPtr x, TPtr y)
        {
            (ulong ulx, ulong uly) =
                ((ulong)(x.Pointer.ToInt64()), (ulong)(y.Pointer.ToInt64()));
            return ulx.CompareTo(uly);
        }

        /// <inheritdoc cref="IEqualityComparer{TPtr}.Equals(TPtr, TPtr)"/>
        public bool Equals(TPtr x, TPtr y) => x.Pointer == y.Pointer;

        /// <inheritdoc cref="IEqualityComparer{TPtr}.GetHashCode(TPtr)"/>
        public int GetHashCode(TPtr ptr) => ptr.Pointer.GetHashCode();
    }
}
