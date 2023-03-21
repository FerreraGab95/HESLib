using System;
using System.Diagnostics;
using System.IO;
using HES.Documents;
using HES.Documents.Contents.Composition;
using HES.Documents.Contents.Fonts;
using HES.Files;
using HES.Graphics;

namespace HES
{
    internal class SimplePdfPageTester : IDisposable
    {
        #region Private Fields

        private bool disposedValue = false;

        #endregion Private Fields

        #region Protected Methods

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    File?.Dispose();
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        #endregion Protected Methods

        #region Public Constructors

        public SimplePdfPageTester()
        {
            File = new PdfFile();
            Document = File.Document;

            Page = new Page(Document);
            Document.Pages.Add(Page);

            PrimitiveComposer = new PrimitiveComposer(Page);
            Gfx = new Gfx(PrimitiveComposer);
        }

        #endregion Public Constructors

        #region Public Properties

        public Document Document { get; set; }
        public PdfFile File { get; set; }
        public HES.Graphics.Gfx Gfx { get; set; }
        public Page Page { get; set; }
        public PrimitiveComposer PrimitiveComposer { get; set; }

        #endregion Public Properties

        #region Public Methods

        public Estilo CriarEstilo()
        {
            return new HES.Estilo(new StandardType1Font(Document, StandardType1Font.FamilyEnum.Times, false, false),
                       new StandardType1Font(Document, StandardType1Font.FamilyEnum.Times, true, false),
                       new StandardType1Font(Document, StandardType1Font.FamilyEnum.Times, false, true));
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above. GC.SuppressFinalize(this);
        }

        public FileInfo Save(string path) => File.Save(path, SerializationModeEnum.Standard);

        public void Save()
        {
            Save(new StackTrace().GetFrame(1).GetMethod().Name + ".pdf");
        }

        #endregion Public Methods

        // To detect redundant calls
        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~SimplePdfPageTester() { // Do not change this code. Put cleanup code in Dispose(bool
        // disposing) above. Dispose(false); }
    }
}