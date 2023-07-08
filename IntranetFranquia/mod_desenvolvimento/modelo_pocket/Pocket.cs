using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using DAL;

namespace Relatorios.mod_desenvolvimento.modelo_pocket
{
    public static class Pocket
    {
        public static StringBuilder MontarCabecalho(StringBuilder _texto, bool print)
        {
            _texto.AppendLine("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            _texto.AppendLine("<html>");
            _texto.AppendLine("<head>");
            _texto.AppendLine("    <title>POCKET</title>");
            _texto.AppendLine("    <meta charset='UTF-8'>");
            _texto.AppendLine("    <style type='text/css'>");
            _texto.AppendLine("        @media print");
            _texto.AppendLine("        {");
            _texto.AppendLine("            .background-force");
            _texto.AppendLine("            {");
            _texto.AppendLine("                -webkit-print-color-adjust: exact;");
            _texto.AppendLine("            }");
            _texto.AppendLine("        }");
            _texto.AppendLine("        .break");
            _texto.AppendLine("        {");
            _texto.AppendLine("             page-break-before: always;");
            _texto.AppendLine("        }");
            _texto.AppendLine("    </style>");
            _texto.AppendLine("</head>");
            _texto.AppendLine("<body onload='" + ((print) ? "window.print();" : "") + "' style='padding: 0; margin: 0;'>");
            return _texto;
        }
        public static StringBuilder MontarRodape(StringBuilder _texto)
        {
            _texto.AppendLine("</body>");
            _texto.AppendLine("</html>");
            return _texto;
        }
        public static StringBuilder AbreLinha(StringBuilder _texto, bool pulaLinha, string divNome)
        {
            _texto.AppendLine("    <div id='div" + divNome + "' align='center' class='" + ((pulaLinha) ? "break" : "") + "' style='font: Calibri;'>");
            _texto.AppendLine("        <table border='0' cellpadding='0' cellspacing='0' " + ((divNome.Contains("dHR")) ? "width='100%'" : "") + " style='padding: 0px; color: black;");
            _texto.AppendLine("            font-size: 9.0pt; font-weight: 700; font-family: Calibri; background: white;");
            _texto.AppendLine("            white-space: nowrap;'>");
            _texto.AppendLine("            <tr valign='top'>");

            return _texto;
        }
        public static StringBuilder FechaLinha(StringBuilder _texto)
        {
            _texto.AppendLine("             </tr>");
            _texto.AppendLine("        </table>");
            _texto.AppendLine("    </div>");

            return _texto;
        }
        public static StringBuilder FechaColuna(StringBuilder _texto)
        {
            _texto.AppendLine("                <td valign='top' style='height:670px; width:33%;'>");
            _texto.AppendLine("                     &nbsp;");
            _texto.AppendLine("                </td>");

            return _texto;
        }
        public static StringBuilder HR(StringBuilder _texto)
        {
            _texto.AppendLine("<hr />");
            return _texto;
        }
        public static StringBuilder ColunaVazia(StringBuilder _texto, int colSpan, bool hr, string widthCol = "60")
        {
            _texto.AppendLine("                <td style='width: " + widthCol + "px;' colspan='" + colSpan + "'>");
            _texto.AppendLine("                    " + ((hr) ? "&nbsp;<hr />" : "&nbsp;"));
            _texto.AppendLine("                </td>");
            return _texto;
        }
        private static string QuebraLinha(string cor)
        {
            StringBuilder quebraLinha = new StringBuilder();
            quebraLinha.Append(cor);
            int primeiroEspaco = quebraLinha.ToString().IndexOf(' ');
            if (primeiroEspaco > 3)
            {
                quebraLinha.Remove(primeiroEspaco, 1);
                quebraLinha.Insert(primeiroEspaco, "<br />");
            }
            return quebraLinha.ToString();
        }


        public static StringBuilder MontarPocket(List<DESENV_PRODUTO> produtos, bool gradeReal)
        {
            StringBuilder _texto = new StringBuilder();

            _texto = MontarCabecalho(_texto, false);
            _texto = AbreLinha(_texto, false, "Pocket");
            _texto = MontarConteudoV2(_texto, produtos, gradeReal);
            _texto = FechaLinha(_texto);
            _texto = MontarRodape(_texto);

            return _texto;
        }
        public static StringBuilder MontarConteudoV2(StringBuilder _texto, List<DESENV_PRODUTO> produtos, bool gradeReal)
        {
            DesenvolvimentoController desenvController = new DesenvolvimentoController();
            ProducaoController prodController = new ProducaoController();
            BaseController baseController = new BaseController();


            DESENV_CORE cor = new DESENV_CORE();

            int totalCor = produtos.Count;
            int iColSpan = totalCor;
            int countCor = 0;
            string corTecido = "";

            string nomeProduto = "";
            var produtoLinx = (baseController.BuscaProduto(produtos[0].MODELO.Trim()));
            if (produtoLinx != null)
                nomeProduto = produtoLinx.DESC_PRODUTO.Trim().Replace(produtoLinx.GRUPO_PRODUTO, "");

            //Colecao 19 - INVERNO 2015
            if (nomeProduto != "")
            {
                var produtoModelo = desenvController.ObterProduto(produtos[0].COLECAO).Where(i => i.MODELO.Trim().ToUpper() == produtos[0].MODELO.Trim().ToUpper());
                foreach (DESENV_PRODUTO p in produtoModelo)
                {
                    var nome = desenvController.ObterProdutoHBPorProduto(p.CODIGO);
                    if (nome != null)
                    {
                        nomeProduto = nome.NOME.Trim();
                        break;
                    }
                }
            }
            //**************************************************************************************/

            _texto.AppendLine("                <td valign='top' style='" + ((produtos.Count <= 3) ? "height:670px;" : "") + "'>");
            _texto.AppendLine("                    <div style='font: Calibri; border: 1px solid #696969; " + ((produtos.Count <= 3) ? "height:670px;" : "") + "'>");
            _texto.AppendLine("                        <table border='0' cellpadding='0' cellspacing='0' width='312px' style='padding: 0px;");
            _texto.AppendLine("                            color: black; font-size: 9.0pt; font-weight: 700;'>");
            _texto.AppendLine("                            <tr style='line-height: 17px;'>");
            _texto.AppendLine("                                <td style='text-align:center; border-bottom: 1px solid #696969;' colspan='" + iColSpan + "' valign='middle'>");
            _texto.AppendLine("                                    <span style='font-size: 16px; color:Red;'>" + ((produtos[0].DESENV_PRODUTO_ORIGEM1 == null) ? "&nbsp;&nbsp;" : produtos[0].DESENV_PRODUTO_ORIGEM1.DESCRICAO) + " - " + produtos[0].GRIFFE.Trim() + "</span>");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                            <tr style='line-height: 28px;'>");
            _texto.AppendLine("                                <td style='text-align: " + ((nomeProduto.Trim() == "") ? "left;" : "center;") + " border-bottom: 1px solid #696969;' colspan='" + iColSpan + "' valign='middle'>");
            _texto.AppendLine("                                    <span style='font-size: 21px;'>&nbsp;&nbsp;" + produtos[0].GRUPO.Trim().ToUpper() + " " + produtos[0].MODELO.Trim().ToUpper() + " " + nomeProduto.Replace(produtos[0].GRUPO.Trim().ToUpper(), "") + "</span>");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                            <tr style='line-height: 28px; border-top: 1px solid #696969;'>");
            _texto.AppendLine("                                <td style='text-align: center;' colspan='" + iColSpan + "' valign='middle'>");
            _texto.AppendLine("                                    <span style='font-size: 20px; color: Red;'>" + ((produtos[0].TECIDO_POCKET != null && produtos[0].TECIDO_POCKET.Trim() != "") ? produtos[0].TECIDO_POCKET.Trim().ToUpper() : "&nbsp;&nbsp;") + "</span>");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                            <tr>");
            _texto.AppendLine("                                <td colspan='" + iColSpan + "' align='center' valign='middle'>");
            _texto.AppendLine("                                    <div style='width: 300px; height: 320px; display: table-cell; vertical-align: middle; text-align: center;'>");
            _texto.AppendLine("                                         <img alt='' src='../.." + ((produtos[0].FOTO != null && produtos[0].FOTO.Trim() != "") ? produtos[0].FOTO.Replace("~", "") : "/") + "' />");
            if (produtos[0].FOTO2 != null && produtos[0].FOTO2.Trim() != "")
            {
                _texto.Append("                                         <img alt='' src='../.." + ((produtos[0].FOTO2 != null && produtos[0].FOTO2.Trim() != "") ? produtos[0].FOTO2.Replace("~", "") : "/") + "' />");
            }
            _texto.AppendLine("                                    </div>");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                            <tr style='line-height: 9px;'>");
            _texto.AppendLine("                                <td colspan='" + iColSpan + "' align='center'>");
            _texto.AppendLine("                                    &nbsp;");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");

            _texto.AppendLine("                            <tr>");
            _texto.AppendLine("                                <td colspan='" + iColSpan + "' style='text-align: center; line-height: 29px; border-bottom: 1px solid #696969;'>");
            _texto.AppendLine("                                    <span style='font-size: 20px; color: Red;'>" + ((produtos[0].REF_MODELAGEM != null && produtos[0].REF_MODELAGEM.Trim() != "") ? produtos[0].REF_MODELAGEM.Trim().ToUpper() : "&nbsp;&nbsp;") + "</span>");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");

            _texto.AppendLine("                            <tr>");
            _texto.AppendLine("                                <td colspan='" + iColSpan + "' style='line-height: 30px; font-size: 12px;'>");
            _texto.AppendLine("                                    <table border='0' width='100%' cellpadding='0' cellspacing='0'>");
            _texto.AppendLine("                                        <tr style='line-height: 30px;'>"); // LOOP
            _texto.AppendLine("                                            <td style='width: 105px; border-bottom: 1px solid #696969; text-align: center;'>");
            _texto.AppendLine("                                                <span style='font-size: 12px;'>" + prodController.ObterMolde(produtos[0].COLECAO.Trim(), produtos[0].GRUPO.Trim(), produtos[0].COR.Trim(), produtos[0].MODELO.Trim()) + "</span>");
            _texto.AppendLine("                                            </td>");

            //QuebraLinha
            string corFornecedor = "";
            foreach (DESENV_PRODUTO p in produtos)
            {
                _texto.AppendLine("                                            <td style='line-height:14px; text-align: center; border-bottom: 1px solid #696969; border-left: 1px solid #696969;'>");

                corFornecedor = ((p.FORNECEDOR_COR != null && p.FORNECEDOR_COR.Trim() != "") ? p.FORNECEDOR_COR.ToUpper().Trim() : "SEM COR");
                if (totalCor == 1)
                    _texto.AppendLine("                                                <span style='font-size: 12px;'>" + ((corFornecedor.Length <= 32) ? corFornecedor : corFornecedor.Substring(0, 32)) + "</span>");
                if (totalCor == 2)
                    _texto.AppendLine("                                                <span style='font-size: 11px;'>" + ((corFornecedor.Length > 18) ? QuebraLinha(corFornecedor) : corFornecedor) + "</span>");
                if (totalCor == 3)
                    _texto.AppendLine("                                                <span style='font-size: 10px;'>" + ((corFornecedor.Length > 10) ? QuebraLinha(corFornecedor) : corFornecedor) + "</span>");

                _texto.AppendLine("                                            </td>");
            }
            _texto.AppendLine("                                        </tr>");

            int widthMostruario = 0;
            if (totalCor == 2)
                widthMostruario = 102;
            else if (totalCor == 3)
                widthMostruario = 68;

            /*COR FORNECEDOR*/
            _texto.AppendLine("                                        <tr style='line-height: 28px;'>");
            _texto.AppendLine("                                            <td style='width: 105px; text-align: right; border-bottom: 1px solid #696969;'>");
            _texto.AppendLine("                                                <span style='font-size: 16px;'>FORNECEDOR&nbsp;&nbsp;</span>");
            _texto.AppendLine("                                            </td>");

            foreach (DESENV_PRODUTO p in produtos)
            {
                cor = desenvController.ObterCorPocket(p.FORNECEDOR_COR);
                if (cor != null)
                    corTecido = cor.IMAGEM;

                _texto.AppendLine("                                            <td class='background-force' style='background: url(../.." + corTecido.Replace("~", "") + "); border-bottom: 1px solid #696969; border-left: 1px solid #696969; " + ((widthMostruario > 0) ? "width:" + widthMostruario.ToString() + "px;" : "") + "'>");
                _texto.AppendLine("                                                 &nbsp;");
                _texto.AppendLine("                                            </td>");
            }
            _texto.Append("                                        </tr>");
            /**/

            /*COR LINX*/
            _texto.AppendLine("                                        <tr style='line-height: 28px;'>");
            _texto.AppendLine("                                            <td style='width: 105px; text-align: right; border-bottom: 1px solid #696969;'>");
            _texto.AppendLine("                                                <span style='font-size: 16px;'>COR LINX&nbsp;&nbsp;</span>");
            _texto.AppendLine("                                            </td>");

            //QuebraLinha
            string corLinx = "";
            foreach (DESENV_PRODUTO p in produtos)
            {
                _texto.Append("                                            <td style='line-height:14px; text-align: center; border-bottom: 1px solid #696969; border-left: 1px solid #696969;'>");

                corLinx = ((p.COR != null && p.COR.Trim() != "") ? prodController.ObterCoresBasicas(p.COR.Trim()).DESC_COR.Trim() : "SEM COR");
                if (totalCor == 1)
                    _texto.AppendLine("                                                <span style='font-size: 12px;'>" + ((corLinx.Length <= 32) ? corLinx : corLinx.Substring(0, 32)) + "</span>");
                if (totalCor == 2)
                    _texto.AppendLine("                                                <span style='font-size: 11px;'>" + ((corLinx.Length > 18) ? QuebraLinha(corLinx) : corLinx) + "</span>");
                if (totalCor == 3)
                    _texto.AppendLine("                                                <span style='font-size: 10px;'>" + ((corLinx.Length > 10) ? QuebraLinha(corLinx) : corLinx) + "</span>");

                _texto.AppendLine("                                            </td>");
            }
            _texto.AppendLine("                                        </tr>");

            /**/
            /*MOSTRUARIO*/
            _texto.AppendLine("                                        <tr style='line-height: 28px;'>");
            _texto.AppendLine("                                            <td style='width: 105px; text-align: right; border-bottom: 1px solid #696969;'>");
            _texto.AppendLine("                                                <span style='font-size: 16px;'>&nbsp;&nbsp;</span>");
            _texto.AppendLine("                                            </td>");
            foreach (DESENV_PRODUTO p in produtos)
            {
                _texto.AppendLine("                                            <td style='border-bottom: 1px solid #696969; border-left: 1px solid #696969; " + ((widthMostruario > 0) ? "width:" + widthMostruario.ToString() + "px;" : "") + "'>");
                _texto.AppendLine("                                                 &nbsp;");
                _texto.AppendLine("                                            </td>");
            }
            _texto.AppendLine("                                        </tr>");
            /**/
            _texto.AppendLine("                                    </table>");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                            <tr>");
            _texto.AppendLine("                                <td colspan='" + iColSpan + "'>");
            _texto.AppendLine("                                    <table border='0' width='100%' cellpadding='0' cellspacing='0'>");
            _texto.AppendLine("                                        <tr style='line-height: 21px;'>");
            _texto.AppendLine("                                            <td style='text-align: right; width: 105px; border-bottom: 1px solid #696969; border-right: 1px solid #696969;'>");
            _texto.AppendLine("                                                <span style='font-size: 12px;'>PRODUÇÃO&nbsp;&nbsp;</span>");
            _texto.AppendLine("                                            </td>");
            _texto.AppendLine("                                            <td style='text-align: center; width: 68px; border-bottom: 1px solid #696969; border-right: 1px solid #696969;'>");
            _texto.AppendLine("                                                <span style='font-size: 12px;'>ATACADO</span>");
            _texto.AppendLine("                                            </td>");
            _texto.AppendLine("                                            <td style='text-align: center; width: 68px; border-bottom: 1px solid #696969; border-right: 1px solid #696969;'>");
            _texto.AppendLine("                                                <span style='font-size: 12px;'>VAREJO</span>");
            _texto.AppendLine("                                            </td>");
            _texto.AppendLine("                                            <td style='text-align: center; border-bottom: 1px solid #696969;'>");
            _texto.AppendLine("                                                <span style='font-size: 12px;'>PETIT</span>");
            _texto.AppendLine("                                            </td>");
            _texto.AppendLine("                                        </tr>");

            if (!gradeReal)
            {
                foreach (DESENV_PRODUTO p in produtos)
                {

                    var produtoFoiLoja = baseController.ObterProdutoDataFoiLoja(p.MODELO, p.COR);
                    if (produtoFoiLoja != null)
                    {
                        _texto.AppendLine("                                        <tr style='line-height: 22px; background-color:#F9FF8C;'>"); //AMARELO
                    }
                    else
                    {

                        if (p.PRODUTO_ACABADO == 'N')
                        {
                            var producaoHB = prodController.ObterHB(p.COLECAO.Trim(), p.MODELO.Trim(), p.COR.Trim(), 'N');
                            if (producaoHB != null)
                            {
                                _texto.AppendLine("                                        <tr style='line-height: 22px; background-color:#98FB98;'>"); //VERDE CORTADO
                            }
                            else
                            {
                                _texto.AppendLine("                                        <tr style='line-height: 22px;'>"); // BRANCO
                            }
                        }
                        else
                        {

                            var produtoEntregue = desenvController.VerificarProdutoComprasLinxEntregue(p.MODELO, p.COR);
                            if (produtoEntregue)
                            {
                                _texto.AppendLine("                                        <tr style='line-height: 22px; background-color:#98FB98;'>"); //VERDE ENTREGUE
                            }
                            else
                            {
                                _texto.AppendLine("                                        <tr style='line-height: 22px;'>"); // BRANCO
                            }
                        }

                    }

                    _texto.AppendLine("                                            <td style='text-align: right; border-bottom: 1px solid #696969; border-right: 1px solid #696969;'>");
                    _texto.AppendLine("                                                <span style='font-size: 13px;'>" + ((p.FORNECEDOR_COR != null && p.FORNECEDOR_COR.Trim() != "") ? p.FORNECEDOR_COR.Trim().ToUpper().Substring(0, (p.FORNECEDOR_COR.Trim().Length > 12) ? 12 : p.FORNECEDOR_COR.Trim().Length) : "SEM COR") + "&nbsp;&nbsp;</span>");
                    _texto.AppendLine("                                            </td>");
                    _texto.AppendLine("                                            <td style='text-align: center; border-bottom: 1px solid #696969; border-right: 1px solid #696969;'>");
                    _texto.AppendLine("                                                <span style='font-size: 16px;'>" + p.QTDE_ATACADO.ToString() + "</span>");
                    _texto.AppendLine("                                            </td>");
                    _texto.AppendLine("                                            <td style='text-align: center; border-bottom: 1px solid #696969; border-right: 1px solid #696969;'>");
                    _texto.AppendLine("                                                <span style='font-size: 16px;'>" + p.QTDE.ToString() + "</span>");
                    _texto.AppendLine("                                            </td>");
                    _texto.AppendLine("                                            <td style='text-align: center; border-bottom: 1px solid #696969;'>");
                    _texto.AppendLine("                                                <span style='font-size: 13px;'>" + ((p.CORTE_PETIT == null) ? "&nbsp;" : p.CORTE_PETIT.ToString()) + "</span>");
                    _texto.AppendLine("                                            </td>");
                    _texto.AppendLine("                                        </tr>");
                }
            }
            else
            {
                foreach (DESENV_PRODUTO p in produtos)
                {
                    //BUSCAR GRADE REAL - COLECAO, PRODUTO, COR (SUM)
                    var grade = prodController.ObterGradeRealProduto(p.COLECAO, p.MODELO, p.COR, 'N');
                    if (grade != null && (grade.QTDE_ATACADO > 0 || grade.QTDE_VAREJO > 0))
                    {
                        _texto.AppendLine("                                        <tr style='line-height: 22px; background-color:#98FB98;'>");
                    }
                    else
                    {
                        _texto.AppendLine("                                        <tr style='line-height: 22px;'>");
                    }

                    //_texto.AppendLine("                                        <tr style='line-height: 22px;'>"); // LOOP
                    _texto.AppendLine("                                            <td style='text-align: right; border-bottom: 1px solid #696969; border-right: 1px solid #696969;'>");
                    _texto.AppendLine("                                                <span style='font-size: 13px;'>" + ((p.FORNECEDOR_COR != null && p.FORNECEDOR_COR.Trim() != "") ? p.FORNECEDOR_COR.Trim().ToUpper().Substring(0, (p.FORNECEDOR_COR.Trim().Length > 12) ? 12 : p.FORNECEDOR_COR.Trim().Length) : "SEM COR") + "&nbsp;&nbsp;</span>");
                    _texto.AppendLine("                                            </td>");
                    _texto.AppendLine("                                            <td style='text-align: center; border-bottom: 1px solid #696969; border-right: 1px solid #696969;'>");
                    _texto.AppendLine("                                                <span style='font-size: 16px;'>" + ((grade != null) ? grade.QTDE_ATACADO : 0).ToString() + "</span>");
                    _texto.AppendLine("                                            </td>");
                    _texto.AppendLine("                                            <td style='text-align: center; border-bottom: 1px solid #696969; border-right: 1px solid #696969;'>");
                    _texto.AppendLine("                                                <span style='font-size: 16px;'>" + ((grade != null) ? grade.QTDE_VAREJO : 0).ToString() + "</span>");
                    _texto.AppendLine("                                            </td>");
                    _texto.AppendLine("                                            <td style='text-align: center; border-bottom: 1px solid #696969;'>");
                    _texto.AppendLine("                                                <span style='font-size: 13px;'>" + ((p.CORTE_PETIT == null) ? "&nbsp;" : p.CORTE_PETIT.ToString()) + "</span>");
                    _texto.AppendLine("                                            </td>");
                    _texto.AppendLine("                                        </tr>");
                }
            }
            countCor = (4 - totalCor);
            for (int i = 0; i < countCor; i++)
            {
                _texto.AppendLine("                                        <tr style='line-height: 21px;'>"); // LOOP
                _texto.AppendLine("                                            <td style='text-align: right; " + ((countCor == (i - 1)) ? "border-bottom: 1px solid #696969;" : "") + "'>");
                _texto.AppendLine("                                                <span style='font-size: 16px;'>&nbsp;</span>");
                _texto.AppendLine("                                            </td>");
                _texto.AppendLine("                                            <td style='text-align: center; " + ((countCor == (i)) ? "border-bottom: 1px solid #696969;" : "") + " '>");
                _texto.AppendLine("                                                <span style='font-size: 16px;'>&nbsp;</span>");
                _texto.AppendLine("                                            </td>");
                _texto.AppendLine("                                            <td style='text-align: center; " + ((countCor == (i)) ? "border-bottom: 1px solid #696969;" : "") + "'>");
                _texto.AppendLine("                                                <span style='font-size: 16px;'>&nbsp;</span>");
                _texto.AppendLine("                                            </td>");
                _texto.AppendLine("                                            <td style='text-align: center; " + ((countCor == (i - 1)) ? "border-bottom: 1px solid #696969;" : "") + "'>");
                _texto.AppendLine("                                                <span style='font-size: 16px;'>&nbsp;</span>");
                _texto.AppendLine("                                            </td>");
                _texto.AppendLine("                                        </tr>");
            }

            _texto.AppendLine("                                    </table>");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                        </table>");
            _texto.AppendLine("                    </div>");
            _texto.AppendLine("                </td>");

            return _texto;
        }
        public static StringBuilder MontarFotoTecido(StringBuilder _texto, string corTecido)
        {
            /*COR*/
            _texto.AppendLine("                                        <tr style='line-height: 70px;'>");
            _texto.AppendLine("                                            <td class='background-force' style='background: url(../.." + corTecido.Replace("~", "") + "); border-bottom: 1px solid #696969; border-left: 1px solid #696969;'>");
            _texto.AppendLine("                                                 &nbsp;");
            _texto.AppendLine("                                            </td>");
            _texto.AppendLine("                                        </tr>");

            return _texto;
        }


        public static StringBuilder MontarPocketHBF(List<DESENV_PRODUTO> produtos, bool print, bool gradeReal)
        {
            StringBuilder _texto = new StringBuilder();

            _texto = MontarCabecalho(_texto, print);
            _texto = AbreLinha(_texto, false, "PocketHBF");
            _texto = MontarConteudoHBF(_texto, produtos, gradeReal);
            _texto = FechaLinha(_texto);
            _texto = MontarRodape(_texto);

            return _texto;
        }
        public static StringBuilder MontarConteudoHBF(StringBuilder _texto, List<DESENV_PRODUTO> produtos, bool gradeReal)
        {
            DesenvolvimentoController desenvController = new DesenvolvimentoController();
            ProducaoController prodController = new ProducaoController();
            BaseController baseController = new BaseController();
            DESENV_CORE cor = new DESENV_CORE();

            int totalCor = produtos.Count;
            int iColSpan = totalCor;
            int countCor = 0;
            string corTecido = "";

            string nomeProduto = "";
            string griffe = produtos[0].GRIFFE.Trim();
            var produtoLinx = (baseController.BuscaProduto(produtos[0].MODELO.Trim()));
            if (produtoLinx != null)
            {
                nomeProduto = produtoLinx.DESC_PRODUTO.Trim().Replace(produtoLinx.GRUPO_PRODUTO, "");
                //griffe = produtoLinx.GRIFFE.Trim();
            }

            //Colecao 19 - INVERNO 2015
            if (nomeProduto != "")
            {
                var produtoModelo = desenvController.ObterProduto(produtos[0].COLECAO).Where(i => i.MODELO.Trim().ToUpper() == produtos[0].MODELO.Trim().ToUpper());
                foreach (DESENV_PRODUTO p in produtoModelo)
                {
                    var nome = desenvController.ObterProdutoHBPorProduto(p.CODIGO);
                    if (nome != null)
                    {
                        nomeProduto = nome.NOME.Trim();
                        break;
                    }
                }
            }
            //**************************************************************************************/

            _texto.AppendLine("                <td valign='top' style='" + ((produtos.Count <= 3) ? "height:670px;" : "") + "'>");
            _texto.AppendLine("                    <div style='font: Calibri; border: 1px solid #696969; " + ((produtos.Count <= 3) ? "height:670px;" : "") + "'>");
            _texto.AppendLine("                        <table border='0' cellpadding='0' cellspacing='0' width='312px' style='padding: 0px;");
            _texto.AppendLine("                            color: black; font-size: 9.0pt; font-weight: 700;'>");
            _texto.AppendLine("                            <tr style='line-height: 17px;'>");
            _texto.AppendLine("                                <td style='text-align:center; border-bottom: 1px solid #696969;' colspan='" + iColSpan + "' valign='middle'>");
            _texto.AppendLine("                                    <span style='font-size: 16px; color:Red;'>" + ((produtos[0].DESENV_PRODUTO_ORIGEM1 == null) ? "&nbsp;&nbsp;" : produtos[0].DESENV_PRODUTO_ORIGEM1.DESCRICAO) + " - " + produtos[0].GRIFFE.Trim() + "</span>");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                            <tr style='line-height: 28px;'>");
            _texto.AppendLine("                                <td style='text-align: " + ((nomeProduto.Trim() == "") ? "left;" : "center;") + " border-bottom: 1px solid #696969;' colspan='" + iColSpan + "' valign='middle'>");
            _texto.AppendLine("                                    <span style='font-size: 21px;'>&nbsp;&nbsp;" + produtos[0].GRUPO.Trim().ToUpper() + " " + produtos[0].MODELO.Trim().ToUpper() + " " + nomeProduto.Replace(produtos[0].GRUPO.Trim().ToUpper(), "") + "</span>");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                            <tr style='line-height: 28px; border-top: 1px solid #696969;'>");
            _texto.AppendLine("                                <td style='text-align: center;' colspan='" + iColSpan + "' valign='middle'>");
            _texto.AppendLine("                                    <span style='font-size: 20px; color: Red;'>" + ((produtos[0].TECIDO_POCKET != null && produtos[0].TECIDO_POCKET.Trim() != "") ? produtos[0].TECIDO_POCKET.Trim().ToUpper() : "&nbsp;&nbsp;") + "</span>");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                            <tr>");
            _texto.AppendLine("                                <td colspan='" + iColSpan + "' align='center' valign='middle'>");
            _texto.AppendLine("                                    <div style='width: 300px; height: 320px; display: table-cell; vertical-align: middle; text-align: center;'>");
            _texto.AppendLine("                                         <img alt='' src='../.." + ((produtos[0].FOTO != null && produtos[0].FOTO.Trim() != "") ? produtos[0].FOTO.Replace("~", "") : "/") + "' />");
            if (produtos[0].FOTO2 != null && produtos[0].FOTO2.Trim() != "")
            {
                _texto.Append("                                         <img alt='' src='../.." + ((produtos[0].FOTO2 != null && produtos[0].FOTO2.Trim() != "") ? produtos[0].FOTO2.Replace("~", "") : "/") + "' />");
            }
            _texto.AppendLine("                                    </div>");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                            <tr style='line-height: 9px;'>");
            _texto.AppendLine("                                <td colspan='" + iColSpan + "' align='center'>");
            _texto.AppendLine("                                    &nbsp;");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");

            _texto.AppendLine("                            <tr>");
            _texto.AppendLine("                                <td colspan='" + iColSpan + "' style='text-align: center; line-height: 29px; border-bottom: 1px solid #696969;'>");
            _texto.AppendLine("                                    <span style='font-size: 20px; color: Red;'>" + ((produtos[0].REF_MODELAGEM != null && produtos[0].REF_MODELAGEM.Trim() != "") ? produtos[0].REF_MODELAGEM.Trim().ToUpper() : "&nbsp;&nbsp;") + "</span>");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");

            _texto.AppendLine("                            <tr>");
            _texto.AppendLine("                                <td colspan='" + iColSpan + "' style='line-height: 30px; font-size: 12px;'>");
            _texto.AppendLine("                                    <table border='0' width='100%' cellpadding='0' cellspacing='0'>");
            _texto.AppendLine("                                        <tr style='line-height: 30px;'>"); // LOOP
            _texto.AppendLine("                                            <td style='width: 105px; text-align: center; border-bottom: 1px solid #696969;'>");
            _texto.AppendLine("                                                <span style='font-size: 11px;'>" + griffe + "</span>");
            _texto.AppendLine("                                            </td>");

            //QuebraLinha
            string corFornecedor = "";
            foreach (DESENV_PRODUTO p in produtos)
            {
                _texto.AppendLine("                                            <td style='line-height:14px; text-align: center; border-bottom: 1px solid #696969; border-left: 1px solid #696969;'>");

                corFornecedor = ((p.FORNECEDOR_COR != null && p.FORNECEDOR_COR.Trim() != "") ? p.FORNECEDOR_COR.ToUpper().Trim() : "SEM COR");
                if (totalCor == 1)
                    _texto.AppendLine("                                                <span style='font-size: 12px;'>" + ((corFornecedor.Length <= 32) ? corFornecedor : corFornecedor.Substring(0, 32)) + "</span>");
                if (totalCor == 2)
                    _texto.AppendLine("                                                <span style='font-size: 11px;'>" + ((corFornecedor.Length > 18) ? QuebraLinha(corFornecedor) : corFornecedor) + "</span>");
                if (totalCor == 3)
                    _texto.AppendLine("                                                <span style='font-size: 10px;'>" + ((corFornecedor.Length > 10) ? QuebraLinha(corFornecedor) : corFornecedor) + "</span>");

                _texto.Append("                                            </td>");
            }
            _texto.AppendLine("                                        </tr>");

            int widthMostruario = 0;
            if (totalCor == 2)
                widthMostruario = 102;
            else if (totalCor == 3)
                widthMostruario = 68;

            /*FORNECEDOR*/
            _texto.AppendLine("                                        <tr style='line-height: 28px;'>");
            _texto.AppendLine("                                            <td style='width: 105px; text-align: right; border-bottom: 1px solid #696969;'>");
            _texto.AppendLine("                                                <span style='font-size: 16px;'>FORNECEDOR&nbsp;&nbsp;</span>");
            _texto.AppendLine("                                            </td>");

            foreach (DESENV_PRODUTO p in produtos)
            {
                cor = desenvController.ObterCorPocket(p.FORNECEDOR_COR);
                if (cor != null)
                    corTecido = cor.IMAGEM;

                _texto.AppendLine("                                            <td class='background-force' style='background: url(../.." + corTecido.Replace("~", "") + "); border-bottom: 1px solid #696969; border-left: 1px solid #696969; " + ((widthMostruario > 0) ? "width:" + widthMostruario.ToString() + "px;" : "") + "'>");
                _texto.AppendLine("                                                 &nbsp;");
                _texto.AppendLine("                                            </td>");
            }
            _texto.AppendLine("                                        </tr>");

            /*COR LINX*/
            _texto.AppendLine("                                        <tr style='line-height: 28px;'>");
            _texto.AppendLine("                                            <td style='width: 105px; text-align: right; border-bottom: 1px solid #696969;'>");
            _texto.AppendLine("                                                <span style='font-size: 16px;'>COR LINX&nbsp;&nbsp;</span>");
            _texto.AppendLine("                                            </td>");

            //QuebraLinha
            string corLinx = "";
            foreach (DESENV_PRODUTO p in produtos)
            {
                _texto.AppendLine("                                            <td style='line-height:14px; text-align: center; border-bottom: 1px solid #696969; border-left: 1px solid #696969;'>");

                corLinx = ((p.COR != null && p.COR.Trim() != "") ? prodController.ObterCoresBasicas(p.COR.Trim()).DESC_COR.Trim() : "SEM COR");
                if (totalCor == 1)
                    _texto.AppendLine("                                                <span style='font-size: 12px;'>" + ((corLinx.Length <= 32) ? corLinx : corLinx.Substring(0, 32)) + "</span>");
                if (totalCor == 2)
                    _texto.AppendLine("                                                <span style='font-size: 11px;'>" + ((corLinx.Length > 18) ? QuebraLinha(corLinx) : corLinx) + "</span>");
                if (totalCor == 3)
                    _texto.AppendLine("                                                <span style='font-size: 10px;'>" + ((corLinx.Length > 10) ? QuebraLinha(corLinx) : corLinx) + "</span>");

                _texto.AppendLine("                                            </td>");
            }
            _texto.AppendLine("                                        </tr>");
            /**/
            /*MOSTRUARIO*/
            _texto.AppendLine("                                        <tr style='line-height: 28px;'>");
            _texto.AppendLine("                                            <td style='width: 105px; text-align: right; border-bottom: 1px solid #696969;'>");
            _texto.AppendLine("                                                <span style='font-size: 16px;'>&nbsp;&nbsp;</span>");
            _texto.AppendLine("                                            </td>");
            foreach (DESENV_PRODUTO p in produtos)
            {
                _texto.AppendLine("                                            <td style='border-bottom: 1px solid #696969; border-left: 1px solid #696969; " + ((widthMostruario > 0) ? "width:" + widthMostruario.ToString() + "px;" : "") + "'>");
                _texto.AppendLine("                                                 &nbsp;");
                _texto.AppendLine("                                            </td>");
            }
            _texto.AppendLine("                                        </tr>");
            /**/
            _texto.AppendLine("                                    </table>");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                            <tr>");
            _texto.AppendLine("                                <td colspan='" + iColSpan + "'>");
            _texto.AppendLine("                                    <table border='0' width='100%' cellpadding='0' cellspacing='0'>");
            _texto.AppendLine("                                        <tr style='line-height: 21px;'>");
            _texto.AppendLine("                                            <td style='text-align: right; width: 105px; border-bottom: 1px solid #696969; border-right: 1px solid #696969;'>");
            _texto.AppendLine("                                                <span style='font-size: 12px;'>PRODUÇÃO&nbsp;&nbsp;</span>");
            _texto.AppendLine("                                            </td>");
            _texto.AppendLine("                                            <td style='text-align: center; width: 68px; border-bottom: 1px solid #696969; border-right: 1px solid #696969;'>");
            _texto.AppendLine("                                                <span style='font-size: 12px;'>ATACADO</span>");
            _texto.AppendLine("                                            </td>");
            _texto.AppendLine("                                            <td style='text-align: center; width: 68px; border-bottom: 1px solid #696969; border-right: 1px solid #696969;'>");
            _texto.AppendLine("                                                <span style='font-size: 12px;'>VAREJO</span>");
            _texto.AppendLine("                                            </td>");
            _texto.AppendLine("                                            <td style='text-align: center; border-bottom: 1px solid #696969;'>");
            _texto.AppendLine("                                                <span style='font-size: 12px;'>PETIT</span>");
            _texto.AppendLine("                                            </td>");
            _texto.AppendLine("                                        </tr>");

            if (!gradeReal)
            {
                foreach (DESENV_PRODUTO p in produtos)
                {
                    _texto.AppendLine("                                        <tr style='line-height: 22px;'>"); // LOOP
                    _texto.AppendLine("                                            <td style='text-align: right; border-bottom: 1px solid #696969; border-right: 1px solid #696969;'>");
                    _texto.AppendLine("                                                <span style='font-size: 13px;'>" + ((p.FORNECEDOR_COR != null && p.FORNECEDOR_COR.Trim() != "") ? p.FORNECEDOR_COR.Trim().ToUpper().Substring(0, (p.FORNECEDOR_COR.Trim().Length > 12) ? 12 : p.FORNECEDOR_COR.Trim().Length) : "SEM COR") + "&nbsp;&nbsp;</span>");
                    _texto.AppendLine("                                            </td>");
                    _texto.AppendLine("                                            <td style='text-align: center; border-bottom: 1px solid #696969; border-right: 1px solid #696969;'>");
                    _texto.AppendLine("                                                <span style='font-size: 16px;'>" + p.QTDE_ATACADO.ToString() + "</span>");
                    _texto.AppendLine("                                            </td>");
                    _texto.AppendLine("                                            <td style='text-align: center; border-bottom: 1px solid #696969; border-right: 1px solid #696969;'>");
                    _texto.AppendLine("                                                <span style='font-size: 16px;'>" + p.QTDE.ToString() + "</span>");
                    _texto.AppendLine("                                            </td>");
                    _texto.AppendLine("                                            <td style='text-align: center; border-bottom: 1px solid #696969;'>");
                    _texto.AppendLine("                                                <span style='font-size: 13px;'>" + ((p.CORTE_PETIT == null) ? "&nbsp;" : p.CORTE_PETIT.ToString()) + "</span>");
                    _texto.AppendLine("                                            </td>");
                    _texto.AppendLine("                                        </tr>");
                }
            }
            else
            {
                foreach (DESENV_PRODUTO p in produtos)
                {
                    //BUSCAR GRADE REAL - COLECAO, PRODUTO, COR (SUM)
                    var grade = prodController.ObterGradeRealProduto(p.COLECAO, p.MODELO, p.COR, 'N');

                    _texto.AppendLine("                                        <tr style='line-height: 22px;'>"); // LOOP
                    _texto.AppendLine("                                            <td style='text-align: right; border-bottom: 1px solid #696969; border-right: 1px solid #696969;'>");
                    _texto.AppendLine("                                                <span style='font-size: 13px;'>" + ((p.FORNECEDOR_COR != null && p.FORNECEDOR_COR.Trim() != "") ? p.FORNECEDOR_COR.Trim().ToUpper().Substring(0, (p.FORNECEDOR_COR.Trim().Length > 12) ? 12 : p.FORNECEDOR_COR.Trim().Length) : "SEM COR") + "&nbsp;&nbsp;</span>");
                    _texto.AppendLine("                                            </td>");
                    _texto.AppendLine("                                            <td style='text-align: center; border-bottom: 1px solid #696969; border-right: 1px solid #696969;'>");
                    _texto.AppendLine("                                                <span style='font-size: 16px;'>" + ((grade != null) ? grade.QTDE_ATACADO : 0).ToString() + "</span>");
                    _texto.AppendLine("                                            </td>");
                    _texto.AppendLine("                                            <td style='text-align: center; border-bottom: 1px solid #696969; border-right: 1px solid #696969;'>");
                    _texto.AppendLine("                                                <span style='font-size: 16px;'>" + ((grade != null) ? grade.QTDE_VAREJO : 0).ToString() + "</span>");
                    _texto.AppendLine("                                            </td>");
                    _texto.AppendLine("                                            <td style='text-align: center; border-bottom: 1px solid #696969;'>");
                    _texto.AppendLine("                                                <span style='font-size: 13px;'>" + ((p.CORTE_PETIT == null) ? "&nbsp;" : p.CORTE_PETIT.ToString()) + "</span>");
                    _texto.AppendLine("                                            </td>");
                    _texto.AppendLine("                                        </tr>");
                }

            }

            countCor = (4 - totalCor);
            for (int i = 0; i < countCor; i++)
            {
                _texto.Append("                                        <tr style='line-height: 21px;'>"); // LOOP
                _texto.AppendLine("                                            <td style='text-align: right; " + ((countCor == (i - 1)) ? "border-bottom: 1px solid #696969;" : "") + "'>");
                _texto.AppendLine("                                                <span style='font-size: 16px;'>&nbsp;</span>");
                _texto.AppendLine("                                            </td>");
                _texto.AppendLine("                                            <td style='text-align: center; " + ((countCor == (i)) ? "border-bottom: 1px solid #696969;" : "") + " '>");
                _texto.AppendLine("                                                <span style='font-size: 16px;'>&nbsp;</span>");
                _texto.AppendLine("                                            </td>");
                _texto.AppendLine("                                            <td style='text-align: center; " + ((countCor == (i)) ? "border-bottom: 1px solid #696969;" : "") + "'>");
                _texto.AppendLine("                                                <span style='font-size: 16px;'>&nbsp;</span>");
                _texto.AppendLine("                                            </td>");
                _texto.AppendLine("                                            <td style='text-align: center; " + ((countCor == (i - 1)) ? "border-bottom: 1px solid #696969;" : "") + "'>");
                _texto.AppendLine("                                                <span style='font-size: 16px;'>&nbsp;</span>");
                _texto.AppendLine("                                            </td>");
                _texto.AppendLine("                                        </tr>");
            }






            _texto.AppendLine("                                    </table>");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                        </table>");
            _texto.AppendLine("                    </div>");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("                <td>");
            _texto.AppendLine("                    &nbsp;&nbsp;&nbsp;&nbsp;");
            _texto.AppendLine("                </td>");


            _texto.AppendLine("            <td valign='bottom' style='border: 1px solid #696969;'>");
            _texto.AppendLine("                    <table border='0' cellpadding='0' cellspacing='16' width='670' style='padding: 0px; color: black; font-size: 12.0pt; font-weight: 700;' height='" + ((produtos[0].GRIFFE.Trim() == "TRIYOU") ? "180px" : "430px") + "'>");
            _texto.AppendLine("                        <tr>");
            _texto.AppendLine("                            <td valign='top'>");
            _texto.AppendLine("                                <table border='0' cellspacing='0' width='100%' style='padding: 0px; color: black; font-size: 12.0pt; font-weight: 700;'>");
            _texto.AppendLine("                                    <tr>");
            _texto.AppendLine("                                        <td>");
            _texto.AppendLine("                                " + ((produtos[0].OBS_IMPRESSAO == null) ? "" : produtos[0].OBS_IMPRESSAO.Replace("\r", "<br/>").Replace("\n", "<br/>")));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                </table>");
            _texto.AppendLine("");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                    </table>");
            _texto.AppendLine("                    <table border='0' cellpadding='0' cellspacing='0' width='670' style='padding: 0px; color: black; font-size: 12.0pt; font-weight: 700;'>");

            if (produtos[0].GRIFFE.Trim() == "TRIYOU")
            {
                _texto.AppendLine("                        <tr>");
                _texto.AppendLine("                            <td>");
                _texto.AppendLine("                                <table cellspacing='0' width='100%' style='padding: 0px; color: black; border: 1px solid #696969; font-size: 12.0pt; font-weight: 700;'>");
                _texto.AppendLine("                                    <tr>");
                _texto.AppendLine("                                        <td colspan='4' style='border: 1px solid #696969; text-align:center;'>XP &nbsp;&nbsp;&nbsp;&nbsp;AVIAMENTOS</td>");
                _texto.AppendLine("                                        <td style='border: 1px solid #696969; width:100px;'>&nbsp;&nbsp;Liberado Por</td>");
                _texto.AppendLine("                                    </tr>");
                _texto.AppendLine("                                    <tr>");
                _texto.AppendLine("                                        <td style='border: 1px solid #696969;'>&nbsp;&nbsp;Descrição</td>");
                _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px; text-align:center;'>&nbsp;&nbsp;Cor</td>");
                _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px;text-align:center;'>&nbsp;&nbsp;Qtde</td>");
                _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px;text-align:center;'>&nbsp;&nbsp;TAM</td>");
                _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px;text-align:left;'>&nbsp;1-</td>");
                _texto.AppendLine("                                    </tr>");
                _texto.AppendLine("                                    <tr>");
                _texto.AppendLine("                                        <td style='border: 1px solid #696969;'>&nbsp;</td>");
                _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px; text-align:center;'>&nbsp;</td>");
                _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px;text-align:center;'>&nbsp;</td>");
                _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px;text-align:center;'>&nbsp;</td>");
                _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px;text-align:left;'>&nbsp;2-</td>");
                _texto.AppendLine("                                    </tr>");
                _texto.AppendLine("                                    <tr>");
                _texto.AppendLine("                                        <td style='border: 1px solid #696969;'>&nbsp;</td>");
                _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px; text-align:center;'>&nbsp;</td>");
                _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px;text-align:center;'>&nbsp;</td>");
                _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px;text-align:center;'>&nbsp;</td>");
                _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px;text-align:center;'>&nbsp;Data</td>");
                _texto.AppendLine("                                    </tr>");
                _texto.AppendLine("                                    <tr>");
                _texto.AppendLine("                                        <td style='border: 1px solid #696969;'>&nbsp;</td>");
                _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px; text-align:center;'>&nbsp;</td>");
                _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px;text-align:center;'>&nbsp;</td>");
                _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px;text-align:center;'>&nbsp;</td>");
                _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px;text-align:center;'>&nbsp;</td>");
                _texto.AppendLine("                                    </tr>");
                _texto.AppendLine("                                    <tr>");
                _texto.AppendLine("                                        <td style='border: 1px solid #696969;'>&nbsp;</td>");
                _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px; text-align:center;'>&nbsp;</td>");
                _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px;text-align:center;'>&nbsp;</td>");
                _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px;text-align:center;'>&nbsp;</td>");
                _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px;text-align:center;'>Costureira</td>");
                _texto.AppendLine("                                    </tr>");
                _texto.AppendLine("                                    <tr>");
                _texto.AppendLine("                                        <td style='border: 1px solid #696969;'>&nbsp;</td>");
                _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px; text-align:center;'>&nbsp;</td>");
                _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px;text-align:center;'>&nbsp;</td>");
                _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px;text-align:center;'>&nbsp;</td>");
                _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px;text-align:left;'>&nbsp;1-</td>");
                _texto.AppendLine("                                    </tr>");
                _texto.AppendLine("                                    <tr>");
                _texto.AppendLine("                                        <td style='border: 1px solid #696969;'>&nbsp;</td>");
                _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px; text-align:center;'>&nbsp;</td>");
                _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px;text-align:center;'>&nbsp;</td>");
                _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px;text-align:center;'>&nbsp;</td>");
                _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px;text-align:left;'>&nbsp;2-</td>");
                _texto.AppendLine("                                    </tr>");
                _texto.AppendLine("                                    <tr>");
                _texto.AppendLine("                                        <td style='border: 1px solid #696969;'>&nbsp;</td>");
                _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px; text-align:center;'>&nbsp;</td>");
                _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px;text-align:center;'>&nbsp;</td>");
                _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px;text-align:center;'>&nbsp;</td>");
                _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px;text-align:left;'>&nbsp;3-</td>");
                _texto.AppendLine("                                    </tr>");
                _texto.AppendLine("                                    <tr>");
                _texto.AppendLine("                                        <td style='border: 1px solid #696969;'>&nbsp;</td>");
                _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px; text-align:center;'>&nbsp;</td>");
                _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px;text-align:center;'>&nbsp;</td>");
                _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px;text-align:center;'>&nbsp;</td>");
                _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px;text-align:left;'>&nbsp;4-</td>");
                _texto.AppendLine("                                    </tr>");
                _texto.AppendLine("                                </table>");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                        </tr>");
                _texto.AppendLine("                        <tr><td>&nbsp;</td></tr>");
            }


            _texto.AppendLine("                        <tr>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                <table cellspacing='0' width='100%' style='padding: 0px; color: black; border: 1px solid #696969; font-size: 12.0pt; font-weight: 700;'>");
            _texto.AppendLine("                                    <tr>");
            _texto.AppendLine("                                        <td colspan='4' style='border: 1px solid #696969; text-align:center;'>" + ((produtos[0].GRIFFE.Trim() == "TRIYOU") ? "PP" : "") + " &nbsp;&nbsp;&nbsp;&nbsp;AVIAMENTOS</td>");
            _texto.AppendLine("                                        <td style='border: 1px solid #696969; width:100px;'>&nbsp;&nbsp;Liberado Por</td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr>");
            _texto.AppendLine("                                        <td style='border: 1px solid #696969;'>&nbsp;&nbsp;Descrição</td>");
            _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px; text-align:center;'>&nbsp;&nbsp;Cor</td>");
            _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px;text-align:center;'>&nbsp;&nbsp;Qtde</td>");
            _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px;text-align:center;'>&nbsp;&nbsp;TAM</td>");
            _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px;text-align:left;'>&nbsp;1-</td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr>");
            _texto.AppendLine("                                        <td style='border: 1px solid #696969;'>&nbsp;</td>");
            _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px; text-align:center;'>&nbsp;</td>");
            _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px;text-align:center;'>&nbsp;</td>");
            _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px;text-align:center;'>&nbsp;</td>");
            _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px;text-align:left;'>&nbsp;2-</td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr>");
            _texto.AppendLine("                                        <td style='border: 1px solid #696969;'>&nbsp;</td>");
            _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px; text-align:center;'>&nbsp;</td>");
            _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px;text-align:center;'>&nbsp;</td>");
            _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px;text-align:center;'>&nbsp;</td>");
            _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px;text-align:center;'>&nbsp;Data</td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr>");
            _texto.AppendLine("                                        <td style='border: 1px solid #696969;'>&nbsp;</td>");
            _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px; text-align:center;'>&nbsp;</td>");
            _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px;text-align:center;'>&nbsp;</td>");
            _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px;text-align:center;'>&nbsp;</td>");
            _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px;text-align:center;'>&nbsp;</td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr>");
            _texto.AppendLine("                                        <td style='border: 1px solid #696969;'>&nbsp;</td>");
            _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px; text-align:center;'>&nbsp;</td>");
            _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px;text-align:center;'>&nbsp;</td>");
            _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px;text-align:center;'>&nbsp;</td>");
            _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px;text-align:center;'>Costureira</td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr>");
            _texto.AppendLine("                                        <td style='border: 1px solid #696969;'>&nbsp;</td>");
            _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px; text-align:center;'>&nbsp;</td>");
            _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px;text-align:center;'>&nbsp;</td>");
            _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px;text-align:center;'>&nbsp;</td>");
            _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px;text-align:left;'>&nbsp;1-</td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr>");
            _texto.AppendLine("                                        <td style='border: 1px solid #696969;'>&nbsp;</td>");
            _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px; text-align:center;'>&nbsp;</td>");
            _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px;text-align:center;'>&nbsp;</td>");
            _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px;text-align:center;'>&nbsp;</td>");
            _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px;text-align:left;'>&nbsp;2-</td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr>");
            _texto.AppendLine("                                        <td style='border: 1px solid #696969;'>&nbsp;</td>");
            _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px; text-align:center;'>&nbsp;</td>");
            _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px;text-align:center;'>&nbsp;</td>");
            _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px;text-align:center;'>&nbsp;</td>");
            _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px;text-align:left;'>&nbsp;3-</td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr>");
            _texto.AppendLine("                                        <td style='border: 1px solid #696969;'>&nbsp;</td>");
            _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px; text-align:center;'>&nbsp;</td>");
            _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px;text-align:center;'>&nbsp;</td>");
            _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px;text-align:center;'>&nbsp;</td>");
            _texto.AppendLine("                                        <td style='border: 1px solid #696969; width: 100px;text-align:left;'>&nbsp;4-</td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                </table>");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                    </table>");
            _texto.AppendLine("                </td>");



            return _texto;
        }


        public static StringBuilder MontarPocketAcessorio(List<DESENV_ACESSORIO> acessorios, bool print)
        {
            StringBuilder _texto = new StringBuilder();

            _texto = MontarCabecalho(_texto, false);
            _texto = AbreLinha(_texto, false, "Pocket");
            _texto = MontarConteudoAcessorio(_texto, acessorios);
            _texto = FechaLinha(_texto);
            _texto = MontarRodape(_texto);

            return _texto;
        }
        public static StringBuilder MontarConteudoAcessorio(StringBuilder _texto, List<DESENV_ACESSORIO> acessorios)
        {
            DesenvolvimentoController desenvController = new DesenvolvimentoController();
            ProducaoController prodController = new ProducaoController();
            DESENV_CORE cor = new DESENV_CORE();

            int totalCor = acessorios.Count;
            int iColSpan = totalCor;
            int countCor = 0;

            string nomeProduto = "";
            var produtoLinx = (new BaseController().BuscaProduto(acessorios[0].PRODUTO.Trim()));
            if (produtoLinx != null)
                nomeProduto = produtoLinx.DESC_PRODUTO.Trim().Replace(produtoLinx.GRUPO_PRODUTO, "");

            //**************************************************************************************/

            _texto.AppendLine("                <td valign='top' style='" + ((acessorios.Count <= 3) ? "height:670px;" : "") + "'>");
            _texto.AppendLine("                    <div style='font: Calibri; border: 1px solid #696969; " + ((acessorios.Count <= 3) ? "height:670px;" : "") + "'>");
            _texto.AppendLine("                        <table border='0' cellpadding='0' cellspacing='0' width='312px' style='padding: 0px;");
            _texto.AppendLine("                            color: black; font-size: 9.0pt; font-weight: 700;'>");
            _texto.AppendLine("                            <tr style='line-height: 17px;'>");
            _texto.AppendLine("                                <td style='text-align:center; border-bottom: 1px solid #696969;' colspan='" + iColSpan + "' valign='middle'>");
            _texto.AppendLine("                                    <span style='font-size: 16px; color:Red;'>" + ((acessorios[0].DESENV_PRODUTO_ORIGEM1 == null) ? "&nbsp;&nbsp;" : acessorios[0].DESENV_PRODUTO_ORIGEM1.DESCRICAO) + "</span>");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                            <tr style='line-height: 28px;'>");
            _texto.AppendLine("                                <td style='text-align: " + ((nomeProduto.Trim() == "") ? "left;" : "center;") + " border-bottom: 1px solid #696969;' colspan='" + iColSpan + "' valign='middle'>");
            _texto.AppendLine("                                    <span style='font-size: 21px;'>&nbsp;&nbsp;" + acessorios[0].GRUPO.Trim().ToUpper() + " " + acessorios[0].PRODUTO.Trim().ToUpper() + " " + nomeProduto.Replace(acessorios[0].GRUPO.Trim().ToUpper(), "") + "</span>");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                            <tr style='line-height: 28px; border-top: 1px solid #696969;'>");
            _texto.AppendLine("                                <td style='text-align: center;' colspan='" + iColSpan + "' valign='middle'>");
            _texto.AppendLine("                                    <span style='font-size: 20px; color: Red;'>&nbsp;</span>");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                            <tr>");
            _texto.AppendLine("                                <td colspan='" + iColSpan + "' align='center' valign='middle'>");
            _texto.AppendLine("                                    <div style='width: 300px; height: 320px; display: table-cell; vertical-align: middle; text-align: center;'>");
            _texto.AppendLine("                                         <img alt='' src='../.." + ((acessorios[0].FOTO1 != null && acessorios[0].FOTO1.Trim() != "") ? acessorios[0].FOTO1.Replace("~", "") : "/") + "' />");
            if (acessorios[0].FOTO2 != null && acessorios[0].FOTO2.Trim() != "")
            {
                _texto.AppendLine("                                         <img alt='' src='../.." + ((acessorios[0].FOTO2 != null && acessorios[0].FOTO2.Trim() != "") ? acessorios[0].FOTO2.Replace("~", "") : "/") + "' />");
            }
            _texto.AppendLine("                                    </div>");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                            <tr style='line-height: 9px;'>");
            _texto.AppendLine("                                <td colspan='" + iColSpan + "' align='center'>");
            _texto.AppendLine("                                    &nbsp;");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");

            _texto.AppendLine("                            <tr>");
            _texto.AppendLine("                                <td colspan='" + iColSpan + "' style='text-align: center; line-height: 29px; border-bottom: 1px solid #696969;'>");
            _texto.AppendLine("                                    <span style='font-size: 20px; color: Red;'>" + acessorios[0].FORNECEDOR + "</span>");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");

            _texto.AppendLine("                            <tr>");
            _texto.AppendLine("                                <td colspan='" + iColSpan + "' style='line-height: 30px; font-size: 12px;'>");
            _texto.AppendLine("                                    <table border='0' width='100%' cellpadding='0' cellspacing='0'>");
            _texto.AppendLine("                                        <tr style='line-height: 30px;'>"); // LOOP
            _texto.AppendLine("                                            <td style='width: 105px; border-bottom: 1px solid #696969;'>");
            _texto.AppendLine("                                                &nbsp;");
            _texto.AppendLine("                                            </td>");

            //QuebraLinha
            string corFornecedor = "";
            foreach (DESENV_ACESSORIO acess in acessorios)
            {
                _texto.AppendLine("                                            <td style='line-height:14px; text-align: center; border-bottom: 1px solid #696969; border-left: 1px solid #696969;'>");

                //corFornecedor = ((acess.COR_FORNECEDOR != null && acess.COR_FORNECEDOR.Trim() != "") ? acess.COR_FORNECEDOR.ToUpper().Trim() : "SEM COR");
                if (totalCor == 1)
                    _texto.AppendLine("                                                <span style='font-size: 12px;'>" + ((corFornecedor.Length <= 32) ? corFornecedor : corFornecedor.Substring(0, 32)) + "</span>");
                if (totalCor == 2)
                    _texto.AppendLine("                                                <span style='font-size: 11px;'>" + ((corFornecedor.Length > 18) ? QuebraLinha(corFornecedor) : corFornecedor) + "</span>");
                if (totalCor == 3)
                    _texto.AppendLine("                                                <span style='font-size: 10px;'>" + ((corFornecedor.Length > 10) ? QuebraLinha(corFornecedor) : corFornecedor) + "</span>");

                _texto.AppendLine("                                            </td>");
            }
            _texto.AppendLine("                                        </tr>");

            int widthMostruario = 0;
            if (totalCor == 2)
                widthMostruario = 102;
            else if (totalCor == 3)
                widthMostruario = 68;

            /*COR FORNECEDOR*/
            _texto.AppendLine("                                        <tr style='line-height: 28px;'>");
            _texto.AppendLine("                                            <td style='width: 105px; text-align: right; border-bottom: 1px solid #696969;'>");
            _texto.AppendLine("                                                <span style='font-size: 16px;'>&nbsp;</span>");
            _texto.AppendLine("                                            </td>");

            foreach (DESENV_ACESSORIO acess in acessorios)
            {

                _texto.AppendLine("                                            <td class='background-force' style='border-bottom: 1px solid #696969; border-left: 1px solid #696969; " + ((widthMostruario > 0) ? "width:" + widthMostruario.ToString() + "px;" : "") + "'>");
                _texto.AppendLine("                                                 &nbsp;");
                _texto.AppendLine("                                            </td>");
            }
            _texto.AppendLine("                                        </tr>");
            /**/

            /*COR LINX*/
            _texto.AppendLine("                                        <tr style='line-height: 28px;'>");
            _texto.AppendLine("                                            <td style='width: 105px; text-align: right; border-bottom: 1px solid #696969;'>");
            _texto.AppendLine("                                                <span style='font-size: 16px;'>COR LINX&nbsp;&nbsp;</span>");
            _texto.AppendLine("                                            </td>");

            //QuebraLinha
            string corLinx = "";
            foreach (DESENV_ACESSORIO acess in acessorios)
            {
                _texto.AppendLine("                                            <td style='line-height:14px; text-align: center; border-bottom: 1px solid #696969; border-left: 1px solid #696969;'>");

                corLinx = ((acess.COR != null && acess.COR.Trim() != "") ? prodController.ObterCoresBasicas(acess.COR.Trim()).DESC_COR.Trim() : "SEM COR");
                if (totalCor == 1)
                    _texto.AppendLine("                                                <span style='font-size: 12px;'>" + ((corLinx.Length <= 32) ? corLinx : corLinx.Substring(0, 32)) + "</span>");
                if (totalCor == 2)
                    _texto.AppendLine("                                                <span style='font-size: 11px;'>" + ((corLinx.Length > 18) ? QuebraLinha(corLinx) : corLinx) + "</span>");
                if (totalCor == 3)
                    _texto.AppendLine("                                                <span style='font-size: 10px;'>" + ((corLinx.Length > 10) ? QuebraLinha(corLinx) : corLinx) + "</span>");

                _texto.AppendLine("                                            </td>");
            }
            _texto.AppendLine("                                        </tr>");

            /**/
            /*MOSTRUARIO*/
            _texto.AppendLine("                                        <tr style='line-height: 28px;'>");
            _texto.AppendLine("                                            <td style='width: 105px; text-align: right; border-bottom: 1px solid #696969;'>");
            _texto.AppendLine("                                                <span style='font-size: 16px;'>&nbsp;&nbsp;</span>");
            _texto.AppendLine("                                            </td>");
            foreach (DESENV_ACESSORIO acess in acessorios)
            {
                _texto.AppendLine("                                            <td style='border-bottom: 1px solid #696969; border-left: 1px solid #696969; " + ((widthMostruario > 0) ? "width:" + widthMostruario.ToString() + "px;" : "") + "'>");
                _texto.AppendLine("                                                 &nbsp;");
                _texto.AppendLine("                                            </td>");
            }
            _texto.AppendLine("                                        </tr>");
            /**/
            _texto.AppendLine("                                    </table>");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                            <tr>");
            _texto.AppendLine("                                <td colspan='" + iColSpan + "'>");
            _texto.AppendLine("                                    <table border='0' width='100%' cellpadding='0' cellspacing='0'>");
            _texto.AppendLine("                                        <tr style='line-height: 21px;'>");
            _texto.AppendLine("                                            <td style='text-align: right; width: 105px; border-bottom: 1px solid #696969; border-right: 1px solid #696969;'>");
            _texto.AppendLine("                                                <span style='font-size: 12px;'>PRODUÇÃO&nbsp;&nbsp;</span>");
            _texto.AppendLine("                                            </td>");
            _texto.AppendLine("                                            <td style='text-align: center; width: 68px; border-bottom: 1px solid #696969; border-right: 1px solid #696969;'>");
            _texto.AppendLine("                                                <span style='font-size: 12px;'>&nbsp;</span>");
            _texto.AppendLine("                                            </td>");
            _texto.AppendLine("                                            <td style='text-align: center; width: 68px; border-bottom: 1px solid #696969; border-right: 1px solid #696969;'>");
            _texto.AppendLine("                                                <span style='font-size: 12px;'>QTDE</span>");
            _texto.AppendLine("                                            </td>");
            _texto.AppendLine("                                            <td style='text-align: center; border-bottom: 1px solid #696969;'>");
            _texto.AppendLine("                                                <span style='font-size: 12px;'>&nbsp;</span>");
            _texto.AppendLine("                                            </td>");
            _texto.AppendLine("                                        </tr>");

            int qtde = 0;
            foreach (DESENV_ACESSORIO acess in acessorios)
            {
                corLinx = ((acess.COR != null && acess.COR.Trim() != "") ? prodController.ObterCoresBasicas(acess.COR.Trim()).DESC_COR.Trim() : "SEM COR");
                qtde = desenvController.ObterAcessorioPedido(acess.CODIGO.ToString()).Where(p => p.PEDIDO != null && p.PEDIDO.Trim() != "").Sum(p => p.QTDE_ORIGINAL);

                _texto.AppendLine("                                        <tr style='line-height: 22px;'>"); // LOOP
                _texto.AppendLine("                                            <td style='text-align: right; border-bottom: 1px solid #696969; border-right: 1px solid #696969;'>");
                _texto.AppendLine("                                                <span style='font-size: 13px;'>" + ((corLinx != null && corLinx.Trim() != "") ? corLinx.Trim().ToUpper().Substring(0, (corLinx.Trim().Length > 12) ? 12 : corLinx.Trim().Length) : "SEM COR") + "&nbsp;&nbsp;</span>");
                _texto.AppendLine("                                            </td>");
                _texto.AppendLine("                                            <td style='text-align: center; border-bottom: 1px solid #696969; border-right: 1px solid #696969;'>");
                _texto.AppendLine("                                                <span style='font-size: 16px;'>&nbsp;</span>");
                _texto.AppendLine("                                            </td>");
                _texto.AppendLine("                                            <td style='text-align: center; border-bottom: 1px solid #696969; border-right: 1px solid #696969;'>");
                //                _texto.Append("                                                <span style='font-size: 16px;'>" + qtde.ToString() + "</span>");
                _texto.AppendLine("                                                <span style='font-size: 16px;'>" + acess.QTDE.ToString() + "</span>");
                _texto.AppendLine("                                            </td>");
                _texto.AppendLine("                                            <td style='text-align: center; border-bottom: 1px solid #696969;'>");
                _texto.AppendLine("                                                <span style='font-size: 13px;'>&nbsp;</span>");
                _texto.AppendLine("                                            </td>");
                _texto.AppendLine("                                        </tr>");
            }


            countCor = (4 - totalCor);
            for (int i = 0; i < countCor; i++)
            {
                _texto.AppendLine("                                        <tr style='line-height: 21px;'>"); // LOOP
                _texto.AppendLine("                                            <td style='text-align: right; " + ((countCor == (i - 1)) ? "border-bottom: 1px solid #696969;" : "") + "'>");
                _texto.AppendLine("                                                <span style='font-size: 16px;'>&nbsp;</span>");
                _texto.AppendLine("                                            </td>");
                _texto.AppendLine("                                            <td style='text-align: center; " + ((countCor == (i)) ? "border-bottom: 1px solid #696969;" : "") + " '>");
                _texto.AppendLine("                                                <span style='font-size: 16px;'>&nbsp;</span>");
                _texto.AppendLine("                                            </td>");
                _texto.AppendLine("                                            <td style='text-align: center; " + ((countCor == (i)) ? "border-bottom: 1px solid #696969;" : "") + "'>");
                _texto.AppendLine("                                                <span style='font-size: 16px;'>&nbsp;</span>");
                _texto.AppendLine("                                            </td>");
                _texto.AppendLine("                                            <td style='text-align: center; " + ((countCor == (i - 1)) ? "border-bottom: 1px solid #696969;" : "") + "'>");
                _texto.AppendLine("                                                <span style='font-size: 16px;'>&nbsp;</span>");
                _texto.AppendLine("                                            </td>");
                _texto.AppendLine("                                        </tr>");
            }

            _texto.AppendLine("                                    </table>");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                        </table>");
            _texto.AppendLine("                    </div>");
            _texto.AppendLine("                </td>");

            return _texto;
        }

        public static StringBuilder MontarGemea(DESENV_PRODUTO produto)
        {
            DesenvolvimentoController desenvController = new DesenvolvimentoController();
            ProducaoController prodController = new ProducaoController();
            StringBuilder _texto = new StringBuilder();

            var produtos = desenvController.ObterProduto(produto.COLECAO, produto.MODELO);

            string nomeProduto = "";
            var produtoLinx = (new BaseController().BuscaProduto(produtos[0].MODELO.Trim()));
            if (produtoLinx != null)
                nomeProduto = produtoLinx.DESC_PRODUTO.Trim().Replace(produtoLinx.GRUPO_PRODUTO, "");


            _texto.AppendLine("<div id='divGemea' align='center' class='' style='font: Calibri;'>");
            _texto.AppendLine("    <table border='0' cellpadding='0' cellspacing='0' style='padding: 0px; color: black;");
            _texto.AppendLine("        font-size: 9.0pt; font-weight: 700; font-family: Calibri; background: white; white-space: nowrap;'>");
            _texto.AppendLine("        <tr>");
            _texto.AppendLine("            <td valign='top' style='height: 178px; '>");
            _texto.AppendLine("                <div style='font: Calibri; border: 1px solid #696969; height: 180px; '>");
            _texto.AppendLine("                    <table border='0' cellpadding='0' cellspacing='0' width='800px' style='padding: 0px;");
            _texto.AppendLine("                            color: black; font-size: 9.0pt; font-weight: 700;'>");
            _texto.AppendLine("                        <tr>");
            _texto.AppendLine("                            <td colspan='4' style='border-bottom: 1px solid #696969; line-height: 26px;'>");
            _texto.AppendLine("                                &nbsp;&nbsp;<span style='font-size: 21px;'><span style='color:red;'>" + ((produtos[0].DESENV_PRODUTO_ORIGEM1 == null) ? "&nbsp;&nbsp;" : produtos[0].DESENV_PRODUTO_ORIGEM1.DESCRICAO) + " </span>- " + produtos[0].GRUPO.Trim().ToUpper() + " " + produtos[0].MODELO.Trim().ToUpper() + " " + nomeProduto.Replace(produtos[0].GRUPO.Trim().ToUpper(), "") + " - " + ((produtos[0].TECIDO_POCKET != null && produtos[0].TECIDO_POCKET.Trim() != "") ? produtos[0].TECIDO_POCKET.Trim().ToUpper() : "&nbsp;&nbsp;") + "</span>");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                        <tr style='text-align:left; background-color:#f1ecec; line-height:5px;' >");
            _texto.AppendLine("                            <td rowspan='10' style='width:200px;'>");
            _texto.AppendLine("                                 <div style='width: 300px; height: 150px; display: table-cell; vertical-align: middle; text-align: center;'>");
            _texto.AppendLine("                                     <img alt='' src='../.." + ((produtos[0].FOTO != null && produtos[0].FOTO.Trim() != "") ? produtos[0].FOTO.Replace("~", "") : "/") + "' height='150' />");
            _texto.AppendLine("                                 </div>");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-left: 1px solid black; border-bottom: 1px solid black'>");
            _texto.AppendLine("                                &nbsp;COR");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-left: 1px solid black; border-bottom: 1px solid black'>");
            _texto.AppendLine("                                &nbsp;ATACADO");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-left: 1px solid black; border-bottom: 1px solid black'>");
            _texto.AppendLine("                                &nbsp;VAREJO");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");

            string corLinx = "";
            foreach (var p in produtos)
            {
                corLinx = ((p.COR != null && p.COR.Trim() != "") ? prodController.ObterCoresBasicas(p.COR.Trim()).DESC_COR.Trim() : "SEM COR");

                _texto.AppendLine("                        <tr style='text-align:left; line-height:5px;'>");
                _texto.AppendLine("                            <td style='border-left: 1px solid black; border-bottom: 1px solid black'>");
                _texto.AppendLine("                                &nbsp; " + corLinx);
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-left: 1px solid black; border-bottom: 1px solid black'>");
                _texto.AppendLine("                                &nbsp; " + p.QTDE_ATACADO.ToString());
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-left: 1px solid black; border-bottom: 1px solid black'>");
                _texto.AppendLine("                                &nbsp; " + p.QTDE.ToString());
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                        </tr>");
            }

            _texto.AppendLine("                    </table>");
            _texto.AppendLine("                </div>");
            _texto.AppendLine("            </td>");
            _texto.AppendLine("        </tr>");
            _texto.AppendLine("    </table>");
            _texto.AppendLine("</div>");

            return _texto;
        }


        public static StringBuilder MontarConteudoCAL1(StringBuilder _texto, List<DESENV_PRODUTO> produtos, bool gradeReal, string modulo, int painel, int posicaoProduto)
        {
            DesenvolvimentoController desenvController = new DesenvolvimentoController();
            ProducaoController prodController = new ProducaoController();
            BaseController baseController = new BaseController();


            DESENV_CORE cor = new DESENV_CORE();

            int totalCor = 1;
            int iColSpan = 2;
            var corTecido = "";
            // campos usados para auxiliar na montagem do relatorio // gambetas!
            var entradaModulo = produtos[0].GRADE;
            var previsao = produtos[0].LINHA;
            var qtdeFaltante = produtos[0].QTDE_MOSTRUARIO.ToString();
            var fornecedorFaccao = produtos[0].FORNECEDOR.Trim();

            string nomeProduto = "";
            var produtoLinx = (baseController.BuscaProduto(produtos[0].MODELO.Trim()));
            if (produtoLinx != null)
                nomeProduto = produtoLinx.DESC_PRODUTO.Trim().Replace(produtoLinx.GRUPO_PRODUTO, "");

            //**************************************************************************************/

            _texto.AppendLine("                <td valign='top' style='" + ((produtos.Count <= 3) ? "height:730px; width:33%;" : "") + "'>");
            _texto.AppendLine("                    <div style='font: Calibri; border: 1px solid #696969; " + ((produtos.Count <= 3) ? "height:730px;" : "") + "'>");
            _texto.AppendLine("                        <table border='0' cellpadding='0' cellspacing='0' width='312px' style='padding: 0px;");
            _texto.AppendLine("                            color: black; font-size: 9.0pt; font-weight: 700;'>");
            _texto.AppendLine("                            <tr style='line-height: 17px;'>");
            _texto.AppendLine("                                <td style='text-align:left; border-bottom: 1px solid #696969; width:14px;' valign='middle'>");
            _texto.AppendLine("                                    &nbsp;&nbsp;<span style='font-size: 14px; color:gray;'>" + posicaoProduto.ToString() + "</span>");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                                <td style='text-align:center; border-bottom: 1px solid #696969;' valign='middle'>");
            _texto.AppendLine("                                    <span style='font-size: 16px; color:Red; margin-right: 14px;'>" + ((produtos[0].DESENV_PRODUTO_ORIGEM1 == null) ? "&nbsp;&nbsp;" : produtos[0].DESENV_PRODUTO_ORIGEM1.DESCRICAO) + " - " + produtos[0].GRIFFE.Trim() + "</span>");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                            <tr style='line-height: 28px;'>");
            _texto.AppendLine("                                <td style='text-align: " + ((nomeProduto.Trim() == "") ? "left;" : "center;") + " border-bottom: 1px solid #696969;' colspan='" + iColSpan + "' valign='middle'>");

            var tituloProduto = produtos[0].GRUPO.Trim().ToUpper() + " " + produtos[0].MODELO.Trim().ToUpper() + " " + nomeProduto.Replace(produtos[0].GRUPO.Trim().ToUpper(), "");

            _texto.AppendLine("                                    <span style='font-size: " + ((tituloProduto.Length < 19) ? "20" : "16") + "px;'>" + tituloProduto + "</span>");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                            <tr style='line-height: 28px; border-top: 1px solid #696969;'>");
            _texto.AppendLine("                                <td style='text-align: center;' colspan='" + iColSpan + "' valign='middle'>");

            var tecidoPocket = ((produtos[0].TECIDO_POCKET != null && produtos[0].TECIDO_POCKET.Trim() != "") ? produtos[0].TECIDO_POCKET.Trim().ToUpper() : "&nbsp;&nbsp;");

            _texto.AppendLine("                                    <span style='font-size: " + ((tecidoPocket.Length < 20) ? "20" : "14") + "px; color: Red;'>" + tecidoPocket + "</span>");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                            <tr>");
            _texto.AppendLine("                                <td colspan='" + iColSpan + "' align='center' valign='middle' style='height: 320px;'>");
            _texto.AppendLine("                                    <div style='width: 300px; max-height: 320px; display: table-cell; vertical-align: middle; text-align: center;'>");
            _texto.AppendLine("                                         <img alt='" + nomeProduto + "' src='../.." + ((produtos[0].FOTO != null && produtos[0].FOTO.Trim() != "") ? produtos[0].FOTO.Replace("~", "") : "/") + "' />");
            _texto.AppendLine("                                    </div>");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                            <tr style='line-height: 9px;'>");
            _texto.AppendLine("                                <td colspan='" + iColSpan + "' align='center'>");
            _texto.AppendLine("                                    &nbsp;");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");

            _texto.AppendLine("                            <tr>");
            _texto.AppendLine("                                <td colspan='" + iColSpan + "' style='text-align: center; line-height: 29px; border-bottom: 1px solid #696969;'>");

            var modelagem = ((produtos[0].REF_MODELAGEM != null && produtos[0].REF_MODELAGEM.Trim() != "") ? produtos[0].REF_MODELAGEM.Trim().ToUpper() : "&nbsp;&nbsp;");

            _texto.AppendLine("                                    <span style='font-size: " + ((modelagem.Length < 20) ? "20" : "14") + "px; color: Red;'>" + modelagem + "</span>");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");

            _texto.AppendLine("                            <tr>");
            _texto.AppendLine("                                <td colspan='" + iColSpan + "' style='line-height: 30px; font-size: 12px;'>");
            _texto.AppendLine("                                    <table border='0' width='100%' cellpadding='0' cellspacing='0'>");
            _texto.AppendLine("                                        <tr style='line-height: 30px;'>"); // LOOP
            _texto.AppendLine("                                            <td style='width: 105px; border-bottom: 1px solid #696969; text-align: center;'>");
            _texto.AppendLine("                                                <span style='font-size: 12px;'>" + prodController.ObterMolde(produtos[0].COLECAO.Trim(), produtos[0].GRUPO.Trim(), produtos[0].COR.Trim(), produtos[0].MODELO.Trim()) + "</span>");
            _texto.AppendLine("                                            </td>");

            //QuebraLinha
            string corFornecedor = "";
            foreach (DESENV_PRODUTO p in produtos)
            {
                _texto.AppendLine("                                            <td style='line-height:14px; text-align: center; border-bottom: 1px solid #696969; border-left: 1px solid #696969;'>");

                corFornecedor = ((p.FORNECEDOR_COR != null && p.FORNECEDOR_COR.Trim() != "") ? p.FORNECEDOR_COR.ToUpper().Trim() : "SEM COR");
                if (totalCor == 1)
                    _texto.AppendLine("                                                <span style='font-size: 12px;'>" + ((corFornecedor.Length <= 32) ? corFornecedor : corFornecedor.Substring(0, 32)) + "</span>");
                if (totalCor == 2)
                    _texto.AppendLine("                                                <span style='font-size: 11px;'>" + ((corFornecedor.Length > 18) ? QuebraLinha(corFornecedor) : corFornecedor) + "</span>");
                if (totalCor == 3)
                    _texto.AppendLine("                                                <span style='font-size: 10px;'>" + ((corFornecedor.Length > 10) ? QuebraLinha(corFornecedor) : corFornecedor) + "</span>");

                _texto.AppendLine("                                            </td>");
            }
            _texto.AppendLine("                                        </tr>");

            int widthMostruario = 0;
            if (totalCor == 2)
                widthMostruario = 102;
            else if (totalCor == 3)
                widthMostruario = 68;

            /*COR FORNECEDOR*/
            _texto.AppendLine("                                        <tr style='line-height: 28px;'>");
            _texto.AppendLine("                                            <td style='width: 105px; text-align: right; border-bottom: 1px solid #696969;'>");
            _texto.AppendLine("                                                <span style='font-size: 14px;'>FORNECEDOR&nbsp;&nbsp;</span>");
            _texto.AppendLine("                                            </td>");

            foreach (DESENV_PRODUTO p in produtos)
            {
                cor = desenvController.ObterCorPocket(p.FORNECEDOR_COR);
                if (cor != null)
                    corTecido = cor.IMAGEM;

                _texto.AppendLine("                                            <td class='background-force' style='background: url(../.." + corTecido.Replace("~", "") + "); border-bottom: 1px solid #696969; border-left: 1px solid #696969; " + ((widthMostruario > 0) ? "width:" + widthMostruario.ToString() + "px;" : "") + "'>");
                _texto.AppendLine("                                                 &nbsp;");
                _texto.AppendLine("                                            </td>");
            }
            _texto.Append("                                        </tr>");
            /**/

            /*COR LINX*/
            _texto.AppendLine("                                        <tr style='line-height: 28px;'>");
            _texto.AppendLine("                                            <td style='width: 105px; text-align: right; border-bottom: 1px solid #696969;'>");
            _texto.AppendLine("                                                <span style='font-size: 14px;'>COR LINX&nbsp;&nbsp;</span>");
            _texto.AppendLine("                                            </td>");

            //QuebraLinha
            string corLinx = "";
            foreach (DESENV_PRODUTO p in produtos)
            {
                _texto.Append("                                            <td style='line-height:14px; text-align: center; border-bottom: 1px solid #696969; border-left: 1px solid #696969;'>");

                corLinx = ((p.COR != null && p.COR.Trim() != "") ? prodController.ObterCoresBasicas(p.COR.Trim()).DESC_COR.Trim() : "SEM COR");
                if (totalCor == 1)
                    _texto.AppendLine("                                                <span style='font-size: 12px;'>" + ((corLinx.Length <= 32) ? corLinx : corLinx.Substring(0, 32)) + "</span>");
                if (totalCor == 2)
                    _texto.AppendLine("                                                <span style='font-size: 11px;'>" + ((corLinx.Length > 18) ? QuebraLinha(corLinx) : corLinx) + "</span>");
                if (totalCor == 3)
                    _texto.AppendLine("                                                <span style='font-size: 10px;'>" + ((corLinx.Length > 10) ? QuebraLinha(corLinx) : corLinx) + "</span>");

                _texto.AppendLine("                                            </td>");
            }
            _texto.AppendLine("                                        </tr>");

            _texto.AppendLine("                                    </table>");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                            <tr>");
            _texto.AppendLine("                                <td colspan='" + iColSpan + "'>");
            _texto.AppendLine("                                    <table border='0' width='100%' cellpadding='0' cellspacing='0'>");


            if (!gradeReal)
            {
                foreach (DESENV_PRODUTO p in produtos)
                {
                    _texto.AppendLine("                                        <tr style='line-height: 22px;'>"); // BRANCO
                    _texto.AppendLine("                                            <td style='text-align: right; width: 105px; border-bottom: 1px solid #696969; border-right: 1px solid #696969;'>");
                    _texto.AppendLine("                                                <span style='font-size: 13px;'>VAREJO&nbsp;&nbsp;</span>");
                    _texto.AppendLine("                                            </td>");
                    _texto.AppendLine("                                            <td style='text-align: center; border-bottom: 1px solid #696969;'>");
                    _texto.AppendLine("                                                <span style='font-size: 16px;'>" + p.QTDE.ToString() + "</span>");
                    _texto.AppendLine("                                            </td>");
                    _texto.AppendLine("                                        </tr>");
                }
            }
            else
            {
                foreach (DESENV_PRODUTO p in produtos)
                {
                    //BUSCAR GRADE REAL - COLECAO, PRODUTO, COR (SUM)
                    var grade = prodController.ObterGradeRealProduto(p.COLECAO, p.MODELO, p.COR, 'N');

                    _texto.AppendLine("                                        <tr style='line-height: 22px;'>"); // LOOP
                    _texto.AppendLine("                                            <td style='text-align: right; width: 105px; border-bottom: 1px solid #696969; border-right: 1px solid #696969;'>");
                    _texto.AppendLine("                                                <span style='font-size: 13px;'>VAREJO&nbsp;&nbsp;</span>");
                    _texto.AppendLine("                                            </td>");
                    _texto.AppendLine("                                            <td style='text-align: center; border-bottom: 1px solid #696969;'>");
                    _texto.AppendLine("                                                <span style='font-size: 16px;'>" + ((grade != null) ? grade.QTDE_VAREJO : 0).ToString() + "</span>");
                    _texto.AppendLine("                                            </td>");
                    _texto.AppendLine("                                        </tr>");
                }
            }


            _texto.AppendLine("                                        <tr style='line-height: 22px;'>");
            _texto.AppendLine("                                            <td style='text-align: right; border-bottom: 1px solid #696969; border-right: 1px solid #696969;'>");
            _texto.AppendLine("                                                <span style='font-size: 11px;'>TEMPO " + modulo + "&nbsp;&nbsp;</span>");
            _texto.AppendLine("                                            </td>");
            _texto.AppendLine("                                            <td style='text-align: center; border-bottom: 1px solid #696969;'>");
            _texto.AppendLine("                                                <span style='font-size: 13px; color: " + ((entradaModulo.Contains("-")) ? "red" : "black") + ";'>" + entradaModulo + "</span>");
            _texto.AppendLine("                                            </td>");
            _texto.AppendLine("                                        </tr>");

            if (painel == 4)
            {

                _texto.AppendLine("                                        <tr style='line-height: 22px;'>");
                _texto.AppendLine("                                            <td style='text-align: right; border-bottom: 1px solid #696969; border-right: 1px solid #696969;'>");
                _texto.AppendLine("                                                <span style='font-size: 13px;'>FACÇÃO&nbsp;&nbsp;</span>");
                _texto.AppendLine("                                            </td>");
                _texto.AppendLine("                                            <td style='text-align: center; border-bottom: 1px solid #696969;'>");
                _texto.AppendLine("                                                <span style='font-size: " + ((fornecedorFaccao.Length < 19) ? "14" : "10") + "px;'>" + fornecedorFaccao + "</span>");
                _texto.AppendLine("                                            </td>");
                _texto.AppendLine("                                        </tr>");
                _texto.AppendLine("                                        <tr style='line-height: 22px;'>");
                _texto.AppendLine("                                            <td style='text-align: right; border-bottom: 1px solid #696969; border-right: 1px solid #696969;'>");
                _texto.AppendLine("                                                <span style='font-size: 13px;'>PREVISÃO&nbsp;&nbsp;</span>");
                _texto.AppendLine("                                            </td>");
                _texto.AppendLine("                                            <td style='text-align: center; border-bottom: 1px solid #696969;'>");
                _texto.AppendLine("                                                <span style='font-size: 13px; color: " + ((previsao.Contains("-")) ? "red" : "black") + ";'>" + previsao + "</span>");
                _texto.AppendLine("                                            </td>");
                _texto.AppendLine("                                        </tr>");
                _texto.AppendLine("                                        <tr style='line-height: 22px;'>");
                _texto.AppendLine("                                            <td style='text-align: right; border-bottom: 1px solid #696969; border-right: 1px solid #696969;'>");
                _texto.AppendLine("                                                <span style='font-size: 13px;'>QTDE FALTANTE&nbsp;&nbsp;</span>");
                _texto.AppendLine("                                            </td>");
                _texto.AppendLine("                                            <td style='text-align: center; border-bottom: 1px solid #696969;'>");
                _texto.AppendLine("                                                <span style='font-size: 13px;'>" + qtdeFaltante + "</span>");
                _texto.AppendLine("                                            </td>");
                _texto.AppendLine("                                        </tr>");
            }

            if (painel == 3 || painel == 4)
            {
                var pedidoZipperDiffDias = desenvController.ObterPedidoZipperDiffDias(produtos[0].COLECAO, produtos[0].MODELO, produtos[0].COR);
                if (pedidoZipperDiffDias != null)
                {
                    _texto.AppendLine("                                        <tr style='line-height: 22px;'>");
                    _texto.AppendLine("                                            <td colspan='2' style='text-align: right;'>");
                    _texto.AppendLine("                                                 <table border='0' width='100%' cellpadding='0' cellspacing='0'>");
                    _texto.AppendLine("                                                     <tr style='line-height: 22px;'>");
                    _texto.AppendLine("                                                         <td style='text-align: center; border-bottom: 1px solid #696969; border-right: 1px solid #696969;'>");
                    _texto.AppendLine("                                                             <span style='font-size: 13px;'>Corte<br />" + Convert.ToDateTime(pedidoZipperDiffDias.DATA_INICIO_CORTE).ToString("dd/MM/yyyy") + "</span>");
                    _texto.AppendLine("                                                         </td>");

                    if (pedidoZipperDiffDias.DATA_PEDIDO == null)
                        _texto.AppendLine("                                                         <td style='text-align: center; border-bottom: 1px solid #696969; border-right: 1px solid #696969;'>");
                    else
                        _texto.AppendLine("                                                         <td style='background-color: lightgreen; text-align: center; border-bottom: 1px solid #696969; border-right: 1px solid #696969;'>");

                    _texto.AppendLine("                                                             <span style='font-size: 13px;'>Pedido Zipper <br />" + ((pedidoZipperDiffDias.DATA_PEDIDO == null) ? "-" : Convert.ToDateTime(pedidoZipperDiffDias.DATA_PEDIDO).ToString("dd/MM/yyyy")) + "</span>");
                    _texto.AppendLine("                                                         </td>");

                    if (pedidoZipperDiffDias.DATA_SEPARADO == null)
                        _texto.AppendLine("                                                         <td style='text-align: center; border-bottom: 1px solid #696969; border-right: 1px solid #696969;'>");
                    else
                        _texto.AppendLine("                                                         <td style='background-color: lightgreen; text-align: center; border-bottom: 1px solid #696969; border-right: 1px solid #696969;'>");

                    _texto.AppendLine("                                                             <span style='font-size: 13px;'>Separado<br />" + ((pedidoZipperDiffDias.DATA_SEPARADO == null) ? "-" : Convert.ToDateTime(pedidoZipperDiffDias.DATA_SEPARADO).ToString("dd/MM/yyyy")) + "</span>");
                    _texto.AppendLine("                                                         </td>");
                    _texto.AppendLine("                                                         <td style='text-align: center; border-bottom: 1px solid #696969;'>");
                    _texto.AppendLine("                                                             <span style='font-size: 13px;'>Dias<br />" + pedidoZipperDiffDias.TOTAL_DIAS_FIFF.ToString() + "</span>");
                    _texto.AppendLine("                                                         </td>");
                    _texto.AppendLine("                                                     </tr>");
                    _texto.AppendLine("                                                 </table>");
                    _texto.AppendLine("                                            </td>");
                    _texto.AppendLine("                                        </tr>");
                }
            }


            _texto.AppendLine("                                        <tr style='line-height: 21px;'>"); // LOOP
            _texto.AppendLine("                                            <td style='text-align: right;'>");
            _texto.AppendLine("                                                <span style='font-size: 16px;'>&nbsp;</span>");
            _texto.AppendLine("                                            </td>");
            _texto.AppendLine("                                            <td style='text-align: center;'>");
            _texto.AppendLine("                                                <span style='font-size: 16px;'>&nbsp;</span>");
            _texto.AppendLine("                                            </td>");
            _texto.AppendLine("                                        </tr>");


            _texto.AppendLine("                                    </table>");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                        </table>");
            _texto.AppendLine("                    </div>");
            _texto.AppendLine("                </td>");

            return _texto;
        }
        public static StringBuilder MontarConteudoPrePedidoTecido(StringBuilder _texto, List<VW_OBTER_PREPEDIDO_TECIDOCOR> tecidos)
        {
            DesenvolvimentoController desenvController = new DesenvolvimentoController();
            ProducaoController prodController = new ProducaoController();
            BaseController baseController = new BaseController();


            // campos usados para auxiliar na montagem do relatorio // gambetas!
            //**************************************************************************************/

            _texto.AppendLine("                <td valign='top' style='height:670px; width:33%;'>");
            _texto.AppendLine("                    <div style='font: Calibri; border: 1px solid #696969; height:670px;'>");
            _texto.AppendLine("                        <table border='0' cellpadding='0' cellspacing='0' width='312px' style='padding: 0px;");
            _texto.AppendLine("                            color: black; font-size: 9.0pt; font-weight: 700;'>");
            _texto.AppendLine("                            <tr style='line-height: 40px;'>");
            _texto.AppendLine("                                <td colspan='2' style='text-align:center; border-bottom: 1px solid #696969;' valign='middle'>");
            _texto.AppendLine("                                    <span style='font-size: 18px; color:Red;'>" + tecidos[0].TECIDO + "</span>");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                            <tr style='line-height: 28px;'>");
            _texto.AppendLine("                                <td style='width:50%; text-align: right; border-bottom: 1px solid #696969; border-right: 1px solid #696969;' valign='middle'>");
            _texto.AppendLine("                                    <span style='font-size: 15px;'>FORNECEDOR&nbsp;&nbsp;</span>");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                                <td style='width:50%; text-align: center; border-bottom: 1px solid #696969;'>");
            _texto.AppendLine("                                    <span style='font-size: 14px;'>" + tecidos[0].FORNECEDOR + "</span>");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");

            _texto.AppendLine("                            <tr>");
            _texto.AppendLine("                                <td colspan='2' style='line-height: 30px; font-size: 12px;'>");
            _texto.AppendLine("                                    <table border='0' width='100%' cellpadding='0' cellspacing='0'>");
            /*COR FORNECEDOR*/
            _texto.AppendLine("                                        <tr style='line-height: 65px;'>");

            var corTecido = "";
            var corFornecedorTecido = desenvController.ObterCorPocket(tecidos[0].COR_FORNECEDOR);
            if (corFornecedorTecido != null)
                corTecido = corFornecedorTecido.IMAGEM;

            int widthMostruario = 0;
            _texto.AppendLine("                                            <td colspan='2' class='background-force' style='background: url(../.." + corTecido.Replace("~", "") + "); border-bottom: 1px solid #696969; " + ((widthMostruario > 0) ? "width:" + widthMostruario.ToString() + "px;" : "") + "'>");
            _texto.AppendLine("                                                 &nbsp;");
            _texto.AppendLine("                                            </td>");

            _texto.Append("                                           </tr>");
            /**/

            var corLinx = ((tecidos[0].COR != null && tecidos[0].COR.Trim() != "") ? prodController.ObterCoresBasicas(tecidos[0].COR.Trim()).DESC_COR.Trim() : "SEM COR");
            _texto.AppendLine("                                        <tr style='line-height: 28px;'>");
            _texto.AppendLine("                                            <td style='width: 50%; text-align: right; border-bottom: 1px solid #696969;'>");
            _texto.AppendLine("                                                <span style='font-size: 15px;'>COR LINX&nbsp;&nbsp;</span>");
            _texto.AppendLine("                                            </td>");
            _texto.AppendLine("                                            <td style='width: 50%; text-align: center; border-bottom: 1px solid #696969; border-left: 1px solid #696969;'>");
            _texto.AppendLine("                                                <span style='font-size: 14px;'>" + ((corLinx.Length <= 32) ? corLinx : corLinx.Substring(0, 32)) + "</span>");
            _texto.AppendLine("                                            </td>");
            _texto.AppendLine("                                        </tr>");

            _texto.AppendLine("                                        <tr style='line-height: 28px;'>");
            _texto.AppendLine("                                            <td style='width:50%; text-align: right; border-bottom: 1px solid #696969;'>");
            _texto.AppendLine("                                                <span style='font-size: 15px;'>COR FORNECEDOR&nbsp;&nbsp;</span>");
            _texto.AppendLine("                                            </td>");
            _texto.AppendLine("                                            <td style='width:50%; text-align: center; border-bottom: 1px solid #696969; border-left: 1px solid #696969;'>");
            _texto.AppendLine("                                                <span style='font-size: 14px;'>" + tecidos[0].COR_FORNECEDOR + "</span>");
            _texto.AppendLine("                                            </td>");
            _texto.AppendLine("                                        </tr>");
            _texto.AppendLine("                                    </table>");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");



            _texto.AppendLine("                            <tr>");
            _texto.AppendLine("                                <td colspan='2'>");
            _texto.AppendLine("                                    <table border='0' width='100%' cellpadding='0' cellspacing='0'>");


            foreach (var t in tecidos.OrderBy(p => p.GRIFFE).ThenBy(p => p.GRUPO_PRODUTO))
            {
                _texto.AppendLine("                                        <tr style='line-height: 21px;'>");
                _texto.AppendLine("                                            <td style='text-align: center; width: 95px;border-bottom: 1px solid #696969;border-right: 1px solid #696969;'>");
                _texto.AppendLine("                                                <span style='font-size: 15px;'>" + t.GRIFFE + "</span>");
                _texto.AppendLine("                                            </td>");
                _texto.AppendLine("                                            <td style='text-align: center;width: 38px;border-bottom: 1px solid #696969;border-right: 1px solid #696969;'>");
                _texto.AppendLine("                                                <span style='font-size: 13px;'>" + t.SKU.ToString() + "</span>");
                _texto.AppendLine("                                            </td>");
                _texto.AppendLine("                                            <td style='text-align: center;width:95px; border-bottom: 1px solid #696969; border-right: 1px solid #696969;'>");
                _texto.AppendLine("                                                <span style='font-size: 12px;'>" + t.GRUPO_PRODUTO + "</span>");
                _texto.AppendLine("                                            </td>");
                _texto.AppendLine("                                            <td style='text-align: center; border-bottom: 1px solid #696969;'>");
                _texto.AppendLine("                                                <span style='font-size: 12px;'>" + t.QTDE_TOTAL.ToString() + "</span>");
                _texto.AppendLine("                                            </td>");
                _texto.AppendLine("                                        </tr>");
            }


            _texto.AppendLine("                                        <tr style='line-height: 21px;'>"); // LOOP
            _texto.AppendLine("                                            <td style='text-align: center;'>");
            _texto.AppendLine("                                                <span style='font-size: 16px;'>&nbsp;</span>");
            _texto.AppendLine("                                            </td>");
            _texto.AppendLine("                                            <td style='text-align: center;'>");
            _texto.AppendLine("                                                <span style='font-size: 16px;'>&nbsp;</span>");
            _texto.AppendLine("                                            </td>");
            _texto.AppendLine("                                            <td style='text-align: center;'>");
            _texto.AppendLine("                                                <span style='font-size: 16px;'>&nbsp;</span>");
            _texto.AppendLine("                                            </td>");
            _texto.AppendLine("                                            <td style='text-align: center;'>");
            _texto.AppendLine("                                                <span style='font-size: 16px;'>&nbsp;</span>");
            _texto.AppendLine("                                            </td>");
            _texto.AppendLine("                                        </tr>");
            _texto.AppendLine("                                    </table>");
            _texto.AppendLine("                                </td>");
            _texto.AppendLine("                            </tr>");
            _texto.AppendLine("                        </table>");
            _texto.AppendLine("                    </div>");
            _texto.AppendLine("                </td>");

            return _texto;
        }




    }
}