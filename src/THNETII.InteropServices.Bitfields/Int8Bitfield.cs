namespace THNETII.InteropServices.Bitfields
{
    public class Int8Bitfield : BitfieldBase, IBitfield<sbyte>
    {
        public new const int TypeWidth = sizeof(sbyte) * 8;

        public Int8Bitfield(int fieldWidth) : base(fieldWidth, TypeWidth) { }

        public sbyte Get(sbyte storage) => (sbyte)(storage & FieldMask);

        public sbyte Set(sbyte field, sbyte storage) => (sbyte)(((byte)field & FieldMask) | ((byte)storage & InvertedMask));

        public void RefSet(sbyte field, ref sbyte storage) => storage = Set(field, storage);
    }
}
