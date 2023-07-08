using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Relatorios.mod_ecom.emailMKT
{
    public static class emailMKT
    {

        public static string MontarCabecalhoAux()
        {
            var texto = new StringBuilder();

            //texto.AppendLine("<!DOCTYPE html>");
            //texto.AppendLine("<html>");
            //texto.AppendLine("<head>");
            //texto.AppendLine("    <title>Email MKT</title>");
            //texto.AppendLine("    <meta charset='utf-8' />");
            //texto.AppendLine("</head>");
            //texto.AppendLine("<body>");
            texto.AppendLine("<table cellspacing='0' cellpadding='0' border='0' width='100%' align='center' bgcolor='#ffffff' class='' style='max-width:656px;width:100%;text-align:center;border:0px; height:100px;'>");
            return texto.ToString();
        }
        public static string MontarRodapeAux()
        {
            var texto = new StringBuilder();
            texto.AppendLine("</table>");
            //texto.AppendLine("</body>");
            //texto.AppendLine("</html>");
            return texto.ToString();
        }


        public static string MontarConteudo(int codigoEmailCorpo, int codigoEmailBloco, string templateHTML, bool htmlParaEmail)
        {
            var texto = "";

            if (codigoEmailBloco == 6 || codigoEmailBloco == 7 || codigoEmailBloco == 8 || codigoEmailBloco == 9)
                texto = MontarEspaco(codigoEmailCorpo, templateHTML, htmlParaEmail);

            else
                texto = MontarProdutos(codigoEmailCorpo, templateHTML, htmlParaEmail);

            return texto;
        }

        public static string MontarEspaco(int codigoEmailCorpo, string templateHTML, bool htmlParaEmail)
        {
            var ecomController = new EcomController();
            var texto = new StringBuilder();

            var corpoLink = ecomController.ObterEmailCorpoLink(codigoEmailCorpo).FirstOrDefault();
            if (corpoLink != null)
            {
                if (!htmlParaEmail)
                    texto.AppendLine(MontarCabecalhoAux());

                texto.AppendLine(templateHTML);

                if (!htmlParaEmail)
                    texto.AppendLine(MontarRodapeAux());
            }

            return texto.ToString();
        }
        //public static string MontarBanner(int codigoEmailCorpo, string templateHTML)
        //{
        //    var ecomController = new EcomController();
        //    var texto = new StringBuilder();

        //    var corpoLink = ecomController.ObterEmailCorpoLink(codigoEmailCorpo).FirstOrDefault();
        //    if (corpoLink != null)
        //    {
        //        texto.AppendLine(MontarCabecalhoAux());

        //        templateHTML = templateHTML.Replace("##LINKBANNER##", corpoLink.LINK_REDIRECT);
        //        templateHTML = templateHTML.Replace("##IMAGEMBANNER##", corpoLink.LINK_IMAGEM);

        //        texto.AppendLine(templateHTML);

        //        texto.AppendLine(MontarRodapeAux());
        //    }

        //    return texto.ToString();
        //}
        //public static string MontarFrase(int codigoEmailCorpo, string templateHTML)
        //{
        //    var ecomController = new EcomController();
        //    var texto = new StringBuilder();

        //    var corpoLink = ecomController.ObterEmailCorpoLink(codigoEmailCorpo).FirstOrDefault();
        //    if (corpoLink != null)
        //    {
        //        texto.AppendLine(MontarCabecalhoAux());

        //        templateHTML = templateHTML.Replace("##FRASE##", corpoLink.TEXTO);
        //        texto.AppendLine(templateHTML);

        //        texto.AppendLine(MontarRodapeAux());
        //    }

        //    return texto.ToString();
        //}
        //public static string MontarCupom(int codigoEmailCorpo, string templateHTML)
        //{
        //    var ecomController = new EcomController();
        //    var texto = new StringBuilder();

        //    var corpoLink = ecomController.ObterEmailCorpoLink(codigoEmailCorpo).FirstOrDefault();
        //    if (corpoLink != null)
        //    {
        //        texto.AppendLine(MontarCabecalhoAux());

        //        templateHTML = templateHTML.Replace("##CUPOM##", corpoLink.TEXTO);
        //        texto.AppendLine(templateHTML);

        //        texto.AppendLine(MontarRodapeAux());
        //    }

        //    return texto.ToString();
        //}
        //public static string MontarTitulo(int codigoEmailCorpo, string templateHTML)
        //{
        //    var ecomController = new EcomController();
        //    var texto = new StringBuilder();

        //    var corpoLink = ecomController.ObterEmailCorpoLink(codigoEmailCorpo).FirstOrDefault();
        //    if (corpoLink != null)
        //    {
        //        texto.AppendLine(MontarCabecalhoAux());

        //        templateHTML = templateHTML.Replace("##TITULO##", corpoLink.TEXTO);
        //        texto.AppendLine(templateHTML);

        //        texto.AppendLine(MontarRodapeAux());
        //    }

        //    return texto.ToString();
        //}
        public static string MontarProdutos(int codigoEmailCorpo, string templateHTML, bool htmlParaEmail)
        {
            var ecomController = new EcomController();
            var texto = new StringBuilder();

            var corpoLink = ecomController.ObterEmailCorpoLink(codigoEmailCorpo);

            if (!htmlParaEmail)
                texto.AppendLine(MontarCabecalhoAux());

            int i = 1;
            foreach (var link in corpoLink)
            {
                templateHTML = templateHTML.Replace("##LINKPRODUTO" + i.ToString() + "##", link.LINK_REDIRECT);
                templateHTML = templateHTML.Replace("##IMAGEMPRODUTO" + i.ToString() + "##", link.LINK_IMAGEM);
                templateHTML = templateHTML.Replace("##ALTPRODUTO" + i.ToString() + "##", link.ALT);

                //titulo
                templateHTML = templateHTML.Replace("##TITULO##", link.TEXTO);
                //cupom
                templateHTML = templateHTML.Replace("##CUPOM##", link.TEXTO);
                //frase
                templateHTML = templateHTML.Replace("##FRASE##", link.TEXTO);
                //banner
                templateHTML = templateHTML.Replace("##LINKBANNER##", link.LINK_REDIRECT);
                templateHTML = templateHTML.Replace("##IMAGEMBANNER##", link.LINK_IMAGEM);

                templateHTML = templateHTML.Replace("##BOTAOTEXTO##", link.TEXTO);
                templateHTML = templateHTML.Replace("##BOTAOLINK##", link.LINK_REDIRECT);


                if (link.PRECO != null)
                    templateHTML = templateHTML.Replace("##PRECOPRODUTO" + i.ToString() + "##", Convert.ToDecimal(link.PRECO).ToString("###,###,##0.00"));
                if (link.PRECO_PROMO != null)
                    templateHTML = templateHTML.Replace("##PRECOPROMOPRODUTO" + i.ToString() + "##", Convert.ToDecimal(link.PRECO_PROMO).ToString("###,###,##0.00"));

                i = i + 1;
            }
            texto.AppendLine(templateHTML);

            if (!htmlParaEmail)
                texto.AppendLine(MontarRodapeAux());

            return texto.ToString();
        }


    }
}