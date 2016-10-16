namespace THNETII.InteropServices.Bitfields
{
    public class UInt8OffsetBitfield : OffsetBitfieldBase, IBitfield<byte>
    {
        public new const int TypeWidth = UInt8Bitfield.TypeWidth;

        public UInt8OffsetBitfield(int fieldOffset, int fieldWidth) : base(fieldOffset, fieldWidth, TypeWidth) { }

        public byte Get(byte storage) => (byte)((storage & FieldMask) >> FieldOffset);

        public byte Set(byte field, byte storage) => (byte)((((uint)field << FieldOffset) & FieldMask) | (storage & InvertedMask));

        public void RefSet(byte field, ref byte storage) => storage = Set(field, storage);
    }
}
