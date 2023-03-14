﻿using System.Drawing;
using Extensions;
using Extensions.Locations;
using HES.Documents.Contents.xObjects;
using HES.Graphics;
using HES.Modelo;

namespace HES
{
    internal class IdentificacaoEmitente : ElementoBase
    {
        public DANFEModel ViewModel { get; private set; }
        public XObject Logo { get; set; }

        public IdentificacaoEmitente(Estilo estilo, DANFEModel viewModel) : base(estilo)
        {
            ViewModel = viewModel;
            Logo = null;
        }

        public override void Draw(Gfx gfx)
        {
            base.Draw(gfx);

            // 7.7.6 Conteúdo do Quadro Dados do Emitente
            // Deverá estar impresso em negrito.A razão social e/ ou nome fantasia deverá ter tamanho
            // mínimo de doze(12) pontos, ou 17 CPP e os demais dados do emitente, endereço,
            // município, CEP, fone / fax deverão ter tamanho mínimo de oito(8) pontos, ou 17 CPP.

            var rp = BoundingBox.InflatedRetangle(0.75F);
            float alturaMaximaLogoHorizontal = 14F;

            Fonte f2 = Estilo.CriarFonteNegrito(12);
            Fonte f3 = Estilo.CriarFonteRegular(8);

            if (Logo == null)
            {
                var f1 = Estilo.CriarFonteRegular(6);
                gfx.DrawString("IDENTIFICAÇÃO DO EMITENTE", rp, f1, AlinhamentoHorizontal.Centro);
                rp = rp.CutTop(f1.AlturaLinha);
            }
            else
            {
                RectangleF rLogo;

                // Logo Horizontal
                if (Logo.Size.Width > Logo.Size.Height)
                {
                    rLogo = new RectangleF(rp.X, rp.Y, rp.Width, alturaMaximaLogoHorizontal);
                    rp = rp.CutTop(alturaMaximaLogoHorizontal);
                }
                // Logo Vertical / Quadrado
                else
                {
                    float lw = rp.Height * Logo.Size.Width / Logo.Size.Height;
                    rLogo = new RectangleF(rp.X, rp.Y, lw, rp.Height);
                    rp = rp.CutLeft(lw);
                }

                gfx.ShowXObject(Logo, rLogo);

            }

            var emitente = ViewModel.Emitente;

            string nome = emitente.RazaoSocial;

            if (ViewModel.PreferirEmitenteNomeFantasia)
            {
                nome = emitente.NomeFantasia.IfBlank(emitente.RazaoSocial);
            }

            var ts = new TextStack(rp) { LineHeightScale = 1 }
                .AddLine(nome, f2)
                .AddLine(emitente.FullBuildingInfo, f3)
                .AddLine(emitente.ToString(AddressPart.Neighborhood, AddressPart.City), f3)
                .AddLine(emitente.ToString(AddressPart.StateCode, AddressPart.PostalCode), f3);

            ts.AlinhamentoHorizontal = AlinhamentoHorizontal.Centro;
            ts.AlinhamentoVertical = AlinhamentoVertical.Centro;
            ts.Draw(gfx);


        }
    }
}
