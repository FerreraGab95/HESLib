using HESDanfe.Modelo;

namespace HESDanfe.Blocos
{
    class BlocoLocalRetirada : BlocoLocalEntregaRetirada
    {
        public BlocoLocalRetirada(DANFEViewModel viewModel, Estilo estilo) 
            : base(viewModel, estilo, viewModel.LocalRetirada)
        {
        }

        public override string Cabecalho => "Informações do local de retirada";
    }
}
