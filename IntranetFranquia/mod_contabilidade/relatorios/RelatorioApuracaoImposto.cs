using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using DAL;

public class RelatorioApuracaoImposto
{

    #region "APURACAO IMPOSTO"
    public StringBuilder MontarApuracaoImposto(int codigoApuracaoEmpresa, string tipoImposto, int codigoUsuario, bool calcula, bool print)
    {
        StringBuilder _texto = new StringBuilder();
        ContabilidadeController contabilController = new ContabilidadeController();

        int contador = 0;
        decimal valorTotal1 = 0;
        decimal valorBaseCalculo1 = 0;

        decimal valorTotal2 = 0;
        decimal valorBaseCalculo2 = 0;

        decimal saldoCredor = 0;
        decimal valorTotalDebito = 0;

        var imposto = contabilController.ObterImpostoTipo(tipoImposto);
        var empresaApuracao = contabilController.ObterApuracaoEmpresa(codigoApuracaoEmpresa);

        _texto.Append("");
        _texto.Append("<!DOCTYPE HTML PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
        _texto.Append("<html>");
        _texto.Append("<head>");
        _texto.Append("    <title>Apuração de Imposto</title>");
        _texto.Append("    <meta charset='UTF-8' />");
        _texto.Append("    <style type='text/css'>");
        _texto.Append("        @media print {");
        _texto.Append("            .background-force {");
        _texto.Append("                -webkit-print-color-adjust: exact;");
        _texto.Append("            }");
        _texto.Append("        }");
        _texto.Append("    </style>");
        _texto.Append("</head>");
        _texto.Append("<body onload='" + ((print) ? "window.print();" : "") + "'>");
        _texto.Append("    <div id='divApuracao' align='center'>");
        _texto.Append("        <table border='0' cellpadding='0' cellspacing='0' width='689' style='width: 517pt; padding: 0px; color: black;");
        _texto.Append("            font-size: 7.5pt; font-family: Arial; background: white;'>");
        _texto.Append("            <tr>");
        _texto.Append("                <td style='line-height: 18px;'>");
        _texto.Append("                    <table border='0' cellpadding='0' cellspacing='0' width='689' style='width: 517pt' class='background-force'>");
        _texto.Append("                        <tr>");
        _texto.Append("                            <td rowspan='3' valign='middle' style='border:1px solid #000; text-align:center;'>");
        _texto.Append("                                <img alt='Handbook' width='145' src='http://cmaxweb.dnsalias.com:8585/image/logo.jpg' />");
        _texto.Append("                            </td>");
        _texto.Append("                            <td colspan='2' valign='bottom' style='text-align: center; border: 1px solid #000; border-left: none; background-color: #FDF5E6;'>");
        _texto.Append("                                <b>" + imposto.TIPO + " NÃO CUMULATIVO - ALIQUOTA " + imposto.ALIQUOTA.ToString("##0.00") + "%</b>");
        _texto.Append("                            </td>");
        _texto.Append("                        </tr>");
        _texto.Append("                        <tr>");
        _texto.Append("                            <td valign='bottom' style='border: 1px solid #000; border-left: none; border-top: none; background-color: #FDF5E6;'>");
        _texto.Append("                                &nbsp;Empresa: <b>" + empresaApuracao.EMPRESA1.NOME.Trim() + "</b>");
        _texto.Append("                            </td>");
        _texto.Append("                            <td valign='bottom' style='border: 1px solid #000; border-left: none; border-top: none; background-color: #FDF5E6;'>");
        _texto.Append("                                &nbsp;Competência: <b>" + empresaApuracao.COMPETENCIA.ToString("dd/MM/yyyy") + "</b>");
        _texto.Append("                            </td>");
        _texto.Append("                        </tr>");
        _texto.Append("                        <tr>");
        _texto.Append("                            <td valign='bottom' style='border: 1px solid #000; border-left: none; border-top: none; background-color: #FDF5E6;'>");
        _texto.Append("                                &nbsp;CNPJ: <b>" + empresaApuracao.EMPRESA1.CNPJ + "</b>");
        _texto.Append("                            </td>");
        _texto.Append("                            <td valign='bottom' style='border: 1px solid #000; border-left: none; border-top: none; background-color: #FDF5E6;'>");
        _texto.Append("                                &nbsp;Telefone: <b>11 5180-3460</b>");
        _texto.Append("                            </td>");
        _texto.Append("                        </tr>");
        _texto.Append("                        <tr>");
        _texto.Append("                            <td colspan='3' style='line-height:10px;'>");
        _texto.Append("                                &nbsp;");
        _texto.Append("                            </td>");
        _texto.Append("                        </tr>");
        _texto.Append("                        <tr style='line-height:13px;'>");
        _texto.Append("                            <td colspan='3' style='width:100%;'>");
        _texto.Append("                                <table border='0' cellpadding='0' cellspacing='0' width='100%'>");
        _texto.Append("                                    <tr style='background-color:#ccc;' class='background-force'>");
        _texto.Append("                                        <td colspan='3'>");
        _texto.Append("                                            &nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td colspan='4'>");
        _texto.Append("                                            <b>DISCRIMINAÇÕES</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");

        _texto.Append("                                    <!-- RECEITAS INICIO -->");

        _texto.Append("                                    <tr style='line-height:15px;'>");
        _texto.Append("                                        <td colspan='3'>");
        _texto.Append("                                            &nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td colspan='3'>");
        _texto.Append("                                            <b>" + imposto.TIT_RECEITA + "</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='text-align:right;'>");
        _texto.Append("                                            <b>Valores em R$" + "</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");

        /*ITENS DA RECEITA*/
        valorTotal1 = 0;
        contador += 1;
        var itemEmpresa = contabilController.ObterApuracaoEmpresaItem(codigoApuracaoEmpresa, 1);
        var item = contabilController.ObterApuracaoItem(1);
        var primeiraLinha = itemEmpresa.Where(p => p.CTB_APURACAO_ITEM == item[0].CODIGO).FirstOrDefault();
        if (primeiraLinha != null)
            valorTotal1 += primeiraLinha.VALOR;

        _texto.Append("                                    <tr>");
        _texto.Append("                                        <td rowspan='" + (item.Count() + 2).ToString() + "' style='width: 20px; background-color: #ccc; border: 1px solid #000; ' align='center' valign='middle' class='background-force'>");
        _texto.Append("                                         <b>R<br />");
        _texto.Append("                                            E<br />");
        _texto.Append("                                            C<br />");
        _texto.Append("                                            E<br />");
        _texto.Append("                                            I<br />");
        _texto.Append("                                            T<br />");
        _texto.Append("                                            A<br />");
        _texto.Append("                                            S<br /></b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='width:8px;'>&nbsp;</td>");
        _texto.Append("                                        <td style='text-align: center; width: 25px; border: 1px solid #000;'>");
        _texto.Append("                                            <b>" + contador.ToString() + "</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='text-align: center; width: 30px; border: 1px solid #000; border-left:none;'>");
        _texto.Append("                                            <b>(+)</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td colspan='2' style='border: 1px solid #000; border-left:none;'>");
        _texto.Append("                                            &nbsp;" + item[0].ITEM);
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='text-align: right; width: 85px; border: 1px solid #000; border-left: none; background-color: #FDF5E6;' class='background-force'>");
        _texto.Append("                                            " + ((primeiraLinha == null) ? "0,00" : primeiraLinha.VALOR.ToString("###,###,###,##0.00")) + "&nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");

        foreach (var ir in item.Where(p => p.CODIGO != item[0].CODIGO))
        {
            contador += 1;
            var i = itemEmpresa.Where(p => p.CTB_APURACAO_ITEM == ir.CODIGO).FirstOrDefault();
            if (i != null)
            {
                if (i.CTB_APURACAO_ITEM == 2 ||
                    i.CTB_APURACAO_ITEM == 3 ||
                    i.CTB_APURACAO_ITEM == 4 ||
                    i.CTB_APURACAO_ITEM == 5 ||
                    i.CTB_APURACAO_ITEM == 6 ||
                    i.CTB_APURACAO_ITEM == 7 ||
                    i.CTB_APURACAO_ITEM == 9 ||
                    i.CTB_APURACAO_ITEM == 38)
                    valorTotal2 += i.VALOR;
                else
                    valorTotal1 += i.VALOR;
            }

            _texto.Append("                                    <tr>");
            _texto.Append("                                        <td>&nbsp;</td>");
            _texto.Append("                                        <td style='text-align: center; width: 25px; border: 1px solid #000; border-top:none;'>");
            _texto.Append("                                            <b>" + contador.ToString() + "</b>");
            _texto.Append("                                        </td>");
            _texto.Append("                                        <td style='text-align: center; width: 30px; border: 1px solid #000; border-left: none; border-top: none;'>");
            _texto.Append("                                            <b>(+)</b>");
            _texto.Append("                                        </td>");
            _texto.Append("                                        <td colspan='2' style='border: 1px solid #000; border-left: none; border-top: none;'>");
            _texto.Append("                                            &nbsp;" + ir.ITEM);
            _texto.Append("                                        </td>");
            _texto.Append("                                        <td style='text-align: right; width: 85px; border: 1px solid #000; border-left: none; border-top: none; background-color: #FDF5E6; ' class='background-force'>");
            _texto.Append("                                            " + ((i == null) ? "0,00" : i.VALOR.ToString("###,###,###,##0.00")) + "&nbsp;");
            _texto.Append("                                        </td>");
            _texto.Append("                                    </tr>");
        }
        _texto.Append("                                    <tr>");
        _texto.Append("                                        <td colspan='7' style='line-height:4px;'>");
        _texto.Append("                                            &nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");

        contador += 1;

        _texto.Append("                                    <tr>");
        _texto.Append("                                        <td>&nbsp;</td>");
        _texto.Append("                                        <td style='text-align: center; width: 25px; border: 1px solid #000;'>");
        _texto.Append("                                            <b>" + contador.ToString() + "</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='text-align: center; width: 30px; border: 1px solid #000; border-left:none;'>");
        _texto.Append("                                            <b>(=)</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td colspan='2' style='border: 1px solid #000; border-left:none;'>");
        _texto.Append("                                            &nbsp;<b>Total da Receita Bruta</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='text-align: right; width: 85px; border: 1px solid #000; border-left: none; background-color: #FDF5E6; ' class='background-force'>");
        _texto.Append("                                            <b>" + (valorTotal1 + valorTotal2).ToString("###,###,###,##0.00") + "</b>&nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");

        _texto.Append("                                    <!-- EXCLUSÕES INICIO -->");
        _texto.Append("                                    <tr style='line-height:15px;'>");
        _texto.Append("                                        <td colspan='3'>");
        _texto.Append("                                            &nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td colspan='3'>");
        _texto.Append("                                            <b>" + imposto.TIT_EXCLUSAO + "</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='text-align:right;'>");
        _texto.Append("                                            <b>Valores em R$</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");

        valorBaseCalculo1 += valorTotal1;
        valorBaseCalculo2 += valorTotal2;

        /*ITENS DA EXCLUSAO*/
        decimal valorInstrumento = 0;

        valorTotal1 = 0;
        contador += 1;
        itemEmpresa = contabilController.ObterApuracaoEmpresaItem(codigoApuracaoEmpresa, 2);
        item = contabilController.ObterApuracaoItem(2);

        string empresa = empresaApuracao.EMPRESA1.NOME.Trim();
        if (empresa == "HANDBOOK STORE CONFECCOES LTDA")
            item = item.Where(p => p.CODIGO != 40 && p.CODIGO != 41 && p.CODIGO != 42 && p.CODIGO != 43 && p.CODIGO != 44 && p.CODIGO != 45 && p.CODIGO != 46).ToList();
        if (empresa == "HB GROUP CONFECCOES LTDA")
            item = item.Where(p => p.CODIGO != 39 && p.CODIGO != 41 && p.CODIGO != 42 && p.CODIGO != 43 && p.CODIGO != 44 && p.CODIGO != 45 && p.CODIGO != 46).ToList();
        if (empresa == "HBF IMPORTADORA LTDA")
            item = item.Where(p => p.CODIGO != 39 && p.CODIGO != 40 && p.CODIGO != 42 && p.CODIGO != 43 && p.CODIGO != 44 && p.CODIGO != 45 && p.CODIGO != 46).ToList();
        if (empresa == "MEGA BITE CONFECÇÕES LTDA")
            item = item.Where(p => p.CODIGO != 39 && p.CODIGO != 40 && p.CODIGO != 41 && p.CODIGO != 43 && p.CODIGO != 44 && p.CODIGO != 45 && p.CODIGO != 46).ToList();
        if (empresa == "TAGZY CONFECCOES LTDA")
            item = item.Where(p => p.CODIGO != 39 && p.CODIGO != 40 && p.CODIGO != 41 && p.CODIGO != 42 && p.CODIGO != 44 && p.CODIGO != 45 && p.CODIGO != 46).ToList();
        if (empresa == "YUGZY CONFECCOES LTDA")
            item = item.Where(p => p.CODIGO != 39 && p.CODIGO != 40 && p.CODIGO != 41 && p.CODIGO != 42 && p.CODIGO != 43 && p.CODIGO != 45 && p.CODIGO != 46).ToList();
        if (empresa == "GABAZEITU COMERCIO DE CONFECCOES EIRELI - EPP")
            item = item.Where(p => p.CODIGO != 39 && p.CODIGO != 40 && p.CODIGO != 41 && p.CODIGO != 42 && p.CODIGO != 43 && p.CODIGO != 44 && p.CODIGO != 46).ToList();
        if (empresa == "C-MAX CLOTHES AND HAPPINESS CONFECÇÕES LTDA")
            item = item.Where(p => p.CODIGO != 39 && p.CODIGO != 40 && p.CODIGO != 41 && p.CODIGO != 42 && p.CODIGO != 43 && p.CODIGO != 44 && p.CODIGO != 45).ToList();

        primeiraLinha = itemEmpresa.Where(p => p.CTB_APURACAO_ITEM == item[0].CODIGO).FirstOrDefault();
        if (primeiraLinha != null)
            valorTotal1 += primeiraLinha.VALOR;

        _texto.Append("                                    <tr>");
        _texto.Append("                                        <td rowspan='" + (item.Count() + 2).ToString() + "' style='width: 20px; background-color: #ccc; border: 1px solid #000; ' align='center' valign='middle' class='background-force'>");
        _texto.Append("                                         <b>E<br />");
        _texto.Append("                                            X<br />");
        _texto.Append("                                            C<br />");
        _texto.Append("                                            L<br />");
        _texto.Append("                                            U<br />");
        _texto.Append("                                            S<br />");
        _texto.Append("                                            Ã<br />");
        _texto.Append("                                            O<br /></b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='width:8px;'>&nbsp;</td>");
        _texto.Append("                                        <td style='text-align: center; width: 25px; border: 1px solid #000;'>");
        _texto.Append("                                            <b>" + contador.ToString() + "</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='text-align: center; width: 30px; border: 1px solid #000; border-left:none;'>");
        _texto.Append("                                            <b>(-)</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td colspan='2' style='border: 1px solid #000; border-left:none;'>");
        _texto.Append("                                            &nbsp;" + item[0].ITEM);
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='text-align: right; width: 85px; border: 1px solid #000; border-left: none; background-color: #FDF5E6; ' class='background-force'>");
        _texto.Append("                                            " + ((primeiraLinha == null) ? "0,00" : primeiraLinha.VALOR.ToString("###,###,###,##0.00")) + "&nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");

        foreach (var ir in item.Where(p => p.CODIGO != item[0].CODIGO))
        {
            contador += 1;
            var i = itemEmpresa.Where(p => p.CTB_APURACAO_ITEM == ir.CODIGO).FirstOrDefault();
            if (i != null)
                valorTotal1 += i.VALOR;

            if (i != null
                && (ir.CODIGO == 39 || ir.CODIGO == 40 || ir.CODIGO == 41 || ir.CODIGO == 42 || ir.CODIGO == 43 || ir.CODIGO == 44 || ir.CODIGO == 45 || ir.CODIGO == 46 || ir.CODIGO == 12))
                valorInstrumento += i.VALOR;

            _texto.Append("                                    <tr>");
            _texto.Append("                                        <td>&nbsp;</td>");
            _texto.Append("                                        <td style='text-align: center; width: 25px; border: 1px solid #000; border-top:none;'>");
            _texto.Append("                                            <b>" + contador.ToString() + "</b>");
            _texto.Append("                                        </td>");
            _texto.Append("                                        <td style='text-align: center; width: 30px; border: 1px solid #000; border-left: none; border-top: none;'>");
            _texto.Append("                                            <b>(-)</b>");
            _texto.Append("                                        </td>");
            _texto.Append("                                        <td colspan='2' style='border: 1px solid #000; border-left: none; border-top: none;'>");
            _texto.Append("                                            &nbsp;" + ir.ITEM);
            _texto.Append("                                        </td>");
            _texto.Append("                                        <td style='text-align: right; width: 85px; border: 1px solid #000; border-left: none; border-top: none; background-color: #FDF5E6; ' class='background-force'>");
            _texto.Append("                                            " + ((i == null) ? "0,00" : i.VALOR.ToString("###,###,###,##0.00")) + "&nbsp;");
            _texto.Append("                                        </td>");
            _texto.Append("                                    </tr>");
        }

        _texto.Append("                                    <tr>");
        _texto.Append("                                        <td colspan='7' style='line-height:4px;'>");
        _texto.Append("                                            &nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");

        contador += 1;

        _texto.Append("                                    <tr>");
        _texto.Append("                                        <td>&nbsp;</td>");
        _texto.Append("                                        <td style='text-align: center; width: 25px; border: 1px solid #000;'>");
        _texto.Append("                                            <b>" + contador.ToString() + "</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='text-align: center; width: 30px; border: 1px solid #000; border-left:none;'>");
        _texto.Append("                                            <b>(=)</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td colspan='2' style='border: 1px solid #000; border-left:none;'>");
        _texto.Append("                                            &nbsp;<b>Total das Exclusões</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='text-align: right; width: 85px; border: 1px solid #000; border-left: none; background-color: #FDF5E6; ' class='background-force'>");
        _texto.Append("                                            <b>" + valorTotal1.ToString("###,###,###,##0.00") + "</b>&nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");

        _texto.Append("                                    <!-- ISENÇÃO INICIO -->");
        _texto.Append("                                    <tr style='line-height:15px;'>");
        _texto.Append("                                        <td colspan='3'>");
        _texto.Append("                                            &nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td colspan='3'>");
        _texto.Append("                                            <b>" + imposto.TIT_ISENCAO + "</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='text-align:right;'>");
        _texto.Append("                                            <b>Valores em R$</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");

        //valorBaseCalculo -= valorTotal;

        /*ITENS DA ISENCAO*/
        valorTotal1 = 0;
        contador += 1;
        itemEmpresa = contabilController.ObterApuracaoEmpresaItem(codigoApuracaoEmpresa, 3);
        item = contabilController.ObterApuracaoItem(3);
        primeiraLinha = itemEmpresa.Where(p => p.CTB_APURACAO_ITEM == item[0].CODIGO).FirstOrDefault();
        if (primeiraLinha != null)
            valorTotal1 += primeiraLinha.VALOR;

        _texto.Append("                                    <tr>");
        _texto.Append("                                        <td rowspan='" + (item.Count() + 2) + "' style='width: 20px; background-color: #ccc; border: 1px solid #000; ' align='center' valign='middle' class='background-force'>");
        _texto.Append("                                         <font size='1'>");
        _texto.Append("                                         <b>I<br />");
        _texto.Append("                                            S<br />");
        _texto.Append("                                            E<br />");
        _texto.Append("                                            N<br />");
        _texto.Append("                                            Ç<br />");
        _texto.Append("                                            Ã<br />");
        _texto.Append("                                            O<br /></b>");
        _texto.Append("                                         </font>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='width:8px;'>&nbsp;</td>");
        _texto.Append("                                        <td style='text-align: center; width: 25px; border: 1px solid #000;'>");
        _texto.Append("                                            <b>" + contador.ToString() + "</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='text-align: center; width: 30px; border: 1px solid #000; border-left:none;'>");
        _texto.Append("                                            <b>(-)</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td colspan='2' style='border: 1px solid #000; border-left:none;'>");
        _texto.Append("                                            &nbsp;" + item[0].ITEM);
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='text-align: right; width: 85px; border: 1px solid #000; border-left: none; background-color: #FDF5E6; ' class='background-force'>");
        _texto.Append("                                            " + ((primeiraLinha == null) ? "0,00" : primeiraLinha.VALOR.ToString("###,###,###,##0.00")) + "&nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");

        foreach (var ir in item.Where(p => p.CODIGO != item[0].CODIGO))
        {
            contador += 1;
            var i = itemEmpresa.Where(p => p.CTB_APURACAO_ITEM == ir.CODIGO).FirstOrDefault();
            if (i != null)
                valorTotal1 += i.VALOR;
            _texto.Append("                                    <tr>");
            _texto.Append("                                        <td>&nbsp;</td>");
            _texto.Append("                                        <td style='text-align: center; width: 25px; border: 1px solid #000; border-top:none;'>");
            _texto.Append("                                            <b>" + contador.ToString() + "</b>");
            _texto.Append("                                        </td>");
            _texto.Append("                                        <td style='text-align: center; width: 30px; border: 1px solid #000; border-left: none; border-top: none;'>");
            _texto.Append("                                            <b>(-)</b>");
            _texto.Append("                                        </td>");
            _texto.Append("                                        <td colspan='2' style='border: 1px solid #000; border-left: none; border-top: none;'>");
            _texto.Append("                                            &nbsp;" + ir.ITEM);
            _texto.Append("                                        </td>");
            _texto.Append("                                        <td style='text-align: right; width: 85px; border: 1px solid #000; border-left: none; border-top: none; background-color: #FDF5E6; ' class='background-force'>");
            _texto.Append("                                            " + ((i == null) ? "0,00" : i.VALOR.ToString("###,###,###,##0.00")) + "&nbsp;");
            _texto.Append("                                        </td>");
            _texto.Append("                                    </tr>");
        }
        _texto.Append("                                    <tr>");
        _texto.Append("                                        <td colspan='7' style='line-height:2px;'>");
        _texto.Append("                                            &nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");

        contador += 1;

        _texto.Append("                                    <tr>");
        _texto.Append("                                        <td>&nbsp;</td>");
        _texto.Append("                                        <td style='text-align: center; width: 25px; border: 1px solid #000;'>");
        _texto.Append("                                            <b>" + contador.ToString() + "</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='text-align: center; width: 30px; border: 1px solid #000; border-left:none;'>");
        _texto.Append("                                            <b>(=)</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td colspan='2' style='border: 1px solid #000; border-left:none;'>");
        _texto.Append("                                            &nbsp;<b>Total das Isenções</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='text-align: right; width: 85px; border: 1px solid #000; border-left: none; background-color: #FDF5E6; ' class='background-force'>");
        _texto.Append("                                            <b>" + valorTotal1.ToString("###,###,###,##0.00") + "</b>&nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");

        valorBaseCalculo1 = valorBaseCalculo1 - valorTotal1 - valorInstrumento;

        //BASE DE CALCULO
        contador += 1;

        _texto.Append("                                    <!-- BASE DE CALCULO INICIO -->");
        _texto.Append("                                    <tr>");
        _texto.Append("                                        <td colspan='7' style='line-height:4px;'>");
        _texto.Append("                                            &nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");
        _texto.Append("                                    <tr>");
        _texto.Append("                                        <td style='width: 20px; background-color: #ccc; border: 1px solid #000; border-bottom:none;' class='background-force'>");
        _texto.Append("                                            &nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='width:8px;'>&nbsp;</td>");
        _texto.Append("                                        <td style='text-align: center; width: 25px; border: 1px solid #000; border-bottom:none;'>");
        _texto.Append("                                            <b>" + contador.ToString() + "</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='text-align: center; width: 30px; border: 1px solid #000; border-left:none; border-bottom:none;'>");
        _texto.Append("                                            <b>(=)</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td colspan='2' style='border: 1px solid #000; border-left:none; border-bottom:none;'>");
        _texto.Append("                                            &nbsp;<b>BASE DE CÁLCULO DO " + imposto.TIPO + " NÃO CUMULATIVO</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='text-align: right; width: 85px; border: 1px solid #000; border-left: none; border-bottom:none; background-color: #FDF5E6; ' class='background-force'>");
        _texto.Append("                                            <b>" + valorBaseCalculo1.ToString("###,###,###,##0.00") + "</b>&nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");

        contador += 1;

        //IMPOSTO DEVIDO 1
        _texto.Append("                                    <tr>");
        _texto.Append("                                        <td style='width: 20px; background-color: #ccc; border: 1px solid #000; border-bottom:none; border-top:none;' class='background-force'>");
        _texto.Append("                                            &nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='width:8px;'>&nbsp;</td>");
        _texto.Append("                                        <td style='text-align: center; width: 25px; border: 1px solid #000; border-bottom:none;'>");
        _texto.Append("                                            <b>" + contador.ToString() + "</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='text-align: center; width: 30px; border: 1px solid #000; border-left:none; border-bottom:none;'>");
        _texto.Append("                                            <b>(=)</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td colspan='2' style='border: 1px solid #000; border-left:none; border-bottom:none;'>");
        _texto.Append("                                            &nbsp;<b>" + imposto.TIPO + " DEVIDO = BASE DE CÁLCULO X " + imposto.ALIQUOTA.ToString("##0.00") + "%</b>");
        _texto.Append("                                        </td>");

        decimal valorTotalDeb1 = (valorBaseCalculo1 * (imposto.ALIQUOTA / 100));

        _texto.Append("                                        <td style='text-align: right; width: 85px; border: 1px solid #000; border-left: none; border-bottom:none; background-color: #FDF5E6; ' class='background-force'>");
        _texto.Append("                                            <b>" + valorTotalDeb1.ToString("###,###,###,##0.00") + "</b>&nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");

        contador += 1;

        _texto.Append("                                    <tr>");
        _texto.Append("                                        <td style='width: 20px; background-color: #ccc; border: 1px solid #000; border-bottom:none; border-top:none;' class='background-force'>");
        _texto.Append("                                            &nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='width:8px;'>&nbsp;</td>");
        _texto.Append("                                        <td style='text-align: center; width: 25px; border: 1px solid #000; border-bottom:none;'>");
        _texto.Append("                                            <b>" + contador.ToString() + "</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='text-align: center; width: 30px; border: 1px solid #000; border-left:none; border-bottom:none;'>");
        _texto.Append("                                            <b>(=)</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td colspan='2' style='border: 1px solid #000; border-left:none; border-bottom:none;'>");
        _texto.Append("                                            &nbsp;<b>BASE DE CÁLCULO DO " + imposto.TIPO + " RECEITAS FINANCEIRAS</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='text-align: right; width: 85px; border: 1px solid #000; border-left: none; border-bottom:none; background-color: #FDF5E6; ' class='background-force'>");
        _texto.Append("                                            <b>" + valorBaseCalculo2.ToString("###,###,###,##0.00") + "</b>&nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");

        contador += 1;

        //IMPOSTO DEVIDO 2
        _texto.Append("                                    <tr>");
        _texto.Append("                                        <td style='width: 20px; background-color: #ccc; border: 1px solid #000; border-bottom:none; border-top:none;' class='background-force'>");
        _texto.Append("                                            &nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='width:8px;'>&nbsp;</td>");
        _texto.Append("                                        <td style='text-align: center; width: 25px; border: 1px solid #000; border-bottom:none;'>");
        _texto.Append("                                            <b>" + contador.ToString() + "</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='text-align: center; width: 30px; border: 1px solid #000; border-left:none; border-bottom:none;'>");
        _texto.Append("                                            <b>(=)</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td colspan='2' style='border: 1px solid #000; border-left:none; border-bottom:none;'>");
        _texto.Append("                                            &nbsp;<b>" + imposto.TIPO + " DEVIDO = BASE DE CÁLCULO X " + Convert.ToDecimal(imposto.ALIQUOTA_FINANCEIRA).ToString("##0.00") + "%</b>");
        _texto.Append("                                        </td>");

        decimal valorTotalDeb2 = (valorBaseCalculo2 * (Convert.ToDecimal(imposto.ALIQUOTA_FINANCEIRA) / 100));

        _texto.Append("                                        <td style='text-align: right; width: 85px; border: 1px solid #000; border-left: none; border-bottom:none; background-color: #FDF5E6; ' class='background-force'>");
        _texto.Append("                                            <b>" + valorTotalDeb2.ToString("###,###,###,##0.00") + "</b>&nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");

        //RESULTADO PIS
        valorTotalDebito = valorTotalDeb1 + valorTotalDeb2;

        contador += 1;
        _texto.Append("                                    <tr>");
        _texto.Append("                                        <td style='width: 20px; background-color: #ccc; border: 1px solid #000; border-top:none;' class='background-force'>");
        _texto.Append("                                            &nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='width:8px;'>&nbsp;</td>");
        _texto.Append("                                        <td style='text-align: center; width: 25px; border: 1px solid #000;'>");
        _texto.Append("                                            <b>" + contador.ToString() + "</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='text-align: center; width: 30px; border: 1px solid #000; border-left:none;'>");
        _texto.Append("                                            <b>(=)</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td colspan='2' style='border: 1px solid #000; border-left:none; '>");
        _texto.Append("                                            &nbsp;<b>DÉBITO " + imposto.TIPO + "</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='text-align: right; width: 85px; border: 1px solid #000; border-left: none;  background-color: #FDF5E6; ' class='background-force'>");
        _texto.Append("                                            <b>" + valorTotalDebito.ToString("###,###,###,##0.00") + "</b>&nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");

        _texto.Append("                                    <!-- CRÉDITO INICIO -->");
        _texto.Append("                                    <tr style='line-height:15px;'>");
        _texto.Append("                                        <td colspan='3'>");
        _texto.Append("                                            &nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td colspan='2'>");
        _texto.Append("                                            <b>" + imposto.TIT_CREDITO + "</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='text-align:right;'>");
        _texto.Append("                                            <b>Valores em R$</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='text-align:right;'>");
        _texto.Append("                                            <b>Valores em R$</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");

        /*ITENS Do CREDITO*/
        valorTotal1 = 0;
        contador += 1;
        itemEmpresa = contabilController.ObterApuracaoEmpresaItem(codigoApuracaoEmpresa, 4);
        item = contabilController.ObterApuracaoItem(4);
        primeiraLinha = itemEmpresa.Where(p => p.CTB_APURACAO_ITEM == item[0].CODIGO).FirstOrDefault();
        if (primeiraLinha != null)
            valorTotal1 += (primeiraLinha.VALOR * (imposto.ALIQUOTA / 100));

        _texto.Append("                                    <tr>");
        _texto.Append("                                        <td rowspan='" + (item.Count() + 2).ToString() + "' style='width: 20px; background-color: #ccc; border: 1px solid #000; ' align='center' valign='middle' class='background-force'>");
        _texto.Append("                                         <b>C<br />");
        _texto.Append("                                            R<br />");
        _texto.Append("                                            É<br />");
        _texto.Append("                                            D<br />");
        _texto.Append("                                            I<br />");
        _texto.Append("                                            T<br />");
        _texto.Append("                                            O<br /></b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='width:8px;'>&nbsp;</td>");
        _texto.Append("                                        <td style='text-align: center; width: 25px; border: 1px solid #000;'>");
        _texto.Append("                                            <b>" + contador.ToString() + "<b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='text-align: center; width: 30px; border: 1px solid #000; border-left:none;'>");
        _texto.Append("                                            <b>(-)</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='border: 1px solid #000; border-left:none;'>");
        _texto.Append("                                            &nbsp;" + item[0].ITEM);
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='text-align: right; width: 85px; border: 1px solid #000; border-left: none; background-color: #FDF5E6; ' class='background-force'>");
        _texto.Append("                                            " + ((primeiraLinha == null) ? "" : primeiraLinha.VALOR.ToString("###,###,###,##0.00")) + "&nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='text-align: right; width: 85px; border: 1px solid #000; border-left: none; background-color: #FDF5E6; ' class='background-force'>");
        _texto.Append("                                            " + ((primeiraLinha == null) ? "" : (primeiraLinha.VALOR * (imposto.ALIQUOTA / 100)).ToString("###,###,###,##0.00")) + "&nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");

        decimal valorAux = 0;
        foreach (var ir in item.Where(p => p.CODIGO != item[0].CODIGO))
        {
            contador += 1;
            var i = itemEmpresa.Where(p => p.CTB_APURACAO_ITEM == ir.CODIGO).FirstOrDefault();
            if (i != null)
            {
                if (ir.CODIGO == 34)
                    valorTotal1 += (i.VALOR / 12) * (Convert.ToDecimal(((imposto.CODIGO == 1) ? 3.00 : 0.65)) / 100);
                else if (ir.CODIGO == 36 || ir.CODIGO == 47)
                    valorTotal1 += i.VALOR;
                else
                    valorTotal1 += (i.VALOR * (imposto.ALIQUOTA / 100));
            }


            if (ir.CODIGO == 34)
            {
                valorAux = ((i == null) ? 0 : (i.VALOR / 12) * (Convert.ToDecimal(((imposto.CODIGO == 1) ? 3.00 : 0.65)) / 100));
            }
            else if (ir.CODIGO == 36 || ir.CODIGO == 47)
            {
                valorAux = ((i == null) ? 0 : i.VALOR);
                saldoCredor = valorAux;
            }
            else
            {
                valorAux = ((i == null) ? 0 : (i.VALOR * (imposto.ALIQUOTA / 100)));
            }

            _texto.Append("                                    <tr>");
            _texto.Append("                                        <td>&nbsp;</td>");
            _texto.Append("                                        <td style='text-align: center; width: 25px; border: 1px solid #000; border-top:none;'>");
            _texto.Append("                                            <b>" + contador.ToString() + "</b>");
            _texto.Append("                                        </td>");
            _texto.Append("                                        <td style='text-align: center; width: 30px; border: 1px solid #000; border-left: none; border-top: none;'>");
            _texto.Append("                                            <b>(-)</b>");
            _texto.Append("                                        </td>");
            _texto.Append("                                        <td style='border: 1px solid #000; border-left: none; border-top: none;'>");
            _texto.Append("                                            &nbsp;<span style='" + ((valorAux < 0) ? "color:red;" : "") + "'>" + ir.ITEM + "</span>");
            _texto.Append("                                        </td>");
            _texto.Append("                                        <td style='text-align: right; width: 85px; border: 1px solid #000; border-left: none; border-top: none; background-color: #FDF5E6; ' class='background-force'>");
            _texto.Append("                                            " + ((i == null || ir.CODIGO == 36 || ir.CODIGO == 47) ? "0,00" : i.VALOR.ToString("###,###,###,##0.00")) + "&nbsp;");
            _texto.Append("                                        </td>");

            _texto.Append("                                        <td style='text-align: right; width: 85px; border: 1px solid #000; border-left: none; border-top: none; background-color: #FDF5E6; ' class='background-force'>");
            _texto.Append("                                            " + valorAux.ToString("###,###,###,##0.00") + "&nbsp;");
            _texto.Append("                                        </td>");
            _texto.Append("                                    </tr>");
        }

        _texto.Append("                                    <tr>");
        _texto.Append("                                        <td colspan='7' style='line-height:4px;'>");
        _texto.Append("                                            &nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");

        contador += 1;

        _texto.Append("                                    <tr>");
        _texto.Append("                                        <td>&nbsp;</td>");
        _texto.Append("                                        <td style='text-align: center; width: 25px; border: 1px solid #000;'>");
        _texto.Append("                                            <b>" + contador.ToString() + "</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='text-align: center; width: 30px; border: 1px solid #000; border-left:none;'>");
        _texto.Append("                                            <b>(=)</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td colspan='2' style='border: 1px solid #000; border-left:none;'>");
        _texto.Append("                                            &nbsp;<b>Total dos Créditos</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='text-align: right; width: 85px; border: 1px solid #000; border-left: none; background-color: #FDF5E6; ' class='background-force'>");
        _texto.Append("                                            <b>" + valorTotal1.ToString("###,###,###,##0.00") + "</b>&nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");

        contador += 1;

        _texto.Append("                                    <!-- IMPOSTO NAO CUMULATIVO INICIO -->");
        _texto.Append("                                    <tr>");
        _texto.Append("                                        <td colspan='7' style='line-height:4px;'>");
        _texto.Append("                                            &nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");
        _texto.Append("                                    <tr>");
        _texto.Append("                                        <td style='width: 20px; background-color: #ccc; border: 1px solid #000; ' class='background-force'>");
        _texto.Append("                                            &nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='width:8px;'>&nbsp;</td>");
        _texto.Append("                                        <td style='text-align: center; width: 25px; border: 1px solid #000;'>");
        _texto.Append("                                            <b>" + contador.ToString() + "</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='text-align: center; width: 30px; border: 1px solid #000; border-left:none;'>");
        _texto.Append("                                            <b>(=)</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td colspan='2' style='border: 1px solid #000; border-left:none;'>");
        _texto.Append("                                            &nbsp;<b>" + imposto.TIPO + " NÃO CUMULATIVO A PAGAR - Código no " + imposto.CODIGO_DARF + "</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='text-align: right; width: 85px; border: 1px solid #000; border-left: none; background-color: #FDF5E6; ' class='background-force'>");
        _texto.Append("                                            <b>" + (((valorBaseCalculo1 * (imposto.ALIQUOTA / 100)) + (valorBaseCalculo2 * (Convert.ToDecimal(imposto.ALIQUOTA_FINANCEIRA) / 100))) - (valorTotal1)).ToString("###,###,###,##0.00") + "</b>&nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");

        _texto.Append("                                    <!-- SALDOS INICIO -->");
        _texto.Append("                                    <tr style='line-height:20px;'>");
        _texto.Append("                                        <td colspan='3'>");
        _texto.Append("                                            &nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td colspan='3'>");
        _texto.Append("                                            <b>DEMONSTRATIVO DOS SALDOS</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='text-align:right;'>");
        _texto.Append("                                            <b>Valores em R$</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");

        contador += 1;

        _texto.Append("                                    <tr>");
        _texto.Append("                                        <td rowspan='6' style='width: 20px; background-color: #ccc; border: 1px solid #000; ' align='center' valign='middle' class='background-force'>");
        _texto.Append("                                         <b>S<br />");
        _texto.Append("                                            A<br />");
        _texto.Append("                                            L<br />");
        _texto.Append("                                            D<br />");
        _texto.Append("                                            O<br /></b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='width:8px;'>&nbsp;</td>");
        _texto.Append("                                        <td style='text-align: center; width: 25px; border: 1px solid #000;'>");
        _texto.Append("                                            <b>" + contador.ToString() + "</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='text-align: center; width: 30px; border: 1px solid #000; border-left:none;'>");
        _texto.Append("                                            <b>(+)</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td colspan='2' style='border: 1px solid #000; border-left:none;'>");
        _texto.Append("                                            &nbsp;<b>Saldo Credor do Período Anterior</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='text-align: right; width: 85px; border: 1px solid #000; border-left: none; background-color: #FDF5E6; ' class='background-force'>");
        _texto.Append("                                            <b>" + saldoCredor.ToString("###,###,###,##0.00") + "</b>&nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");

        contador += 1;

        _texto.Append("                                    <tr>");
        _texto.Append("                                        <td>&nbsp;</td>");
        _texto.Append("                                        <td style='text-align: center; width: 25px; border: 1px solid #000; border-top:none;'>");
        _texto.Append("                                            <b>" + contador.ToString() + "</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='text-align: center; width: 30px; border: 1px solid #000; border-left: none; border-top: none;'>");
        _texto.Append("                                            <b>(+)</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td colspan='2' style='border: 1px solid #000; border-left: none; border-top: none;'>");
        _texto.Append("                                            &nbsp;Créditos do Período de Apuração");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='text-align: right; width: 85px; border: 1px solid #000; border-left: none; border-top: none; background-color: #FDF5E6; ' class='background-force'>");
        _texto.Append("                                            " + valorTotal1.ToString("###,###,###,##0.00") + "&nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");

        contador += 1;

        _texto.Append("                                    <tr>");
        _texto.Append("                                        <td>&nbsp;</td>");
        _texto.Append("                                        <td style='text-align: center; width: 25px; border: 1px solid #000; border-top:none;'>");
        _texto.Append("                                            <b>" + contador.ToString() + "</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='text-align: center; width: 30px; border: 1px solid #000; border-left: none; border-top: none;'>");
        _texto.Append("                                            <b>(=)</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td colspan='2' style='border: 1px solid #000; border-left: none; border-top: none;'>");
        _texto.Append("                                            &nbsp;<b>Total do Créditos Apurados</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='text-align: right; width: 85px; border: 1px solid #000; border-left: none; border-top: none; background-color: #FDF5E6; ' class='background-force'>");
        _texto.Append("                                            <b>" + valorTotal1.ToString("###,###,###,##0.00") + "</b>&nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");

        contador += 1;

        _texto.Append("                                    <tr>");
        _texto.Append("                                        <td>&nbsp;</td>");
        _texto.Append("                                        <td style='text-align: center; width: 25px; border: 1px solid #000; border-top:none;'>");
        _texto.Append("                                            <b>" + contador.ToString() + "</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='text-align: center; width: 30px; border: 1px solid #000; border-left: none; border-top: none;'>");
        _texto.Append("                                            <b>(-)</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td colspan='2' style='border: 1px solid #000; border-left: none; border-top: none;'>");
        _texto.Append("                                            &nbsp;Débitos do Período de Apuração");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='text-align: right; width: 85px; border: 1px solid #000; border-left: none; border-top: none; background-color: #FDF5E6; ' class='background-force'>");
        _texto.Append("                                            " + valorTotalDebito.ToString("###,###,###,##0.00") + "&nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");

        contador += 1;

        _texto.Append("                                    <tr>");
        _texto.Append("                                        <td>&nbsp;</td>");
        _texto.Append("                                        <td style='text-align: center; width: 25px; border: 1px solid #000; border-top:none;'>");
        _texto.Append("                                            <b>" + contador.ToString() + "</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='text-align: center; width: 30px; border: 1px solid #000; border-left: none; border-top: none;'>");
        _texto.Append("                                            <b>(=)</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td colspan='2' style='border: 1px solid #000; border-left: none; border-top: none;'>");
        _texto.Append("                                            &nbsp;<b>Saldo Devedor do Período de Apuração - Código no " + imposto.CODIGO_DARF + "</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='text-align: right; width: 85px; border: 1px solid #000; border-left: none; border-top: none; background-color: #FDF5E6; ' class='background-force'>");
        _texto.Append("                                            <b>" + (((valorTotalDebito - valorTotal1) < 0) ? 0 : (valorTotalDebito - valorTotal1)).ToString("###,###,###,##0.00") + "</b>&nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");

        contador += 1;

        _texto.Append("                                    <tr>");
        _texto.Append("                                        <td>&nbsp;</td>");
        _texto.Append("                                        <td style='text-align: center; width: 25px; border: 1px solid #000; border-top:none;'>");
        _texto.Append("                                            <b>" + contador.ToString() + "</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='text-align: center; width: 30px; border: 1px solid #000; border-left: none; border-top: none;'>");
        _texto.Append("                                            <b>(=)</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td colspan='2' style='border: 1px solid #000; border-left: none; border-top: none;'>");
        _texto.Append("                                            &nbsp;<b>Saldo Credor do Período de Apuração</b>");
        _texto.Append("                                        </td>");

        decimal saldoCredorApurado = 0;
        if ((valorTotalDebito - valorTotal1) < 0)
            saldoCredorApurado = ((valorTotalDebito - valorTotal1) * (-1));

        _texto.Append("                                        <td style='text-align: right; width: 85px; border: 1px solid #000; border-left: none; border-top: none; background-color: #FDF5E6; ' class='background-force'>");
        _texto.Append("                                            <b>" + (saldoCredorApurado).ToString("###,###,###,##0.00") + "</b>&nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");

        _texto.Append("                                </table>");
        _texto.Append("                            </td>");
        _texto.Append("                        </tr>");

        _texto.Append("                    </table>");
        _texto.Append("                </td>");
        _texto.Append("            </tr>");
        _texto.Append("        </table>");
        _texto.Append("    </div>");
        _texto.Append("</body>");
        _texto.Append("</html>");

        if (calcula)
        {
            bool insert = false;

            CTB_APURACAO_EMPRESA_CALC apuracaoCalc = null;
            apuracaoCalc = contabilController.ObterApuracaoCalculo(codigoApuracaoEmpresa, imposto.CODIGO);
            if (apuracaoCalc == null)
            {
                apuracaoCalc = new CTB_APURACAO_EMPRESA_CALC();
                insert = true;
            }

            apuracaoCalc.CTB_APURACAO_EMPRESA = codigoApuracaoEmpresa;
            apuracaoCalc.CTB_IMPOSTO_TIPO = imposto.CODIGO;
            apuracaoCalc.ALIQUOTA = imposto.ALIQUOTA;
            apuracaoCalc.VALOR_DARF = ((valorTotalDebito - valorTotal1) < 0) ? 0 : (valorTotalDebito - valorTotal1);
            apuracaoCalc.DATA_INCLUSAO = DateTime.Now;
            apuracaoCalc.USUARIO_INCLUSAO = codigoUsuario;

            if (insert)
                contabilController.InserirApuracaoCalculo(apuracaoCalc);
            else
                contabilController.AtualizarApuracaoCalculo(apuracaoCalc);
        }

        return _texto;
    }
    #endregion

    #region "DARF"
    public StringBuilder MontarDARF(int codigoApuracaoEmpresa, string tipoImposto, bool print)
    {
        StringBuilder _texto = new StringBuilder();

        _texto = MontarDARFCabecalho(_texto, tipoImposto, print);
        _texto = MontarDARFCorpo(_texto, codigoApuracaoEmpresa, tipoImposto);
        _texto = MontarDARFCorpo(_texto, codigoApuracaoEmpresa, tipoImposto);
        _texto = MontarDARFRodape(_texto);

        return _texto;
    }
    private StringBuilder MontarDARFCabecalho(StringBuilder _texto, string tipoImposto, bool print)
    {
        _texto.Append("");
        _texto.Append("<!DOCTYPE HTML PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
        _texto.Append("<html>");
        _texto.Append("<head>");
        _texto.Append("    <title>DARF</title>");
        _texto.Append("    <meta charset='UTF-8' />");
        _texto.Append("    <style type='text/css'>");
        _texto.Append("        @media print {");
        _texto.Append("            .background-force {");
        _texto.Append("                -webkit-print-color-adjust: exact;");
        _texto.Append("            }");
        _texto.Append("        }");
        _texto.Append("    </style>");
        _texto.Append("</head>");
        _texto.Append("<body onload='" + ((print) ? "window.print();" : "") + "'>");
        _texto.Append("    <div id='divDARF' align='center'>");
        _texto.Append("        <table border='0' cellpadding='0' cellspacing='0' width='689' style='width: 517pt; padding: 0px; color: black;");
        _texto.Append("           font-size: 7.5pt; font-family: Arial; background: white;'>");
        _texto.Append("            <tr>");
        _texto.Append("                <td style='line-height: 10px;'>");
        _texto.Append("                    <table border='0' cellpadding='0' cellspacing='0' width='689' style='width: 517pt' class='background-force'>");

        var imposto = new ContabilidadeController().ObterImpostoTipo(tipoImposto);

        _texto.Append("                        <tr>");
        _texto.Append("                            <td colspan='2' valign='middle' style='line-height:20px; text-align: left; border-bottom: 1px solid #000; background-color: #FDF5E6;'>");
        _texto.Append("                                <h3>DARF - " + imposto.TIPO + " Não Cumulativo - Código: " + imposto.CODIGO_DARF.Replace("DARF", "").Trim() + "</h3>");
        _texto.Append("                            </td>");
        _texto.Append("                        </tr>");
        _texto.Append("                        <tr>");
        _texto.Append("                            <td colspan='2' style='line-height:35px;'>");
        _texto.Append("                                &nbsp;");
        _texto.Append("                            </td>");
        _texto.Append("                        </tr>");

        return _texto;
    }
    private StringBuilder MontarDARFRodape(StringBuilder _texto)
    {
        _texto.Append("");
        _texto.Append("                    </table>");
        _texto.Append("                </td>");
        _texto.Append("            </tr>");
        _texto.Append("        </table>");
        _texto.Append("    </div>");
        _texto.Append("</body>");
        _texto.Append("</html>");

        return _texto;
    }
    private StringBuilder MontarDARFCorpo(StringBuilder _texto, int codigoApuracaoEmpresa, string tipoImposto)
    {
        ContabilidadeController contabilController = new ContabilidadeController();

        var empresaApuracao = contabilController.ObterApuracaoEmpresa(codigoApuracaoEmpresa);
        var imposto = contabilController.ObterImpostoTipo(tipoImposto);
        var apuracaoCalc = contabilController.ObterApuracaoCalculo(codigoApuracaoEmpresa, imposto.CODIGO);

        _texto.Append("");
        _texto.Append("                        <tr>");
        _texto.Append("                            <td style='width: 400px'>");
        _texto.Append("                                <table border='0' cellpadding='0' cellspacing='0' width='100%' class='background-force'>");
        _texto.Append("                                    <tr>");
        _texto.Append("                                        <td style='width: 70px; border: 1px solid #000; border-right:none;'>");
        _texto.Append("                                            &nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='border: 1px solid #000; border-left:none;'>");
        _texto.Append("                                            <p>");
        _texto.Append("                                                <b>MINISTÉRIO DA FAZENDA</b><br />");
        _texto.Append("                                                SECRETARIA DA RECEITA FEDERAL<br />");
        _texto.Append("                                                <span style='font-size:9px;'>Documento de Arrecadação de Receitas Federais</span>");
        _texto.Append("                                            </p>");
        _texto.Append("                                            <p>");
        _texto.Append("                                                <h1>DARF</h1>");
        _texto.Append("                                            </p>");
        _texto.Append("                                            <br />");
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");
        _texto.Append("                                    <tr>");
        _texto.Append("                                        <td colspan='2' style='border: 1px solid #000; border-top:none; border-bottom:none;'>");
        _texto.Append("                                            &nbsp;<span style='font-size:5pt;'><b>01</b> - NOME / TELEFONE</span>");
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");
        _texto.Append("                                    <tr style='line-height:24px;'>");
        _texto.Append("                                        <td colspan='2' style='border: 1px solid #000; border-top:none; border-bottom:none;'>");
        _texto.Append("                                            &nbsp;<b>" + empresaApuracao.EMPRESA1.NOME + "</b>");
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");
        _texto.Append("                                    <tr style='line-height:10px;'>");
        _texto.Append("                                        <td colspan='2' style='border: 1px solid #000; border-top:none; border-bottom:none;'>");
        _texto.Append("                                            &nbsp;Telefone: &nbsp;&nbsp; 11 5180-3460");
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");
        _texto.Append("                                    <tr style=''>");
        _texto.Append("                                        <td colspan='2' style='border: 1px solid #000; border-bottom:none; text-align:center;' valign='bottom'>");
        _texto.Append("                                            <br /><br />");
        _texto.Append("                                            <span style='font-size:9px;'>Veja no Verso</span>");
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");
        _texto.Append("                                    <tr style=''>");
        _texto.Append("                                        <td colspan='2' style='border: 1px solid #000; border-top: none;  border-bottom: none; text-align: center;' valign='top'>");
        _texto.Append("                                            <span style='font-size:9px;'>Instruções para preenchimento</span>");
        _texto.Append("                                            <br /><br /><br />");
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");
        _texto.Append("                                    <tr style='line-height:20px;'>");
        _texto.Append("                                        <td colspan='2' style='border: 1px solid #000; border-bottom:none; text-align:center;' valign='middle'>");
        _texto.Append("                                            <p style='font-size:11px;'><b>ATENÇÃO</b></p>");
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");
        _texto.Append("                                    <tr style=''>");
        _texto.Append("                                        <td colspan='2' style='border: 1px solid #000; border-bottom:none; text-align:justify;' valign='middle'>");
        _texto.Append("                                            <p style='font-size:7.5pt;'>");
        _texto.Append("                                                É vedado o recolhimento de tributos e contribuições administrados pela Secretaria da Receita Federal cujo valor total seja inferior a R$ 10,00. Ocorrendo tal situação, adicione esse valor ao tributo/contribuição de mesmo código de períodos subseqüentes, até que o total seja igual ou superior a R$ 10,00.");
        _texto.Append("                                            </p>");
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");
        _texto.Append("                                    <tr style=''>");
        _texto.Append("                                        <td colspan='2' style='border: 1px solid #000; border-bottom:none;'>");
        _texto.Append("                                            <span style='font-size:7px;'>");
        _texto.Append("                                                Aprovado pela IN/SRF Nº 81/96.");
        _texto.Append("                                            </span>");
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");
        _texto.Append("                                </table>");
        _texto.Append("                            </td>");
        _texto.Append("                            <td valign='top'>");
        _texto.Append("                                <table border='0' cellpadding='0' cellspacing='0' style='width: 100%' class='background-force'>");
        _texto.Append("                                    <tr>");
        _texto.Append("                                        <td valign='top' style='border-top: 1px solid #000;'>");
        _texto.Append("                                            &nbsp;<span style='font-size:5pt;'><b>02</b> PERÍODO DE APURAÇÃO</span>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td rowspan='2' valign='middle' style='border: 1px solid #000; border-bottom:none; text-align:center;'>");
        _texto.Append("                                            " + empresaApuracao.COMPETENCIA.ToString("dd.MM.yyyy"));
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");
        _texto.Append("                                    <tr>");
        _texto.Append("                                        <td style='line-height:16px;'>&nbsp;</td>");
        _texto.Append("                                    </tr>");
        _texto.Append("                                    <tr>");
        _texto.Append("                                        <td valign='top' style='border-top: 1px solid #000;'>");
        _texto.Append("                                            &nbsp;<span style='font-size:5pt;'><b>03</b> NÚMERO DO CPF OU CNPJ</span>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td rowspan='2' valign='middle' style='border: 1px solid #000; border-bottom:none; text-align:center;'>");
        _texto.Append("                                            " + empresaApuracao.EMPRESA1.CNPJ);
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");
        _texto.Append("                                    <tr>");
        _texto.Append("                                        <td style='line-height:16px;'>&nbsp;</td>");
        _texto.Append("                                    </tr>");
        _texto.Append("                                    <tr>");
        _texto.Append("                                        <td valign='top' style='border-top: 1px solid #000;'>");
        _texto.Append("                                            &nbsp;<span style='font-size:5pt;'><b>04</b> CÓDIGO DA RECEITA</span>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td rowspan='2' valign='middle' style='border: 1px solid #000; border-bottom:none; text-align:center;'>");
        _texto.Append("                                            " + imposto.CODIGO_DARF.Replace("DARF", "").Trim());
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");
        _texto.Append("                                    <tr>");
        _texto.Append("                                        <td style='line-height:16px;'>&nbsp;</td>");
        _texto.Append("                                    </tr>");
        _texto.Append("                                    <tr>");
        _texto.Append("                                        <td valign='top' style='border-top: 1px solid #000;'>");
        _texto.Append("                                            &nbsp;<span style='font-size:5pt;'><b>05</b> NÚMERO DE REFERÊNCIA</span>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td rowspan='2' valign='middle' style='border: 1px solid #000; border-bottom:none; text-align:center;'>");
        _texto.Append("                                            &nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");
        _texto.Append("                                    <tr>");
        _texto.Append("                                        <td style='line-height:16px;'>&nbsp;</td>");
        _texto.Append("                                    </tr>");
        _texto.Append("                                    <tr>");
        _texto.Append("                                        <td valign='top' style='border-top: 1px solid #000;'>");
        _texto.Append("                                            &nbsp;<span style='font-size:5pt;'><b>06</b> DATA DE VENCIMENTO</span>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td rowspan='2' valign='middle' style='border: 1px solid #000; border-bottom:none; text-align:center;'>");
        _texto.Append("                                            &nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");
        _texto.Append("                                    <tr>");
        _texto.Append("                                        <td style='line-height:16px;'>&nbsp;</td>");
        _texto.Append("                                    </tr>");
        _texto.Append("                                    <tr>");
        _texto.Append("                                        <td valign='top' style='border-top: 1px solid #000;'>");
        _texto.Append("                                            &nbsp;<span style='font-size:5pt;'><b>07</b> VALOR PRINCIPAL</span>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td rowspan='2' valign='middle' style='border: 1px solid #000; border-bottom:none; text-align:right;'>");
        _texto.Append("                                            " + ((apuracaoCalc == null) ? "0,00" : apuracaoCalc.VALOR_DARF.ToString("###,###,###,##0.00")) + "&nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");
        _texto.Append("                                    <tr>");
        _texto.Append("                                        <td style='line-height:16px;'>&nbsp;</td>");
        _texto.Append("                                    </tr>");
        _texto.Append("                                    <tr>");
        _texto.Append("                                        <td valign='top' style='border-top: 1px solid #000;'>");
        _texto.Append("                                            &nbsp;<span style='font-size:5pt;'><b>08</b> VALOR DA MULTA</span>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td rowspan='2' valign='middle' style='border: 1px solid #000; border-bottom:none; text-align:right;'>");
        _texto.Append("                                            &nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");
        _texto.Append("                                    <tr>");
        _texto.Append("                                        <td style='line-height:16px;'>&nbsp;</td>");
        _texto.Append("                                    </tr>");
        _texto.Append("                                    <tr>");
        _texto.Append("                                        <td valign='top' style='border-top: 1px solid #000;'>");
        _texto.Append("                                            &nbsp;<span style='font-size:5pt;'><b>09</b> VALOR DOS JUROS E/OU<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ENCARGOS DL - 1025/69</span>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td rowspan='2' valign='middle' style='border: 1px solid #000; border-bottom:none; text-align:right;'>");
        _texto.Append("                                            &nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");
        _texto.Append("                                    <tr>");
        _texto.Append("                                        <td style='line-height:6px;'>&nbsp;</td>");
        _texto.Append("                                    </tr>");
        _texto.Append("                                    <tr>");
        _texto.Append("                                        <td valign='top' style='border-top: 1px solid #000;'>");
        _texto.Append("                                            &nbsp;<span style='font-size:5pt;'><b>10</b> VALOR TOTAL</span>");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td rowspan='2' valign='middle' style='border: 1px solid #000; border-bottom:none; text-align:right;'>");
        _texto.Append("                                            " + ((apuracaoCalc == null) ? "0,00" : apuracaoCalc.VALOR_DARF.ToString("###,###,###,##0.00")) + "&nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");
        _texto.Append("                                    <tr>");
        _texto.Append("                                        <td style='line-height:16px;'>&nbsp;</td>");
        _texto.Append("                                    </tr>");
        _texto.Append("                                    <tr>");
        _texto.Append("                                        <td colspan='2' valign='top' style='border-top: 1px solid #000; border-right: 1px solid #000; '>");
        _texto.Append("                                            &nbsp;<span style='font-size:5pt;'><b>11</b> AUTENTICAÇÃO BANCÁRIA (Somente nas 1ª e 2ª vias()</span>");
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");
        _texto.Append("                                    <tr>");
        _texto.Append("                                        <td colspan='2' valign='top' style='line-height:46px; border-right: 1px solid #000; '>");
        _texto.Append("                                            &nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");
        _texto.Append("                                </table>");
        _texto.Append("                            </td>");
        _texto.Append("                        </tr>");
        _texto.Append("                        <tr>");
        _texto.Append("                            <td colspan='2'>");
        _texto.Append("                                <br /><br />");
        _texto.Append("                                <hr style='border-top: dotted 1px;' />");
        _texto.Append("                                <br /><br />");
        _texto.Append("                            </td>");
        _texto.Append("                        </tr>");

        return _texto;
    }
    #endregion

}