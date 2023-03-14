using Extensions.BR;
using Extensions.Locations;

namespace HES.Modelo
{
    public class LocalEntregaRetiradaViewModel : AddressInfo
    {




        public string NomeRazaoSocial
        {
            get => this[nameof(NomeRazaoSocial)];
            set => this[nameof(NomeRazaoSocial)] = PropCleaner(value);
        }

        public string CnpjCpf
        {
            get => this[nameof(CnpjCpf)].FormatarCPFOuCNPJ();
            set => this[nameof(CnpjCpf)] = PropCleaner(value);
        }

        public string Telefone
        {
            get => this[nameof(Telefone)].FormatarTelefone();
            set => this[nameof(Telefone)] = PropCleaner(value);
        }

        public string InscricaoEstadual
        {
            get => this[nameof(InscricaoEstadual)];
            set => this[nameof(InscricaoEstadual)] = PropCleaner(value);
        }





    }
}
