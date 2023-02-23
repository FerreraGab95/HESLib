using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using HESDanfe.Blocos;
using HESDanfe.Modelo;
using org.pdfclown.documents;
using org.pdfclown.documents.contents.fonts;
using PDF = org.pdfclown.files;

namespace HESDanfe
{
    public class DANFE : IDisposable
    {
        #region Private Methods

        private DanfePagina CriarPagina()
        {
            DanfePagina p = new DanfePagina(this);
            Paginas.Add(p);
            p.DesenharBlocos(Paginas.Count == 1);

            if (LicenseKey != Utils.GenerateLicenseKey("hes.com.br"))
                p.DesenharCreditos(null);

            // Ambiente de homologação
            // 7. O DANFE emitido para representar NF-e cujo uso foi autorizado em ambiente de
            // homologação sempre deverá conter a frase “SEM VALOR FISCAL” no quadro “Informações
            // Complementares” ou em marca d’água destacada.
            if (ViewModel.TipoAmbiente == Esquemas.NFe.TAmb.Homologacao)
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
                GC.Collect();
                disposedValue = true;
            }
        }

        #endregion Protected Methods

        #region Internal Fields

        internal List<BlocoBase> _Blocos;
        internal StandardType1Font.FamilyEnum _FonteFamilia;
        internal org.pdfclown.documents.contents.xObjects.XObject _LogoObject = null;
        internal bool disposedValue = false;

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

        #region Public Fields

        public static StandardType1Font FonteItalico;
        public static StandardType1Font FonteNegrito;
        public static StandardType1Font FonteRegular;

        #endregion Public Fields

        #region Public Constructors

        public DANFE(string xmlNFePath, string xmlCCePath, string logoPath) : this(DANFEViewModel.CriarDeArquivoXml(xmlNFePath, xmlCCePath), logoPath)
        {
        }

        public DANFE(string xmlNFePath, string logoPath) : this(DANFEViewModel.CriarDeArquivoXml(xmlNFePath, null), logoPath)
        {
        }

        public DANFE(string xmlNFePath) : this(DANFEViewModel.CriarDeArquivoXml(xmlNFePath, null), null)
        {
        }

        public DANFE(DANFEViewModel viewModel) : this(viewModel, null)
        {
        }

        public DANFE(DANFEViewModel viewModel, string logoPath)
        {
            ViewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            PDFFile = new PDF.File();
            AdicionarLogo(logoPath);
        }

        #endregion Public Constructors

        #region Public Properties

        public string Autor { get; set; } = Assembly.GetEntryAssembly()?.GetName()?.Name ?? "H&S";

        public static string LicenseKey
        {
            get; set;
        }

        public PDF.File PDFFile { get; private set; }
        public DANFEViewModel ViewModel { get; set; }

        #endregion Public Properties

        #region Public Methods

        public static IEnumerable<FileInfo> GerarPDF(string PathNFe, string PathCCe, DirectoryInfo outputPath) => GerarPDF(PathNFe, PathCCe, null, outputPath);

        public static FileInfo GerarPDF(string PathNFe, DirectoryInfo outputPath) => GerarPDF(PathNFe, null, null, outputPath).FirstOrDefault();

        public static IEnumerable<FileInfo> GerarPDF(string PathNFe, string PathCCe, string PathLogo, DirectoryInfo outputPath)
        {
            using (var d = new DANFE(PathNFe, PathCCe, PathLogo))
            {
                return d.Gerar(outputPath);
            }
        }

        public void AdicionarLogo(string logoPath)
        {
            if (!string.IsNullOrWhiteSpace(logoPath) && File.Exists(logoPath))
            {
                if (Path.GetFileName(logoPath).EndsWith("pdf", StringComparison.InvariantCultureIgnoreCase))
                {
                    AdicionarLogoPdf(logoPath);
                }
                else
                {
                    AdicionarLogoImagem(logoPath);
                }
            }
        }

        public void AdicionarLogoImagem(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            var img = org.pdfclown.documents.contents.entities.Image.Get(stream) ?? throw new InvalidOperationException("O logotipo não pode ser carregado, certifique-se que a imagem esteja no formato JPEG não progressivo.");
            _LogoObject = img.ToXObject(PdfDocument);
        }

        public void AdicionarLogoImagem(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException(nameof(path));

            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                AdicionarLogoImagem(fs);
            }
        }

        public void AdicionarLogoPdf(Stream stream)
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

            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                AdicionarLogoPdf(fs);
            }
        }

        public void CopiarParaStream(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            PDFFile.Save(new org.pdfclown.bytes.Stream(stream), PDF.SerializationModeEnum.Incremental);
        }

        public void Dispose() => Dispose(true);

        public IEnumerable<FileInfo> Gerar(DirectoryInfo path)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));

            if (path.Exists == false)
            {
                path.Create();
            }

            var nfe = Path.Combine(path.FullName, $"DANFE-{ViewModel.ChaveAcesso}.pdf");

            yield return Gerar(nfe, TipoDocumento.DANFE);

            if (!string.IsNullOrWhiteSpace(ViewModel.TextoCorrecao) && ViewModel.SequenciaCorrecao > 0)
            {
                var cce = Path.Combine(path.FullName, $"DANFE-{ViewModel.ChaveAcesso}-CCE-{ViewModel.SequenciaCorrecao}.pdf");
                yield return Gerar(cce, TipoDocumento.CCE);
            }
        }

        public FileInfo Gerar(string path, TipoDocumento TipoDocumento) => Gerar(new FileInfo(path), TipoDocumento);

        public FileInfo Gerar(FileInfo path, TipoDocumento TipoDocumento)
        {
            foreach (var p in PDFFile.Document.Pages)
            {
                p.Delete();
            }

            // De acordo com o item 7.7, a fonte deve ser Times New Roman ou Courier New.
            _FonteFamilia = StandardType1Font.FamilyEnum.Times;
            FonteRegular = new StandardType1Font(PdfDocument, _FonteFamilia, false, false);
            FonteNegrito = new StandardType1Font(PdfDocument, _FonteFamilia, true, false);
            FonteItalico = new StandardType1Font(PdfDocument, _FonteFamilia, false, true);

            EstiloPadrao = Extensions.CriarEstilo();

            Paginas = new List<DanfePagina>();

            _Blocos = new List<BlocoBase>();

            Canhoto = CriarBloco<BlocoCanhoto>();
            IdentificacaoEmitente = AdicionarBloco<BlocoIdentificacaoEmitente>();

            IdentificacaoEmitente.Logo = _LogoObject;

            AdicionarBloco<BlocoDestinatarioRemetente>();

            if (TipoDocumento == TipoDocumento.DANFE)
            {
                if (ViewModel.LocalRetirada != null && ViewModel.ExibirBlocoLocalRetirada)
                    AdicionarBloco<BlocoLocalRetirada>();

                if (ViewModel.LocalEntrega != null && ViewModel.ExibirBlocoLocalEntrega)
                    AdicionarBloco<BlocoLocalEntrega>();

                if (ViewModel.Duplicatas.Count > 0)
                    AdicionarBloco<BlocoDuplicataFatura>();

                AdicionarBloco<BlocoCalculoImposto>(ViewModel.Orientacao == Orientacao.Paisagem ? EstiloPadrao : Extensions.CriarEstilo(4.75F));
                AdicionarBloco<BlocoTransportador>();
                AdicionarBloco<BlocoDadosAdicionais>(Extensions.CriarEstilo(tFonteCampoConteudo: 8));

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
            else if (TipoDocumento == TipoDocumento.CCE)
            {
                AdicionarBloco<BlocoCCE>();
                CriarPagina();
            }

            //metadata do PDF
            var ass = Assembly.GetEntryAssembly()?.GetName() ?? Assembly.GetExecutingAssembly()?.GetName();
            var info = PdfDocument.Information;
            info[new org.pdfclown.objects.PdfName("ChaveAcesso")] = ViewModel.ChaveAcesso;
            info[new org.pdfclown.objects.PdfName("TipoDocumento")] = $"{TipoDocumento}";
            info.CreationDate = ViewModel.DataHoraEmissao ?? DateTime.Now;
            info.Creator = $"{ass?.Name} {ass?.Version} - Disponível em https://github.com/zonaro/HESDanfe";
            info.Author = Autor;

            info.Subject = TipoDocumento == TipoDocumento.DANFE ? "Documento Auxiliar da NFe" : "Carta de Correção Eletrônica";
            info.Title = $"{TipoDocumento}";

            PreencherNumeroFolhas();

            PDFFile.Save(path.FullName, PDF.SerializationModeEnum.Incremental);

            return path;
        }

        #endregion Public Methods
    }



}