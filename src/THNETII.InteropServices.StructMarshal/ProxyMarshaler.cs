using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.StructMarshal
{
    public static class ProxyMarshaler
    {
        public static readonly string ProxyRootNamespace = $"{typeof(ProxyMarshaler).Namespace}.Proxy";
        internal static readonly TypeInfo MarshalTypeRef = typeof(Marshal).GetTypeInfo();
        internal static readonly MethodInfo MarshalSizeOfGeneric =
            MarshalTypeRef.GetMethod(
                nameof(Marshal.SizeOf),
                BindingFlags.Public | BindingFlags.Static,
                Type.DefaultBinder,
                Type.EmptyTypes,
                null
                );

        public static MarshalLayoutAttribute GetMarshalLayout(TypeInfo typeInfo)
        {
            var mla = typeInfo.GetCustomAttribute<MarshalLayoutAttribute>();
            var sla = typeInfo.StructLayoutAttribute;
            if (sla != null)
            {
                if (mla == null)
                    mla = (MarshalLayoutAttribute)sla;
                else // if (mla != null)
                {
                    var pmla = mla;
                    if (pmla.LayoutKind == LayoutKind.Auto)
                        mla = new MarshalLayoutAttribute(sla.Value);
                    if (pmla.CharSet == default(CharSet))
                        mla.CharSet = sla.CharSet;
                    if (pmla.Pack == default(int))
                        mla.Pack = sla.Pack;
                    if (pmla.Size == default(int))
                        mla.Size = sla.Size;
                }
            }
            return mla;
        }

        public static TypeInfo CreateMarshalProxyType(TypeInfo originTypeInfo, out IReadOnlyList<ProxyMarshalerFieldInfo> proxyMarshalerFieldInfos)
        {
            var originMarshalLayout = GetMarshalLayout(originTypeInfo);
            var originMembers = originTypeInfo
                .GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(mi => mi.GetCustomAttribute<MarshalSkipMemberAttribute>() == null)
                .Where(mi => (mi.MemberType & (MemberTypes.Field | MemberTypes.Property)) != 0);
            proxyMarshalerFieldInfos = GetProxyMarshalerFieldInfos(originMembers);

            var proxyAssemblyGuid = originTypeInfo.GUID;
            var proxyAssemblyName = $"{ProxyRootNamespace}.Guid_{proxyAssemblyGuid:N}";
            var proxyTypeName = $"{proxyAssemblyName}.{originTypeInfo.Name}";
            var proxyAssemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(proxyAssemblyName), AssemblyBuilderAccess.Run);
            var proxyModuleBuilder = proxyAssemblyBuilder.DefineDynamicModule(proxyAssemblyName);
            var proxyTypeAttrs = TypeAttributes.Public | TypeAttributes.Class;
            TypeBuilder proxyTypeBuilder;
            if (originMarshalLayout != null)
            {
                switch (originMarshalLayout.LayoutKind)
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
                switch (originMarshalLayout.CharSet)
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
                proxyTypeBuilder = proxyModuleBuilder.DefineType(proxyTypeName, proxyTypeAttrs, null, (PackingSize)originMarshalLayout.Pack, originMarshalLayout.Size);
            }
            else
                proxyTypeBuilder = proxyModuleBuilder.DefineType(proxyTypeName, proxyTypeAttrs, null);

            foreach (var pmfi in proxyMarshalerFieldInfos)
            {
                var proxyFieldBuilder = proxyTypeBuilder.DefineField(pmfi.FieldName, pmfi.ProxyFieldType, FieldAttributes.Public);
                if (pmfi.MarshalAsBuilder != null)
                    proxyFieldBuilder.SetCustomAttribute(pmfi.MarshalAsBuilder);
                if (pmfi.FieldOffsetBuilder != null)
                    proxyFieldBuilder.SetCustomAttribute(pmfi.FieldOffsetBuilder);
            }

            var proxyTypeInfo = proxyTypeBuilder.CreateTypeInfo();
            foreach (var pmfi in proxyMarshalerFieldInfos)
                pmfi.ProxyFieldInfo = proxyTypeInfo.GetField(pmfi.FieldName, BindingFlags.Public | BindingFlags.Instance);
            return proxyTypeInfo;
        }

        private static IReadOnlyList<ProxyMarshalerFieldInfo> GetProxyMarshalerFieldInfos(IEnumerable<MemberInfo> memberInfos)
        {
            var pmfis_mixed = memberInfos.Select(mi =>
            {
                var pmfi = new ProxyMarshalerFieldInfo
                {
                    OriginMemberInfo = mi,
                    MemberOrder = mi.GetCustomAttribute<MarshalFieldOrderAttribute>()?.Value,
                    MarshalAsBuilder = mi.GetCustomAttribute<MarshalAsAttribute>().ToCustomAttributeBuilder(),
                    FieldOffsetBuilder = mi.GetCustomAttribute<FieldOffsetAttribute>().ToCustomAttributeBuilder()
                };
                switch (mi)
                {
                    case PropertyInfo pi:
                        pmfi.ProxyFieldType = pi.PropertyType;
                        break;
                    case FieldInfo fi:
                        pmfi.ProxyFieldType = fi.FieldType;
                        break;
                }
                pmfi.SetMarshalToMethodsDefault();
                return pmfi;
            });
            var pmfis_ordered = new List<ProxyMarshalerFieldInfo>();
            var pmfis_sequential = new List<ProxyMarshalerFieldInfo>();
            int pmfis_count = 0;
            foreach (var pmfi in pmfis_mixed)
            {
                if (pmfi.MemberOrder.HasValue)
                    pmfis_ordered.Add(pmfi);
                else
                    pmfis_sequential.Add(pmfi);
                pmfis_count++;
            }
            pmfis_ordered = pmfis_ordered.OrderBy(pmfi => pmfi.MemberOrder).ToList();
            var pmfis_final = new List<ProxyMarshalerFieldInfo>(pmfis_count);
            using (var pmfis_ordered_enumerator = pmfis_ordered.GetEnumerator())
            {
                var pmfis_ordered_moved = pmfis_ordered_enumerator.MoveNext();
                using (var pmfis_sequential_enumerator = pmfis_sequential.GetEnumerator())
                {
                    var pmfis_sequential_moved = pmfis_sequential_enumerator.MoveNext();

                    for (int i = 0; (pmfis_ordered_moved || pmfis_sequential_moved); i++)
                    {
                        ProxyMarshalerFieldInfo pmfi_to_add = null;
                        if (pmfis_ordered_moved)
                        {
                            pmfi_to_add = pmfis_ordered_enumerator.Current;
                            if (pmfi_to_add.MemberOrder.Value == i)
                            {
                                pmfis_final.Add(pmfi_to_add);
                                pmfis_ordered_moved = pmfis_ordered_enumerator.MoveNext();
                                continue;
                            }
                        }
                        if (pmfis_sequential_moved)
                        {
                            pmfi_to_add = pmfis_sequential_enumerator.Current;
                            pmfis_final.Add(pmfi_to_add);
                            pmfis_sequential_moved = pmfis_sequential_enumerator.MoveNext();
                            continue;
                        }
                        else if (pmfis_ordered_moved) // pmfis_sequential exhausted, but still ordered items
                        {
                            // Skip i forward to expected ordered pmfi
                            // Note: i is incremented once at iteration end
                            i = pmfis_ordered_enumerator.Current.MemberOrder.Value - 1;
                        }
                    }
                }
            }
            
            return pmfis_final;
        }
    }

    public class ProxyMarshaler<T> : ICustomMarshaler where T : class, new()
    {
        private static readonly IReadOnlyList<ProxyMarshalerFieldInfo> proxyMarshalerFieldInfos;

        public static int ProxySizeOf { get; }
        public static TypeInfo ProxyTypeInfo { get; }

        static ProxyMarshaler()
        {
            var originTypeInfo = typeof(T).GetTypeInfo();

            ProxyTypeInfo = ProxyMarshaler.CreateMarshalProxyType(originTypeInfo, out proxyMarshalerFieldInfos);
            var marshalSizeOfGeneric = ProxyMarshaler.MarshalSizeOfGeneric;
            var marhshalSizeOfProxy = marshalSizeOfGeneric.MakeGenericMethod(ProxyTypeInfo);
            ProxySizeOf = (int)marhshalSizeOfProxy.Invoke(obj: null, parameters: null);
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
            var ProxyObj = Activator.CreateInstance(ProxyTypeInfo);
            foreach (var pmfi in proxyMarshalerFieldInfos)
                pmfi.MarshalOriginToProxy(ManagedObj, ProxyObj);
            var ProxyPtr = Marshal.AllocCoTaskMem(ProxySizeOf);
            Marshal.StructureToPtr(ProxyObj, ProxyPtr, false);
            return ProxyPtr;
        }

        public object MarshalNativeToManaged(IntPtr pNativeData)
        {
            if (pNativeData == IntPtr.Zero)
                return null;
            var proxyInstance = Marshal.PtrToStructure(pNativeData, (Type)ProxyTypeInfo);
            var targetInstance = Activator.CreateInstance<T>();
            foreach (var pmfi in proxyMarshalerFieldInfos)
                pmfi.MarshalProxyToOrigin(proxyInstance, targetInstance);
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
        public static ProxyMarshaler<T> GetInstance(string cookie = null)
            => new ProxyMarshaler<T>();
    }
}
