using System;

namespace THNETII.InteropServices.Bitfields
{
    public abstract class BitfieldBase : IBitfield
    {
        private int fieldWidth;
        private uint fieldMask;
        private uint notMask;

        public BitfieldBase(int fieldWidth, int typeWidth)
        {
            if (typeWidth < 1 || typeWidth > (sizeof(uint) * 8))
                throw new ArgumentOutOfRangeException(paramName: nameof(typeWidth), actualValue: typeWidth, message: $"The type width of a bitfield must be a positive integer value not greater than {sizeof(uint) * 8}");
            if (fieldWidth < 1 || fieldWidth > typeWidth)
                throw new ArgumentOutOfRangeException(paramName: nameof(fieldWidth), actualValue: fieldWidth, message: $"The width of a bitfield with a {typeWidth}-bit storage type must be a value between 0 and {typeWidth}.");

            this.fieldWidth = fieldWidth;
            fieldMask = 0;
            for (int i = 0; i < fieldWidth; i++)
                fieldMask |= (1U << i);
            notMask = TypeMask & ~fieldMask;

            TypeWidth = typeWidth;
            TypeMask = 0U;
            for (int i = 0; i < typeWidth; i++)
                TypeMask |= (1U << i);
        }

        public virtual int TypeWidth { get; }

        public virtual uint TypeMask { get; }

        public virtual int FieldWidth => fieldWidth;

        public virtual uint FieldMask => fieldMask;

        public virtual uint InvertedMask => notMask;
    }
}