namespace THNETII.InteropServices.Bitfields
{
    public class Int8OffsetBitfield : OffsetBitfieldBase, IBitfield<sbyte>
    {
        public new const int TypeWidth = Int8Bitfield.TypeWidth;

        public Int8OffsetBitfield(int fieldOffset, int fieldWidth) : base(fieldOffset, fieldWidth, TypeWidth) { }

        public sbyte Get(sbyte storage) => (sbyte)(((byte)storage & FieldMask) >> FieldOffset);

        public sbyte Set(sbyte field, sbyte storage) => (sbyte)((((uint)field << FieldOffset) & FieldMask) | ((byte)storage & InvertedMask));

        public void RefSet(sbyte field, ref sbyte storage) => storage = Set(field, storage);
    }
}
