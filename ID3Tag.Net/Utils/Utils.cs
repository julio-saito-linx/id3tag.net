﻿using System;
using System.Text;

namespace ID3Tag
{
    internal static class Utils
    {
        internal static int CalculateTagHeaderSize(byte[] size)
        {
            int curValue = 0;

            curValue += CalculateByteValue(size[3], 0, 7);
            curValue += CalculateByteValue(size[2], 7, 7);
            curValue += CalculateByteValue(size[1], 14, 7);
            curValue += CalculateByteValue(size[0], 21, 7);

            return curValue;
        }

        internal static int CalculateExtendedHeaderSize(byte[] size)
        {
            int curValue = 0;

            curValue += CalculateByteValue(size[3], 0, 8);
            curValue += CalculateByteValue(size[2], 8, 8);
            curValue += CalculateByteValue(size[1], 16, 8);
            curValue += CalculateByteValue(size[0], 24, 8);

            return curValue;
        }

        internal static int CalculateExtendedHeaderPaddingSize(byte[] size)
        {
            int curValue = 0;

            curValue += CalculateByteValue(size[3], 0, 8);
            curValue += CalculateByteValue(size[2], 8, 8);
            curValue += CalculateByteValue(size[1], 16, 8);
            curValue += CalculateByteValue(size[0], 24, 8);

            return curValue;
        }

        internal static long CalculateFrameHeaderSize(byte[] size)
        {
            int curValue = 0;

            curValue += CalculateByteValue(size[3], 0, 8);
            curValue += CalculateByteValue(size[2], 8, 8);
            curValue += CalculateByteValue(size[1], 16, 8);
            curValue += CalculateByteValue(size[0], 24, 8);

            return curValue;
        }

        internal static string GetFrameID(byte[] frameID)
        {
            var sb = new StringBuilder();
            sb.Append(Convert.ToChar(frameID[0]));
            sb.Append(Convert.ToChar(frameID[1]));
            sb.Append(Convert.ToChar(frameID[2]));
            sb.Append(Convert.ToChar(frameID[3]));

            return sb.ToString();
        }

        private static int CalculateByteValue(byte b, int index, int maxBit)
        {
            var curIndex = index;
            int curValue = 0;

            if ((b & 0x01) == 0x01)
            {
                curValue += Convert.ToInt32(Math.Pow(2, curIndex));
            }
            curIndex++;

            if ((b & 0x02) == 0x02)
            {
                curValue += Convert.ToInt32(Math.Pow(2, curIndex));
            }
            curIndex++;

            if ((b & 0x04) == 0x04)
            {
                curValue += Convert.ToInt32(Math.Pow(2, curIndex));
            }
            curIndex++;

            if ((b & 0x08) == 0x08)
            {
                curValue += Convert.ToInt32(Math.Pow(2, curIndex));
            }
            curIndex++;

            if ((b & 0x10) == 0x10)
            {
                curValue += Convert.ToInt32(Math.Pow(2, curIndex));
            }
            curIndex++;

            if ((b & 0x20) == 0x20)
            {
                curValue += Convert.ToInt32(Math.Pow(2, curIndex));
            }
            curIndex++;

            if ((b & 0x40) == 0x40)
            {
                curValue += Convert.ToInt32(Math.Pow(2, curIndex));
            }
            curIndex++;

            if (maxBit > 7)
            {
                if ((b & 0x80) == 0x80)
                {
                    curValue += Convert.ToInt32(Math.Pow(2, curIndex));
                }
            }

            return curValue;
        }

        public static string BytesToString(byte[] bytes)
        {
            var sb = new StringBuilder();

            foreach (var byteValue in bytes)
            {
                sb.AppendFormat("{0:X2} ", byteValue);
            }

            return sb.ToString();
        }
    }
}