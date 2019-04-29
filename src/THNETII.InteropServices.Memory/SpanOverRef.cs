using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#if NETSTANDARD1_3
using System.Linq;
#endif // NETSTANDARD1_3

namespace THNETII.InteropServices.Memory
{
    /// <summary>
    /// Supplies Runtime helper methods to create spans over ref and in values.
    /// </summary>
    public static class SpanOverRef
    {
        private const string CreateSpan = nameof(CreateSpan);
        private const string CreateReadOnlySpan = nameof(CreateReadOnlySpan);

        private static readonly MethodInfo CreateSpanGenericDefinition;
        private static readonly MethodInfo CreateReadOnlySpanGenericDefinition;

        [SuppressMessage("Performance", "CA1810:Initialize reference type static fields inline", Justification = "Optimization")]
        static SpanOverRef()
        {
            var memoryMarshalMethods = typeof(MemoryMarshal)
#if NETSTANDARD1_3 || NETSTANDARD1_6
                .GetTypeInfo()
#endif // NETSTANDARD1_3 || NETSTANDARD1_6
#if NETSTANDARD1_3
                .GetDeclaredMethods(CreateSpan)
                .Concat(typeof(MemoryMarshal).GetTypeInfo().GetDeclaredMethods(CreateReadOnlySpan))
#else // NETSTANDARD1_3
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
#endif
                ;
            foreach (MethodInfo mi in memoryMarshalMethods)
            {
                if (CreateSpanGenericDefinition != null &&
                    CreateReadOnlySpanGenericDefinition != null)
                    break;
                switch (mi.Name)
                {
                    case CreateSpan:
                        AssignMethodInfoIfCorrect(mi, ref CreateSpanGenericDefinition);
                        break;
                    case CreateReadOnlySpan:
                        AssignMethodInfoIfCorrect(mi, ref CreateReadOnlySpanGenericDefinition);
                        break;
                }
            }
        }

        private static void AssignMethodInfoIfCorrect(MethodInfo mi, ref MethodInfo target)
        {
            if (!mi.IsGenericMethod)
                return;
            var @params = mi.GetParameters();
            if (@params.Length != 2)
                return;
            var refParam = @params[0];
            if (!refParam.ParameterType.IsByRef)
                return;
            var lengthParam = @params[1];
            Type intTypeRef = typeof(int);
            if (lengthParam.ParameterType != intTypeRef)
                return;
            target = mi;
        }

        /// <summary>
        /// Gets a value indicating whether creating spans is supported by the runtime.
        /// </summary>
        /// <remarks>
        /// Starting with .NET Core 2.1, the .NET Core runtime supports the
        /// creation of Spans over <c>ref</c> and <c>in</c> values without needing
        /// to pin them in memory first.
        /// <para>
        /// Without the runtime support, Spans need to be constructed by retrieving
        /// the pointer to the value. For values that are stored on the heap,
        /// their containing object (or array) needs to be pinned first. Values
        /// on the stack are not subject to garbage collection and can thus be
        /// safely created regardless of runtime support.
        /// </para>
        /// </remarks>
        public static bool IsCreateSpanSupported => CreateSpanGenericDefinition != null;

        internal delegate Span<T> CreateSpanDelegate<T>(ref T reference, int length);
        internal delegate ReadOnlySpan<T> CreateReadOnlySpanDelegate<T>(ref T reference, int length);

        internal static class CreateSpanInvoker<T>
        {
            private static readonly Lazy<CreateSpanDelegate<T>> createSpanLazy =
                new Lazy<CreateSpanDelegate<T>>(MakeCreateSpan);

            private static CreateSpanDelegate<T> MakeCreateSpan()
            {
                return (CreateSpanDelegate<T>)CreateSpanGenericDefinition?
                    .MakeGenericMethod(typeof(T))
                    .CreateDelegate(typeof(CreateSpanDelegate<T>));
            }

            private static readonly Lazy<CreateReadOnlySpanDelegate<T>> createReadOnlySpanLazy =
                new Lazy<CreateReadOnlySpanDelegate<T>>(MakeCreateReadOnlySpan);

            private static CreateReadOnlySpanDelegate<T> MakeCreateReadOnlySpan()
            {
                return (CreateReadOnlySpanDelegate<T>)CreateReadOnlySpanGenericDefinition?
                    .MakeGenericMethod(typeof(T))
                    .CreateDelegate(typeof(CreateReadOnlySpanDelegate<T>));
            }

            internal static CreateSpanDelegate<T> CreateSpan =>
                createSpanLazy.Value;
            internal static CreateReadOnlySpanDelegate<T> CreateReadOnlySpan =>
                createReadOnlySpanLazy.Value;
        }

        /// <summary>
        /// Creates a writable span of the specified reference and, optionally,
        /// over the values coming after it in contiguous memory.
        /// </summary>
        /// <typeparam name="T">The type of the item in the returned span.</typeparam>
        /// <param name="reference">A reference to the value to span over.</param>
        /// <param name="length">The number of elements to span over, defaults to <c>1</c>.</param>
        /// <returns>
        /// A <see cref="Span{T}"/> starting at <paramref name="reference"/> with the specified <paramref name="length"/>.
        /// </returns>
        /// <remarks>
        /// <note>
        /// If <see cref="IsCreateSpanSupported"/> is <see langword="false"/>, calling this
        /// method is unsafe and can lead to access violation exceptions when
        /// <paramref name="reference"/> is located on the heap without being fixed/pinned.
        /// In such cases, you should call <see cref="GetPinnedSpan{T}(ref T, object, out GCHandle, int)"/> instead.
        /// </note>
        /// <para>
        /// If <paramref name="reference"/> is stack-local, the returned span is
        /// only valid as long as <paramref name="reference"/> is valid on its
        /// stack frame.
        /// </para>
        /// </remarks>
        public static Span<T> GetSpan<T>(ref T reference, int length = 1)
            where T : struct
        {
            if (IsCreateSpanSupported)
                return CreateSpanUnsafe(ref reference, length);
            else
                return GetFixedPointerSpan(ref reference, length);
        }

        /// <summary>
        /// Creates a read-only span of the specified reference and, optionally,
        /// over the values coming after it in contiguous memory.
        /// </summary>
        /// <typeparam name="T">The type of the item in the returned span.</typeparam>
        /// <param name="reference">A reference to the value to span over.</param>
        /// <param name="length">The number of elements to span over, defaults to <c>1</c>.</param>
        /// <returns>
        /// A <see cref="ReadOnlySpan{T}"/> starting at <paramref name="reference"/> with the specified <paramref name="length"/>.
        /// </returns>
        /// <remarks>
        /// <note>
        /// If <see cref="IsCreateSpanSupported"/> is <see langword="false"/>, calling this
        /// method is unsafe and can lead to access violation exceptions when
        /// <paramref name="reference"/> is located on the heap without being fixed/pinned.
        /// In such cases, you should call <see cref="GetPinnedSpan{T}(ref T, object, out GCHandle, int)"/> instead.
        /// </note>
        /// <para>
        /// If <paramref name="reference"/> is stack-local, the returned span is
        /// only valid as long as <paramref name="reference"/> is valid on its
        /// stack frame.
        /// </para>
        /// </remarks>
        public static ReadOnlySpan<T> GetReadOnlySpan<T>(in T reference, int length = 1)
            where T : struct
        {
            if (IsCreateSpanSupported)
                return CreateReadOnlySpanUnsafe(reference, length);
            else
                return GetFixedPointerReadOnlySpan(reference, length);
        }

        /// <summary>
        /// Creates a writable span of the specified reference and
        /// pins the object it is contained in to ensure the span remains valid.
        /// </summary>
        /// <typeparam name="T">The type of the item in the returned span.</typeparam>
        /// <param name="reference">A reference to the value to span over.</param>
        /// <param name="container">The object on the heap where <paramref name="reference"/> is contained in.</param>
        /// <param name="pinnedHandle">Receives the <see cref="GCHandle"/> that was allocated for pinning <paramref name="container"/>. <see cref="GCHandle.IsAllocated"/> must be checked before calling <see cref="GCHandle.Free"/>.</param>
        /// <param name="length">The number of elements to span over, defaults to <c>1</c>.</param>
        /// <returns>
        /// A <see cref="Span{T}"/> starting at <paramref name="reference"/> with the specified <paramref name="length"/>.
        /// </returns>
        /// <remarks>
        /// If <see cref="IsCreateSpanSupported"/> returns <see langword="true"/>, the runtime
        /// supports creating spans without pinning <paramref name="container"/>. In that case,
        /// <paramref name="pinnedHandle"/> is set to its default value and its
        /// <see cref="GCHandle.IsAllocated"/> property will be <see langword="false"/>.
        /// <para>
        /// When done with the returned span, callers should call <see cref="GCHandle.Free"/> on the returned <paramref name="pinnedHandle"/>
        /// if and only if the <see cref="GCHandle.IsAllocated"/> property of <paramref name="pinnedHandle"/>
        /// is <see langword="true"/>.
        /// </para>
        /// </remarks>
        public static unsafe Span<T> GetPinnedSpan<T>(ref T reference, object container, out GCHandle pinnedHandle, int length = 1)
        {
            if (IsCreateSpanSupported)
            {
                pinnedHandle = default;
                return CreateSpanUnsafe(ref reference, length);
            }
            else
            {
                pinnedHandle = GCHandle.Alloc(container, GCHandleType.Pinned);
                return GetUnsafePointerSpan(ref reference, length);
            }
        }

        /// <summary>
        /// Creates a read-only span of the specified reference and
        /// pins the object it is contained in to ensure the span remains valid.
        /// </summary>
        /// <typeparam name="T">The type of the item in the returned span.</typeparam>
        /// <param name="reference">A reference to the value to span over.</param>
        /// <param name="container">The object on the heap where <paramref name="reference"/> is contained in.</param>
        /// <param name="pinnedHandle">Receives the <see cref="GCHandle"/> that was allocated for pinning <paramref name="container"/>. <see cref="GCHandle.IsAllocated"/> must be checked before calling <see cref="GCHandle.Free"/>.</param>
        /// <param name="length">The number of elements to span over, defaults to <c>1</c>.</param>
        /// <returns>
        /// A <see cref="ReadOnlySpan{T}"/> starting at <paramref name="reference"/> with the specified <paramref name="length"/>.
        /// </returns>
        /// <remarks>
        /// If <see cref="IsCreateSpanSupported"/> returns <see langword="true"/>, the runtime
        /// supports creating spans without pinning <paramref name="container"/>. In that case,
        /// <paramref name="pinnedHandle"/> is set to its default value and its
        /// <see cref="GCHandle.IsAllocated"/> property will be <see langword="false"/>.
        /// <para>
        /// When done with the returned span, callers should call <see cref="GCHandle.Free"/> on the returned <paramref name="pinnedHandle"/>
        /// if and only if the <see cref="GCHandle.IsAllocated"/> property of <paramref name="pinnedHandle"/>
        /// is <see langword="true"/>.
        /// </para>
        /// </remarks>
        public static unsafe ReadOnlySpan<T> GetPinnedReadOnlySpan<T>(in T reference, object container, out GCHandle pinnedHandle, int length = 1)
        {
            if (IsCreateSpanSupported)
            {
                pinnedHandle = default;
                return CreateReadOnlySpanUnsafe(reference, length);
            }
            else
            {
                pinnedHandle = GCHandle.Alloc(container, GCHandleType.Pinned);
                return GetUnsafePointerSpan(ref Unsafe.AsRef(reference), length);
            }
        }

        private static Span<T> CreateSpanUnsafe<T>(ref T input, int length = 1) =>
            CreateSpanInvoker<T>.CreateSpan(ref input, length);

        private static ReadOnlySpan<T> CreateReadOnlySpanUnsafe<T>(in T input, int length = 1) =>
            CreateSpanInvoker<T>.CreateReadOnlySpan(ref Unsafe.AsRef(input), length);

        private static unsafe Span<T> GetUnsafePointerSpan<T>(ref T input, int length = 1)
        {
            void* ptr = Unsafe.AsPointer(ref input);
            return new Span<T>(ptr, length);
        }

        internal static unsafe Span<T> GetFixedPointerSpanUnmanaged<T>(ref T input, int length = 1)
            where T : unmanaged
        {
            fixed (T* ptr = &input)
            {
                return new Span<T>(ptr, length);
            }
        }

        private delegate Span<T> FixedPointerSpanDelegate<T>(ref T input, int length = 1)
            where T : struct;

        private static readonly Lazy<MethodInfo> GetFixedPointerSpanUnmanagedLazy =
            new Lazy<MethodInfo>(GetFixedPointerSpanUnmanagedGenericDefinition);

        private static MethodInfo GetFixedPointerSpanUnmanagedGenericDefinition()
        {
            return typeof(SpanOverRef)
#if NETSTANDARD1_3 || NETSTANDARD1_6
                    .GetTypeInfo()
#endif // NETSTANDARD1_3 || NETSTANDARD1_6
#if NETSTANDARD1_3
                    .GetDeclaredMethod(nameof(GetFixedPointerSpanUnmanaged))
#else // !NETSTANDARD1_3
                    .GetMethod(nameof(GetFixedPointerSpanUnmanaged), BindingFlags.NonPublic | BindingFlags.Static)
#endif // !NETSTANDARD1_3
                    ;
        }

        private struct FixedPointerSpanInvoker<T> where T : struct
        {
            private static readonly Lazy<FixedPointerSpanDelegate<T>> delegateLazy =
                new Lazy<FixedPointerSpanDelegate<T>>(MakeDelegate);

            private static FixedPointerSpanDelegate<T> MakeDelegate()
            {
                var generic = GetFixedPointerSpanUnmanagedLazy.Value;
                return (FixedPointerSpanDelegate<T>)generic
                    .MakeGenericMethod(typeof(T))
                    .CreateDelegate(typeof(FixedPointerSpanDelegate<T>));
            }

            public static FixedPointerSpanDelegate<T> Invoke => delegateLazy.Value;
        }

        /// <summary>
        /// Gets a span constructed from the fixed pointer of the specified
        /// struct value, optionally, over the values coming after it in
        /// contiguous memory.
        /// </summary>
        /// <typeparam name="T">The type of the item(s) in the returned span.</typeparam>
        /// <param name="input">A reference to the value to span over.</param>
        /// <param name="length">The number of elements to span over, defaults to <c>1</c>.</param>
        /// <returns>
        /// A <see cref="Span{T}"/> starting at <paramref name="input"/> with the specified <paramref name="length"/>.
        /// </returns>
        /// <remarks>
        /// The value referenced by <paramref name="input"/> will only be fixed
        /// during construction of the <see cref="Span{T}"/>. Therefore, the
        /// returned span is only guranteed to remain safe if the value is not
        /// stored on the managed object heap.
        /// <para>
        /// If the executing runtime supports internal pointers, the returned
        /// Span is guaranteed to remain safe, even if the referred value is
        /// stored on the managed heap. If <see cref="IsCreateSpanSupported"/>
        /// returns <see langword="true"/>, this feature <em>should</em> be
        /// supported.
        /// </para>
        /// </remarks>
        /// <seealso cref="GetSpan{T}(ref T, int)"/>
        public static Span<T> GetFixedPointerSpan<T>(ref T input, int length = 1)
            where T : struct => FixedPointerSpanInvoker<T>.Invoke(ref input, length);

        /// <summary>
        /// Gets a read-only span constructed from the fixed pointer of the
        /// specified struct value, and optionally, over the values coming after
        /// it in contiguous memory.
        /// </summary>
        /// <typeparam name="T">The type of the item(s) in the returned span.</typeparam>
        /// <param name="input">A reference to the value to span over.</param>
        /// <param name="length">The number of elements to span over, defaults to <c>1</c>.</param>
        /// <returns>
        /// A <see cref="Span{T}"/> starting at <paramref name="input"/> with the specified <paramref name="length"/>.
        /// </returns>
        /// <remarks>
        /// The value referenced by <paramref name="input"/> will only be fixed
        /// during construction of the <see cref="Span{T}"/>. Therefore, the
        /// returned span is only guranteed to remain safe if the value is not
        /// stored on the managed object heap.
        /// <para>
        /// If the executing runtime supports internal pointers, the returned
        /// Span is guaranteed to remain safe, even if the referred value is
        /// stored on the managed heap. If <see cref="IsCreateSpanSupported"/>
        /// returns <see langword="true"/>, this feature <em>should</em> be
        /// supported.
        /// </para>
        /// </remarks>
        /// <seealso cref="GetReadOnlySpan{T}(in T, int)"/>
        public static ReadOnlySpan<T> GetFixedPointerReadOnlySpan<T>(in T input, int length = 1)
            where T : struct => GetFixedPointerSpan(ref Unsafe.AsRef(input), length);
    }
}
