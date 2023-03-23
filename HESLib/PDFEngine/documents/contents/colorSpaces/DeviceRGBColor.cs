/*
  Copyright 2006-2012 Stefano Chizzolini. http://www.pdfclown.org

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
using HES.Objects;
using drawing = System.Drawing;

namespace HES.Documents.Contents.ColorSpaces
{
    /**
      <summary>Device Red-Green-Blue color value [PDF:1.6:4.5.3].</summary>
    */

    [PDF(VersionEnum.PDF11)]
    public sealed class DeviceRGBColor
        : DeviceColor
    {
        #region Internal Constructors

        internal DeviceRGBColor(
            IList<PdfDirectObject> components
                               ) : base(
            DeviceRGBColorSpace.Default,
            new PdfArray(components)
                                       )
        { }

        #endregion Internal Constructors

        #region Public Fields

        public static readonly DeviceRGBColor Black = Get(drawing::Color.Black);
        public static readonly DeviceRGBColor Default = Black;
        public static readonly DeviceRGBColor White = Get(drawing::Color.White);

        #endregion Public Fields

        /**
          <summary>Gets the color corresponding to the specified components.</summary>
          <param name="components">Color components to convert.</param>
        */

        #region Public Constructors

        public DeviceRGBColor(
        double r,
        double g,
        double b
                             ) : this(
        new List<PdfDirectObject>(
            new PdfDirectObject[]
            {
                PdfReal.Get(NormalizeComponent(r)),
                PdfReal.Get(NormalizeComponent(g)),
                PdfReal.Get(NormalizeComponent(b))
            }
                                 )
                                     )
        { }

        #endregion Public Constructors

        #region Public Properties

        public double B
        {
            get => GetComponentValue(2);
            set => SetComponentValue(2, value);
        }

        public double G
        {
            get => GetComponentValue(1);
            set => SetComponentValue(1, value);
        }

        public double R
        {
            get => GetComponentValue(0);
            set => SetComponentValue(0, value);
        }

        #endregion Public Properties

        #region Public Methods

        public new static DeviceRGBColor Get(
                                         PdfArray components
                                            ) => (components != null
                ? new DeviceRGBColor(components)
                : Default
                   );

        /**
          <summary>Gets the color corresponding to the specified system color.</summary>
          <param name="color">System color to convert.</param>
        */

        public static DeviceRGBColor Get(
            drawing::Color? color
                                        ) => (color.HasValue
                ? new DeviceRGBColor(color.Value.R / 255d, color.Value.G / 255d, color.Value.B / 255d)
                : Default);

        /**
          <summary>Gets/Sets the blue component.</summary>
        */

        public override object Clone(
            Document context
                                    ) => throw new NotImplementedException();

        #endregion Public Methods

        /**
          <summary>Gets/Sets the green component.</summary>
        */
        /**
          <summary>Gets/Sets the red component.</summary>
        */
    }
}