using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace Relatorios.utils
{
    public static class StringUtil
    {
        public static string RemoverAcento(this string text)
        {
            StringBuilder sbReturn = new StringBuilder();
            var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();
            foreach (char letter in arrayText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    sbReturn.Append(letter);
            }
            return sbReturn.ToString();
        }


        public static string ObterMes(int mes)
        {
            var mesAux = "";

            switch (mes)
            {
                case 1:
                    mesAux = "Janeiro";
                    break;
                case 2:
                    mesAux = "Fevereiro";
                    break;
                case 3:
                    mesAux = "Março";
                    break;
                case 4:
                    mesAux = "Abril";
                    break;
                case 5:
                    mesAux = "Maio";
                    break;
                case 6:
                    mesAux = "Junho";
                    break;
                case 7:
                    mesAux = "Julho";
                    break;
                case 8:
                    mesAux = "Agosto";
                    break;
                case 9:
                    mesAux = "Setembro";
                    break;
                case 10:
                    mesAux = "Outubro";
                    break;
                case 11:
                    mesAux = "Novembro";
                    break;
                case 12:
                    mesAux = "Dezembro";
                    break;
            }

            return mesAux;
        }



    }
}