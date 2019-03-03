using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

#if !NETSTANDARD1_3
using System;
#endif // !NETSTANDARD1_3

namespace THNETII.InteropServices.NativeMemory
{
#if !NETSTANDARD1_3
    /// <summary>
    /// Caches the marshaled size of a <see cref="Type"/> from Reflection.
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + "(),nq}")]
    public sealed class SizeOf
    {
        internal SizeOf(Type type, int bytes) : this(type, bytes, bits: bytes * 8) { }
        internal SizeOf(Type type, int bytes, int bits) : base()
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Bytes = bytes;
            Bits = bits;
        }

        /// <summary>
        /// Gets the type of the stored size.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Gets the number of bytes that the type will occupy in native memory
        /// when marshaled.
        /// </summary>
        /// <value>The cached result from calling the static <see cref="Marshal.SizeOf{T}()"/> method of the <see cref="Marshal"/> class.</value>
        public int Bytes { get; }

        /// <summary>
        /// Gets the number of bits that the type will occupy in native memory
        /// when marshaled.
        /// </summary>
        /// <value><c><see cref="Bytes"/> * 8</c></value>
        /// <remarks>One Byte is assumed to hold 8 bits.</remarks>
        public int Bits { get; }

        /// <summary>
        /// Gets a <see cref="SizeOf"/> instance representing the marshaled size for the specified type.
        /// </summary>
        /// <param name="type">The type whose size is to be returned.</param>
        /// <returns>A new or cached <see cref="SizeOf"/> instance obtained from the <see cref="SizeOf{T}.Default"/> property of the static <see cref="SizeOf{T}"/> type.</returns>
        public static SizeOf GetSizeOf(Type type)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            var genericSizeOf = typeof(SizeOf<>).MakeGenericType(type)
#if NETSTANDARD1_6
                .GetTypeInfo()
#endif // NETSTANDARD1_6
                ;
            var defaultPropertyInfo = genericSizeOf.GetProperty(nameof(SizeOf<int>.Default), BindingFlags.Public | BindingFlags.Static);

            return (SizeOf)defaultPropertyInfo.GetValue(null);
        }

        private string DebuggerDisplay() => FormattableString.Invariant($"SizeOf<{Type}>, {nameof(Bytes)} = {Bytes}, {nameof(Bits)} = {Bits}");
    }
#endif // !NETSTANDARD1_3

    /// <summary>
    /// Statically stores the marshaled size of a type.
    /// </summary>
    /// <typeparam name="T">The type to marshal.</typeparam>
    [SuppressMessage("Design", "CA1000: Do not declare static members on generic types")]
    public static class SizeOf<T>
    {
        private static int CalculateByteSize()
        {
            var type = typeof(T)
#if NETSTANDARD1_3 || NETSTANDARD1_6
                .GetTypeInfo()
#endif // NETSTANDARD1_3 || NETSTANDARD1_6
                ;
            if (type.IsPrimitive && type.IsValueType)
                return Unsafe.SizeOf<T>();
            return Marshal.SizeOf<T>();
        }

        /// <summary>
        /// Gets the number of bytes that the type will occupy in native memory
        /// when marshaled.
        /// </summary>
        /// <value>The cached result from calling the static <see cref="Marshal.SizeOf{T}()"/> method of the <see cref="Marshal"/> class.</value>
        /// <remarks>For primitive value tasks the more reliable <see cref="Unsafe.SizeOf{T}"/> method of the <see cref="Unsafe"/> class is used.</remarks>
        public static int Bytes { get; } = CalculateByteSize();

        /// <summary>
        /// Gets the number of bits that the type will occupy in native memory
        /// when marshaled.
        /// </summary>
        /// <value><c><see cref="Bytes"/> * 8</c></value>
        /// <remarks>One Byte is assumed to hold 8 bits.</remarks>
        public static int Bits { get; } = Bytes * 8;

#if !NETSTANDARD1_3
        /// <summary>
        /// Gets a cached <see cref="SizeOf"/> instance representing the marshaled size of <typeparamref name="T"/>.
        /// </summary>
        /// <value>A <see cref="SizeOf"/> singleton instance that is initialized with the values from <see cref="Bytes"/> and <see cref="Bits"/>.</value>
        public static SizeOf Default { get; } = new SizeOf(typeof(T), Bytes, Bits);
#endif // !NETSTANDARD1_3
    }
}
