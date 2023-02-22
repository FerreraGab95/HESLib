using System;
using DanfeSharp.Esquemas.NFe;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DanfeSharp.Test
{
    [TestClass]
    public class DanfeTest
    {
        #region Public Methods

        [TestMethod]
        public void ComBlocoLocalEntrega()
        {
            var model = FabricaFake.DanfeViewModel_1();
            model.LocalEntrega = FabricaFake.LocalEntregaRetiradaFake();
            DocumentoFiscal d = new DocumentoFiscal(model);
            d.Gerar();
            d.SalvarTestPdf();
        }

        [TestMethod]
        public void ComBlocoLocalRetirada()
        {
            var model = FabricaFake.DanfeViewModel_1();
            model.LocalRetirada = FabricaFake.LocalEntregaRetiradaFake();
            DocumentoFiscal d = new DocumentoFiscal(model);
            d.Gerar();
            d.SalvarTestPdf();
        }

        [TestMethod]
        public void Contingencia_SVC_AN()
        {
            var model = FabricaFake.DanfeViewModel_1();
            model.TipoEmissao = FormaEmissao.ContingenciaSVCAN;
            model.ContingenciaDataHora = DateTime.Now;
            model.ContingenciaJustificativa = "Aqui vai o motivo da contingência";
            model.Orientacao = Orientacao.Retrato;
            DocumentoFiscal d = new DocumentoFiscal(model);
            d.Gerar();
            d.SalvarTestPdf();
        }

        [TestMethod]
        public void Contingencia_SVC_RS()
        {
            var model = FabricaFake.DanfeViewModel_1();
            model.TipoEmissao = FormaEmissao.ContingenciaSVCRS;
            model.ContingenciaDataHora = DateTime.Now;
            model.ContingenciaJustificativa = "Aqui vai o motivo da contingência";
            model.Orientacao = Orientacao.Retrato;
            DocumentoFiscal d = new DocumentoFiscal(model);
            d.Gerar();
            d.SalvarTestPdf();
        }

        [TestMethod]
        public void OpcaoPreferirEmitenteNomeFantasia_False()
        {
            var model = FabricaFake.DanfeViewModel_1();
            model.Orientacao = Orientacao.Retrato;
            model.PreferirEmitenteNomeFantasia = false;
            DocumentoFiscal d = new DocumentoFiscal(model);
            d.Gerar();
            d.SalvarTestPdf();
        }

        [TestMethod]
        public void Paisagem()
        {
            var model = FabricaFake.DanfeViewModel_1();
            model.Orientacao = Orientacao.Paisagem;
            DocumentoFiscal d = new DocumentoFiscal(model);
            d.Gerar();
            d.SalvarTestPdf();
        }

        [TestMethod]
        public void Paisagem_2Canhotos()
        {
            var model = FabricaFake.DanfeViewModel_1();
            model.Orientacao = Orientacao.Paisagem;
            model.QuantidadeCanhotos = 2;
            DocumentoFiscal d = new DocumentoFiscal(model);
            d.Gerar();
            d.SalvarTestPdf();
        }

        [TestMethod]
        public void Paisagem_SemCanhoto()
        {
            var model = FabricaFake.DanfeViewModel_1();
            model.Orientacao = Orientacao.Paisagem;
            model.QuantidadeCanhotos = 0;
            DocumentoFiscal d = new DocumentoFiscal(model);
            d.Gerar();
            d.SalvarTestPdf();
        }

        [TestMethod]
        public void PaisagemHomologacao()
        {
            var model = FabricaFake.DanfeViewModel_1();
            model.TipoAmbiente = TAmb.Homologacao;
            model.Orientacao = Orientacao.Paisagem;
            DocumentoFiscal d = new DocumentoFiscal(model);
            d.Gerar();
            d.SalvarTestPdf();
        }

        [TestMethod]
        public void PaisagemSemIcmsInterestadual()
        {
            var model = FabricaFake.DanfeViewModel_1();
            model.Orientacao = Orientacao.Paisagem;
            model.ExibirIcmsInterestadual = false;
            DocumentoFiscal d = new DocumentoFiscal(model);
            d.Gerar();
            d.SalvarTestPdf();
        }

        [TestMethod]
        public void Retrato()
        {
            var model = FabricaFake.DanfeViewModel_1();
            model.Orientacao = Orientacao.Retrato;
            DocumentoFiscal d = new DocumentoFiscal(model);
            d.Gerar();
            d.SalvarTestPdf();
        }

        [TestMethod]
        public void Retrato_2Canhotos()
        {
            var model = FabricaFake.DanfeViewModel_1();
            model.Orientacao = Orientacao.Retrato;
            model.QuantidadeCanhotos = 2;
            DocumentoFiscal d = new DocumentoFiscal(model);
            d.Gerar();
            d.SalvarTestPdf();
        }

        [TestMethod]
        public void Retrato_SemCanhoto()
        {
            var model = FabricaFake.DanfeViewModel_1();
            model.Orientacao = Orientacao.Retrato;
            model.QuantidadeCanhotos = 0;
            DocumentoFiscal d = new DocumentoFiscal(model);
            d.Gerar();
            d.SalvarTestPdf();
        }

        [TestMethod]
        public void RetratoHomologacao()
        {
            var model = FabricaFake.DanfeViewModel_1();
            model.TipoAmbiente = TAmb.Homologacao;
            model.Orientacao = Orientacao.Retrato;
            DocumentoFiscal d = new DocumentoFiscal(model);
            d.Gerar();
            d.SalvarTestPdf();
        }

        [TestMethod]
        public void RetratoSemIcmsInterestadual()
        {
            var model = FabricaFake.DanfeViewModel_1();
            model.Orientacao = Orientacao.Retrato;
            model.ExibirIcmsInterestadual = false;
            DocumentoFiscal d = new DocumentoFiscal(model);
            d.Gerar();
            d.SalvarTestPdf();
        }

        #endregion Public Methods
    }
}