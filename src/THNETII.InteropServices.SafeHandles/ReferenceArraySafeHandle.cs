using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.SafeHandles
{
    public class ReferenceArraySafeHandle<T> : ArraySafeHandle<T>
    {
        protected ReferenceArraySafeHandle() : this(IntPtr.Zero) { }
        /// <param name="invalidHandleValue">The value of an invalid handle (usually <see cref="IntPtr.Zero"/>).</param>
        /// <param name="ownsHandle"><c>true</c> to reliably let the instance release the handle during the finalization phase; otherwise, <c>false</c> (not recommended).</param>
        protected ReferenceArraySafeHandle(IntPtr invalidHandleValue, bool ownsHandle = false) : base(invalidHandleValue, ownsHandle) { }

        /// <summary>
        /// Marshals the contents of the native memory segment referred to by this instance into a managed array with instances of <typeparamref name="T"/>.
        /// <para>
        /// <note type="important">
        /// The <see cref="ReferenceArraySafeHandle{T}"/> class does not maintain information about the allocation size of the native memory that is controlled by the handle.
        /// Ensure that the value of the <paramref name="length"/> parameter is valid.
        /// </note>
        /// </para>
        /// </summary>
        /// <param name="length">The number of pointers contained in the native array that is controlled by this instance.</param>
        /// <returns>A managed array containing instances of type <typeparamref name="T"/>. The returned array will contain exactly <paramref name="length"/> items. If the native array contains <see cref="IntPtr.Zero"/> values, some of the returned items can have <c>null</c> values, if <typeparamref name="T"/> is a reference type.</returns>
        /// <exception cref="ObjectDisposedException">The handle has been closed and the controlled native memory has been released back to the system.</exception>
        /// <exception cref="InvalidOperationException">
        /// The handle controlling the native memory has an invalid value.<br/>
        /// -<em>or</em>-<br/>
        /// The specified item type <typeparamref name="T"/> could not be marshaled by the built-in default Marshaler. In this case, the thrown <see cref="ArgumentException"/> is contained in the <see cref="Exception.InnerException"/> member.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is less than <c>0</c> (zero).</exception>
        /// <exception cref="MissingMethodException">The class specified by <typeparamref name="T"/> does not have an accessible default constructor.</exception>
        public override T[] MarshalToManagedArray(int length)
        {
            if (IsClosed)
                throw new ObjectDisposedException(nameof(handle));
            else if (IsInvalid)
                throw new InvalidOperationException("The native memory handle has an invalid value.");
            else if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length), length, "The array length must be a non-negative value");
            var itemPtrArray = new IntPtr[length];
            Marshal.Copy(handle, itemPtrArray, 0, length);
            try { return itemPtrArray.Select(itemPtrValue => Marshal.PtrToStructure<T>(itemPtrValue)).ToArray(); }
            catch (ArgumentException marshalArgumentExcept) { throw new InvalidOperationException(marshalArgumentExcept.Message, marshalArgumentExcept); }
        }
    }
}
