using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace HESDanfe
{
    internal static class Utils
    {
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




        /// <summary>
        /// Verifica se uma string contém outra string no formato chave: valor.
        /// </summary>
        public static bool StringContemChaveValor(this string str, string chave, string valor)
        {
            if (string.IsNullOrWhiteSpace(chave)) throw new ArgumentException(nameof(chave));
            if (string.IsNullOrWhiteSpace(str)) return false;

            return Regex.IsMatch(str, $@"({chave}):?\s*{valor}", RegexOptions.IgnoreCase);
        }





        public static string TipoDFeDeChaveAcesso(this string chaveAcesso)
        {
            if (string.IsNullOrWhiteSpace(chaveAcesso)) throw new ArgumentException(nameof(chaveAcesso));

            if (chaveAcesso.Length == 44)
            {
                string f = chaveAcesso.Substring(20, 2);

                if (f == "55") return "NF-e";
                else if (f == "57") return "CT-e";
                else if (f == "65") return "NFC-e";
            }

            return "DF-e Desconhecido";
        }
    }
}
