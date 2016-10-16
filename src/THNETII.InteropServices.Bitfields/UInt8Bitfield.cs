namespace THNETII.InteropServices.Bitfields
{
    public class UInt8Bitfield : BitfieldBase, IBitfield<byte>
    {
        public new const int TypeWidth = sizeof(byte) * 8;

        public UInt8Bitfield(int fieldWidth) : base(fieldWidth, TypeWidth) { }

        public byte Get(byte storage) => (byte)(storage & FieldMask);

        public byte Set(byte field, byte storage) => (byte)((field & FieldMask) | (storage & InvertedMask));

        public void RefSet(byte field, ref byte storage) => storage = Set(field, storage);
    }
}
