using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.SafeHandles
{
    /// <summary>
    /// Represents a safe handle for a native memory segment that contains a continuous array of structure values.
    /// <para>
    /// The <see cref="Int32CastValueArraySafeHandle{T}"/> class maked use of optimized marshaling, by reading the entire memory segment as a sequence of <see cref="int"/> values.
    /// It must therefore be possible to cast <see cref="int"/> values to values of type <typeparamref name="T"/> (if e.g. <typeparamref name="T"/> is an enumeration type.)
    /// </para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Int32CastValueArraySafeHandle<T> : Int32ArraySafeHandle where T : struct
    {
        /// <summary>
        /// Initializes a new safe handle with a zero-pointer.
        /// </summary>
        protected Int32CastValueArraySafeHandle() : this(IntPtr.Zero) { }
        /// <summary>
        /// Initializes a new safe handle with the specified invalid handle value.
        /// </summary>
        /// <param name="invalidHandleValue">The value of an invalid handle (usually <see cref="IntPtr.Zero"/>).</param>
        /// <param name="ownsHandle"><c>true</c> to reliably let the instance release the handle during the finalization phase; otherwise, <c>false</c> (not recommended).</param>
        protected Int32CastValueArraySafeHandle(IntPtr invalidHandleValue, bool ownsHandle = false) : base(invalidHandleValue, ownsHandle) { }

        /// <summary>
        /// Marshals the contents of the native memory segment referred to by this instance into a managed array with instances of <typeparamref name="T"/>.
        /// <para>
        /// <note type="important">
        /// The <see cref="Int32CastValueArraySafeHandle{T}"/> class does not maintain information about the allocation size of the native memory that is controlled by the handle.
        /// Ensure that the value of the <paramref name="length"/> parameter is valid.
        /// </note>
        /// </para>
        /// </summary>
        /// <param name="length">The number of items contained in the native array that is controlled by this instance.</param>
        /// <returns>A managed array containing instances of type <typeparamref name="T"/>. The returned array will contain exactly <paramref name="length"/> items.</returns>
        /// <exception cref="ObjectDisposedException">The handle has been closed and the controlled native memory has been released back to the system.</exception>
        /// <exception cref="InvalidOperationException">The handle controlling the native memory has an invalid value.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is less than <c>0</c> (zero).</exception>
        /// <exception cref="InvalidCastException">An element in the memory segment cannot be cast from <see cref="int"/> to type <typeparamref name="T"/>.</exception>
        public new T[] MarshalToManagedArray(int length) => base.MarshalToManagedArray(length).Cast<T>().ToArray();
    }
}
