namespace THNETII.InteropServices.Bitfields
{
    public class Int32Bitfield : BitfieldBase, IBitfield<int>
    {
        public new const int TypeWidth = sizeof(int) * 8;

        public Int32Bitfield(int fieldWidth) : base(fieldWidth, TypeWidth) { }

        public int Get(int storage) => (int)((uint)storage & FieldMask);

        public int Set(int field, int storage) => (int)(((uint)field & FieldMask) | ((uint)storage & InvertedMask));

        public void RefSet(int field, ref int storage) => storage = Set(field, storage);
    }
}
