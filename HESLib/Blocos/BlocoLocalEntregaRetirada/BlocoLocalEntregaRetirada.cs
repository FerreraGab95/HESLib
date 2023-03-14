using System;
using Extensions.BR;
using HES.Modelo;

namespace HES.Blocos
{
    abstract class BlocoLocalEntregaRetirada : BlocoBase
    {
        public LocalEntregaRetiradaViewModel Model { get; private set; }

        public BlocoLocalEntregaRetirada(DANFEModel viewModel, Estilo estilo, LocalEntregaRetiradaViewModel localModel) : base(viewModel, estilo)
        {
            Model = localModel ?? throw new ArgumentNullException(nameof(localModel));

            AdicionarLinhaCampos()
            .ComCampo("Nome/Razão Social", Model.NomeRazaoSocial)
            .ComCampo(Model.CnpjCpf.PegarRotuloDocumento(), Model.CnpjCpf.FormatarCPFOuCNPJ(), AlinhamentoHorizontal.Centro)
            .ComCampo("Inscrição Estadual", Model.InscricaoEstadual, AlinhamentoHorizontal.Centro)
            .ComLarguras(0, 45F * Proporcao, 30F * Proporcao);

            AdicionarLinhaCampos()
            .ComCampo("Endereço", Model.FullBuildingInfo)
            .ComCampo("Bairro", Model.Neighborhood)
            .ComCampo("CEP", Model.PostalCode, AlinhamentoHorizontal.Centro)
            .ComLarguras(0, 45F * Proporcao, 30F * Proporcao);

            AdicionarLinhaCampos()
            .ComCampo("Município", Model.City)
            .ComCampo("UF", Model.StateCode, AlinhamentoHorizontal.Centro)
            .ComCampo("Telefone/Fax", Model.Telefone, AlinhamentoHorizontal.Centro)
            .ComLarguras(0, 7F * Proporcao, 30F * Proporcao);
        }

        public override PosicaoBloco Posicao => PosicaoBloco.Topo;

    }
}
