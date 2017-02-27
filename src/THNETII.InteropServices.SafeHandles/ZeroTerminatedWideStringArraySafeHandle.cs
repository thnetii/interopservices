using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.SafeHandles
{
    public class ZeroTerminatedWideStringArraySafeHandle : ReferenceArraySafeHandle<string>
    {
        protected ZeroTerminatedWideStringArraySafeHandle() : this(IntPtr.Zero) { }
        /// <param name="invalidHandleValue">The value of an invalid handle (usually <see cref="IntPtr.Zero"/>).</param>
        /// <param name="ownsHandle"><c>true</c> to reliably let the instance release the handle during the finalization phase; otherwise, <c>false</c> (not recommended).</param>
        protected ZeroTerminatedWideStringArraySafeHandle(IntPtr invalidHandleValue, bool ownsHandle = false) : base(invalidHandleValue, ownsHandle) { }

        /// <summary>
        /// Marshals the contents of the native memory segment referred to by this instance into a managed string array.
        /// <para>
        /// <note type="important">
        /// The <see cref="ZeroTerminatedWideStringArraySafeHandle"/> class does not maintain information about the allocation size of the native memory that is controlled by the handle.
        /// Ensure that the value of the <paramref name="length"/> parameter is valid.
        /// </note>
        /// </para>
        /// </summary>
        /// <param name="length">The number of pointers contained in the native array that is controlled by this instance.</param>
        /// <returns>A managed string array. The returned array will contain exactly <paramref name="length"/> items. If the native array contains <see cref="IntPtr.Zero"/> values, some of the returned items can have <c>null</c> values.</returns>
        /// <exception cref="ObjectDisposedException">The handle has been closed and the controlled native memory has been released back to the system.</exception>
        /// <exception cref="InvalidOperationException">
        /// The handle controlling the native memory has an invalid value.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is less than <c>0</c> (zero).</exception>
        public override string[] MarshalToManagedArray(int length)
        {
            if (IsClosed)
                throw new ObjectDisposedException(nameof(handle));
            else if (IsInvalid)
                throw new InvalidOperationException("The native memory handle has an invalid value.");
            else if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length), length, "The array length must be a non-negative value");
            var itemPtrArray = new IntPtr[length];
            Marshal.Copy(handle, itemPtrArray, 0, length);
            return itemPtrArray.Select(itemPtrValue => Marshal.PtrToStringUni(itemPtrValue)).ToArray();
        }
    }
}
