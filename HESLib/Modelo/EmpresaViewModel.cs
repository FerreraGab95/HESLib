using Extensions.BR;
using Extensions.Locations;

namespace HES.Modelo
{
    public class EmpresaViewModel : AddressInfo
    {


        /// <summary>
        /// <para>Razão Social ou Nome</para>
        /// <para>Tag xNome</para>
        /// </summary>
        public string RazaoSocial
        {
            get => this[nameof(RazaoSocial)];
            set => this[nameof(RazaoSocial)] = value;
        }

        /// <summary>
        /// <para>Nome fantasia</para>
        /// <para>Tag xFant</para>
        /// </summary>

        public string NomeFantasia
        {
            get => Label;
            set => Label = value;
        }
        /// <summary>
        /// <para>Telefone</para>
        /// <para>Tag fone</para>
        /// </summary>
        public string Telefone
        {
            get => this[nameof(Telefone)];
            set => this[nameof(Telefone)] = value?.FormatarTelefone();
        }


        /// <summary>
        /// <para>CNPJ ou CPF</para>
        /// <para>Tag CNPJ ou CPF</para>
        /// </summary>
        public string CnpjCpf
        {
            get => this[nameof(CnpjCpf)];
            set => this[nameof(CnpjCpf)] = value?.FormatarCPFOuCNPJ();
        }


        /// <summary>
        /// <para>Inscrição Estadual</para>
        /// <para>Tag IE</para>
        /// </summary>
        public string Ie
        {
            get => this[nameof(Ie)];
            set => this[nameof(Ie)] = value;
        }

        /// <summary>
        /// <para>IE do Substituto Tributário</para>
        /// <para>Tag IEST</para>
        /// </summary>
        public string IeSt
        {
            get => this[nameof(IeSt)];
            set => this[nameof(IeSt)] = value;
        }

        /// <summary>
        /// <para>Inscrição Municipal</para>
        /// <para>Tag IM</para>
        /// </summary>
        public string IM
        {
            get => this[nameof(IM)];
            set => this[nameof(IM)] = value;
        }

        /// <summary>
        /// <para>Email</para>
        /// <para>Tag email</para>
        /// </summary>
        public string Email
        {
            get => this[nameof(Email)];
            set => this[nameof(Email)] = value;
        }

        /// <summary>
        /// Código de Regime Tributário
        /// </summary>
        public string CRT
        {
            get => this[nameof(CRT)];
            set => this[nameof(CRT)] = value;
        }




    }
}
