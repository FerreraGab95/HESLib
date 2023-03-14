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