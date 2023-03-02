using System.Diagnostics;
using System.IO;
using HES.Modelo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HES.Test
{
    [TestClass]
    public class DanfeXmlTests
    {
        #region Public Fields

        public readonly string InputXmlDirectoryPrefix = Path.Combine("Xml", "NFe");
        public readonly string OutputDirectory = Path.Combine("Output", "DeXml");

        #endregion Public Fields

        #region Public Constructors

        public DanfeXmlTests()
        {
            if (!Directory.Exists(OutputDirectory))
                Directory.CreateDirectory(OutputDirectory);
        }

        #endregion Public Constructors

        #region Public Methods

        [TestMethod]
        public void Teste()
        {
            DANFE.LicenseKey = "147A-6E0A-D208-792D-47A7-FCB1-A8B1";

            var nfe = @"C:\svn\EnviaSADWeb\Envia\Uploads\XmlDistribuicao\35210701818337000173550010000618721000618732-procNFe.xml";
            var cce = @"C:/svn/EnviaSADWeb/Envia/Uploads/XmlDistribuicaoEventos/35230204667427000107550000234567891234567909_110110_01-proceventonfe.xml";
            var logo = @"C:\Users\H&S\Pictures\logo_envia_large.jpg";

            foreach (var pdf in DANFE.GerarPDF(nfe, cce, logo, new DirectoryInfo(@"C:\Teste\testeDANFE")))
            {
                Debug.WriteLine(pdf, "PDF Gerado");
            }
        }

        [TestMethod]

        public void TestarUnico()
        {
            DANFE.LicenseKey = "147A-6E0A-D208-792D-47A7-FCB1-A8B1";



            var nfe = @"C:\svn\EnviaSADWeb\Envia\Uploads\XmlDistribuicao\35210701818337000173550010000618721000618732-procNFe.xml";
            var cce = @"C:/svn/EnviaSADWeb/Envia/Uploads/XmlDistribuicaoEventos/35230204667427000107550000234567891234567909_110110_01-proceventonfe.xml";
            var logo = @"C:\Users\H&S\Pictures\logo_envia_large.jpg";

            var d = new DANFE(nfe, cce, logo);
            d.ViewModel.TextoCorrecao = Extensions.Util.RandomIpsum(100);
            d.GerarUnico();
        }

        public void TestXml(string xmlPath, string ccePath = null)
        {
            var outPdfFilePath = Path.Combine(OutputDirectory, Path.GetFileNameWithoutExtension(xmlPath) + ".pdf");
            if (ccePath != null)
            {
                ccePath = Path.Combine(InputXmlDirectoryPrefix, ccePath);
            }
            var model = DANFEModel.CriarDeArquivoXml(Path.Combine(InputXmlDirectoryPrefix, xmlPath), ccePath);
            DANFE danfe = new DANFE(model);
            danfe.Gerar(outPdfFilePath, TipoDocumento.DANFE);

        }

        [TestMethod]
        public void v1() => TestXml("v1.00/v1.xml");

        [TestMethod]
        public void v2_Retrato() => TestXml("v2.00/v2_Retrato.xml");

        [TestMethod]
        public void v3_10_Retrato() => TestXml("v3.10/v3.10_Retrato.xml");

        [TestMethod]
        public void v4_ComLocalEntrega() => TestXml("v4.00/v4_ComLocalEntrega.xml");

        [TestMethod]
        public void v4_ComLocalRetirada() => TestXml("v4.00/v4_ComLocalRetirada.xml");

        #endregion Public Methods
    }
}