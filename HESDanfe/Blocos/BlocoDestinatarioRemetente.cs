using HESDanfe.Modelo;
using InnerLibs.BR;

namespace HESDanfe.Blocos
{
    internal class BlocoDestinatarioRemetente : BlocoBase
    {
        public BlocoDestinatarioRemetente(DANFEModel viewModel, Estilo estilo) : base(viewModel, estilo)
        {
            var destinatario = viewModel.Destinatario;

            AdicionarLinhaCampos()
            .ComCampo(Utils.RazaoSocial, destinatario.RazaoSocial)
            .ComCampo(Utils.CnpjCpf, destinatario.CnpjCpf.FormatCPFOrCNPJ(), AlinhamentoHorizontal.Centro)
            .ComCampo("Data de Emissão", viewModel.DataHoraEmissao.Formatar(), AlinhamentoHorizontal.Centro)
            .ComLarguras(0, 45F * Proporcao, 30F * Proporcao);

            AdicionarLinhaCampos()
            .ComCampo(Utils.Endereco, destinatario.EnderecoLinha1)
            .ComCampo(Utils.BairroDistrito, destinatario.EnderecoBairro)
            .ComCampo(Utils.Cep, destinatario.EnderecoCep.FormatCEP(), AlinhamentoHorizontal.Centro)
            .ComCampo("Data Entrada / Saída", ViewModel.DataSaidaEntrada.Formatar(), AlinhamentoHorizontal.Centro)
            .ComLarguras(0, 45F * Proporcao, 25F * Proporcao, 30F * Proporcao);

            AdicionarLinhaCampos()
            .ComCampo(Utils.Municipio, destinatario.Municipio)
            .ComCampo(Utils.FoneFax, destinatario.Telefone.FormatTelephoneNumber(), AlinhamentoHorizontal.Centro)
            .ComCampo(Utils.UF, destinatario.EnderecoUf, AlinhamentoHorizontal.Centro)
            .ComCampo(Utils.InscricaoEstadual, destinatario.Ie, AlinhamentoHorizontal.Centro)
            .ComCampo("Hora Entrada / Saída", ViewModel.HoraSaidaEntrada.Formatar(), AlinhamentoHorizontal.Centro)
            .ComLarguras(0, 35F * Proporcao, 7F * Proporcao, 40F * Proporcao, 30F * Proporcao);
        }

        public override string Cabecalho => "Destinatário / Remetente";
        public override PosicaoBloco Posicao => PosicaoBloco.Topo;
    }
}
