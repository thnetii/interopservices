using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.StructMarshal
{
    static class ProxyMarshaler
    {
        public static readonly string ProxyRootNamespace = $"{typeof(ProxyMarshaler).Namespace}.Proxy";
        public static readonly TypeInfo MarshalTypeRef = typeof(Marshal).GetTypeInfo();
        public static readonly MethodInfo MarshalSizeOfGeneric = 
            MarshalTypeRef.GetMethod(
                nameof(Marshal.SizeOf),
                BindingFlags.Public | BindingFlags.Static,
                Type.DefaultBinder,
                Type.EmptyTypes,
                null
                );
    }

    public class ProxyMarshaler<T> : ICustomMarshaler where T : class, new()
    {
        private static readonly ProxyMarshaler<T> instance;
        public static int ProxySizeOf { get; }
        public static TypeInfo ProxyTypeInfo { get; }

        static ProxyMarshaler()
        {
            var originTypeInfo = typeof(T).GetTypeInfo();
            var originStructLayout = originTypeInfo.StructLayoutAttribute;
            var originMembers = originTypeInfo
                .GetMembers(BindingFlags.Instance)
                .Where(mi => (mi.MemberType & (MemberTypes.Field | MemberTypes.Property)) != 0)
                .ToArray();
            for (int i = 0; i < originMembers.Length; i++)
            {
                var originMemberInfo = originMembers[i];
                switch (originMemberInfo)
                {
                    case FieldInfo originFieldInfo:
                        break;
                    case PropertyInfo originPropertyInfo:
                        break;
                }
            }

            var proxyAssemblyGuid = originTypeInfo.GUID;
            var proxyAssemblyName = $"{ProxyMarshaler.ProxyRootNamespace}.Guid_{proxyAssemblyGuid:N}";
            var proxyTypeName = $"{proxyAssemblyName}.{originTypeInfo.Name}";
            var proxyAssemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(proxyAssemblyName), AssemblyBuilderAccess.Run);
            var proxyModuleBuilder = proxyAssemblyBuilder.DefineDynamicModule(proxyAssemblyName);
            var proxyTypeAttrs = TypeAttributes.Public | TypeAttributes.Class;
            TypeBuilder proxyTypeBuilder;
            if (originStructLayout != null)
            {
                switch (originStructLayout.Value)
                {
                    case LayoutKind.Auto:
                        proxyTypeAttrs |= TypeAttributes.AutoLayout;
                        break;
                    case LayoutKind.Explicit:
                        proxyTypeAttrs |= TypeAttributes.ExplicitLayout;
                        break;
                    case LayoutKind.Sequential:
                        proxyTypeAttrs |= TypeAttributes.SequentialLayout;
                        break;
                }
                switch (originStructLayout.CharSet)
                {
                    case CharSet.Ansi:
                        proxyTypeAttrs |= TypeAttributes.AnsiClass;
                        break;
                    case CharSet.Auto:
                        proxyTypeAttrs |= TypeAttributes.AutoClass;
                        break;
                    case CharSet.Unicode:
                        proxyTypeAttrs |= TypeAttributes.UnicodeClass;
                        break;
                }
                proxyTypeBuilder = proxyModuleBuilder.DefineType(proxyTypeName, proxyTypeAttrs, null, (PackingSize)originStructLayout.Pack, originStructLayout.Size);
            }
            else
                proxyTypeBuilder = proxyModuleBuilder.DefineType(proxyTypeName, proxyTypeAttrs, null);
            var proxyFieldBuilder = proxyTypeBuilder.DefineField(null, null, FieldAttributes.Public | FieldAttributes.HasFieldMarshal);
            

            ProxyTypeInfo = proxyTypeBuilder.CreateTypeInfo();
            var marshalSizeOfGeneric = ProxyMarshaler.MarshalSizeOfGeneric;
            var marhshalSizeOfProxy = marshalSizeOfGeneric.MakeGenericMethod(ProxyTypeInfo);
            ProxySizeOf = (int)marhshalSizeOfProxy.Invoke(obj: null, parameters: null);
            instance = new ProxyMarshaler<T>();
        }

        public void CleanUpNativeData(IntPtr pNativeData)
        {
            if (pNativeData == IntPtr.Zero)
                return; // No need to clean up a null-pointer
            Marshal.FreeCoTaskMem(pNativeData);
        }

        public IntPtr MarshalManagedToNative(object ManagedObj)
        {
            if (ReferenceEquals(ManagedObj, null))
                return IntPtr.Zero;
            throw new NotImplementedException();
        }

        public object MarshalNativeToManaged(IntPtr pNativeData)
        {
            if (pNativeData == IntPtr.Zero)
                return null;
            var targetInstance = Activator.CreateInstance<T>();

            return targetInstance;
        }

        /// <inheritdoc />
        [DebuggerStepThrough]
        void ICustomMarshaler.CleanUpManagedData(object ManagedObj) { }

        /// <inheritdoc />
        [DebuggerStepThrough]
        int ICustomMarshaler.GetNativeDataSize() => -1;

        /// <summary>
        /// Returns a singleton instance of the <see cref="ProxyMarshaler{T}"/>
        /// capable of marshaling <typeparamref name="T" />.
        /// </summary>
        /// <param name="cookie">Optional marshal cookie. Discarded.</param>
        /// <returns>A new instance of the <see cref="ProxyMarshaler{T}"/> class.</returns>
        /// <remarks>
        /// The <paramref name="cookie"/> parameter is discarded by the
        /// <see cref="ProxyMarshaler{T}"/> class, as the Porxy Marshaler
        /// uses Attribute reflection instead of marshal cookies.
        /// </remarks>
        [DebuggerStepThrough]
        public static ProxyMarshaler<T> GetInstance(string cookie = null) => instance;
    }
}
