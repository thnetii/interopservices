using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.StructMarshal
{
    internal static class CustomAttributeExtensions
    {
        private static readonly ConstructorInfo marshalAsAttributeCtorInfo = GetMarshalInfoConstructorInfo();
        private static readonly ConstructorInfo fieldOffsetAttributeCtorInfo = GetFieldOffsetConstructorInfo();

        private static ConstructorInfo GetMarshalInfoConstructorInfo()
        {
            var paramTypes = new Type[] { typeof(UnmanagedType) };
            return CustomAttributeMemeberInfos<MarshalAsAttribute>.TypeInfo.GetConstructor(paramTypes);
        }

        private static ConstructorInfo GetFieldOffsetConstructorInfo()
        {
            var paramTypes = new Type[] { typeof(int) };
            return CustomAttributeMemeberInfos<FieldOffsetAttribute>.TypeInfo.GetConstructor(paramTypes);
        }

        private static CustomAttributeBuilder ToCustomAttributeBuilder<T>(this T attribute, ConstructorInfo constructorInfo, params object[] ctorParams)
        {
            if (attribute == null)
                return null;

            var propertyValues = CustomAttributeMemeberInfos<T>.PropertyInfos
                .Select(pi => pi.GetValue(attribute))
                .ToArray();
            var fieldValues = CustomAttributeMemeberInfos<T>.FieldInfos
                .Select(fi => fi.GetValue(attribute))
                .ToArray();

            return new CustomAttributeBuilder(constructorInfo, ctorParams,
                CustomAttributeMemeberInfos<T>.PropertyInfos, propertyValues,
                CustomAttributeMemeberInfos<T>.FieldInfos, fieldValues
                );
        }

        public static CustomAttributeBuilder ToCustomAttributeBuilder(this MarshalAsAttribute attribute)
            => ToCustomAttributeBuilder(attribute, marshalAsAttributeCtorInfo, attribute?.Value ?? default(UnmanagedType));

        public static CustomAttributeBuilder ToCustomAttributeBuilder(this FieldOffsetAttribute attribute)
            => ToCustomAttributeBuilder(attribute, fieldOffsetAttributeCtorInfo, attribute?.Value ?? default(int));

        private class CustomAttributeMemeberInfos<T>
        {
            public static readonly TypeInfo TypeInfo = typeof(T).GetTypeInfo();
            public static PropertyInfo[] PropertyInfos = GetPropertyInfos();
            public static FieldInfo[] FieldInfos = GetFieldInfos();

            private static PropertyInfo[] GetPropertyInfos()
            {
                return TypeInfo.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(pi => pi.CanWrite && pi.CanRead)
                    .ToArray();
            }

            private static FieldInfo[] GetFieldInfos()
            {
                return TypeInfo.GetFields(BindingFlags.Public | BindingFlags.Instance);
            }
        }
    }
}
