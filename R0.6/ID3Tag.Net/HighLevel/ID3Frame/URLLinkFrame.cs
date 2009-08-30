﻿using System.Text;
using ID3Tag.LowLevel;

namespace ID3Tag.HighLevel.ID3Frame
{
    /// <summary>
    /// This frame is intended for URL links concerning the audiofile in a similar way to 
    /// the other "W"-frames. The frame body consists of a description of the string, 
    /// represented as a terminated string, followed by the actual URL. The URL is always 
    /// encoded with ISO-8859-1. There may be more than one "WXXX" frame in each tag, but 
    /// only one with the same description.
    /// </summary>
    public class UrlLinkFrame : Frame
    {
        /// <summary>
        /// Creates a new instance of UrlLinkFrame.
        /// </summary>
        public UrlLinkFrame()
        {
        }

        /// <summary>
        /// Creates a new instance of UrlLinkFrame.
        /// </summary>
        /// <param name="id">the frame ID.</param>
        /// <param name="url">the url.</param>
        public UrlLinkFrame(string id, string url)
        {
            Descriptor.ID = id;
            URL = url;
        }

        /// <summary>
        /// The URL.
        /// </summary>
        public string URL { get; set; }

        /// <summary>
        /// The frame Type.
        /// </summary>
        public override FrameType Type
        {
            get { return FrameType.URLLink; }
        }

		/// <summary>
		/// Convert the URLLinkFrame.
		/// </summary>
		/// <param name="version">The version.</param>
		/// <returns>the RawFrame.</returns>
        public override RawFrame Convert(TagVersion version)
        {
            var flag = Descriptor.GetFlags();
			var payloadBytes = Converter.GetContentBytes(TextEncodingType.Ansi, 28591, URL);

            var rawFrame = RawFrame.CreateFrame(Descriptor.ID, flag, payloadBytes, version);
            return rawFrame;
        }

		/// <summary>
		/// Import the raw frame data.
		/// </summary>
		/// <param name="rawFrame">the raw frame.</param>
		/// <param name="codePage">Default code page for Ansi encoding. Pass 0 to use default system encoding code page.</param>
        public override void Import(RawFrame rawFrame, int codePage)
        {
            ImportRawFrameHeader(rawFrame);

            // Simple ANSI coding in the payload!
			var chars = Converter.Extract(TextEncodingType.Ansi, 28591, rawFrame.Payload, true);
            URL = new string(chars);
        }

        /// <summary>
        /// Overwrite ToString.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var stringBuilder = new StringBuilder("URL Link Frame : ");

            stringBuilder.Append("URL : ");
            stringBuilder.Append(URL);

            return stringBuilder.ToString();
        }
    }
}