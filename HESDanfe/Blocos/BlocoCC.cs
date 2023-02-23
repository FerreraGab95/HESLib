using System;
using HESDanfe.Graphics;
using HESDanfe.Modelo;

namespace HESDanfe.Blocos
{
    internal class BlocoCCE : BlocoBase
    {
        #region Private Fields

        private CampoMultilinha _correcao;
        private FlexibleLine _Linha;

        #endregion Private Fields

        #region Public Fields

        public const float AlturaMinima = 100;
        public const float CorrecaoLarguraPorcentagem = 100;


        #endregion Public Fields

        #region Public Constructors

        public BlocoCCE(DANFEViewModel viewModel, Estilo estilo) : base(viewModel, estilo)
        {

            var pretext = new FlexibleLine() { Height = 25 }
            .ComElemento(new CampoMultilinha("", viewModel.TextoCondicaoDeUso, estilo) { IsConteudoNegrito = true })
            .ComLarguras(CorrecaoLarguraPorcentagem);

            _correcao = new CampoMultilinha("Correção", viewModel.TextoCorrecao, estilo);

            _Linha = new FlexibleLine() { Height = _correcao.Height }
            .ComElemento(_correcao)
            .ComLarguras(CorrecaoLarguraPorcentagem);

            var postext = new FlexibleLine() { Height = 35 }
            .ComElemento(new CampoMultilinha("", $"PROTOCOLO: {viewModel.ProtocoloCorrecao}{Environment.NewLine}DATA/HORA: {viewModel.DataHoraCorrecao:dd/MM/yyyy HH:mm:ss}{Environment.NewLine}EVENTO: {viewModel.SequenciaCorrecao}", estilo))
            .ComElemento(new CampoMultilinha("Reservado ao Fisco", ViewModel.TextoAdicionalFisco, estilo))
            .ComLarguras(75, 25);

            MainVerticalStack.Add(pretext, _Linha, postext);

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
                if (_correcao != null && _Linha != null)
                {
                    _Linha.Width = value;
                    _Linha.Posicionar();
                    _correcao.Height = AlturaMinima;
                    _Linha.Height = _correcao.Height;
                }
            }
        }

        #endregion Public Properties

        #region Public Methods

        public override void Draw(Gfx gfx) => base.Draw(gfx);

        #endregion Public Methods
    }
}