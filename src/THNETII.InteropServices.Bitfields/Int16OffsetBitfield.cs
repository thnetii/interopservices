namespace THNETII.InteropServices.Bitfields
{
    public class Int16OffsetBitfield : OffsetBitfieldBase, IBitfield<short>
    {
        public new const int TypeWidth = Int16Bitfield.TypeWidth;

        public Int16OffsetBitfield(int fieldOffset, int fieldWidth) : base(fieldOffset, fieldWidth, TypeWidth) { }

        public short Get(short storage) => (short)(((ushort)storage & FieldMask) >> FieldOffset);

        public short Set(short field, short storage) => (short)((((uint)field << FieldOffset) & FieldMask) | ((ushort)storage & InvertedMask));

        public void RefSet(short field, ref short storage) => storage = Set(field, storage);
    }
}
