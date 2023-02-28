/*
  Copyright 2010 Stefano Chizzolini. http://www.pdfclown.org

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

using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using HESDanfe.Documents;
using HESDanfe.Documents.Contents;

namespace HESDanfe.Tools
{
    /**
      <summary>Tool for rendering <see cref="IContentContext">content contexts</see>.</summary>
    */

    public sealed class Renderer
    {
        /**
          <summary>Printable document.</summary>
          <remarks>It wraps a page collection for printing purposes.</remarks>
        */

        #region Public Methods

        public static PrintDocument GetPrintDocument(
            Document document
                                                    )
        { return new PrintDocument(document.Pages); }

        public static PrintDocument GetPrintDocument(
            IList<Page> pages
                                                    )
        { return new PrintDocument(pages); }

        public Image Render(
            Contents contents,
            SizeF size
                           )
        { return Render(contents, size, null); }

        public Image Render(
            IContentContext contentContext,
            SizeF size
                           )
        { return Render(contentContext, size, null); }

        public Image Render(
            Contents contents,
            SizeF size,
            RectangleF? area
                           )
        { return Render(contents.ContentContext, size, area); }

        public Image Render(
            IContentContext contentContext,
            SizeF size,
            RectangleF? area
                           )
        {
            //TODO:area!
            Image image = new Bitmap(
                (int)size.Width,
                (int)size.Height,
                PixelFormat.Format24bppRgb
                                    );
            contentContext.Render(System.Drawing.Graphics.FromImage(image), size);
            return image;
        }

        #endregion Public Methods

        #region Public Classes

        public sealed class PrintDocument
                                : System.Drawing.Printing.PrintDocument
        {
            #region Private Fields

            private int pageIndex;
            private IList<Page> pages;
            private int pagesCount;

            #endregion Private Fields

            #region Protected Methods

            protected override void OnBeginPrint(
                PrintEventArgs e
                                                )
            {
                pageIndex = -1;

                base.OnBeginPrint(e);
            }

            protected override void OnPrintPage(
                PrintPageEventArgs e
                                               )
            {
                PrinterSettings printerSettings = e.PageSettings.PrinterSettings;
                switch (printerSettings.PrintRange)
                {
                    case PrintRange.SomePages:
                        if (pageIndex < printerSettings.FromPage)
                        { pageIndex = printerSettings.FromPage; }
                        else
                        { pageIndex++; }

                        e.HasMorePages = (pageIndex < printerSettings.ToPage);
                        break;

                    default:
                        pageIndex++;

                        e.HasMorePages = (pageIndex + 1 < pagesCount);
                        break;
                }

                Page page = pages[pageIndex];
                page.Render(e.Graphics, e.PageBounds.Size);

                base.OnPrintPage(e);
            }

            #endregion Protected Methods

            #region Public Constructors

            public PrintDocument(
                                 IList<Page> pages
                                )
            { Pages = pages; }

            #endregion Public Constructors

            #region Public Properties

            public IList<Page> Pages
            {
                get => pages;
                set
                {
                    pages = value;
                    pagesCount = pages.Count;
                }
            }

            #endregion Public Properties
        }

        #endregion Public Classes

        /**
          <summary>Wraps the specified document into a printable object.</summary>
          <param name="document">Document to wrap for printing.</param>
          <returns>Printable object.</returns>
        */
        /**
          <summary>Wraps the specified page collection into a printable object.</summary>
          <param name="pages">Page collection to print.</param>
          <returns>Printable object.</returns>
        */
        /**
          <summary>Prints silently the specified document.</summary>
          <param name="document">Document to print.</param>
          <returns>Whether the print was fulfilled.</returns>
        */
        /**
          <summary>Prints the specified document.</summary>
          <param name="document">Document to print.</param>
          <param name="silent">Whether to avoid showing a print dialog.</param>
          <returns>Whether the print was fulfilled.</returns>
        */
        /**
          <summary>Prints silently the specified page collection.</summary>
          <param name="pages">Page collection to print.</param>
          <returns>Whether the print was fulfilled.</returns>
        */
        /**
          <summary>Prints the specified page collection.</summary>
          <param name="pages">Page collection to print.</param>
          <param name="silent">Whether to avoid showing a print dialog.</param>
          <returns>Whether the print was fulfilled.</returns>
        */
        /**
          <summary>Renders the specified contents into an image context.</summary>
          <param name="contents">Source contents.</param>
          <param name="size">Image size expressed in device-space units (that is typically pixels).</param>
          <returns>Image representing the rendered contents.</returns>
         */
        /**
          <summary>Renders the specified content context into an image context.</summary>
          <param name="contentContext">Source content context.</param>
          <param name="size">Image size expressed in device-space units (that is typically pixels).</param>
          <returns>Image representing the rendered contents.</returns>
         */
        /**
          <summary>Renders the specified contents into an image context.</summary>
          <param name="contents">Source contents.</param>
          <param name="size">Image size expressed in device-space units (that is typically pixels).</param>
          <param name="area">Content area to render; <code>null</code> corresponds to the entire
           <see cref="IContentContext.Box">content bounding box</see>.</param>
          <returns>Image representing the rendered contents.</returns>
         */
        /**
          <summary>Renders the specified content context into an image context.</summary>
          <param name="contentContext">Source content context.</param>
          <param name="size">Image size expressed in device-space units (that is typically pixels).</param>
          <param name="area">Content area to render; <code>null</code> corresponds to the entire
           <see cref="IContentContext.Box">content bounding box</see>.</param>
          <returns>Image representing the rendered contents.</returns>
         */
    }
}