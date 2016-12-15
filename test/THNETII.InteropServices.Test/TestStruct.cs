using System.Runtime.InteropServices;

namespace THNETII.InteropServices.Test
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public class TestStruct
    {
        public static readonly int SizeOf = Marshal.SizeOf<TestStruct>();

        public int intField;
        public double doubleField;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string stringField;
    }
}
