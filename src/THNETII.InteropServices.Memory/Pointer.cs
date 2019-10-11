using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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

    public interface IAnsiStringPointer : IArrayPointer<byte> { }

    public interface IConstAnsiStringPointer : IConstArrayPointer<byte> { }

    public interface IUnicodeStringPointer : IArrayPointer<char> { }

    public interface IConstUnicodeStringPointer : IConstArrayPointer<char> { }

    public interface IAutoStringPointer : IArrayPointer<byte> { }

    public interface IConstAutoStringPointer : IConstArrayPointer<byte> { }

    public interface ITerminatedAnsiStringPointer : IAnsiStringPointer { }

    public interface IConstTerminatedAnsiStringPointer : IConstAnsiStringPointer { }

    public interface ITerminatedUnicodeStringPointer : IUnicodeStringPointer { }

    public interface IConstTerminatedUnicodeStringPointer : IConstUnicodeStringPointer { }

    public interface ITerminatedAutoStringPointer : IAutoStringPointer { }

    public interface IConstTerminatedAutoStringPointer : IConstAutoStringPointer { }

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

        public static T Create<T>(IntPtr ptr) where T : unmanaged, IPointer
        {
            PointerBlit<T> blit = new PointerBlit<T> { ptr = ptr };
            return blit.@struct;
        }

        public static bool HasValue<T>(this T ptr) where T : struct, IPointer =>
            ptr.Pointer != IntPtr.Zero;

        public static ref TValue GetReference<TPtr, TValue>(this TPtr ptr)
            where TPtr : struct, IPointer<TValue> where TValue : struct =>
            ref ptr.Pointer.AsRefStruct<TValue>();

        public static ref readonly TValue GetReadOnlyReference<TPtr, TValue>(this TPtr ptr)
            where TPtr : struct, IPointer<TValue> where TValue : struct =>
            ref ptr.Pointer.AsRefStruct<TValue>();

        public static Span<TItem> GetSpan<TPtr, TItem>(this TPtr ptr,
            int count)
            where TPtr : struct, IArrayPointer<TItem>
            where TItem : struct =>
            ptr.Pointer.AsRefStructSpan<TItem>(count);

        public static ReadOnlySpan<TItem> GetReadOnlySpan<TPtr, TItem>(
            this TPtr ptr, int count)
            where TPtr : struct, IConstArrayPointer<TItem>
            where TItem : struct =>
            ptr.Pointer.AsRefStructSpan<TItem>(count);
    }

    public static class AnsiStringPointer
    {
        public static string MarshalToString<TPtr>(this TPtr ptr, int length)
            where TPtr : struct, IAnsiStringPointer =>
            ptr.Pointer.MarshalAsAnsiString(length);

        public static string MarshalToString<TPtr>(this TPtr ptr)
            where TPtr : struct, ITerminatedAnsiStringPointer =>
            ptr.Pointer.MarshalAsAnsiString();

        public static Span<byte> GetTerminatedSpan<TPtr>(this TPtr ptr)
            where TPtr : struct, ITerminatedAnsiStringPointer =>
            ptr.Pointer.ToZeroTerminatedByteSpan();
    }

    public static class ConstAnsiStringPointer
    {
        public static string MarshalToString<TPtr>(this TPtr ptr, int length)
            where TPtr : struct, IConstAnsiStringPointer =>
            ptr.Pointer.MarshalAsAnsiString(length);

        public static string MarshalToString<TPtr>(this TPtr ptr)
            where TPtr : struct, IConstTerminatedAnsiStringPointer =>
            ptr.Pointer.MarshalAsAnsiString();

        public static ReadOnlySpan<byte> GetTerminatedSpan<TPtr>(this TPtr ptr)
            where TPtr : struct, ITerminatedAnsiStringPointer =>
            ptr.Pointer.ToZeroTerminatedByteSpan();
    }

    public static class UnicodeStringPointer
    {
        public static string MarshalToString<TPtr>(this TPtr ptr, int length)
            where TPtr : struct, IUnicodeStringPointer =>
            ptr.Pointer.MarshalAsUnicodeString(length);

        public static string MarshalToString<TPtr>(this TPtr ptr)
            where TPtr : struct, ITerminatedUnicodeStringPointer =>
            ptr.Pointer.MarshalAsUnicodeString();

        public static Span<char> GetTerminatedSpan<TPtr>(this TPtr ptr)
            where TPtr : struct, ITerminatedUnicodeStringPointer =>
            ptr.Pointer.ToZeroTerminatedUnicodeSpan();
    }

    public static class ConstUnicodePointer
    {
        public static string MarshalToString<TPtr>(this TPtr ptr, int length)
            where TPtr : struct, IConstUnicodeStringPointer =>
            ptr.Pointer.MarshalAsUnicodeString(length);

        public static string MarshalToString<TPtr>(this TPtr ptr)
            where TPtr : struct, IConstTerminatedUnicodeStringPointer =>
            ptr.Pointer.MarshalAsUnicodeString();

        public static ReadOnlySpan<char> GetTerminatedSpan<TPtr>(this TPtr ptr)
            where TPtr : struct, IConstTerminatedUnicodeStringPointer =>
            ptr.Pointer.ToZeroTerminatedUnicodeSpan();
    }

    public static class AutoStringPointer
    {
        public static string MarshalToString<TPtr>(this TPtr ptr, int length)
            where TPtr : struct, IAutoStringPointer =>
            ptr.Pointer.MarshalAsAutoString(length);

        public static string MarshalToString<TPtr>(this TPtr ptr)
            where TPtr : struct, ITerminatedAutoStringPointer =>
            ptr.Pointer.MarshalAsAutoString();
    }

    public static class ConstAutoStringPointer
    {
        public static string MarshalToString<TPtr>(this TPtr ptr, int length)
            where TPtr : struct, IConstAutoStringPointer =>
            ptr.Pointer.MarshalAsAutoString(length);

        public static string MarshalToString<TPtr>(this TPtr ptr)
            where TPtr : struct, IConstTerminatedAutoStringPointer =>
            ptr.Pointer.MarshalAsAutoString();
    }

    public sealed class PointerComparer<TPtr> : IComparer<TPtr>, IEqualityComparer<TPtr>
        where TPtr : IPointer
    {
        [SuppressMessage("Design", "CA1000: Do not declare static members on generic types")]
        public static PointerComparer<TPtr> Instance { get; } =
            new PointerComparer<TPtr>();

        private PointerComparer() : base() { }

        public int Compare(TPtr x, TPtr y)
        {
            (ulong ulx, ulong uly) =
                ((ulong)(x.Pointer.ToInt64()), (ulong)(y.Pointer.ToInt64()));
            return ulx.CompareTo(uly);
        }

        public bool Equals(TPtr x, TPtr y) => x.Pointer == y.Pointer;

        public int GetHashCode(TPtr ptr) => ptr.Pointer.GetHashCode();
    }
}
