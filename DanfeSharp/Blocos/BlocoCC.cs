using DanfeSharp.Graphics;
using DanfeSharp.Modelo;

namespace DanfeSharp.Blocos
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
        private const string pre_cc = @"A Carta de Correção e disciplinada pelo parágrafo 1º-A do art. 7º do Convênio S/N, de 15 de dezembro de 1970 e pode ser utilizada para
regularização de erro ocorrido na emissão de documento fiscal, desde que o erro não esteja relacionado com:
I - As variáveis que determinam o valor do imposto tais como: base de cálculo, alíquota, diferença de preço, quantidade, valor da operação
ou da prestação;
II - A correção de dados cadastrais que implique mudançaa do remetente ou do destinatário;
III - A data de emissão ou de saída.";

        #endregion Public Fields

        #region Public Constructors

        public BlocoCCE(DocumentoFiscalViewModel viewModel, Estilo estilo) : base(viewModel, estilo)
        {

            _correcao = new CampoMultilinha("Correção", viewModel.TextoCorrecao, estilo);

            _Linha = new FlexibleLine() { Height = _correcao.Height }
            .ComElemento(_correcao)
            .ComLarguras(CorrecaoLarguraPorcentagem);



            var pretext = new FlexibleLine() { Height = 25 }
            .ComElemento(new CampoMultilinha("", pre_cc, estilo) { IsConteudoNegrito = true })
            .ComLarguras(CorrecaoLarguraPorcentagem);

            var postext = new FlexibleLine() { Height = 35 }
          .ComElemento(new CampoMultilinha("", "{outro texto}", estilo))
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