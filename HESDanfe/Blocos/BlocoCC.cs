using System;
using System.Linq;
using HESDanfe.Graphics;
using HESDanfe.Modelo;

namespace HESDanfe.Blocos
{
    internal class BlocoCCE : BlocoBase
    {
        #region Private Fields

        private CampoMultilinha CampoCorrecao;
        private CampoMultilinha CampoCondicao;
        private CampoMultilinha CampoObservacoes;
        private CampoMultilinha CampoFisco;
        private FlexibleLine LinhaCorrecao;
        private FlexibleLine LinhaCondicao;
        private FlexibleLine LinhaRodape;

        #endregion Private Fields

        #region Public Fields





        #endregion Public Fields

        #region Public Constructors

        public BlocoCCE(DANFEViewModel viewModel, Estilo estilo) : base(viewModel, estilo)
        {




            CampoCondicao = new CampoMultilinha("", viewModel.TextoCondicaoDeUso, estilo);

            LinhaCondicao = new FlexibleLine() { Height = CampoCondicao.Height }
            .ComElemento(CampoCondicao)
            .ComLarguras(100);


            CampoObservacoes = new CampoMultilinha("", $"PROTOCOLO: {viewModel.ProtocoloCorrecao}{Environment.NewLine}DATA/HORA: {viewModel.DataHoraCorrecao:dd/MM/yyyy HH:mm:ss}{Environment.NewLine}EVENTO: {viewModel.SequenciaCorrecao}", estilo);
            CampoFisco = new CampoMultilinha("Reservado ao Fisco", ViewModel.TextoAdicionalFisco, estilo);
            LinhaRodape = new FlexibleLine() { Height = new[] { CampoFisco.Height, CampoObservacoes.Height }.Max() }
            .ComElemento(CampoObservacoes)
            .ComElemento(CampoFisco)
            .ComLarguras(60, 40);

            CampoCorrecao = new CampoMultilinha("Correção", viewModel.TextoCorrecao, estilo);

            LinhaCorrecao = new FlexibleLine() { Height = CampoCorrecao.Height }
            .ComElemento(CampoCorrecao)
            .ComLarguras(100);


            MainVerticalStack.Add(LinhaCondicao, LinhaCorrecao, LinhaRodape);

        }

        #endregion Public Constructors

        #region Public Properties

        public override string Cabecalho => "";

        public override PosicaoBloco Posicao => PosicaoBloco.Topo;

        public override float Width
        {
            get => base.Width;
            set
            {
                base.Width = value;
                // Força o ajuste da altura do InfComplementares

                if (CampoCondicao != null && LinhaCondicao != null)
                {
                    LinhaCondicao.Width = value;
                    LinhaCondicao.Posicionar();
                    CampoCondicao.Height = 20;
                    LinhaCondicao.Height = CampoCondicao.Height;
                }

                if (LinhaRodape != null)
                {
                    LinhaRodape.Width = value;
                    LinhaRodape.Posicionar();
                    CampoObservacoes.Height = 25;
                    LinhaRodape.Height = new[] { CampoFisco.Height, CampoObservacoes.Height }.Max();
                }

                if (CampoCorrecao != null && LinhaCorrecao != null)
                {
                    LinhaCorrecao.Width = value;
                    LinhaCorrecao.Posicionar();
                    CampoCorrecao.Height = 100 - (LinhaCondicao.Height + LinhaRodape.Height);
                    LinhaCorrecao.Height = CampoCorrecao.Height;
                }
            }
        }

        #endregion Public Properties

        #region Public Methods

        public override void Draw(Gfx gfx) => base.Draw(gfx);

        #endregion Public Methods
    }
}