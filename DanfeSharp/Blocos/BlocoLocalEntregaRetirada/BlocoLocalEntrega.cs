using HESDanfe.Modelo;

namespace HESDanfe.Blocos
{
    class BlocoLocalEntrega : BlocoLocalEntregaRetirada
    {
        public BlocoLocalEntrega(DocumentoFiscalViewModel viewModel, Estilo estilo) 
            : base(viewModel, estilo, viewModel.LocalEntrega)
        {
        }

        public override string Cabecalho => "Informações do local de entrega";
    }
}
