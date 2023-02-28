using System;
using System.Drawing;
using System.Linq;
using HESDanfe.Blocos;
using HESDanfe.Documents;
using HESDanfe.Documents.Contents.Composition;
using HESDanfe.Files;
using HESDanfe.Graphics;

namespace HESDanfe
{
    internal class DanfePagina
    {
        public DANFE Danfe { get; private set; }
        public Page PdfPage { get; private set; }
        public PrimitiveComposer PrimitiveComposer { get; private set; }
        public Gfx Gfx { get; private set; }
        public RectangleF RetanguloNumeroFolhas { get; set; }
        public RectangleF RetanguloCorpo { get; private set; }
        public RectangleF RetanguloDesenhavel { get; private set; }
        public RectangleF RetanguloCreditos { get; private set; }
        public RectangleF Retangulo { get; private set; }

        public DanfePagina(DANFE danfe, PdfFile pdf)
        {
            danfe = danfe ?? throw new ArgumentNullException(nameof(pdf));
            pdf = pdf ?? throw new ArgumentNullException(nameof(danfe));
            this.Danfe = danfe;
            PdfPage = new Page(pdf.Document);
            pdf.Document.Pages.Add(PdfPage);

            PrimitiveComposer = new PrimitiveComposer(PdfPage);
            Gfx = new Gfx(PrimitiveComposer);

            if (danfe.ViewModel.Orientacao == Orientacao.Retrato)
                Retangulo = new RectangleF(0, 0, Utils.A4Largura, Utils.A4Altura);
            else
                Retangulo = new RectangleF(0, 0, Utils.A4Altura, Utils.A4Largura);

            RetanguloDesenhavel = Retangulo.InflatedRetangle(danfe.ViewModel.Margem);
            RetanguloCreditos = new RectangleF(RetanguloDesenhavel.X, RetanguloDesenhavel.Bottom + danfe.EstiloPadrao.PaddingSuperior, RetanguloDesenhavel.Width, Retangulo.Height - RetanguloDesenhavel.Height - danfe.EstiloPadrao.PaddingSuperior);
            PdfPage.Size = new SizeF(Retangulo.Width.ToPoint(), Retangulo.Height.ToPoint());
        }

        public void DesenharCreditos(string info) => Gfx.DrawString(info, RetanguloCreditos, Danfe.EstiloPadrao.CriarFonteItalico(6), AlinhamentoHorizontal.Direita);

        private void DesenharCanhoto()
        {
            if (Danfe.ViewModel.QuantidadeCanhotos == 0) return;

            var canhoto = Danfe.Canhoto;
            canhoto.SetPosition(RetanguloDesenhavel.Location);

            if (Danfe.ViewModel.Orientacao == Orientacao.Retrato)
            {
                canhoto.Width = RetanguloDesenhavel.Width;

                for (int i = 0; i < Danfe.ViewModel.QuantidadeCanhotos; i++)
                {
                    canhoto.Draw(Gfx);
                    canhoto.Y += canhoto.Height;
                }

                RetanguloDesenhavel = RetanguloDesenhavel.CutTop(canhoto.Height * Danfe.ViewModel.QuantidadeCanhotos);
            }
            else
            {
                canhoto.Width = RetanguloDesenhavel.Height;
                Gfx.PrimitiveComposer.BeginLocalState();
                Gfx.PrimitiveComposer.Rotate(90, new PointF(0, canhoto.Width + canhoto.X + canhoto.Y).ToPointMeasure());

                for (int i = 0; i < Danfe.ViewModel.QuantidadeCanhotos; i++)
                {
                    canhoto.Draw(Gfx);
                    canhoto.Y += canhoto.Height;
                }

                Gfx.PrimitiveComposer.End();
                RetanguloDesenhavel = RetanguloDesenhavel.CutLeft(canhoto.Height * Danfe.ViewModel.QuantidadeCanhotos);

            }
        }

        public void DesenhaNumeroPaginas(int n, int total)
        {
            if (n <= 0) throw new ArgumentOutOfRangeException(nameof(n));
            if (total <= 0) throw new ArgumentOutOfRangeException(nameof(n));
            if (n > total) throw new ArgumentOutOfRangeException("O número da página atual deve ser menor que o total.");

            Gfx.DrawString($"Folha {n}/{total}", RetanguloNumeroFolhas, Danfe.EstiloPadrao.FonteNumeroFolhas, AlinhamentoHorizontal.Centro);
            Gfx.Flush();
        }

        public void DesenharAvisoHomologacao()
        {
            TextStack ts = new TextStack(RetanguloCorpo) { AlinhamentoVertical = AlinhamentoVertical.Centro, AlinhamentoHorizontal = AlinhamentoHorizontal.Centro, LineHeightScale = 0.9F }
                        .AddLine("SEM VALOR FISCAL", Danfe.EstiloPadrao.CriarFonteRegular(48))
                        .AddLine("AMBIENTE DE HOMOLOGAÇÃO", Danfe.EstiloPadrao.CriarFonteRegular(30));

            Gfx.PrimitiveComposer.BeginLocalState();
            Gfx.PrimitiveComposer.SetFillColor(new HESDanfe.Documents.Contents.ColorSpaces.DeviceRGBColor(0.35, 0.35, 0.35));
            ts.Draw(Gfx);
            Gfx.PrimitiveComposer.End();
        }

        public void DesenharBlocos(bool isPrimeirapagina = false)
        {
            if (isPrimeirapagina && Danfe.ViewModel.QuantidadeCanhotos > 0) DesenharCanhoto();

            var blocos = isPrimeirapagina ? Danfe.Blocos : Danfe.Blocos.Where(x => x.VisivelSomentePrimeiraPagina == false);

            foreach (var bloco in blocos)
            {
                bloco.Width = RetanguloDesenhavel.Width;

                if (bloco.Posicao == PosicaoBloco.Topo)
                {
                    bloco.SetPosition(RetanguloDesenhavel.Location);
                    RetanguloDesenhavel = RetanguloDesenhavel.CutTop(bloco.Height);
                }
                else
                {
                    bloco.SetPosition(RetanguloDesenhavel.X, RetanguloDesenhavel.Bottom - bloco.Height);
                    RetanguloDesenhavel = RetanguloDesenhavel.CutBottom(bloco.Height);
                }

                bloco.Draw(Gfx);

                if (bloco is BlocoIdentificacaoEmitente)
                {
                    var rf = (bloco as BlocoIdentificacaoEmitente).RetanguloNumeroFolhas;
                    RetanguloNumeroFolhas = rf;
                }
            }

            RetanguloCorpo = RetanguloDesenhavel;
            Gfx.Flush();
        }
    }
}
