/*
  Copyright 2006-2010 Stefano Chizzolini. http://www.pdfclown.org

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

namespace HES.Bytes
{
    /**
      <summary>Output stream interface.</summary>
    */

    public interface IOutputStream
        : IStream
    {
        /**
          <summary>Writes a byte array into the stream.</summary>
          <param name="data">Byte array to write into the stream.</param>
        */

        #region Public Methods

        void Write(
        byte[] data
                  );

        /**
          <summary>Writes a byte range into the stream.</summary>
          <param name="data">Byte array to write into the stream.</param>
          <param name="offset">Location in the byte array at which writing begins.</param>
          <param name="length">Number of bytes to write.</param>
        */

        void Write(
            byte[] data,
            int offset,
            int length
                  );

        /**
          <summary>Writes a string into the stream.</summary>
          <param name="data">String to write into the stream.</param>
        */

        void Write(
            string data
                  );

        /**
          <summary>Writes an <see cref="IInputStream"/> into the stream.</summary>
          <param name="data">IInputStream to write into the stream.</param>
        */

        void Write(
            IInputStream data
                  );

        #endregion Public Methods
    }
}