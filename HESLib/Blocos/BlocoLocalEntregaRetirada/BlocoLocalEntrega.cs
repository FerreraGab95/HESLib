using HES.Modelo;

namespace HES.Blocos
{
    class BlocoLocalEntrega : BlocoLocalEntregaRetirada
    {
        public BlocoLocalEntrega(DANFEModel viewModel, Estilo estilo) 
            : base(viewModel, estilo, viewModel.LocalEntrega)
        {
        }

        public override string Cabecalho => "Informações do local de entrega";
    }
}
