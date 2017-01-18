using System;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.SafeHandles
{
    /// <summary>
    /// Represents a safe handle for a native memory segment that contains a continuous array of structure values.
    /// </summary>
    /// <typeparam name="T">The managed type that is stored in the items of the array in native memory.</typeparam>
    public class ValueArraySafeHandle<T> : ArraySafeHandle<T>
    {
        /// <summary>
        /// The size, in bytes, occupied by one instance of <typeparamref name="T"/> in native memory.
        /// </summary>
        protected static readonly int ValueItemSizeOf = Marshal.SizeOf<T>();

        /// <summary>
        /// Marshals the contents of the native memory segment referred to by this instance into a managed array with instances of <typeparamref name="T"/>.
        /// <para>
        /// <note type="important">
        /// The <see cref="ValueArraySafeHandle{T}"/> class does not maintain information about the allocation size of the native memory that is controlled by the handle.
        /// Ensure that the value of the <paramref name="length"/> parameter is valid.
        /// </note>
        /// </para>
        /// </summary>
        /// <param name="length">The number of items contained in the native array that is controlled by this instance.</param>
        /// <returns>A managed array containing instances of type <typeparamref name="T"/>. The returned array will contain exactly <paramref name="length"/> items.</returns>
        /// <exception cref="ObjectDisposedException">The handle has been closed and the controlled native memory has been released back to the system.</exception>
        /// <exception cref="InvalidOperationException">The handle controlling the native memory has an invalid value.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is less than <c>0</c> (zero).</exception>
        public override T[] MarshalToManagedArray(int length)
        {
            if (IsClosed)
                throw new ObjectDisposedException(nameof(handle));
            else if (IsInvalid)
                throw new InvalidOperationException("The native memory handle has an invalid value.");
            else if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length), length, "The array length must be a non-negative value");
            var array = new T[length];
            var pItem = handle;
            for (int i = 0; i < length; i++, pItem += ValueItemSizeOf)
                array[i] = Marshal.PtrToStructure<T>(pItem);
            return array;
        }

        /// <summary>
        /// Initializes a new Value array safe handle with a zero-pointer.
        /// </summary>
        protected ValueArraySafeHandle() : this(IntPtr.Zero) { }
        /// <summary>
        /// Initializes a new safe handle with the specified invalid handle value.
        /// </summary>
        /// <param name="invalidHandleValue">The value of an invalid handle (usually <see cref="IntPtr.Zero"/>).</param>
        /// <param name="ownsHandle"><c>true</c> to reliably let the instance release the handle during the finalization phase; otherwise, <c>false</c> (not recommended).</param>
        protected ValueArraySafeHandle(IntPtr invalidHandleValue, bool ownsHandle = false) : base(invalidHandleValue, ownsHandle) { }
    }
}
