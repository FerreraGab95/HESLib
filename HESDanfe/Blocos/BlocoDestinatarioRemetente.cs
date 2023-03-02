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
            .ComCampo(Extensions.Util.RazaoSocial, destinatario.RazaoSocial)
            .ComCampo(Extensions.Util.CnpjCpf, destinatario.CnpjCpf.FormatCPFOrCNPJ(), AlinhamentoHorizontal.Centro)
            .ComCampo("Data de Emissão", viewModel.DataHoraEmissao.Formatar(), AlinhamentoHorizontal.Centro)
            .ComLarguras(0, 45F * Proporcao, 30F * Proporcao);

            AdicionarLinhaCampos()
            .ComCampo(Extensions.Util.Endereco, destinatario.EnderecoLinha1)
            .ComCampo(Extensions.Util.BairroDistrito, destinatario.EnderecoBairro)
            .ComCampo(Extensions.Util.Cep, destinatario.EnderecoCep.FormatCEP(), AlinhamentoHorizontal.Centro)
            .ComCampo("Data Entrada / Saída", ViewModel.DataSaidaEntrada.Formatar(), AlinhamentoHorizontal.Centro)
            .ComLarguras(0, 45F * Proporcao, 25F * Proporcao, 30F * Proporcao);

            AdicionarLinhaCampos()
            .ComCampo(Extensions.Util.Municipio, destinatario.Municipio)
            .ComCampo(Extensions.Util.FoneFax, destinatario.Telefone.FormatTelephoneNumber(), AlinhamentoHorizontal.Centro)
            .ComCampo(Extensions.Util.UF, destinatario.EnderecoUf, AlinhamentoHorizontal.Centro)
            .ComCampo(Extensions.Util.InscricaoEstadual, destinatario.Ie, AlinhamentoHorizontal.Centro)
            .ComCampo("Hora Entrada / Saída", ViewModel.HoraSaidaEntrada.Formatar(), AlinhamentoHorizontal.Centro)
            .ComLarguras(0, 35F * Proporcao, 7F * Proporcao, 40F * Proporcao, 30F * Proporcao);
        }

        public override string Cabecalho => "Destinatário / Remetente";
        public override PosicaoBloco Posicao => PosicaoBloco.Topo;
    }
}
