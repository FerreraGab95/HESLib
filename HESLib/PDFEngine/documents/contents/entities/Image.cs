/*
  Copyright 2006-2011 Stefano Chizzolini. http://www.pdfclown.org

  Contributors:
    * Stefano Chizzolini (original code developer, http://www.stefanochizzolini.it)

  This file should be part of the source code distribution of "PDF Clown library" (the
  Program): see the accompanying README files for more info.

  This Program is free software; you can redistribute it and/or modify it under the terms
  of the GNU Lesser General Public License as published by the Free Software Foundation;
  either version 3 of the License, or (at your option) any later version.

  This Program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY,
  either expressed or implied; without even the implied warranty of MERCHANTABILITY or
  FITNESS FOR A PARTICULAR PURPOSE. See the License for more details.

  You should have received a copy of the GNU Lesser General Public License along with this
  Program (see README files); if not, go to the GNU website (http://www.gnu.org/licenses/).

  Redistribution and use, with or without modification, are permitted provided that such
  redistributions retain the above copyright notice, license and disclaimer, along with
  this list of conditions.
*/

using System.IO;

namespace HES.Documents.Contents.Entities
{
    /**
      <summary>Abstract image object [PDF:1.6:4.8].</summary>
    */

    public abstract class Image : Entity
    {
        #region Private Fields

        private int bitsPerComponent;

        private int height;

        private System.IO.Stream stream;

        private int width;

        #endregion Private Fields

        #region Protected Constructors

        protected Image(System.IO.Stream stream)
        {
            this.stream = stream;
        }

        #endregion Protected Constructors

        #region Protected Properties

        protected System.IO.Stream Stream => stream;

        #endregion Protected Properties

        #region Public Properties

        public int BitsPerComponent
        {
            get => bitsPerComponent;
            protected set => bitsPerComponent = value;
        }

        public int Height
        {
            get => height;
            protected set => height = value;
        }

        public int Width
        {
            get => width;
            protected set => width = value;
        }

        #endregion Public Properties

        #region Public Methods

        public static Image Get(string path)
        {
            return Get(new FileStream(path, FileMode.Open, FileAccess.Read));
        }

        public static Image Get(System.IO.Stream stream)
        {
            // Get the format identifier!
            byte[] formatMarkerBytes = new byte[2];
            stream.Read(formatMarkerBytes, 0, 2);

            // Is JPEG?
            /*
              NOTE: JPEG files are identified by a SOI (Start Of Image) marker [ISO 10918-1].
            */
            if (formatMarkerBytes[0] == 0xFF
                && formatMarkerBytes[1] == 0xD8) // JPEG.
            { return new JpegImage(stream); }
            else // Unknown.
            { return null; }
        }

        #endregion Public Methods

        /**
          <summary>Gets/Sets the number of bits per color component
          [PDF:1.6:4.8.2].</summary>
        */
        /**
          <summary>Gets/Sets the height of the image in samples [PDF:1.6:4.8.2].</summary>
        */
        /**
          <summary>Gets/Sets the width of the image in samples [PDF:1.6:4.8.2].</summary>
        */
        /**
          <summary>Gets the underlying stream.</summary>
        */
    }
}