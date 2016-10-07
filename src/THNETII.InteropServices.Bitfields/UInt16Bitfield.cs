namespace THNETII.InteropServices.Bitfields
{
    public class UInt16Bitfield : BitfieldBase, IBitfield<ushort>
    {
        private const int typeWidth = sizeof(ushort) * 8;
        private const uint typeMask = 0xFFFF;

        public override int TypeWidth => typeWidth;

        public override uint TypeMask => typeMask;

        public UInt16Bitfield(int fieldWidth) : base(fieldWidth) { }

        public ushort Get(ushort storage) => (ushort)(storage & fieldMask);

        public ushort Set(ushort field, ushort storage) => (ushort)((field & fieldMask) | (storage & notMask));

        public void RefSet(ushort field, ref ushort storage)
        {
            storage = (ushort)((field & fieldMask) | (storage & notMask));
        }
    }
}
