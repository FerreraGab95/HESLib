using System.Diagnostics;
using System.IO;

namespace HESDanfe.Test
{
    public static class TestUtils
    {
        public const string OutputDirectoryName = "Output";

        public static void SalvarTestPdf(this DANFE d, string outputName = null)
        {
            if (!Directory.Exists(OutputDirectoryName)) Directory.CreateDirectory(OutputDirectoryName);

            if (outputName == null) outputName = new StackTrace().GetFrame(1).GetMethod().Name;

            d.Gerar(Path.Combine(OutputDirectoryName, outputName + ".pdf"), Modelo.TipoDocumento.DANFE);
        }


    }
}
