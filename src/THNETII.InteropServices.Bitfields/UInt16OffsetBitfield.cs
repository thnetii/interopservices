namespace THNETII.InteropServices.Bitfields
{
    public class UInt16OffsetBitfield : OffsetBitfieldBase, IBitfield<ushort>
    {
        public new const int TypeWidth = UInt16Bitfield.TypeWidth;

        public UInt16OffsetBitfield(int fieldOffset, int fieldWidth) : base(fieldOffset, fieldWidth, TypeWidth) { }

        public ushort Get(ushort storage) => (ushort)((storage & FieldMask) >> FieldOffset);

        public ushort Set(ushort field, ushort storage) => (ushort)((((uint)field << FieldOffset) & FieldMask) | (storage & InvertedMask));

        public void RefSet(ushort field, ref ushort storage) => storage = Set(field, storage);
    }
}
