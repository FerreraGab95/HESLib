using System;
using System.Xml;
using System.Xml.Serialization;
using InnerLibs;
using InnerLibs.BR;

namespace HESDanfe.Esquemas
{
    [Serializable]
    [XmlType(AnonymousType = true, Namespace = Namespaces.NFe)]
    public class NFReferenciada
    {
        [XmlElement("refCTe", typeof(string))]
        [XmlElement("refECF", typeof(RefECF))]
        [XmlElement("refNF", typeof(RefNF))]
        [XmlElement("refNFP", typeof(RefNFP))]
        [XmlElement("refNFe", typeof(string))]
        [XmlChoiceIdentifier("TipoNFReferenciada")]
        public object Item;

        [XmlIgnore]
        public TipoNFReferenciada TipoNFReferenciada { get; set; }

        public override string ToString()
        {
            if (TipoNFReferenciada == TipoNFReferenciada.refCTe || TipoNFReferenciada == TipoNFReferenciada.refNFe)
            {
                string chaveAcesso = Item.ToString();
                return $"{Utils.TipoDFeDeChaveAcesso(chaveAcesso)} Ref.: {Utils.FormatarChaveAcesso(Item.ToString())}";
            }
            else
                return Item.ToString();
        }

    }

    [Serializable]
    [XmlType(AnonymousType = true, Namespace = Namespaces.NFe)]
    public enum TipoNFReferenciada
    {
        refCTe,
        refECF,
        refNF,
        refNFP,
        refNFe,
    }

    [Serializable]
    [XmlType(AnonymousType = true, Namespace = Namespaces.NFe)]
    public class RefECF
    {
        public string mod { get; set; }
        public string nECF { get; set; }
        public string nCOO { get; set; }

        public override string ToString() => $"ECF Ref.: Modelo: {mod} ECF: {nECF} COO: {nCOO}";
    }

    [Serializable]
    [XmlType(AnonymousType = true, Namespace = Namespaces.NFe)]
    public class RefNF
    {
        public string AAMM { get; set; }
        public string CNPJ { get; set; }
        public string mod { get; set; }
        public string serie { get; set; }
        public string nNF { get; set; }

        public override string ToString() => $"NF Ref.: Série: {serie} Número: {nNF} Emitente: {CNPJ.FormatCNPJ()} Modelo: {mod}";
    }

    [Serializable]
    [XmlType(AnonymousType = true, Namespace = Namespaces.NFe)]
    public class RefNFP
    {
        public string AAMM { get; set; }
        public string CNPJ { get; set; }
        public string CPF { get; set; }
        public string IE { get; set; }
        public string mod { get; set; }
        public string serie { get; set; }
        public string nNF { get; set; }

        public override string ToString() => $"NFP Ref.: Série: {serie} Número: {nNF} Emitente: {CNPJ.IfBlank(CPF).FormatCPFOrCNPJ()} Modelo: {mod} IE: {IE}";
    }
}
