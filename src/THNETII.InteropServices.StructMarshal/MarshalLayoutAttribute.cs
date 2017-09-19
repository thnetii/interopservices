using System;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.StructMarshal
{
    /// <summary>
    /// Lets you control the physical layout of the data fields of a class or structure
    /// that are used when the <see cref="ProxyMarshaler{T}"/> marshals instances to
    /// and from native memory.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = true, AllowMultiple = false)]
    public sealed class MarshalLayoutAttribute : Attribute
    {
        /// <summary>
        /// Initalizes a new instance of the <see cref="MarshalLayoutAttribute"/>
        /// class with the specified <see cref="System.Runtime.InteropServices.LayoutKind"/> enumeration
        /// member.
        /// </summary>
        /// <param name="layoutKind">
        /// One of the enumeration values that specifes how the class or structure should
        /// be arranged.
        /// </param>
        public MarshalLayoutAttribute(LayoutKind layoutKind)
        {
            LayoutKind = layoutKind;
        }

        /// <summary>
        /// Gets the <see cref="System.Runtime.InteropServices.LayoutKind"/> value that specifies how the
        /// class or structure is arranged.
        /// </summary>
        /// <seealso cref="StructLayoutAttribute.Value"/>
        public LayoutKind LayoutKind { get; }

        /// <summary>
        /// Indicates how string data fields within the class or structure should be marshaled by deafult.
        /// </summary>
        /// <seealso cref="StructLayoutAttribute.CharSet"/>
        public CharSet CharSet { get; set; }

        /// <summary>
        /// Controls the alignment of data fields of a class or structure in memory.
        /// </summary>
        /// <seealso cref="StructLayoutAttribute.Pack"/>
        public int Pack { get; set; }

        /// <summary>
        /// Indicates the absolute size of the class or structure.
        /// </summary>
        /// <seealso cref="StructLayoutAttribute.Size"/>
        public int Size { get; set; }

        /// <summary>
        /// Explicit conversion from <see cref="MarshalLayoutAttribute"/> to <see cref="StructLayoutAttribute"/>.
        /// </summary>
        /// <param name="marshalLayoutAttribute">The <see cref="MarshalLayoutAttribute"/> instance to convert.</param>
        /// <returns>
        /// A new <see cref="StructLayoutAttribute"/> instance that is semantically equivalent to
        /// <paramref name="marshalLayoutAttribute"/>, or <c>null</c> if <paramref name="marshalLayoutAttribute"/>
        /// is <c>null</c>.
        /// </returns>
        public static explicit operator StructLayoutAttribute(MarshalLayoutAttribute marshalLayoutAttribute)
        {
            if (marshalLayoutAttribute == null)
                return null;
            return new StructLayoutAttribute(marshalLayoutAttribute.LayoutKind)
            {
                CharSet = marshalLayoutAttribute.CharSet,
                Pack = marshalLayoutAttribute.Pack,
                Size = marshalLayoutAttribute.Size
            };
        }

        /// <summary>
        /// Explicit conversion from <see cref="StructLayoutAttribute"/> to <see cref="MarshalLayoutAttribute"/>.
        /// </summary>
        /// <param name="structLayoutAttribute">The <see cref="StructLayoutAttribute"/> instance to convert.</param>
        /// <returns>
        /// A new <see cref="MarshalLayoutAttribute"/> instance that is semantically equivalent to
        /// <paramref name="structLayoutAttribute"/>, or <c>null</c> if <paramref name="structLayoutAttribute"/>
        /// is <c>null</c>.
        /// </returns>
        public static explicit operator MarshalLayoutAttribute(StructLayoutAttribute structLayoutAttribute)
        {
            if (structLayoutAttribute == null)
                return null;
            return new MarshalLayoutAttribute(structLayoutAttribute.Value)
            {
                CharSet = structLayoutAttribute.CharSet,
                Pack = structLayoutAttribute.Pack,
                Size = structLayoutAttribute.Size
            };
        }
    }
}
