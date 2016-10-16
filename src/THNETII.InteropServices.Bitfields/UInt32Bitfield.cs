namespace THNETII.InteropServices.Bitfields
{
    public class UInt32Bitfield : BitfieldBase, IBitfield<uint>
    {
        public new const int TypeWidth = sizeof(uint) * 8;

        public UInt32Bitfield(int fieldWidth) : base(fieldWidth, TypeWidth) { }

        public uint Get(uint storage) => storage & FieldMask;

        public uint Set(uint field, uint storage) => (field & FieldMask) | (storage & InvertedMask);

        public void RefSet(uint field, ref uint storage) => storage = Set(field, storage);
    }
}
