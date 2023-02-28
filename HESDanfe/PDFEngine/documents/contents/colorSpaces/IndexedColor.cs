/*
  Copyright 2010-2012 Stefano Chizzolini. http://www.pdfclown.org

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

using System;
using System.Collections.Generic;
using HESDanfe.Objects;

namespace HESDanfe.Documents.Contents.ColorSpaces
{
    /**
      <summary>Indexed color value [PDF:1.6:4.5.5].</summary>
    */

    [PDF(VersionEnum.PDF11)]
    public sealed class IndexedColor
        : Color
    {
        #region Internal Constructors

        internal IndexedColor(
            IList<PdfDirectObject> components
                             ) : base(
            null, //TODO:consider color space reference!
            new PdfArray(components)
                                     )
        { }

        #endregion Internal Constructors

        #region Public Fields

        public static readonly IndexedColor Default = new IndexedColor(0);

        #endregion Public Fields

        /**
          <summary>Gets the color corresponding to the specified components.</summary>
          <param name="components">Color components to convert.</param>
        */

        #region Public Constructors

        public IndexedColor(
        int index
                           ) : this(
        new List<PdfDirectObject>(
            new PdfDirectObject[] { PdfInteger.Get(index) }
                                 )
                                   )
        { }

        #endregion Public Constructors

        #region Public Properties

        public override IList<PdfDirectObject> Components => (PdfArray)BaseDataObject;

        public int Index
        {
            get => ((PdfInteger)((PdfArray)BaseDataObject)[0]).IntValue;
            set => ((PdfArray)BaseDataObject)[0] = PdfInteger.Get(value);
        }

        #endregion Public Properties

        #region Public Methods

        public static IndexedColor Get(
                          PdfArray components
                                      )
        {
            return (components != null
                ? new IndexedColor(components)
                : Default
                   );
        }

        public override object Clone(
            Document context
                                    )
        { throw new NotImplementedException(); }

        #endregion Public Methods

        /**
          <summary>Gets the color index.</summary>
        */
    }
}