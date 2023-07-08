using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using DAL;
using Relatorios.mod_producao.relatorios;

public class RelatorioHB
{
    public int CODIGO { get; set; }
    public string HB { get; set; }
    public string COLECAO { get; set; }
    public string COD_COR { get; set; }
    public string DESC_COR { get; set; }
    public string COR_FORNECEDOR { get; set; }
    public string GRUPO { get; set; }
    public string NOME { get; set; }
    public string DATA { get; set; }
    public string GRUPO_TECIDO { get; set; }
    public string TECIDO { get; set; }
    public string FORNECEDOR { get; set; }
    public string LARGURA { get; set; }
    public string LIQUIDAR { get; set; }
    public string LIQUIDAR_PQNO { get; set; }
    public string MODELAGEM { get; set; }
    public string FOTO_PECA { get; set; }
    public string FOTO_TECIDO { get; set; }
    public string CUSTO_TECIDO { get; set; }
    public string OBS { get; set; }
    public string UNIDADE_MEDIDA { get; set; }
    public string RETALHO { get; set; }
    public string PORPECA { get; set; }
    public string PORCORTE { get; set; }
    public string CODIGO_PRODUTO_LINX { get; set; }
    public string NUMERO_PEDIDO { get; set; }
    public string PC_VAREJO { get; set; }
    public string PC_ATACADO { get; set; }
    public string AMPLIACAO_OUTRO { get; set; }
    public string MOLDE { get; set; }
    public string MOSTRUARIO { get; set; }
    public string PROD_GRADE { get; set; }
    public string ORDEM_PRODUCAO { get; set; }
    public string VOLUME { get; set; }
    public string GABARITO { get; set; }
    public string DATA_IMP_FIC_LOGISTICA { get; set; }
    public string GRIFFE { get; set; }
    public string SIGNED { get; set; }
    public string SIGNED_NOME { get; set; }
    public string KIDS { get; set; }
    public string ESTAMPARIA { get; set; }

    public string NOME_AMPLIACAO { get; set; }
    public string NOME_RISCO { get; set; }
    public string NOME_MODELAGEM { get; set; }

    public string FaseEstamparia { get; set; }
    public string FaseLavanderia { get; set; }
    public List<PROD_HB_COMPOSICAO> COMPOSICAO { get; set; }
    public PROD_HB_GRADE GRADE { get; set; }
    public PROD_HB_GRADE GRADEREAL { get; set; }
    public PROD_HB_GRADE GRADEATACADO { get; set; }
    public List<PROD_HB> DETALHE { get; set; }
    public List<PROD_HB_PROD_AVIAMENTO> AVIAMENTO { get; set; }
    public PROD_HB_PROD_PROCESSO PROCESSO { get; set; }
    public List<SP_OBTER_GRADE_CORTEResult> GRADE_CORTE { get; set; }
    public List<PROD_HB_AMPLIACAO_MEDIDA> AMPLIACAOMEDIDA { get; set; }

    public StringBuilder MontarCabecalho(StringBuilder _texto, string titulo)
    {
        _texto.AppendLine("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
        _texto.AppendLine(" <html>");
        _texto.AppendLine("     <head>");
        _texto.AppendLine("         <title>" + titulo + "</title>   ");
        _texto.AppendLine("         <meta charset='UTF-8'>          ");
        _texto.AppendLine("         <style type='text/css'>");
        _texto.AppendLine("             .breakafter");
        _texto.AppendLine("             {");
        _texto.AppendLine("                 page-break-after: always;");
        _texto.AppendLine("             }");
        _texto.AppendLine("             .breakbefore");
        _texto.AppendLine("             {");
        _texto.AppendLine("                 page-break-before: always;");
        _texto.AppendLine("             }");
        _texto.AppendLine("         </style>");
        _texto.AppendLine("     </head>");
        _texto.AppendLine("");
        _texto.AppendLine("<body onLoad='window.print();'>");
        return _texto;
    }
    public StringBuilder MontarRodape(StringBuilder _texto)
    {
        _texto.AppendLine("</body></html>");
        return _texto;
    }

    #region "AMPLIACAO"
    public StringBuilder MontarAmpliacao(StringBuilder _texto, RelatorioHB _relatorio, string breakClass)
    {
        var _gradeNome = new ProducaoController().ObterGradeNome(Convert.ToInt32(_relatorio.PROD_GRADE));

        _texto.Append("    <div id='divAmpliacao' align='center' class='" + breakClass + "'>");
        _texto.Append("        <table border='0' cellpadding='0' cellspacing='0' width='689' style='width: 517pt;");
        _texto.Append("            padding: 0px; color: black; font-size: 10.5pt; font-weight: 700; font-family: Arial, sans-serif;");
        _texto.Append("            background: white; white-space: nowrap;'>");
        _texto.Append("            <tr>");
        _texto.Append("                <td style='line-height: 30px;'>");
        _texto.Append("                    <table border='1' cellpadding='0' cellspacing='0' width='689' style='border-collapse: collapse;");
        _texto.Append("                        width: 517pt'>");
        _texto.Append("                        <tr>");
        _texto.Append("                            <td colspan='5' style='text-align: center;'>");
        _texto.Append("                                <h2>");
        _texto.Append("                                    MEDIDAS A SEREM SEGUIDAS");
        _texto.Append("                                    <br />");
        _texto.Append("                                    NA FABRICAÇÃO DE PEÇAS</h2>");
        _texto.Append("                            </td>");
        _texto.Append("                        </tr>");
        _texto.Append("                        <tr style='border-right: 1px solid #000; border-top: 1px solid #000;'>");
        _texto.Append("                            <td style='width: 150px; border-right: 1px solid #000; border-left: 1px solid #000;'>");
        _texto.Append("                                &nbsp;NOME");
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='border-right: 1px solid #000;'>");
        _texto.Append("                                &nbsp; " + _relatorio.NOME);
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='border-right: 1px solid #000; border-top: 0px solid #000;'>");
        _texto.Append("                                &nbsp;");
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='width: 150px; border-right: 1px solid #000;'>");
        _texto.Append("                                &nbsp;HB");
        _texto.Append("                            </td>");
        _texto.Append("                            <td>");
        _texto.Append("                                &nbsp; " + _relatorio.HB.ToString());
        _texto.Append("                            </td>");
        _texto.Append("                        </tr>");
        _texto.Append("                        <tr style='border: 1px solid #000; border-top: 0px solid #000;'>");
        _texto.Append("                            <td style='border-right: 1px solid #000; border-top: 1px solid #000; text-align: left'>");
        _texto.Append("                                &nbsp;MOSTRUÁRIO");
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='border-right: 1px solid #000; border-top: 1px solid #000; text-align: left'>");
        _texto.Append("                                &nbsp; " + ((_relatorio.MOSTRUARIO == "S") ? "SIM" : "NÃO"));
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='border-right: 1px solid #000; border-top: 0px solid #000;border-bottom: 0px solid #000;'>");
        _texto.Append("                                &nbsp;&nbsp;&nbsp;&nbsp;");
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='border-right: 1px solid #000; border-top: 1px solid #000; text-align: left'>");
        _texto.Append("                                &nbsp;TECIDO");
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='border-top: 1px solid #000;'>");
        _texto.Append("                                &nbsp; " + (_relatorio.TECIDO + ((_relatorio.COR_FORNECEDOR.Trim() == "") ? "" : " - " + _relatorio.COR_FORNECEDOR.Trim())));
        _texto.Append("                            </td>");
        _texto.Append("                        </tr>");

        _texto.Append("                        <tr style='border: 1px solid #000; border-top: 0px solid #000;'>");
        _texto.Append("                            <td style='border-right: 1px solid #000; border-top: 1px solid #000; text-align: left'>");
        _texto.Append("                                &nbsp;GABARITO");
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='border-right: 1px solid #000; border-top: 1px solid #000; text-align: left'>");
        _texto.Append("                                &nbsp; " + ((_relatorio.GABARITO == "S") ? "SIM" : "NÃO"));
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='border-right: 1px solid #000; border-top: 0px solid #000;border-bottom: 0px solid #000;'>");
        _texto.Append("                                &nbsp;&nbsp;&nbsp;&nbsp;");
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='border-right: 1px solid #000; border-top: 1px solid #000; text-align: left'>");
        _texto.Append("                                &nbsp;ESTAMPARIA");
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='border-top: 1px solid #000;'>");
        _texto.Append("                                &nbsp; " + ((_relatorio.ESTAMPARIA == "S") ? "SIM" : "NÃO"));
        _texto.Append("                            </td>");
        _texto.Append("                        </tr>");

        _texto.Append("                    </table>");
        _texto.Append("                </td>");
        _texto.Append("            </tr>");
        _texto.Append("            <tr>");
        _texto.Append("                <td style='line-height: 23px;'>");
        _texto.Append("                    <table border='0' cellpadding='0' cellspacing='0' width='689' style='border-collapse: collapse;");
        _texto.Append("                        width: 517pt'>");
        _texto.Append("                        <tr>");
        _texto.Append("                            <td colspan='9' style='text-align: center; border-right: 1px solid #000;border-left: 1px solid #000;'>");
        _texto.Append("                                <h3>");
        _texto.Append("                                    MEDIDAS DO ELÁSTICO</h3>");
        _texto.Append("                            </td>");
        _texto.Append("                        </tr>");
        _texto.Append("                        <tr style='text-align: center; border-top: 1px solid #000; border-bottom: 1px solid #000;'>");
        _texto.Append("                            <td style='width: 150px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.Append("                                LOCAL");
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='width: 60px; border-right: 1px solid #000;'>");
        _texto.Append("                                " + ((_gradeNome != null) ? _gradeNome.GRADE_EXP : "EXP"));
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='width: 60px; border-right: 1px solid #000;'>");
        _texto.Append("                                " + ((_gradeNome != null) ? _gradeNome.GRADE_XP : "XP"));
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='width: 60px; border-right: 1px solid #000;'>");
        _texto.Append("                                " + ((_gradeNome != null) ? _gradeNome.GRADE_PP : "PP"));
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='width: 60px; border-right: 1px solid #000;'>");
        _texto.Append("                                " + ((_gradeNome != null) ? _gradeNome.GRADE_P : "P"));
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='width: 60px; border-right: 1px solid #000;'>");
        _texto.Append("                                " + ((_gradeNome != null) ? _gradeNome.GRADE_M : "M"));
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='width: 60px; border-right: 1px solid #000;'>");
        _texto.Append("                                " + ((_gradeNome != null) ? _gradeNome.GRADE_G : "G"));
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='width: 60px; border-right: 1px solid #000;'>");
        _texto.Append("                                " + ((_gradeNome != null) ? _gradeNome.GRADE_GG : "GG"));
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='border-right: 1px solid #000;'>");
        _texto.Append("                                LARGURA");
        _texto.Append("                            </td>");
        _texto.Append("                        </tr>");
        foreach (PROD_HB_AMPLIACAO_MEDIDA medidaElastico in _relatorio.AMPLIACAOMEDIDA.Where(p => p.PROD_HB_AMPLIACAO == 1))
        {
            _texto.Append("                        <tr style='text-align: center; border-bottom: 1px solid #000;'>");
            _texto.Append("                            <td style='border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.Append("                                " + medidaElastico.LOCAL);
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                " + medidaElastico.GRADE_EXP);
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                " + medidaElastico.GRADE_XP);
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                " + medidaElastico.GRADE_PP);
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                " + medidaElastico.GRADE_P);
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                " + medidaElastico.GRADE_M);
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                " + medidaElastico.GRADE_G);
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                " + medidaElastico.GRADE_GG);
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                " + medidaElastico.LARGURA);
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
        }
        _texto.Append("                    </table>");
        _texto.Append("                </td>");
        _texto.Append("            </tr>");
        _texto.Append("            <tr>");
        _texto.Append("                <td style='line-height: 23px;'>");
        _texto.Append("                    <table border='0' cellpadding='0' cellspacing='0' width='689' style='border-collapse: collapse;");
        _texto.Append("                        width: 517pt'>");
        _texto.Append("                        <tr>");
        _texto.Append("                            <td colspan='4' style='text-align: center; border-right: 1px solid #000;border-left: 1px solid #000;'>");
        _texto.Append("                                <h3>");
        _texto.Append("                                    MEDIDAS DO GALÃO</h3>");
        _texto.Append("                            </td>");
        _texto.Append("                        </tr>");
        _texto.Append("                        <tr style='text-align: center; border-top: 1px solid #000;'>");
        _texto.Append("                            <td style='width: 150px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.Append("                                QUANTIDADE");
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='width: 150px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.Append("                                LOCAL");
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='width: 150px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.Append("                                COMPRIMENTO");
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='border-right: 1px solid #000;'>");
        _texto.Append("                                LARGURA");
        _texto.Append("                            </td>");
        _texto.Append("                        </tr>");
        string descricaoGalao = "";
        foreach (PROD_HB_AMPLIACAO_MEDIDA medidaGalao in _relatorio.AMPLIACAOMEDIDA.Where(p => p.PROD_HB_AMPLIACAO == 2))
        {
            _texto.Append("                        <tr style='text-align: center; border-top: 1px solid #000;'>");
            _texto.Append("                            <td style='border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.Append("                                " + medidaGalao.QTDE);
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                " + medidaGalao.LOCAL);
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                " + medidaGalao.COMPRIMENTO);
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                " + medidaGalao.LARGURA);
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");

            descricaoGalao = medidaGalao.DESCRICAO;
        }
        _texto.Append("                        <tr>");
        _texto.Append("                            <td colspan='4' style='border: 1px solid #000;'>");
        _texto.Append("                                &nbsp;" + descricaoGalao);
        _texto.Append("                            </td>");
        _texto.Append("                        </tr>");
        _texto.Append("                    </table>");
        _texto.Append("                </td>");
        _texto.Append("            </tr>");
        _texto.Append("            <tr>");
        _texto.Append("                <td style='line-height: 23px;'>");
        _texto.Append("                    <table border='0' cellpadding='0' cellspacing='0' width='689' style='border-collapse: collapse;");
        _texto.Append("                        width: 517pt'>");
        _texto.Append("                        <tr>");
        _texto.Append("                            <td colspan='7' style='text-align: center; border-right: 1px solid #000;border-left: 1px solid #000;'>");
        _texto.Append("                                <h3>");
        _texto.Append("                                    CORTAR ALÇA C/</h3>");
        _texto.Append("                            </td>");
        _texto.Append("                        </tr>");
        _texto.Append("                        <tr style='text-align: center; border-top: 1px solid #000; border-bottom: 1px solid #000;'>");
        _texto.Append("                            <td style='width: 60px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.Append("                                " + ((_gradeNome != null) ? _gradeNome.GRADE_EXP : "EXP"));
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='width: 60px; border-right: 1px solid #000;'>");
        _texto.Append("                                " + ((_gradeNome != null) ? _gradeNome.GRADE_XP : "XP"));
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='width: 60px; border-right: 1px solid #000;'>");
        _texto.Append("                                " + ((_gradeNome != null) ? _gradeNome.GRADE_PP : "PP"));
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='width: 60px; border-right: 1px solid #000;'>");
        _texto.Append("                                " + ((_gradeNome != null) ? _gradeNome.GRADE_P : "P"));
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='width: 60px; border-right: 1px solid #000;'>");
        _texto.Append("                                " + ((_gradeNome != null) ? _gradeNome.GRADE_M : "M"));
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='width: 60px; border-right: 1px solid #000;'>");
        _texto.Append("                                " + ((_gradeNome != null) ? _gradeNome.GRADE_G : "G"));
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='width: 60px; border-right: 1px solid #000;'>");
        _texto.Append("                                " + ((_gradeNome != null) ? _gradeNome.GRADE_GG : "GG"));
        _texto.Append("                            </td>");
        _texto.Append("                        </tr>");
        foreach (PROD_HB_AMPLIACAO_MEDIDA medidaAlcaPronta in _relatorio.AMPLIACAOMEDIDA.Where(p => p.PROD_HB_AMPLIACAO == 3))
        {
            _texto.Append("                        <tr style='text-align: center; border-bottom: 1px solid #000;'>");
            _texto.Append("                            <td style='border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.Append("                                " + medidaAlcaPronta.GRADE_EXP);
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                " + medidaAlcaPronta.GRADE_XP);
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                " + medidaAlcaPronta.GRADE_PP);
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                " + medidaAlcaPronta.GRADE_P);
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                " + medidaAlcaPronta.GRADE_M);
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                " + medidaAlcaPronta.GRADE_G);
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                " + medidaAlcaPronta.GRADE_GG);
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
        }
        _texto.Append("                    </table>");
        _texto.Append("                </td>");
        _texto.Append("            </tr>");
        _texto.Append("            <tr>");
        _texto.Append("                <td style='line-height: 23px;'>");
        _texto.Append("                    <table border='0' cellpadding='0' cellspacing='0' width='689' style='border-collapse: collapse;");
        _texto.Append("                        width: 517pt'>");
        _texto.Append("                        <tr>");
        _texto.Append("                            <td style='text-align: center; border-right: 1px solid #000;border-left: 1px solid #000;'>");
        _texto.Append("                                <h3>");
        _texto.Append("                                    OUTROS</h3>");
        _texto.Append("                            </td>");
        _texto.Append("                        </tr>");
        _texto.Append("                        <tr style='text-align: left; border-top: 1px solid #000; border-bottom: 1px solid #000;'>");
        _texto.Append("                            <td style='border-left: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.Append("                                <textarea style='width: 680px; height: 150px; overflow: hidden;'>" + _relatorio.AMPLIACAO_OUTRO + "</textarea>");
        _texto.Append("                            </td>");
        _texto.Append("                        </tr>");
        _texto.Append("                    </table>");
        _texto.Append("                </td>");
        _texto.Append("            </tr>");
        _texto.Append("        </table>");
        _texto.Append("    </div>");

        return _texto;
    }
    #endregion

    #region "RISCO"
    public StringBuilder MontarTecido(StringBuilder _texto, RelatorioHB _relatorio)
    {
        ProducaoController prodController = new ProducaoController();

        _texto.Append("<div id='fichaCorte' align='center'>");
        _texto.Append("<table border='0' cellpadding='0' cellspacing='0' width='689' style='width: 517pt; padding: 0px; color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif; background: white; white-space: nowrap;'>");
        _texto.Append("<tr>");
        _texto.Append("     <td style='line-height: 16px;'>");
        _texto.Append("     <table border='0' cellpadding='0' cellspacing='0' width='689' style='border-collapse: collapse;");
        _texto.Append("     width: 517pt'>");
        _texto.Append("         <tr style='border-right: 1px solid #000; border-top: 2px solid #000;'>");
        _texto.Append("             <td style='width: 70px; border-right: 1px solid #000; border-left: 1px solid #000;'>");
        _texto.Append("                 HB");
        _texto.Append("             </td>");
        _texto.Append("             <td style='width: 150px; border-right: 1px solid #000;'>");
        _texto.Append("                 GRUPO");
        _texto.Append("             </td>");
        _texto.Append("             <td style='width: 202px; border-right: 1px solid #000;'>");
        _texto.Append("                 NOME");
        _texto.Append("             </td>");
        _texto.Append("             <td style='border-right: 1px solid #000;'>");
        _texto.Append("                 DATA");
        _texto.Append("             </td>");
        _texto.Append("             <td style='border-right: 1px solid #000;'>");
        _texto.Append("                 MOLDE");
        _texto.Append("             </td>");
        _texto.Append("             <td>");
        _texto.Append("                 GRIFFE");
        _texto.Append("             </td>");
        _texto.Append("         </tr>");
        _texto.Append("         <tr style='border: 1px solid #000; border-top: none;'>");
        _texto.Append("             <td style='border-right: 1px solid #000; text-align: center'>");
        _texto.Append("                 " + _relatorio.HB);
        _texto.Append("             </td>");
        _texto.Append("             <td style='border-right: 1px solid #000; text-align: center'>");
        _texto.Append("                 " + _relatorio.GRUPO);
        _texto.Append("             </td>");
        _texto.Append("             <td style='border-right: 1px solid #000; text-align: center'>");
        _texto.Append("                 " + _relatorio.CODIGO_PRODUTO_LINX + " - " + _relatorio.NOME);
        _texto.Append("             </td>");
        _texto.Append("             <td style='border-right: 1px solid #000; text-align: center'>");
        _texto.Append("                 " + _relatorio.DATA);
        _texto.Append("             </td>");
        _texto.Append("             <td style='border-right: 1px solid #000; text-align: center'>");
        _texto.Append("                 " + _relatorio.MOLDE);
        _texto.Append("             </td>");
        _texto.Append("             <td style='text-align: center'>");
        _texto.Append("                 " + _relatorio.GRIFFE);
        _texto.Append("             </td>");
        _texto.Append("         </tr>");
        _texto.Append("     </table>");
        _texto.Append(" </td>");
        _texto.Append("</tr>");
        _texto.Append("<tr>");
        _texto.Append(" <td style='line-height: 18px;'>");
        _texto.Append("     <table border='0' cellpadding='0' cellspacing='0' width='689' style='border-collapse: collapse;");
        _texto.Append("     width: 517pt'>");
        _texto.Append("         <tr>");
        _texto.Append("             <td style='width: 221px; border-right: 1px solid #000; border-left: 1px solid #000;'>");
        _texto.Append("                 TECIDO - " + RetornarUnidadeMedidaMaterial(_relatorio.GRUPO_TECIDO, _relatorio.TECIDO));
        _texto.Append("             </td>");
        _texto.Append("             <td style='width: 100px; border-right: 1px solid #000;'>");
        _texto.Append("                 COR");
        _texto.Append("             </td>");
        if (_relatorio.SIGNED == "S")
        {
            _texto.Append("             <td style='width: 201px; border-right: 1px solid #000;'>");
            _texto.Append("                 FORNECEDOR");
            _texto.Append("             </td>");
            _texto.Append("             <td style='border-right: 1px solid #000;'>");
            _texto.Append("                 LARGURA");
            _texto.Append("             </td>");
            _texto.Append("             <td style='border-right: 1px solid #000;'>");
            _texto.Append("                 SIGNED");
            _texto.Append("             </td>");
        }
        else
        {
            _texto.Append("             <td style='width: 253px; border-right: 1px solid #000;'>");
            _texto.Append("                 FORNECEDOR");
            _texto.Append("             </td>");
            _texto.Append("             <td style='border-right: 1px solid #000;'>");
            _texto.Append("                 LARGURA");
            _texto.Append("             </td>");
        }
        _texto.Append("         </tr>");
        _texto.Append("         <tr style='border-right: 1px solid #000;'>");
        _texto.Append("             <td style='border-right: 1px solid #000; border-left: 1px solid #000; text-align: center;'>");
        _texto.Append("                 " + (_relatorio.TECIDO + ((_relatorio.COR_FORNECEDOR.Trim() == "") ? "" : " - " + _relatorio.COR_FORNECEDOR.Trim())));
        _texto.Append("             </td>");
        _texto.Append("             <td style='border-right: 1px solid #000; text-align: center;'>");
        _texto.Append("                 " + _relatorio.DESC_COR);
        _texto.Append("             </td>");
        _texto.Append("             <td style='border-right: 1px solid #000; text-align: center;'>");
        _texto.Append("                 " + _relatorio.FORNECEDOR);
        _texto.Append("             </td>");
        if (_relatorio.SIGNED == "S")
        {
            _texto.Append("             <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.Append("                 " + _relatorio.LARGURA);
            _texto.Append("             </td>");
            _texto.Append("             <td style='text-align: center'>");
            _texto.Append("                 " + _relatorio.SIGNED_NOME);
            _texto.Append("             </td>");
        }
        else
        {
            _texto.Append("             <td style='text-align: center'>");
            _texto.Append("                 " + _relatorio.LARGURA);
            _texto.Append("             </td>");
        }
        _texto.Append("         </tr>");
        _texto.Append("     </table>");
        _texto.Append(" </td>");
        _texto.Append("</tr>");

        if (_relatorio.MOSTRUARIO == "N")
        {
            decimal gastoMostruario = 0;
            var g = prodController.ObterHBGastoMostruario(_relatorio.CODIGO_PRODUTO_LINX);
            if (g != null)
                gastoMostruario = g.GASTO_MOSTRUARIO;

            _texto.AppendLine("<tr>");
            _texto.AppendLine(" <td style='line-height: 18px;'>");
            _texto.AppendLine("     <table border='0' cellpadding='0' cellspacing='0' width='689' style='border-collapse: collapse;");
            _texto.AppendLine("     width: 517pt'>");
            _texto.AppendLine("         <tr>");
            _texto.AppendLine("             <td style='width: 221px; border-right: 1px solid #000; border-left: 1px solid #000; border-top: 1px solid #000;'>");
            _texto.AppendLine("                 GASTO MOSTRUÁRIO");
            _texto.AppendLine("             </td>");
            _texto.AppendLine("         </tr>");

            _texto.AppendLine("         <tr style='border-right: 1px solid #000;'>");
            _texto.AppendLine("             <td style='border-right: 1px solid #000; border-left: 1px solid #000; text-align: left;'>");
            _texto.AppendLine("                 &nbsp;" + gastoMostruario.ToString("###,###,###0.000"));
            _texto.AppendLine("             </td>");
            _texto.AppendLine("         </tr>");
            _texto.AppendLine("     </table>");
            _texto.AppendLine(" </td>");
            _texto.AppendLine("</tr>");
        }

        return _texto;
    }
    public StringBuilder MontarComposicao(StringBuilder _texto, RelatorioHB _relatorio)
    {
        _texto.Append("<tr>");
        _texto.Append("     <td style='line-height: 16px;'>");
        _texto.Append("         <table border='0' cellpadding='0' cellspacing='0' width='689' style='border-collapse: collapse;");
        _texto.Append("         width: 517pt'>");
        _texto.Append("             <tr style='border-top: 2px solid #000;'>");
        _texto.Append("                 <td style='width: 100%; border-left: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.Append("                     COMPOSIÇÃO");
        _texto.Append("                 </td>");
        _texto.Append("             </tr>");
        foreach (PROD_HB_COMPOSICAO comp in _relatorio.COMPOSICAO)
        {
            _texto.Append("             <tr style='border: 1px solid #000'>");
            _texto.Append("                 <td style='text-align: center;'>");
            _texto.Append("                     " + comp.QTDE.ToString() + "% " + comp.DESCRICAO);
            _texto.Append("                 </td>");
            _texto.Append("             </tr>");
        }
        _texto.Append("         </table>");
        _texto.Append("     </td>");
        _texto.Append("</tr>");

        return _texto;
    }
    public StringBuilder MontarGrade(StringBuilder _texto, RelatorioHB _relatorio)
    {
        var _gradeNome = new ProducaoController().ObterGradeNome(Convert.ToInt32(_relatorio.PROD_GRADE));

        _texto.Append("<tr>");
        _texto.Append("    <td style='line-height: 23px;'>");
        _texto.Append("        <table border='0' cellpadding='0' cellspacing='0' width='689' style='border-collapse: collapse;");
        _texto.Append("            width: 517pt'>");
        _texto.Append("            <tr style='text-align: center; border-top: 1px solid #000;'>");
        _texto.Append("                <td style='width: 130px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.Append("                    &nbsp;");
        _texto.Append("                </td>");
        _texto.Append("                <td style='width: 70px; border-right: 1px solid #000;'>");
        _texto.Append("                " + ((_gradeNome != null) ? _gradeNome.GRADE_EXP : "EXP"));
        _texto.Append("                </td>");
        _texto.Append("                <td style='width: 70px; border-right: 1px solid #000;'>");
        _texto.Append("                " + ((_gradeNome != null) ? _gradeNome.GRADE_XP : "XP"));
        _texto.Append("                </td>");
        _texto.Append("                <td style='width: 70px; border-right: 1px solid #000;'>");
        _texto.Append("                " + ((_gradeNome != null) ? _gradeNome.GRADE_PP : "PP"));
        _texto.Append("                </td>");
        _texto.Append("                <td style='width: 70px; border-right: 1px solid #000;'>");
        _texto.Append("                " + ((_gradeNome != null) ? _gradeNome.GRADE_P : "P"));
        _texto.Append("                </td>");
        _texto.Append("                <td style='width: 70px; border-right: 1px solid #000;'>");
        _texto.Append("                " + ((_gradeNome != null) ? _gradeNome.GRADE_M : "M"));
        _texto.Append("                </td>");
        _texto.Append("                <td style='width: 70px; border-right: 1px solid #000;'>");
        _texto.Append("                " + ((_gradeNome != null) ? _gradeNome.GRADE_G : "G"));
        _texto.Append("                </td>");
        _texto.Append("                <td style='width: 70px; border-right: 1px solid #000;'>");
        _texto.Append("                " + ((_gradeNome != null) ? _gradeNome.GRADE_GG : "GG"));
        _texto.Append("                </td>");
        _texto.Append("                <td style='border-right: 1px solid #000;'>");
        _texto.Append("                    TOTAL");
        _texto.Append("                </td>");
        _texto.Append("            </tr>");
        _texto.Append("            <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
        _texto.Append("                <td style='text-align: left; border-right: 1px solid #000;'>");
        _texto.Append("                    GRADE PREVISTA");
        _texto.Append("                </td>");
        _texto.Append("                <td style='border-right: 1px solid #000;'>");
        _texto.Append("                    " + _relatorio.GRADE.GRADE_EXP);
        _texto.Append("                </td>");
        _texto.Append("                <td style='border-right: 1px solid #000;'>");
        _texto.Append("                    " + _relatorio.GRADE.GRADE_XP);
        _texto.Append("                </td>");
        _texto.Append("                <td style='border-right: 1px solid #000;'>");
        _texto.Append("                    " + _relatorio.GRADE.GRADE_PP);
        _texto.Append("                </td>");
        _texto.Append("                <td style='border-right: 1px solid #000;'>");
        _texto.Append("                    " + _relatorio.GRADE.GRADE_P);
        _texto.Append("                </td>");
        _texto.Append("                <td style='border-right: 1px solid #000;'>");
        _texto.Append("                    " + _relatorio.GRADE.GRADE_M);
        _texto.Append("                </td>");
        _texto.Append("                <td style='border-right: 1px solid #000;'>");
        _texto.Append("                    " + _relatorio.GRADE.GRADE_G);
        _texto.Append("                </td>");
        _texto.Append("                <td style='border-right: 1px solid #000;'>");
        _texto.Append("                    " + _relatorio.GRADE.GRADE_GG);
        _texto.Append("                </td>");
        _texto.Append("                <td>");
        _texto.Append("                    " + (Convert.ToInt32(_relatorio.GRADE.GRADE_EXP) + Convert.ToInt32(_relatorio.GRADE.GRADE_XP) + Convert.ToInt32(_relatorio.GRADE.GRADE_PP) + Convert.ToInt32(_relatorio.GRADE.GRADE_P) + Convert.ToInt32(_relatorio.GRADE.GRADE_M) + Convert.ToInt32(_relatorio.GRADE.GRADE_G) + Convert.ToInt32(_relatorio.GRADE.GRADE_GG)));
        _texto.Append("                </td>");
        _texto.Append("            </tr>");
        _texto.Append("            <tr style='border-bottom: 1px solid #000; text-align: center;'>");
        _texto.Append("                <td style='text-align: left; border-left: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.Append("                    GRADE REAL");
        _texto.Append("                </td>");
        _texto.Append("                <td style='border-right: 1px solid #000;'>");
        _texto.Append("                    " + ((_relatorio.GRADEREAL == null) ? "&nbsp;" : _relatorio.GRADEREAL.GRADE_EXP.ToString()));
        _texto.Append("                </td>");
        _texto.Append("                <td style='border-right: 1px solid #000;'>");
        _texto.Append("                    " + ((_relatorio.GRADEREAL == null) ? "&nbsp;" : _relatorio.GRADEREAL.GRADE_XP.ToString()));
        _texto.Append("                </td>");
        _texto.Append("                <td style='border-right: 1px solid #000;'>");
        _texto.Append("                    " + ((_relatorio.GRADEREAL == null) ? "&nbsp;" : _relatorio.GRADEREAL.GRADE_PP.ToString()));
        _texto.Append("                </td>");
        _texto.Append("                <td style='border-right: 1px solid #000;'>");
        _texto.Append("                    " + ((_relatorio.GRADEREAL == null) ? "&nbsp;" : _relatorio.GRADEREAL.GRADE_P.ToString()));
        _texto.Append("                </td>");
        _texto.Append("                <td style='border-right: 1px solid #000;'>");
        _texto.Append("                    " + ((_relatorio.GRADEREAL == null) ? "&nbsp;" : _relatorio.GRADEREAL.GRADE_M.ToString()));
        _texto.Append("                </td>");
        _texto.Append("                <td style='border-right: 1px solid #000;'>");
        _texto.Append("                    " + ((_relatorio.GRADEREAL == null) ? "&nbsp;" : _relatorio.GRADEREAL.GRADE_G.ToString()));
        _texto.Append("                </td>");
        _texto.Append("                <td style='border-right: 1px solid #000;'>");
        _texto.Append("                    " + ((_relatorio.GRADEREAL == null) ? "&nbsp;" : _relatorio.GRADEREAL.GRADE_GG.ToString()));
        _texto.Append("                </td>");
        _texto.Append("                <td style='border-right: 1px solid #000;'>");
        _texto.Append("                    " + ((_relatorio.GRADEREAL == null) ? "&nbsp;" : (Convert.ToInt32(_relatorio.GRADEREAL.GRADE_EXP) + Convert.ToInt32(_relatorio.GRADEREAL.GRADE_XP) + Convert.ToInt32(_relatorio.GRADEREAL.GRADE_PP) + Convert.ToInt32(_relatorio.GRADEREAL.GRADE_P) + Convert.ToInt32(_relatorio.GRADEREAL.GRADE_M) + Convert.ToInt32(_relatorio.GRADEREAL.GRADE_G) + Convert.ToInt32(_relatorio.GRADEREAL.GRADE_GG)).ToString()));
        _texto.Append("                </td>");
        _texto.Append("            </tr>");
        _texto.Append("        </table>");
        _texto.Append("    </td>");
        _texto.Append("</tr>");

        return _texto;
    }
    public StringBuilder MontarGradeCorte(StringBuilder _texto, RelatorioHB _relatorio)
    {
        _texto.Append("<tr>");
        _texto.Append("    <td style='line-height: 23px;'>");
        _texto.Append("        <table border='1' cellpadding='0' cellspacing='0' width='689' style='border-collapse: collapse;");
        _texto.Append("            width: 517pt'>");
        _texto.Append("            <tr style='line-height: 12px;'>");
        _texto.Append("                <td style='border-left: 1px solid #000; border: 1px solid #000;'>");
        _texto.Append("                    &nbsp;");
        _texto.Append("                </td>");
        _texto.Append("            </tr>");
        _texto.Append("            <tr>");
        _texto.Append("                <td>");
        _texto.Append("                    <table border='0' cellpadding='0' cellspacing='0' width='100%' style='border-collapse: collapse;'>");

        int i = 0;
        int cont = 0;
        if (_relatorio.GRADE_CORTE == null || _relatorio.GRADE_CORTE.Count <= 0 || _relatorio.GRADE_CORTE.Count <= 6)
            cont = 6;
        else if (_relatorio.GRADE_CORTE.Count <= 9)
            cont = 9;
        else if (_relatorio.GRADE_CORTE.Count <= 12)
            cont = 12;
        else if (_relatorio.GRADE_CORTE.Count <= 15)
            cont = 15;
        else if (_relatorio.GRADE_CORTE.Count <= 18)
            cont = 18;

        for (i = 0; i < cont; )
        {
            _texto.Append("                        <tr style='border-top: 1px solid #000;'>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                GASTO/ " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 0) ? _relatorio.GRADE_CORTE[i + 0].RISCO_NOME.ToString() : "&nbsp;"));
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.Append("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 0) ? Convert.ToDecimal(_relatorio.GRADE_CORTE[i + 0].GASTO).ToString() : "&nbsp;"));
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                GASTO/ " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > (i + 1)) ? _relatorio.GRADE_CORTE[(i + 1)].RISCO_NOME.ToString() : "&nbsp;"));
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.Append("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > (i + 1)) ? Convert.ToDecimal(_relatorio.GRADE_CORTE[(i + 1)].GASTO).ToString() : "&nbsp;"));
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                GASTO/ " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 2) ? _relatorio.GRADE_CORTE[i + 2].RISCO_NOME.ToString() : "&nbsp;"));
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.Append("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 2) ? Convert.ToDecimal(_relatorio.GRADE_CORTE[i + 2].GASTO).ToString() : "&nbsp;"));
            _texto.Append("                            </td>");
            if (i == 0)
            {
                _texto.Append("                            <td rowspan='###ROWCOLIMG###' style='text-align: center; width: 82px;'>");
                _texto.Append("                                 <img alt='Foto Tecido' Width='130px' Height='180px' src='..\\.." + _relatorio.FOTO_TECIDO.Replace("~", "").Replace("/", "\\") + "' />");
                _texto.Append("                            </td>");
            }
            _texto.Append("                        </tr>");


            _texto.Append("                        <tr style='border-top: 1px solid #000;'>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                GASTO/KG");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.Append("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 0) ? Convert.ToDecimal(_relatorio.GRADE_CORTE[i + 0].GASTO_KGS).ToString() : "&nbsp;"));
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                GASTO/KG");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.Append("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > (i + 1)) ? Convert.ToDecimal(_relatorio.GRADE_CORTE[(i + 1)].GASTO_KGS).ToString() : "&nbsp;"));
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                GASTO/KG");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.Append("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 2) ? Convert.ToDecimal(_relatorio.GRADE_CORTE[i + 2].GASTO_KGS).ToString() : "&nbsp;"));
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");


            _texto.Append("                        <tr style='border: 1px solid #000; border-left: none;'>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                " + (i + 1).ToString() + "ª GRADE");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.Append("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 0) ? (_relatorio.GRADE_CORTE[i + 0].GRADE).ToString() : "&nbsp;"));
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                " + (i + 2).ToString() + "ª GRADE");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.Append("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 1) ? (_relatorio.GRADE_CORTE[i + 1].GRADE).ToString() : "&nbsp;"));
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                " + (i + 3).ToString() + "ª GRADE");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.Append("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 2) ? (_relatorio.GRADE_CORTE[i + 2].GRADE).ToString() : "&nbsp;"));
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                        <tr style='border: 1px solid #000; border-left: none;'>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                FOLHAS");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.Append("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 0) ? Convert.ToInt32(_relatorio.GRADE_CORTE[i + 0].FOLHA).ToString() : "&nbsp;"));
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                FOLHAS");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.Append("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 1) ? Convert.ToInt32(_relatorio.GRADE_CORTE[i + 1].FOLHA).ToString() : "&nbsp;"));
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                FOLHAS");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.Append("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 2) ? Convert.ToInt32(_relatorio.GRADE_CORTE[i + 2].FOLHA).ToString() : "&nbsp;"));
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                        <tr style='border: 1px solid #000; border-left: none;'>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                ENFESTO");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.Append("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 0) ? ((_relatorio.GRADE_CORTE[i + 0].ENFESTO == null) ? "&nbsp;" : _relatorio.GRADE_CORTE[i + 0].ENFESTO.Replace("/", "<br>").ToString()) : "&nbsp;"));
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                ENFESTO");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.Append("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 1) ? ((_relatorio.GRADE_CORTE[i + 1].ENFESTO == null) ? "&nbsp;" : _relatorio.GRADE_CORTE[i + 1].ENFESTO.Replace("/", "<br>").ToString()) : "&nbsp;"));
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                ENFESTO");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.Append("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 2) ? ((_relatorio.GRADE_CORTE[i + 2].ENFESTO == null) ? "&nbsp;" : _relatorio.GRADE_CORTE[i + 2].ENFESTO.Replace("/", "<br>").ToString()) : "&nbsp;"));
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                        <tr style='border: 1px solid #000; border-left: none;'>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                CORTE");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.Append("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 0) ? ((_relatorio.GRADE_CORTE[i + 0].CORTE == null) ? "&nbsp;" : _relatorio.GRADE_CORTE[i + 0].CORTE.Replace("/", "<br>").ToString()) : "&nbsp;"));
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                CORTE");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.Append("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 1) ? ((_relatorio.GRADE_CORTE[i + 1].CORTE == null) ? "&nbsp;" : _relatorio.GRADE_CORTE[i + 1].CORTE.Replace("/", "<br>").ToString()) : "&nbsp;"));
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000;'>");
            _texto.Append("                                CORTE");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.Append("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 2) ? ((_relatorio.GRADE_CORTE[i + 2].CORTE == null) ? "&nbsp;" : _relatorio.GRADE_CORTE[i + 2].CORTE.Replace("/", "<br>").ToString()) : "&nbsp;"));
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                        <tr style='border: 1px solid #000; border-left: none; border-bottom: 2px solid #000;'>");
            _texto.Append("                            <td style='border-right: 1px solid #000; width: 90px;'>");
            _texto.Append("                                AMARRADOR");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.Append("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 0) ? ((_relatorio.GRADE_CORTE[i + 0].AMARRADOR == null) ? "&nbsp;" : _relatorio.GRADE_CORTE[i + 0].AMARRADOR.Replace("/", "<br>").ToString()) : "&nbsp;"));
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; width: 90px;'>");
            _texto.Append("                                AMARRADOR");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.Append("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 1) ? ((_relatorio.GRADE_CORTE[i + 1].AMARRADOR == null) ? "&nbsp;" : _relatorio.GRADE_CORTE[i + 1].AMARRADOR.Replace("/", "<br>").ToString()) : "&nbsp;"));
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; width: 90px;'>");
            _texto.Append("                                AMARRADOR");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.Append("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 2) ? ((_relatorio.GRADE_CORTE[i + 2].AMARRADOR == null) ? "&nbsp;" : _relatorio.GRADE_CORTE[i + 2].AMARRADOR.Replace("/", "<br>").ToString()) : "&nbsp;"));
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");

            i = i + 3;
        }
        _texto.Append("                        <tr style='line-height: 12px;'>");
        _texto.Append("                            <td colspan='6' style='border-bottom: 1px solid #000;'>");
        _texto.Append("                                &nbsp;");
        _texto.Append("                            </td>");
        _texto.Append("                        </tr>");

        _texto.Append("                    </table>");
        _texto.Append("                </td>");
        _texto.Append("            </tr>");
        _texto.Append("        </table>");
        _texto.Append("    </td>");
        _texto.Append("</tr>");

        _texto.Replace("###ROWCOLIMG###", (i * 2).ToString());

        return _texto;
    }
    public StringBuilder MontarLinhasEmBranco(StringBuilder _texto)
    {
        //_texto.Append("<tr>");
        //_texto.Append("    <td style='border: 1px solid #000; border-top: none;'>");
        //_texto.Append("        &nbsp;");
        //_texto.Append("    </td>");
        //_texto.Append("</tr>");
        //_texto.Append("<tr>");
        //_texto.Append("    <td style='border: 1px solid #000; border-top: none;'>");
        //_texto.Append("        &nbsp;");
        //_texto.Append("    </td>");
        //_texto.Append("</tr>");
        //_texto.Append("<tr>");
        //_texto.Append("    <td style='border: 1px solid #000; border-top: none;'>");
        //_texto.Append("        &nbsp;");
        //_texto.Append("    </td>");
        //_texto.Append("</tr>");
        //_texto.Append("<tr>");
        //_texto.Append("    <td style='border: 1px solid #000; border-top: none;'>");
        //_texto.Append("        &nbsp;");
        //_texto.Append("    </td>");
        //_texto.Append("</tr>");
        //_texto.Append("<tr>");
        //_texto.Append("    <td style='border: 1px solid #000; border-top: none;'>");
        //_texto.Append("        &nbsp;");
        //_texto.Append("    </td>");
        //_texto.Append("</tr>");

        return _texto;
    }
    public StringBuilder MontarDetalhes(StringBuilder _texto, RelatorioHB _relatorio)
    {
        _texto.Append("<tr>");
        _texto.Append("     <td style='line-height: 18px;'>");
        _texto.Append("         <table border='0' cellpadding='0' cellspacing='0' width='689' style='border-collapse: collapse;");
        _texto.Append("         width: 517pt'>");
        _texto.Append("             <tr style='border-top: 1px solid #000;'>");
        _texto.Append("                 <td style='width: 100%; border-left: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.Append("                     DETALHES");
        _texto.Append("                 </td>");
        _texto.Append("             </tr>");
        foreach (PROD_HB hb in _relatorio.DETALHE)
        {
            _texto.Append("             <tr style='border: 1px solid #000'>");
            _texto.Append("                 <td style='text-align: center;'>");
            _texto.Append("                     " + hb.PROD_DETALHE1.DESCRICAO);
            _texto.Append("                 </td>");
            _texto.Append("             </tr>");
        }
        _texto.Append("             <tr style='border: 1px solid #000'>");
        _texto.Append("                 <td style='text-align: center;'>");
        _texto.Append("                     &nbsp;");
        _texto.Append("                 </td>");
        _texto.Append("             </tr>");
        _texto.Append("         </table>");
        _texto.Append("     </td>");
        _texto.Append("</tr>");

        return _texto;
    }
    public StringBuilder MontarModelagem(StringBuilder _texto, RelatorioHB _relatorio)
    {
        _texto.Append("<tr>");
        _texto.Append("    <td style='line-height: 17px;'>");
        _texto.Append("        <table border='0' cellpadding='0' cellspacing='0' width='689' style='border-collapse: collapse;");
        _texto.Append("            width: 517pt'>");
        _texto.Append("            <tr style='border-right: 1px solid #000; border-top: 1px solid #000;'>");
        _texto.Append("                <td style='width: 170px; border-right: 1px solid #000; border-left: 1px solid #000;'>");
        _texto.Append("                    MODELAGEM");
        _texto.Append("                </td>");
        _texto.Append("                <td style='width: 130px; border-right: 1px solid #000;'>");
        _texto.Append("                    TOTAL DE PARTES");
        _texto.Append("                </td>");
        _texto.Append("                <td style='width: 50px; border-right: 1px solid #000;'>");
        _texto.Append("                    RISCO");
        _texto.Append("                </td>");
        _texto.Append("                <td style='width: 130px; border-right: 1px solid #000;'>");
        _texto.Append("                    CONFERIDO");
        _texto.Append("                </td>");
        _texto.Append("                <td style='width: 130px; border-right: 1px solid #000;'>");
        _texto.Append("                    LIQUIDAR");
        _texto.Append("                </td>");
        _texto.Append("                <td>");
        _texto.Append("                    MOSTRUÁRIO");
        _texto.Append("                </td>");
        _texto.Append("            </tr>");
        _texto.Append("            <tr style='border: 1px solid #000; border-top: none;'>");
        _texto.Append("                <td style='border-right: 1px solid #000; text-align: center'>");
        _texto.Append("                    " + _relatorio.MODELAGEM);
        _texto.Append("                </td>");
        _texto.Append("                <td style='border-right: 1px solid #000; text-align: center'>");
        _texto.Append("                    &nbsp;");
        _texto.Append("                </td>");
        _texto.Append("                <td style='border-right: 1px solid #000; text-align: center'>");
        _texto.Append("                    &nbsp;");
        _texto.Append("                </td>");
        _texto.Append("                <td style='border-right: 1px solid #000; text-align: center'>");
        _texto.Append("                    &nbsp;");
        _texto.Append("                </td>");

        string liq = "";
        if (_relatorio.LIQUIDAR == "Sim")
            liq = _relatorio.LIQUIDAR;
        else if (_relatorio.LIQUIDAR_PQNO == "Sim")
            liq = _relatorio.LIQUIDAR_PQNO;
        else
            liq = "Não";

        _texto.Append("                <td style='border-right: 1px solid #000; text-align: center'>");
        _texto.Append("                    " + liq);
        _texto.Append("                </td>");

        _texto.Append("                <td style='text-align: center'>");
        _texto.Append("                    " + ((_relatorio.MOSTRUARIO == "S") ? "Sim" : "Não"));
        _texto.Append("                </td>");
        _texto.Append("            </tr>");
        _texto.Append("        </table>");
        _texto.Append("    </td>");
        _texto.Append("</tr>");


        return _texto;
    }
    public StringBuilder MontarPeca(StringBuilder _texto, RelatorioHB _relatorio)
    {
        _texto.Append("<tr>");
        _texto.Append("    <td style='line-height: 17px;'>");
        _texto.Append("        <table border='1' cellpadding='0' cellspacing='0' width='689' style='border-collapse: collapse;");
        _texto.Append("            width: 517pt'>");
        _texto.Append("            <tr style='border-right: 1px solid #000; border-top: 1px solid #000;'>");
        _texto.Append("                <td style='width: 252px; border-right: 1px solid #000; border-left: 1px solid #000;'>");
        _texto.Append("                    <table cellpadding='0' cellspacing='0' width='100%'>");
        _texto.Append("                        <tr>");
        _texto.Append("                            <td style='width:220px;'>");
        _texto.Append("                                CONSUMO PARA CUSTO");
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='border-left: 1px solid #000;'>");
        _texto.Append("                                VOLUME");
        _texto.Append("                            </td>");
        _texto.Append("                        </tr>");
        _texto.Append("                        <tr>");
        _texto.Append("                            <td style='border-bottom: 1px solid #000;'>");
        _texto.Append("                                &nbsp;");
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='text-align:center; border-bottom: 1px solid #000;border-left: 1px solid #000;'>");
        _texto.Append("                                " + _relatorio.VOLUME);
        _texto.Append("                            </td>");
        _texto.Append("                        </tr>");
        _texto.Append("                        <tr>");
        _texto.Append("                            <td colspan='2'>");
        _texto.Append("                                TECIDO PRINCIPAL $");
        _texto.Append("                            </td>");
        _texto.Append("                        </tr>");
        _texto.Append("                        <tr>");
        _texto.Append("                            <td colspan='2' style='border-bottom: 1px solid #000;'>");
        _texto.Append("                                &nbsp;" + _relatorio.CUSTO_TECIDO);
        _texto.Append("                            </td>");
        _texto.Append("                        </tr>");
        _texto.Append("                        <tr>");
        _texto.Append("                            <td colspan='2'>");
        _texto.Append("                                <table cellpadding='0' cellspacing='0' width='100%'>");
        _texto.Append("                                    <tr>");
        _texto.Append("                                        <td style='border-bottom: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.Append("                                            &nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='border-bottom: 1px solid #000; border-right: 1px solid #000; text-align: center;'>");
        _texto.Append("                                            POR PEÇA");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='border-bottom: 1px solid #000; text-align: center;'>");
        _texto.Append("                                            POR CORTE");
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");
        _texto.Append("                                    <tr>");
        _texto.Append("                                        <td style='border-bottom: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.Append("                                            KILOS");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='border-bottom: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.Append("                                            " + ((_relatorio.UNIDADE_MEDIDA == "3") ? _relatorio.PORPECA : "&nbsp;"));
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='border-bottom: 1px solid #000;'>");
        _texto.Append("                                            " + ((_relatorio.UNIDADE_MEDIDA == "3") ? _relatorio.PORCORTE : "&nbsp;"));
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");
        _texto.Append("                                    <tr>");
        _texto.Append("                                        <td style='border-bottom: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.Append("                                            METROS");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='border-bottom: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.Append("                                            " + ((_relatorio.UNIDADE_MEDIDA == "1") ? _relatorio.PORPECA : "&nbsp;"));
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='border-bottom: 1px solid #000;'>");
        _texto.Append("                                            " + ((_relatorio.UNIDADE_MEDIDA == "1") ? _relatorio.PORCORTE : "&nbsp;"));
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");
        _texto.Append("                                    <tr>");
        _texto.Append("                                        <td style='border-bottom: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.Append("                                            RETALHOS");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='border-bottom: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.Append("                                            &nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='border-bottom: 1px solid #000;'>");
        _texto.Append("                                            " + _relatorio.RETALHO);
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");
        _texto.Append("                                    <tr>");
        _texto.Append("                                        <td style='border-bottom: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.Append("                                            &nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='border-bottom: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.Append("                                            &nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='border-bottom: 1px solid #000;'>");
        _texto.Append("                                            &nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");
        _texto.Append("                                    <tr>");
        _texto.Append("                                        <td>");
        _texto.Append("                                            &nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td>");
        _texto.Append("                                            &nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td>");
        _texto.Append("                                            &nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");
        _texto.Append("                                </table>");
        _texto.Append("                            </td>");
        _texto.Append("                        </tr>");
        _texto.Append("                    </table>");
        _texto.Append("                </td>");
        _texto.Append("                <td style='text-align: center; width: 200px; border-right: 1px solid #000; border-left: 1px solid #000;'>");
        _texto.Append("                     <img alt='Foto Peça' Width='130px' Height='180px' src='..\\.." + _relatorio.FOTO_PECA.Replace("~", "").Replace("/", "\\") + "' />");
        _texto.Append("                </td>");
        _texto.Append("            </tr>");
        _texto.Append("        </table>");
        _texto.Append("    </td>");
        _texto.Append("</tr>");

        return _texto;
    }
    public StringBuilder MontarOBS(StringBuilder _texto, RelatorioHB _relatorio)
    {
        _texto.Append("<tr>");
        _texto.Append("    <td style='line-height: 17px;'>");
        _texto.Append("         <table border='0' cellpadding='0' cellspacing='0' width='689' style='border-collapse: collapse;");
        _texto.Append("         width: 517pt'>");
        _texto.Append("             <tr>");
        _texto.Append("                  <td style='border: 1px solid #000; border-top: 1px solid #000; width:33%'>");
        _texto.Append("                      Nome Ampliação");
        _texto.Append("                  </td>");
        _texto.Append("                  <td style='border: 1px solid #000; border-top: 1px solid #000; width:33%'>");
        _texto.Append("                      Nome Risco");
        _texto.Append("                  </td>");
        _texto.Append("                  <td style='border: 1px solid #000; border-top: 1px solid #000; width:33%'>");
        _texto.Append("                      Nome Modelagem");
        _texto.Append("                  </td>");
        _texto.Append("             </tr>");
        _texto.Append("             <tr style='line-height: 25px; vertical-align: top;' valign='top'>");
        _texto.Append("                  <td style='border: 1px solid #000; border-top: 1px solid #000; width:33%'>");
        _texto.Append("                                 " + _relatorio.NOME_AMPLIACAO);
        _texto.Append("                  </td>");
        _texto.Append("                  <td style='border: 1px solid #000; border-top: 1px solid #000; width:33%'>");
        _texto.Append("                                 " + _relatorio.NOME_RISCO);
        _texto.Append("                  </td>");
        _texto.Append("                  <td style='border: 1px solid #000; border-top: 1px solid #000; width:33%'>");
        _texto.Append("                                 " + _relatorio.NOME_MODELAGEM);
        _texto.Append("                  </td>");
        _texto.Append("             </tr>");
        _texto.Append("         </table>");
        _texto.Append("     </td>");
        _texto.Append("</tr>");

        _texto.Append("<tr>");
        _texto.Append("     <td style='border: 1px solid #000; border-top: 1px solid #000;'>");
        _texto.Append("         OBSERVAÇÃO");
        _texto.Append("     </td>");
        _texto.Append("</tr>");
        _texto.Append("<tr style='line-height: 25px; vertical-align: top;' valign='top'>");
        _texto.Append("     <td style='border: 1px solid #000; border-top: 1px solid #000;'>");
        _texto.Append("                    " + _relatorio.OBS.Replace("\r", "<br/>").Replace("\n", "<br/>"));
        _texto.Append("     </td>");
        _texto.Append("</tr>");
        _texto.Append("</table></div>");
        return _texto;
    }


    #endregion

    #region "DETALHES"
    public StringBuilder MontarDetalhe(StringBuilder _texto, RelatorioHB _relatorio, string breakClass)
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();
        int count = 0;
        var _gradeNome = prodController.ObterGradeNome(Convert.ToInt32(_relatorio.PROD_GRADE));

        string griffe = "";
        string signed = "N";
        string signedNome = "";

        foreach (PROD_HB d in _relatorio.DETALHE)
        {
            count += 1;

            if (breakClass == "breakafter" && count == _relatorio.DETALHE.Count())
                breakClass = "";

            _relatorio.GRADE_CORTE = prodController.ObterGradeCorteHB(d.CODIGO);

            PROD_HB _prod_HB = prodController.ObterHB(Convert.ToInt32(d.CODIGO_PAI));
            var desenvProduto = desenvController.ObterProduto(_prod_HB.COLECAO, _prod_HB.CODIGO_PRODUTO_LINX, _prod_HB.COR);
            if (desenvProduto != null)
            {
                griffe = "";
                signed = "N";
                signedNome = "";

                if (desenvProduto.GRIFFE != null)
                    griffe = desenvProduto.GRIFFE;

                if (desenvProduto.SIGNED != null)
                    signed = desenvProduto.SIGNED.ToString();

                if (desenvProduto.SIGNED_NOME != null)
                    signedNome = desenvProduto.SIGNED_NOME;
            }


            _texto.AppendLine("    <br />");
            _texto.AppendLine("    <div id='divDetalhe" + d.PROD_DETALHE.ToString() + "' align='center' class='" + breakClass + "'>");
            _texto.AppendLine("        <table border='0' cellpadding='0' cellspacing='0' width='689' style='width: 517pt;");
            _texto.AppendLine("            padding: 0px; color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
            _texto.AppendLine("            background: white; white-space: nowrap;'>");
            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td style='line-height: 16px;'>");
            _texto.AppendLine("                    <table border='0' cellpadding='0' cellspacing='0' width='689' style='border-collapse: collapse;");
            _texto.AppendLine("                        width: 517pt'>");
            _texto.AppendLine("                        <tr style='border-right: 1px solid #000; border-top: 2px solid #000;'>");
            _texto.AppendLine("                            <td style='width: 70px; border-right: 1px solid #000; border-left: 1px solid #000;'>");
            _texto.AppendLine("                                HB");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 150px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                GRUPO");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 202px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                NOME");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                DATA");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                MOLDE");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                GRIFFE");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                        <tr style='border: 1px solid #000; border-top: none;'>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; text-align: center'>");
            _texto.AppendLine("                                " + d.HB);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; text-align: center'>");
            _texto.AppendLine("                                " + d.GRUPO);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; text-align: center'>");
            _texto.AppendLine("                                " + d.NOME);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; text-align: center'>");
            _texto.AppendLine("                                " + d.DATA_INCLUSAO);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; text-align: center'>");
            _texto.AppendLine("                                " + d.MOLDE);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align: center'>");
            _texto.AppendLine("                                " + griffe.Trim());
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                    </table>");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");
            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td style='line-height: 18px;'>");
            _texto.AppendLine("                    <table border='0' cellpadding='0' cellspacing='0' width='689' style='border-collapse: collapse;");
            _texto.AppendLine("                        width: 517pt'>");
            _texto.AppendLine("                        <tr style='border-right: 1px solid #000; border-top: 0px solid #000;'>");
            _texto.AppendLine("                            <td style='width: 221px; border-right: 1px solid #000; border-left: 1px solid #000;'>");
            _texto.AppendLine("                                DETALHE");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; border-left: 1px solid #000;'>");
            _texto.AppendLine("                                COMPOSIÇÃO");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 100px; border-right: 1px solid #000; border-left: 1px solid #000;'>");
            _texto.AppendLine("                                VOLUME");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 100px;'>");
            _texto.AppendLine("                                MOSTRUÁRIO");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                        <tr style='border: 1px solid #000; border-top: none;'>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; text-align: center'>");
            _texto.AppendLine("                                <i><b>" + d.PROD_DETALHE1.DESCRICAO.ToUpper() + "</b></i>");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; text-align:center;'>");
            _texto.AppendLine("                                <table width='100%'>");
            foreach (PROD_HB_COMPOSICAO comp in prodController.ObterComposicaoHB(d.CODIGO))
            {
                _texto.AppendLine("                                    <tr>");
                _texto.AppendLine("                                        <td>");
                _texto.AppendLine("                                         " + comp.QTDE.ToString() + "% " + comp.DESCRICAO);
                _texto.AppendLine("                                         </td>");
                _texto.AppendLine("                                    </tr>");
            }
            _texto.AppendLine("                                </table>");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; text-align: center'>");
            _texto.AppendLine("                                " + ((d.VOLUME == null) ? "" : d.VOLUME.ToString()) + "");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; text-align: center'>");
            _texto.AppendLine("                                " + ((d.MOSTRUARIO == 'S') ? "SIM" : "NÃO") + "");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                    </table>");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");
            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td style='line-height: 18px;'>");
            _texto.AppendLine("                    <table border='0' cellpadding='0' cellspacing='0' width='689' style='border-collapse: collapse;");
            _texto.AppendLine("                        width: 517pt'>");
            _texto.AppendLine("                        <tr>");
            _texto.AppendLine("                            <td style='width: 221px; border-right: 1px solid #000; border-left: 1px solid #000;'>");
            _texto.AppendLine("                                TECIDO");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 100px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                T. PARTES");
            _texto.AppendLine("                            </td>");
            if (signed == "S")
            {
                _texto.AppendLine("                            <td style='width: 201px; border-right: 1px solid #000;'>");
                _texto.AppendLine("                                FORNECEDOR");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                LARGURA");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                SIGNED");
                _texto.AppendLine("                            </td>");
            }
            else
            {
                _texto.AppendLine("                            <td style='width: 253px; border-right: 1px solid #000;'>");
                _texto.AppendLine("                                FORNECEDOR");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                LARGURA");
                _texto.AppendLine("                            </td>");
            }
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                        <tr style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; border-left: 1px solid #000; text-align: center;'>");
            _texto.AppendLine("                                " + (d.TECIDO + ((d.COR_FORNECEDOR.Trim() == "") ? "" : " - " + d.COR_FORNECEDOR.Trim())));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.AppendLine("                                " + d.FORNECEDOR.Trim());
            _texto.AppendLine("                            </td>");
            if (signed == "S")
            {
                _texto.AppendLine("                            <td style='border-right: 1px solid #000; text-align: center;'>");
                _texto.AppendLine("                                " + ((d.LARGURA == null) ? "" : d.LARGURA.ToString()));
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='text-align: center'>");
                _texto.AppendLine("                                " + signedNome);
                _texto.AppendLine("                            </td>");
            }
            else
            {
                _texto.AppendLine("                            <td style='text-align: center'>");
                _texto.AppendLine("                                " + ((d.LARGURA == null) ? "" : d.LARGURA.ToString()));
                _texto.AppendLine("                            </td>");
            }
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                    </table>");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");
            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td style='line-height: 23px;'>");
            _texto.AppendLine("                    <table border='0' cellpadding='0' cellspacing='0' width='689' style='border-collapse: collapse;");
            _texto.AppendLine("                        width: 517pt'>");
            _texto.AppendLine("                        <tr style='text-align: center; border-top: 2px solid #000;'>");
            _texto.AppendLine("                            <td style='width: 130px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 70px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                             " + ((_gradeNome != null) ? _gradeNome.GRADE_EXP : "EXP"));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 70px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                             " + ((_gradeNome != null) ? _gradeNome.GRADE_XP : "XP"));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 70px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                             " + ((_gradeNome != null) ? _gradeNome.GRADE_PP : "PP"));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 70px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                             " + ((_gradeNome != null) ? _gradeNome.GRADE_P : "P"));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 70px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                             " + ((_gradeNome != null) ? _gradeNome.GRADE_M : "M"));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 70px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                             " + ((_gradeNome != null) ? _gradeNome.GRADE_G : "G"));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 70px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                             " + ((_gradeNome != null) ? _gradeNome.GRADE_GG : "GG"));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                TOTAL");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                        <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
            _texto.AppendLine("                            <td style='text-align: left; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                GRADE PREVISTA");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                " + _relatorio.GRADE.GRADE_EXP);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                " + _relatorio.GRADE.GRADE_XP);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                " + _relatorio.GRADE.GRADE_PP);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                " + _relatorio.GRADE.GRADE_P);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                " + _relatorio.GRADE.GRADE_M);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                " + _relatorio.GRADE.GRADE_G);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                " + _relatorio.GRADE.GRADE_GG);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                " + (Convert.ToInt32(_relatorio.GRADE.GRADE_EXP) + Convert.ToInt32(_relatorio.GRADE.GRADE_XP) + Convert.ToInt32(_relatorio.GRADE.GRADE_PP) + Convert.ToInt32(_relatorio.GRADE.GRADE_P) + Convert.ToInt32(_relatorio.GRADE.GRADE_M) + Convert.ToInt32(_relatorio.GRADE.GRADE_G) + Convert.ToInt32(_relatorio.GRADE.GRADE_GG)));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                        <tr style='border-bottom: 1px solid #000; text-align: center;'>");
            _texto.AppendLine("                            <td style='text-align: left; border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                GRADE REAL");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                " + ((_relatorio.GRADEREAL == null) ? "&nbsp;" : _relatorio.GRADEREAL.GRADE_EXP.ToString()));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                " + ((_relatorio.GRADEREAL == null) ? "&nbsp;" : _relatorio.GRADEREAL.GRADE_XP.ToString()));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                " + ((_relatorio.GRADEREAL == null) ? "&nbsp;" : _relatorio.GRADEREAL.GRADE_PP.ToString()));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                " + ((_relatorio.GRADEREAL == null) ? "&nbsp;" : _relatorio.GRADEREAL.GRADE_P.ToString()));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                " + ((_relatorio.GRADEREAL == null) ? "&nbsp;" : _relatorio.GRADEREAL.GRADE_M.ToString()));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                " + ((_relatorio.GRADEREAL == null) ? "&nbsp;" : _relatorio.GRADEREAL.GRADE_G.ToString()));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                " + ((_relatorio.GRADEREAL == null) ? "&nbsp;" : _relatorio.GRADEREAL.GRADE_GG.ToString()));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                " + ((_relatorio.GRADEREAL == null) ? "&nbsp;" : (Convert.ToInt32(_relatorio.GRADEREAL.GRADE_EXP) + Convert.ToInt32(_relatorio.GRADEREAL.GRADE_XP) + Convert.ToInt32(_relatorio.GRADEREAL.GRADE_PP) + Convert.ToInt32(_relatorio.GRADEREAL.GRADE_P) + Convert.ToInt32(_relatorio.GRADEREAL.GRADE_M) + Convert.ToInt32(_relatorio.GRADEREAL.GRADE_G) + Convert.ToInt32(_relatorio.GRADEREAL.GRADE_GG)).ToString()));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                    </table>");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");
            _texto.AppendLine("<tr>");
            _texto.AppendLine("    <td style='line-height: 23px;'>");
            _texto.AppendLine("        <table border='1' cellpadding='0' cellspacing='0' width='689' style='border-collapse: collapse;");
            _texto.AppendLine("            width: 517pt'>");
            _texto.AppendLine("            <tr style='line-height: 12px;'>");
            _texto.AppendLine("                <td style='border-left: 1px solid #000; border: 1px solid #000;'>");
            _texto.AppendLine("                    &nbsp;");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");
            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td>");
            _texto.AppendLine("                    <table border='0' cellpadding='0' cellspacing='0' width='100%' style='border-collapse: collapse;'>");

            int i = 0;
            int cont = 0;
            if (_relatorio.GRADE_CORTE == null || _relatorio.GRADE_CORTE.Count <= 0 || _relatorio.GRADE_CORTE.Count <= 6)
                cont = 6;
            else if (_relatorio.GRADE_CORTE.Count <= 9)
                cont = 9;
            else if (_relatorio.GRADE_CORTE.Count <= 12)
                cont = 12;
            else if (_relatorio.GRADE_CORTE.Count <= 15)
                cont = 15;
            else if (_relatorio.GRADE_CORTE.Count <= 18)
                cont = 18;

            for (i = 0; i < cont; )
            {
                _texto.Append("                        <tr style='border-top: 1px solid #000;'>");
                _texto.Append("                            <td style='border-right: 1px solid #000;'>");
                _texto.Append("                                GASTO/ " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 0) ? _relatorio.GRADE_CORTE[i + 0].RISCO_NOME.ToString() : "&nbsp;"));
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
                _texto.Append("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 0) ? Convert.ToDecimal(_relatorio.GRADE_CORTE[i + 0].GASTO).ToString() : "&nbsp;"));
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right: 1px solid #000;'>");
                _texto.Append("                                GASTO/ " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > (i + 1)) ? _relatorio.GRADE_CORTE[(i + 1)].RISCO_NOME.ToString() : "&nbsp;"));
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
                _texto.Append("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > (i + 1)) ? Convert.ToDecimal(_relatorio.GRADE_CORTE[(i + 1)].GASTO).ToString() : "&nbsp;"));
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right: 1px solid #000;'>");
                _texto.Append("                                GASTO/ " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 2) ? _relatorio.GRADE_CORTE[i + 2].RISCO_NOME.ToString() : "&nbsp;"));
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
                _texto.Append("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 2) ? Convert.ToDecimal(_relatorio.GRADE_CORTE[i + 2].GASTO).ToString() : "&nbsp;"));
                _texto.Append("                            </td>");
                if (i == 0)
                {
                    string foto = "";
                    if (_relatorio.FOTO_TECIDO != null)
                        foto = _relatorio.FOTO_TECIDO.Replace("~", "").Replace("/", "\\");
                    _texto.Append("                            <td rowspan='###ROWCOLIMG###' style='text-align: center; width: 82px;'>");
                    _texto.Append("                                 <img alt='Foto Tecido' Width='130px' Height='180px' src='..\\.." + foto + "' />");
                    _texto.Append("                            </td>");
                }
                _texto.Append("                        </tr>");

                _texto.Append("                        <tr style='border-top: 1px solid #000;'>");
                _texto.Append("                            <td style='border-right: 1px solid #000;'>");
                _texto.Append("                                GASTO/KG ");
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
                _texto.Append("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 0) ? Convert.ToDecimal(_relatorio.GRADE_CORTE[i + 0].GASTO_KGS).ToString() : "&nbsp;"));
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right: 1px solid #000;'>");
                _texto.Append("                                GASTO/KG ");
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
                _texto.Append("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > (i + 1)) ? Convert.ToDecimal(_relatorio.GRADE_CORTE[(i + 1)].GASTO_KGS).ToString() : "&nbsp;"));
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right: 1px solid #000;'>");
                _texto.Append("                                GASTO/KG ");
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
                _texto.Append("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 2) ? Convert.ToDecimal(_relatorio.GRADE_CORTE[i + 2].GASTO_KGS).ToString() : "&nbsp;"));
                _texto.Append("                            </td>");
                _texto.Append("                        </tr>");

                _texto.Append("                        <tr style='border: 1px solid #000; border-left: none;'>");
                _texto.Append("                            <td style='border-right: 1px solid #000;'>");
                _texto.Append("                                " + (i + 1).ToString() + "ª GRADE");
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
                _texto.Append("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 0) ? (_relatorio.GRADE_CORTE[i + 0].GRADE).ToString() : "&nbsp;"));
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right: 1px solid #000;'>");
                _texto.Append("                                " + (i + 2).ToString() + "ª GRADE");
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
                _texto.Append("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 1) ? (_relatorio.GRADE_CORTE[i + 1].GRADE).ToString() : "&nbsp;"));
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right: 1px solid #000;'>");
                _texto.Append("                                " + (i + 3).ToString() + "ª GRADE");
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
                _texto.Append("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 2) ? (_relatorio.GRADE_CORTE[i + 2].GRADE).ToString() : "&nbsp;"));
                _texto.Append("                            </td>");
                _texto.Append("                        </tr>");
                _texto.Append("                        <tr style='border: 1px solid #000; border-left: none;'>");
                _texto.Append("                            <td style='border-right: 1px solid #000;'>");
                _texto.Append("                                FOLHAS");
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
                _texto.Append("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 0) ? Convert.ToInt32(_relatorio.GRADE_CORTE[i + 0].FOLHA).ToString() : "&nbsp;"));
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right: 1px solid #000;'>");
                _texto.Append("                                FOLHAS");
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
                _texto.Append("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 1) ? Convert.ToInt32(_relatorio.GRADE_CORTE[i + 1].FOLHA).ToString() : "&nbsp;"));
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right: 1px solid #000;'>");
                _texto.Append("                                FOLHAS");
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
                _texto.Append("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 2) ? Convert.ToInt32(_relatorio.GRADE_CORTE[i + 2].FOLHA).ToString() : "&nbsp;"));
                _texto.Append("                            </td>");
                _texto.Append("                        </tr>");
                _texto.Append("                        <tr style='border: 1px solid #000; border-left: none;'>");
                _texto.Append("                            <td style='border-right: 1px solid #000;'>");
                _texto.Append("                                ENFESTO");
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
                _texto.Append("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 0) ? ((_relatorio.GRADE_CORTE[i + 0].ENFESTO == null) ? "&nbsp;" : _relatorio.GRADE_CORTE[i + 0].ENFESTO.Replace("/", "<br>").ToString()) : "&nbsp;"));
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right: 1px solid #000;'>");
                _texto.Append("                                ENFESTO");
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
                _texto.Append("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 1) ? ((_relatorio.GRADE_CORTE[i + 1].ENFESTO == null) ? "&nbsp;" : _relatorio.GRADE_CORTE[i + 1].ENFESTO.Replace("/", "<br>").ToString()) : "&nbsp;"));
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right: 1px solid #000;'>");
                _texto.Append("                                ENFESTO");
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
                _texto.Append("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 2) ? ((_relatorio.GRADE_CORTE[i + 2].ENFESTO == null) ? "&nbsp;" : _relatorio.GRADE_CORTE[i + 2].ENFESTO.Replace("/", "<br>").ToString()) : "&nbsp;"));
                _texto.Append("                            </td>");
                _texto.Append("                        </tr>");
                _texto.Append("                        <tr style='border: 1px solid #000; border-left: none;'>");
                _texto.Append("                            <td style='border-right: 1px solid #000;'>");
                _texto.Append("                                CORTE");
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
                _texto.Append("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 0) ? ((_relatorio.GRADE_CORTE[i + 0].CORTE == null) ? "&nbsp;" : _relatorio.GRADE_CORTE[i + 0].CORTE.Replace("/", "<br>").ToString()) : "&nbsp;"));
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right: 1px solid #000;'>");
                _texto.Append("                                CORTE");
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
                _texto.Append("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 1) ? ((_relatorio.GRADE_CORTE[i + 1].CORTE == null) ? "&nbsp;" : _relatorio.GRADE_CORTE[i + 1].CORTE.Replace("/", "<br>").ToString()) : "&nbsp;"));
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right: 1px solid #000;'>");
                _texto.Append("                                CORTE");
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
                _texto.Append("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 2) ? ((_relatorio.GRADE_CORTE[i + 2].CORTE == null) ? "&nbsp;" : _relatorio.GRADE_CORTE[i + 2].CORTE.Replace("/", "<br>").ToString()) : "&nbsp;"));
                _texto.Append("                            </td>");
                _texto.Append("                        </tr>");
                _texto.Append("                        <tr style='border: 1px solid #000; border-left: none; border-bottom: 2px solid #000;'>");
                _texto.Append("                            <td style='border-right: 1px solid #000; width: 90px;'>");
                _texto.Append("                                AMARRADOR");
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
                _texto.Append("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 0) ? ((_relatorio.GRADE_CORTE[i + 0].AMARRADOR == null) ? "&nbsp;" : _relatorio.GRADE_CORTE[i + 0].AMARRADOR.Replace("/", "<br>").ToString()) : "&nbsp;"));
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right: 1px solid #000; width: 90px;'>");
                _texto.Append("                                AMARRADOR");
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
                _texto.Append("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 1) ? ((_relatorio.GRADE_CORTE[i + 1].AMARRADOR == null) ? "&nbsp;" : _relatorio.GRADE_CORTE[i + 1].AMARRADOR.Replace("/", "<br>").ToString()) : "&nbsp;"));
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right: 1px solid #000; width: 90px;'>");
                _texto.Append("                                AMARRADOR");
                _texto.Append("                            </td>");
                _texto.Append("                            <td style='border-right: 1px solid #000; text-align: center;'>");
                _texto.Append("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 2) ? ((_relatorio.GRADE_CORTE[i + 2].AMARRADOR == null) ? "&nbsp;" : _relatorio.GRADE_CORTE[i + 2].AMARRADOR.Replace("/", "<br>").ToString()) : "&nbsp;"));
                _texto.Append("                            </td>");
                _texto.Append("                        </tr>");

                i = i + 3;
            }

            _texto.Replace("###ROWCOLIMG###", (i * 2).ToString());

            _texto.AppendLine("                    </table>");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");
            _texto.AppendLine("        </table>");
            _texto.AppendLine("    </td>");
            _texto.AppendLine("</tr>");
            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td style='border: 1px solid #000; border-top: none;'>");
            _texto.AppendLine("                    &nbsp;");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");
            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td style='border: 1px solid #000; border-top: none;'>");
            _texto.AppendLine("                    &nbsp;");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");
            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td style='border: 1px solid #000; border-top: none;'>");
            _texto.AppendLine("                    &nbsp;");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");
            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td style='line-height: 17px;'>");
            _texto.AppendLine("                    <table border='1' cellpadding='0' cellspacing='0' width='689' style='border-collapse: collapse;");
            _texto.AppendLine("                        width: 517pt'>");
            _texto.AppendLine("                        <tr style='border-right: 1px solid #000; border-top: 1px solid #000;'>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; border-left: 1px solid #000;'>");
            _texto.AppendLine("                                <table cellpadding='0' cellspacing='0' width='100%'>");
            _texto.AppendLine("                                    <tr style='' valign='top'>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000; border-top: 1px solid #000; width: 200px;'>");
            _texto.AppendLine("                                            CONSUMO PARA CUSTO");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000; border-top: 1px solid #000; width: 202px;'>");
            _texto.AppendLine("                                            POR PEÇA");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000; border-top: 1px solid #000;'>");
            _texto.AppendLine("                                            POR CORTE");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr style='' valign='top'>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000; border-top: 0px solid #000; width: 200px;'>");
            _texto.AppendLine("                                            R$");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000; border-top: 0px solid #000; width: 202px;'>");
            _texto.AppendLine("                                            &nbsp;");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000; border-top: 0px solid #000;'>");
            _texto.AppendLine("                                            &nbsp;");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr style='' valign='top'>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000; border-top: 1px solid #000; width: 200px; text-align:center;'>");
            _texto.AppendLine("                                            " + ((d.CUSTO_TECIDO != null) ? Convert.ToDecimal(d.CUSTO_TECIDO).ToString("###0.00") : "&nbsp;"));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000; border-top: 1px solid #000; width: 202px;'>");
            _texto.AppendLine("                                            KILO");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000; border-top: 1px solid #000;'>");
            _texto.AppendLine("                                            &nbsp;");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr style='' valign='top'>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000; border-top: 0px solid #000; width: 200px;'>");
            _texto.AppendLine("                                            &nbsp;");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000; border-top: 0px solid #000; width: 202px;'>");
            //KG = 3
            _texto.AppendLine("                                            " + ((d.UNIDADE_MEDIDA == 3) ? _relatorio.PORPECA : "&nbsp;"));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000; border-top: 0px solid #000;'>");
            _texto.AppendLine("                                            " + ((d.UNIDADE_MEDIDA == 3) ? _relatorio.PORCORTE : "&nbsp;"));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr style='' valign='top'>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000; border-top: 1px solid #000; width: 200px;'>");
            _texto.AppendLine("                                            &nbsp;");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000; border-top: 1px solid #000; width: 202px;'>");
            _texto.AppendLine("                                            METROS");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000; border-top: 1px solid #000;'>");
            _texto.AppendLine("                                            &nbsp;");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr style='' valign='top'>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000; border-top: 0px solid #000; width: 200px;'>");
            _texto.AppendLine("                                            &nbsp;");
            _texto.AppendLine("                                        </td>");
            //METRO = 1
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000; border-top: 0px solid #000; width: 202px;'>");
            _texto.AppendLine("                                            " + ((d.UNIDADE_MEDIDA == 1) ? _relatorio.PORPECA : "&nbsp;"));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000; border-top: 0px solid #000;'>");
            _texto.AppendLine("                                            " + ((d.UNIDADE_MEDIDA == 1) ? _relatorio.PORCORTE : "&nbsp;"));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr style='' valign='top'>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000; border-top: 1px solid #000; width: 200px;'>");
            _texto.AppendLine("                                            &nbsp;");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000; border-top: 1px solid #000; width: 202px;'>");
            _texto.AppendLine("                                            RETALHOS");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000; border-top: 1px solid #000;'>");
            _texto.AppendLine("                                            &nbsp;");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr style='' valign='top'>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000; border-top: 0px solid #000; width: 200px;'>");
            _texto.AppendLine("                                            &nbsp;");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000; border-top: 0px solid #000; width: 202px;'>");
            _texto.AppendLine("                                            " + _relatorio.RETALHO);
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000; border-top: 0px solid #000;'>");
            _texto.AppendLine("                                            &nbsp;");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                </table>");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                    </table>");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");
            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td style='border: 1px solid #000; border-top: 1px solid #000;'>");
            _texto.AppendLine("                    OBSERVAÇÃO");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");
            _texto.AppendLine("            <tr style='line-height: 25px; vertical-align: top;' valign='top'>");
            _texto.AppendLine("                <td style='border: 1px solid #000; border-top: 1px solid #000;'>");
            _texto.AppendLine("                    " + d.OBSERVACAO.Replace("\r", "<br/>").Replace("\n", "<br/>"));
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");
            _texto.AppendLine("        </table>");
            _texto.AppendLine("    </div>");
        }

        return _texto;
    }
    #endregion

    #region "FACCAO/AVIAMENTO"
    public StringBuilder MontarFaccaoAviamento(StringBuilder _texto, RelatorioHB _relatorio, string nomeRel, string breakClass)
    {
        string _tipo = "";
        int totalGrade = (Convert.ToInt32(_relatorio.GRADE.GRADE_EXP) +
                            Convert.ToInt32(_relatorio.GRADE.GRADE_XP) +
                            Convert.ToInt32(_relatorio.GRADE.GRADE_PP) +
                            Convert.ToInt32(_relatorio.GRADE.GRADE_P) +
                            Convert.ToInt32(_relatorio.GRADE.GRADE_M) +
                            Convert.ToInt32(_relatorio.GRADE.GRADE_G) +
                            Convert.ToInt32(_relatorio.GRADE.GRADE_GG));

        int totalGradeAtacado = 0;
        if (_relatorio.GRADEATACADO != null)
        {
            totalGradeAtacado = (Convert.ToInt32(_relatorio.GRADEATACADO.GRADE_EXP) +
                                        Convert.ToInt32(_relatorio.GRADEATACADO.GRADE_XP) +
                                        Convert.ToInt32(_relatorio.GRADEATACADO.GRADE_PP) +
                                        Convert.ToInt32(_relatorio.GRADEATACADO.GRADE_P) +
                                        Convert.ToInt32(_relatorio.GRADEATACADO.GRADE_M) +
                                        Convert.ToInt32(_relatorio.GRADEATACADO.GRADE_G) +
                                        Convert.ToInt32(_relatorio.GRADEATACADO.GRADE_GG));
        }

        if (_relatorio.MOSTRUARIO == "S")
        {
            _tipo = "MOSTRUÁRIO";
        }
        else
        {
            if (totalGrade == totalGradeAtacado)
                _tipo = "ATACADO";

            if (totalGrade > 0 && totalGradeAtacado == 0)
                _tipo = "VAREJO";

            if (totalGrade > 0 && totalGradeAtacado > 0 && totalGrade != totalGradeAtacado)
                _tipo = "VAREJO/ATACADO";
        }

        var _gradeNome = new ProducaoController().ObterGradeNome(Convert.ToInt32(_relatorio.PROD_GRADE));

        var dataFim = new ProducaoController().ObterProcessoHB(_relatorio.CODIGO, 3);

        _texto.Append("    <br />");
        _texto.Append("    <span>" + ((nomeRel == "1") ? "FACÇÃO" : "AVIAMENTO") + "</span>");
        _texto.Append("    <div id='div" + nomeRel + "' align='center' class='" + breakClass + "'>");
        _texto.Append("        <table border='1' cellpadding='0' cellspacing='0' width='689' style='width: 517pt;");
        _texto.Append("            padding: 0px; color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
        _texto.Append("            background: white; white-space: nowrap;'>");
        _texto.Append("            <tr>");
        _texto.Append("                <td style='line-height: 20px;'>");
        _texto.Append("                    <table border='0' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
        _texto.Append("                        width: 517pt'>");
        _texto.Append("                        <tr style='text-align: left; border-top: 2px solid #000; border-bottom: none;'>");
        _texto.Append("                            <td style='width: 115px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.Append("                                HB: " + _relatorio.HB);
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='width: 185px; border-right: 1px solid #000;'>");
        _texto.Append("                                GRUPO: " + _relatorio.GRUPO);
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='border-right: 1px solid #000;'>");
        _texto.Append("                                NOME: " + _relatorio.CODIGO_PRODUTO_LINX + " - " + _relatorio.NOME);
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='border-right: 1px solid #000; border-bottom: 1px solid #000;' valign='top'>");
        _texto.Append("                                MOSTRUÁRIO: " + ((_relatorio.MOSTRUARIO == "S") ? "SIM" : "NÃO"));
        _texto.Append("                            </td>");
        _texto.Append("                        </tr>");
        _texto.Append("                        <tr style='border: 1px solid #000;'>");
        _texto.Append("                            <td style='border-left: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.Append("                                DATA: " + ((_relatorio.PROCESSO == null) ? "&nbsp;" : Convert.ToDateTime(_relatorio.PROCESSO.DATA_FIM).ToString("dd/MM/yyyy")));
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='border-right: 1px solid #000;'>");
        _texto.Append("                                COLEÇÃO: " + _relatorio.COLECAO);
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='border-right: 1px solid #000;'>");
        _texto.Append("                                COR: " + _relatorio.COD_COR + " - " + _relatorio.DESC_COR);
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='border-top: 1px solid #FFF; text-align: left;'>");
        _texto.Append("                                O.P.: " + _relatorio.ORDEM_PRODUCAO);
        _texto.Append("                            </td>");
        _texto.Append("                        </tr>");

        _texto.Append("                        <tr style='border: 1px solid #000;'>");
        if (_relatorio.SIGNED == "S")
        {
            _texto.Append("                            <td colspan='2' style='border-right: 1px solid #000;'>");
            _texto.Append("                                GRIFFE: " + _relatorio.GRIFFE);
            _texto.Append("                            </td>");
            _texto.Append("                            <td colspan='2' style='border-right: 1px solid #000;'>");
            _texto.Append("                                SIGNED: " + _relatorio.SIGNED_NOME);
            _texto.Append("                            </td>");
        }
        else
        {
            _texto.Append("                            <td colspan='4' style='border-right: 1px solid #000;'>");
            _texto.Append("                                GRIFFE: " + _relatorio.GRIFFE);
            _texto.Append("                            </td>");
        }
        _texto.Append("                        </tr>");

        _texto.Append("                    </table>");
        _texto.Append("                </td>");
        _texto.Append("            </tr>");
        _texto.Append("            <tr>");
        _texto.Append("                <td style='line-height: 16px;'>");
        _texto.Append("                    <table border='0' cellpadding='0' cellspacing='0' width='689' style='border-collapse: collapse;");
        _texto.Append("                        width: 517pt'>");
        _texto.Append("                        <tr style='border-top: 1px solid #000;'>");
        _texto.Append("                            <td colspan='2' style='border-left: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.Append("                                COMPOSIÇÃO");
        _texto.Append("                            </td>");
        _texto.Append("                        </tr>");
        foreach (SP_OBTER_COMPOSICAO_HBResult c in new ProducaoController().ObterListaComposicaoHB(_relatorio.CODIGO))
        {
            _texto.Append("                        <tr style='border: 1px solid #000'>");
            _texto.Append("                            <td style='text-align: left; width: 200px; '>");
            _texto.Append("                            " + c.DESCRICAO);
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='text-align: left;'>");
            _texto.Append("                            " + c.COMPOSICAO);
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
        }
        _texto.Append("                    </table>");
        _texto.Append("                </td>");
        _texto.Append("            </tr>");
        _texto.Append("            <tr>");
        _texto.Append("                <td style='border-right: 1px solid #000; border-left: 1px solid #000; line-height: 10px;'>");
        _texto.Append("                    &nbsp;");
        _texto.Append("                </td>");
        _texto.Append("            </tr>");
        _texto.Append("            <tr>");
        _texto.Append("                <td>");
        _texto.Append("                    <table border='0' cellpadding='0' cellspacing='0' width='689' style='border-collapse: collapse;");
        _texto.Append("                        width: 517pt'>");
        _texto.Append("                        <tr style='text-align: center; border-top: 2px solid #000;'>");
        _texto.Append("                            <td style='width: 55px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.Append("                                &nbsp;");
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='width: 55px; border-right: 1px solid #000;'>");
        _texto.Append("                             " + ((_gradeNome != null) ? _gradeNome.GRADE_EXP : "EXP"));
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='width: 55px; border-right: 1px solid #000;'>");
        _texto.Append("                             " + ((_gradeNome != null) ? _gradeNome.GRADE_XP : "XP"));
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='width: 55px; border-right: 1px solid #000;'>");
        _texto.Append("                             " + ((_gradeNome != null) ? _gradeNome.GRADE_PP : "PP"));
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='width: 55px; border-right: 1px solid #000;'>");
        _texto.Append("                             " + ((_gradeNome != null) ? _gradeNome.GRADE_P : "P"));
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='width: 55px; border-right: 1px solid #000;'>");
        _texto.Append("                             " + ((_gradeNome != null) ? _gradeNome.GRADE_M : "M"));
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='width: 55px; border-right: 1px solid #000;'>");
        _texto.Append("                             " + ((_gradeNome != null) ? _gradeNome.GRADE_G : "G"));
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='width: 55px; border-right: 1px solid #000;'>");
        _texto.Append("                             " + ((_gradeNome != null) ? _gradeNome.GRADE_GG : "GG"));
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='width: 55px; border-right: 1px solid #000;'>");
        _texto.Append("                                TOTAL");
        _texto.Append("                            </td>");
        _texto.Append("                            <td rowspan='4' style='border: 1px solid #000;' valign='middle'>");
        _texto.Append("                                <img alt='Foto Tecido' width='90' height='120' src='..\\.." + _relatorio.FOTO_TECIDO.Replace("~", "").Replace("/", "\\") + "' />");
        _texto.Append("                            </td>");
        _texto.Append("                        </tr>");
        _texto.Append("                        <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
        _texto.Append("                            <td style='text-align: left; border-right: 1px solid #000;'>");
        _texto.Append("                                VAREJO");
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + (_relatorio.GRADE.GRADE_EXP - ((_relatorio.GRADEATACADO == null) ? 0 : _relatorio.GRADEATACADO.GRADE_EXP)));
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + (_relatorio.GRADE.GRADE_XP - ((_relatorio.GRADEATACADO == null) ? 0 : _relatorio.GRADEATACADO.GRADE_XP)));
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + (_relatorio.GRADE.GRADE_PP - ((_relatorio.GRADEATACADO == null) ? 0 : _relatorio.GRADEATACADO.GRADE_PP)));
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + (_relatorio.GRADE.GRADE_P - ((_relatorio.GRADEATACADO == null) ? 0 : _relatorio.GRADEATACADO.GRADE_P)));
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + (_relatorio.GRADE.GRADE_M - ((_relatorio.GRADEATACADO == null) ? 0 : _relatorio.GRADEATACADO.GRADE_M)));
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + (_relatorio.GRADE.GRADE_G - ((_relatorio.GRADEATACADO == null) ? 0 : _relatorio.GRADEATACADO.GRADE_G)));
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + (_relatorio.GRADE.GRADE_GG - ((_relatorio.GRADEATACADO == null) ? 0 : _relatorio.GRADEATACADO.GRADE_GG)));
        _texto.Append("                            </td>");
        _texto.Append("                            <td>");
        _texto.Append("                                " + (totalGrade - totalGradeAtacado).ToString());
        _texto.Append("                            </td>");
        _texto.Append("                        </tr>");
        _texto.Append("                        <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
        _texto.Append("                            <td style='text-align: left; border-right: 1px solid #000;'>");
        _texto.Append("                                ATACADO");
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + ((_relatorio.GRADEATACADO == null) ? "" : _relatorio.GRADEATACADO.GRADE_EXP.ToString()));
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + ((_relatorio.GRADEATACADO == null) ? "" : _relatorio.GRADEATACADO.GRADE_XP.ToString()));
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + ((_relatorio.GRADEATACADO == null) ? "" : _relatorio.GRADEATACADO.GRADE_PP.ToString()));
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + ((_relatorio.GRADEATACADO == null) ? "" : _relatorio.GRADEATACADO.GRADE_P.ToString()));
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + ((_relatorio.GRADEATACADO == null) ? "" : _relatorio.GRADEATACADO.GRADE_M.ToString()));
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + ((_relatorio.GRADEATACADO == null) ? "" : _relatorio.GRADEATACADO.GRADE_G.ToString()));
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + ((_relatorio.GRADEATACADO == null) ? "" : _relatorio.GRADEATACADO.GRADE_GG.ToString()));
        _texto.Append("                            </td>");
        _texto.Append("                            <td>");
        _texto.Append("                                " + ((totalGradeAtacado <= 0) ? "&nbsp;" : totalGradeAtacado.ToString()));
        _texto.Append("                            </td>");
        _texto.Append("                        </tr>");
        _texto.Append("                        <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
        _texto.Append("                            <td style='text-align: left; border-right: 1px solid #000;'>");
        _texto.Append("                                TOTAL");
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + _relatorio.GRADE.GRADE_EXP);
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + _relatorio.GRADE.GRADE_XP);
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + _relatorio.GRADE.GRADE_PP);
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + _relatorio.GRADE.GRADE_P);
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + _relatorio.GRADE.GRADE_M);
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + _relatorio.GRADE.GRADE_G);
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + _relatorio.GRADE.GRADE_GG);
        _texto.Append("                            </td>");
        _texto.Append("                            <td>");
        _texto.Append("                            " + totalGrade.ToString());
        _texto.Append("                            </td>");
        _texto.Append("                        </tr>");
        _texto.Append("                    </table>");
        _texto.Append("                </td>");
        _texto.Append("            </tr>");
        _texto.Append("            <tr>");
        _texto.Append("                <td>");
        _texto.Append("                    <table border='0' cellpadding='0' cellspacing='0' width='689' style='border-collapse: collapse;");
        _texto.Append("                        width: 517pt'>");
        _texto.Append("                        <tr style='text-align: center; border-top: 1px solid #000;'>");
        _texto.Append("                            <td style='width: 110px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.Append("                                1ª GRADE");
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='width: 110px; border-right: 1px solid #000;'>");
        _texto.Append("                                2ª GRADE");
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='width: 110px; border-right: 1px solid #000;'>");
        _texto.Append("                                3ª GRADE");
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='width: 110px; border-right: 1px solid #000;'>");
        _texto.Append("                                4ª GRADE");
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='width: 110px; border-right: 1px solid #000;'>");
        _texto.Append("                                5ª GRADE");
        _texto.Append("                            </td>");
        _texto.Append("                        </tr>");
        _texto.Append("                        <tr style='text-align: center; border-top: 1px solid #000;'>");
        _texto.Append("                            <td style='width: 110px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.Append("                                " + ((_relatorio.GRADE_CORTE.Count > 0) ? _relatorio.GRADE_CORTE[0].GRADE : "&nbsp;"));
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='width: 110px; border-right: 1px solid #000;'>");
        _texto.Append("                                " + ((_relatorio.GRADE_CORTE.Count > 1) ? _relatorio.GRADE_CORTE[1].GRADE : "&nbsp;"));
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='width: 110px; border-right: 1px solid #000;'>");
        _texto.Append("                                " + ((_relatorio.GRADE_CORTE.Count > 2) ? _relatorio.GRADE_CORTE[2].GRADE : "&nbsp;"));
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='width: 110px; border-right: 1px solid #000;'>");
        _texto.Append("                                " + ((_relatorio.GRADE_CORTE.Count > 3) ? _relatorio.GRADE_CORTE[3].GRADE : "&nbsp;"));
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='width: 110px; border-right: 1px solid #000;'>");
        _texto.Append("                                " + ((_relatorio.GRADE_CORTE.Count > 4) ? _relatorio.GRADE_CORTE[4].GRADE : "&nbsp;"));
        _texto.Append("                            </td>");
        _texto.Append("                        </tr>");
        _texto.Append("                        <tr style='text-align: center; border-top: 1px solid #000;'>");
        _texto.Append("                            <td style='width: 110px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.Append("                                " + ((_relatorio.GRADE_CORTE.Count > 0) ? _relatorio.GRADE_CORTE[0].FOLHA.ToString() : "&nbsp;"));
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='width: 110px; border-right: 1px solid #000;'>");
        _texto.Append("                                " + ((_relatorio.GRADE_CORTE.Count > 1) ? _relatorio.GRADE_CORTE[1].FOLHA.ToString() : "&nbsp;"));
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='width: 110px; border-right: 1px solid #000;'>");
        _texto.Append("                                " + ((_relatorio.GRADE_CORTE.Count > 2) ? _relatorio.GRADE_CORTE[2].FOLHA.ToString() : "&nbsp;"));
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='width: 110px; border-right: 1px solid #000;'>");
        _texto.Append("                                " + ((_relatorio.GRADE_CORTE.Count > 3) ? _relatorio.GRADE_CORTE[3].FOLHA.ToString() : "&nbsp;"));
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='width: 110px; border-right: 1px solid #000;'>");
        _texto.Append("                                " + ((_relatorio.GRADE_CORTE.Count > 4) ? _relatorio.GRADE_CORTE[4].FOLHA.ToString() : "&nbsp;"));
        _texto.Append("                            </td>");
        _texto.Append("                        </tr>");
        _texto.Append("                        <tr style='text-align: center; border-bottom: 1px solid #000; border-top: 1px solid #000;'>");
        _texto.Append("                            <td style='width: 110px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.Append("                                &nbsp;");
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='width: 110px; border-right: 1px solid #000;'>");
        _texto.Append("                                &nbsp;");
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='width: 110px; border-right: 1px solid #000;'>");
        _texto.Append("                                &nbsp;");
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='width: 110px; border-right: 1px solid #000;'>");
        _texto.Append("                                &nbsp;");
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='width: 110px; border-right: 1px solid #000;'>");
        _texto.Append("                                &nbsp;");
        _texto.Append("                            </td>");
        _texto.Append("                        </tr>");

        if (_relatorio.GRADE_CORTE.Count > 5)
        {
            _texto.Append("                        <tr style='text-align: center; border-top: 1px solid #000;'>");
            _texto.Append("                            <td style='width: 110px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.Append("                                6ª GRADE");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='width: 110px; border-right: 1px solid #000;'>");
            _texto.Append("                                7ª GRADE");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='width: 110px; border-right: 1px solid #000;'>");
            _texto.Append("                                8ª GRADE");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='width: 110px; border-right: 1px solid #000;'>");
            _texto.Append("                                9ª GRADE");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='width: 110px; border-right: 1px solid #000;'>");
            _texto.Append("                                10ª GRADE");
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                        <tr style='text-align: center; border-top: 1px solid #000;'>");
            _texto.Append("                            <td style='width: 110px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.Append("                                " + ((_relatorio.GRADE_CORTE.Count > 5) ? _relatorio.GRADE_CORTE[5].GRADE : "&nbsp;"));
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='width: 110px; border-right: 1px solid #000;'>");
            _texto.Append("                                " + ((_relatorio.GRADE_CORTE.Count > 6) ? _relatorio.GRADE_CORTE[6].GRADE : "&nbsp;"));
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='width: 110px; border-right: 1px solid #000;'>");
            _texto.Append("                                " + ((_relatorio.GRADE_CORTE.Count > 7) ? _relatorio.GRADE_CORTE[7].GRADE : "&nbsp;"));
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='width: 110px; border-right: 1px solid #000;'>");
            _texto.Append("                                " + ((_relatorio.GRADE_CORTE.Count > 8) ? _relatorio.GRADE_CORTE[8].GRADE : "&nbsp;"));
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='width: 110px; border-right: 1px solid #000;'>");
            _texto.Append("                                " + ((_relatorio.GRADE_CORTE.Count > 9) ? _relatorio.GRADE_CORTE[9].GRADE : "&nbsp;"));
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                        <tr style='text-align: center; border-top: 1px solid #000;'>");
            _texto.Append("                            <td style='width: 110px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.Append("                                " + ((_relatorio.GRADE_CORTE.Count > 5) ? _relatorio.GRADE_CORTE[5].FOLHA.ToString() : "&nbsp;"));
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='width: 110px; border-right: 1px solid #000;'>");
            _texto.Append("                                " + ((_relatorio.GRADE_CORTE.Count > 6) ? _relatorio.GRADE_CORTE[6].FOLHA.ToString() : "&nbsp;"));
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='width: 110px; border-right: 1px solid #000;'>");
            _texto.Append("                                " + ((_relatorio.GRADE_CORTE.Count > 7) ? _relatorio.GRADE_CORTE[7].FOLHA.ToString() : "&nbsp;"));
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='width: 110px; border-right: 1px solid #000;'>");
            _texto.Append("                                " + ((_relatorio.GRADE_CORTE.Count > 8) ? _relatorio.GRADE_CORTE[8].FOLHA.ToString() : "&nbsp;"));
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='width: 110px; border-right: 1px solid #000;'>");
            _texto.Append("                                " + ((_relatorio.GRADE_CORTE.Count > 9) ? _relatorio.GRADE_CORTE[9].FOLHA.ToString() : "&nbsp;"));
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            _texto.Append("                        <tr style='text-align: center; border-bottom: 1px solid #000; border-top: 1px solid #000;'>");
            _texto.Append("                            <td style='width: 110px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='width: 110px; border-right: 1px solid #000;'>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='width: 110px; border-right: 1px solid #000;'>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='width: 110px; border-right: 1px solid #000;'>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='width: 110px; border-right: 1px solid #000;'>");
            _texto.Append("                                &nbsp;");
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
        }
        _texto.Append("                    </table>");
        _texto.Append("                </td>");
        _texto.Append("            </tr>");

        _texto.Append("            <tr>");
        _texto.Append("                <td style='border-right: 1px solid #000; border-left: 1px solid #000; line-height: 5px;'>");
        _texto.Append("                    &nbsp;");
        _texto.Append("                </td>");
        _texto.Append("            </tr>");
        _texto.Append("            <tr>");
        _texto.Append("                <td style='line-height: 20px;'>");
        _texto.Append("                    <table border='0' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
        _texto.Append("                        width: 517pt'>");
        _texto.Append("                        <tr style='text-align: left; border-top: 2px solid #000; border-bottom: none;'>");
        _texto.Append("                            <td style='width: 273px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.Append("                                SKIL OU BORDADO - (" + _relatorio.FaseEstamparia + ")");
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='width: 210px; border-right: 1px solid #000;'>");
        _texto.Append("                                LAVANDERIA - (" + _relatorio.FaseLavanderia + ")");
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='border-right: 1px solid #000; border-bottom: 1px dotted #000;' valign='top'>");
        _texto.Append("                                MODELAGEM");
        _texto.Append("                            </td>");
        _texto.Append("                        </tr>");
        _texto.Append("                        <tr style='border: 1px solid #000; line-height: 50px;'>");
        _texto.Append("                            <td style='border-left: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.Append("                                &nbsp;");
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='border-right: 1px solid #000;'>");
        _texto.Append("                                &nbsp;");
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='border-top: 1px solid #000; text-align: center;'>");
        _texto.Append("                                " + _relatorio.MODELAGEM);
        _texto.Append("                            </td>");
        _texto.Append("                        </tr>");
        _texto.Append("                    </table>");
        _texto.Append("                </td>");
        _texto.Append("            </tr>");
        _texto.Append("            <tr>");
        _texto.Append("                <td style='line-height: 16px; border-bottom: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.Append("                    <table border='0' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
        _texto.Append("                        width: 517pt'>");
        _texto.Append("                        <tr>");
        _texto.Append("                            <td style='width: 485px; border-left: 1px solid #000; border-right: 1px solid #000;'");
        _texto.Append("                                valign='top'>");
        _texto.Append("                                <table border='0' cellpadding='0' cellspacing='0' width='100%' style='border-collapse: collapse;'>");
        _texto.Append("                                    <tr style='border-top: 1px solid #000;'>");
        _texto.Append("                                        <td style='width: 100%; border-left: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.Append("                                            AVIAMENTOS - " + _tipo);
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");

        foreach (PROD_HB_PROD_AVIAMENTO av in _relatorio.AVIAMENTO)
        {
            _texto.Append("                                    <tr style='border: 1px solid #000'>");
            _texto.Append("                                        <td style='text-align: center;'>");
            if (av.PROD_AVIAMENTO1 != null)
                _texto.Append("                                            " + (av.QTDE + ((av.QTDE_EXTRA == null) ? 0 : av.QTDE_EXTRA)).ToString().Replace(",000", "") + " " + av.PROD_AVIAMENTO1.UNIDADE_MEDIDA1.DESCRICAO + "s - " + av.PROD_AVIAMENTO1.DESCRICAO + " - " + av.DESCRICAO + " " + ((av.COMPRA_EXTRA == 'S') ? " - Extra" : ""));
            else
                _texto.Append("                                            " + (av.QTDE + ((av.QTDE_EXTRA == null) ? 0 : av.QTDE_EXTRA)).ToString().Replace(",000", "") + " " + av.SUBGRUPO + " - " + av.COR_FORNECEDOR + " - Quantidade: " + av.QTDE_POR_PECA.ToString() + " " + av.DESCRICAO + " " + ((av.COMPRA_EXTRA == 'S') ? " - Extra" : ""));
            _texto.Append("                                        </td>");
            _texto.Append("                                    </tr>");
        }
        _texto.Append("                                </table>");
        _texto.Append("                            </td>");
        _texto.Append("                            <td valign='top'>");
        _texto.Append("                                <table border='0' cellpadding='0' cellspacing='0' width='100%' style='border-collapse: collapse;'>");
        _texto.Append("                                    <tr style='border-top: 1px solid #000;'>");
        _texto.Append("                                        <td style='width: 100%; border-bottom: 1px solid #FFF;'>");
        _texto.Append("                                            TECIDO");
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");
        _texto.Append("                                    <tr style='border-top: 1px solid #000; line-height: 33px;'>");
        _texto.Append("                                        <td style='width: 100%; border-right: 1px solid #000;'>");
        _texto.Append("                                            " + (_relatorio.TECIDO + ((_relatorio.COR_FORNECEDOR.Trim() == "") ? "" : "<br /> - " + _relatorio.COR_FORNECEDOR.Trim())));
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");
        foreach (PROD_HB p in _relatorio.DETALHE)
        {
            _texto.Append("                                    <tr style='border-top: 1px solid #000; border-right: 1px solid #000; border-bottom: 1px solid #000;'>");
            _texto.Append("                                        <td>");
            _texto.Append("                                            DET " + p.PROD_DETALHE1.DESCRICAO.ToUpper());
            _texto.Append("                                        </td>");
            _texto.Append("                                    </tr>");
        }
        _texto.Append("                                </table>");
        _texto.Append("                            </td>");
        _texto.Append("                        </tr>");
        _texto.Append("                    </table>");
        _texto.Append("                </td>");
        _texto.Append("            </tr>");
        _texto.Append("            <tr>");
        _texto.Append("                <td style='border-right: 1px solid #000; border-left: 1px solid #000; line-height: 5px;'>");
        _texto.Append("                    &nbsp;");
        _texto.Append("                </td>");
        _texto.Append("            </tr>");
        _texto.Append("            <tr>");
        _texto.Append("                <td style='border-right: 1px solid #000; border-left: 1px solid #000; text-align: center;'>");
        _texto.Append("                    GRADE DE QUANTIDADE ENTREGUE PELO FACCIONISTA");
        _texto.Append("                </td>");
        _texto.Append("            </tr>");
        _texto.Append("            <tr>");
        _texto.Append("                <td style='line-height: 16px; border-bottom: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.Append("                    <table border='0' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
        _texto.Append("                        width: 517pt'>");
        _texto.Append("                        <tr>");
        _texto.Append("                            <td>");
        _texto.Append("                                <table border='0' cellpadding='0' cellspacing='0' width='100%'>");
        _texto.Append("                                    <tr>");
        _texto.Append("                                        <td style='border-right: 1px solid #000; text-align: center; width: 50px;'>");
        _texto.Append("                                         " + ((_gradeNome != null) ? _gradeNome.GRADE_EXP : "EXP"));
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='border-right: 1px solid #000; text-align: center; width: 50px;'>");
        _texto.Append("                                         " + ((_gradeNome != null) ? _gradeNome.GRADE_XP : "XP"));
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='border-right: 1px solid #000; text-align: center; width: 50px;'>");
        _texto.Append("                                         " + ((_gradeNome != null) ? _gradeNome.GRADE_PP : "PP"));
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='border-right: 1px solid #000; text-align: center; width: 50px;'>");
        _texto.Append("                                         " + ((_gradeNome != null) ? _gradeNome.GRADE_P : "P"));
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='border-right: 1px solid #000; text-align: center; width: 50px;'>");
        _texto.Append("                                         " + ((_gradeNome != null) ? _gradeNome.GRADE_M : "M"));
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='border-right: 1px solid #000; text-align: center; width: 50px;'>");
        _texto.Append("                                         " + ((_gradeNome != null) ? _gradeNome.GRADE_G : "G"));
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='border-right: 1px solid #000; text-align: center; width: 50px;'>");
        _texto.Append("                                         " + ((_gradeNome != null) ? _gradeNome.GRADE_GG : "GG"));
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='border-right: 1px solid #000; text-align: center; width: 50px;'>");
        _texto.Append("                                            TOTAL");
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");
        _texto.Append("                                    <tr>");
        _texto.Append("                                        <td style='border: 1px solid #000; text-align: center; width: 50px; border-left: none;'>");
        _texto.Append("                                            &nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='border: 1px solid #000; text-align: center; width: 50px; border-left: none;'>");
        _texto.Append("                                            &nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='border: 1px solid #000; text-align: center; width: 50px; border-left: 1px solid #FFF;'>");
        _texto.Append("                                            &nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='border: 1px solid #000; text-align: center; width: 50px; border-left: 1px solid #FFF;'>");
        _texto.Append("                                            &nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='border: 1px solid #000; text-align: center; width: 50px; border-left: 1px solid #FFF;'>");
        _texto.Append("                                            &nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='border: 1px solid #000; text-align: center; width: 50px; border-left: 1px solid #FFF;'>");
        _texto.Append("                                            &nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='border: 1px solid #000; text-align: center; width: 50px; border-left: 1px solid #FFF;'>");
        _texto.Append("                                            &nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='border: 1px solid #000; text-align: center; width: 50px; border-left: 1px solid #FFF;'>");
        _texto.Append("                                            &nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");
        _texto.Append("                                    <tr>");
        _texto.Append("                                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                                            &nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                                            &nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                                            &nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                                            &nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                                            &nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                                            &nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                                            &nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                                            &nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");
        _texto.Append("                                    <tr>");
        _texto.Append("                                        <td style='border: 1px solid #000;' colspan='8'>");
        _texto.Append("                                            &nbsp;");
        _texto.Append("                                        </td>");
        _texto.Append("                                    </tr>");
        _texto.Append("                                </table>");
        _texto.Append("                            </td>");
        _texto.Append("                            <td valign='middle' style='text-align: center;'>");
        _texto.Append("                                &nbsp;");
        _texto.Append("                            </td>");
        _texto.Append("                        </tr>");
        _texto.Append("                    </table>");
        _texto.Append("                </td>");
        _texto.Append("            </tr>");
        _texto.Append("        </table>");

        _texto.Append("        <div>");
        _texto.Append("            <hr style='border: 1px dashed #000;' />");
        _texto.Append("        </div>");
        _texto.Append("        <table border='0' cellpadding='0' cellspacing='0' width='689' style='width: 517pt;");
        _texto.Append("            padding: 0px; color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
        _texto.Append("            background: white; white-space: nowrap;'>");
        _texto.Append("            <tr>");
        _texto.Append("                <td style='border: 1px solid #000; line-height: 25px; width: 320px; border-right: none;'>");
        _texto.Append("                    <b> &nbsp;RECIBO DE FACÇÃO</b>");
        _texto.Append("                </td>");
        _texto.Append("                <td style='border: 1px solid #000;'>");
        _texto.Append("                    &nbsp;FACÇÃO: &nbsp;");
        _texto.Append("                </td>");
        _texto.Append("            </tr>");
        _texto.Append("            <tr>");
        _texto.Append("                <td style='line-height: 20px;' colspan='2'>");
        _texto.Append("                    <table border='0' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
        _texto.Append("                        width: 517pt'>");
        _texto.Append("                        <tr style='text-align: left; border-top:none; border-bottom: none;'>");
        _texto.Append("                            <td style='width: 215px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.Append("                                &nbsp;Coleção: " + _relatorio.COLECAO);
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='width: 250px; border-right: 1px solid #000;'>");
        _texto.Append("                                &nbsp;HB: " + _relatorio.HB);
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='border-right: 1px solid #000;' valign='top'>");
        _texto.Append("                                &nbsp;Nome:" + _relatorio.CODIGO_PRODUTO_LINX + " - " + _relatorio.NOME);
        _texto.Append("                            </td>");
        _texto.Append("                        </tr>");
        _texto.Append("                        <tr style='border: 1px solid #000;'>");
        _texto.Append("                            <td style='border-left: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.Append("                                &nbsp;Saída: " + ((dataFim.DATA_FIM != null) ? Convert.ToDateTime(dataFim.DATA_FIM).ToString("dd/MM/yyyy") : ""));
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='border-right: 1px solid #000;'>");
        _texto.Append("                                &nbsp;QTDE: " + totalGrade.ToString());
        _texto.Append("                            </td>");
        _texto.Append("                            <td style=''>");
        _texto.Append("                                &nbsp;Cor: " + _relatorio.COD_COR + " - " + _relatorio.DESC_COR);
        _texto.Append("                            </td>");
        _texto.Append("                        </tr>");
        _texto.Append("                        <tr>");
        _texto.Append("                            <td colspan='3' style='line-height: 5px;'>");
        _texto.Append("                                &nbsp;");
        _texto.Append("                            </td>");
        _texto.Append("                        </tr>");
        _texto.Append("                        <tr style='text-align: left; border-top: 1px solid #000; border-bottom: none;'>");
        _texto.Append("                            <td style='border-left: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.Append("                                &nbsp;Etiqueta Tamanho: &nbsp;");
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='border-right: 1px solid #000;'>");
        _texto.Append("                                &nbsp;Etiqueta Composição: &nbsp;");
        _texto.Append("                            </td>");
        _texto.Append("                            <td style='border-right: 1px solid #000;' valign='top'>");
        _texto.Append("                                &nbsp;Gabarito: " + ((_relatorio.GABARITO == "S") ? "SIM" : "NÃO"));
        _texto.Append("                            </td>");
        _texto.Append("                        </tr>");
        _texto.Append("                    </table>");
        _texto.Append("                </td>");
        _texto.Append("            </tr>");
        _texto.Append("            <tr>");
        _texto.Append("                <td style='line-height: 20px;' valign='top'>");
        _texto.Append("                    <table border='0' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
        _texto.Append("                        width: 100%'>");
        _texto.Append("                        <tr style='text-align: left; border-top: 1px solid #000;; border-bottom: 1px solid #000;'>");
        _texto.Append("                            <td style='width: 215px; border-left: 1px solid #000; border-right: none; background-color: #c8a1a1'>");
        _texto.Append("                                &nbsp; " + ((_relatorio.DETALHE == null || _relatorio.DETALHE.Count() <= 0) ? "Sem " : "") + "Detalhes");
        _texto.Append("                            </td>");
        _texto.Append("                        </tr>");

        string preencherVazio = " " +
                                " <tr style='border: 1px solid #000;'>" +
                                "       <td style='border-left: 1px solid #000; border-right: 1px solid #000;'>" +
                                "           &nbsp;" +
                                "       </td>" +
                                " </tr>" +
                                " ";

        int totalDetalhe = 0;
        foreach (PROD_HB p in _relatorio.DETALHE)
        {

            _texto.Append("                        <tr style='border: 1px solid #000;'>");
            _texto.Append("                            <td style='border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.Append("                                &nbsp;" + p.PROD_DETALHE1.DESCRICAO.ToUpper());
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            totalDetalhe += 1;

        }
        _texto.Append("                        ***DET");

        _texto.Append("                    </table>");
        _texto.Append("                </td>");
        _texto.Append("                <td style='line-height: 20px;' valign='top'>");
        _texto.Append("                    <table border='0' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
        _texto.Append("                        width: 100%'>");
        _texto.Append("                        <tr style='text-align: left; border-top: 1px solid #000; border-bottom: 1px solid #000;'>");
        _texto.Append("                            <td style='width: 215px; border-left: 1px solid #000; border-right: 1px solid #000; background-color: #c8a1a1'>");
        _texto.Append("                                &nbsp;Aviamentos");
        _texto.Append("                            </td>");
        _texto.Append("                        </tr>");

        int totalAviamento = 0;
        foreach (PROD_HB_PROD_AVIAMENTO av in _relatorio.AVIAMENTO)
        {
            _texto.Append("                        <tr style='border: 1px solid #000;'>");
            _texto.Append("                            <td style='border-left: 1px solid #000; border-right: 1px solid #000;'>");
            if (av.PROD_AVIAMENTO1 != null)
                _texto.Append("                                             &nbsp;" + av.PROD_AVIAMENTO1.UNIDADE_MEDIDA1.DESCRICAO + "s - " + av.PROD_AVIAMENTO1.DESCRICAO + " - " + av.DESCRICAO + " " + ((av.COMPRA_EXTRA == 'S') ? " - Extra" : ""));
            else
                _texto.Append("                                             &nbsp;" + av.SUBGRUPO + " - " + av.COR_FORNECEDOR + " " + ((av.COMPRA_EXTRA == 'S') ? " - Extra" : ""));
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
            totalAviamento += 1;
        }
        _texto.Append("                        ***AVI");

        _texto.Append("                    </table>");
        _texto.Append("                </td>");
        _texto.Append("            </tr>");
        _texto.Append("            <tr style='line-height: 40px;'>");
        _texto.Append("                <td style='line-height: 40px;'>");
        _texto.Append("                    DATA:________________________________________ ");
        _texto.Append("                </td>");
        _texto.Append("                <td>");
        _texto.Append("                    ASSINATURA:_____________________________________ ");
        _texto.Append("                </td>");
        _texto.Append("            </tr>");
        _texto.Append("        </table>");

        _texto.Append("    </div>");

        //PREENCHE LINHAS VAZIAS
        if (totalDetalhe > totalAviamento)
        {
            int qtdeLinhaAviamento = totalDetalhe - totalAviamento;

            string auxConcat = "";
            int i = 0;
            while (i < qtdeLinhaAviamento)
            {
                auxConcat += preencherVazio;
                i += 1;
            }

            _texto.Replace("***AVI", auxConcat);
        }
        else if (totalAviamento > totalDetalhe)
        {
            int qtdeLinhaDetalhe = totalAviamento - totalDetalhe;

            string auxConcat = "";
            int i = 0;
            while (i < qtdeLinhaDetalhe)
            {
                auxConcat += preencherVazio;
                i += 1;
            }

            _texto.Replace("***DET", auxConcat);
        }

        _texto.Replace("***DET", "");
        _texto.Replace("***AVI", "");

        return _texto;
    }
    #endregion

    #region "LOGISTICA"
    public StringBuilder MontarLogistica1(StringBuilder _texto, RelatorioHB _relatorio, string breakClass)
    {
        ProducaoController prodController = new ProducaoController();
        bool etiquetaCodigoBarraOK = false;

        var prodHB = prodController.ObterHB(_relatorio.CODIGO);
        if (prodHB != null)
        {
            if (prodHB.DATA_BAIXA_ETIQUETA != null)
                etiquetaCodigoBarraOK = true;
        }

        var _gradeNome = prodController.ObterGradeNome(_relatorio.CODIGO);

        _texto.Append(" <br />");
        _texto.Append("    <span>LOGÍSTICA " + ((DATA_IMP_FIC_LOGISTICA == "") ? "" : " - REIMPRESSÃO") + "</span>");
        if (_relatorio.DETALHE.Count <= 0)
            _texto.Append("<div id='divGrade' align='center'>");
        else
            _texto.Append("<div id='divGrade' align='center' class='" + breakClass + "'>");
        _texto.Append("     <table border='0' cellpadding='0' cellspacing='0' style='width: 517pt; padding: 0px;");
        _texto.Append("     color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
        _texto.Append("     background: white; white-space: nowrap;'>");
        _texto.Append("         <tr>");
        _texto.Append("             <td style='line-height: 20px;'>");
        _texto.Append("                 <table border='0' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
        _texto.Append("                 width: 517pt'>");
        _texto.Append("                     <tr style='text-align: left; border-top: 2px solid #000; border-bottom: none;'>");
        _texto.Append("                         <td colspan='2' style='width: 100px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.Append("                             &nbsp;COLEÇÃO: " + _relatorio.COLECAO);
        _texto.Append("                         </td>");
        _texto.Append("                         <td style='border-right: 1px solid #000; border-bottom: 1px dotted #FFF;' valign='top'>");
        if (_relatorio.SIGNED == "S")
            _texto.Append("                             &nbsp;SIGNED: " + _relatorio.SIGNED_NOME);
        else
            _texto.Append("                             &nbsp;");
        _texto.Append("                         </td>");
        _texto.Append("                         <td style='border-right: 1px solid #000; border-bottom: 1px dotted #FFF;' valign='top'>");
        _texto.Append("                             &nbsp;MOSTRUÁRIO: " + ((_relatorio.MOSTRUARIO == "S") ? "SIM" : "NÃO"));
        _texto.Append("                         </td>");
        _texto.Append("                     </tr>");
        _texto.Append("                     <tr style='border: 1px solid #000;'>");
        _texto.Append("                         <td style='border-left: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.Append("                             &nbsp;HB: " + _relatorio.HB);
        _texto.Append("                         </td>");
        _texto.Append("                         <td style='border-right: 1px solid #000;'>");
        _texto.Append("                             &nbsp;CÓD: " + _relatorio.CODIGO_PRODUTO_LINX);
        _texto.Append("                         </td>");
        _texto.Append("                         <td style='border-right: 1px solid #000;'>");
        _texto.Append("                             &nbsp;PRODUTO: " + _relatorio.GRUPO + " " + _relatorio.NOME.Replace(_relatorio.GRUPO, ""));
        _texto.Append("                         </td>");
        _texto.Append("                         <td style='border-right: 1px solid #000; border-bottom: 1px dotted #FFF;' valign='top'>");
        _texto.Append("                             &nbsp;COR: " + _relatorio.COD_COR + " - " + _relatorio.DESC_COR);
        _texto.Append("                         </td>");
        _texto.Append("                     </tr>");
        _texto.Append("                     <tr style='border: 1px solid #000;'>");
        _texto.Append("                         <td colspan='2' style='border-left: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.Append("                             &nbsp;QUANTIDADE: " + (Convert.ToInt32(_relatorio.GRADE.GRADE_EXP) + Convert.ToInt32(_relatorio.GRADE.GRADE_XP) + Convert.ToInt32(_relatorio.GRADE.GRADE_PP) + Convert.ToInt32(_relatorio.GRADE.GRADE_P) + Convert.ToInt32(_relatorio.GRADE.GRADE_M) + Convert.ToInt32(_relatorio.GRADE.GRADE_G) + Convert.ToInt32(_relatorio.GRADE.GRADE_GG)));
        _texto.Append("                         </td>");
        _texto.Append("                         <td style='border-right: 1px solid #000;'>");
        _texto.Append("                             &nbsp;ETI. CÓD. BARRAS: " + ((etiquetaCodigoBarraOK) ? "OK" : "-"));
        _texto.Append("                         </td>");
        _texto.Append("                         <td style='border-right: 1px solid #000; border-bottom: 1px dotted #FFF;' valign='top'>");
        _texto.Append("                             &nbsp;O.P.: " + _relatorio.ORDEM_PRODUCAO);
        _texto.Append("                         </td>");
        _texto.Append("                     </tr>");
        _texto.Append("                 </table>");
        _texto.Append("             </td>");
        _texto.Append("         </tr>");

        _texto.Append("            <tr>");
        _texto.Append("                <td style='line-height: 16px;'>");
        _texto.Append("                    <table border='0' cellpadding='0' cellspacing='0' width='689' style='border-collapse: collapse;");
        _texto.Append("                        width: 517pt'>");
        _texto.Append("                        <tr style='border-top: 1px solid #000;'>");
        _texto.Append("                            <td colspan='2' style='border-left: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.Append("                                COMPOSIÇÃO");
        _texto.Append("                            </td>");
        _texto.Append("                        </tr>");
        foreach (SP_OBTER_COMPOSICAO_HBResult c in new ProducaoController().ObterListaComposicaoHB(_relatorio.CODIGO))
        {
            _texto.Append("                        <tr style='border: 1px solid #000'>");
            _texto.Append("                            <td style='text-align: left; width: 200px; '>");
            _texto.Append("                            " + c.DESCRICAO);
            _texto.Append("                            </td>");
            _texto.Append("                            <td style='text-align: left;'>");
            _texto.Append("                            " + c.COMPOSICAO);
            _texto.Append("                            </td>");
            _texto.Append("                        </tr>");
        }
        _texto.Append("                    </table>");
        _texto.Append("                </td>");
        _texto.Append("            </tr>");

        _texto.Append("            <tr>");
        _texto.Append("                <td style='line-height: 16px;'>");
        _texto.Append("                    <table border='0' cellpadding='0' cellspacing='0' width='689' style='border-collapse: collapse;");
        _texto.Append("                        width: 517pt'>");
        _texto.Append("                        <tr style='border-top: 1px solid #000;'>");
        _texto.Append("                            <td colspan='2' style='border-left: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.Append("                                AVIAMENTO");
        _texto.Append("                            </td>");
        _texto.Append("                        </tr>");
        foreach (PROD_HB_PROD_AVIAMENTO av in _relatorio.AVIAMENTO)
        {
            _texto.Append("                                    <tr style='border: 1px solid #000'>");
            _texto.Append("                                        <td colspan='2' style='text-align: center;'>");
            if (av.PROD_AVIAMENTO1 != null)
                _texto.Append("                                            " + (av.QTDE + ((av.QTDE_EXTRA == null) ? 0 : av.QTDE_EXTRA)).ToString().Replace(",000", "") + " " + av.PROD_AVIAMENTO1.UNIDADE_MEDIDA1.DESCRICAO + "s - " + av.PROD_AVIAMENTO1.DESCRICAO + " - " + av.DESCRICAO + " " + ((av.COMPRA_EXTRA == 'S') ? " - Extra" : ""));
            else
                _texto.Append("                                            " + (av.QTDE + ((av.QTDE_EXTRA == null) ? 0 : av.QTDE_EXTRA)).ToString().Replace(",000", "") + " " + av.SUBGRUPO + " - " + av.COR_FORNECEDOR + " - Quantidade: " + av.QTDE_POR_PECA.ToString() + " " + av.DESCRICAO + " " + ((av.COMPRA_EXTRA == 'S') ? " - Extra" : ""));
            _texto.Append("                                        </td>");
            _texto.Append("                                    </tr>");
        }

        _texto.Append("                    </table>");
        _texto.Append("                </td>");
        _texto.Append("            </tr>");


        _texto.Append("         <tr>");
        _texto.Append("             <td style='border-right: 1px solid #000; border-left: 1px solid #000;'>");
        _texto.Append("                 &nbsp;");
        _texto.Append("             </td>");
        _texto.Append("         </tr>");
        _texto.Append("         <tr>");
        _texto.Append("             <td>");
        _texto.Append("                 <table border='0' cellpadding='0' cellspacing='0' width='689' style='border-collapse: collapse;");
        _texto.Append("                 width: 517pt; " + ((DATA_IMP_FIC_LOGISTICA == "") ? "" : "background:#cac7c9;") + "'>");
        _texto.Append("                     <tr style='text-align: center; border-top: 1px solid #000;'>");
        _texto.Append("                         <td style='width: 100px; text-align: left; border-right: 1px solid #000; border-left: 1px solid #000;'>");
        _texto.Append("                             DATA:");
        _texto.Append("                         </td>");
        _texto.Append("                         <td style='width: 55px; border-right: 1px solid #000;'>");
        _texto.Append("                             " + ((_relatorio.PROCESSO == null) ? "&nbsp;" : Convert.ToDateTime(_relatorio.PROCESSO.DATA_FIM).ToString("dd/MM/yyyy")));
        _texto.Append("                         </td>");
        _texto.Append("                         <td style='width: 55px; border-right: 1px solid #000;'>");
        _texto.Append("                             " + ((_gradeNome != null) ? _gradeNome.GRADE_EXP : "EXP"));
        _texto.Append("                         </td>");
        _texto.Append("                         <td style='width: 55px; border-right: 1px solid #000;'>");
        _texto.Append("                             " + ((_gradeNome != null) ? _gradeNome.GRADE_XP : "XP"));
        _texto.Append("                         </td>");
        _texto.Append("                         <td style='width: 55px; border-right: 1px solid #000;'>");
        _texto.Append("                             " + ((_gradeNome != null) ? _gradeNome.GRADE_PP : "PP"));
        _texto.Append("                         </td>");
        _texto.Append("                         <td style='width: 55px; border-right: 1px solid #000;'>");
        _texto.Append("                             " + ((_gradeNome != null) ? _gradeNome.GRADE_P : "P"));
        _texto.Append("                         </td>");
        _texto.Append("                         <td style='width: 55px; border-right: 1px solid #000;'>");
        _texto.Append("                             " + ((_gradeNome != null) ? _gradeNome.GRADE_M : "M"));
        _texto.Append("                         </td>");
        _texto.Append("                         <td style='width: 55px; border-right: 1px solid #000;'>");
        _texto.Append("                             " + ((_gradeNome != null) ? _gradeNome.GRADE_G : "G"));
        _texto.Append("                         </td>");
        _texto.Append("                         <td style='width: 55px; border-right: 1px solid #000;'>");
        _texto.Append("                             " + ((_gradeNome != null) ? _gradeNome.GRADE_GG : "GG"));
        _texto.Append("                         </td>");
        _texto.Append("                         <td style='width: 55px; border-right: 1px solid #000;'>");
        _texto.Append("                             TOTAL");
        _texto.Append("                         </td>");
        _texto.Append("                     </tr>");
        _texto.Append("                    <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
        _texto.Append("                        <td style='text-align: left; border-right: 1px solid #000;'>");
        _texto.Append("                            VAREJO:");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + (_relatorio.GRADE.GRADE_EXP - ((_relatorio.GRADEATACADO == null) ? 0 : _relatorio.GRADEATACADO.GRADE_EXP)));
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + (_relatorio.GRADE.GRADE_XP - ((_relatorio.GRADEATACADO == null) ? 0 : _relatorio.GRADEATACADO.GRADE_XP)));
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + (_relatorio.GRADE.GRADE_PP - ((_relatorio.GRADEATACADO == null) ? 0 : _relatorio.GRADEATACADO.GRADE_PP)));
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + (_relatorio.GRADE.GRADE_P - ((_relatorio.GRADEATACADO == null) ? 0 : _relatorio.GRADEATACADO.GRADE_P)));
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + (_relatorio.GRADE.GRADE_M - ((_relatorio.GRADEATACADO == null) ? 0 : _relatorio.GRADEATACADO.GRADE_M)));
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + (_relatorio.GRADE.GRADE_G - ((_relatorio.GRADEATACADO == null) ? 0 : _relatorio.GRADEATACADO.GRADE_G)));
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + (_relatorio.GRADE.GRADE_GG - ((_relatorio.GRADEATACADO == null) ? 0 : _relatorio.GRADEATACADO.GRADE_GG)));
        _texto.Append("                        </td>");
        _texto.Append("                        <td>");
        _texto.Append("                            " + (
                                                               Convert.ToInt32(_relatorio.GRADE.GRADE_EXP - ((_relatorio.GRADEATACADO == null) ? 0 : _relatorio.GRADEATACADO.GRADE_EXP)) +
                                                               Convert.ToInt32(_relatorio.GRADE.GRADE_XP - ((_relatorio.GRADEATACADO == null) ? 0 : _relatorio.GRADEATACADO.GRADE_XP)) +
                                                               Convert.ToInt32(_relatorio.GRADE.GRADE_PP - ((_relatorio.GRADEATACADO == null) ? 0 : _relatorio.GRADEATACADO.GRADE_PP)) +
                                                               Convert.ToInt32(_relatorio.GRADE.GRADE_P - ((_relatorio.GRADEATACADO == null) ? 0 : _relatorio.GRADEATACADO.GRADE_P)) +
                                                               Convert.ToInt32(_relatorio.GRADE.GRADE_M - ((_relatorio.GRADEATACADO == null) ? 0 : _relatorio.GRADEATACADO.GRADE_M)) +
                                                               Convert.ToInt32(_relatorio.GRADE.GRADE_G - ((_relatorio.GRADEATACADO == null) ? 0 : _relatorio.GRADEATACADO.GRADE_G)) +
                                                               Convert.ToInt32(_relatorio.GRADE.GRADE_GG - ((_relatorio.GRADEATACADO == null) ? 0 : _relatorio.GRADEATACADO.GRADE_GG))
                                                        ));
        _texto.Append("                        </td>");
        _texto.Append("                    </tr>");
        _texto.Append("                    <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
        _texto.Append("                        <td style='text-align: left; border-right: 1px solid #000;'>");
        _texto.Append("                            ATACADO:");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + ((_relatorio.GRADEATACADO == null) ? "0" : _relatorio.GRADEATACADO.GRADE_EXP.ToString()));
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + ((_relatorio.GRADEATACADO == null) ? "0" : _relatorio.GRADEATACADO.GRADE_XP.ToString()));
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + ((_relatorio.GRADEATACADO == null) ? "0" : _relatorio.GRADEATACADO.GRADE_PP.ToString()));
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + ((_relatorio.GRADEATACADO == null) ? "0" : _relatorio.GRADEATACADO.GRADE_P.ToString()));
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + ((_relatorio.GRADEATACADO == null) ? "0" : _relatorio.GRADEATACADO.GRADE_M.ToString()));
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + ((_relatorio.GRADEATACADO == null) ? "0" : _relatorio.GRADEATACADO.GRADE_G.ToString()));
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + ((_relatorio.GRADEATACADO == null) ? "0" : _relatorio.GRADEATACADO.GRADE_GG.ToString()));
        _texto.Append("                        </td>");
        _texto.Append("                        <td>");
        _texto.Append("                            " + (
                                                            (_relatorio.GRADEATACADO == null) ? "0" :
                                                            (Convert.ToInt32(_relatorio.GRADEATACADO.GRADE_EXP) +
                                                            Convert.ToInt32(_relatorio.GRADEATACADO.GRADE_XP) +
                                                            Convert.ToInt32(_relatorio.GRADEATACADO.GRADE_PP) +
                                                            Convert.ToInt32(_relatorio.GRADEATACADO.GRADE_P) +
                                                            Convert.ToInt32(_relatorio.GRADEATACADO.GRADE_M) +
                                                            Convert.ToInt32(_relatorio.GRADEATACADO.GRADE_G) +
                                                            Convert.ToInt32(_relatorio.GRADEATACADO.GRADE_GG)).ToString()
                                                        ));
        _texto.Append("                        </td>");
        _texto.Append("                    </tr>");

        _texto.Append("                    <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
        _texto.Append("                        <td style='text-align: left; border-right: 1px solid #000;'>");
        _texto.Append("                            TOTAL:");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + _relatorio.GRADE.GRADE_EXP);
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + _relatorio.GRADE.GRADE_XP);
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + _relatorio.GRADE.GRADE_PP);
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + _relatorio.GRADE.GRADE_P);
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + _relatorio.GRADE.GRADE_M);
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + _relatorio.GRADE.GRADE_G);
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + _relatorio.GRADE.GRADE_GG);
        _texto.Append("                        </td>");
        _texto.Append("                        <td>");
        _texto.Append("                            " + (
                                                               Convert.ToInt32(_relatorio.GRADE.GRADE_EXP +
                                                               _relatorio.GRADE.GRADE_XP +
                                                               _relatorio.GRADE.GRADE_PP +
                                                               _relatorio.GRADE.GRADE_P +
                                                               _relatorio.GRADE.GRADE_M +
                                                               _relatorio.GRADE.GRADE_G +
                                                               _relatorio.GRADE.GRADE_GG)
                                                        ));
        _texto.Append("                        </td>");
        _texto.Append("                    </tr>");

        _texto.Append("                    <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
        _texto.Append("                        <td style='text-align: left; border-right: 1px solid #000;'>");
        _texto.Append("                            FACÇÃO:");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                    </tr>");
        _texto.Append("                    <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
        _texto.Append("                        <td style='text-align: left; border-right: 1px solid #000;'>");
        _texto.Append("                            S/ CONF:");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                    </tr>");
        _texto.Append("                    <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
        _texto.Append("                        <td style='text-align: left; border-right: 1px solid #000;'>");
        _texto.Append("                            LAVAR:");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                    </tr>");
        _texto.Append("                </table>");
        _texto.Append("            </td>");
        _texto.Append("        </tr>");
        _texto.Append("        <tr>");
        _texto.Append("            <td style='border: 1px solid #FFF;'>");
        _texto.Append("                &nbsp;");
        _texto.Append("            </td>");
        _texto.Append("        </tr>");
        _texto.Append("        <tr>");
        _texto.Append("            <td style='border: 1px solid #FFF;'>");
        _texto.Append("                <table border='0' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
        _texto.Append("                    width: 100%'>");
        _texto.Append("                    <tr>");
        _texto.Append("                        <td style='width: 200px;'>");
        _texto.Append("                            <table border='0' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
        _texto.Append("                                width: 100%'>");
        _texto.Append("                                <tr>");
        _texto.Append("                                    <td colspan='2'>");
        _texto.Append("                                        RESUMO");
        _texto.Append("                                    </td>");
        _texto.Append("                                </tr>");
        _texto.Append("                                <tr>");
        _texto.Append("                                    <td colspan='2'>");
        _texto.Append("                                        &nbsp;");
        _texto.Append("                                    </td>");
        _texto.Append("                                </tr>");
        _texto.Append("                                <tr>");
        _texto.Append("                                    <td style='width: 122px; border: 1px solid #000;'>");
        _texto.Append("                                        CORTE");
        _texto.Append("                                    </td>");
        _texto.Append("                                    <td style='border: 1px solid #000;'>");
        _texto.Append("                                        &nbsp;");
        _texto.Append("                                    </td>");
        _texto.Append("                                </tr>");
        _texto.Append("                                <tr>");
        _texto.Append("                                    <td style='border: 1px solid #000;'>");
        _texto.Append("                                        1ª QUALIDADE");
        _texto.Append("                                    </td>");
        _texto.Append("                                    <td style='border: 1px solid #000;'>");
        _texto.Append("                                        &nbsp;");
        _texto.Append("                                    </td>");
        _texto.Append("                                </tr>");
        _texto.Append("                                <tr>");
        _texto.Append("                                    <td style='border: 1px solid #000;'>");
        _texto.Append("                                        DOAÇÃO/PAULA");
        _texto.Append("                                    </td>");
        _texto.Append("                                    <td style='border: 1px solid #000;'>");
        _texto.Append("                                        &nbsp;");
        _texto.Append("                                    </td>");
        _texto.Append("                                </tr>");
        _texto.Append("                                <tr>");
        _texto.Append("                                    <td style='border: 1px solid #000;'>");
        _texto.Append("                                        S. CAETANO");
        _texto.Append("                                    </td>");
        _texto.Append("                                    <td style='border: 1px solid #000;'>");
        _texto.Append("                                        &nbsp;");
        _texto.Append("                                    </td>");
        _texto.Append("                                </tr>");
        _texto.Append("                                <tr>");
        _texto.Append("                                    <td style='border: 1px solid #000;'>");
        _texto.Append("                                        DATA INÍCIO");
        _texto.Append("                                    </td>");
        _texto.Append("                                    <td style='border: 1px solid #000;'>");
        _texto.Append("                                        &nbsp;");
        _texto.Append("                                    </td>");
        _texto.Append("                                </tr>");
        _texto.Append("                                <tr>");
        _texto.Append("                                    <td style='border: 1px solid #000;'>");
        _texto.Append("                                        DATA TÉRMINO");
        _texto.Append("                                    </td>");
        _texto.Append("                                    <td style='border: 1px solid #000;'>");
        _texto.Append("                                        &nbsp;");
        _texto.Append("                                    </td>");
        _texto.Append("                                </tr>");
        _texto.Append("                                <tr>");
        _texto.Append("                                    <td style='border: 1px solid #000;'>");
        _texto.Append("                                        TOTAL DIAS");
        _texto.Append("                                    </td>");
        _texto.Append("                                    <td style='border: 1px solid #000;'>");
        _texto.Append("                                        &nbsp;");
        _texto.Append("                                    </td>");
        _texto.Append("                                </tr>");
        _texto.Append("                            </table>");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='width: 69px;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='' valign='top'>");
        _texto.Append("                            <table border='0' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
        _texto.Append("                                width: 100%'>");
        _texto.Append("                                <tr>");
        _texto.Append("                                    <td colspan='4'>");
        _texto.Append("                                        &nbsp;");
        _texto.Append("                                    </td>");
        _texto.Append("                                    <td colspan='3' rowspan='6' valign='top' style='text-align: center;'>");
        _texto.Append("                                        <img alt='Foto Peça' width='90' height='120' src='..\\.." + _relatorio.FOTO_PECA.Replace("~", "").Replace("/", "\\") + "' />");
        _texto.Append("                                    </td>");
        _texto.Append("                                </tr>");
        _texto.Append("                                <tr>");
        _texto.Append("                                    <td>");
        _texto.Append("                                        &nbsp;");
        _texto.Append("                                    </td>");
        _texto.Append("                                </tr>");
        _texto.Append("                                <tr style='border: 1px solid #000;'>");
        _texto.Append("                                    <td style='text-align: center; border-right: 1px solid #000; width: 68px;'>");
        _texto.Append("                                        S/CONF");
        _texto.Append("                                    </td>");
        _texto.Append("                                    <td style='text-align: center; border-right: 1px solid #000; width: 68px;'>");
        _texto.Append("                                        AMOSTRA");
        _texto.Append("                                    </td>");
        _texto.Append("                                    <td style='text-align: center; border-right: 1px solid #000; width: 68px;'>");
        _texto.Append("                                        DOAÇÃO");
        _texto.Append("                                    </td>");
        _texto.Append("                                    <td style='text-align: center; border-right: 1px solid #000; width: 68px;'>");
        _texto.Append("                                        SCS");
        _texto.Append("                                    </td>");
        _texto.Append("                                </tr>");
        _texto.Append("                                <tr>");
        _texto.Append("                                    <td style='text-align: center; border-right: 1px solid #000; border-left: 1px solid #000;'>");
        _texto.Append("                                        &nbsp;");
        _texto.Append("                                    </td>");
        _texto.Append("                                    <td style='text-align: center; border-right: 1px solid #000;'>");
        _texto.Append("                                        &nbsp;");
        _texto.Append("                                    </td>");
        _texto.Append("                                    <td style='text-align: center; border-right: 1px solid #000;'>");
        _texto.Append("                                        &nbsp;");
        _texto.Append("                                    </td>");
        _texto.Append("                                    <td style='text-align: center; border-right: 1px solid #000;'>");
        _texto.Append("                                        &nbsp;");
        _texto.Append("                                    </td>");
        _texto.Append("                                </tr>");
        _texto.Append("                                <tr style='border-bottom: 1px solid #000;'>");
        _texto.Append("                                    <td style='text-align: center; border-right: 1px solid #000; border-left: 1px solid #000;'>");
        _texto.Append("                                        &nbsp;");
        _texto.Append("                                    </td>");
        _texto.Append("                                    <td style='text-align: center; border-right: 1px solid #000;'>");
        _texto.Append("                                        &nbsp;");
        _texto.Append("                                    </td>");
        _texto.Append("                                    <td style='text-align: center; border-right: 1px solid #000;'>");
        _texto.Append("                                        &nbsp;");
        _texto.Append("                                    </td>");
        _texto.Append("                                    <td style='text-align: center; border-right: 1px solid #000;'>");
        _texto.Append("                                        &nbsp;");
        _texto.Append("                                    </td>");
        _texto.Append("                                </tr>");
        _texto.Append("                                <tr>");
        _texto.Append("                                    <td colspan='4'>");
        _texto.Append("                                        SCS");
        _texto.Append("                                    </td>");
        _texto.Append("                                </tr>");
        _texto.Append("                                <tr style='border: 1px solid #000;'>");
        _texto.Append("                                    <td style='text-align: center; border-right: 1px solid #000; width: 30px;'>");
        _texto.Append("                                     " + ((_gradeNome != null) ? _gradeNome.GRADE_EXP : "EXP"));
        _texto.Append("                                    </td>");
        _texto.Append("                                    <td style='text-align: center; border-right: 1px solid #000; width: 30px;'>");
        _texto.Append("                                     " + ((_gradeNome != null) ? _gradeNome.GRADE_XP : "XP"));
        _texto.Append("                                    </td>");
        _texto.Append("                                    <td style='text-align: center; border-right: 1px solid #000; width: 30px;'>");
        _texto.Append("                                     " + ((_gradeNome != null) ? _gradeNome.GRADE_PP : "PP"));
        _texto.Append("                                    </td>");
        _texto.Append("                                    <td style='text-align: center; border-right: 1px solid #000; width: 30px;'>");
        _texto.Append("                                     " + ((_gradeNome != null) ? _gradeNome.GRADE_P : "P"));
        _texto.Append("                                    </td>");
        _texto.Append("                                    <td style='text-align: center; border-right: 1px solid #000; width: 50px;'>");
        _texto.Append("                                     " + ((_gradeNome != null) ? _gradeNome.GRADE_M : "M"));
        _texto.Append("                                    </td>");
        _texto.Append("                                    <td style='text-align: center; border-right: 1px solid #000; width: 50px;'>");
        _texto.Append("                                     " + ((_gradeNome != null) ? _gradeNome.GRADE_G : "G"));
        _texto.Append("                                    </td>");
        _texto.Append("                                    <td style='text-align: center; width: 50px;'>");
        _texto.Append("                                     " + ((_gradeNome != null) ? _gradeNome.GRADE_GG : "GG"));
        _texto.Append("                                    </td>");
        _texto.Append("                                </tr>");
        _texto.Append("                                <tr>");
        _texto.Append("                                    <td style='text-align: center; border-right: 1px solid #000; border-left: 1px solid #000;'>");
        _texto.Append("                                        &nbsp;");
        _texto.Append("                                    </td>");
        _texto.Append("                                    <td style='text-align: center; border-right: 1px solid #000;'>");
        _texto.Append("                                        &nbsp;");
        _texto.Append("                                    </td>");
        _texto.Append("                                    <td style='text-align: center; border-right: 1px solid #000;'>");
        _texto.Append("                                        &nbsp;");
        _texto.Append("                                    </td>");
        _texto.Append("                                    <td style='text-align: center; border-right: 1px solid #000;'>");
        _texto.Append("                                        &nbsp;");
        _texto.Append("                                    </td>");
        _texto.Append("                                    <td style='text-align: center; border-right: 1px solid #000;'>");
        _texto.Append("                                        &nbsp;");
        _texto.Append("                                    </td>");
        _texto.Append("                                    <td style='text-align: center; border-right: 1px solid #000;'>");
        _texto.Append("                                        &nbsp;");
        _texto.Append("                                    </td>");
        _texto.Append("                                    <td style='border-right: 1px solid #000;'>");
        _texto.Append("                                        &nbsp;");
        _texto.Append("                                    </td>");
        _texto.Append("                                </tr>");
        _texto.Append("                                <tr style='border-bottom: 1px solid #000;'>");
        _texto.Append("                                    <td style='text-align: center; border-right: 1px solid #000; border-left: 1px solid #000;'>");
        _texto.Append("                                        &nbsp;");
        _texto.Append("                                    </td>");
        _texto.Append("                                    <td style='text-align: center; border-right: 1px solid #000;'>");
        _texto.Append("                                        &nbsp;");
        _texto.Append("                                    </td>");
        _texto.Append("                                    <td style='text-align: center; border-right: 1px solid #000;'>");
        _texto.Append("                                        &nbsp;");
        _texto.Append("                                    </td>");
        _texto.Append("                                    <td style='text-align: center; border-right: 1px solid #000;'>");
        _texto.Append("                                        &nbsp;");
        _texto.Append("                                    </td>");
        _texto.Append("                                    <td style='text-align: center; border-right: 1px solid #000;'>");
        _texto.Append("                                        &nbsp;");
        _texto.Append("                                    </td>");
        _texto.Append("                                    <td style='text-align: center; border-right: 1px solid #000;'>");
        _texto.Append("                                        &nbsp;");
        _texto.Append("                                    </td>");
        _texto.Append("                                    <td style='border-right: 1px solid #000;'>");
        _texto.Append("                                        &nbsp;");
        _texto.Append("                                    </td>");
        _texto.Append("                                </tr>");
        _texto.Append("                            </table>");
        _texto.Append("                        </td>");
        _texto.Append("                    </tr>");
        _texto.Append("                </table>");
        _texto.Append("            </td>");
        _texto.Append("        </tr>");
        _texto.Append("        <tr>");
        _texto.Append("            <td style='border: 1px solid #FFF;'>");
        _texto.Append("                &nbsp;");
        _texto.Append("            </td>");
        _texto.Append("        </tr>");
        _texto.Append("        <tr>");
        _texto.Append("            <td>");
        _texto.Append("                <table border='0' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
        _texto.Append("                    width: 517pt'>");
        _texto.Append("                    <tr style='text-align: center; border-top: 1px solid #000;'>");
        _texto.Append("                        <td style='width: 161px; border-right: 1px solid #000; border-left: 1px solid #000;'>");
        _texto.Append("                            <span style='font-size: 11px;'>ESTOQUE VAREJO</span>");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='width: 55px; border-right: 1px solid #000;'>");
        _texto.Append("                         " + ((_gradeNome != null) ? _gradeNome.GRADE_EXP : "EXP"));
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='width: 55px; border-right: 1px solid #000;'>");
        _texto.Append("                         " + ((_gradeNome != null) ? _gradeNome.GRADE_XP : "XP"));
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='width: 55px; border-right: 1px solid #000;'>");
        _texto.Append("                         " + ((_gradeNome != null) ? _gradeNome.GRADE_PP : "PP"));
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='width: 55px; border-right: 1px solid #000;'>");
        _texto.Append("                         " + ((_gradeNome != null) ? _gradeNome.GRADE_P : "P"));
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='width: 55px; border-right: 1px solid #000;'>");
        _texto.Append("                         " + ((_gradeNome != null) ? _gradeNome.GRADE_M : "M"));
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='width: 55px; border-right: 1px solid #000;'>");
        _texto.Append("                         " + ((_gradeNome != null) ? _gradeNome.GRADE_G : "G"));
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='width: 55px; border-right: 1px solid #000;'>");
        _texto.Append("                         " + ((_gradeNome != null) ? _gradeNome.GRADE_GG : "GG"));
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='width: 55px; border-right: 1px solid #000;'>");
        _texto.Append("                            TOTAL");
        _texto.Append("                        </td>");
        _texto.Append("                    </tr>");
        _texto.Append("                    <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                    </tr>");
        _texto.Append("                    <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                    </tr>");
        _texto.Append("                    <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                    </tr>");
        _texto.Append("                    <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                    </tr>");
        _texto.Append("                    <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                    </tr>");
        _texto.Append("                    <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                    </tr>");
        _texto.Append("                    <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            TOTAL FINAL");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                    </tr>");
        _texto.Append("                </table>");
        _texto.Append("            </td>");
        _texto.Append("        </tr>");
        _texto.Append("        <tr style='border: 1px solid #FFF;'>");
        _texto.Append("            <td style='border: 1px solid #FFF;'>");
        _texto.Append("                &nbsp;");
        _texto.Append("            </td>");
        _texto.Append("        </tr>");
        _texto.Append("        <tr>");
        _texto.Append("            <td>");
        _texto.Append("                <table border='0' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
        _texto.Append("                    width: 517pt'>");
        _texto.Append("                    <tr style='text-align: center; border-top: 1px solid #000;'>");
        _texto.Append("                        <td style='width: 161px; border-right: 1px solid #000; border-left: 1px solid #000;'>");
        _texto.Append("                            <span style='font-size: 11px;'>ESTOQUE ATACADO</span>");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='width: 55px; border-right: 1px solid #000;'>");
        _texto.Append("                         " + ((_gradeNome != null) ? _gradeNome.GRADE_EXP : "EXP"));
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='width: 55px; border-right: 1px solid #000;'>");
        _texto.Append("                         " + ((_gradeNome != null) ? _gradeNome.GRADE_XP : "XP"));
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='width: 55px; border-right: 1px solid #000;'>");
        _texto.Append("                         " + ((_gradeNome != null) ? _gradeNome.GRADE_PP : "PP"));
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='width: 55px; border-right: 1px solid #000;'>");
        _texto.Append("                         " + ((_gradeNome != null) ? _gradeNome.GRADE_P : "P"));
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='width: 55px; border-right: 1px solid #000;'>");
        _texto.Append("                         " + ((_gradeNome != null) ? _gradeNome.GRADE_M : "M"));
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='width: 55px; border-right: 1px solid #000;'>");
        _texto.Append("                         " + ((_gradeNome != null) ? _gradeNome.GRADE_G : "G"));
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='width: 55px; border-right: 1px solid #000;'>");
        _texto.Append("                         " + ((_gradeNome != null) ? _gradeNome.GRADE_GG : "GG"));
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='width: 55px; border-right: 1px solid #000;'>");
        _texto.Append("                            TOTAL");
        _texto.Append("                        </td>");
        _texto.Append("                    </tr>");
        _texto.Append("                    <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                    </tr>");
        _texto.Append("                    <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                    </tr>");
        _texto.Append("                    <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                    </tr>");
        _texto.Append("                    <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            TOTAL FINAL");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                    </tr>");
        _texto.Append("                </table>");
        _texto.Append("            </td>");
        _texto.Append("        </tr>");
        _texto.Append("        <tr>");
        _texto.Append("            <td>");
        _texto.Append("                &nbsp;");
        _texto.Append("            </td>");
        _texto.Append("        </tr>");
        _texto.Append("        <tr>");
        _texto.Append("            <td style='line-height: 13px;'>");
        _texto.Append("                <table border='0' cellpadding='0' cellspacing='0' width='689' style='border-collapse: collapse;");
        _texto.Append("                    width: 517pt'>");
        _texto.Append("                    <tr style='border-top: 1px solid #000;'>");
        _texto.Append("                        <td style='width: 100%; border-left: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.Append("                            OBSERVAÇÕES:");
        _texto.Append("                        </td>");
        _texto.Append("                    </tr>");
        _texto.Append("                    <tr style='border: 1px solid #000; border-top: 1px solid #FFF; line-height: 25px;'>");
        _texto.Append("                        <td style='text-align: center;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                    </tr>");
        _texto.Append("                </table>");
        _texto.Append("            </td>");
        _texto.Append("        </tr>");
        _texto.Append("        <tr>");
        _texto.Append("            <td>");
        _texto.Append("                 <div>");
        _texto.Append("                     <hr style='border: 1px dashed #000;' />");
        _texto.Append("                 </div>");
        _texto.Append("            </td>");
        _texto.Append("        </tr>");
        _texto.Append("         <tr>");
        _texto.Append("             <td style='line-height: 20px;'>");
        _texto.Append("                 <table border='0' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
        _texto.Append("                 width: 517pt'>");
        _texto.Append("                     <tr style='text-align: left; border-top: 1px solid #000; border-bottom: none;'>");
        _texto.Append("                         <td style='width: 140px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.Append("                             &nbsp;COL: " + _relatorio.COLECAO);
        _texto.Append("                         </td>");
        _texto.Append("                         <td style='border-right: 1px solid #000;'>");
        _texto.Append("                             &nbsp;HB: " + _relatorio.HB);
        _texto.Append("                         </td>");
        _texto.Append("                         <td style='border-right: 1px solid #000;'>");
        _texto.Append("                             &nbsp;PRODUTO: " + _relatorio.CODIGO_PRODUTO_LINX + " - " + _relatorio.NOME);
        _texto.Append("                         </td>");
        _texto.Append("                         <td style='border-right: 1px solid #000; border-bottom: 1px dotted #FFF;' valign='top'>");
        _texto.Append("                             &nbsp;COR: " + _relatorio.COD_COR + " - " + _relatorio.DESC_COR);
        _texto.Append("                         </td>");
        _texto.Append("                     </tr>");
        _texto.Append("                 </table>");
        _texto.Append("             </td>");
        _texto.Append("         </tr>");
        _texto.Append("         <tr>");
        _texto.Append("             <td>");
        _texto.Append("                 <table border='0' cellpadding='0' cellspacing='0' width='689' style='border-collapse: collapse;");
        _texto.Append("                 width: 517pt; " + ((DATA_IMP_FIC_LOGISTICA == "") ? "" : "background:#cac7c9;") + "'>");
        _texto.Append("                     <tr style='text-align: center; border-top: 1px solid #000;'>");
        _texto.Append("                         <td style='width: 100px; text-align: left; border-right: 1px solid #000; border-left: 1px solid #000;'>");
        _texto.Append("                             &nbsp;DATA:");
        _texto.Append("                         </td>");
        _texto.Append("                         <td style='width: 55px; border-right: 1px solid #000;'>");
        _texto.Append("                             " + ((_relatorio.PROCESSO == null) ? "&nbsp;" : Convert.ToDateTime(_relatorio.PROCESSO.DATA_FIM).ToString("dd/MM/yyyy")));
        _texto.Append("                         </td>");
        _texto.Append("                         <td style='width: 55px; border-right: 1px solid #000;'>");
        _texto.Append("                             " + ((_gradeNome != null) ? _gradeNome.GRADE_EXP : "EXP"));
        _texto.Append("                         </td>");
        _texto.Append("                         <td style='width: 55px; border-right: 1px solid #000;'>");
        _texto.Append("                             " + ((_gradeNome != null) ? _gradeNome.GRADE_XP : "XP"));
        _texto.Append("                         </td>");
        _texto.Append("                         <td style='width: 55px; border-right: 1px solid #000;'>");
        _texto.Append("                             " + ((_gradeNome != null) ? _gradeNome.GRADE_PP : "PP"));
        _texto.Append("                         </td>");
        _texto.Append("                         <td style='width: 55px; border-right: 1px solid #000;'>");
        _texto.Append("                             " + ((_gradeNome != null) ? _gradeNome.GRADE_P : "P"));
        _texto.Append("                         </td>");
        _texto.Append("                         <td style='width: 55px; border-right: 1px solid #000;'>");
        _texto.Append("                             " + ((_gradeNome != null) ? _gradeNome.GRADE_M : "M"));
        _texto.Append("                         </td>");
        _texto.Append("                         <td style='width: 55px; border-right: 1px solid #000;'>");
        _texto.Append("                             " + ((_gradeNome != null) ? _gradeNome.GRADE_G : "G"));
        _texto.Append("                         </td>");
        _texto.Append("                         <td style='width: 55px; border-right: 1px solid #000;'>");
        _texto.Append("                             " + ((_gradeNome != null) ? _gradeNome.GRADE_GG : "GG"));
        _texto.Append("                         </td>");
        _texto.Append("                         <td style='width: 55px; border-right: 1px solid #000;'>");
        _texto.Append("                             TOTAL");
        _texto.Append("                         </td>");
        _texto.Append("                     </tr>");
        _texto.Append("                    <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
        _texto.Append("                        <td style='text-align: left; border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;VAREJO:");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + (_relatorio.GRADE.GRADE_EXP - ((_relatorio.GRADEATACADO == null) ? 0 : _relatorio.GRADEATACADO.GRADE_EXP)));
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + (_relatorio.GRADE.GRADE_XP - ((_relatorio.GRADEATACADO == null) ? 0 : _relatorio.GRADEATACADO.GRADE_XP)));
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + (_relatorio.GRADE.GRADE_PP - ((_relatorio.GRADEATACADO == null) ? 0 : _relatorio.GRADEATACADO.GRADE_PP)));
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + (_relatorio.GRADE.GRADE_P - ((_relatorio.GRADEATACADO == null) ? 0 : _relatorio.GRADEATACADO.GRADE_P)));
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + (_relatorio.GRADE.GRADE_M - ((_relatorio.GRADEATACADO == null) ? 0 : _relatorio.GRADEATACADO.GRADE_M)));
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + (_relatorio.GRADE.GRADE_G - ((_relatorio.GRADEATACADO == null) ? 0 : _relatorio.GRADEATACADO.GRADE_G)));
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + (_relatorio.GRADE.GRADE_GG - ((_relatorio.GRADEATACADO == null) ? 0 : _relatorio.GRADEATACADO.GRADE_GG)));
        _texto.Append("                        </td>");
        _texto.Append("                        <td>");
        _texto.Append("                            " + (
                                                               Convert.ToInt32(_relatorio.GRADE.GRADE_EXP - ((_relatorio.GRADEATACADO == null) ? 0 : _relatorio.GRADEATACADO.GRADE_EXP)) +
                                                               Convert.ToInt32(_relatorio.GRADE.GRADE_XP - ((_relatorio.GRADEATACADO == null) ? 0 : _relatorio.GRADEATACADO.GRADE_XP)) +
                                                               Convert.ToInt32(_relatorio.GRADE.GRADE_PP - ((_relatorio.GRADEATACADO == null) ? 0 : _relatorio.GRADEATACADO.GRADE_PP)) +
                                                               Convert.ToInt32(_relatorio.GRADE.GRADE_P - ((_relatorio.GRADEATACADO == null) ? 0 : _relatorio.GRADEATACADO.GRADE_P)) +
                                                               Convert.ToInt32(_relatorio.GRADE.GRADE_M - ((_relatorio.GRADEATACADO == null) ? 0 : _relatorio.GRADEATACADO.GRADE_M)) +
                                                               Convert.ToInt32(_relatorio.GRADE.GRADE_G - ((_relatorio.GRADEATACADO == null) ? 0 : _relatorio.GRADEATACADO.GRADE_G)) +
                                                               Convert.ToInt32(_relatorio.GRADE.GRADE_GG - ((_relatorio.GRADEATACADO == null) ? 0 : _relatorio.GRADEATACADO.GRADE_GG))
                                                        ));
        _texto.Append("                        </td>");
        _texto.Append("                    </tr>");
        _texto.Append("                    <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
        _texto.Append("                        <td style='text-align: left; border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;ATACADO:");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + ((_relatorio.GRADEATACADO == null) ? "0" : _relatorio.GRADEATACADO.GRADE_EXP.ToString()));
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + ((_relatorio.GRADEATACADO == null) ? "0" : _relatorio.GRADEATACADO.GRADE_XP.ToString()));
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + ((_relatorio.GRADEATACADO == null) ? "0" : _relatorio.GRADEATACADO.GRADE_PP.ToString()));
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + ((_relatorio.GRADEATACADO == null) ? "0" : _relatorio.GRADEATACADO.GRADE_P.ToString()));
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + ((_relatorio.GRADEATACADO == null) ? "0" : _relatorio.GRADEATACADO.GRADE_M.ToString()));
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + ((_relatorio.GRADEATACADO == null) ? "0" : _relatorio.GRADEATACADO.GRADE_G.ToString()));
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + ((_relatorio.GRADEATACADO == null) ? "0" : _relatorio.GRADEATACADO.GRADE_GG.ToString()));
        _texto.Append("                        </td>");
        _texto.Append("                        <td>");
        _texto.Append("                            " + (
                                                            (_relatorio.GRADEATACADO == null) ? "0" :
                                                            (Convert.ToInt32(_relatorio.GRADEATACADO.GRADE_EXP) +
                                                            Convert.ToInt32(_relatorio.GRADEATACADO.GRADE_XP) +
                                                            Convert.ToInt32(_relatorio.GRADEATACADO.GRADE_PP) +
                                                            Convert.ToInt32(_relatorio.GRADEATACADO.GRADE_P) +
                                                            Convert.ToInt32(_relatorio.GRADEATACADO.GRADE_M) +
                                                            Convert.ToInt32(_relatorio.GRADEATACADO.GRADE_G) +
                                                            Convert.ToInt32(_relatorio.GRADEATACADO.GRADE_GG)).ToString()
                                                        ));
        _texto.Append("                        </td>");
        _texto.Append("                    </tr>");

        _texto.Append("                    <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
        _texto.Append("                        <td style='text-align: left; border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;TOTAL:");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            &nbsp;");
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + _relatorio.GRADE.GRADE_EXP);
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + _relatorio.GRADE.GRADE_XP);
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + _relatorio.GRADE.GRADE_PP);
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + _relatorio.GRADE.GRADE_P);
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + _relatorio.GRADE.GRADE_M);
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + _relatorio.GRADE.GRADE_G);
        _texto.Append("                        </td>");
        _texto.Append("                        <td style='border-right: 1px solid #000;'>");
        _texto.Append("                            " + _relatorio.GRADE.GRADE_GG);
        _texto.Append("                        </td>");
        _texto.Append("                        <td>");
        _texto.Append("                            " + (
                                                               Convert.ToInt32(_relatorio.GRADE.GRADE_EXP +
                                                               _relatorio.GRADE.GRADE_XP +
                                                               _relatorio.GRADE.GRADE_PP +
                                                               _relatorio.GRADE.GRADE_P +
                                                               _relatorio.GRADE.GRADE_M +
                                                               _relatorio.GRADE.GRADE_G +
                                                               _relatorio.GRADE.GRADE_GG)
                                                        ));
        _texto.Append("                        </td>");
        _texto.Append("                    </tr>");
        _texto.Append("                </table>");
        _texto.Append("            </td>");
        _texto.Append("        </tr>");
        _texto.Append("    </table>");
        _texto.Append("</div>");

        return _texto;
    }
    public StringBuilder MontarLogisticaV2(StringBuilder _texto, RelatorioHB _relatorio, string breakClass, string diretorioPadrao)
    {
        int contLinha = 0;
        int tLinhaVarejo = 6;
        int tLinhaAtacado = 5;
        int tLinhaControleQ = 9;
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();

        bool etiquetaCodigoBarraOK = false;

        var prodHB = prodController.ObterHB(_relatorio.CODIGO);
        if (prodHB != null)
        {
            if (prodHB.DATA_BAIXA_ETIQUETA != null)
                etiquetaCodigoBarraOK = true;
        }

        var _gradeNome = new ProducaoController().ObterGradeNome(Convert.ToInt32(_relatorio.PROD_GRADE));

        //criar codigo de barras
        string nomeCodigoBarra = "";
        if (_relatorio.MOSTRUARIO == "S")
        {
            try
            {
                nomeCodigoBarra = baseController.ObterCodigoBarra(_relatorio.CODIGO_PRODUTO_LINX, _relatorio.COD_COR, _relatorio.GRADE);
                if (nomeCodigoBarra != null && nomeCodigoBarra != "")
                    Util.GerarCodigoBarras(diretorioPadrao, nomeCodigoBarra);
            }
            catch (Exception ex)
            {
            }
        }

        _texto.AppendLine(" <br />");
        _texto.AppendLine("    <span>LOGÍSTICA " + ((DATA_IMP_FIC_LOGISTICA == "") ? "" : " - REIMPRESSÃO") + "</span>");
        if (_relatorio.DETALHE.Count <= 0)
            _texto.AppendLine("<div id='divLog' align='center'>");
        else
            _texto.AppendLine("<div id='divLog' align='center' class='" + breakClass + "'>");
        _texto.AppendLine("        <table border='0' cellpadding='0' cellspacing='0' style='width: 517pt; padding: 0px;");
        _texto.AppendLine("            color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
        _texto.AppendLine("            background: white; white-space: nowrap;'>");
        _texto.AppendLine("            <tr>");
        _texto.AppendLine("                <td style='line-height: 20px;'>");
        _texto.AppendLine("                    <table border='0' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
        _texto.AppendLine("                        width: 517pt'>");
        _texto.AppendLine("                        <tr style='text-align: left; border-top: 2px solid #000; border-bottom: none;'>");
        _texto.AppendLine("                            <td rowspan='4' style='border: 1px solid #000;'>");
        _texto.AppendLine("                                <img alt='Foto Peça' width='90' height='120' src='..\\.." + _relatorio.FOTO_PECA.Replace("~", "").Replace("/", "\\") + "' />");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 184px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;HB: " + _relatorio.HB.ToString() + ((_relatorio.SIGNED == "S") ? (" - " + _relatorio.SIGNED_NOME) : ""));
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 251px; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;COLEÇÃO: " + _relatorio.COLECAO);
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td colspan='2' style='border-right: 1px solid #000; border-bottom: 1px dotted #FFF;'>");
        _texto.AppendLine("                                &nbsp;MOSTRUÁRIO: " + ((_relatorio.MOSTRUARIO == "S") ? "SIM" : "NÃO"));
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                        </tr>");
        _texto.AppendLine("                        <tr style='border: 1px solid #000;'>");
        _texto.AppendLine("                            <td style='border-left: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;QTDE: " + (Convert.ToInt32(_relatorio.GRADE.GRADE_EXP) + Convert.ToInt32(_relatorio.GRADE.GRADE_XP) + Convert.ToInt32(_relatorio.GRADE.GRADE_PP) + Convert.ToInt32(_relatorio.GRADE.GRADE_P) + Convert.ToInt32(_relatorio.GRADE.GRADE_M) + Convert.ToInt32(_relatorio.GRADE.GRADE_G) + Convert.ToInt32(_relatorio.GRADE.GRADE_GG)));
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;NOME: " + _relatorio.GRUPO + " " + _relatorio.NOME.Replace(_relatorio.GRUPO, ""));
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;OP: " + _relatorio.ORDEM_PRODUCAO);
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='border-right: 1px solid #000; border-bottom: 1px dotted #FFF;'>");
        _texto.AppendLine("                                 &nbsp;ETI. CÓD. BARRAS: " + ((etiquetaCodigoBarraOK) ? "OK" : "-"));
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                        </tr>");
        _texto.AppendLine("                        <tr style='border: 1px solid #000;'>");
        _texto.AppendLine("                            <td style='border-left: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;PRODUTO: " + _relatorio.CODIGO_PRODUTO_LINX);
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;COR: " + _relatorio.COD_COR + " - " + _relatorio.DESC_COR);
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td colspan='2' style='border-right: 1px solid #000; border-bottom: 1px dotted #FFF;'>");
        _texto.AppendLine("                                &nbsp;GRIFFE: " + _relatorio.GRIFFE);
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                        </tr>");
        _texto.AppendLine("                        <tr>");
        _texto.AppendLine("                            <td colspan='4'>");
        _texto.AppendLine("                                <table border='0' cellpadding='0' cellspacing='0' width='689' style='border-collapse: collapse; width: 517pt;'>");
        _texto.AppendLine("                                    <tr style='text-align: center; border-top: 1px solid #000;'>");
        _texto.AppendLine("                                        <td style=' text-align: left; border-right: 1px solid #000; border-left: 1px solid #000;'>");
        _texto.AppendLine("                                            &nbsp;DATA: " + ((_relatorio.PROCESSO == null) ? "&nbsp;" : Convert.ToDateTime(_relatorio.PROCESSO.DATA_FIM).ToString("dd/MM/yyyy")));
        _texto.AppendLine("                                        </td>");
        _texto.AppendLine("                                        <td style='width: 62px; border-right: 1px solid #000; '>");
        _texto.AppendLine("                                            " + ((_gradeNome != null) ? _gradeNome.GRADE_EXP : "EXP"));
        _texto.AppendLine("                                        </td>");
        _texto.AppendLine("                                        <td style='width: 62px; border-right: 1px solid #000; '>");
        _texto.AppendLine("                                            " + ((_gradeNome != null) ? _gradeNome.GRADE_XP : "XP"));
        _texto.AppendLine("                                        </td>");
        _texto.AppendLine("                                        <td style='width: 62px; border-right: 1px solid #000; '>");
        _texto.AppendLine("                                            " + ((_gradeNome != null) ? _gradeNome.GRADE_PP : "PP"));
        _texto.AppendLine("                                        </td>");
        _texto.AppendLine("                                        <td style='width: 62px; border-right: 1px solid #000; '>");
        _texto.AppendLine("                                            " + ((_gradeNome != null) ? _gradeNome.GRADE_P : "P"));
        _texto.AppendLine("                                        </td>");
        _texto.AppendLine("                                        <td style='width: 62px; border-right: 1px solid #000; '>");
        _texto.AppendLine("                                            " + ((_gradeNome != null) ? _gradeNome.GRADE_M : "M"));
        _texto.AppendLine("                                        </td>");
        _texto.AppendLine("                                        <td style='width: 62px; border-right: 1px solid #000; '>");
        _texto.AppendLine("                                            " + ((_gradeNome != null) ? _gradeNome.GRADE_G : "G"));
        _texto.AppendLine("                                        </td>");
        _texto.AppendLine("                                        <td style='width: 62px; border-right: 1px solid #000; '>");
        _texto.AppendLine("                                            " + ((_gradeNome != null) ? _gradeNome.GRADE_GG : "GG"));
        _texto.AppendLine("                                        </td>");
        _texto.AppendLine("                                        <td style='width: 62px; border-right: 1px solid #000; '>");
        _texto.AppendLine("                                            TOTAL");
        _texto.AppendLine("                                        </td>");
        _texto.AppendLine("                                    </tr>");
        _texto.AppendLine("                                    <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
        _texto.AppendLine("                                        <td style='text-align: left; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                            &nbsp;VAREJO");
        _texto.AppendLine("                                        </td>");
        _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                                            " + (_relatorio.GRADE.GRADE_EXP - ((_relatorio.GRADEATACADO == null) ? 0 : _relatorio.GRADEATACADO.GRADE_EXP)));
        _texto.AppendLine("                                        </td>");
        _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                                            " + (_relatorio.GRADE.GRADE_XP - ((_relatorio.GRADEATACADO == null) ? 0 : _relatorio.GRADEATACADO.GRADE_XP)));
        _texto.AppendLine("                                        </td>");
        _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                                            " + (_relatorio.GRADE.GRADE_PP - ((_relatorio.GRADEATACADO == null) ? 0 : _relatorio.GRADEATACADO.GRADE_PP)));
        _texto.AppendLine("                                        </td>");
        _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                                            " + (_relatorio.GRADE.GRADE_P - ((_relatorio.GRADEATACADO == null) ? 0 : _relatorio.GRADEATACADO.GRADE_P)));
        _texto.AppendLine("                                        </td>");
        _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                                            " + (_relatorio.GRADE.GRADE_M - ((_relatorio.GRADEATACADO == null) ? 0 : _relatorio.GRADEATACADO.GRADE_M)));
        _texto.AppendLine("                                        </td>");
        _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                                            " + (_relatorio.GRADE.GRADE_G - ((_relatorio.GRADEATACADO == null) ? 0 : _relatorio.GRADEATACADO.GRADE_G)));
        _texto.AppendLine("                                        </td>");
        _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                                            " + (_relatorio.GRADE.GRADE_GG - ((_relatorio.GRADEATACADO == null) ? 0 : _relatorio.GRADEATACADO.GRADE_GG)));
        _texto.AppendLine("                                        </td>");
        _texto.AppendLine("                                        <td>");
        _texto.AppendLine("                                            " + (
                                                                               Convert.ToInt32(_relatorio.GRADE.GRADE_EXP - ((_relatorio.GRADEATACADO == null) ? 0 : _relatorio.GRADEATACADO.GRADE_EXP)) +
                                                                               Convert.ToInt32(_relatorio.GRADE.GRADE_XP - ((_relatorio.GRADEATACADO == null) ? 0 : _relatorio.GRADEATACADO.GRADE_XP)) +
                                                                               Convert.ToInt32(_relatorio.GRADE.GRADE_PP - ((_relatorio.GRADEATACADO == null) ? 0 : _relatorio.GRADEATACADO.GRADE_PP)) +
                                                                               Convert.ToInt32(_relatorio.GRADE.GRADE_P - ((_relatorio.GRADEATACADO == null) ? 0 : _relatorio.GRADEATACADO.GRADE_P)) +
                                                                               Convert.ToInt32(_relatorio.GRADE.GRADE_M - ((_relatorio.GRADEATACADO == null) ? 0 : _relatorio.GRADEATACADO.GRADE_M)) +
                                                                               Convert.ToInt32(_relatorio.GRADE.GRADE_G - ((_relatorio.GRADEATACADO == null) ? 0 : _relatorio.GRADEATACADO.GRADE_G)) +
                                                                               Convert.ToInt32(_relatorio.GRADE.GRADE_GG - ((_relatorio.GRADEATACADO == null) ? 0 : _relatorio.GRADEATACADO.GRADE_GG))
                                                                        ));
        _texto.AppendLine("                                        </td>");
        _texto.AppendLine("                                    </tr>");
        _texto.AppendLine("                                    <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
        _texto.AppendLine("                                        <td style='text-align: left; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                            &nbsp;ATACADO");
        _texto.AppendLine("                                        </td>");
        _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                                            " + ((_relatorio.GRADEATACADO == null) ? "0" : _relatorio.GRADEATACADO.GRADE_EXP.ToString()));
        _texto.AppendLine("                                        </td>");
        _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                                            " + ((_relatorio.GRADEATACADO == null) ? "0" : _relatorio.GRADEATACADO.GRADE_XP.ToString()));
        _texto.AppendLine("                                        </td>");
        _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                                            " + ((_relatorio.GRADEATACADO == null) ? "0" : _relatorio.GRADEATACADO.GRADE_PP.ToString()));
        _texto.AppendLine("                                        </td>");
        _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                                            " + ((_relatorio.GRADEATACADO == null) ? "0" : _relatorio.GRADEATACADO.GRADE_P.ToString()));
        _texto.AppendLine("                                        </td>");
        _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                                            " + ((_relatorio.GRADEATACADO == null) ? "0" : _relatorio.GRADEATACADO.GRADE_M.ToString()));
        _texto.AppendLine("                                        </td>");
        _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                                            " + ((_relatorio.GRADEATACADO == null) ? "0" : _relatorio.GRADEATACADO.GRADE_G.ToString()));
        _texto.AppendLine("                                        </td>");
        _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                                            " + ((_relatorio.GRADEATACADO == null) ? "0" : _relatorio.GRADEATACADO.GRADE_GG.ToString()));
        _texto.AppendLine("                                        </td>");
        _texto.AppendLine("                                        <td>");
        _texto.AppendLine("                                             " + (
                                                                             (_relatorio.GRADEATACADO == null) ? "0" :
                                                                             (Convert.ToInt32(_relatorio.GRADEATACADO.GRADE_EXP) +
                                                                             Convert.ToInt32(_relatorio.GRADEATACADO.GRADE_XP) +
                                                                             Convert.ToInt32(_relatorio.GRADEATACADO.GRADE_PP) +
                                                                             Convert.ToInt32(_relatorio.GRADEATACADO.GRADE_P) +
                                                                             Convert.ToInt32(_relatorio.GRADEATACADO.GRADE_M) +
                                                                             Convert.ToInt32(_relatorio.GRADEATACADO.GRADE_G) +
                                                                             Convert.ToInt32(_relatorio.GRADEATACADO.GRADE_GG)).ToString()
                                                                         ));
        _texto.AppendLine("                                        </td>");
        _texto.AppendLine("                                    </tr>");
        _texto.AppendLine("                                    <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
        _texto.AppendLine("                                        <td style='text-align: left; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                            &nbsp;TOTAL");
        _texto.AppendLine("                                        </td>");
        _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                                            " + _relatorio.GRADE.GRADE_EXP);
        _texto.AppendLine("                                        </td>");
        _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                                            " + _relatorio.GRADE.GRADE_XP);
        _texto.AppendLine("                                        </td>");
        _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                                            " + _relatorio.GRADE.GRADE_PP);
        _texto.AppendLine("                                        </td>");
        _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                                            " + _relatorio.GRADE.GRADE_P);
        _texto.AppendLine("                                        </td>");
        _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                                            " + _relatorio.GRADE.GRADE_M);
        _texto.AppendLine("                                        </td>");
        _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                                            " + _relatorio.GRADE.GRADE_G);
        _texto.AppendLine("                                        </td>");
        _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                                            " + _relatorio.GRADE.GRADE_GG);
        _texto.AppendLine("                                        </td>");
        _texto.AppendLine("                                        <td>");
        _texto.AppendLine("                                             " + (
                                                                                Convert.ToInt32(_relatorio.GRADE.GRADE_EXP +
                                                                                _relatorio.GRADE.GRADE_XP +
                                                                                _relatorio.GRADE.GRADE_PP +
                                                                                _relatorio.GRADE.GRADE_P +
                                                                                _relatorio.GRADE.GRADE_M +
                                                                                _relatorio.GRADE.GRADE_G +
                                                                                _relatorio.GRADE.GRADE_GG)
                                                                         ));
        _texto.AppendLine("                                        </td>");
        _texto.AppendLine("                                    </tr>");
        _texto.AppendLine("                                </table>");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                        </tr>");
        _texto.AppendLine("                    </table>");
        _texto.AppendLine("                </td>");
        _texto.AppendLine("            </tr>");
        _texto.AppendLine("            <tr>");
        _texto.AppendLine("                <td style='border: 1px solid #FFF;'>");
        _texto.AppendLine("                    &nbsp;");
        _texto.AppendLine("                </td>");
        _texto.AppendLine("            </tr>");

        //COMPOSICAO
        _texto.AppendLine("            <tr>");
        _texto.AppendLine("                <td style='line-height: 16px;'>");
        _texto.AppendLine("                    <table border='0' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
        _texto.AppendLine("                        width: 100%;'>");
        _texto.AppendLine("                        <tr style=''>");
        _texto.AppendLine("                           <td>");
        _texto.AppendLine("                              <table border='0' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
        _texto.AppendLine("                                  width: 100%;'>");
        _texto.AppendLine("                                  <tr style='border-top: 1px solid #000;'>");
        _texto.AppendLine("                                      <td colspan='2' style='border-left: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                          COMPOSIÇÃO");
        _texto.AppendLine("                                      </td>");
        _texto.AppendLine("                                  </tr>");
        foreach (SP_OBTER_COMPOSICAO_HBResult c in new ProducaoController().ObterListaComposicaoHB(_relatorio.CODIGO))
        {
            _texto.AppendLine("                             <tr style='border: 1px solid #000'>");
            _texto.AppendLine("                                 <td style='text-align: left; width: 200px; '>");
            _texto.AppendLine("                                     " + c.DESCRICAO);
            _texto.AppendLine("                                 </td>");
            _texto.AppendLine("                                 <td style='text-align: left;'>");
            _texto.AppendLine("                                 " + c.COMPOSICAO);
            _texto.AppendLine("                                 </td>");
            _texto.AppendLine("                             </tr>");
        }
        _texto.AppendLine("                             </table>");

        _texto.AppendLine("                          </td>");
        if (_relatorio.MOSTRUARIO == "S")
        {
            _texto.AppendLine("                          <td style='text-align:right;'>");
            _texto.AppendLine("                             <img alt='Barras' width='310' height='100' src='..\\.." + ("~/Image_BARCODE/" + nomeCodigoBarra + ".png").Replace("~", "").Replace("/", "\\") + "' />");
            _texto.AppendLine("                          </td>");
        }
        _texto.AppendLine("                        </tr>");
        _texto.AppendLine("                    </table>");
        _texto.AppendLine("                </td>");
        _texto.AppendLine("            </tr>");
        _texto.AppendLine("            <tr>");
        _texto.AppendLine("                <td style='border: 1px solid #FFF;'>");
        _texto.AppendLine("                    &nbsp;");
        _texto.AppendLine("                </td>");
        _texto.AppendLine("            </tr>");
        //AVIAMENTO
        _texto.AppendLine("            <tr>");
        _texto.AppendLine("                <td style='line-height: 16px;'>");
        _texto.AppendLine("                    <table border='0' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
        _texto.AppendLine("                        width: 100%'>");
        _texto.AppendLine("                        <tr style='border-top: 1px solid #000;'>");
        _texto.AppendLine("                            <td colspan='2' style='border-left: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                AVIAMENTO");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                        </tr>");
        foreach (PROD_HB_PROD_AVIAMENTO av in _relatorio.AVIAMENTO)
        {
            _texto.AppendLine("                                    <tr style='border: 1px solid #000'>");
            _texto.AppendLine("                                        <td colspan='2' style='text-align: center;'>");
            if (av.PROD_AVIAMENTO1 != null)
                _texto.AppendLine("                                            " + (av.QTDE + ((av.QTDE_EXTRA == null) ? 0 : av.QTDE_EXTRA)).ToString().Replace(",000", "") + " " + av.PROD_AVIAMENTO1.UNIDADE_MEDIDA1.DESCRICAO + "s - " + av.PROD_AVIAMENTO1.DESCRICAO + " - " + av.DESCRICAO + " " + ((av.COMPRA_EXTRA == 'S') ? " - Extra" : ""));
            else
                _texto.AppendLine("                                            " + (av.QTDE + ((av.QTDE_EXTRA == null) ? 0 : av.QTDE_EXTRA)).ToString().Replace(",000", "") + " " + av.SUBGRUPO + " - " + av.COR_FORNECEDOR + " - Quantidade: " + av.QTDE_POR_PECA.ToString() + " " + av.DESCRICAO + " " + ((av.COMPRA_EXTRA == 'S') ? " - Extra" : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                    </tr>");
        }

        _texto.AppendLine("                    </table>");
        _texto.AppendLine("                </td>");
        _texto.AppendLine("            </tr>");

        _texto.AppendLine("            <tr>");
        _texto.AppendLine("                <td style='border: 1px solid #FFF;'>");
        _texto.AppendLine("                    &nbsp;");
        _texto.AppendLine("                </td>");
        _texto.AppendLine("            </tr>");
        //VAREJO
        _texto.AppendLine("            <tr>");
        _texto.AppendLine("                <td>");
        _texto.AppendLine("                    <table border='0' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
        _texto.AppendLine("                        width: 100%'>");
        _texto.AppendLine("                        <tr style='text-align: center; border-top: 1px solid #000;'>");
        _texto.AppendLine("                            <td style='border-right: 1px solid #000; border-left: 1px solid #000;'>");
        _texto.AppendLine("                                <span style='font-size: 11px;'>ESTOQUE VAREJO</span>");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
        _texto.AppendLine("                                " + ((_gradeNome != null) ? _gradeNome.GRADE_EXP : "EXP"));
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
        _texto.AppendLine("                                " + ((_gradeNome != null) ? _gradeNome.GRADE_XP : "XP"));
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
        _texto.AppendLine("                                " + ((_gradeNome != null) ? _gradeNome.GRADE_PP : "PP"));
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
        _texto.AppendLine("                                " + ((_gradeNome != null) ? _gradeNome.GRADE_P : "P"));
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
        _texto.AppendLine("                                " + ((_gradeNome != null) ? _gradeNome.GRADE_M : "M"));
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
        _texto.AppendLine("                                " + ((_gradeNome != null) ? _gradeNome.GRADE_G : "G"));
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
        _texto.AppendLine("                                " + ((_gradeNome != null) ? _gradeNome.GRADE_GG : "GG"));
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                TOTAL");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                        </tr>");
        //INICIO LOOP ESTOQUE VAREJO
        for (contLinha = 1; contLinha <= tLinhaVarejo; contLinha++)
        {
            _texto.AppendLine("                        <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
        }
        // FIM LOOP ESTOQUE VAREJO

        _texto.AppendLine("                        <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
        _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                                TOTAL FINAL");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                        </tr>");
        _texto.AppendLine("                    </table>");
        _texto.AppendLine("                </td>");
        _texto.AppendLine("            </tr>");
        _texto.AppendLine("            <tr style='border: 1px solid #FFF;'>");
        _texto.AppendLine("                <td style='border: 1px solid #FFF;'>");
        _texto.AppendLine("                    &nbsp;");
        _texto.AppendLine("                </td>");
        _texto.AppendLine("            </tr>");

        //ATACADO
        _texto.AppendLine("            <tr>");
        _texto.AppendLine("                <td>");
        _texto.AppendLine("                    <table border='0' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
        _texto.AppendLine("                        width: 100%'>");
        _texto.AppendLine("                        <tr style='text-align: center; border-top: 1px solid #000;'>");
        _texto.AppendLine("                            <td style='border-right: 1px solid #000; border-left: 1px solid #000;'>");
        _texto.AppendLine("                                <span style='font-size: 11px;'>ESTOQUE ATACADO</span>");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
        _texto.AppendLine("                                " + ((_gradeNome != null) ? _gradeNome.GRADE_EXP : "EXP"));
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
        _texto.AppendLine("                                " + ((_gradeNome != null) ? _gradeNome.GRADE_XP : "XP"));
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
        _texto.AppendLine("                                " + ((_gradeNome != null) ? _gradeNome.GRADE_PP : "PP"));
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
        _texto.AppendLine("                                " + ((_gradeNome != null) ? _gradeNome.GRADE_P : "P"));
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
        _texto.AppendLine("                                " + ((_gradeNome != null) ? _gradeNome.GRADE_M : "M"));
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
        _texto.AppendLine("                                " + ((_gradeNome != null) ? _gradeNome.GRADE_G : "G"));
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
        _texto.AppendLine("                                " + ((_gradeNome != null) ? _gradeNome.GRADE_GG : "GG"));
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                TOTAL");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                        </tr>");
        //INICIO LOOP ESTOQUE ATACADO
        for (contLinha = 1; contLinha <= tLinhaAtacado; contLinha++)
        {
            _texto.AppendLine("                        <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
        }
        // FIM LOOP ESTOQUE ATACADO

        _texto.AppendLine("                        <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
        _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                                TOTAL FINAL");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                        </tr>");
        _texto.AppendLine("                    </table>");
        _texto.AppendLine("                </td>");
        _texto.AppendLine("            </tr>");
        _texto.AppendLine("            <tr style='border: 1px solid #FFF;'>");
        _texto.AppendLine("                <td style='border: 1px solid #FFF;'>");
        _texto.AppendLine("                    &nbsp;");
        _texto.AppendLine("                </td>");
        _texto.AppendLine("            </tr>");

        //CONTROLE DE QUALIDADE
        _texto.AppendLine("            <tr>");
        _texto.AppendLine("                <td>");
        _texto.AppendLine("                    <table border='0' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
        _texto.AppendLine("                        width: 100%'>");
        _texto.AppendLine("                        <tr style='text-align: center; border-top: 1px solid #000;'>");
        _texto.AppendLine("                            <td style='border-right: 1px solid #000; border-left: 1px solid #000;'>");
        _texto.AppendLine("                                <span style='font-size: 11px;'>CONTROLE DE QUALIDADE</span>");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
        _texto.AppendLine("                                " + ((_gradeNome != null) ? _gradeNome.GRADE_EXP : "EXP"));
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
        _texto.AppendLine("                                " + ((_gradeNome != null) ? _gradeNome.GRADE_XP : "XP"));
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
        _texto.AppendLine("                                " + ((_gradeNome != null) ? _gradeNome.GRADE_PP : "PP"));
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
        _texto.AppendLine("                                " + ((_gradeNome != null) ? _gradeNome.GRADE_P : "P"));
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
        _texto.AppendLine("                                " + ((_gradeNome != null) ? _gradeNome.GRADE_M : "M"));
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
        _texto.AppendLine("                                " + ((_gradeNome != null) ? _gradeNome.GRADE_G : "G"));
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
        _texto.AppendLine("                                " + ((_gradeNome != null) ? _gradeNome.GRADE_GG : "GG"));
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                TOTAL");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                        </tr>");
        //INICIO LOOP CONTROLE DE QUALIDADE
        for (contLinha = 1; contLinha <= tLinhaControleQ; contLinha++)
        {
            _texto.AppendLine("                        <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
        }
        // FIM LOOP CONTROLE DE QUALIDADE

        _texto.AppendLine("                        <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
        _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                                TOTAL FINAL");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                        </tr>");
        _texto.AppendLine("                    </table>");
        _texto.AppendLine("                </td>");
        _texto.AppendLine("            </tr>");
        _texto.AppendLine("            <tr style='border: 1px solid #FFF;'>");
        _texto.AppendLine("                <td style='border: 1px solid #FFF;'>");
        _texto.AppendLine("                    &nbsp;");
        _texto.AppendLine("                </td>");
        _texto.AppendLine("            </tr>");

        //RODAPE
        _texto.AppendLine("            <tr>");
        _texto.AppendLine("                <td>");
        _texto.AppendLine("                    <table border='0' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
        _texto.AppendLine("                        width: 100%'>");
        _texto.AppendLine("                        <tr style='text-align: center; line-height: 18px;'>");
        _texto.AppendLine("                            <td style='border: 1px solid #000;'>");
        _texto.AppendLine("                                PILOTO");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='border: 1px solid #000;'>");
        _texto.AppendLine("                                MOSTRUÁRIO");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td colspan='8' style='text-align:left;'>");
        _texto.AppendLine("                                SCS - 2ª QUALIDADE");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                        </tr>");
        _texto.AppendLine("                        <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
        _texto.AppendLine("                            <td style='width: 120px; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 120px; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='border-top: 1px solid #fff; border-bottom: 1px solid #fff;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                " + ((_gradeNome != null) ? _gradeNome.GRADE_EXP : "EXP"));
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
        _texto.AppendLine("                                " + ((_gradeNome != null) ? _gradeNome.GRADE_XP : "XP"));
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
        _texto.AppendLine("                                " + ((_gradeNome != null) ? _gradeNome.GRADE_PP : "PP"));
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
        _texto.AppendLine("                                " + ((_gradeNome != null) ? _gradeNome.GRADE_P : "P"));
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
        _texto.AppendLine("                                " + ((_gradeNome != null) ? _gradeNome.GRADE_M : "M"));
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
        _texto.AppendLine("                                " + ((_gradeNome != null) ? _gradeNome.GRADE_G : "G"));
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
        _texto.AppendLine("                                " + ((_gradeNome != null) ? _gradeNome.GRADE_GG : "GG"));
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                TOTAL");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                        </tr>");
        _texto.AppendLine("                        <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
        _texto.AppendLine("                            <td style='width: 120px; border-top: 1px solid #fff; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 120px; border-top: 1px solid #fff; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='border-top: 1px solid #fff; border-bottom: 1px solid #fff;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                        </tr>");
        _texto.AppendLine("                        <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
        _texto.AppendLine("                            <td style='border: 1px solid #000;'>");
        _texto.AppendLine("                                PERDA");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='border: 1px solid #000;'>");
        _texto.AppendLine("                                AMOSTRA");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='border-top: 1px solid #fff; border-bottom: 1px solid #fff;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                        </tr>");
        _texto.AppendLine("                        <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
        _texto.AppendLine("                            <td style='width: 120px; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 120px; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='border-top: 1px solid #fff; border-bottom: 1px solid #fff;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                        </tr>");
        _texto.AppendLine("                        <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
        _texto.AppendLine("                            <td style='width: 120px; border-top: 1px solid #fff; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 120px; border-top: 1px solid #fff; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='border-top: 1px solid #fff; border-bottom: 1px solid #fff;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                        </tr>");
        _texto.AppendLine("                    </table>");
        _texto.AppendLine("                </td>");
        _texto.AppendLine("            </tr>");
        _texto.AppendLine("            <tr style='border: 1px solid #FFF;'>");
        _texto.AppendLine("                <td style='border: 1px solid #FFF;'>");
        _texto.AppendLine("                    &nbsp;");
        _texto.AppendLine("                </td>");
        _texto.AppendLine("            </tr>");
        _texto.AppendLine("            <tr>");
        _texto.AppendLine("                <td style='line-height: 12px;'>");
        _texto.AppendLine("                    <table border='0' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
        _texto.AppendLine("                        width: 100%;'>");
        _texto.AppendLine("                        <tr style='border-top: 1px solid #000;'>");
        _texto.AppendLine("                            <td style='width: 100%; border-left: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                OBSERVAÇÕES:");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                        </tr>");
        _texto.AppendLine("                        <tr style='border: 1px solid #000; border-top: 1px solid #FFF; line-height: 15px;'>");
        _texto.AppendLine("                            <td style='text-align: center;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                        </tr>");
        _texto.AppendLine("                    </table>");
        _texto.AppendLine("                </td>");
        _texto.AppendLine("            </tr>");
        _texto.AppendLine("            <tr>");
        _texto.AppendLine("                <td>");
        _texto.AppendLine("                    &nbsp;");
        _texto.AppendLine("                </td>");
        _texto.AppendLine("            </tr>");
        _texto.AppendLine("        </table>");
        _texto.AppendLine("</div>");

        return _texto;
    }

    #endregion

    #region "RISCO CAD"
    public StringBuilder MontarRiscoCAD(StringBuilder _texto, RelatorioHB _relatorio)
    {
        ProducaoController prodController = new ProducaoController();


        _texto.AppendLine("<div id='fichaCAD' align='center'>");
        _texto.AppendLine("<table border='0' cellpadding='0' cellspacing='0' width='689' style='width: 517pt; padding: 0px; color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif; background: white; white-space: nowrap;'>");
        _texto.AppendLine("<tr>");
        _texto.AppendLine("     <td style='line-height: 16px;'>");
        _texto.AppendLine("     <table border='0' cellpadding='0' cellspacing='0' width='689' style='border-collapse: collapse;");
        _texto.AppendLine("     width: 517pt'>");
        _texto.AppendLine("         <tr style='border-right: 1px solid #000; border-top: 2px solid #000;'>");
        _texto.AppendLine("             <td style='width: 70px; border-right: 1px solid #000; border-left: 1px solid #000;'>");
        _texto.AppendLine("                 HB");
        _texto.AppendLine("             </td>");
        _texto.AppendLine("             <td style='width: 150px; border-right: 1px solid #000;'>");
        _texto.AppendLine("                 GRUPO");
        _texto.AppendLine("             </td>");
        _texto.AppendLine("             <td style='width: 202px; border-right: 1px solid #000;'>");
        _texto.AppendLine("                 NOME");
        _texto.AppendLine("             </td>");
        _texto.AppendLine("             <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                 DATA");
        _texto.AppendLine("             </td>");
        _texto.AppendLine("             <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                 MOLDE");
        _texto.AppendLine("             </td>");
        _texto.AppendLine("             <td>");
        _texto.AppendLine("                 GRIFFE");
        _texto.AppendLine("             </td>");
        _texto.AppendLine("         </tr>");
        _texto.AppendLine("         <tr style='border: 1px solid #000; border-top: none;'>");
        _texto.AppendLine("             <td style='border-right: 1px solid #000; text-align: center'>");
        _texto.AppendLine("                 " + _relatorio.HB);
        _texto.AppendLine("             </td>");
        _texto.AppendLine("             <td style='border-right: 1px solid #000; text-align: center'>");
        _texto.AppendLine("                 " + _relatorio.GRUPO);
        _texto.AppendLine("             </td>");
        _texto.AppendLine("             <td style='border-right: 1px solid #000; text-align: center'>");
        _texto.AppendLine("                 " + _relatorio.CODIGO_PRODUTO_LINX + " - " + _relatorio.NOME);
        _texto.AppendLine("             </td>");
        _texto.AppendLine("             <td style='border-right: 1px solid #000; text-align: center'>");
        _texto.AppendLine("                 " + _relatorio.DATA);
        _texto.AppendLine("             </td>");
        _texto.AppendLine("             <td style='border-right: 1px solid #000; text-align: center'>");
        _texto.AppendLine("                 " + _relatorio.MOLDE);
        _texto.AppendLine("             </td>");
        _texto.AppendLine("             <td style='text-align: center'>");
        _texto.AppendLine("                 " + _relatorio.GRIFFE);
        _texto.AppendLine("             </td>");
        _texto.AppendLine("         </tr>");
        _texto.AppendLine("     </table>");
        _texto.AppendLine(" </td>");
        _texto.AppendLine("</tr>");
        _texto.AppendLine("<tr>");
        _texto.AppendLine(" <td style='line-height: 18px;'>");
        _texto.AppendLine("     <table border='0' cellpadding='0' cellspacing='0' width='689' style='border-collapse: collapse;");
        _texto.AppendLine("     width: 517pt'>");
        _texto.AppendLine("         <tr>");
        _texto.AppendLine("             <td style='width: 221px; border-right: 1px solid #000; border-left: 1px solid #000;'>");
        _texto.AppendLine("                 TECIDO - " + RetornarUnidadeMedidaMaterial(_relatorio.GRUPO_TECIDO, _relatorio.TECIDO));
        _texto.AppendLine("             </td>");
        _texto.AppendLine("             <td style='width: 100px; border-right: 1px solid #000;'>");
        _texto.AppendLine("                 COR");
        _texto.AppendLine("             </td>");
        if (_relatorio.SIGNED == "S")
        {
            _texto.AppendLine("             <td style='width: 201px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                 FORNECEDOR");
            _texto.AppendLine("             </td>");
            _texto.AppendLine("             <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                 LARGURA");
            _texto.AppendLine("             </td>");
            _texto.AppendLine("             <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                 SIGNED");
            _texto.AppendLine("             </td>");
        }
        else
        {
            _texto.AppendLine("             <td style='width: 253px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                 FORNECEDOR");
            _texto.AppendLine("             </td>");
            _texto.AppendLine("             <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                 LARGURA");
            _texto.AppendLine("             </td>");
        }
        _texto.AppendLine("         </tr>");
        _texto.AppendLine("         <tr style='border-right: 1px solid #000;'>");
        _texto.AppendLine("             <td style='border-right: 1px solid #000; border-left: 1px solid #000; text-align: center;'>");
        _texto.AppendLine("                 " + (_relatorio.TECIDO + ((_relatorio.COR_FORNECEDOR.Trim() == "") ? "" : " - " + _relatorio.COR_FORNECEDOR.Trim())));
        _texto.AppendLine("             </td>");
        _texto.AppendLine("             <td style='border-right: 1px solid #000; text-align: center;'>");
        _texto.AppendLine("                 " + _relatorio.DESC_COR);
        _texto.AppendLine("             </td>");
        _texto.AppendLine("             <td style='border-right: 1px solid #000; text-align: center;'>");
        _texto.AppendLine("                 " + _relatorio.FORNECEDOR);
        _texto.AppendLine("             </td>");
        if (_relatorio.SIGNED == "S")
        {
            _texto.AppendLine("             <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.AppendLine("                 " + _relatorio.LARGURA);
            _texto.AppendLine("             </td>");
            _texto.AppendLine("             <td style='text-align: center'>");
            _texto.AppendLine("                 " + _relatorio.SIGNED_NOME);
            _texto.AppendLine("             </td>");
        }
        else
        {
            _texto.AppendLine("             <td style='text-align: center'>");
            _texto.AppendLine("                 " + _relatorio.LARGURA);
            _texto.AppendLine("             </td>");
        }
        _texto.AppendLine("         </tr>");
        _texto.AppendLine("     </table>");
        _texto.AppendLine(" </td>");
        _texto.AppendLine("</tr>");

        if (_relatorio.MOSTRUARIO == "N")
        {
            decimal gastoMostruario = 0;
            var g = prodController.ObterHBGastoMostruario(_relatorio.CODIGO_PRODUTO_LINX);
            if (g != null)
                gastoMostruario = g.GASTO_MOSTRUARIO;

            _texto.AppendLine("<tr>");
            _texto.AppendLine(" <td style='line-height: 18px;'>");
            _texto.AppendLine("     <table border='0' cellpadding='0' cellspacing='0' width='689' style='border-collapse: collapse;");
            _texto.AppendLine("     width: 517pt'>");
            _texto.AppendLine("         <tr>");
            _texto.AppendLine("             <td style='width: 221px; border-right: 1px solid #000; border-left: 1px solid #000; border-top: 1px solid #000;'>");
            _texto.AppendLine("                 GASTO MOSTRUÁRIO");
            _texto.AppendLine("             </td>");
            _texto.AppendLine("         </tr>");

            _texto.AppendLine("         <tr style='border-right: 1px solid #000;'>");
            _texto.AppendLine("             <td style='border-right: 1px solid #000; border-left: 1px solid #000; text-align: left;'>");
            _texto.AppendLine("                 &nbsp;" + gastoMostruario.ToString("###,###,###0.000"));
            _texto.AppendLine("             </td>");
            _texto.AppendLine("         </tr>");
            _texto.AppendLine("     </table>");
            _texto.AppendLine(" </td>");
            _texto.AppendLine("</tr>");
        }

        var _gradeNome = new ProducaoController().ObterGradeNome(Convert.ToInt32(_relatorio.PROD_GRADE));

        _texto.AppendLine("<tr>");
        _texto.AppendLine("    <td style='line-height: 23px;'>");
        _texto.AppendLine("        <table border='0' cellpadding='0' cellspacing='0' width='689' style='border-collapse: collapse;");
        _texto.AppendLine("            width: 517pt'>");
        _texto.AppendLine("            <tr style='text-align: center; border-top: 1px solid #000;'>");
        _texto.AppendLine("                <td style='width: 130px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.AppendLine("                    &nbsp;");
        _texto.AppendLine("                </td>");
        _texto.AppendLine("                <td style='width: 70px; border-right: 1px solid #000;'>");
        _texto.AppendLine("                " + ((_gradeNome != null) ? _gradeNome.GRADE_EXP : "EXP"));
        _texto.AppendLine("                </td>");
        _texto.AppendLine("                <td style='width: 70px; border-right: 1px solid #000;'>");
        _texto.AppendLine("                " + ((_gradeNome != null) ? _gradeNome.GRADE_XP : "XP"));
        _texto.AppendLine("                </td>");
        _texto.AppendLine("                <td style='width: 70px; border-right: 1px solid #000;'>");
        _texto.AppendLine("                " + ((_gradeNome != null) ? _gradeNome.GRADE_PP : "PP"));
        _texto.AppendLine("                </td>");
        _texto.AppendLine("                <td style='width: 70px; border-right: 1px solid #000;'>");
        _texto.AppendLine("                " + ((_gradeNome != null) ? _gradeNome.GRADE_P : "P"));
        _texto.AppendLine("                </td>");
        _texto.AppendLine("                <td style='width: 70px; border-right: 1px solid #000;'>");
        _texto.AppendLine("                " + ((_gradeNome != null) ? _gradeNome.GRADE_M : "M"));
        _texto.AppendLine("                </td>");
        _texto.AppendLine("                <td style='width: 70px; border-right: 1px solid #000;'>");
        _texto.AppendLine("                " + ((_gradeNome != null) ? _gradeNome.GRADE_G : "G"));
        _texto.AppendLine("                </td>");
        _texto.AppendLine("                <td style='width: 70px; border-right: 1px solid #000;'>");
        _texto.AppendLine("                " + ((_gradeNome != null) ? _gradeNome.GRADE_GG : "GG"));
        _texto.AppendLine("                </td>");
        _texto.AppendLine("                <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                    TOTAL");
        _texto.AppendLine("                </td>");
        _texto.AppendLine("            </tr>");
        _texto.AppendLine("            <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
        _texto.AppendLine("                <td style='text-align: left; border-right: 1px solid #000;'>");
        _texto.AppendLine("                    GRADE PREVISTA");
        _texto.AppendLine("                </td>");
        _texto.AppendLine("                <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                    " + _relatorio.GRADE.GRADE_EXP);
        _texto.AppendLine("                </td>");
        _texto.AppendLine("                <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                    " + _relatorio.GRADE.GRADE_XP);
        _texto.AppendLine("                </td>");
        _texto.AppendLine("                <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                    " + _relatorio.GRADE.GRADE_PP);
        _texto.AppendLine("                </td>");
        _texto.AppendLine("                <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                    " + _relatorio.GRADE.GRADE_P);
        _texto.AppendLine("                </td>");
        _texto.AppendLine("                <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                    " + _relatorio.GRADE.GRADE_M);
        _texto.AppendLine("                </td>");
        _texto.AppendLine("                <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                    " + _relatorio.GRADE.GRADE_G);
        _texto.AppendLine("                </td>");
        _texto.AppendLine("                <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                    " + _relatorio.GRADE.GRADE_GG);
        _texto.AppendLine("                </td>");
        _texto.AppendLine("                <td>");
        _texto.AppendLine("                    " + (Convert.ToInt32(_relatorio.GRADE.GRADE_EXP) + Convert.ToInt32(_relatorio.GRADE.GRADE_XP) + Convert.ToInt32(_relatorio.GRADE.GRADE_PP) + Convert.ToInt32(_relatorio.GRADE.GRADE_P) + Convert.ToInt32(_relatorio.GRADE.GRADE_M) + Convert.ToInt32(_relatorio.GRADE.GRADE_G) + Convert.ToInt32(_relatorio.GRADE.GRADE_GG)));
        _texto.AppendLine("                </td>");
        _texto.AppendLine("            </tr>");
        _texto.AppendLine("            <tr style='border-bottom: 1px solid #000; text-align: center;'>");
        _texto.AppendLine("                <td style='text-align: left; border-left: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.AppendLine("                    GRADE REAL");
        _texto.AppendLine("                </td>");
        _texto.AppendLine("                <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                    " + ((_relatorio.GRADEREAL == null) ? "&nbsp;" : _relatorio.GRADEREAL.GRADE_EXP.ToString()));
        _texto.AppendLine("                </td>");
        _texto.AppendLine("                <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                    " + ((_relatorio.GRADEREAL == null) ? "&nbsp;" : _relatorio.GRADEREAL.GRADE_XP.ToString()));
        _texto.AppendLine("                </td>");
        _texto.AppendLine("                <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                    " + ((_relatorio.GRADEREAL == null) ? "&nbsp;" : _relatorio.GRADEREAL.GRADE_PP.ToString()));
        _texto.AppendLine("                </td>");
        _texto.AppendLine("                <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                    " + ((_relatorio.GRADEREAL == null) ? "&nbsp;" : _relatorio.GRADEREAL.GRADE_P.ToString()));
        _texto.AppendLine("                </td>");
        _texto.AppendLine("                <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                    " + ((_relatorio.GRADEREAL == null) ? "&nbsp;" : _relatorio.GRADEREAL.GRADE_M.ToString()));
        _texto.AppendLine("                </td>");
        _texto.AppendLine("                <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                    " + ((_relatorio.GRADEREAL == null) ? "&nbsp;" : _relatorio.GRADEREAL.GRADE_G.ToString()));
        _texto.AppendLine("                </td>");
        _texto.AppendLine("                <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                    " + ((_relatorio.GRADEREAL == null) ? "&nbsp;" : _relatorio.GRADEREAL.GRADE_GG.ToString()));
        _texto.AppendLine("                </td>");
        _texto.AppendLine("                <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                    " + ((_relatorio.GRADEREAL == null) ? "&nbsp;" : (Convert.ToInt32(_relatorio.GRADEREAL.GRADE_EXP) + Convert.ToInt32(_relatorio.GRADEREAL.GRADE_XP) + Convert.ToInt32(_relatorio.GRADEREAL.GRADE_PP) + Convert.ToInt32(_relatorio.GRADEREAL.GRADE_P) + Convert.ToInt32(_relatorio.GRADEREAL.GRADE_M) + Convert.ToInt32(_relatorio.GRADEREAL.GRADE_G) + Convert.ToInt32(_relatorio.GRADEREAL.GRADE_GG)).ToString()));
        _texto.AppendLine("                </td>");
        _texto.AppendLine("            </tr>");
        _texto.AppendLine("        </table>");
        _texto.AppendLine("    </td>");
        _texto.AppendLine("</tr>");

        _texto.AppendLine("<tr>");
        _texto.AppendLine("    <td style='line-height: 23px;'>");
        _texto.AppendLine("        <table border='1' cellpadding='0' cellspacing='0' width='689' style='border-collapse: collapse;");
        _texto.AppendLine("            width: 517pt'>");
        _texto.AppendLine("            <tr style='line-height: 12px;'>");
        _texto.AppendLine("                <td style='border-left: 1px solid #000; border: 1px solid #000;'>");
        _texto.AppendLine("                    &nbsp;");
        _texto.AppendLine("                </td>");
        _texto.AppendLine("            </tr>");
        _texto.AppendLine("            <tr>");
        _texto.AppendLine("                <td>");
        _texto.AppendLine("                    <table border='0' cellpadding='0' cellspacing='0' width='100%' style='border-collapse: collapse;'>");


        int i = 0;
        for (i = 0; i < 12; )
        {
            _texto.AppendLine("                        <tr style='border-top: 1px solid #000;'>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                GASTO/ " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 0) ? _relatorio.GRADE_CORTE[i + 0].RISCO_NOME.ToString() : "&nbsp;"));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.AppendLine("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 0) ? Convert.ToDecimal(_relatorio.GRADE_CORTE[i + 0].GASTO).ToString() : "&nbsp;"));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                GASTO/ " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 1) ? _relatorio.GRADE_CORTE[i + 1].RISCO_NOME.ToString() : "&nbsp;"));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.AppendLine("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 1) ? Convert.ToDecimal(_relatorio.GRADE_CORTE[i + 1].GASTO).ToString() : "&nbsp;"));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                GASTO/ " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 2) ? _relatorio.GRADE_CORTE[i + 2].RISCO_NOME.ToString() : "&nbsp;"));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.AppendLine("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 2) ? Convert.ToDecimal(_relatorio.GRADE_CORTE[i + 2].GASTO).ToString() : "&nbsp;"));
            _texto.AppendLine("                            </td>");
            if (i == 0)
            {
                _texto.AppendLine("                            <td rowspan='26' style='text-align: center; width: 82px;'>");
                _texto.AppendLine("                                 <img alt='Foto Tecido' Width='130px' Height='180px' src='..\\.." + _relatorio.FOTO_TECIDO.Replace("~", "").Replace("/", "\\") + "' />");
                _texto.AppendLine("                            </td>");
            }

            _texto.AppendLine("                        <tr style='border-top: 1px solid #000;'>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                GASTO/KG ");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.AppendLine("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 0) ? Convert.ToDecimal(_relatorio.GRADE_CORTE[i + 0].GASTO_KGS).ToString() : "&nbsp;"));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                GASTO/KG ");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.AppendLine("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 1) ? Convert.ToDecimal(_relatorio.GRADE_CORTE[i + 1].GASTO_KGS).ToString() : "&nbsp;"));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                GASTO/KG ");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.AppendLine("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 2) ? Convert.ToDecimal(_relatorio.GRADE_CORTE[i + 2].GASTO_KGS).ToString() : "&nbsp;"));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");

            _texto.AppendLine("                        <tr style='border: 1px solid #000; border-left: none;'>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                " + (i + 1).ToString() + "ª GRADE");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.AppendLine("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 0) ? (_relatorio.GRADE_CORTE[i + 0].GRADE).ToString() : "&nbsp;"));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                " + (i + 2).ToString() + "ª GRADE");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.AppendLine("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 1) ? (_relatorio.GRADE_CORTE[i + 1].GRADE).ToString() : "&nbsp;"));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                " + (i + 3).ToString() + "ª GRADE");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.AppendLine("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 2) ? (_relatorio.GRADE_CORTE[i + 2].GRADE).ToString() : "&nbsp;"));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                        <tr style='border: 1px solid #000; border-left: none;'>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                FOLHAS");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.AppendLine("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 0) ? Convert.ToInt32(_relatorio.GRADE_CORTE[i + 0].FOLHA).ToString() : "&nbsp;"));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                FOLHAS");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.AppendLine("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 1) ? Convert.ToInt32(_relatorio.GRADE_CORTE[i + 1].FOLHA).ToString() : "&nbsp;"));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                FOLHAS");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.AppendLine("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 2) ? Convert.ToInt32(_relatorio.GRADE_CORTE[i + 2].FOLHA).ToString() : "&nbsp;"));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                        <tr style='line-height: 12px;'>");
            _texto.AppendLine("                            <td colspan='6' style='border-bottom: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");

            i = i + 3;
        }
        _texto.AppendLine("                        <tr style='line-height: 12px;'>");
        _texto.AppendLine("                            <td colspan='6' style='border-bottom: 1px solid #000;'>");
        _texto.AppendLine("                                &nbsp;");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                        </tr>");
        _texto.AppendLine("                    </table>");
        _texto.AppendLine("                </td>");
        _texto.AppendLine("            </tr>");
        _texto.AppendLine("        </table>");
        _texto.AppendLine("    </td>");
        _texto.AppendLine("</tr>");

        _texto.AppendLine("<tr>");
        _texto.AppendLine("    <td style='line-height: 17px;'>");
        _texto.AppendLine("        <table border='0' cellpadding='0' cellspacing='0' width='689' style='border-collapse: collapse;");
        _texto.AppendLine("            width: 517pt'>");
        _texto.AppendLine("            <tr style='border-right: 1px solid #000; border-top: 1px solid #000;'>");
        _texto.AppendLine("                <td style='width: 170px; border-right: 1px solid #000; border-left: 1px solid #000;'>");
        _texto.AppendLine("                    MODELAGEM");
        _texto.AppendLine("                </td>");
        _texto.AppendLine("                <td style='width: 130px; border-right: 1px solid #000;'>");
        _texto.AppendLine("                    TOTAL DE PARTES");
        _texto.AppendLine("                </td>");
        _texto.AppendLine("                <td style='width: 50px; border-right: 1px solid #000;'>");
        _texto.AppendLine("                    RISCO");
        _texto.AppendLine("                </td>");
        _texto.AppendLine("                <td style='width: 130px; border-right: 1px solid #000;'>");
        _texto.AppendLine("                    CONFERIDO");
        _texto.AppendLine("                </td>");
        _texto.AppendLine("                <td style='width: 130px; border-right: 1px solid #000;'>");
        _texto.AppendLine("                    LIQUIDAR");
        _texto.AppendLine("                </td>");
        _texto.AppendLine("                <td>");
        _texto.AppendLine("                    MOSTRUÁRIO");
        _texto.AppendLine("                </td>");
        _texto.AppendLine("            </tr>");
        _texto.AppendLine("            <tr style='border: 1px solid #000; border-top: none;'>");
        _texto.AppendLine("                <td style='border-right: 1px solid #000; text-align: center'>");
        _texto.AppendLine("                    " + _relatorio.MODELAGEM);
        _texto.AppendLine("                </td>");
        _texto.AppendLine("                <td style='border-right: 1px solid #000; text-align: center'>");
        _texto.AppendLine("                    &nbsp;");
        _texto.AppendLine("                </td>");
        _texto.AppendLine("                <td style='border-right: 1px solid #000; text-align: center'>");
        _texto.AppendLine("                    &nbsp;");
        _texto.AppendLine("                </td>");
        _texto.AppendLine("                <td style='border-right: 1px solid #000; text-align: center'>");
        _texto.AppendLine("                    &nbsp;");
        _texto.AppendLine("                </td>");

        string liq = "";
        if (_relatorio.LIQUIDAR == "Sim")
            liq = _relatorio.LIQUIDAR;
        else if (_relatorio.LIQUIDAR_PQNO == "Sim")
            liq = _relatorio.LIQUIDAR_PQNO;
        else
            liq = "Não";

        _texto.AppendLine("                <td style='border-right: 1px solid #000; text-align: center'>");
        _texto.AppendLine("                    " + liq);
        _texto.AppendLine("                </td>");

        _texto.AppendLine("                <td style='text-align: center'>");
        _texto.AppendLine("                    " + ((_relatorio.MOSTRUARIO == "S") ? "Sim" : "Não"));
        _texto.AppendLine("                </td>");
        _texto.AppendLine("            </tr>");
        _texto.AppendLine("        </table>");
        _texto.AppendLine("    </td>");
        _texto.AppendLine("</tr>");



        return _texto;
    }
    public StringBuilder MontarReciboFaccao(StringBuilder _texto, RelatorioHB _relatorio, string breakClass)
    {
        _texto.AppendLine("<tr>");
        _texto.AppendLine("                <td style='line-height: 20px;'>");
        _texto.AppendLine("                <div id='divRecFacc' align='center' class='" + breakClass + "'>");
        _texto.AppendLine("                    <div>");
        _texto.AppendLine("                        <hr style='border: 1px dashed #000;' />");
        _texto.AppendLine("                    </div>");
        _texto.AppendLine("                    <table border='0' cellpadding='0' cellspacing='0' width='689' style='width: 517pt;");
        _texto.AppendLine("                            padding: 0px; color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
        _texto.AppendLine("                            background: white; white-space: nowrap;'>");
        _texto.AppendLine("                        <tr>");
        _texto.AppendLine("                            <td style='border: 1px solid #000; line-height: 25px; width: 320px; border-right: none;'>");
        _texto.AppendLine("                                <b> &nbsp;FACÇÃO:</b>");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='border: 1px solid #000;'>");
        //_texto.AppendLine("                                &nbsp;RECIBO DE FACÇÃO");
        _texto.AppendLine("                                <div style='float:left;'>");
        _texto.AppendLine("                                    &nbsp;RECIBO DE FACÇÃO");
        _texto.AppendLine("                                </div>");
        _texto.AppendLine("                                <div style='float:right;'>");
        _texto.AppendLine("                                    DATA RISCO: " + _relatorio.PROCESSO.DATA_INICIO + "&nbsp;");
        _texto.AppendLine("                                </div>");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                        </tr>");
        _texto.AppendLine("                        <tr>");
        _texto.AppendLine("                            <td style='line-height: 20px;' colspan='2'>");
        _texto.AppendLine("                                <table border='0' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
        _texto.AppendLine("                                    width: 517pt'>");
        _texto.AppendLine("                                    <tr style='text-align: left; border-top:none; border-bottom: none;'>");
        _texto.AppendLine("                                        <td style='width: 215px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                            &nbsp;Coleção: " + _relatorio.COLECAO);
        _texto.AppendLine("                                        </td>");
        _texto.AppendLine("                                        <td style='width: 215px; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                            &nbsp;HB: " + _relatorio.HB.ToString());
        _texto.AppendLine("                                        </td>");
        _texto.AppendLine("                                        <td style='border-right: 1px solid #000;' valign='top'>");
        _texto.AppendLine("                                            &nbsp;Nome: " + _relatorio.CODIGO_PRODUTO_LINX + " - " + _relatorio.NOME);
        _texto.AppendLine("                                        </td>");
        _texto.AppendLine("                                    </tr>");
        _texto.AppendLine("                                    <tr style='border: 1px solid #000;'>");
        _texto.AppendLine("                                        <td style='border-left: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                            &nbsp;Saída:");
        _texto.AppendLine("                                        </td>");
        _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                                            &nbsp;");
        _texto.AppendLine("                                        </td>");
        _texto.AppendLine("                                        <td style=''>");
        _texto.AppendLine("                                            &nbsp;Cor: " + _relatorio.DESC_COR);
        _texto.AppendLine("                                        </td>");
        _texto.AppendLine("                                    </tr>");
        _texto.AppendLine("                                    <tr>");
        _texto.AppendLine("                                        <td colspan='3'>");
        _texto.AppendLine("                                            &nbsp;");
        _texto.AppendLine("                                        </td>");
        _texto.AppendLine("                                    </tr>");
        _texto.AppendLine("                                    <tr style='text-align: left; border-top: 1px solid #000; border-bottom: none;'>");
        _texto.AppendLine("                                        <td style='border-left: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                            &nbsp;Etiqueta Tamanho:");
        _texto.AppendLine("                                        </td>");
        _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                                            &nbsp;");
        _texto.AppendLine("                                        </td>");
        _texto.AppendLine("                                        <td style='border-right: 1px solid #000;' valign='top'>");
        _texto.AppendLine("                                            &nbsp;");
        _texto.AppendLine("                                        </td>");
        _texto.AppendLine("                                    </tr>");
        _texto.AppendLine("                                    <tr style='text-align: left; border-top: 1px solid #000; border-bottom: none;'>");
        _texto.AppendLine("                                        <td style='border-left: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                            &nbsp;Etiqueta Composição:");
        _texto.AppendLine("                                        </td>");
        _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                                            &nbsp;");
        _texto.AppendLine("                                        </td>");
        _texto.AppendLine("                                        <td style='border-right: 1px solid #000;' valign='top'>");
        _texto.AppendLine("                                            &nbsp;");
        _texto.AppendLine("                                        </td>");
        _texto.AppendLine("                                    </tr>");
        _texto.AppendLine("                                    <tr style='text-align: left; border-top: 1px solid #000; border-bottom: none;'>");
        _texto.AppendLine("                                        <td style='border-left: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                            &nbsp;Gabarito: " + ((_relatorio.GABARITO == "S") ? "Sim" : "Não"));
        _texto.AppendLine("                                        </td>");
        _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                                            &nbsp;");
        _texto.AppendLine("                                        </td>");
        _texto.AppendLine("                                        <td style='border-right: 1px solid #000;' valign='top'>");
        _texto.AppendLine("                                            &nbsp;");
        _texto.AppendLine("                                        </td>");
        _texto.AppendLine("                                    </tr>");
        _texto.AppendLine("                                    <tr style='text-align: left; border-top: 1px solid #000; border-bottom: none;'>");
        _texto.AppendLine("                                        <td style='border-left: 1px solid #000; border-right: 1px solid #000;'>");
        _texto.AppendLine("                                            &nbsp;Estamparia: " + ((_relatorio.ESTAMPARIA == "S") ? "Sim" : "Não"));
        _texto.AppendLine("                                        </td>");
        _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
        _texto.AppendLine("                                            &nbsp;");
        _texto.AppendLine("                                        </td>");
        _texto.AppendLine("                                        <td style='border-right: 1px solid #000;' valign='top'>");
        _texto.AppendLine("                                            &nbsp;");
        _texto.AppendLine("                                        </td>");
        _texto.AppendLine("                                    </tr>");
        _texto.AppendLine("                                </table>");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                        </tr>");
        _texto.AppendLine("                        <tr>");
        _texto.AppendLine("                            <td style='line-height: 20px;' valign='top'>");
        _texto.AppendLine("                                <table border='0' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
        _texto.AppendLine("                                    width: 100%'>");
        _texto.AppendLine("                                    <tr style='text-align: left; border-top: 1px solid #000;; border-bottom: 1px solid #000;'>");
        _texto.AppendLine("                                        <td style='width: 215px; border-left: 1px solid #000; border-right: none; background-color: #c8a1a1'>");
        _texto.AppendLine("                                            &nbsp; " + ((_relatorio.DETALHE == null || _relatorio.DETALHE.Count() <= 0) ? "Sem " : "") + "Detalhes");
        _texto.AppendLine("                                        </td>");
        _texto.AppendLine("                                    </tr>");

        string preencherVazio = " " +
                                " <tr style='border: 1px solid #000;'>" +
                                "       <td style='border-left: 1px solid #000; border-right: 1px solid #000;'>" +
                                "           &nbsp;" +
                                "       </td>" +
                                " </tr>" +
                                " ";

        int totalDetalhe = 0;
        foreach (PROD_HB p in _relatorio.DETALHE)
        {

            _texto.AppendLine("                        <tr style='border: 1px solid #000;'>");
            _texto.AppendLine("                            <td style='border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;" + p.PROD_DETALHE1.DESCRICAO.ToUpper());
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            totalDetalhe += 1;

        }
        _texto.Append("                        ***DET");

        _texto.AppendLine("                    </table>");
        _texto.AppendLine("                </td>");
        _texto.AppendLine("                <td style='line-height: 20px;' valign='top'>");
        _texto.AppendLine("                    <table border='0' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
        _texto.AppendLine("                        width: 100%'>");
        _texto.AppendLine("                        <tr style='text-align: left; border-top: 1px solid #000; border-bottom: 1px solid #000;'>");
        _texto.AppendLine("                            <td style='width: 215px; border-left: 1px solid #000; border-right: 1px solid #000; background-color: #c8a1a1'>");
        _texto.AppendLine("                                &nbsp;Aviamentos");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                        </tr>");

        int totalAviamento = 0;
        if (_relatorio.AVIAMENTO != null)
        {
            foreach (PROD_HB_PROD_AVIAMENTO av in _relatorio.AVIAMENTO)
            {
                _texto.AppendLine("                        <tr style='border: 1px solid #000;'>");
                _texto.AppendLine("                            <td style='border-left: 1px solid #000; border-right: 1px solid #000;'>");
                if (av.PROD_AVIAMENTO1 != null)
                    _texto.AppendLine("                                             &nbsp;" + av.PROD_AVIAMENTO1.UNIDADE_MEDIDA1.DESCRICAO + "s - " + av.PROD_AVIAMENTO1.DESCRICAO + " - " + av.DESCRICAO + " " + ((av.COMPRA_EXTRA == 'S') ? " - Extra" : ""));
                else
                    _texto.AppendLine("                                             &nbsp;" + av.SUBGRUPO + " - " + av.COR_FORNECEDOR + " " + ((av.COMPRA_EXTRA == 'S') ? " - Extra" : ""));
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                        </tr>");
                totalAviamento += 1;
            }
        }
        _texto.AppendLine("                        ***AVI");

        _texto.AppendLine("                    </table>");
        _texto.AppendLine("                </td>");
        _texto.AppendLine("                        </tr>");
        _texto.AppendLine("                        <tr>");
        _texto.AppendLine("                            <td style='line-height: 40px;'>");
        _texto.AppendLine("                                DATA: _________________________________________");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                            <td style='line-height: 20px;'>");
        _texto.AppendLine("                                ASSINATURA: __________________________________");
        _texto.AppendLine("                            </td>");
        _texto.AppendLine("                        </tr>");
        _texto.AppendLine("");
        _texto.AppendLine("                    </table>");



        _texto.AppendLine("                </div>");
        _texto.AppendLine("                </td>");

        _texto.AppendLine("</tr>");

        _texto.AppendLine("<tr>");
        _texto.AppendLine("     <td style='border: 1px solid #000; border-top: 1px solid #000;'>");
        _texto.AppendLine("         OBSERVAÇÃO");
        _texto.AppendLine("     </td>");
        _texto.AppendLine("</tr>");
        _texto.AppendLine("<tr style='line-height: 25px; vertical-align: top;' valign='top'>");
        _texto.AppendLine("     <td style='border: 1px solid #000; border-top: 1px solid #000;'>");
        _texto.AppendLine("                    " + _relatorio.OBS.Replace("\r", "<br/>").Replace("\n", "<br/>"));
        _texto.AppendLine("     </td>");
        _texto.AppendLine("</tr>");
        _texto.AppendLine("</table></div>");


        //PREENCHE LINHAS VAZIAS
        if (totalDetalhe > totalAviamento)
        {
            int qtdeLinhaAviamento = totalDetalhe - totalAviamento;

            string auxConcat = "";
            int i = 0;
            while (i < qtdeLinhaAviamento)
            {
                auxConcat += preencherVazio;
                i += 1;
            }

            _texto.Replace("***AVI", auxConcat);
        }
        else if (totalAviamento > totalDetalhe)
        {
            int qtdeLinhaDetalhe = totalAviamento - totalDetalhe;

            string auxConcat = "";
            int i = 0;
            while (i < qtdeLinhaDetalhe)
            {
                auxConcat += preencherVazio;
                i += 1;
            }

            _texto.Replace("***DET", auxConcat);
        }

        _texto.Replace("***DET", "");
        _texto.Replace("***AVI", "");

        return _texto;
    }

    public StringBuilder MontarDetalheRiscoCAD(StringBuilder _texto, RelatorioHB _relatorio, string breakClass)
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();
        int count = 0;
        var _gradeNome = prodController.ObterGradeNome(Convert.ToInt32(_relatorio.PROD_GRADE));

        string griffe = "";
        string signed = "N";
        string signedNome = "";

        _relatorio.DETALHE = _relatorio.DETALHE.OrderByDescending(p => ((p.PROD_DETALHE == 4 || p.PROD_DETALHE == 5) ? 1 : 0)).ToList();

        foreach (PROD_HB d in _relatorio.DETALHE)
        {

            if (_relatorio.MOSTRUARIO == "Não" || _relatorio.MOSTRUARIO == "N")
            {

                if (d.PROD_DETALHE != 4 && d.PROD_DETALHE != 5)
                {
                    count += 1;

                    if (count == 1)
                        breakClass = "breakbefore";
                    else if (count == 3)
                        breakClass = "breakbefore";
                    else
                        breakClass = "";
                }
            }

            _relatorio.GRADE_CORTE = prodController.ObterGradeCorteHB(d.CODIGO);

            PROD_HB _prod_HB = prodController.ObterHB(Convert.ToInt32(d.CODIGO_PAI));
            var desenvProduto = desenvController.ObterProduto(_prod_HB.COLECAO, _prod_HB.CODIGO_PRODUTO_LINX, _prod_HB.COR);
            if (desenvProduto != null)
            {
                griffe = "";
                signed = "N";
                signedNome = "";

                if (desenvProduto.GRIFFE != null)
                    griffe = desenvProduto.GRIFFE;

                if (desenvProduto.SIGNED != null)
                    signed = desenvProduto.SIGNED.ToString();

                if (desenvProduto.SIGNED_NOME != null)
                    signedNome = desenvProduto.SIGNED_NOME;
            }


            _texto.AppendLine("    <br />");
            _texto.AppendLine("    <div id='divDetalhe" + d.PROD_DETALHE.ToString() + "' align='center' class='" + breakClass + "'>");
            _texto.AppendLine("        <table border='0' cellpadding='0' cellspacing='0' width='689' style='width: 517pt;");
            _texto.AppendLine("            padding: 0px; color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
            _texto.AppendLine("            background: white; white-space: nowrap;'>");
            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td style='line-height: 16px;'>");
            _texto.AppendLine("                    <table border='0' cellpadding='0' cellspacing='0' width='689' style='border-collapse: collapse;");
            _texto.AppendLine("                        width: 517pt'>");
            _texto.AppendLine("                        <tr style='border-right: 1px solid #000; border-top: 2px solid #000;'>");
            _texto.AppendLine("                            <td style='width: 70px; border-right: 1px solid #000; border-left: 1px solid #000;'>");
            _texto.AppendLine("                                HB");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 150px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                GRUPO");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 202px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                NOME");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                DATA");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                MOLDE");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                GRIFFE");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                        <tr style='border: 1px solid #000; border-top: none;'>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; text-align: center'>");
            _texto.AppendLine("                                " + d.HB);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; text-align: center'>");
            _texto.AppendLine("                                " + d.GRUPO);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; text-align: center'>");
            _texto.AppendLine("                                " + d.NOME);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; text-align: center'>");
            _texto.AppendLine("                                " + d.DATA_INCLUSAO);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; text-align: center'>");
            _texto.AppendLine("                                " + d.MOLDE);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='text-align: center'>");
            _texto.AppendLine("                                " + griffe.Trim());
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                    </table>");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");
            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td style='line-height: 18px;'>");
            _texto.AppendLine("                    <table border='0' cellpadding='0' cellspacing='0' width='689' style='border-collapse: collapse;");
            _texto.AppendLine("                        width: 517pt'>");
            _texto.AppendLine("                        <tr style='border-right: 1px solid #000; border-top: 0px solid #000;'>");
            _texto.AppendLine("                            <td style='width: 221px; border-right: 1px solid #000; border-left: 1px solid #000;'>");
            _texto.AppendLine("                                DETALHE");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; border-left: 1px solid #000;'>");
            _texto.AppendLine("                                TOTAL DE PARTES");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 100px; border-right: 1px solid #000; border-left: 1px solid #000;'>");
            _texto.AppendLine("                                LARGURA");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 100px;'>");
            _texto.AppendLine("                                MOSTRUÁRIO");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                        <tr style='border: 1px solid #000; border-top: none;'>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; text-align: center'>");
            _texto.AppendLine("                                <i><b>" + d.PROD_DETALHE1.DESCRICAO.ToUpper() + "</b></i>");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; text-align:center;'>");
            _texto.AppendLine("                                <table width='100%'>");
            _texto.AppendLine("                                    <tr>");
            _texto.AppendLine("                                        <td>");
            _texto.AppendLine("                                         &nbsp;");
            _texto.AppendLine("                                         </td>");
            _texto.AppendLine("                                    </tr>");

            _texto.AppendLine("                                </table>");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; text-align: center'>");
            _texto.AppendLine("                                " + ((d.LARGURA == null) ? "" : d.LARGURA.ToString()) + "");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; text-align: center'>");
            _texto.AppendLine("                                " + ((d.MOSTRUARIO == 'S') ? "SIM" : "NÃO") + "");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                    </table>");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");

            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td style='line-height: 18px;'>");
            _texto.AppendLine("                    <table border='0' cellpadding='0' cellspacing='0' width='689' style='border-collapse: collapse;");
            _texto.AppendLine("                        width: 517pt'>");
            _texto.AppendLine("                        <tr>");
            _texto.AppendLine("                            <td style='width: 221px; border-right: 1px solid #000; border-left: 1px solid #000;'>");
            _texto.AppendLine("                                TECIDO" + RetornarUnidadeMedidaMaterial(d.GRUPO_TECIDO, d.TECIDO));
            _texto.AppendLine("                            </td>");
            if (signed == "S")
            {
                _texto.AppendLine("                            <td style='width: 201px; border-right: 1px solid #000;'>");
                _texto.AppendLine("                                FORNECEDOR");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                SIGNED");
                _texto.AppendLine("                            </td>");
            }
            else
            {
                _texto.AppendLine("                            <td style='width: 253px; border-right: 1px solid #000;'>");
                _texto.AppendLine("                                FORNECEDOR");
                _texto.AppendLine("                            </td>");
            }
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                        <tr style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; border-left: 1px solid #000; text-align: center;'>");
            _texto.AppendLine("                                " + (d.TECIDO + ((d.COR_FORNECEDOR.Trim() == "") ? "" : " - " + d.COR_FORNECEDOR.Trim())));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.AppendLine("                                " + d.FORNECEDOR.Trim());
            _texto.AppendLine("                            </td>");
            if (signed == "S")
            {
                _texto.AppendLine("                            <td style='text-align: center'>");
                _texto.AppendLine("                                " + signedNome);
                _texto.AppendLine("                            </td>");
            }
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                    </table>");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");



            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td style='line-height: 23px;'>");
            _texto.AppendLine("                    <table border='0' cellpadding='0' cellspacing='0' width='689' style='border-collapse: collapse;");
            _texto.AppendLine("                        width: 517pt'>");
            _texto.AppendLine("                        <tr style='text-align: center; border-top: 2px solid #000;'>");
            _texto.AppendLine("                            <td style='width: 130px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 70px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                             " + ((_gradeNome != null) ? _gradeNome.GRADE_EXP : "EXP"));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 70px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                             " + ((_gradeNome != null) ? _gradeNome.GRADE_XP : "XP"));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 70px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                             " + ((_gradeNome != null) ? _gradeNome.GRADE_PP : "PP"));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 70px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                             " + ((_gradeNome != null) ? _gradeNome.GRADE_P : "P"));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 70px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                             " + ((_gradeNome != null) ? _gradeNome.GRADE_M : "M"));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 70px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                             " + ((_gradeNome != null) ? _gradeNome.GRADE_G : "G"));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 70px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                             " + ((_gradeNome != null) ? _gradeNome.GRADE_GG : "GG"));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                TOTAL");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                        <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
            _texto.AppendLine("                            <td style='text-align: left; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                GRADE PREVISTA");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                " + _relatorio.GRADE.GRADE_EXP);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                " + _relatorio.GRADE.GRADE_XP);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                " + _relatorio.GRADE.GRADE_PP);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                " + _relatorio.GRADE.GRADE_P);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                " + _relatorio.GRADE.GRADE_M);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                " + _relatorio.GRADE.GRADE_G);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                " + _relatorio.GRADE.GRADE_GG);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                " + (Convert.ToInt32(_relatorio.GRADE.GRADE_EXP) + Convert.ToInt32(_relatorio.GRADE.GRADE_XP) + Convert.ToInt32(_relatorio.GRADE.GRADE_PP) + Convert.ToInt32(_relatorio.GRADE.GRADE_P) + Convert.ToInt32(_relatorio.GRADE.GRADE_M) + Convert.ToInt32(_relatorio.GRADE.GRADE_G) + Convert.ToInt32(_relatorio.GRADE.GRADE_GG)));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                        <tr style='border-bottom: 1px solid #000; text-align: center;'>");
            _texto.AppendLine("                            <td style='text-align: left; border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                GRADE REAL");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                " + ((_relatorio.GRADEREAL == null) ? "&nbsp;" : _relatorio.GRADEREAL.GRADE_EXP.ToString()));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                " + ((_relatorio.GRADEREAL == null) ? "&nbsp;" : _relatorio.GRADEREAL.GRADE_XP.ToString()));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                " + ((_relatorio.GRADEREAL == null) ? "&nbsp;" : _relatorio.GRADEREAL.GRADE_PP.ToString()));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                " + ((_relatorio.GRADEREAL == null) ? "&nbsp;" : _relatorio.GRADEREAL.GRADE_P.ToString()));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                " + ((_relatorio.GRADEREAL == null) ? "&nbsp;" : _relatorio.GRADEREAL.GRADE_M.ToString()));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                " + ((_relatorio.GRADEREAL == null) ? "&nbsp;" : _relatorio.GRADEREAL.GRADE_G.ToString()));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                " + ((_relatorio.GRADEREAL == null) ? "&nbsp;" : _relatorio.GRADEREAL.GRADE_GG.ToString()));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                " + ((_relatorio.GRADEREAL == null) ? "&nbsp;" : (Convert.ToInt32(_relatorio.GRADEREAL.GRADE_EXP) + Convert.ToInt32(_relatorio.GRADEREAL.GRADE_XP) + Convert.ToInt32(_relatorio.GRADEREAL.GRADE_PP) + Convert.ToInt32(_relatorio.GRADEREAL.GRADE_P) + Convert.ToInt32(_relatorio.GRADEREAL.GRADE_M) + Convert.ToInt32(_relatorio.GRADEREAL.GRADE_G) + Convert.ToInt32(_relatorio.GRADEREAL.GRADE_GG)).ToString()));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                    </table>");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");
            _texto.AppendLine("<tr>");
            _texto.AppendLine("    <td style='line-height: 23px;'>");
            _texto.AppendLine("        <table border='1' cellpadding='0' cellspacing='0' width='689' style='border-collapse: collapse;");
            _texto.AppendLine("            width: 517pt'>");
            _texto.AppendLine("            <tr style='line-height: 12px;'>");
            _texto.AppendLine("                <td style='border-left: 1px solid #000; border: 1px solid #000;'>");
            _texto.AppendLine("                    &nbsp;");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");
            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td>");
            _texto.AppendLine("                    <table border='0' cellpadding='0' cellspacing='0' width='100%' style='border-collapse: collapse;'>");

            _texto.AppendLine("                        <tr style='border-top: 1px solid #000;'>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                RISCO");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                RISCO");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                RISCO");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td rowspan='26' style='text-align: center; width: 82px;'>");
            _texto.AppendLine("                                 <img alt='Foto Tecido' Width='130px' Height='180px' src='..\\.." + _relatorio.FOTO_TECIDO.Replace("~", "").Replace("/", "\\") + "' />");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");

            int i = 0;
            for (i = 0; i < 7; i++)
            {
                _texto.AppendLine("                        <tr style='border-top: 1px solid #000;'>");
                _texto.AppendLine("                            <td style='border-right: none;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000; text-align: center;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: none;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000; text-align: center;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: none;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000; text-align: center;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                        </tr>");
            }
            //int i = 0;
            //for (i = 0; i < 9; )
            //{
            //    _texto.AppendLine("                        <tr style='border-top: 1px solid #000;'>");
            //    _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            //    _texto.AppendLine("                                GASTO/ " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 0) ? _relatorio.GRADE_CORTE[i + 0].RISCO_NOME.ToString() : "&nbsp;"));
            //    _texto.AppendLine("                            </td>");
            //    _texto.AppendLine("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            //    _texto.AppendLine("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 0) ? Convert.ToDecimal(_relatorio.GRADE_CORTE[i + 0].GASTO).ToString() : "&nbsp;"));
            //    _texto.AppendLine("                            </td>");
            //    _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            //    _texto.AppendLine("                                GASTO/ " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 1) ? _relatorio.GRADE_CORTE[i + 1].RISCO_NOME.ToString() : "&nbsp;"));
            //    _texto.AppendLine("                            </td>");
            //    _texto.AppendLine("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            //    _texto.AppendLine("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 1) ? Convert.ToDecimal(_relatorio.GRADE_CORTE[i + 1].GASTO).ToString() : "&nbsp;"));
            //    _texto.AppendLine("                            </td>");
            //    _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            //    _texto.AppendLine("                                GASTO/ " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 2) ? _relatorio.GRADE_CORTE[i + 2].RISCO_NOME.ToString() : "&nbsp;"));
            //    _texto.AppendLine("                            </td>");
            //    _texto.AppendLine("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            //    _texto.AppendLine("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 2) ? Convert.ToDecimal(_relatorio.GRADE_CORTE[i + 2].GASTO).ToString() : "&nbsp;"));
            //    _texto.AppendLine("                            </td>");
            //    if (i == 0)
            //    {
            //        _texto.AppendLine("                            <td rowspan='26' style='text-align: center; width: 82px;'>");
            //        _texto.AppendLine("                                 <img alt='Foto Tecido' Width='130px' Height='180px' src='..\\.." + _relatorio.FOTO_TECIDO.Replace("~", "").Replace("/", "\\") + "' />");
            //        _texto.AppendLine("                            </td>");
            //    }
            //    _texto.AppendLine("                        </tr>");
            //    _texto.AppendLine("                        <tr style='border: 1px solid #000; border-left: none;'>");
            //    _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            //    _texto.AppendLine("                                " + (i + 1).ToString() + "ª GRADE");
            //    _texto.AppendLine("                            </td>");
            //    _texto.AppendLine("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            //    _texto.AppendLine("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 0) ? (_relatorio.GRADE_CORTE[i + 0].GRADE).ToString() : "&nbsp;"));
            //    _texto.AppendLine("                            </td>");
            //    _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            //    _texto.AppendLine("                                " + (i + 2).ToString() + "ª GRADE");
            //    _texto.AppendLine("                            </td>");
            //    _texto.AppendLine("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            //    _texto.AppendLine("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 1) ? (_relatorio.GRADE_CORTE[i + 1].GRADE).ToString() : "&nbsp;"));
            //    _texto.AppendLine("                            </td>");
            //    _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            //    _texto.AppendLine("                                " + (i + 3).ToString() + "ª GRADE");
            //    _texto.AppendLine("                            </td>");
            //    _texto.AppendLine("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            //    _texto.AppendLine("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 2) ? (_relatorio.GRADE_CORTE[i + 2].GRADE).ToString() : "&nbsp;"));
            //    _texto.AppendLine("                            </td>");
            //    _texto.AppendLine("                        </tr>");
            //    _texto.AppendLine("                        <tr style='border: 1px solid #000; border-left: none;'>");
            //    _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            //    _texto.AppendLine("                                FOLHAS");
            //    _texto.AppendLine("                            </td>");
            //    _texto.AppendLine("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            //    _texto.AppendLine("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 0) ? Convert.ToInt32(_relatorio.GRADE_CORTE[i + 0].FOLHA).ToString() : "&nbsp;"));
            //    _texto.AppendLine("                            </td>");
            //    _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            //    _texto.AppendLine("                                FOLHAS");
            //    _texto.AppendLine("                            </td>");
            //    _texto.AppendLine("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            //    _texto.AppendLine("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 1) ? Convert.ToInt32(_relatorio.GRADE_CORTE[i + 1].FOLHA).ToString() : "&nbsp;"));
            //    _texto.AppendLine("                            </td>");
            //    _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            //    _texto.AppendLine("                                FOLHAS");
            //    _texto.AppendLine("                            </td>");
            //    _texto.AppendLine("                            <td style='border-right: 1px solid #000; text-align: center;'>");
            //    _texto.AppendLine("                                " + ((_relatorio.GRADE_CORTE != null && _relatorio.GRADE_CORTE.Count > i + 2) ? Convert.ToInt32(_relatorio.GRADE_CORTE[i + 2].FOLHA).ToString() : "&nbsp;"));
            //    _texto.AppendLine("                            </td>");
            //    _texto.AppendLine("                        </tr>");
            //    _texto.AppendLine("                        <tr style='line-height: 12px;'>");
            //    _texto.AppendLine("                            <td colspan='6' style='border-bottom: 1px solid #000;'>");
            //    _texto.AppendLine("                                &nbsp;");
            //    _texto.AppendLine("                            </td>");
            //    _texto.AppendLine("                        </tr>");

            //    i = i + 3;
            //}
            _texto.AppendLine("                        <tr style='line-height: 12px;'>");
            _texto.AppendLine("                            <td colspan='6' style='border-bottom: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                    </table>");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");
            _texto.AppendLine("        </table>");
            _texto.AppendLine("    </td>");
            _texto.AppendLine("</tr>");

            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td style='border: 1px solid #000; border-top: 1px solid #000;'>");
            _texto.AppendLine("                    OBSERVAÇÃO");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");
            _texto.AppendLine("            <tr style='line-height: 25px; vertical-align: top;' valign='top'>");
            _texto.AppendLine("                <td style='border: 1px solid #000; border-top: 1px solid #000;'>");
            _texto.AppendLine("                    " + d.OBSERVACAO.Replace("\r", "<br/>").Replace("\n", "<br/>"));
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");
            // se for galao ou galao proprio
            if (d.PROD_DETALHE == 4 || d.PROD_DETALHE == 5)
            {
                _texto.AppendLine("            <tr>");
                _texto.AppendLine("                <td>");
                _texto = MontarDetalheGalao(_texto);
                _texto.AppendLine("                </td>");
                _texto.AppendLine("            </tr>");

            }
            _texto.AppendLine("        </table>");
            _texto.AppendLine("    </div>");



        }

        return _texto;
    }
    public StringBuilder MontarDetalheGalao(StringBuilder _texto)
    {

        _texto.AppendLine("<div id='CampoBranco' align='center' style='border: 0px solid #000;'>");
        _texto.AppendLine("    <div style='border: 0px solid #000; background-color: transparent;'>");
        _texto.AppendLine("        <table cellpadding='0' cellspacing='0' style='width: 100%; padding: 0px; color: black; height:350px;");
        _texto.AppendLine("                font-size: 10.0pt; font-weight: 700; font-family: Arial, sans-serif; background: white; white-space: nowrap;");
        _texto.AppendLine("                border: 1px solid #000;'>");
        _texto.AppendLine("            <tr style='height:25px;'>");
        _texto.AppendLine("                <td colspan='3' style='vertical-align:top;text-align:center; border-bottom: 1px solid #000;'>");
        _texto.AppendLine("                    Galão (Medidas)");
        _texto.AppendLine("                </td>");
        _texto.AppendLine("            </tr>");
        _texto.AppendLine("            <tr style='vertical-align:top;'>");
        _texto.AppendLine("                <td style='text-align:left; width:33%; border-right: 1px solid #000; border-bottom: 1px solid #000;'>");
        _texto.AppendLine("                    &nbsp;Fio Reto");
        _texto.AppendLine("                </td>");
        _texto.AppendLine("                <td style='text-align:center; width:33%; border-bottom: 1px solid #000;'>");
        _texto.AppendLine("                    &nbsp");
        _texto.AppendLine("                </td>");
        _texto.AppendLine("                <td style='text-align: center; width: 34%; border-bottom: 1px solid #000;'>");
        _texto.AppendLine("                    &nbsp");
        _texto.AppendLine("                </td>");
        _texto.AppendLine("            </tr>");

        _texto.AppendLine("            <tr style='vertical-align:top;'>");
        _texto.AppendLine("                <td style='text-align:left; width:33%; border-right: 1px solid #000; border-bottom: 1px solid #000;'>");
        _texto.AppendLine("                    &nbsp;Viés");
        _texto.AppendLine("                </td>");
        _texto.AppendLine("                <td style='text-align:center; width:33%; border-bottom: 1px solid #000;'>");
        _texto.AppendLine("                    &nbsp;");
        _texto.AppendLine("                </td>");
        _texto.AppendLine("                <td style='text-align: center; width: 34%; border-bottom: 1px solid #000;'>");
        _texto.AppendLine("                    &nbsp;");
        _texto.AppendLine("                </td>");
        _texto.AppendLine("            </tr>");

        _texto.AppendLine("            <tr style='vertical-align:top;'>");
        _texto.AppendLine("                <td style='text-align:left; width:33%; border-right: 1px solid #000; border-bottom: 1px solid #000;'>");
        _texto.AppendLine("                    &nbsp;Atravessado");
        _texto.AppendLine("                </td>");
        _texto.AppendLine("                <td style='text-align:center; width:33%; border-bottom: 1px solid #000;'>");
        _texto.AppendLine("                    &nbsp;");
        _texto.AppendLine("                </td>");
        _texto.AppendLine("                <td style='text-align: center; width: 34%; border-bottom: 1px solid #000;'>");
        _texto.AppendLine("                    &nbsp;");
        _texto.AppendLine("                </td>");
        _texto.AppendLine("            </tr>");

        _texto.AppendLine("        </table>");
        _texto.AppendLine("    </div>");
        _texto.AppendLine("</div>");

        return _texto;
    }

    #endregion


    #region "DADOS AUXILIARES"
    private string RetornarUnidadeMedidaMaterial(string grupo, string subGrupo)
    {
        string retorno = "0";
        //PREENCHER UNIDADE DE MEDIDA AUTOMATICAMENTE
        var material = new DesenvolvimentoController().ObterMaterial().Where(p => p.GRUPO.Trim() == grupo.Trim() &&
                                                                                  p.SUBGRUPO.Trim() == subGrupo.Trim()).ToList();
        if (material != null && material.Count() > 0)
        {
            if (material[0].UNIDADE.UNIDADE1.Trim() == "KG")
                retorno = "KG";
            else if (material[0].UNIDADE.UNIDADE1.Trim() == "MT")
                retorno = "Metro";
            else if (material[0].UNIDADE.UNIDADE1.Trim() == "UN")
                retorno = "Unidade";
        }

        return retorno;
    }
    #endregion


}