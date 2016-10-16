namespace THNETII.InteropServices.Bitfields
{
    public class Int32OffsetBitfield : OffsetBitfieldBase, IBitfield<int>
    {
        public new const int TypeWidth = Int32Bitfield.TypeWidth;

        public Int32OffsetBitfield(int fieldOffset, int fieldWidth) : base(fieldOffset, fieldWidth, TypeWidth) { }

        public int Get(int storage) => (int)(((uint)storage & FieldMask) >> FieldOffset);

        public int Set(int field, int storage) => (int)((((uint)field << FieldOffset) & FieldMask) | ((uint)storage & InvertedMask));

        public void RefSet(int field, ref int storage) => storage = Set(field, storage);
    }
}
