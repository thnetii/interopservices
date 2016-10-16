namespace THNETII.InteropServices.Bitfields
{
    public class UInt32OffsetBitfield : OffsetBitfieldBase, IBitfield<uint>
    {
        public new const int TypeWidth = UInt32Bitfield.TypeWidth;

        public UInt32OffsetBitfield(int fieldOffset, int fieldWidth) : base(fieldOffset, fieldWidth, TypeWidth) { }

        public uint Get(uint storage) => (storage & FieldMask) >> FieldOffset;

        public uint Set(uint field, uint storage) => ((field << FieldOffset) & FieldMask) | (storage & InvertedMask);

        public void RefSet(uint field, ref uint storage) => storage = Set(field, storage);
    }
}
