namespace THNETII.InteropServices.Bitfields
{
    public class Int16Bitfield : BitfieldBase, IBitfield<short>
    {
        private const int typeWidth = sizeof(short) * 8;
        private const uint typeMask = 0xFFFF;

        public override int TypeWidth => typeWidth;

        public override uint TypeMask => typeMask;

        public Int16Bitfield(int fieldWidth) : base(fieldWidth) { }

        public short Get(short storage) => (short)(storage & fieldMask);

        public short Set(short field, short storage) => (short)(((ushort)field & fieldMask) | ((ushort)storage & notMask));

        public void RefSet(short field, ref short storage)
        {
            storage = (short)(((ushort)field & fieldMask) | ((ushort)storage & notMask));
        }
    }
}
