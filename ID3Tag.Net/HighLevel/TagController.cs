﻿using ID3Tag.HighLevel.ID3Frame;
using ID3Tag.LowLevel;

namespace ID3Tag.HighLevel
{
    internal class TagController : ITagController
    {
        #region ITagController Members

        public TagContainer Decode(Id3TagInfo info)
        {
            var container = new TagContainer();
            var descriptor = container.Tag;

            // Decode the ID3 Tag info
            var majorVersion = info.MajorVersion;
            var revision = info.Revision;

            descriptor.SetVersion(majorVersion, revision);
            descriptor.SetHeaderFlags(info.UnsynchronisationFlag, info.ExtendedHeaderAvailable, info.Experimental);

            if (info.ExtendedHeaderAvailable)
            {
                var extendedHeader = info.ExtendHeader;
                descriptor.SetExtendedHeader(extendedHeader.PaddingSize, extendedHeader.CRCDataPresent,
                                             extendedHeader.CRC);
            }

            foreach (var rawFrame in info.Frames)
            {
                //
                //  Analyse the frame ID
                //
                var frame = AnalyseFrameID(rawFrame);
                if (frame != null)
                {
                    frame.Import(rawFrame);
                    container.Add(frame);
                }
                else
                {
                    throw new ID3TagException("Frame analysing failed!");
                }
            }

            return container;
        }

        public Id3TagInfo Encode(TagContainer container)
        {
            var tagInfo = new Id3TagInfo();
            var tag = container.Tag;

            tagInfo.MajorVersion = tag.MajorVersion;
            tagInfo.Revision = tag.Revision;
            tagInfo.Experimental = tag.ExperimentalIndicator;
            tagInfo.UnsynchronisationFlag = tag.Unsynchronisation;
            tagInfo.ExtendedHeaderAvailable = tag.ExtendedHeader;
            if (tagInfo.ExtendedHeaderAvailable)
            {
                tagInfo.ExtendHeader = ExtendedTagHeader.Create(tag.PaddingSize, tag.CrcDataPresent, tag.Crc);
            }

            foreach (var frame in container)
            {
                var rawFrame = frame.Convert();
                tagInfo.Frames.Add(rawFrame);
            }

            return tagInfo;
        }

        #endregion

        private static IFrame AnalyseFrameID(RawFrame rawFrame)
        {
            IFrame frame;
            if (rawFrame.ID[0] == 'T' || rawFrame.ID[0] == 'W')
            {
                switch (rawFrame.ID[0])
                {
                    case 'T':
                        if (rawFrame.ID != "TXXX")
                        {
                            frame = new TextFrame();
                        }
                        else
                        {
                            frame = new UserDefinedTextFrame();
                        }
                        break;
                    case 'W':
                        if (rawFrame.ID != "WXXX")
                        {
                            frame = new UrlLinkFrame();
                        }
                        else
                        {
                            frame = new UserDefinedURLLinkFrame();
                        }
                        break;
                    default:
                        throw new ID3TagException("Unknown Text or URL frame!");
                }
            }
            else
            {
                // Other frames
                switch (rawFrame.ID)
                {
                    case "AENC":
                        frame = new AudioEncryptionFrame();
                        break;
                    case "PRIV":
                        frame = new PrivateFrame();
                        break;
                    case "MCDI":
                        frame = new MusicCdIdentifierFrame();
                        break;
                    case "COMM":
                        frame = new CommentFrame();
                        break;
                    default:
                        frame = new UnknownFrame();
                        break;
                }
            }
            return frame;
        }
    }
}