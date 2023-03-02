using HES.Graphics;
using HES.Modelo;

namespace HES.Blocos
{
    internal class BlocoCondicaoCCE : BlocoBase
    {
        #region Private Fields


        private CampoMultilinha CampoCondicao;


        private FlexibleLine LinhaCondicao;


        #endregion Private Fields

        #region Public Fields





        #endregion Public Fields

        #region Public Constructors

        public BlocoCondicaoCCE(DANFEModel viewModel, Estilo estilo) : base(viewModel, estilo)
        {




            CampoCondicao = new CampoMultilinha("", viewModel.TextoCondicaoDeUso, estilo);

            LinhaCondicao = new FlexibleLine() { Height = CampoCondicao.Height }
            .ComElemento(CampoCondicao)
            .ComLarguras(100);




            MainVerticalStack.Add(LinhaCondicao);

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




            }
        }

        #endregion Public Properties

        #region Public Methods

        public override void Draw(Gfx gfx) => base.Draw(gfx);

        #endregion Public Methods
    }
}