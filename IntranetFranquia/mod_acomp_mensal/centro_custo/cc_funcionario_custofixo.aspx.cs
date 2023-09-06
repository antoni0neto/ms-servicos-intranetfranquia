using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.DataVisualization.Charting;
using DAL;
using System.IO;
using System.Drawing;
using System.Text;
using System.Linq.Expressions;
using System.Linq.Dynamic;

namespace Relatorios.mod_acomp_mensal.centro_custo
{
    public partial class cc_funcionario_custofixo : System.Web.UI.Page
    {
        DREController dreController = new DREController();
        BaseController baseController = new BaseController();
        ContabilidadeController contabilidadeController = new ContabilidadeController();

        Color corTitulo = Color.Gainsboro;
        Color corFundo = Color.WhiteSmoke;
        string tagCorNegativo = "#CD2626";

        decimal totalSalarioLiquido = 0;
        decimal totalBeneficioVT = 0;

        StringBuilder sbCustoFixo_Acrescentados = new StringBuilder();
        StringBuilder sbCustoFixo_Retirados = new StringBuilder();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregarMesAno();
                CarregarCentroCustoGrupo();
                CarregarCentroCusto(0, "X");
                CarregarFilial();
            }
        }

        #region "DADOS INICIAIS"
        private void CarregarFilial()
        {
            List<EmpresaFilial> filial = new List<EmpresaFilial>();

            filial = contabilidadeController.BuscaEmpresaFiliail();

            if (filial != null)
            {
                filial.Insert(0, new EmpresaFilial { NumeroCnpj = "0", NomeFantasia = "" });
                ddlFilial.DataSource = filial;
                ddlFilial.DataBind();
            }
        }
        private void CarregarMesAno()
        {
            ddlMesAno.DataSource = baseController.BuscaReferencias().OrderByDescending(p => p.ANO).ThenByDescending(i => i.MES).Where(a => a.ANO > 2015);
            ddlMesAno.DataBind();

            string sMesAnoAtual = DateTime.Now.ToString().Substring(3, 7);
            ddlMesAno.SelectedIndex = ddlMesAno.Items.IndexOf(ddlMesAno.Items.FindByText(sMesAnoAtual));
        }
        private void CarregarCentroCustoGrupo()
        {
            var centroCustoGrupo = dreController.ObterCentroCustoGrupo();
            if (centroCustoGrupo != null)
            {
                centroCustoGrupo.Insert(0, new CTB_CENTRO_CUSTO_GRUPO { ID_GRUPO_CENTRO_CUSTO = -1, DESC_GRUPO_CENTRO_CUSTO = "NÃO IDENTIFICADO" });
                centroCustoGrupo.Insert(0, new CTB_CENTRO_CUSTO_GRUPO { ID_GRUPO_CENTRO_CUSTO = 0, DESC_GRUPO_CENTRO_CUSTO = "" });

                ddlCCustoGrupo.DataSource = centroCustoGrupo;
                ddlCCustoGrupo.DataBind();
            }
        }

        protected void ddlCCustoGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                if (ddlCCustoGrupo.SelectedValue != "" && ddlCCustoGrupo.SelectedValue != "0")
                    if (ddlCCustoGrupo.SelectedValue == "-1")
                    {
                        CarregarCentroCusto(Convert.ToInt32(ddlCCustoGrupo.SelectedValue), "N");
                    }
                    else
                    {
                        CarregarCentroCusto(Convert.ToInt32(ddlCCustoGrupo.SelectedValue), "X");
                    }
                else
                    CarregarCentroCusto(0, "X");
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message + "\n" + ex.StackTrace;
            }
        }

        private void CarregarCentroCusto(int idGrupo, String sNaoIdentificado)
        {
            var centroCusto = dreController.ObterCentroCusto().Where(p => !p.INATIVA).ToList();

            if (sNaoIdentificado == "N")
            {
                centroCusto = dreController.ObterCentroCusto().Where(p => 1 == 2).ToList();
                centroCusto.Insert(0, new CTB_CENTRO_CUSTO { CENTRO_CUSTO = "N", DESC_CENTRO_CUSTO = "NÃO IDENTIFICADO" });
            }
            else
            {
                centroCusto = dreController.ObterCentroCusto().Where(p => !p.INATIVA).ToList();
            }

            if (centroCusto != null)
            {
                if (idGrupo > 0)
                    centroCusto = centroCusto.Where(p => p.ID_GRUPO_CENTRO_CUSTO == idGrupo).OrderBy(x => x.DESC_CENTRO_CUSTO).ToList();

                centroCusto.Insert(0, new CTB_CENTRO_CUSTO { CENTRO_CUSTO = "0", DESC_CENTRO_CUSTO = "" });

                ddlCCusto.DataSource = centroCusto;
                ddlCCusto.DataBind();
            }
        }
        private void CarregarCentroCustoFuncionario(Int32 Mes, Int32 Ano, int? centroCustoGrupo, string centroCusto, string filial)
        {
            List<SP_FUNCIONARIO_CFResult> gCCFuncionario = new List<SP_FUNCIONARIO_CFResult>();

            gCCFuncionario = contabilidadeController.BuscaFuncionarioCustoFixo(Mes, Ano, centroCustoGrupo, centroCusto, filial);

            gvCentroCustoFuncionario.Columns[0].SortExpression = "DESC_GRUPO_CENTRO_CUSTO";
            gvCentroCustoFuncionario.DataSource = gCCFuncionario;
            gvCentroCustoFuncionario.DataBind();
        }

        private void limparMensagens()
        {
            labErro.Text = "";
            labErro.Visible = false;

            labSucesso.Text = "";
            labSucesso.Visible = false;
        }

        #endregion

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            Int32 Mes;
            int Ano;
            int? centroCustoGrupo = null;
            string centroCusto = "";
            string filial = "";

            limparMensagens();

            totalSalarioLiquido = 0;
            totalBeneficioVT = 0;

            Mes = Convert.ToInt32(ddlMesAno.SelectedItem.Text.Split('/')[0]);
            Ano = Convert.ToInt32(ddlMesAno.SelectedItem.Text.Split('/')[1]);

            //Obter Grupo Centro de Custo
            if (ddlCCustoGrupo.SelectedValue.Trim() != "" && ddlCCustoGrupo.SelectedValue.Trim() != "0")
                centroCustoGrupo = Convert.ToInt32(ddlCCustoGrupo.SelectedValue);

            //Obter Centro de Custo
            if (ddlCCusto.SelectedValue.Trim() != "" && ddlCCusto.SelectedValue.Trim() != "0")
                centroCusto = ddlCCusto.SelectedValue;

            //Obter Filial
            if (ddlFilial.SelectedValue.Trim() != "" && ddlFilial.SelectedValue.Trim() != "0")
                filial = ddlFilial.SelectedValue;

            try
            {

                labErro.Text = "";
                CarregarCentroCustoFuncionario(Mes, Ano, centroCustoGrupo, centroCusto, filial);
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message + "\n" + ex.StackTrace;
            }
        }
        private string FormatarValor(decimal? valor)
        {
            string tagCor = "#000";
            string retorno = "";
            if (valor < 0)
                tagCor = tagCorNegativo;

            retorno = "<font size='2' face='Calibri' color='" + tagCor + "'>" + Convert.ToDecimal(valor).ToString("###,###,###,##0.00;(###,###,###,##0.00)") + "</font> ";
            return retorno;
        }

        protected void gvCentroCustoFuncionario_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                limparMensagens();

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (e.Row.DataItem != null)
                    {
                        SP_FUNCIONARIO_CFResult fcf = e.Row.DataItem as SP_FUNCIONARIO_CFResult;

                        if (fcf != null)
                        {
                            HiddenField _hdnGrupoCC = e.Row.FindControl("hdnCentroCustoGrupo") as HiddenField;
                            DropDownList _ddlGrupoCC = e.Row.FindControl("ddlCentroCustoGrupo") as DropDownList;
                            if (_ddlGrupoCC != null)
                            {
                                var centroCustoGrupo = dreController.ObterCentroCustoGrupo();
                                if (centroCustoGrupo != null)
                                {
                                    centroCustoGrupo.Insert(0, new CTB_CENTRO_CUSTO_GRUPO { ID_GRUPO_CENTRO_CUSTO = 0, DESC_GRUPO_CENTRO_CUSTO = "NÃO IDENTIFICADO" });
                                    centroCustoGrupo.Insert(0, new CTB_CENTRO_CUSTO_GRUPO { ID_GRUPO_CENTRO_CUSTO = -1, DESC_GRUPO_CENTRO_CUSTO = "" });

                                    _ddlGrupoCC.DataSource = centroCustoGrupo;
                                    _ddlGrupoCC.DataBind();
                                    if (_hdnGrupoCC != null)
                                    {
                                        _ddlGrupoCC.SelectedValue = _hdnGrupoCC.Value;
                                    }
                                }
                            }

                            HiddenField _hdnCCusto = e.Row.FindControl("hdnDescricaoCentroCusto") as HiddenField;
                            DropDownList _ddlCCusto = e.Row.FindControl("ddlCentroCusto") as DropDownList;
                            if (_ddlCCusto != null)
                            {
                                var centroCusto = dreController.ObterCentroCusto().Where(p => !p.INATIVA).ToList();
                                if (centroCusto != null)
                                {
                                    centroCusto = centroCusto.Where(p => p.ID_GRUPO_CENTRO_CUSTO == Convert.ToInt32(_hdnGrupoCC.Value)).OrderBy(x => x.DESC_CENTRO_CUSTO).ToList();

                                    centroCusto.Insert(0, new CTB_CENTRO_CUSTO { CENTRO_CUSTO = "N", DESC_CENTRO_CUSTO = "NÃO IDENTIFICADO" });
                                    centroCusto.Insert(0, new CTB_CENTRO_CUSTO { CENTRO_CUSTO = "0", DESC_CENTRO_CUSTO = "" });

                                    _ddlCCusto.DataSource = centroCusto;
                                    _ddlCCusto.DataBind();
                                    if (_hdnCCusto != null)
                                    {
                                        _ddlCCusto.SelectedIndex = _ddlCCusto.Items.IndexOf(_ddlCCusto.Items.FindByText(_hdnCCusto.Value.ToString()));
                                    }
                                }
                            }

                            Literal _SalarioLiquido = e.Row.FindControl("litSalarioLiquido") as Literal;
                            if (_SalarioLiquido != null)
                            {
                                _SalarioLiquido.Text = FormatarValor(fcf.SALARIO_LIQUIDO);
                                totalSalarioLiquido += (string.IsNullOrEmpty(fcf.SALARIO_LIQUIDO.ToString().Trim()) ? 0 : Convert.ToDecimal(fcf.SALARIO_LIQUIDO));
                            }

                            Literal _VT = e.Row.FindControl("litBeneficioVT") as Literal;
                            if (_VT != null)
                            {
                                _VT.Text = FormatarValor(fcf.BENEFICIO_VT);
                                totalBeneficioVT += (string.IsNullOrEmpty(fcf.BENEFICIO_VT.ToString().Trim()) ? 0 : Convert.ToDecimal(fcf.BENEFICIO_VT));
                            }

                            HiddenField _CF = e.Row.FindControl("hdnCutoFixo") as HiddenField;
                            if (_CF != null)
                            {
                                _CF.Value = fcf.CUSTO_FIXO.ToString();

                                CheckBox _Chk = e.Row.FindControl("chkCutoFixo") as CheckBox;
                                if (_Chk != null)
                                {
                                    if (_CF.Value.ToString().Trim() == "S")
                                    {
                                        _Chk.Checked = true;
                                    }
                                    else if (_CF.Value.ToString().Trim() == "T")
                                    {
                                        _Chk.Checked = true;
                                        e.Row.BackColor = Color.Cyan;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message + "\n" + ex.StackTrace;
                labErro.Visible = true;
            }
        }

        protected void gvCentroCustoFuncionario_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvCentroCustoFuncionario.FooterRow;

            if (footer != null)
            {
                footer.Cells[0].Text = "Localizado  " + Convert.ToString(gvCentroCustoFuncionario.Rows.Count) + "  Registros";
                footer.Cells[0].HorizontalAlign = HorizontalAlign.Left;
                footer.Cells[4].Text = "Totais";
                footer.Cells[4].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[5].Text = totalSalarioLiquido.ToString("N2");
                footer.Cells[5].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[6].Text = totalBeneficioVT.ToString("N2");
                footer.Cells[6].BackColor = System.Drawing.Color.PeachPuff;
            }
        }

        protected void btGravar_Click(object sender, EventArgs e)
        {
            limparMensagens();

            try
            {
                if (gvCentroCustoFuncionario.Rows.Count == 0)
                {
                    labErro.Text = "Não existem registros para receberem a alteração! Verifique o filtro...";
                    labErro.Visible = true;
                    return;
                }

                FUNCIONARIO_CUSTOFIXO_AUX FUNCIONARIO_CF_AUX = new FUNCIONARIO_CUSTOFIXO_AUX();
                FUNCIONARIO_CUSTOFIXO_HIST FUNCIONARIO_CF_HIST = new FUNCIONARIO_CUSTOFIXO_HIST();
                FUNCIONARIO_CENTROCUSTO_MESANO FUNCIONARIO_CC = new FUNCIONARIO_CENTROCUSTO_MESANO();

                Int32 Mes;
                int Ano;
                Boolean bemailOK = false;

                Mes = Convert.ToInt32(ddlMesAno.SelectedItem.Text.Split('/')[0]);
                Ano = Convert.ToInt32(ddlMesAno.SelectedItem.Text.Split('/')[1]);

                sbCustoFixo_Acrescentados.Clear();
                sbCustoFixo_Retirados.Clear();

                foreach (GridViewRow row in gvCentroCustoFuncionario.Rows)
                {
                    HiddenField _CPF = (HiddenField)row.FindControl("hdnFuncionario");
                    HiddenField _GrupoCCustoDescricao = (HiddenField)row.FindControl("hdnDescricaoCentroCustoGrupo");
                    HiddenField _GrupoCCusto = (HiddenField)row.FindControl("hdnCentroCustoGrupo");
                    HiddenField _CCustoDescricao = (HiddenField)row.FindControl("hdnDescricaoCentroCusto");
                    HiddenField _CCusto = (HiddenField)row.FindControl("hdnCentroCusto");
                    DropDownList _ddlCCusto = (DropDownList)row.FindControl("ddlCentroCusto");
                    DropDownList _ddlCCustoGrupo = (DropDownList)row.FindControl("ddlCentroCustoGrupo");

                    CheckBox chk = (CheckBox)row.FindControl("chkCutoFixo");

                    if (chk != null)
                    {
                        HiddenField _CutoFixo = (HiddenField)row.FindControl("hdnCutoFixo");

                        if ((_CutoFixo.Value == "N" && chk.Checked == true) || (_CutoFixo.Value == "T" && chk.Checked == true) || (_CutoFixo.Value == "S" && chk.Checked == false) || (_CutoFixo.Value == "T" && chk.Checked == false) || (_CutoFixo.Value == "S" && chk.Checked == true))
                        {
                            Literal _Funcionario = (Literal)row.FindControl("litFuncionario");
                            Literal _FilialDescricao = (Literal)row.FindControl("litFilial");
                            Literal _CargoDescricao = (Literal)row.FindControl("litCargo");

                            if (_CPF != null)
                            {
                                FUNCIONARIO_CF_AUX.CPF = _CPF.Value.ToString().Trim();
                            }
                            FUNCIONARIO_CF_AUX.MES = Convert.ToByte(Mes);
                            FUNCIONARIO_CF_AUX.ANO = Convert.ToInt16(Ano);
                            FUNCIONARIO_CF_AUX.CODIGO_USUARIO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                            if (_GrupoCCusto != null && _CCusto != null)
                            {
                                FUNCIONARIO_CF_AUX.CCUSTO_ANTERIOR = _GrupoCCusto.Value.ToString().Trim() + '.' + _CCusto.Value.ToString().Trim();
                            }
                            FUNCIONARIO_CF_AUX.DTINCLUSAO = baseController.BuscaDataBanco().data;

                            FUNCIONARIO_CF_HIST.CPF = FUNCIONARIO_CF_AUX.CPF;
                            FUNCIONARIO_CF_HIST.MES = Convert.ToByte(Mes);
                            FUNCIONARIO_CF_HIST.ANO = Convert.ToInt16(Ano);
                            FUNCIONARIO_CF_HIST.CODIGO_USUARIO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                            FUNCIONARIO_CF_HIST.DTINCLUSAO = baseController.BuscaDataBanco().data;

                            if (chk.Checked)
                            {
                                if ((_CutoFixo.Value == "N") || _CutoFixo.Value == "T" || _CutoFixo.Value == "S")
                                {
                                    FUNCIONARIO_CUSTOFIXO_AUX _FUNCIONARIO_CF_AUX = contabilidadeController.ObterFUNCIONARIO_CUSTOFIXO_AUX(FUNCIONARIO_CF_AUX);
                                    if (_FUNCIONARIO_CF_AUX == null)
                                    {
                                        contabilidadeController.IncluirFUNCIONARIO_CUSTOFIXO_AUX(FUNCIONARIO_CF_AUX);
                                        FUNCIONARIO_CF_HIST.CUSTO_FIXO = 'S';
                                        contabilidadeController.IncluirFUNCIONARIO_CUSTOFIXO_HIST(FUNCIONARIO_CF_HIST);

                                        if (_CutoFixo.Value == "N")
                                        {
                                            sbCustoFixo_Acrescentados.Append(String.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td></tr>", _Funcionario.Text.Trim(), _CPF.Value.ToString().Trim(), _FilialDescricao.Text.Trim(), _CargoDescricao.Text.Trim(), _GrupoCCustoDescricao.Value.Trim() + " - " + _CCustoDescricao.Value.Trim()));
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if ((_CutoFixo.Value == "S") || _CutoFixo.Value == "T")
                                {
                                    if ((_CutoFixo.Value == "S"))
                                    {
                                        contabilidadeController.ExcluirFUNCIONARIO_CUSTOFIXO_AUX(FUNCIONARIO_CF_AUX);
                                    }

                                    sbCustoFixo_Retirados.Append(String.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td></tr>", _Funcionario.Text.Trim(), _CPF.Value.ToString().Trim(), _FilialDescricao.Text.Trim(), _CargoDescricao.Text.Trim(), _GrupoCCustoDescricao.Value.Trim() + " - " + _CCustoDescricao.Value.Trim()));
                                    FUNCIONARIO_CF_HIST.CUSTO_FIXO = 'N';
                                    contabilidadeController.IncluirFUNCIONARIO_CUSTOFIXO_HIST(FUNCIONARIO_CF_HIST);
                                }
                            }
                        }
                    }

                    if (_ddlCCusto != null && _ddlCCustoGrupo != null && _CPF != null && _GrupoCCusto != null && _CCusto != null)
                    {
                        if (_ddlCCustoGrupo.SelectedItem.ToString() != "NÃO IDENTIFICADO" && _ddlCCusto.SelectedItem.ToString() != "NÃO IDENTIFICADO")
                        {
                            FUNCIONARIO_CC.CPF = _CPF.Value.ToString().Trim();
                            FUNCIONARIO_CC.MES = Convert.ToByte(Mes);
                            FUNCIONARIO_CC.ANO = Convert.ToInt16(Ano);
                            FUNCIONARIO_CC.CODIGO_USUARIO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                            FUNCIONARIO_CC.DTINCLUSAO = baseController.BuscaDataBanco().data;
                            FUNCIONARIO_CC.CENTRO_CUSTO = _ddlCCustoGrupo.SelectedValue.ToString().Trim() + '.' + _ddlCCusto.SelectedValue.ToString().Trim();

                            FUNCIONARIO_CENTROCUSTO_MESANO _FUNCIONARIO_CC = contabilidadeController.ObterFUNCIONARIO_CENTROCUSTO_MESANO(FUNCIONARIO_CC);
                            if (_FUNCIONARIO_CC != null)
                            {
                                contabilidadeController.ExcluirFUNCIONARIO_CENTROCUSTO_MESANO(_FUNCIONARIO_CC);
                            }

                            contabilidadeController.IncluirFUNCIONARIO_CENTROCUSTO_MESANO(FUNCIONARIO_CC);

                            int iMes = Mes + 1;

                            for (int i = iMes; iMes <= 12; i++)
                            {
                                FUNCIONARIO_CC.CPF = _CPF.Value.ToString().Trim();
                                FUNCIONARIO_CC.MES = Convert.ToByte(iMes);
                                FUNCIONARIO_CC.ANO = Convert.ToInt16(Ano);
                                FUNCIONARIO_CC.CODIGO_USUARIO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                                FUNCIONARIO_CC.DTINCLUSAO = baseController.BuscaDataBanco().data;
                                FUNCIONARIO_CC.CENTRO_CUSTO = _ddlCCustoGrupo.SelectedValue.ToString().Trim() + '.' + _ddlCCusto.SelectedValue.ToString().Trim();

                                FUNCIONARIO_CENTROCUSTO_MESANO _FUNCIONARIO_CC_Posteriores = contabilidadeController.ObterFUNCIONARIO_CENTROCUSTO_MESANO(FUNCIONARIO_CC);
                                if (_FUNCIONARIO_CC_Posteriores == null)
                                {
                                    contabilidadeController.IncluirFUNCIONARIO_CENTROCUSTO_MESANO(FUNCIONARIO_CC);
                                }

                                iMes++;
                            }
                        }
                    }
                }

                if ((Ano > Convert.ToDateTime(baseController.BuscaDataBanco().data).Year) || ((Ano == Convert.ToDateTime(baseController.BuscaDataBanco().data).Year) && (Mes >= Convert.ToDateTime(baseController.BuscaDataBanco().data).Month)))
                {
                    if (sbCustoFixo_Acrescentados.ToString().Trim() != "" || sbCustoFixo_Retirados.ToString().Trim() != "")
                    {
                        EnviarEmail();
                        bemailOK = true;
                    }
                }

                labSucesso.Text = "Processo Finalizado com Sucesso !!" + (bemailOK == true ? " e-mail Enviado com Sucesso !!" : "");
                labSucesso.Visible = true;
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message + "\n" + ex.StackTrace;
                labErro.Visible = true;
            }
        }

        private void EnviarEmail()
        {
            USUARIO usuario = (USUARIO)Session["USUARIO"];

            email_envio email = new email_envio();
            email.ASSUNTO = "Intranet:  Definição de Custo Fixo - DRE";
            email.REMETENTE = usuario;
            email.MENSAGEM = MontarCorpoEmail();

            List<string> destinatario = new List<string>();
            //Adiciona e-mails RH
            var usuarioEmail = new UsuarioController().ObterEmailUsuarioTela(9, 12).Where(p => p.EMAIL != null && p.EMAIL.Trim() != "").ToList();
            foreach (var usu in usuarioEmail)
                if (usu != null)
                    destinatario.Add(usu.EMAIL);
            //Adicionar remetente
            destinatario.Add(usuario.EMAIL);

            email.DESTINATARIOS = destinatario;

            if (destinatario.Count > 0)
                email.EnviarEmail();
        }
        private string MontarCorpoEmail()
        {
            USUARIO usuario = (USUARIO)Session["USUARIO"];

            StringBuilder sb = new StringBuilder();
            sb.Append("");

            sb.Append("<!DOCTYPE HTML PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            sb.Append("<html>");
            sb.Append("<head>");
            sb.Append("    <title>Definição de Custo Fixo - DRE</title>");
            sb.Append("    <meta charset='UTF-8' />");
            sb.Append("</head>");
            sb.Append("<body>");
            sb.Append("    <div style='color: black; font-size: 10.2pt; font-weight: 700; font-family: Arial, sans-serif;");
            sb.Append("        background: white; white-space: nowrap;'>");
            sb.Append("        <br />");
            sb.Append("        <div id='divPrincipal' align='left'>");
            sb.Append("            <table border='0' cellpadding='0' cellspacing='0' width='100%' style='padding: 0px;");
            sb.Append("                color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
            sb.Append("                background: white; white-space: nowrap;'>");
            sb.Append("                <tr style='text-align: left;'>");
            sb.Append("                    <td>");
            sb.Append("                        Lista de funcionários que sofreram alteração de Centro de Custo referente à Custo Fixo na data " + DateTime.Today.ToShortDateString());
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("                <tr>");
            sb.Append("                    <td>");
            sb.Append("                        </ br>");
            sb.Append("                    </td>");
            sb.Append("                </tr>");

            if (sbCustoFixo_Acrescentados.ToString().Trim() != "")
            {
                sb.Append("                <tr>");
                sb.Append("                    <td>");
                sb.Append("                        </ br>");
                sb.Append("                    </td>");
                sb.Append("                </tr>");
                sb.Append("                <tr>");
                sb.Append("                    <td align='center'>");
                sb.Append("                        Funcionários que foram acrescentados no Custo Fixo");
                sb.Append("                    </td>");
                sb.Append("                </tr>");
                sb.Append("                <tr>");
                sb.Append("                    <td>");
                sb.Append("                        <table cellpadding='2' cellspacing='2' width='100%' style='padding: 0px; color: black;");
                sb.Append("                            font-size: 8.0pt; font-weight: 700; font-family: Arial, sans-serif; white-space: nowrap;");
                sb.Append("                            border: 1px solid #ccc;'>");
                sb.Append("                            <tr style='background-color: #ccc;'>");
                sb.Append("                                <td width='25%' style='text-align: center;'>");
                sb.Append("                                    Funcionário");
                sb.Append("                                </td>");
                sb.Append("                                <td width='10%' style='text-align: center;'>");
                sb.Append("                                    CPF");
                sb.Append("                                </td>");
                sb.Append("                                <td width='25%' style='text-align: center;'>");
                sb.Append("                                    Filial");
                sb.Append("                                </td>");
                sb.Append("                                <td width='20%' style='text-align: center;'>");
                sb.Append("                                    Cargo");
                sb.Append("                                </td>");
                sb.Append("                                <td width='20%' style='text-align: center;'>");
                sb.Append("                                    Centro Custo Atual");
                sb.Append("                                </td>");
                sb.Append("                            </tr>");
                sb.Append(sbCustoFixo_Acrescentados.ToString());
                sb.Append("                        </table>");
                sb.Append("                    </td>");
                sb.Append("                </tr>");
            }

            if (sbCustoFixo_Retirados.ToString().Trim() != "")
            {
                sb.Append("                <tr>");
                sb.Append("                    <td>");
                sb.Append("                        </ br>");
                sb.Append("                    </td>");
                sb.Append("                </tr>");
                sb.Append("                <tr>");
                sb.Append("                    <td align='center'>");
                sb.Append("                        Funcionários que foram retirados do Custo Fixo");
                sb.Append("                    </td>");
                sb.Append("                </tr>");
                sb.Append("                <tr>");
                sb.Append("                    <td>");
                sb.Append("                        <table cellpadding='2' cellspacing='2' width='100%' style='padding: 0px; color: black;");
                sb.Append("                            font-size: 8.0pt; font-weight: 700; font-family: Arial, sans-serif; white-space: nowrap;");
                sb.Append("                            border: 1px solid #ccc;'>");
                sb.Append("                            <tr style='background-color: #ccc;'>");
                sb.Append("                                <td width='25%' style='text-align: center;'>");
                sb.Append("                                    Funcionário");
                sb.Append("                                </td>");
                sb.Append("                                <td width='10%' style='text-align: center;'>");
                sb.Append("                                    CPF");
                sb.Append("                                </td>");
                sb.Append("                                <td width='25%' style='text-align: center;'>");
                sb.Append("                                    Filial");
                sb.Append("                                </td>");
                sb.Append("                                <td width='20%' style='text-align: center;'>");
                sb.Append("                                    Cargo");
                sb.Append("                                </td>");
                sb.Append("                                <td width='20%' style='text-align: center;'>");
                sb.Append("                                    Centro Custo Atual");
                sb.Append("                                </td>");
                sb.Append("                            </tr>");
                sb.Append(sbCustoFixo_Retirados.ToString());
                sb.Append("                        </table>");
                sb.Append("                    </td>");
                sb.Append("                </tr>");
            }
            sb.Append("            </table>");
            sb.Append("        </div>");

            sb.Append("        <br />");
            sb.Append("        <br />");

            sb.Append("        <span>Enviado por: " + (new BaseController().BuscaUsuario(usuario.CODIGO_USUARIO).NOME_USUARIO.ToUpper()) + "</span>");
            sb.Append("    </div>");
            sb.Append("</body>");
            sb.Append("</html>");

            return sb.ToString();
        }

        protected void chkTodos_CheckedChanged(object sender, EventArgs e)
        {
            limparMensagens();

            foreach (GridViewRow row in gvCentroCustoFuncionario.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("chkCutoFixo");

                if (chk != null)
                {
                    chk.Checked = (sender as CheckBox).Checked;
                }
            }
        }

        protected void ddlCentroCustoGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList _ddl = (DropDownList)sender;
            GridViewRow rowgrid = (GridViewRow)_ddl.NamingContainer;

            DropDownList _ddlCCusto = (DropDownList)rowgrid.FindControl("ddlCentroCusto");
            DropDownList _ddlCCustoGrupo = (DropDownList)rowgrid.FindControl("ddlCentroCustoGrupo");

            if (_ddlCCusto != null && _ddlCCustoGrupo != null)
            {
                var centroCusto = dreController.ObterCentroCusto().Where(p => !p.INATIVA).ToList();
                if (centroCusto != null)
                {
                    centroCusto = centroCusto.Where(p => p.ID_GRUPO_CENTRO_CUSTO == Convert.ToInt32(_ddlCCustoGrupo.SelectedValue)).OrderBy(x => x.DESC_CENTRO_CUSTO).ToList();

                    centroCusto.Insert(0, new CTB_CENTRO_CUSTO { CENTRO_CUSTO = "N", DESC_CENTRO_CUSTO = "NÃO IDENTIFICADO" });
                    centroCusto.Insert(0, new CTB_CENTRO_CUSTO { CENTRO_CUSTO = "0", DESC_CENTRO_CUSTO = "" });

                    _ddlCCusto.DataSource = centroCusto;
                    _ddlCCusto.DataBind();
                }
            }
        }

        protected void ddlCentroCusto_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList _ddl = (DropDownList)sender;
            GridViewRow rowgrid = (GridViewRow)_ddl.NamingContainer;
            HiddenField _hdnCCusto = (HiddenField)rowgrid.FindControl("hdnCentroCusto");

            if (_hdnCCusto.Value.ToString() == "N")
            {
                if (Convert.ToString(_ddl.SelectedValue) != "N")
                {
                    rowgrid.Cells[0].BackColor = Color.LightGreen;
                    rowgrid.Cells[1].BackColor = Color.LightGreen;
                }
                else
                {
                    rowgrid.Cells[0].BackColor = Color.Empty;
                    rowgrid.Cells[1].BackColor = Color.Empty;
                }
            }
            else
            {
                if (Convert.ToInt32(_ddl.SelectedValue) != Convert.ToInt32(_hdnCCusto.Value))
                {
                    rowgrid.Cells[0].BackColor = Color.LightGreen;
                    rowgrid.Cells[1].BackColor = Color.LightGreen;
                }
                else
                {
                    rowgrid.Cells[0].BackColor = Color.Empty;
                    rowgrid.Cells[1].BackColor = Color.Empty;
                }
            }
        }

        protected void gvCentroCustoFuncionario_Sorting(object sender, GridViewSortEventArgs e)
        {
            Int32 Mes;
            int Ano;
            int? centroCustoGrupo = null;
            string centroCusto = "";
            string filial = "";
            IEnumerable<SP_FUNCIONARIO_CFResult> gCCFuncionario = new List<SP_FUNCIONARIO_CFResult>();

            Mes = Convert.ToInt32(ddlMesAno.SelectedItem.Text.Split('/')[0]);
            Ano = Convert.ToInt32(ddlMesAno.SelectedItem.Text.Split('/')[1]);

            //Obter Grupo Centro de Custo
            if (ddlCCustoGrupo.SelectedValue.Trim() != "" && ddlCCustoGrupo.SelectedValue.Trim() != "0")
                centroCustoGrupo = Convert.ToInt32(ddlCCustoGrupo.SelectedValue);

            //Obter Centro de Custo
            if (ddlCCusto.SelectedValue.Trim() != "" && ddlCCusto.SelectedValue.Trim() != "0")
                centroCusto = ddlCCusto.SelectedValue;

            //Obter Filial
            if (ddlFilial.SelectedValue.Trim() != "" && ddlFilial.SelectedValue.Trim() != "0")
                filial = ddlFilial.SelectedValue;

            gCCFuncionario = contabilidadeController.BuscaFuncionarioCustoFixo(Mes, Ano, centroCustoGrupo, centroCusto, filial);

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            gCCFuncionario = gCCFuncionario.OrderBy(e.SortExpression + sortDirection);
            gvCentroCustoFuncionario.DataSource = gCCFuncionario;
            gvCentroCustoFuncionario.DataBind();
        }
    }
}