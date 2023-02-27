using System;
using HESDanfe.Graphics;
using HESDanfe.Modelo;

namespace HESDanfe.Blocos
{
    internal class BlocoCCE : BlocoBase
    {
        #region Private Fields

        private CampoMultilinha _correcao;
        private CampoMultilinha _condicao;
        private FlexibleLine _linha_correcao;
        private FlexibleLine _linha_condicao;
        private FlexibleLine _linha_fisco;

        #endregion Private Fields

        #region Public Fields





        #endregion Public Fields

        #region Public Constructors

        public BlocoCCE(DANFEViewModel viewModel, Estilo estilo) : base(viewModel, estilo)
        {


            _correcao = new CampoMultilinha("Correção", viewModel.TextoCorrecao, estilo);

            _linha_correcao = new FlexibleLine() { Height = _correcao.Height }
            .ComElemento(_correcao)
            .ComLarguras(100);

            _condicao = new CampoMultilinha("", viewModel.TextoCondicaoDeUso, estilo);

            _linha_condicao = new FlexibleLine() { Height = _condicao.Height }
            .ComElemento(_condicao)
            .ComLarguras(100);

            _linha_fisco = new FlexibleLine() { Height = 35 }
            .ComElemento(new CampoMultilinha("", $"PROTOCOLO: {viewModel.ProtocoloCorrecao}{Environment.NewLine}DATA/HORA: {viewModel.DataHoraCorrecao:dd/MM/yyyy HH:mm:ss}{Environment.NewLine}EVENTO: {viewModel.SequenciaCorrecao}", estilo))
            .ComElemento(new CampoMultilinha("Reservado ao Fisco", ViewModel.TextoAdicionalFisco, estilo))
            .ComLarguras(75, 25);

            MainVerticalStack.Add(_linha_condicao, _linha_correcao, _linha_fisco);

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
                if (_correcao != null && _linha_correcao != null)
                {
                    _linha_correcao.Width = value;
                    _linha_correcao.Posicionar();
                    _correcao.Height = 50;
                    _linha_correcao.Height = _correcao.Height;
                }

                if (_condicao != null && _linha_condicao != null)
                {
                    _linha_condicao.Width = value;
                    _linha_condicao.Posicionar();
                    _condicao.Height = 30;
                    _linha_condicao.Height = _condicao.Height;

                }

                if (_linha_fisco != null)
                {
                    _linha_fisco.Posicionar();
                    _linha_fisco.Height = 100 - _condicao.Height + _correcao.Height;
                }
            }
        }

        #endregion Public Properties

        #region Public Methods

        public override void Draw(Gfx gfx) => base.Draw(gfx);

        #endregion Public Methods
    }
}