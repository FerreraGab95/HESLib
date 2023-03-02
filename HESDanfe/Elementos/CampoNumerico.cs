using HESDanfe.Graphics;

namespace HESDanfe.Elementos
{
    /// <summary>
    /// Campo para valores numéricos.
    /// </summary>
    internal class CampoNumerico : Campo
    {
        private double? ConteudoNumerico { get; set; }
        public int CasasDecimais { get; set; }

        public CampoNumerico(string cabecalho, double? conteudoNumerico, Estilo estilo, int casasDecimais = 2) : base(cabecalho, null, estilo, AlinhamentoHorizontal.Direita)
        {
            CasasDecimais = casasDecimais;
            ConteudoNumerico = conteudoNumerico;
        }

        protected override void DesenharConteudo(Gfx gfx)
        {
            base.Conteudo = ConteudoNumerico.HasValue ? ConteudoNumerico.Value.ToString($"N{CasasDecimais}", Extensions.Util.Cultura) : null;
            base.DesenharConteudo(gfx);
        }
    }
}
