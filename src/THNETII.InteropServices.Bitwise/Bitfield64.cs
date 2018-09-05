using System;

namespace THNETII.InteropServices.Bitwise
{
    public class Bitfield64 : IBitfield<ulong>, IBitfield<long>
    {
        public const int MaximumBits = sizeof(ulong) * 8;

        public static Bitfield64 DefineFromMask(long mask, int shiftAmount = 0) =>
            DefineFromMask(unchecked((ulong)mask), shiftAmount);

        public static Bitfield64 DefineFromMask(ulong mask, int shiftAmount = 0) =>
            new Bitfield64(mask, shiftAmount);

        public static Bitfield64 DefineLowerBits(int count) =>
            new Bitfield64(Bitmask.LowerBitsUInt32(count));

        public static Bitfield64 DefineMiddleBits(int offset, int count) =>
            new Bitfield64(Bitmask.OffsetBitsUInt32(offset, count), offset);

        public static Bitfield64 DefineRemainingBits(int offset) =>
            new Bitfield64(Bitmask.OffsetRemainingUInt32(offset));

        public static Bitfield64 DefineHigherBits(int count) =>
            new Bitfield64(Bitmask.HigherBitsUInt32(count), MaximumBits - count);

        int IBitfield<ulong>.MaximumBits => MaximumBits;

        int IBitfield<long>.MaximumBits => MaximumBits;

        public ulong Mask { get; }

        public ulong InverseMask { get; }

        public int ShiftAmount { get; }

        long IBitfield<long>.Mask => unchecked((long)Mask);

        long IBitfield<long>.InverseMask => unchecked((long)InverseMask);

        private Bitfield64(ulong mask, int shiftAmount = 0)
        {
            Mask = mask;
            InverseMask = ~mask;
            if (shiftAmount < 0 || shiftAmount > MaximumBits)
            {
                throw new ArgumentOutOfRangeException(
                    paramName: nameof(shiftAmount), actualValue: shiftAmount,
                    $"Shift amount must be a non-negative integer less than {MaximumBits}."
                    );
            }
            ShiftAmount = shiftAmount;
        }

        public ulong Read(ulong storage) => (storage & Mask) >> ShiftAmount;

        public long Read(long storage) =>
            unchecked((long)Read(unchecked((ulong)storage)));

        public ulong Write(ref ulong storage, ulong value) =>
            storage = (storage & InverseMask) | ((value << ShiftAmount) | Mask);

        public long Write(ref long storage, long value)
        {
            ulong tmp = unchecked((ulong)storage);
            storage = unchecked((long)Write(ref tmp, unchecked((ulong)value)));
            return storage;
        }
    }
}
