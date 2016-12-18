using System;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.SafeHandles
{
    /// <summary>
    /// Represents a safe handle to native memory that contains a single instance of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The managed type that is stored in native memory.</typeparam>
    public class SafeHandle<T> : SafeHandle
    {
        /// <summary>
        /// Gets a value indicating whether the handle value is invalid.
        /// </summary>
        /// <value><c>true</c> if the handle value is invalid; otherwise, <c>false</c>.</value>
        /// <remarks>The default implementation defines <see cref="IntPtr.Zero"/> as the invalid handle value.</remarks>
        public override bool IsInvalid => handle == IntPtr.Zero;

        /// <summary>
        /// Executes the code required to free the handle.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the handle is released successfully; otherwise, in the event of a catastrophic
        /// failure, <c>false</c>. In this case, it generates a releaseHandleFailed MDA Managed
        /// Debugging Assistant.
        /// </returns>
        /// <remarks>The default implementation defined in the <see cref="SafeHandle{T}"/> class does not do anything to release the handle. Override this member to provide a specific release implementation.</remarks>
        protected override bool ReleaseHandle() => true;

        /// <summary>
        /// Marshals the contents of the native memory segment referred to by this instance into a managed instance of <typeparamref name="T"/>.
        /// <para>
        /// <note type="important">
        /// The <see cref="SafeHandle{T}"/> class does not maintain information about the allocation size of the native memory that is controlled by the handle.
        /// </note>
        /// </para>
        /// </summary>
        /// <returns>A managed instance of type <typeparamref name="T"/>.</returns>
        /// <exception cref="ObjectDisposedException">The handle has been closed and the controlled native memory has been released back to the system.</exception>
        /// <exception cref="InvalidOperationException">
        /// The native memory handle has an invalid value. Is this case, <see cref="IsInvalid"/> evaluates to <c>true</c>.
        /// <br/>-<em>or</em>-<br/>
        /// The layout of <typepararef name="T"/> is not sequential or explicit.
        /// </exception>
        /// <exception cref="MissingMethodException">The class specified by <typepararef name="T"/> does not have an accessible default constructor.</exception>
        /// <exception cref="NotSupportedException">Direct marshaling to a managed instance is not supported. This exception is thrown for exmaple by all array handle classes that derive from <see cref="SafeHandle{T}"/>.</exception>
        public virtual T MarshalToManagedInstance()
        {
            if (IsClosed)
                throw new ObjectDisposedException(nameof(handle));
            else if (IsInvalid)
                throw new InvalidOperationException("The native memory handle has an invalid value.");
            try { return Marshal.PtrToStructure<T>(handle); }
            catch (ArgumentException argExcept) { throw new InvalidOperationException(argExcept.Message, argExcept); }
        }

        /// <summary>
        /// Initializes a new safe handle with a zero-pointer.
        /// </summary>
        protected SafeHandle() : this(IntPtr.Zero, ownsHandle: true) { }
        /// <summary>
        /// Initializes a new safe handle with the specified invalid handle value.
        /// </summary>
        protected SafeHandle(IntPtr invalidHandleValue, bool ownsHandle) : base(invalidHandleValue, ownsHandle) { }
    }
}
