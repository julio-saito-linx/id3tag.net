﻿using System;

namespace ID3Tag.LowLevel
{
    public class ExtendedTagHeader
    {
        private ExtendedTagHeader()
        {
        }

        public bool CRCDataPresent { get; private set; }
        public int PaddingSize { get; private set; }
        public byte[] CRC { get; private set; }

        #region Create Extended Header

        internal static ExtendedTagHeader Create(int paddingSize, bool crcDataPresent, byte[] crc)
        {
            var extendedHeader = new ExtendedTagHeader
                                     {
                                         PaddingSize = paddingSize,
                                         CRCDataPresent = crcDataPresent,
                                         CRC = crc
                                     };

            return extendedHeader;
        }

        internal static ExtendedTagHeader Create(byte[] content)
        {
            var flags = new byte[2];
            var paddingBytes = new byte[4];

            Array.Copy(content, 0, flags, 0, 2);
            Array.Copy(content, 2, paddingBytes, 0, 4);

            var extendedHeader = new ExtendedTagHeader();
            extendedHeader.CRCDataPresent = (flags[0] & 0x80) == 0x80;
            extendedHeader.PaddingSize = Utils.CalculateExtendedHeaderPaddingSize(paddingBytes);

            if (extendedHeader.CRCDataPresent)
            {
                var crcBytes = new byte[4];
                Array.Copy(content, 6, crcBytes, 0, 4);
                extendedHeader.CRC = crcBytes;
            }

            return extendedHeader;
        }

        #endregion
    }
}