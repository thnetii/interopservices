using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace THNETII.InteropServices.Memory
{
    public unsafe delegate void FixedAction(void* ptr);
    public unsafe delegate TResult FixedFunc<out TResult>(void* ptr);

    internal delegate void UseImplDelegate<T>(ref T input, FixedAction action);
    internal delegate TResult UseImplDelegate<T, TResult>(ref T input, FixedFunc<TResult> action);

    public static class Fixed
    {

        private static readonly Lazy<MethodInfo> UseImplActionGeneric = new Lazy<MethodInfo>(GetUseImplActionGeneric);
        private static readonly Lazy<MethodInfo> UseImplFuncGeneric = new Lazy<MethodInfo>(GetUseImplFuncGeneric);

        private static IEnumerable<MethodInfo> GetMethods()
        {
            IEnumerable<MethodInfo> fixedMethods = typeof(Fixed)
#if NETSTANDARD1_3 || NETSTANDARD1_6
                .GetTypeInfo()
#endif // NETSTANDARD1_3 || NETSTANDARD1_6
#if NETSTANDARD1_3
                .GetDeclaredMethods(null)
#else // !NETSTANDARD1_3
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
#endif // !NETSTANDARD1_3
                ;
            return fixedMethods;
        }

        private static MethodInfo GetUseImplActionGeneric()
        {
            IEnumerable<MethodInfo> fixedMethods = GetMethods();
            return fixedMethods
                .Where(mi => mi.Name == nameof(UseImpl))
                .Where(mi => mi.IsGenericMethodDefinition)
                .Where(mi =>
                {
                    var pis = mi.GetParameters();
                    if (pis.Length != 2)
                        return false;
                    return pis[1].ParameterType == typeof(FixedAction);
                })
                .FirstOrDefault();
        }

        private static MethodInfo GetUseImplFuncGeneric()
        {
            IEnumerable<MethodInfo> fixedMethods = GetMethods();
            return fixedMethods
                .Where(mi => mi.Name == nameof(UseImpl))
                .Where(mi => mi.IsGenericMethodDefinition)
                .Where(mi =>
                {
                    var pis = mi.GetParameters();
                    if (pis.Length != 2)
                        return false;
                    var funcType = pis[1].ParameterType
#if NETSTANDARD1_3 || NETSTANDARD1_6
                        .GetTypeInfo()
#endif // NETSTANDARD1_3 || NETSTANDARD1_6
                        ;
                    if (!funcType.IsGenericType)
                        return false;
                    var funcTypeGeneric = funcType.GetGenericTypeDefinition();
                    return funcTypeGeneric == typeof(FixedFunc<>);
                })
                .FirstOrDefault();
        }



        private static class FixedActionInvoker<T>
        {
            private static readonly Lazy<UseImplDelegate<T>> useImplLazy =
                new Lazy<UseImplDelegate<T>>(MakeUseImpl);

            private static UseImplDelegate<T> MakeUseImpl()
            {
                var generic = UseImplActionGeneric.Value;
                return (UseImplDelegate<T>)generic?
                    .MakeGenericMethod(typeof(T))
                    .CreateDelegate(typeof(UseImplDelegate<T>));
            }

            public static UseImplDelegate<T> UseImpl => useImplLazy.Value
                ?? throw new InvalidOperationException();
        }

        private static class FixedFuncInvoker<T, TResult>
        {
            private static readonly Lazy<UseImplDelegate<T, TResult>> useImplLazy =
                new Lazy<UseImplDelegate<T, TResult>>(MakeUseImpl);

            private static UseImplDelegate<T, TResult> MakeUseImpl()
            {
                var generic = UseImplFuncGeneric.Value;
                return (UseImplDelegate<T, TResult>)generic?
                    .MakeGenericMethod(typeof(T), typeof(TResult))
                    .CreateDelegate(typeof(UseImplDelegate<T, TResult>));
            }

            public static UseImplDelegate<T, TResult> UseImpl => useImplLazy.Value
                ?? throw new InvalidOperationException();
        }

        internal static unsafe void UseImpl<T>(ref T input, FixedAction action)
            where T : unmanaged
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            fixed (T* ptr = &input)
            {
                action(ptr);
            }
        }

        internal static unsafe TResult UseImpl<T, TResult>(ref T input, FixedFunc<TResult> func)
            where T : unmanaged
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            fixed (T* ptr = &input)
            {
                return func(ptr);
            }
        }

        public static unsafe void Use<T>(ref T input, FixedAction action)
            where T : struct
        {
            FixedActionInvoker<T>.UseImpl(ref input, action);
        }

        public static unsafe TResult Use<T, TResult>(ref T input, FixedFunc<TResult> func)
            where T : struct
        {
            return FixedFuncInvoker<T, TResult>.UseImpl(ref input, func);
        }
    }
}
