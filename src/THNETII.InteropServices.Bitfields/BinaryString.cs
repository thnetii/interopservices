using System;
using System.Text;

namespace THNETII.InteropServices.Bitfields
{
    public class BinaryString
    {
        private static readonly BinaryString numericInstance = new BinaryString(setChar: '1', unsetChar: '0');

        public BinaryString Numeric => numericInstance;

        public char SetChar { get; }
        public char UnsetChar { get; }

        public BinaryString(char setChar, char unsetChar)
        {
            if (setChar == unsetChar)
                throw new ArgumentException($"{nameof(setChar)} must not be equal to ${unsetChar}, as the binary digits must be distiguishable from each other.");
            SetChar = setChar;
            UnsetChar = unsetChar;
        }

        public string ToBinaryString(byte b)
        {
            var strBuf = new StringBuilder(sizeof(byte) * 8);
            var mask = 1U << (sizeof(byte) * 8 - 1);
            for (int i = 0; i < sizeof(byte) * 8; i++, mask >>= 1)
                strBuf.Append((b & mask) != 0 ? SetChar : UnsetChar);
            return strBuf.ToString();
        }

        public string ToBinaryString(sbyte b) => ToBinaryString((byte)b);

        public string ToBinaryString(ushort w)
        {
            var strBuf = new StringBuilder(sizeof(ushort) * 8);
            var mask = 1U << (sizeof(ushort) * 8 - 1);
            for (int i = 0; i < sizeof(ushort) * 8; i++, mask >>= 1)
                strBuf.Append((w & mask) != 0 ? SetChar : UnsetChar);
            return strBuf.ToString();
        }

        public string ToBinaryString(short w) => ToBinaryString((ushort)w);


        public string ToBinaryString(uint dw)
        {
            var strBuf = new StringBuilder(sizeof(uint) * 8);
            var mask = 1U << (sizeof(uint) * 8 - 1);
            for (int i = 0; i < sizeof(uint) * 8; i++, mask >>= 1)
                strBuf.Append((dw & mask) != 0 ? SetChar : UnsetChar);
            return strBuf.ToString();
        }

        public string ToBinaryString(int dw) => ToBinaryString((uint)dw);
    }
}
