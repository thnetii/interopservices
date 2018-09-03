using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace THNETII.InteropServices.Bitfield
{
    public interface IBitfieldDefinition<T>
    {
        T Mask { get; }
        int ShiftAmount { get; }

        T Read(T storage);
        void Write(ref T storage, T value);
    }

    public class BitfieldUInt32Definition : IBitfieldDefinition<uint>
    {
        private const int totalBits = sizeof(uint) * 8;

        public uint Mask { get; }
        public int ShiftAmount { get; }

        public BitfieldUInt32Definition(uint mask, int shift = 0)
        {
            if (shift < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(shift), shift,
                    "Bitwise shift amount must be non-negative"
                    );
            }
            else if (shift >= totalBits)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(shift), shift,
                    $"Bitwise shift amount must be less than the total number of bits in the storage type ({totalBits} bits)"
                    );
            }

            Mask = mask;
            ShiftAmount = shift;
        }

        public uint Read(uint storage) => (storage & Mask) >> ShiftAmount;

        public void Write(ref uint storage, uint value) =>
            storage |= (value << ShiftAmount) & Mask;
    }

    public class BitfieldUInt64Definition : IBitfieldDefinition<ulong>
    {
        private const int totalBits = sizeof(uint) * 8;

        public ulong Mask { get; }

        public int ShiftAmount { get; }

        public BitfieldUInt64Definition(uint mask, int shift = 0)
        {
            if (shift < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(shift), shift,
                    "Bitwise shift amount must be non-negative"
                    );
            }
            else if (shift >= totalBits)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(shift), shift,
                    $"Bitwise shift amount must be less than the total number of bits in the storage type ({totalBits} bits)"
                    );
            }

            Mask = mask;
            ShiftAmount = shift;
        }

        public ulong Read(ulong storage) => (storage & Mask) >> ShiftAmount;

        public void Write(ref ulong storage, ulong value)
        {
            storage |= (value << ShiftAmount) & Mask;
        }
    }

    public class BitfieldInt32Definition : IBitfieldDefinition<int>
    {
        private readonly BitfieldUInt32Definition definition;

        public BitfieldInt32Definition(uint mask, int shift) =>
            definition = new BitfieldUInt32Definition(mask, shift);

        public int Mask => unchecked((int)(definition.Mask));

        public int ShiftAmount => definition.ShiftAmount;

        public int Read(int storage) =>
            unchecked((int)definition.Read(unchecked((uint)storage)));

        public void Write(ref int storage, int value)
        {
            Span<int> storageSpan;
            Span<uint> castSpan;
            unsafe
            {
                var ptr = Unsafe.AsPointer(ref storage);
                storageSpan = new Span<int>(ptr, length: 1);
            }
            castSpan = MemoryMarshal.Cast<int, uint>(storageSpan);
            definition.Write(ref castSpan[0], unchecked((uint)value));
        }
    }

    public class BitfieldInt64Definition : IBitfieldDefinition<long>
    {
        private readonly BitfieldUInt64Definition definition;

        public BitfieldInt64Definition(uint mask, int shift) =>
            definition = new BitfieldUInt64Definition(mask, shift);

        public long Mask => unchecked((long)(definition.Mask));

        public int ShiftAmount => definition.ShiftAmount;

        public long Read(long storage) =>
            unchecked((long)definition.Read(unchecked((ulong)storage)));

        public void Write(ref long storage, long value)
        {
            Span<long> storageSpan;
            Span<ulong> castSpan;
            unsafe
            {
                var ptr = Unsafe.AsPointer(ref storage);
                storageSpan = new Span<long>(ptr, sizeof(long) * 1);
            }
            castSpan = MemoryMarshal.Cast<long, ulong>(storageSpan);
            definition.Write(ref castSpan[0], unchecked((ulong)value));
        }
    }

    public class BitfieldUInt16Definition : IBitfieldDefinition<ushort>
    {
        private readonly BitfieldUInt32Definition definition;

        public ushort Mask => unchecked((ushort)(definition.Mask));

        public int ShiftAmount => definition.ShiftAmount;

        public ushort Read(ushort storage) =>
            unchecked((ushort)definition.Read(storage));

        public void Write(ref ushort storage, ushort value)
        {
            uint tmp = storage;
            definition.Write(ref tmp, value);
            storage = unchecked((ushort)tmp);
        }
    }

    public class BitfieldInt16Definition : IBitfieldDefinition<short>
    {
        private readonly BitfieldInt32Definition definition;

        public short Mask => unchecked((short)(definition.Mask));

        public int ShiftAmount => definition.ShiftAmount;

        public short Read(short storage) =>
            unchecked((short)definition.Read(storage));

        public void Write(ref short storage, short value)
        {
            int tmp = storage;
            definition.Write(ref tmp, value);
            storage = unchecked((short)tmp);
        }
    }

    public class BitfieldUInt8Definition : IBitfieldDefinition<byte>
    {
        private readonly BitfieldUInt32Definition definition;

        public byte Mask => unchecked((byte)(definition.Mask));

        public int ShiftAmount => definition.ShiftAmount;

        public byte Read(byte storage) =>
            unchecked((byte)definition.Read(storage));

        public void Write(ref byte storage, byte value)
        {
            uint tmp = storage;
            definition.Write(ref tmp, value);
            storage = unchecked((byte)tmp);
        }
    }

    public class BitfieldInt8Definition : IBitfieldDefinition<sbyte>
    {
        private readonly BitfieldInt32Definition definition;

        public sbyte Mask => unchecked((sbyte)(definition.Mask));

        public int ShiftAmount => definition.ShiftAmount;

        public sbyte Read(sbyte storage) =>
            unchecked((sbyte)definition.Read(storage));

        public void Write(ref sbyte storage, sbyte value)
        {
            int tmp = storage;
            definition.Write(ref tmp, value);
            storage = unchecked((sbyte)tmp);
        }
    }
}
