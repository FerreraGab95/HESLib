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

namespace HES.Documents.Contents.ColorSpaces
{
    /**
      <summary>Device Cyan-Magenta-Yellow-Key color value [PDF:1.6:4.5.3].</summary>
      <remarks>The 'Key' component is renamed 'Black' to avoid semantic
      ambiguities.</remarks>
    */

    [PDF(VersionEnum.PDF11)]
    public sealed class DeviceCMYKColor
        : DeviceColor
    {
        #region Internal Constructors

        internal DeviceCMYKColor(
            IList<PdfDirectObject> components
                                ) : base(
            DeviceCMYKColorSpace.Default,
            new PdfArray(components)
                                        )
        { }

        #endregion Internal Constructors

        #region Public Fields

        public static readonly DeviceCMYKColor Black = new DeviceCMYKColor(0, 0, 0, 1);
        public static readonly DeviceCMYKColor Default = Black;
        public static readonly DeviceCMYKColor White = new DeviceCMYKColor(0, 0, 0, 0);

        #endregion Public Fields

        /**
          <summary>Gets the color corresponding to the specified components.</summary>
          <param name="components">Color components to convert.</param>
        */

        #region Public Constructors

        public DeviceCMYKColor(
        double c,
        double m,
        double y,
        double k
                              ) : this(
        new List<PdfDirectObject>(
            new PdfDirectObject[]
            {
                PdfReal.Get(NormalizeComponent(c)),
                PdfReal.Get(NormalizeComponent(m)),
                PdfReal.Get(NormalizeComponent(y)),
                PdfReal.Get(NormalizeComponent(k))
            }
                                 )
                                      )
        { }

        #endregion Public Constructors

        #region Public Properties

        public double C
        {
            get => GetComponentValue(0);
            set => SetComponentValue(0, value);
        }

        public double K
        {
            get => GetComponentValue(3);
            set => SetComponentValue(3, value);
        }

        public double M
        {
            get => GetComponentValue(1);
            set => SetComponentValue(1, value);
        }

        public double Y
        {
            get => GetComponentValue(2);
            set => SetComponentValue(2, value);
        }

        #endregion Public Properties

        #region Public Methods

        public new static DeviceCMYKColor Get(
                                              PdfArray components
                                             ) => (components != null
                ? new DeviceCMYKColor(components)
                : Default
                   );

        /**
          <summary>Gets/Sets the cyan component.</summary>
        */

        public override object Clone(
            Document context
                                    ) => throw new NotImplementedException();

        #endregion Public Methods

        /**
          <summary>Gets/Sets the black (key) component.</summary>
        */
        /**
          <summary>Gets/Sets the magenta component.</summary>
        */
        /**
          <summary>Gets/Sets the yellow component.</summary>
        */
    }
}