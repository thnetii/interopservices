namespace THNETII.InteropServices.Bitfields
{
    public class UInt16Bitfield : BitfieldBase, IBitfield<ushort>
    {
        public new const int TypeWidth = sizeof(ushort) * 8;

        public UInt16Bitfield(int fieldWidth) : base(fieldWidth, TypeWidth) { }

        public ushort Get(ushort storage) => (ushort)(storage & FieldMask);

        public ushort Set(ushort field, ushort storage) => (ushort)((field & FieldMask) | (storage & InvertedMask));

        public void RefSet(ushort field, ref ushort storage) => storage = Set(field, storage);
    }
}
