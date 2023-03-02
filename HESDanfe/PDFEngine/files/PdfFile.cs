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
using System.IO;
using System.Reflection;
using Extensions;
using HESDanfe.Bytes;
using HESDanfe.Documents;
using HESDanfe.Objects;
using HESDanfe.Tokens;


namespace HESDanfe.Files
{
    /**
      <summary>PDF file representation.</summary>
    */

    public sealed class PdfFile
    : IDisposable
    {
        /**
          <summary>File configuration.</summary>
        */

        #region Private Fields

        private static Random hashCodeGenerator = new Random();

        private readonly Document document;

        private readonly int hashCode = hashCodeGenerator.Next();

        private readonly IndirectObjects indirectObjects;

        private readonly PdfDictionary trailer;

        private readonly Version version;

        private Cloner cloner;

        private ConfigurationImpl configuration;

        private string path;

        private Reader reader;

        #endregion Private Fields

        #region Private Properties

        private string TempPath => (path == null ? System.IO.Path.Combine(System.IO.Path.GetTempPath(), "temp.tmp") : path + ".tmp");

        #endregion Private Properties

        #region Private Methods

        private void Initialize(
                               )
        { configuration = new ConfigurationImpl(this); }

        private PdfDictionary PrepareTrailer(
            PdfDictionary trailer
                                            )
        { return (PdfDictionary)new ImplicitContainer(this, trailer).DataObject; }

        #endregion Private Methods

        #region Private Classes

        private sealed class ImplicitContainer
            : PdfIndirectObject
        {
            #region Public Constructors

            public ImplicitContainer(
                PdfFile file,
                PdfDataObject dataObject
                                    ) : base(file, dataObject, new XRefEntry(int.MinValue, int.MinValue))
            { }

            #endregion Public Constructors
        }

        #endregion Private Classes

        #region Public Constructors

        public PdfFile(
                      )
        {
            Initialize();

            version = VersionEnum.PDF14.GetVersion();
            trailer = PrepareTrailer(new PdfDictionary());
            indirectObjects = new IndirectObjects(this, null);
            document = new Document(this);
        }

        public PdfFile(
            string path
                      ) : this(
            new Bytes.Stream(
                new FileStream(
                path,
                FileMode.Open,
                FileAccess.Read
                              )
                            )
                              )
        { this.path = path; }

        public PdfFile(
            IInputStream stream
                      )
        {
            Initialize();

            reader = new Reader(stream, this);

            Reader.FileInfo info = reader.ReadInfo();
            version = info.Version;
            trailer = PrepareTrailer(info.Trailer);
            if (trailer.ContainsKey(PdfName.Encrypt)) // Encrypted file.
                throw new NotImplementedException("Encrypted files are currently not supported.");

            indirectObjects = new IndirectObjects(this, info.XrefEntries);
            document = new Document(trailer[PdfName.Root]);
            document.Configuration.XrefMode = (PdfName.XRef.Equals(trailer[PdfName.Type])
                ? Document.ConfigurationImpl.XRefModeEnum.Compressed
                : Document.ConfigurationImpl.XRefModeEnum.Plain);
        }

        #endregion Public Constructors

        #region Public Properties

        public Cloner Cloner
        {
            get
            {
                if (cloner == null)
                { cloner = new Cloner(this); }

                return cloner;
            }
            set => cloner = value;
        }

        public ConfigurationImpl Configuration => configuration;

        public Document Document => document;

        public FileIdentifier ID => FileIdentifier.Wrap(Trailer[PdfName.ID]);

        public IndirectObjects IndirectObjects => indirectObjects;

        public string Path
        {
            get => path;
            set => path = value;
        }

        public Reader Reader => reader;

        public PdfDictionary Trailer => trailer;

        public Version Version => version;

        #endregion Public Properties

        #region Public Methods

        public void Dispose(
                           )
        {
            if (reader != null)
            {
                reader.Dispose();
                reader = null;

                /*
                  NOTE: If the temporary file exists (see Save() method), it must overwrite the document file.
                */
                if (System.IO.File.Exists(TempPath))
                {
                    System.IO.File.Delete(path);
                    System.IO.File.Move(TempPath, path);
                }
            }

            GC.SuppressFinalize(this);
        }

        public override int GetHashCode(
                                       )
        { return hashCode; }

        public PdfReference Register(
            PdfDataObject obj
                                    )
        { return indirectObjects.Add(obj).Reference; }

        public void Save(
                        )
        { Save(SerializationModeEnum.Standard); }

        public FileInfo Save(SerializationModeEnum mode)
        {
            /*
              NOTE: The document file cannot be directly overwritten as it's locked for reading by the
              open stream; its update is therefore delayed to its disposal, when the temporary file will
              overwrite it (see Dispose() method).
            */
            return Save(TempPath, mode);
        }

        public FileInfo Save(string path, SerializationModeEnum mode)
        {
            path.CreateDirectoryIfNotExists();
            FileStream outputStream = new System.IO.FileStream(path, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            Save(new Bytes.Stream(outputStream), mode);
            outputStream.Flush();
            outputStream.Close();
            return path.ToFileInfo();
        }

        public void Save(IOutputStream stream, SerializationModeEnum mode)
        {
            if (Reader == null)
            {
                try
                {
                    string assemblyTitle = ((AssemblyTitleAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyTitleAttribute))).Title;
                    string assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                    Document.Information.Producer = assemblyTitle + " " + assemblyVersion;
                }
                catch
                {/* NOOP */}
            }

            Writer writer = Writer.Get(this, stream);
            writer.Write(mode);
        }

        public void Unregister(
            PdfReference reference
                              )
        { indirectObjects.RemoveAt(reference.ObjectNumber); }

        #endregion Public Methods

        #region Public Classes

        public sealed class ConfigurationImpl
        {
            #region Private Fields

            private readonly PdfFile file;
            private string realFormat;

            #endregion Private Fields

            #region Internal Constructors

            internal ConfigurationImpl(
                PdfFile file
                                      )
            { this.file = file; }

            #endregion Internal Constructors

            /**
              <summary>Gets the file associated with this configuration.</summary>
            */

            #region Public Properties

            public PdfFile File => file;

            /**
              <summary>Gets/Sets the format applied to real number serialization.</summary>
            */

            public string RealFormat
            {
                get
                {
                    if (realFormat == null)
                    { SetRealFormat(5); }
                    return realFormat;
                }
                set => realFormat = value;
            }

            #endregion Public Properties

            /**
              <param name="decimalPlacesCount">Number of digits in decimal places.</param>
              <seealso cref="RealFormat"/>
            */

            #region Public Methods

            public void SetRealFormat(
                int decimalPlacesCount
                                     )
            { realFormat = "0." + new string('#', decimalPlacesCount); }

            #endregion Public Methods
        }

        #endregion Public Classes

        /**
          <summary>Gets/Sets the default cloner.</summary>
        */
        /**
          <summary>Gets the file configuration.</summary>
        */
        /**
          <summary>Gets the high-level representation of the file content.</summary>
        */
        /**
          <summary>Gets the identifier of this file.</summary>
        */
        /**
          <summary>Gets the indirect objects collection.</summary>
        */
        /**
          <summary>Gets/Sets the file path.</summary>
        */
        /**
          <summary>Gets the data reader backing this file.</summary>
          <returns><code>null</code> in case of newly-created file.</returns>
        */
        /**
          <summary>Registers an <b>internal data object</b>.</summary>
        */
        /**
          <summary>Serializes the file to the current file-system path using the <see
          cref="SerializationModeEnum.Standard">standard serialization mode</see>.</summary>
        */
        /**
          <summary>Serializes the file to the current file-system path.</summary>
          <param name="mode">Serialization mode.</param>
        */
        /**
          <summary>Serializes the file to the specified file system path.</summary>
          <param name="path">Target path.</param>
          <param name="mode">Serialization mode.</param>
        */
        /**
          <summary>Serializes the file to the specified stream.</summary>
          <remarks>It's caller responsibility to close the stream after this method ends.</remarks>
          <param name="stream">Target stream.</param>
          <param name="mode">Serialization mode.</param>
        */
        /**
          <summary>Gets the file trailer.</summary>
        */
        /**
          <summary>Unregisters an internal object.</summary>
        */
        /**
          <summary>Gets the file header version [PDF:1.6:3.4.1].</summary>
          <remarks>This property represents just the original file version; to get the actual version,
          use the <see cref="HESDanfe.Documents.Document.Version">Document.Version</see> method.
          </remarks>
        */
    }
}