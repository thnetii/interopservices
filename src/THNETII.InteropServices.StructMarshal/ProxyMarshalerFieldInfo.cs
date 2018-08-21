using System;
using System.Reflection;
using System.Reflection.Emit;

namespace THNETII.InteropServices.StructMarshal
{
    public class ProxyMarshalerFieldInfo
    {
        public MemberInfo OriginMemberInfo { get; internal set; }
        public string FieldName => OriginMemberInfo?.Name;
        public Type ProxyFieldType { get; internal set; }
        public int? MemberOrder { get; internal set; }
        public CustomAttributeBuilder MarshalAsBuilder { get; internal set; }
        public CustomAttributeBuilder FieldOffsetBuilder { get; internal set; }
        public FieldInfo ProxyFieldInfo { get; internal set; }

        internal Action<object, object> MarshalOriginToProxy { get; set; }
        internal Action<object, object> MarshalProxyToOrigin { get; set; }

        internal void SetMarshalToMethodsDefault()
        {
            switch (OriginMemberInfo)
            {
                case FieldInfo fi:
                    MarshalOriginToProxy = (origin, proxy) => ProxyFieldInfo.SetValue(proxy, fi.GetValue(origin));
                    MarshalProxyToOrigin = (proxy, origin) => fi.SetValue(origin, ProxyFieldInfo.GetValue(proxy));
                    break;
                case PropertyInfo pi:
                    Action<object, object> CreateTryCatchWrapper(Action<object, object> action) => (a, b) =>
                    {
                        try { action(a, b); }
                        catch (Exception except) when (except is ArgumentException || except is MethodAccessException)
                        { throw new InvalidOperationException(except.Message, except); }
                    };
                    Action<object, object> simpleOriginPropToProxy =
                        (origin, proxy) => ProxyFieldInfo.SetValue(proxy, pi.GetValue(origin));
                    Action<object, object> simpleProxyToOriginProp =
                        (origin, proxy) => pi.SetValue(origin, ProxyFieldInfo.GetValue(proxy));
                    MarshalOriginToProxy = CreateTryCatchWrapper(simpleOriginPropToProxy);
                    MarshalProxyToOrigin = CreateTryCatchWrapper(simpleProxyToOriginProp);
                    break;
            }
        }
    }
}
