using DanfeSharp.Modelo;

namespace DanfeSharp.Blocos
{
    class BlocoLocalRetirada : BlocoLocalEntregaRetirada
    {
        public BlocoLocalRetirada(DocumentoFiscalViewModel viewModel, Estilo estilo) 
            : base(viewModel, estilo, viewModel.LocalRetirada)
        {
        }

        public override string Cabecalho => "Informações do local de retirada";
    }
}
