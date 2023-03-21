using System.Collections.Generic;
using Extensions;

namespace HES.Modelo
{
    public class TransportadoraViewModel : EmpresaViewModel
    {
        public static readonly Dictionary<int, string> ModalidadesFrete = new Dictionary<int, string>()
        {
            {0, "Por conta Remetente"},
            {1, "Por conta Destinatário"},
            {2, "Por conta Terceiros"},
            {3, "Próprio, por conta Rem."},
            {4, "Próprio, por conta Dest."},
            {9, "Sem Transporte"}
        };

        /// <summary>
        /// <para>Modalidade do frete.</para>
        /// <para>Tag modFrete</para>
        /// </summary>
        public int ModalidadeFrete
        {
            get => this[nameof(ModalidadeFrete)]?.ToInt() ?? 0;
            set => this[nameof(ModalidadeFrete)] = $"{value}".RemoveMask();
        }

        /// <summary>
        /// <para>Registro Nacional de Transportador de Carga (ANTT).</para>
        /// <para>Tag RNTC</para>
        /// </summary>
        public string CodigoAntt
        {
            get => this[nameof(CodigoAntt)];
            set => this[nameof(CodigoAntt)] = value;
        }

        /// <summary>
        /// <para>Placa do Veículo.</para>
        /// <para>Tag placa</para>
        /// </summary>
        public string Placa
        {
            get => this[nameof(Placa)];
            set => this[nameof(Placa)] = value;
        }

        /// <summary>
        /// <para>Sigla da UF do Veículo</para>
        /// <para>Tag UF</para>
        /// </summary>
        public string VeiculoUf
        {
            get => this[nameof(VeiculoUf)];
            set => this[nameof(VeiculoUf)] = value;
        }

        /// <summary>
        /// <para>Quantidade de volumes transportados.</para>
        /// <para>Tag qVol</para>
        /// </summary>
        public double? QuantidadeVolumes
        {
            get => this[nameof(QuantidadeVolumes)]?.ToDouble();
            set => this[nameof(QuantidadeVolumes)] = $"{value}".RemoveMask();
        }

        /// <summary>
        /// <para>Espécie dos volumes transportados.</para>
        /// <para>Tag esp</para>
        /// </summary>
        public string Especie
        {
            get => this[nameof(Especie)];
            set => this[nameof(Especie)] = value;
        }

        /// <summary>
        /// <para>Marca dos volumes transportados.</para>
        /// <para>Tag marca</para>
        /// </summary>
        public string Marca
        {
            get => this[nameof(Marca)];
            set => this[nameof(Marca)] = value;
        }

        /// <summary>
        /// <para>Numeração dos volumes transportados.</para>
        /// <para>Tag nVol</para>
        /// </summary>
        public string Numeracao
        {
            get => this[nameof(Numeracao)];
            set => this[nameof(Numeracao)] = value;
        }

        /// <summary>
        /// <para>Peso Líquido (em kg).</para>
        /// <para>Tag pesoL</para>
        /// </summary>
        public double? PesoLiquido
        {
            get => this[nameof(PesoLiquido)]?.ToDouble();
            set => this[nameof(PesoLiquido)] = $"{value}".RemoveMask();
        }

        /// <summary>
        /// <para>Peso Bruto (em kg).</para>
        /// <para>Tag pesoB</para>
        /// </summary>
        public double? PesoBruto
        {
            get => this[nameof(PesoBruto)]?.ToDouble();
            set => this[nameof(PesoBruto)] = $"{value}".RemoveMask();
        }

        public string ModalidadeFreteString
        {
            get
            {
                string result;
                if (ModalidadesFrete.ContainsKey(ModalidadeFrete))
                {
                    result = $"{ModalidadeFrete}-{ModalidadesFrete[ModalidadeFrete]}";
                }
                else
                {
                    result = $"({ModalidadeFrete})";
                }

                return result;
            }
        }
    }
}
