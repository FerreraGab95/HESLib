using System;
using HESDanfe.Modelo;
using InnerLibs.BR;

namespace HESDanfe.Blocos
{
    abstract class BlocoLocalEntregaRetirada : BlocoBase
    {
        public LocalEntregaRetiradaViewModel Model { get; private set; }

        public BlocoLocalEntregaRetirada(DANFEViewModel viewModel, Estilo estilo, LocalEntregaRetiradaViewModel localModel) : base(viewModel, estilo)
        {
            Model = localModel ?? throw new ArgumentNullException(nameof(localModel));

            AdicionarLinhaCampos()
            .ComCampo(Utils.NomeRazaoSocial, Model.NomeRazaoSocial)
            .ComCampo(Utils.CnpjCpf, Model.CnpjCpf.FormatCPFOrCNPJ(), AlinhamentoHorizontal.Centro)
            .ComCampo(Utils.InscricaoEstadual, Model.InscricaoEstadual, AlinhamentoHorizontal.Centro)
            .ComLarguras(0, 45F * Proporcao, 30F * Proporcao);

            AdicionarLinhaCampos()
            .ComCampo(Utils.Endereco, Model.Endereco)
            .ComCampo(Utils.BairroDistrito, Model.Bairro)
            .ComCampo(Utils.Cep, Model.Cep.FormatCEP(), AlinhamentoHorizontal.Centro)
            .ComLarguras(0, 45F * Proporcao, 30F * Proporcao);

            AdicionarLinhaCampos()
            .ComCampo(Utils.Municipio, Model.Municipio)
            .ComCampo(Utils.UF, Model.Uf, AlinhamentoHorizontal.Centro)
            .ComCampo(Utils.FoneFax, Model.Telefone.FormatTelephoneNumber(), AlinhamentoHorizontal.Centro)
            .ComLarguras(0, 7F * Proporcao, 30F * Proporcao);
        }

        public override PosicaoBloco Posicao => PosicaoBloco.Topo;

    }
}
