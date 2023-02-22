using System.IO;
using HESDanfe.Modelo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HESDanfe.Test
{
    [TestClass]
    public class DanfeXmlTests
    {
        public readonly string OutputDirectory = Path.Combine("Output", "DeXml");
        public readonly string InputXmlDirectoryPrefix = Path.Combine("Xml", "NFe");

        public DanfeXmlTests()
        {
            if (!Directory.Exists(OutputDirectory))
                Directory.CreateDirectory(OutputDirectory);
        }

        public void TestXml(string xmlPath, string ccePath = null)
        {
            var outPdfFilePath = Path.Combine(OutputDirectory, Path.GetFileNameWithoutExtension(xmlPath) + ".pdf");
            var model = DocumentoFiscalViewModel.CriarDeArquivoXml(Path.Combine(InputXmlDirectoryPrefix, xmlPath), Path.Combine(InputXmlDirectoryPrefix, ccePath));
            using (DANFE danfe = new DANFE(model))
            {
                danfe.Gerar(new FileInfo(outPdfFilePath), TipoDocumento.DANFE);

            }
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
    }
}
