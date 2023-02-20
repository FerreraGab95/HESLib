using System;
using System.Collections.Generic;
using DanfeSharp.Blocos;
using DanfeSharp.Modelo;
using org.pdfclown.documents;
using org.pdfclown.documents.contents.fonts;
using PDF = org.pdfclown.files;

namespace DanfeSharp
{
    public class DocumentoFiscal : IDisposable
    {
        #region Private Fields

        private StandardType1Font.FamilyEnum _FonteFamilia;
        private StandardType1Font _FonteItalico;
        private StandardType1Font _FonteNegrito;
        private StandardType1Font _FonteRegular;
        private org.pdfclown.documents.contents.xObjects.XObject _LogoObject = null;
        private bool disposedValue = false;

        #endregion Private Fields

        #region Private Methods

        private void AdicionarMetadata()
        {
            var info = PdfDocument.Information;
            info[new org.pdfclown.objects.PdfName("ChaveAcesso")] = ViewModel.ChaveAcesso;
            info[new org.pdfclown.objects.PdfName("TipoDocumento")] = $"{ViewModel.TipoDocumento}";
            info.CreationDate = DateTime.Now;
            info.Creator = string.Format("{0} {1} - {2}", "H&S Technologies DanfeSharp", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version, "https://github.com/zonaro/DanfeSharp");
            info.Title = ViewModel.TipoDocumento == TipoDocumento.DANFE ? "DANFE (Documento Auxiliar da NFe)" : "CCE (Carta de Correção Eletrônica)";
        }

        private Estilo CriarEstilo(float tFonteCampoCabecalho = 6, float tFonteCampoConteudo = 10) => new Estilo(_FonteRegular, _FonteNegrito, _FonteItalico, tFonteCampoCabecalho, tFonteCampoConteudo);

        private DanfePagina CriarPagina()
        {
            DanfePagina p = new DanfePagina(this);
            Paginas.Add(p);
            p.DesenharBlocos(Paginas.Count == 1);
            //p.DesenharCreditos();

            // Ambiente de homologação
            // 7. O DANFE emitido para representar NF-e cujo uso foi autorizado em ambiente de
            // homologação sempre deverá conter a frase “SEM VALOR FISCAL” no quadro “Informações
            // Complementares” ou em marca d’água destacada.
            if (ViewModel.TipoAmbiente == 2)
                p.DesenharAvisoHomologacao();

            return p;
        }

        #endregion Private Methods

        #region Protected Methods

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    PDFFile.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        #endregion Protected Methods

        #region Internal Fields

        internal List<BlocoBase> _Blocos;

        #endregion Internal Fields

        #region Internal Properties

        internal BlocoCanhoto Canhoto { get; private set; }
        internal Estilo EstiloPadrao { get; private set; }
        internal BlocoIdentificacaoEmitente IdentificacaoEmitente { get; private set; }
        internal List<DanfePagina> Paginas { get; private set; }
        internal Document PdfDocument => PDFFile.Document;

        #endregion Internal Properties

        #region Internal Methods

        internal T AdicionarBloco<T>() where T : BlocoBase
        {
            var bloco = CriarBloco<T>();
            _Blocos.Add(bloco);
            return bloco;
        }

        internal T AdicionarBloco<T>(Estilo estilo) where T : BlocoBase
        {
            var bloco = CriarBloco<T>(estilo);
            _Blocos.Add(bloco);
            return bloco;
        }

        internal void AdicionarBloco(BlocoBase bloco) => _Blocos.Add(bloco);

        internal T CriarBloco<T>() where T : BlocoBase => (T)Activator.CreateInstance(typeof(T), ViewModel, EstiloPadrao);

        internal T CriarBloco<T>(Estilo estilo) where T : BlocoBase => (T)Activator.CreateInstance(typeof(T), ViewModel, estilo);

        internal void PreencherNumeroFolhas()
        {
            int nFolhas = Paginas.Count;
            for (int i = 0; i < Paginas.Count; i++)
            {
                Paginas[i].DesenhaNumeroPaginas(i + 1, nFolhas);
            }
        }

        #endregion Internal Methods

        #region Public Constructors

        public DocumentoFiscal(string xmlPath, string logoPath = null) : this(DocumentoFiscalViewModel.CriarDeArquivoXml(xmlPath), logoPath)
        {
        }

        public DocumentoFiscal(DocumentoFiscalViewModel viewModel, string logoPath = null)
        {
            ViewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            PDFFile = new PDF.File();
            AdicionarLogo(logoPath);
        }

        #endregion Public Constructors

        #region Public Properties

        public PDF.File PDFFile { get; private set; }
        public DocumentoFiscalViewModel ViewModel { get; set; }

        #endregion Public Properties

        #region Public Methods

        public static string GerarPDF(string xmlPath, string logoPath)
        {
            using (var d = new DocumentoFiscal(xmlPath, logoPath))
            {
                return d.Gerar(xmlPath + ".pdf");
            }
        }

        public void AdicionarLogo(string logoPath)
        {
            if (!string.IsNullOrWhiteSpace(logoPath) && System.IO.File.Exists(logoPath))
            {
                if (System.IO.Path.GetFileName(logoPath).EndsWith("pdf", StringComparison.InvariantCultureIgnoreCase))
                {
                    AdicionarLogoPdf(logoPath);
                }
                else
                {
                    AdicionarLogoImagem(logoPath);
                }
            }
        }

        public void AdicionarLogoImagem(System.IO.Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            var img = org.pdfclown.documents.contents.entities.Image.Get(stream);
            if (img == null) throw new InvalidOperationException("O logotipo não pode ser carregado, certifique-se que a imagem esteja no formato JPEG não progressivo.");
            _LogoObject = img.ToXObject(PdfDocument);
        }

        public void AdicionarLogoImagem(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException(nameof(path));

            using (var fs = new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                AdicionarLogoImagem(fs);
            }
        }

        public void AdicionarLogoPdf(System.IO.Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            using (var pdfFile = new PDF.File(new org.pdfclown.bytes.Stream(stream)))
            {
                _LogoObject = pdfFile.Document.Pages[0].ToXObject(PdfDocument);
            }
        }

        public void AdicionarLogoPdf(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException(nameof(path));

            using (var fs = new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                AdicionarLogoPdf(fs);
            }
        }

        public void CopiarParaStream(System.IO.Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            PDFFile.Save(new org.pdfclown.bytes.Stream(stream), PDF.SerializationModeEnum.Incremental);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose() =>
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);

        public string Gerar(string path)
        {
            if (!string.IsNullOrWhiteSpace(path))
                return Gerar(new System.IO.FileInfo(path))?.FullName;
            return null;
        }

        public System.IO.FileInfo Gerar(System.IO.FileInfo path = null)
        {
            foreach (var p in PDFFile.Document.Pages)
            {
                p.Delete();
            }

            // De acordo com o item 7.7, a fonte deve ser Times New Roman ou Courier New.
            _FonteFamilia = StandardType1Font.FamilyEnum.Times;
            _FonteRegular = new StandardType1Font(PdfDocument, _FonteFamilia, false, false);
            _FonteNegrito = new StandardType1Font(PdfDocument, _FonteFamilia, true, false);
            _FonteItalico = new StandardType1Font(PdfDocument, _FonteFamilia, false, true);

            EstiloPadrao = CriarEstilo();

            Paginas = new List<DanfePagina>();

            _Blocos = new List<BlocoBase>();

            Canhoto = CriarBloco<BlocoCanhoto>();
            IdentificacaoEmitente = AdicionarBloco<BlocoIdentificacaoEmitente>();

            IdentificacaoEmitente.Logo = _LogoObject;

            AdicionarBloco<BlocoDestinatarioRemetente>();

            if (ViewModel.TipoDocumento == TipoDocumento.DANFE)
            {
                if (ViewModel.LocalRetirada != null && ViewModel.ExibirBlocoLocalRetirada)
                    AdicionarBloco<BlocoLocalRetirada>();

                if (ViewModel.LocalEntrega != null && ViewModel.ExibirBlocoLocalEntrega)
                    AdicionarBloco<BlocoLocalEntrega>();

                if (ViewModel.Duplicatas.Count > 0)
                    AdicionarBloco<BlocoDuplicataFatura>();

                AdicionarBloco<BlocoCalculoImposto>(ViewModel.Orientacao == Orientacao.Paisagem ? EstiloPadrao : CriarEstilo(4.75F));
                AdicionarBloco<BlocoTransportador>();
                AdicionarBloco<BlocoDadosAdicionais>(CriarEstilo(tFonteCampoConteudo: 8));

                if (ViewModel.CalculoIssqn.Mostrar)
                    AdicionarBloco<BlocoCalculoIssqn>();

                var tabela = new TabelaProdutosServicos(ViewModel, EstiloPadrao);

                while (true)
                {
                    DanfePagina p = CriarPagina();

                    tabela.SetPosition(p.RetanguloCorpo.Location);
                    tabela.SetSize(p.RetanguloCorpo.Size);
                    tabela.Draw(p.Gfx);

                    p.Gfx.Stroke();
                    p.Gfx.Flush();

                    if (tabela.CompletamenteDesenhada) break;
                }
            }
            else if (ViewModel.TipoDocumento == TipoDocumento.CCE)
            {
                AdicionarBloco<BlocoCC>();
                CriarPagina();
            }

            AdicionarMetadata();

            PreencherNumeroFolhas();

            if (path != null)
            {
                PDFFile.Save(path.FullName, PDF.SerializationModeEnum.Incremental);
            }

            return path;
        }

        #endregion Public Methods

        // To detect redundant calls
        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Danfe() { // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        // Dispose(false); }

        // TODO: uncomment the following line if the finalizer is overridden above.// GC.SuppressFinalize(this);
    }
}