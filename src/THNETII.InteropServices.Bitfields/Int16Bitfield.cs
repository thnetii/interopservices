namespace THNETII.InteropServices.Bitfields
{
    public class Int16Bitfield : BitfieldBase, IBitfield<short>
    {
        public new const int TypeWidth = sizeof(short) * 8;

        public Int16Bitfield(int fieldWidth) : base(fieldWidth, TypeWidth) { }

        public short Get(short storage) => (short)(storage & FieldMask);

        public short Set(short field, short storage) => (short)(((ushort)field & FieldMask) | ((ushort)storage & InvertedMask));

        public void RefSet(short field, ref short storage) => storage = Set(field, storage);
    }
}
