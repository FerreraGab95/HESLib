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

using HES.Objects;

namespace HES.Documents.Contents.ColorSpaces
{
    /**
      <summary>Device color space [PDF:1.6:4.5.3].</summary>
    */

    [PDF(VersionEnum.PDF11)]
    public abstract class DeviceColorSpace
        : ColorSpace
    {
        #region Protected Constructors

        protected DeviceColorSpace(
        Document context,
        PdfName baseDataObject
                                  ) : base(context, baseDataObject)
        { }

        protected DeviceColorSpace(
            PdfDirectObject baseObject
                                  ) : base(baseObject)
        { }

        #endregion Protected Constructors
    }
}