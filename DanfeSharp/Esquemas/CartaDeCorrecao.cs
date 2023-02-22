using System;
using System.Xml.Serialization;
using HESDanfe.Esquemas.NFe;

namespace HESDanfe.Esquemas
{
    // using System.Xml.Serialization;
    // XmlSerializer serializer = new XmlSerializer(typeof(ProcEventoNFe));
    // using (StringReader reader = new StringReader(xml))
    // {
    //    var test = (ProcEventoNFe)serializer.Deserialize(reader);
    // }

    [XmlRoot(ElementName = "detEvento")]
    public class DetEvento
    {

        [XmlElement(ElementName = "descEvento")]
        public string DescEvento { get; set; }

        [XmlElement(ElementName = "xCorrecao")]
        public string XCorrecao { get; set; }

        [XmlElement(ElementName = "xCondUso")]
        public string XCondUso { get; set; }

        [XmlAttribute(AttributeName = "versao")]
        public double Versao { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "infEvento")]
    public class InfEvento
    {

        [XmlElement(ElementName = "cOrgao")]
        public int COrgao { get; set; }

        [XmlElement(ElementName = "tpAmb")]
        public TAmb TpAmb { get; set; }

        [XmlElement(ElementName = "CNPJ")]
        public double CNPJ { get; set; }

        [XmlElement(ElementName = "chNFe")]
        public double ChNFe { get; set; }

        [XmlElement(ElementName = "dhEvento")]
        public DateTime DhEvento { get; set; }

        [XmlElement(ElementName = "tpEvento")]
        public int TpEvento { get; set; }

        [XmlElement(ElementName = "nSeqEvento")]
        public int NSeqEvento { get; set; }

        [XmlElement(ElementName = "verEvento")]
        public double VerEvento { get; set; }

        [XmlElement(ElementName = "detEvento")]
        public DetEvento DetEvento { get; set; }

        [XmlAttribute(AttributeName = "Id")]
        public string Id { get; set; }

        [XmlText]
        public string Text { get; set; }

        [XmlElement(ElementName = "verAplic")]
        public string VerAplic { get; set; }

        [XmlElement(ElementName = "cStat")]
        public int CStat { get; set; }

        [XmlElement(ElementName = "xMotivo")]
        public string XMotivo { get; set; }

        [XmlElement(ElementName = "xEvento")]
        public string XEvento { get; set; }

        [XmlElement(ElementName = "CNPJDest")]
        public string CNPJDest { get; set; }

        [XmlElement(ElementName = "dhRegEvento")]
        public DateTime DhRegEvento { get; set; }

        [XmlElement(ElementName = "nProt")]
        public double NProt { get; set; }

        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
    }

    [XmlRoot(ElementName = "evento")]
    public class Evento
    {

        [XmlElement(ElementName = "infEvento")]
        public InfEvento InfEvento { get; set; }

        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }

        [XmlAttribute(AttributeName = "versao")]
        public double Versao { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlType(Namespace = Namespaces.NFe)]
    [XmlRoot(ElementName = "retEvento")]
    public class RetEvento
    {

        [XmlElement(ElementName = "infEvento")]
        public InfEvento InfEvento { get; set; }



        [XmlAttribute(AttributeName = "versao")]
        public double Versao { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(Namespace = "http://www.portalfiscal.inf.br/nfe", ElementName = "procEventoNFe", DataType = "string", IsNullable = true)]
    public class ProcEventoNFe
    {

        [XmlElement(ElementName = "evento")]
        public Evento Evento { get; set; }

        [XmlElement(ElementName = "retEvento")]
        public RetEvento RetEvento { get; set; }

        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }

        [XmlAttribute(AttributeName = "versao")]
        public double Versao { get; set; }

        [XmlText]
        public string Text { get; set; }
    }


}
