using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using InnerLibs;
using org.pdfclown.documents.contents.composition;

namespace HESDanfe
{
    internal static class Utils
    {
        #region Private Fields

        private const float PointFactor = 72F / 25.4F;

        #endregion Private Fields

        #region Internal Fields

        internal const string BairroDistrito = "Bairro / Distrito";

        internal const string Cep = "CEP";

        internal const string CnpjCpf = "CNPJ / CPF";

        internal const string Endereco = "Endereço";

        internal const string FoneFax = "Fone / Fax";

        internal const string FormatoMoeda = "#,0.00##";

        internal const string FormatoNumero = "#,0.####";

        internal const string FormatoNumeroNF = @"000\.000\.000";

        internal const string InscricaoEstadual = "Inscrição Estadual";

        internal const string Municipio = "Município";

        internal const string NomeRazaoSocial = "Nome / Razão Social";

        internal const string Quantidade = "Quantidade";

        internal const string RazaoSocial = "Razão Social";

        internal const string TextoConsulta = "Consulta de autenticidade no portal nacional da NF-e www.nfe.fazenda.gov.br/portal ou no site da Sefaz Autorizadora";

        internal const string UF = "UF";

        #endregion Internal Fields

        #region Internal Methods

        internal static Estilo CriarEstilo(float tFonteCampoCabecalho = 6, float tFonteCampoConteudo = 10) => new Estilo(DANFE.FonteRegular, DANFE.FonteNegrito, DANFE.FonteItalico, tFonteCampoCabecalho, tFonteCampoConteudo);

        internal static string Formatar(this double number, string formato = FormatoMoeda) => number.ToString(formato, Cultura);

        internal static string Formatar(this int number, string formato = FormatoMoeda) => number.ToString(formato, Cultura);

        internal static string Formatar(this int? number, string formato = FormatoMoeda) => number.HasValue ? number.Value.Formatar(formato) : string.Empty;

        internal static string Formatar(this double? number, string formato = FormatoMoeda) => number.HasValue ? number.Value.Formatar(formato) : string.Empty;

        internal static string Formatar(this DateTime? dateTime) => dateTime.HasValue ? dateTime.Value.ToString("dd/MM/yyyy") : string.Empty;

        internal static string Formatar(this TimeSpan? timeSpan) => timeSpan.HasValue ? timeSpan.Value.ToString() : string.Empty;

        internal static string FormatarChaveAcesso(string chaveAcesso) => Regex.Replace(chaveAcesso, ".{4}", "$0 ").TrimEnd();

        internal static string FormatarDataHora(this DateTime? dateTime) => dateTime.HasValue ? dateTime.Value.ToString("dd/MM/yyyy HH:mm:ss") : string.Empty;

        internal static string FormatarMoeda(this double? number) => number.HasValue ? number.Value.ToString("C", Cultura) : string.Empty;

        internal static string GenerateLicenseKey(this string productIdentifier)
        {
            Encoder enc = Encoding.Unicode.GetEncoder();
            byte[] unicodeText = new byte[productIdentifier.Length * 2];
            enc.GetBytes(productIdentifier.ToCharArray(), 0, productIdentifier.Length, unicodeText, 0, true);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(unicodeText);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                sb.Append(result[i].ToString("X2"));
            }

            productIdentifier = sb.ToString().Substring(0, 28).ToUpper();
            char[] serialArray = productIdentifier.ToCharArray();
            StringBuilder licenseKey = new StringBuilder();

            int j;
            for (int i = 0; i < 28; i++)
            {
                for (j = i; j < 4 + i; j++)
                {
                    licenseKey.Append(serialArray[j]);
                }
                if (j == 28)
                {
                    break;
                }
                else
                {
                    i = (j) - 1;
                    licenseKey.Append("-");
                }
            }
            return licenseKey.ToString();
        }

        internal static AssemblyName GetAssemblyName() => Assembly.GetEntryAssembly()?.GetName() ?? Assembly.GetExecutingAssembly()?.GetName();

        internal static string GetCompanyName()
        {
            var versionInfo = FileVersionInfo.GetVersionInfo((Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly()).Location);
            var c = versionInfo?.CompanyName;
            return c.IfBlank("H&S Technologies");

        }

        #endregion Internal Methods

        #region Public Fields

        public const float A4Altura = 297;

        public const float A4Largura = 210;

        /// <summary>
        /// Altura do campo em milímetros.
        /// </summary>
        public const float CampoAltura = 6.75F;

        /// <summary>
        /// Cultura pt-BR
        /// </summary>
        public static readonly CultureInfo Cultura = new CultureInfo(1046);

        #endregion Public Fields

        #region Public Constructors

        static Utils()
        {
            Cultura.NumberFormat.CurrencyPositivePattern = 2;
            Cultura.NumberFormat.CurrencyNegativePattern = 9;
        }

        #endregion Public Constructors

        #region Public Methods

        public static StringBuilder AppendChaveValor(this StringBuilder sb, string chave, string valor)
        {
            if (sb.Length > 0) sb.Append(' ');
            return sb.Append(chave).Append(": ").Append(valor);
        }

        public static RectangleF CutBottom(this RectangleF r, float height) => new RectangleF(r.X, r.Y, r.Width, r.Height - height);

        public static RectangleF CutLeft(this RectangleF r, float width) => new RectangleF(r.X + width, r.Y, r.Width - width, r.Height);

        public static RectangleF CutTop(this RectangleF r, float height) => new RectangleF(r.X, r.Y + height, r.Width, r.Height - height);

        public static RectangleF InflatedRetangle(this RectangleF rect, float top, float button, float horizontal) => new RectangleF(rect.X + horizontal, rect.Y + top, rect.Width - 2 * horizontal, rect.Height - top - button);

        public static RectangleF InflatedRetangle(this RectangleF rect, float value) => rect.InflatedRetangle(value, value, value);

        /// <summary>
        /// Verifica se uma string contém outra string no formato chave: valor.
        /// </summary>
        public static bool StringContemChaveValor(this string str, string chave, string valor)
        {
            if (Ext.IsBlank(chave)) throw new ArgumentException(nameof(chave));
            if (Ext.IsBlank(str)) return false;

            return Regex.IsMatch(str, $@"({chave}):?\s*{valor}", RegexOptions.IgnoreCase);
        }

        public static string TipoDFeDeChaveAcesso(this string chaveAcesso)
        {
            if (Ext.IsBlank(chaveAcesso)) throw new ArgumentException(nameof(chaveAcesso));

            if (chaveAcesso.Length == 44)
            {
                string f = chaveAcesso.Substring(20, 2);

                if (f == "55") return "NF-e";
                else if (f == "57") return "CT-e";
                else if (f == "65") return "NFC-e";
            }

            return "DF-e Desconhecido";
        }

        /// <summary>
        /// Converts Point to Millimeters
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static float ToMm(this float point) => point / PointFactor;

        /// <summary>
        /// Converts Point to Millimeters
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static SizeF ToMm(this SizeF s) => new SizeF(s.Width.ToMm(), s.Height.ToMm());

        /// <summary>
        /// Converts Point to Millimeters
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static double ToMm(this double point) => point / PointFactor;

        public static XAlignmentEnum ToPdfClownAlignment(this AlinhamentoHorizontal ah)
        {
            switch (ah)
            {
                case AlinhamentoHorizontal.Centro:
                    return XAlignmentEnum.Center;

                case AlinhamentoHorizontal.Direita:
                    return XAlignmentEnum.Right;

                case AlinhamentoHorizontal.Esquerda:
                default:
                    return XAlignmentEnum.Left;
            }

            throw new InvalidOperationException();
        }

        public static YAlignmentEnum ToPdfClownAlignment(this AlinhamentoVertical av)
        {
            switch (av)
            {
                case AlinhamentoVertical.Topo:
                    return YAlignmentEnum.Top;

                case AlinhamentoVertical.Centro:
                    return YAlignmentEnum.Middle;

                case AlinhamentoVertical.Base:
                    return YAlignmentEnum.Bottom;
            }

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Converts Millimeters to Point
        /// </summary>
        /// <param name="mm"></param>
        /// <returns></returns>
        public static float ToPoint(this float mm) => PointFactor * mm;

        /// <summary>
        /// Converts Millimeters to Point
        /// </summary>
        /// <param name="mm"></param>
        /// <returns></returns>
        public static double ToPoint(this double mm) => PointFactor * mm;

        /// <summary>
        /// Converts Point to Millimeters
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static SizeF ToPointMeasure(this SizeF s) => new SizeF(s.Width.ToPoint(), s.Height.ToPoint());

        public static RectangleF ToPointMeasure(this RectangleF r) => new RectangleF(r.X.ToPoint(), r.Y.ToPoint(), r.Width.ToPoint(), r.Height.ToPoint());

        public static PointF ToPointMeasure(this PointF r) => new PointF(r.X.ToPoint(), r.Y.ToPoint());

        #endregion Public Methods
    }
}