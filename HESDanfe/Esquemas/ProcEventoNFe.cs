using System;
using System.Xml.Serialization;
using HESDanfe.Esquemas.NFe;

namespace HESDanfe.Esquemas
{
    [XmlRoot(ElementName = "detEvento")]
    public class DetEvento
    {
        #region Public Properties

        [XmlElement(ElementName = "descEvento")]
        public string DescEvento { get; set; }

        [XmlText]
        public string Text { get; set; }

        [XmlAttribute(AttributeName = "versao")]
        public string Versao { get; set; }

        [XmlElement(ElementName = "xCondUso")]
        public string XCondUso { get; set; }

        [XmlElement(ElementName = "xCorrecao")]
        public string XCorrecao { get; set; }

        #endregion Public Properties
    }

    [XmlRoot(ElementName = "evento")]
    public class Evento
    {
        #region Public Properties

        [XmlElement(ElementName = "infEvento")]
        public InfEvento InfEvento { get; set; }

        [XmlText]
        public string Text { get; set; }

        [XmlAttribute(AttributeName = "versao")]
        public string Versao { get; set; }

        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }

        #endregion Public Properties
    }

    [XmlRoot(ElementName = "infEvento")]
    public class InfEvento
    {
        #region Public Properties

        [XmlElement(ElementName = "chNFe")]
        public string ChNFe { get; set; }

        [XmlElement(ElementName = "CNPJ")]
        public string CNPJ { get; set; }

        [XmlElement(ElementName = "CNPJDest")]
        public string CNPJDest { get; set; }

        [XmlElement(ElementName = "cOrgao")]
        public int COrgao { get; set; }

        [XmlElement(ElementName = "cStat")]
        public int CStat { get; set; }

        [XmlElement(ElementName = "detEvento")]
        public DetEvento DetEvento { get; set; }

        [XmlElement(ElementName = "dhEvento")]
        public DateTime DhEvento { get; set; }

        [XmlElement(ElementName = "dhRegEvento")]
        public DateTime DhRegEvento { get; set; }

        [XmlAttribute(AttributeName = "Id")]
        public string Id { get; set; }

        [XmlElement(ElementName = "nProt")]
        public string NProt { get; set; }

        [XmlElement(ElementName = "nSeqEvento")]
        public int NSeqEvento { get; set; }

        [XmlText]
        public string Text { get; set; }

        [XmlElement(ElementName = "tpAmb")]
        public TAmb TpAmb { get; set; }

        [XmlElement(ElementName = "tpEvento")]
        public int TpEvento { get; set; }

        [XmlElement(ElementName = "verAplic")]
        public string VerAplic { get; set; }

        [XmlElement(ElementName = "verEvento")]
        public string VerEvento { get; set; }

        [XmlElement(ElementName = "xEvento")]
        public string XEvento { get; set; }

        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }

        [XmlElement(ElementName = "xMotivo")]
        public string XMotivo { get; set; }

        #endregion Public Properties
    }

    [XmlRoot(Namespace = Namespaces.NFe, ElementName = "procEventoNFe", DataType = "string", IsNullable = true)]
    public class ProcEventoNFe
    {
        #region Public Properties

        [XmlElement(ElementName = "evento")]
        public Evento Evento { get; set; }

        [XmlElement(ElementName = "retEvento")]
        public RetEvento RetEvento { get; set; }

        [XmlText]
        public string Text { get; set; }

        [XmlAttribute(AttributeName = "versao")]
        public string Versao { get; set; }

        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }

        #endregion Public Properties
    }

    [XmlType(Namespace = Namespaces.NFe)]
    [XmlRoot(ElementName = "retEvento")]
    public class RetEvento
    {
        #region Public Properties

        [XmlElement(ElementName = "infEvento")]
        public InfEvento InfEvento { get; set; }

        [XmlText]
        public string Text { get; set; }

        [XmlAttribute(AttributeName = "versao")]
        public string Versao { get; set; }

        #endregion Public Properties
    }
}