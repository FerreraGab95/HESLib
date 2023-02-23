using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using HESDanfe.Esquemas;
using HESDanfe.Esquemas.NFe;

namespace HESDanfe.Modelo
{
    /// <summary>
    /// Modelo de dados utilizado para o DANFE ou CC.
    /// </summary>
    public class DocumentoFiscalViewModel

    {
        #region Private Fields

        private float _Margem;
        private int _quant_canhoto;

        #endregion Private Fields

        #region Private Methods

        private static EmpresaViewModel CreateEmpresaFrom(Empresa empresa)
        {
            EmpresaViewModel model = new EmpresaViewModel();

            model.RazaoSocial = empresa.xNome;
            model.CnpjCpf = !string.IsNullOrWhiteSpace(empresa.CNPJ) ? empresa.CNPJ : empresa.CPF;
            model.Ie = empresa.IE;
            model.IeSt = empresa.IEST;
            model.Email = empresa.email;

            var end = empresa.Endereco;

            if (end != null)
            {
                model.EnderecoLogadrouro = end.xLgr;
                model.EnderecoNumero = end.nro;
                model.EnderecoBairro = end.xBairro;
                model.Municipio = end.xMun;
                model.EnderecoUf = end.UF;
                model.EnderecoCep = end.CEP;
                model.Telefone = end.fone;
                model.EnderecoComplemento = end.xCpl;
            }

            if (empresa is Emitente)
            {
                var emit = empresa as Emitente;
                model.IM = emit.IM;
                model.CRT = emit.CRT;
                model.NomeFantasia = emit.xFant;
            }

            return model;
        }

        private static LocalEntregaRetiradaViewModel CreateLocalRetiradaEntrega(LocalEntregaRetirada local)
        {
            var m = new LocalEntregaRetiradaViewModel()
            {
                NomeRazaoSocial = local.xNome,
                CnpjCpf = !string.IsNullOrWhiteSpace(local.CNPJ) ? local.CNPJ : local.CPF,
                InscricaoEstadual = local.IE,
                Bairro = local.xBairro,
                Municipio = local.xMun,
                Uf = local.UF,
                Cep = local.CEP,
                Telefone = local.fone
            };

            StringBuilder sb = new StringBuilder();
            sb.Append(local.xLgr);

            if (!string.IsNullOrWhiteSpace(local.nro))
            {
                sb.Append(", ").Append(local.nro);
            }

            if (!string.IsNullOrWhiteSpace(local.xCpl))
            {
                sb.Append(" - ").Append(local.xCpl);
            }

            m.Endereco = sb.ToString();

            return m;
        }

        private static DocumentoFiscalViewModel CriarDeArquivoXmlInternal(TextReader nfeReader, TextReader cceReader = null)
        {
            ProcNFe nfe = null;
            ProcEventoNFe cce = null;
            XmlSerializer nfeSerializer = new XmlSerializer(typeof(ProcNFe));
            XmlSerializer cceSerializer = new XmlSerializer(typeof(ProcEventoNFe));

            try
            {
                nfe = (ProcNFe)nfeSerializer.Deserialize(nfeReader);
                if (cceReader != null)
                {
                    cce = (ProcEventoNFe)cceSerializer.Deserialize(cceReader);
                }
                return Create(nfe, cce);
            }
            catch (InvalidOperationException e)
            {
                if (e.InnerException is XmlException ex)
                {
                    throw new Exception(string.Format("Não foi possível interpretar o Xml. Linha {0} Posição {1}.", ex.LineNumber, ex.LinePosition));
                }

                throw new XmlException("O Xml não parece ser uma NF-e processada.", e);
            }
        }

        #endregion Private Methods

        #region Internal Methods

        internal static CalculoImpostoViewModel CriarCalculoImpostoViewModel(ICMSTotal i) => new CalculoImpostoViewModel()
        {
            ValorAproximadoTributos = i.vTotTrib,
            BaseCalculoIcms = i.vBC,
            ValorIcms = i.vICMS,
            BaseCalculoIcmsSt = i.vBCST,
            ValorIcmsSt = i.vST,
            ValorTotalProdutos = i.vProd,
            ValorFrete = i.vFrete,
            ValorSeguro = i.vSeg,
            Desconto = i.vDesc,
            ValorII = i.vII,
            ValorIpi = i.vIPI,
            ValorPis = i.vPIS,
            ValorCofins = i.vCOFINS,
            OutrasDespesas = i.vOutro,
            ValorTotalNota = i.vNF,
            vFCPUFDest = i.vFCPUFDest,
            vICMSUFDest = i.vICMSUFDest,
            vICMSUFRemet = i.vICMSUFRemet
        };

        internal static void ExtrairDatas(DocumentoFiscalViewModel model, InfNFe infNfe)
        {
            var ide = infNfe.ide;

            if (infNfe.Versao.Maior >= 3)
            {
                if (ide.dhEmi.HasValue) model.DataHoraEmissao = ide.dhEmi?.DateTimeOffsetValue.DateTime;
                if (ide.dhSaiEnt.HasValue) model.DataSaidaEntrada = ide.dhSaiEnt?.DateTimeOffsetValue.DateTime;

                if (model.DataSaidaEntrada.HasValue)
                {
                    model.HoraSaidaEntrada = model.DataSaidaEntrada?.TimeOfDay;
                }
            }
            else
            {
                model.DataHoraEmissao = ide.dEmi;
                model.DataSaidaEntrada = ide.dSaiEnt;

                if (!string.IsNullOrWhiteSpace(ide.hSaiEnt))
                {
                    model.HoraSaidaEntrada = TimeSpan.Parse(ide.hSaiEnt);
                }
            }
        }

        #endregion Internal Methods

        #region Public Fields

        public static readonly IEnumerable<FormaEmissao> FormasEmissaoSuportadas = new FormaEmissao[] { FormaEmissao.Normal, FormaEmissao.ContingenciaSVCAN, FormaEmissao.ContingenciaSVCRS };

        #endregion Public Fields

        #region Public Constructors

        public DocumentoFiscalViewModel()
        {
            QuantidadeCanhotos = 1;
            Margem = 4;
            Orientacao = Orientacao.Retrato;
            CalculoImposto = new CalculoImpostoViewModel();
            Emitente = new EmpresaViewModel();
            Destinatario = new EmpresaViewModel();
            Duplicatas = new List<DuplicataViewModel>();
            Produtos = new List<ProdutoViewModel>();
            Transportadora = new TransportadoraViewModel();
            CalculoIssqn = new CalculoIssqnViewModel();
            NotasFiscaisReferenciadas = new List<string>();
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// View Model do bloco Cálculo do Imposto
        /// </summary>
        public CalculoImpostoViewModel CalculoImposto { get; set; }

        /// <summary>
        /// View Model do Bloco Cálculo do Issqn
        /// </summary>
        public CalculoIssqnViewModel CalculoIssqn { get; set; }

        /// <summary>
        /// Chave de Acesso
        /// </summary>
        public string ChaveAcesso { get; set; }

        /// <summary>
        /// Código do status da resposta, cStat, do elemento infProt.
        /// </summary>
        public int? CodigoStatusReposta { get; set; }

        public DateTime? ContingenciaDataHora { get; set; }

        public string ContingenciaJustificativa { get; set; }

        /// <summary>
        /// Tag xCont
        /// </summary>
        public string Contrato { get; set; }

        /// <summary>
        /// <para>Data e Hora de emissão do Documento Fiscal</para>
        /// <para>Tag dhEmi ou dEmi</para>
        /// </summary>
        public DateTime? DataHoraEmissao { get; set; }

        /// <summary>
        /// <para>Data de Saída ou da Entrada da Mercadoria/Produto</para>
        /// <para>Tag dSaiEnt e dhSaiEnt</para>
        /// </summary>
        public DateTime? DataSaidaEntrada { get; set; }

        /// <summary>
        /// Descrição do status da resposta, xMotivo, do elemento infProt.
        /// </summary>
        public string DescricaoStatusReposta { get; set; }

        /// <summary>
        /// Dados do Destinatário
        /// </summary>
        public EmpresaViewModel Destinatario { get; set; }

        /// <summary>
        /// Faturas da Nota Fiscal
        /// </summary>
        public List<DuplicataViewModel> Duplicatas { get; set; }

        /// <summary>
        /// Dados do Emitente
        /// </summary>
        public EmpresaViewModel Emitente { get; set; }

        /// <summary>
        /// Exibi o bloco "Informações do local de entrega" quando o elemento "entrega" estiver disponível.
        /// </summary>
        public bool ExibirBlocoLocalEntrega { get; set; } = true;

        /// <summary>
        /// Exibi o bloco "Informações do local de retirada" quando o elemento "retirada" estiver disponível.
        /// </summary>
        public bool ExibirBlocoLocalRetirada { get; set; } = true;

        /// <summary>
        /// Exibi os valores do ICMS Interestadual e Valor Total dos Impostos no bloco Cálculos do Imposto.
        /// </summary>
        public bool ExibirIcmsInterestadual { get; set; } = true;

        /// <summary>
        /// Exibi os valores do PIS e COFINS no bloco Cálculos do Imposto.
        /// </summary>
        public bool ExibirPisConfins { get; set; } = true;

        /// <summary>
        /// <para>Hora de Saída ou da Entrada da Mercadoria/Produto</para>
        /// <para>Tag dSaiEnt e hSaiEnt</para>
        /// </summary>
        public TimeSpan? HoraSaidaEntrada { get; set; }

        /// <summary>
        /// <para>Informações adicionais de interesse do Fisco</para>
        /// <para>Tag infAdFisco</para>
        /// </summary>
        public string InformacoesAdicionaisFisco { get; set; }

        /// <summary>
        /// <para>Informações Complementares de interesse do Contribuinte</para>
        /// <para>Tag infCpl</para>
        /// </summary>
        public string InformacoesComplementares { get; set; }

        public bool IsPaisagem => Orientacao == Orientacao.Paisagem;

        public bool IsRetrato => Orientacao == Orientacao.Retrato;

        public LocalEntregaRetiradaViewModel LocalEntrega { get; set; }

        public LocalEntregaRetiradaViewModel LocalRetirada { get; set; }

        /// <summary>
        /// Magens horizontais e verticais do DANFE.
        /// </summary>
        public float Margem
        {
            get => _Margem;
            set
            {
                if (value >= 2 && value <= 5)
                    _Margem = value;
                else
                    throw new ArgumentOutOfRangeException("A margem deve ser entre 2 e 5.");
            }
        }

        public bool MostrarCalculoIssqn { get; set; }

        /// <summary>
        /// <para>Descrição da Natureza da Operação</para>
        /// <para>Tag natOp</para>
        /// </summary>
        public string NaturezaOperacao { get; set; }

        /// <summary>
        /// <para>Número do Documento Fiscal</para>
        /// <para>Tag nNF</para>
        /// </summary>
        public int NfNumero { get; set; }

        /// <summary>
        /// <para>Série do Documento Fiscal</para>
        /// <para>Tag serie</para>
        /// </summary>
        public int NfSerie { get; set; }

        /// <summary>
        /// Tag xNEmp
        /// </summary>
        public string NotaEmpenho { get; set; }

        /// <summary>
        /// Informações de Notas Fiscais referenciadas que serão levadas no texto adicional.
        /// </summary>
        public List<string> NotasFiscaisReferenciadas { get; set; }

        public Orientacao Orientacao { get; set; } = Orientacao.Retrato;

        /// <summary>
        /// Tag xPed
        /// </summary>
        public string Pedido { get; set; }

        /// <summary>
        /// Exibe o Nome Fantasia, caso disponível, ao invés da Razão Social no quadro identificação
        /// do emitente.
        /// </summary>
        public bool PreferirEmitenteNomeFantasia { get; set; } = true;

        /// <summary>
        /// Produtos da Nota Fiscal
        /// </summary>
        public List<ProdutoViewModel> Produtos { get; set; }

        /// <summary>
        /// Numero do protocolo com sua data e hora
        /// </summary>
        public string ProtocoloAutorizacao { get; set; }

        /// <summary>
        /// Quantidade de canhotos a serem impressos.
        /// </summary>
        public int QuantidadeCanhotos
        {
            get => _quant_canhoto;
            set
            {
                if (value >= 0 && value <= 2)
                    _quant_canhoto = value;
                else
                    throw new ArgumentOutOfRangeException("A quantidade de canhotos deve de 0 a 2.");
            }
        }

        public virtual string TextoAdicional
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                if (!string.IsNullOrEmpty(InformacoesComplementares))
                    sb.AppendChaveValor("Inf. Contribuinte", InformacoesComplementares).Replace(";", "\r\n");

                if (!string.IsNullOrEmpty(Destinatario.Email))
                {
                    // Adiciona um espaço após a virgula caso necessário, isso facilita a quebra de linha.
                    var destEmail = Regex.Replace(Destinatario.Email, @"(?<=\S)([,;])(?=\S)", "$1 ").Trim(new char[] { ' ', ',', ';' });
                    sb.AppendChaveValor("Email do Destinatário", destEmail);
                }

                if (!string.IsNullOrEmpty(InformacoesAdicionaisFisco))
                    sb.AppendChaveValor("Inf. fisco", InformacoesAdicionaisFisco);

                if (!string.IsNullOrEmpty(Pedido) && !Utils.StringContemChaveValor(InformacoesComplementares, "Pedido", Pedido))
                    sb.AppendChaveValor("Pedido", Pedido);

                if (!string.IsNullOrEmpty(Contrato) && !Utils.StringContemChaveValor(InformacoesComplementares, "Contrato", Contrato))
                    sb.AppendChaveValor("Contrato", Contrato);

                if (!string.IsNullOrEmpty(NotaEmpenho))
                    sb.AppendChaveValor("Nota de Empenho", NotaEmpenho);

                foreach (var nfref in NotasFiscaisReferenciadas)
                {
                    if (sb.Length > 0) sb.Append(" ");
                    sb.Append(nfref);
                }

                #region NT 2013.003 Lei da Transparência

                if (CalculoImposto.ValorAproximadoTributos.HasValue && (string.IsNullOrEmpty(InformacoesComplementares) ||
                    !Regex.IsMatch(InformacoesComplementares, @"((valor|vlr?\.?)\s+(aprox\.?|aproximado)\s+(dos\s+)?(trib\.?|tributos))|((trib\.?|tributos)\s+(aprox\.?|aproximado))", RegexOptions.IgnoreCase)))
                {
                    if (sb.Length > 0) sb.Append("\r\n");
                    sb.Append("Valor Aproximado dos Tributos: ");
                    sb.Append(CalculoImposto.ValorAproximadoTributos.FormatarMoeda());
                }

                #endregion NT 2013.003 Lei da Transparência

                return sb.ToString();
            }
        }

        public virtual string TextoAdicionalFisco
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                if (TipoEmissao == FormaEmissao.ContingenciaSVCAN || TipoEmissao == FormaEmissao.ContingenciaSVCRS)
                {
                    sb.Append("Contingência ");

                    if (TipoEmissao == FormaEmissao.ContingenciaSVCAN)
                        sb.Append("SVC-AN");

                    if (TipoEmissao == FormaEmissao.ContingenciaSVCRS)
                        sb.Append("SVC-RS");

                    if (ContingenciaDataHora.HasValue)
                    {
                        sb.Append($" - {ContingenciaDataHora.FormatarDataHora()}");
                    }

                    if (!string.IsNullOrWhiteSpace(ContingenciaJustificativa))
                    {
                        sb.Append($" - {ContingenciaJustificativa}");
                    }

                    sb.Append(".");
                }

                return sb.ToString();
            }
        }

        public string TextoCondicaoDeUso { get; internal set; }
        public string TextoCorrecao { get; set; }
        public int SequenciaCorrecao { get; set; } = 0;
        public virtual string TextoRecebimento => $"Recebemos de {Emitente.RazaoSocial} os produtos e/ou serviços constantes na Nota Fiscal Eletrônica indicada {(Orientacao == Orientacao.Retrato ? "abaixo" : "ao lado")}. Emissão: {DataHoraEmissao.Formatar()} Valor Total: R$ {CalculoImposto.ValorTotalNota.Formatar()} Destinatário: {Destinatario.RazaoSocial}";

        /// <summary>
        /// Tipo de Ambiente
        /// </summary>
        public TAmb TipoAmbiente { get; set; } = TAmb.Producao;

        /// <summary>
        /// Tipo de emissão
        /// </summary>
        public FormaEmissao TipoEmissao { get; set; }

        /// <summary>
        /// <para>Tipo de Operação - 0-entrada / 1-saída</para>
        /// <para>Tag tpNF</para>
        /// </summary>
        public Tipo TipoNF { get; set; }

        /// <summary>
        /// Dados da Transportadora
        /// </summary>
        public TransportadoraViewModel Transportadora { get; set; }

        #endregion Public Properties

        #region Public Methods

        public static DocumentoFiscalViewModel Create(ProcNFe procNfe, ProcEventoNFe procEventoNFe = null)
        {
            var model = new DocumentoFiscalViewModel();

            var infNfe = procNfe.NFe.infNFe;
            var ide = infNfe.ide;
            model.TipoEmissao = ide.tpEmis;

            if (ide.mod != 55)
            {
                throw new NotSupportedException("Somente o mod==55 está implementado.");
            }

            if (!FormasEmissaoSuportadas.Contains(model.TipoEmissao))
            {
                throw new NotSupportedException($"O tpEmis {ide.tpEmis} não é suportado.");
            }

            model.Orientacao = ide.tpImp == 1 ? Orientacao.Retrato : Orientacao.Paisagem;

            var infProt = procNfe.protNFe.infProt;
            model.CodigoStatusReposta = infProt.cStat;
            model.DescricaoStatusReposta = infProt.xMotivo;

            model.TipoAmbiente = ide.tpAmb;
            model.NfNumero = ide.nNF;
            model.NfSerie = ide.serie;
            model.NaturezaOperacao = ide.natOp;
            model.ChaveAcesso = procNfe.NFe.infNFe.Id.Substring(3);
            model.TipoNF = ide.tpNF;

            model.Emitente = CreateEmpresaFrom(infNfe.emit);
            model.Destinatario = CreateEmpresaFrom(infNfe.dest);

            // Local retirada e entrega
            if (infNfe.retirada != null)
            {
                model.LocalRetirada = CreateLocalRetiradaEntrega(infNfe.retirada);
            }

            if (infNfe.entrega != null)
            {
                model.LocalEntrega = CreateLocalRetiradaEntrega(infNfe.entrega);
            }

            model.NotasFiscaisReferenciadas = ide.NFref.Select(x => x.ToString()).ToList();

            // Informações adicionais de compra
            if (infNfe.compra != null)
            {
                model.Contrato = infNfe.compra.xCont;
                model.NotaEmpenho = infNfe.compra.xNEmp;
                model.Pedido = infNfe.compra.xPed;
            }

            foreach (var det in infNfe.det)
            {
                var produto = new ProdutoViewModel
                {
                    Codigo = det.prod.cProd,
                    Descricao = det.prod.xProd,
                    Ncm = det.prod.NCM,
                    Cfop = det.prod.CFOP,
                    Unidade = det.prod.uCom,
                    Quantidade = det.prod.qCom,
                    ValorUnitario = det.prod.vUnCom,
                    ValorTotal = det.prod.vProd,
                    InformacoesAdicionais = det.infAdProd
                };

                var imposto = det.imposto;

                if (imposto != null)
                {
                    if (imposto.ICMS != null)
                    {
                        var icms = imposto.ICMS.ICMS;

                        if (icms != null)
                        {
                            produto.ValorIcms = icms.vICMS;
                            produto.BaseIcms = icms.vBC;
                            produto.AliquotaIcms = icms.pICMS;
                            produto.OCst = icms.orig + icms.CST + icms.CSOSN;
                        }
                    }

                    if (imposto.IPI != null)
                    {
                        var ipi = imposto.IPI.IPITrib;

                        if (ipi != null)
                        {
                            produto.ValorIpi = ipi.vIPI;
                            produto.AliquotaIpi = ipi.pIPI;
                        }
                    }
                }

                model.Produtos.Add(produto);
            }

            if (infNfe.cobr != null)
            {
                foreach (var item in infNfe.cobr.dup)
                {
                    var duplicata = new DuplicataViewModel
                    {
                        Numero = item.nDup,
                        Valor = item.vDup,
                        Vecimento = item.dVenc
                    };

                    model.Duplicatas.Add(duplicata);
                }
            }

            model.CalculoImposto = CriarCalculoImpostoViewModel(infNfe.total.ICMSTot);

            var issqnTotal = infNfe.total.ISSQNtot;

            if (issqnTotal != null)
            {
                var c = model.CalculoIssqn;
                c.InscricaoMunicipal = infNfe.emit.IM;
                c.BaseIssqn = issqnTotal.vBC;
                c.ValorTotalServicos = issqnTotal.vServ;
                c.ValorIssqn = issqnTotal.vISS;
                c.Mostrar = true;
            }

            var transp = infNfe.transp;
            var transportadora = transp.transporta;
            var transportadoraModel = model.Transportadora;

            transportadoraModel.ModalidadeFrete = (int)transp.modFrete;

            if (transp.veicTransp != null)
            {
                transportadoraModel.VeiculoUf = transp.veicTransp.UF;
                transportadoraModel.CodigoAntt = transp.veicTransp.RNTC;
                transportadoraModel.Placa = transp.veicTransp.placa;
            }

            if (transportadora != null)
            {
                transportadoraModel.RazaoSocial = transportadora.xNome;
                transportadoraModel.EnderecoUf = transportadora.UF;
                transportadoraModel.CnpjCpf = !string.IsNullOrWhiteSpace(transportadora.CNPJ) ? transportadora.CNPJ : transportadora.CPF;
                transportadoraModel.EnderecoLogadrouro = transportadora.xEnder;
                transportadoraModel.Municipio = transportadora.xMun;
                transportadoraModel.Ie = transportadora.IE;
            }

            var vol = transp.vol.FirstOrDefault();

            if (vol != null)
            {
                transportadoraModel.QuantidadeVolumes = vol.qVol;
                transportadoraModel.Especie = vol.esp;
                transportadoraModel.Marca = vol.marca;
                transportadoraModel.Numeracao = vol.nVol;
                transportadoraModel.PesoBruto = vol.pesoB;
                transportadoraModel.PesoLiquido = vol.pesoL;
            }

            var infAdic = infNfe.infAdic;
            if (infAdic != null)
            {
                model.InformacoesComplementares = procNfe.NFe.infNFe.infAdic.infCpl;
                model.InformacoesAdicionaisFisco = procNfe.NFe.infNFe.infAdic.infAdFisco;
            }

            var infoProto = procNfe.protNFe.infProt;

            model.ProtocoloAutorizacao = string.Format(Formatador.Cultura, "{0} - {1}", infoProto.nProt, infoProto.dhRecbto.DateTimeOffsetValue.DateTime);

            ExtrairDatas(model, infNfe);

            // Contingência SVC-AN e SVC-RS
            if (model.TipoEmissao == FormaEmissao.ContingenciaSVCAN || model.TipoEmissao == FormaEmissao.ContingenciaSVCRS)
            {
                model.ContingenciaDataHora = ide.dhCont?.DateTimeOffsetValue.DateTime;
                model.ContingenciaJustificativa = ide.xJust;
            }

            //implementação da carta de correção
            if (procEventoNFe != null)
            {
                var inf = procEventoNFe.Evento?.InfEvento;

                model.SequenciaCorrecao = inf.NSeqEvento;

                var det = inf?.DetEvento;
                if (det != null)
                {
                    model.TextoCondicaoDeUso = det.XCondUso?.Replace(";", Environment.NewLine);
                    model.TextoCorrecao = det.XCorrecao;
                }
            }

            return model;
        }

        /// <summary>
        /// Cria o modelo a partir de um arquivo xml.
        /// </summary>
        /// <param name="caminhoNFe"></param>
        /// <returns></returns>
        public static DocumentoFiscalViewModel CriarDeArquivoXml(string caminhoNFe, string caminhoCCe)
        {
            using (var sr = new StreamReader(caminhoNFe, true))
            {
                if (string.IsNullOrWhiteSpace(caminhoCCe))
                {
                    return CriarDeArquivoXmlInternal(sr);
                }
                else
                {
                    using (var sr2 = new StreamReader(caminhoCCe, true))
                    {
                        return CriarDeArquivoXmlInternal(sr, sr2);
                    }
                }
            }
        }

        /// <summary>
        /// Cria o modelo a partir de um arquivo xml contido num stream.
        /// </summary>
        /// <param name="nfeStream"></param>
        /// <returns>Modelo</returns>
        public static DocumentoFiscalViewModel CriarDeArquivoXml(Stream nfeStream, Stream cceStream = null)
        {
            if (nfeStream == null) throw new ArgumentNullException(nameof(nfeStream));

            using (var sr = new StreamReader(nfeStream, true))
            {
                if (cceStream != null)
                {
                    using (var sr2 = new StreamReader(cceStream, true))
                    {
                        return CriarDeArquivoXmlInternal(sr, sr2);
                    }
                }
                else
                {
                    return CriarDeArquivoXmlInternal(sr);
                }
            }
        }

        /// <summary>
        /// Cria o modelo a partir de uma string xml.
        /// </summary>
        public static DocumentoFiscalViewModel CriarDeStringXml(string nfeXML, string cceXML = null)
        {
            if (nfeXML == null) throw new ArgumentNullException(nameof(nfeXML));

            using (var sr = new StringReader(nfeXML))
            {
                if (cceXML != null)
                {
                    using (var sr2 = new StringReader(cceXML))
                    {
                        return CriarDeArquivoXmlInternal(sr, sr2);
                    }
                }
                else
                {
                    return CriarDeArquivoXmlInternal(sr);
                }
            }
        }

        #endregion Public Methods
    }
}