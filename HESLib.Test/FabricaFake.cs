using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using Extensions.BR;
using HES.Modelo;

namespace HES.Test
{
    public static class FabricaFake
    {
        public const double v = 1234.56;

        public static LocalEntregaRetiradaViewModel LocalEntregaRetiradaFake()
        {
            var t = Brasil.GerarEnderecoFake<LocalEntregaRetiradaViewModel>();
            t.NomeRazaoSocial = "Umbrella Corp";
            t.InscricaoEstadual = "361499373647";
            t.CnpjCpf = "22257735000138";
            t.Telefone = "1012345678";
            return t;
        }

        public static CalculoImpostoViewModel CalculoImpostoViewModel()
        {
            return new CalculoImpostoViewModel
            {
                BaseCalculoIcms = v,
                BaseCalculoIcmsSt = v,
                Desconto = v,
                OutrasDespesas = v,
                ValorAproximadoTributos = v,
                ValorCofins = v,
                ValorFrete = v,
                ValorIcms = v,
                ValorIcmsSt = v,
                ValorII = v,
                ValorIpi = v,
                ValorPis = v,
                ValorSeguro = v,
                ValorTotalNota = v,
                ValorTotalProdutos = v,
                vFCPUFDest = v,
                vICMSUFDest = v,
                vICMSUFRemet = v
            };
        }

        public static CalculoIssqnViewModel CalculoIssqnViewModel()
        {
            return new CalculoIssqnViewModel
            {
                BaseIssqn = v,
                InscricaoMunicipal = "123456789",
                Mostrar = true,
                ValorIssqn = v,
                ValorTotalServicos = v
            };
        }

        public static DANFEModel DanfeViewModel_1()
        {
            var m = new DANFEModel()
            {

                ChaveAcesso = new Extensions.BR.ChaveNFe()
                {
                    Nota = 888888888,
                    Serie = 888,
                },
                Emitente = Brasil.GerarEnderecoFake<EmpresaViewModel>().With(x =>
                {
                    x.CnpjCpf = new string('0', 14);
                    x.RazaoSocial = "Abstergo do Brasil Indústria de Tecnologia Ltda.";
                    x.NomeFantasia = "Abstergo";
                    x.Email = "fake@mail.123";
                    x.Ie = "87787";
                    x.IeSt = "87878";
                    x.IM = "45454";
                    x.Telefone = "0000000000";
                    x.CRT = "3";
                }),
                Destinatario = Brasil.GerarEnderecoFake<EmpresaViewModel>().With(x =>
                {
                    x.CnpjCpf = new string('1', 14);
                    x.RazaoSocial = "Umbrella Corp Ltda";
                    x.Email = "fake@mail.123";

                    x.Ie = "87787";
                    x.IeSt = "87878";
                    x.IM = "45454";
                    x.Telefone = "0000000000";
                }),
                InformacoesComplementares = "Aqui vai as informações complementares.",
                Transportadora = Brasil.GerarEnderecoFake<TransportadoraViewModel>().With(t =>
                {
                    t.RazaoSocial = "Correios";
                    t.CnpjCpf = new string('8', 14);
                    t.VeiculoUf = "RS";
                    t.QuantidadeVolumes = 123.1234;
                    t.CodigoAntt = new string('8', 20);
                    t.Especie = "Especie";
                    t.Placa = "MMMWWWW";
                    t.Ie = "12334";
                    t.PesoLiquido = 456.7794;
                    t.Marca = "HES";
                    t.ModalidadeFrete = 4;
                    t.PesoBruto = 101.1234;
                })
            };

            m.CalculoImposto = CalculoImpostoViewModel();
            m.CalculoIssqn = CalculoIssqnViewModel();

            m.Duplicatas = new List<DuplicataViewModel>();

            for (int i = 1; i <= 10; i++)
            {
                var d = new DuplicataViewModel()
                {
                    Numero = i.ToString(),
                    Valor = i * Math.PI,
                    Vecimento = new DateTime(9999, 12, 30)
                };

                m.Duplicatas.Add(d);
            }

            m.Produtos = new List<ProdutoViewModel>();

            for (int i = 1; i <= 100; i++)
            {
                var p = new ProdutoViewModel()
                {
                    Descricao = $"Produto da linha {i}",
                    Codigo = i.ToString(),
                    Quantidade = i * Math.PI * 10,
                    AliquotaIcms = 99.88,
                    Unidade = "PEC",
                    Ncm = new string('8', 8)
                };

                if (i % 10 == 0)
                {
                    p.Descricao = string.Concat(Enumerable.Repeat(p.Descricao + " ", 15));
                }

                m.Produtos.Add(p);
            }


            return m;
        }



    }
}
