﻿using System;
using Extensions;
using HES.Elementos;
using HES.Graphics;
using HES.Modelo;


namespace HES.Blocos
{
    /// <summary>
    /// Define um bloco básico do DANFE.
    /// </summary>
    internal abstract class BlocoBase : ElementoBase
    {
        /// <summary>
        /// Constante de proporção dos campos para o formato retrato A4, porcentagem dividida pela largura desenhável.
        /// </summary>
        public const float Proporcao = 100F / 200F;

        public DANFEModel ViewModel { get; private set; }

        public abstract PosicaoBloco Posicao { get; }

        /// <summary>
        /// Pilha principal.
        /// </summary>
        public VerticalStack MainVerticalStack { get; private set; }

        /// <summary>
        /// Quando verdadeiro, o bloco é mostrado apenas na primeira página, caso contário é mostrado em todas elas.
        /// </summary>
        public virtual bool VisivelSomentePrimeiraPagina => true;

        public virtual string Cabecalho => null;

        public BlocoBase(DANFEModel viewModel, Estilo estilo) : base(estilo)
        {
            MainVerticalStack = new VerticalStack();
            ViewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));

            if (Cabecalho.IsNotBlank())
            {
                MainVerticalStack.Add(new CabecalhoBloco(estilo, Cabecalho));
            }
        }

        public LinhaCampos AdicionarLinhaCampos()
        {
            var l = new LinhaCampos(Estilo, Width)
            {
                Width = Width,
                Height = Extensions.Util.CampoAltura
            };
            MainVerticalStack.Add(l);
            return l;
        }

        public override void Draw(Gfx gfx)
        {
            base.Draw(gfx);
            MainVerticalStack.SetPosition(X, Y);
            MainVerticalStack.Width = Width;
            MainVerticalStack.Draw(gfx);
        }

        public override float Height { get => MainVerticalStack.Height; set => throw new NotSupportedException(); }
        public override bool PossuiContorno => false;
    }
}
