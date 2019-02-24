using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.Runtime
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
#if NETSTANDARD1_6
                .GetTypeInfo()
#endif
                .GetMethods(BindingFlags.Public | BindingFlags.Static);
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
        /// Creates a writable single-item span of the specified reference.
        /// </summary>
        /// <typeparam name="T">The type of the item in the returned span.</typeparam>
        /// <param name="reference">A reference to the value to span over.</param>
        /// <returns>
        /// A <see cref="Span{T}"/> value with a length of <c>1</c>.
        /// </returns>
        /// <remarks>
        /// <note>
        /// If <see cref="IsCreateSpanSupported"/> is <c>false</c>, calling this
        /// method is unsafe and can lead to access violation exceptions when
        /// <paramref name="reference"/> is located on the heap without being fixed/pinned.
        /// In such cases, you should call <see cref="GetPinnedSpan{T}(ref T, object, out GCHandle)"/> instead.
        /// </note>
        /// <para>
        /// If <paramref name="reference"/> is stack-local, the returned span is
        /// only valid as long as <paramref name="reference"/> is valid on its
        /// stack frame.
        /// </para>
        /// </remarks>
        public static Span<T> GetSpan<T>(ref T reference)
        {
            if (IsCreateSpanSupported)
                return CreateSpanUnsafe(ref reference);
            else
                return GetStackLocalSpanUnsafe(ref reference);
        }

        /// <summary>
        /// Creates a read-only single-item span of the specified reference.
        /// </summary>
        /// <typeparam name="T">The type of the item in the returned span.</typeparam>
        /// <param name="reference">A reference to the value to span over.</param>
        /// <returns>
        /// A <see cref="ReadOnlySpan{T}"/> value with a length of <c>1</c>.
        /// </returns>
        /// <remarks>
        /// <note>
        /// If <see cref="IsCreateSpanSupported"/> is <c>false</c>, calling this
        /// method is unsafe and can lead to access violation exceptions when
        /// <paramref name="reference"/> is located on the heap without being fixed/pinned.
        /// In such cases, you should call <see cref="GetPinnedSpan{T}(ref T, object, out GCHandle)"/> instead.
        /// </note>
        /// <para>
        /// If <paramref name="reference"/> is stack-local, the returned span is
        /// only valid as long as <paramref name="reference"/> is valid on its
        /// stack frame.
        /// </para>
        /// </remarks>
        public static ReadOnlySpan<T> GetReadOnlySpan<T>(in T reference)
        {
            if (IsCreateSpanSupported)
                return CreateReadOnlySpanUnsafe(reference);
            else
                return GetStackLocalSpanUnsafe(ref Unsafe.AsRef(reference));
        }

        /// <summary>
        /// Creates a writable single-item span of the specified reference and
        /// pins the object it is contained in to ensure the span remains valid.
        /// </summary>
        /// <typeparam name="T">The type of the item in the returned span.</typeparam>
        /// <param name="reference">A reference to the value to span over.</param>
        /// <param name="container">The object on the heap where <paramref name="reference"/> is contained in.</param>
        /// <param name="pinnedHandle">Receives the <see cref="GCHandle"/> that was allocated for pinning <paramref name="container"/>. <see cref="GCHandle.IsAllocated"/> must be checked before calling <see cref="GCHandle.Free"/>.</param>
        /// <returns>
        /// A <see cref="Span{T}"/> value with a length of <c>1</c>.
        /// </returns>
        /// <remarks>
        /// If <see cref="IsCreateSpanSupported"/> returns <c>true</c>, the runtime
        /// supports creating spans without pinning <paramref name="container"/>. In that case,
        /// <paramref name="pinnedHandle"/> is set to its default value and its
        /// <see cref="GCHandle.IsAllocated"/> property will be <c>false</c>.
        /// <para>
        /// When done with the returned span, callers should call <see cref="GCHandle.Free"/> on the returned <paramref name="pinnedHandle"/>
        /// if and only if the <see cref="GCHandle.IsAllocated"/> property of <paramref name="pinnedHandle"/>
        /// is <c>true</c>.
        /// </para>
        /// </remarks>
        public static unsafe Span<T> GetPinnedSpan<T>(ref T reference, object container, out GCHandle pinnedHandle)
        {
            if (IsCreateSpanSupported)
            {
                pinnedHandle = default;
                return CreateSpanUnsafe(ref reference);
            }
            else
            {
                pinnedHandle = GCHandle.Alloc(container, GCHandleType.Pinned);
                return GetStackLocalSpanUnsafe(ref reference);
            }
        }

        /// <summary>
        /// Creates a read-only single-item span of the specified reference and
        /// pins the object it is contained in to ensure the span remains valid.
        /// </summary>
        /// <typeparam name="T">The type of the item in the returned span.</typeparam>
        /// <param name="reference">A reference to the value to span over.</param>
        /// <param name="container">The object on the heap where <paramref name="reference"/> is contained in.</param>
        /// <param name="pinnedHandle">Receives the <see cref="GCHandle"/> that was allocated for pinning <paramref name="container"/>. <see cref="GCHandle.IsAllocated"/> must be checked before calling <see cref="GCHandle.Free"/>.</param>
        /// <returns>
        /// A <see cref="ReadOnlySpan{T}"/> value with a length of <c>1</c>.
        /// </returns>
        /// <remarks>
        /// If <see cref="IsCreateSpanSupported"/> returns <c>true</c>, the runtime
        /// supports creating spans without pinning <paramref name="container"/>. In that case,
        /// <paramref name="pinnedHandle"/> is set to its default value and its
        /// <see cref="GCHandle.IsAllocated"/> property will be <c>false</c>.
        /// <para>
        /// When done with the returned span, callers should call <see cref="GCHandle.Free"/> on the returned <paramref name="pinnedHandle"/>
        /// if and only if the <see cref="GCHandle.IsAllocated"/> property of <paramref name="pinnedHandle"/>
        /// is <c>true</c>.
        /// </para>
        /// </remarks>
        public static unsafe ReadOnlySpan<T> GetPinnedReadOnlySpan<T>(in T reference, object container, out GCHandle pinnedHandle)
        {
            if (IsCreateSpanSupported)
            {
                pinnedHandle = default;
                return CreateReadOnlySpanUnsafe(reference);
            }
            else
            {
                pinnedHandle = GCHandle.Alloc(container, GCHandleType.Pinned);
                return GetStackLocalSpanUnsafe(ref Unsafe.AsRef(reference));
            }
        }

        private static Span<T> CreateSpanUnsafe<T>(ref T input) =>
            CreateSpanInvoker<T>.CreateSpan(ref input, length: 1);

        private static ReadOnlySpan<T> CreateReadOnlySpanUnsafe<T>(in T input) =>
            CreateSpanInvoker<T>.CreateReadOnlySpan(ref Unsafe.AsRef(input), length: 1);

        private static unsafe Span<T> GetStackLocalSpanUnsafe<T>(ref T input)
        {
            void* ptr = Unsafe.AsPointer(ref input);
            return new Span<T>(ptr, length: 1);
        }
    }
}
