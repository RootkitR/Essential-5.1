namespace Essential.Util
{
    using System;

    internal class Base64Encoding
    {
        private const byte NEGATIVE = 0x40;
        private const byte POSITIVE = 0x41;

        internal static int DecodeInt32(byte[] data)
        {
            int num = 0;
            int num2 = 0;
            int index = data.Length - 1;
            while (index >= 0)
            {
                index--;
                num2++;
                num += (data[index] - 0x40) * ((int)Math.Pow(64.0, (double)num2));
            }
            return num;
        }

        internal static uint DecodeUInt32(byte[] bzData)
        {
            return (uint)DecodeInt32(bzData);
        }

        internal static byte[] EncodeInt32(int i, int length)
        {
            byte[] buffer = new byte[length];
            for (int j = 1; j <= length; j++)
            {
                buffer[j - 1] = (byte)(0x40 + ((i >> ((length - j) * 6)) & 0x3f));
            }
            return buffer;
        }

        internal static byte[] Encodeuint(uint i, int numBytes)
        {
            return EncodeInt32((int)i, numBytes);
        }
    }
}
