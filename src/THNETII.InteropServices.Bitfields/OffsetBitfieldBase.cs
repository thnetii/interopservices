using System;

namespace THNETII.InteropServices.Bitfields
{
    public class OffsetBitfieldBase : BitfieldBase, IOffsetBitfield
    {
        public OffsetBitfieldBase(int fieldOffset, int fieldWidth, int typeWidth) : base(fieldWidth, typeWidth)
        {
            if (fieldOffset < 0 || (fieldOffset + fieldWidth) >= typeWidth)
            {
                throw new ArgumentOutOfRangeException(paramName: nameof(fieldOffset), actualValue: fieldOffset,
                    message: "The specified bitfield offset causes the field to overflow over the width of its underlying type.");
            }

            FieldOffset = fieldOffset;
            FieldMask = base.FieldMask << fieldOffset;
            InvertedMask = TypeMask & ~FieldMask;
        }

        public override uint FieldMask { get; }

        public override uint InvertedMask { get; }

        public int FieldOffset { get; }
    }
}
