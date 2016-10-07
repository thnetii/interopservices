using System;

namespace THNETII.InteropServices.Bitfields
{
    public abstract class BitfieldBase : IBitfield
    {
        protected readonly int fieldWidth;
        protected readonly uint fieldMask;
        protected readonly uint notMask;

        public BitfieldBase(int fieldWidth)
        {
            if (fieldWidth < 1 || fieldWidth > 7)
                throw new ArgumentOutOfRangeException(paramName: nameof(fieldWidth), actualValue: fieldWidth, message: $"The width of a bitfield with a {TypeWidth}-bit storage type must be a value between 0 and {TypeWidth}.");

            this.fieldWidth = fieldWidth;
            fieldMask = 0;
            for (int i = 0; i < fieldWidth; i++)
                fieldMask |= (1U << i);
            notMask = TypeMask & ~fieldMask;
        }

        public abstract int TypeWidth { get; }

        public abstract uint TypeMask { get; }

        public virtual int FieldWidth => fieldWidth;

        public virtual uint FieldMask => fieldMask;

        public virtual uint InvertedMask => notMask;
    }
}