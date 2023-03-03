using Extensions;
using Extensions.BR;
using HES.Modelo;

namespace HES.Blocos
{
    internal class BlocoDestinatarioRemetente : BlocoBase
    {
        public BlocoDestinatarioRemetente(DANFEModel viewModel, Estilo estilo) : base(viewModel, estilo)
        {
            var destinatario = viewModel.Destinatario;

            AdicionarLinhaCampos()
            .ComCampo("Razão Social", destinatario.RazaoSocial)
            .ComCampo(destinatario.CnpjCpf.PegarRotuloDocumento(), destinatario.CnpjCpf.FormatarCPFOuCNPJ(), AlinhamentoHorizontal.Centro)
            .ComCampo("Data de Emissão", viewModel.DataHoraEmissao.Formatar(), AlinhamentoHorizontal.Centro)
            .ComLarguras(0, 45F * Proporcao, 30F * Proporcao);

            AdicionarLinhaCampos()
            .ComCampo("Endereço", destinatario.EnderecoLinha1)
            .ComCampo("Bairro", destinatario.EnderecoBairro)
            .ComCampo("CEP", destinatario.EnderecoCep.FormatarCEP(), AlinhamentoHorizontal.Centro)
            .ComCampo("Data Entrada / Saída", ViewModel.DataSaidaEntrada.Formatar(), AlinhamentoHorizontal.Centro)
            .ComLarguras(0, 45F * Proporcao, 25F * Proporcao, 30F * Proporcao);

            AdicionarLinhaCampos()
            .ComCampo("Município", destinatario.Municipio)
            .ComCampo("Telefone/Fax", destinatario.Telefone.FormatarTelefone(), AlinhamentoHorizontal.Centro)
            .ComCampo("UF", destinatario.EnderecoUf, AlinhamentoHorizontal.Centro)
            .ComCampo("Inscrição Estadual", destinatario.Ie, AlinhamentoHorizontal.Centro)
            .ComCampo("Hora Entrada / Saída", ViewModel.HoraSaidaEntrada.Formatar(), AlinhamentoHorizontal.Centro)
            .ComLarguras(0, 35F * Proporcao, 7F * Proporcao, 40F * Proporcao, 30F * Proporcao);
        }

        public override string Cabecalho => "Destinatário / Remetente";
        public override PosicaoBloco Posicao => PosicaoBloco.Topo;
    }
}
