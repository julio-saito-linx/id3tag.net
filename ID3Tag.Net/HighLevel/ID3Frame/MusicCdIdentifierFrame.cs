﻿using System.Text;
using ID3Tag.LowLevel;

namespace ID3Tag.HighLevel.ID3Frame
{
    /// <summary>
    /// This frame is intended for music that comes from a CD, so that the CD can be identified 
    /// in databases such as the CDDB. The frame consists of a binary dump of the Table Of Contents, 
    /// TOC, from the CD, which is a header of 4 bytes and then 8 bytes/track on the CD plus 8 bytes 
    /// for the 'lead out' making a maximum of 804 bytes. The offset to the beginning of every 
    /// track on the CD should be described with a four bytes absolute CD-frame address per 
    /// track, and not with absolute time. This frame requires a present and valid "TRCK" frame, 
    /// even if the CD's only got one track. There may only be one "MCDI" frame in each tag.
    /// </summary>
    public class MusicCdIdentifierFrame : Frame
    {
        /// <summary>
        /// Creates a new instance of MusicCdIdentifierFrame
        /// </summary>
        public MusicCdIdentifierFrame()
        {
            TOC = new byte[0];
        }

        /// <summary>
        /// Creates a new instance of MusicCdIdentifierFrame
        /// </summary>
        /// <param name="toc">the toc.</param>
        public MusicCdIdentifierFrame(byte[] toc)
        {
            Descriptor.ID = "MCDI";
            TOC = toc;
        }

        /// <summary>
        /// The TOC of CD.
        /// </summary>
        public byte[] TOC { get; set; }

        public override FrameType Type
        {
            get { return FrameType.MusicCDIdentifier; }
        }

        public override RawFrame Convert()
        {
            var flagBytes = Descriptor.GetFlagBytes();
            var frame = RawFrame.CreateFrame(Descriptor.ID, flagBytes, TOC);
            return frame;
        }

        public override void Import(RawFrame rawFrame)
        {
            ImportRawFrameHeader(rawFrame);

            TOC = rawFrame.Payload;
        }

        /// <summary>
        /// Overwrites ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var sb = new StringBuilder("Music CD Identifier : ");

            sb.AppendFormat("ID : {0} ", Descriptor.ID);

            sb.Append("TOC : ");
            if (TOC != null)
            {
                sb.Append(Utils.BytesToString(TOC));
            }

            return sb.ToString();
        }
    }
}