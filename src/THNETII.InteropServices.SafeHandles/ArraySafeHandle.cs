using System;

namespace THNETII.InteropServices.SafeHandles
{
    public abstract class ArraySafeHandle<T> : SafeHandle<T[]>
    {
        protected ArraySafeHandle() : this(IntPtr.Zero) { }
        protected ArraySafeHandle(IntPtr invalidHandleValue, bool ownsHandle = false) : base(invalidHandleValue, ownsHandle) { }

        /// <summary>
        /// Marshals the contents of the native memory segment referred to by this instance into a managed array instance.
        /// <para><note>The <see cref="ArraySafeHandle{T}"/> and its derived types do not support direct marshaling to a managed instance. Use the <see cref="MarshalToManagedArray"/> method instead.</note></para>
        /// </summary>
        /// <returns>This overridden implementation of <see cref="MarshalToManagedInstance"/> always throws a <see cref="NotSupportedException"/> instance.</returns>
        /// <exception cref="NotSupportedException">An array handle cannot be marshaled directly to a managed instance.</exception>
        public override T[] MarshalToManagedInstance() => throw new NotSupportedException($"An array handle cannot be marshaled directly to a managed instance. Use the {nameof(MarshalToManagedArray)} method instead.");

        public abstract T[] MarshalToManagedArray(int length);
    }
}