using System;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.SafeHandles
{
    /// <summary>
    /// Represents a safe handle for a native memory segment that contains a continuous array of integer values.
    /// <para>
    /// The <see cref="IntPtrArraySafeHandle"/> class maked use of optimized marshaling, by reading the entire memory segment as a sequence of <see cref="IntPtr"/> values.
    /// </para>
    /// </summary>
    public class IntPtrArraySafeHandle : ValueArraySafeHandle<IntPtr>
    {
        /// <summary>
        /// Initializes a new safe handle with a zero-pointer.
        /// </summary>
        protected IntPtrArraySafeHandle() : this(IntPtr.Zero) { }
        /// <summary>
        /// Initializes a new safe handle with the specified invalid handle value.
        /// </summary>
        /// <param name="invalidHandleValue">The value of an invalid handle (usually <see cref="IntPtr.Zero"/>).</param>
        /// <param name="ownsHandle"><c>true</c> to reliably let the instance release the handle during the finalization phase; otherwise, <c>false</c> (not recommended).</param>
        protected IntPtrArraySafeHandle(IntPtr invalidHandleValue, bool ownsHandle = false) : base(invalidHandleValue, ownsHandle) { }

        /// <summary>
        /// Marshals the contents of the native memory segment referred to by this instance into a managed array of <see cref="IntPtr"/> values.
        /// <para>
        /// <note type="important">
        /// The <see cref="IntPtrArraySafeHandle"/> class does not maintain information about the allocation size of the native memory that is controlled by the handle.
        /// Ensure that the value of the <paramref name="length"/> parameter is valid.
        /// </note>
        /// </para>
        /// </summary>
        /// <param name="length">The number of items contained in the native array that is controlled by this instance.</param>
        /// <returns>A managed array containing <paramref name="length"/> <see cref="IntPtr"/> values.</returns>
        /// <exception cref="ObjectDisposedException">The handle has been closed and the controlled native memory has been released back to the system.</exception>
        /// <exception cref="InvalidOperationException">The handle controlling the native memory has an invalid value.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is less than <c>0</c> (zero).</exception>
        public override IntPtr[] MarshalToManagedArray(int length)
        {
            if (IsClosed)
                throw new ObjectDisposedException(nameof(handle));
            else if (IsInvalid)
                throw new InvalidOperationException("The native memory handle has an invalid value.");
            else if (length < 1)
                throw new ArgumentOutOfRangeException(nameof(length), length, "Value must be a positive integer.");
            else if (length == 0)
                return new IntPtr[0];
            var iArray = new IntPtr[length];
            Marshal.Copy(handle, iArray, 0, length);
            return iArray;
        }
    }
}