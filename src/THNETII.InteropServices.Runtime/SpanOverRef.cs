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
            ParameterInfo[] @params;
            ParameterInfo refParam, lengthParam;
            Type intTypeRef = typeof(int);
            foreach (MethodInfo mi in memoryMarshalMethods)
            {
                if (CreateSpanGenericDefinition != null &&
                    CreateReadOnlySpanGenericDefinition != null)
                    break;
                switch (mi.Name)
                {
                    case CreateSpan:
                        if (!mi.IsGenericMethod)
                            continue;
                        @params = mi.GetParameters();
                        if (@params.Length != 2)
                            continue;
                        refParam = @params[0];
                        if (!refParam.ParameterType.IsByRef)
                            continue;
                        lengthParam = @params[1];
                        if (lengthParam.ParameterType != intTypeRef)
                            continue;
                        CreateSpanGenericDefinition = mi;
                        break;
                    case CreateReadOnlySpan:
                        if (!mi.IsGenericMethod)
                            continue;
                        @params = mi.GetParameters();
                        if (@params.Length != 2)
                            continue;
                        refParam = @params[0];
                        if (!refParam.ParameterType.IsByRef)
                            continue;
                        lengthParam = @params[1];
                        if (lengthParam.ParameterType != intTypeRef)
                            continue;
                        CreateReadOnlySpanGenericDefinition = mi;
                        break;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether creating spans is supported byt the runtime.
        /// </summary>
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
        /// Creates a writable single-item span of the specified reference; or, if creating
        /// spans over reference is not supported by the runtime, copies the specified value to heap memory.
        /// </summary>
        /// <typeparam name="T">The type of the item in the returned span.</typeparam>
        /// <param name="reference">A reference to the value to span over. If unsuccessful, the value will be copied to the heap instead.</param>
        /// <param name="isCopy">On return receives value that indicates whether the returned span is a heap-copy or provides direct access to <paramref name="reference"/>.</param>
        /// <returns>
        /// A <see cref="Span{T}"/> value with a length of <c>1</c>.
        /// <para>
        /// If <paramref name="isCopy"/> is <c>true</c>, the span wraps over a
        /// copy of <paramref name="reference"/> which resides on the heap.
        /// </para>
        /// <para>
        /// If <paramref name="isCopy"/> is <c>false</c>, the span wraps directly
        /// over the specified <paramref name="reference"/> reference.
        /// </para>
        /// </returns>
        public static Span<T> CopyOrSpan<T>(ref T reference, out bool isCopy)
        {
            if (IsCreateSpanSupported)
            {
                isCopy = false;
                return CreateSpanInvoker<T>.CreateSpan(ref reference, length: 1);
            }
            else
            {
                isCopy = true;
                return new[] { reference };
            }
        }

        /// <summary>
        /// Creates a read-only single-item span of the specified value; or, if creating
        /// spans over references is not supported by the runtime, copies the specified value to heap memory.
        /// </summary>
        /// <typeparam name="T">The type of the item in the returned span.</typeparam>
        /// <param name="input">A read-only reference to the value to span over. If unsuccessful, the value will be copied to the heap instead.</param>
        /// <param name="isCopy">On return receives value that indicates whether the returned span is a heap-copy or provides direct access to <paramref name="input"/>.</param>
        /// <returns>
        /// A <see cref="ReadOnlySpan{T}"/> value with a length of <c>1</c>.
        /// <para>
        /// If <paramref name="isCopy"/> is <c>true</c>, the span wraps over a
        /// copy of <paramref name="input"/> which resides on the heap.
        /// </para>
        /// <para>
        /// If <paramref name="isCopy"/> is <c>false</c>, the span wraps directly
        /// over the specified <paramref name="input"/> value.
        /// </para>
        /// </returns>
        public static ReadOnlySpan<T> CopyOrSpanReadOnly<T>(in T input, out bool isCopy) =>
            CopyOrSpanReadOnly(input, out isCopy, out var _);

        /// <summary>
        /// Creates a read-only single-item span of the specified value; or, if creating
        /// spans over references is not supported by the runtime, copies the specified value to heap memory.
        /// </summary>
        /// <typeparam name="T">The type of the item in the returned span.</typeparam>
        /// <param name="input">A read-only reference to the value to span over. If unsuccessful, the value will be copied to the heap instead.</param>
        /// <param name="isCopy">On return receives value that indicates whether the returned span is a heap-copy or provides direct access to <paramref name="input"/>.</param>
        /// <param name="copySpan">On return receives a writable to span to the heap-copy of <paramref name="input"/> if <paramref name="isCopy"/> is <c>true</c>; otherwise, receives an empty span.</param>
        /// <returns>
        /// A <see cref="ReadOnlySpan{T}"/> value with a length of <c>1</c>.
        /// <para>
        /// If <paramref name="isCopy"/> is <c>true</c>, the span wraps over a
        /// copy of <paramref name="input"/> which resides on the heap. <paramref name="copySpan"/>
        /// provides write acess to copied value.
        /// </para>
        /// <para>
        /// If <paramref name="isCopy"/> is <c>false</c>, the span wraps directly
        /// over the specified <paramref name="input"/> value. <paramref name="copySpan"/>
        /// will be set to an empy span.
        /// </para>
        /// </returns>
        public static ReadOnlySpan<T> CopyOrSpanReadOnly<T>(in T input, out bool isCopy, out Span<T> copySpan)
        {
            if (IsCreateSpanSupported)
            {
                isCopy = false;
                copySpan = Span<T>.Empty;
                return CreateSpanInvoker<T>.CreateReadOnlySpan(
                    ref Unsafe.AsRef(input), length: 1);
            }
            else
            {
                isCopy = true;
                copySpan = new[] { input };
                return copySpan;
            }
        }
    }
}
