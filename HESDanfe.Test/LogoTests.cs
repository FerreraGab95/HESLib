﻿using System;
using System.IO;
using System.Runtime.CompilerServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HESDanfe.Test
{
    [TestClass]
    public class LogoTests
    {
        public static readonly string OutputDirectoryName = Path.Combine("Output", "ComLogo");

        static LogoTests()
        {
            if (!Directory.Exists(OutputDirectoryName)) Directory.CreateDirectory(OutputDirectoryName);
        }

        public void TestLogo(string logoPath, [CallerMemberName] string pdfName = null)
        {
            var model = FabricaFake.DanfeViewModel_1();
            model.Orientacao = Orientacao.Retrato;
            using (DANFE d = new DANFE(model))
            {
                if (logoPath.EndsWith(".pdf", StringComparison.InvariantCultureIgnoreCase))
                {
                    d.AdicionarLogoPdf(logoPath);
                    model.Emitente.NomeFantasia = "Logo Vetor Ltda.";
                }
                else
                {
                    d.AdicionarLogoImagem(logoPath);
                    model.Emitente.NomeFantasia = "Logo Raster Ltda.";
                }


                d.Gerar(Path.Combine(OutputDirectoryName, pdfName + ".pdf"), Modelo.TipoDocumento.DANFE);
            }
        }


        [TestMethod]
        public void LogoQuadradoJPG() => TestLogo("Logos/JPG/Quadrado.jpg");

        [TestMethod]
        public void LogoHorizontalJPG() => TestLogo("Logos/JPG/Horizontal.jpg");

        [TestMethod]
        public void LogoVerticalJPG() => TestLogo("Logos/JPG/Vertical.jpg");

        [TestMethod]
        public void LogoQuadradoPDF() => TestLogo("Logos/PDF/Quadrado.pdf");

        [TestMethod]
        public void LogoHorizontalPDF() => TestLogo("Logos/PDF/Horizontal.pdf");

        [TestMethod]
        public void LogoVerticalPDF() => TestLogo("Logos/PDF/Vertical.pdf");

    }
}
