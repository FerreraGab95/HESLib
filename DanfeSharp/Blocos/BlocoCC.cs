using DanfeSharp.Graphics;
using DanfeSharp.Modelo;

namespace DanfeSharp.Blocos
{
    internal class BlocoCC : BlocoBase
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

        public BlocoCC(DocumentoFiscalViewModel viewModel, Estilo estilo) : base(viewModel, estilo)
        {

            _correcao = new CampoMultilinha("Correção", viewModel.TextoCorrecao, estilo);

            _Linha = new FlexibleLine() { Height = _correcao.Height }
            .ComElemento(_correcao)
            .ComLarguras(CorrecaoLarguraPorcentagem)
             ;

            MainVerticalStack.Add(_Linha);
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