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

namespace Relatorios
{
    public partial class fisc_apuracao_imposto : System.Web.UI.Page
    {
        ContabilidadeController contabilController = new ContabilidadeController();
        BaseController baseController = new BaseController();

        decimal tValorTotal = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Valida queryString
                if (Request.QueryString["t"] == null || Request.QueryString["t"] == "")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                string tela = Request.QueryString["t"].ToString();
                if (tela != "1" && tela != "2")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                if (tela == "1")
                    hrefVoltar.HRef = "fisc_menu.aspx";

                if (tela == "2")
                    hrefVoltar.HRef = "../mod_contabilidade/contabil_menu.aspx";


                hidCodigoPerfilUsuario.Value = ((USUARIO)Session["USUARIO"]).CODIGO_PERFIL.ToString();

                CarregarDataAno();
                CarregarEmpresa();
                CarregarImposto();

                btSair.Visible = false;
            }

            //Evitar duplo clique no botão
            btGerar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btGerar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarDataAno()
        {
            var dataAno = baseController.ObterDataAno();
            if (dataAno != null)
            {
                dataAno = dataAno.Where(p => p.STATUS == 'A').ToList();
                ddlAno.DataSource = dataAno;
                ddlAno.DataBind();

                if (ddlAno.Items.Count > 0)
                {
                    try
                    {
                        ddlAno.SelectedValue = DateTime.Now.Year.ToString();
                    }
                    catch (Exception) { }
                }
            }
        }
        private void CarregarEmpresa()
        {
            var empresa = baseController.ObterEmpresa().OrderBy(p => p.NOME).ToList();

            empresa.Insert(0, new EMPRESA { CODIGO_EMPRESA = 0, NOME = "Selecione" });
            ddlEmpresa.DataSource = empresa;
            ddlEmpresa.DataBind();
        }
        private void CarregarImposto()
        {
            var imposto = contabilController.ObterImpostoTipo();

            imposto.Insert(0, new CTB_IMPOSTO_TIPO { TIPO = "Selecione" });
            ddlImposto.DataSource = imposto;
            ddlImposto.DataBind();
        }

        private void CarregarItem(int apuracaoTipo, DropDownList ddlItem)
        {
            string empresa = "";
            var item = contabilController.ObterApuracaoItem(apuracaoTipo);

            empresa = ddlEmpresa.SelectedItem.Text.Trim();
            if (apuracaoTipo == 2)
            {
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
            }

            item.Insert(0, new CTB_APURACAO_ITEM { CODIGO = 0, ITEM = "Selecione" });
            ddlItem.DataSource = item;
            ddlItem.DataBind();
        }

        private bool ValidarCampos()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            labEmpresa.ForeColor = _OK;
            if (ddlEmpresa.SelectedValue.Trim() == "0")
            {
                labEmpresa.ForeColor = _notOK;
                retorno = false;
            }

            labMes.ForeColor = _OK;
            if (ddlMes.SelectedValue.Trim() == "")
            {
                labMes.ForeColor = _notOK;
                retorno = false;
            }

            return retorno;
        }
        private void HabilitarCampos(bool enable)
        {
            gvReceita.Enabled = enable;
            gvExclusao.Enabled = enable;
            gvIsencao.Enabled = enable;
            gvCredito.Enabled = enable;
            btIncluirReceita.Enabled = enable;
            btIncluirExclusao.Enabled = enable;
            btIncluirIsencao.Enabled = enable;
            btIncluirCredito.Enabled = enable;
        }
        #endregion

        #region "APURACAO"
        protected void btGerar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                int codigoPerfilUsuario = Convert.ToInt32(hidCodigoPerfilUsuario.Value);

                if (!ValidarCampos())
                {
                    labErro.Text = "Preencha os campos em vermelho.";
                    return;
                }

                int codigoApuracaoEmpresa = 0;
                DateTime data = DateTime.Now;
                string mes = ddlMes.SelectedValue;
                string ano = ddlAno.SelectedValue;

                string diaFim = DateTime.DaysInMonth(Convert.ToInt32(ano), Convert.ToInt32(mes)).ToString();

                data = Convert.ToDateTime(ano + "-" + mes + "-" + ((diaFim.Length == 1) ? ("0" + diaFim) : diaFim));

                CTB_APURACAO_EMPRESA apuracaoEmp = null;
                apuracaoEmp = contabilController.ObterApuracaoEmpresa(Convert.ToInt32(ddlEmpresa.SelectedValue), data);
                if (apuracaoEmp == null)
                {
                    apuracaoEmp = new CTB_APURACAO_EMPRESA();
                    apuracaoEmp.EMPRESA = Convert.ToInt32(ddlEmpresa.SelectedValue);
                    apuracaoEmp.COMPETENCIA = data;
                    apuracaoEmp.DATA_ALTERACAO = DateTime.Now;
                    apuracaoEmp.USUARIO_ALTERACAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                    codigoApuracaoEmpresa = contabilController.InserirApuracaoEmpresa(apuracaoEmp);
                }
                else
                {
                    codigoApuracaoEmpresa = apuracaoEmp.CODIGO;
                }
                hidCTBApuracaoEmpresa.Value = codigoApuracaoEmpresa.ToString();
                divApuracao.Visible = true;

                //insere faturamento antecipadamente
                HabilitarCampos(false);
                if (codigoPerfilUsuario == 1 || codigoPerfilUsuario == 13 || codigoPerfilUsuario == 24 || codigoPerfilUsuario == 54) //SOMENTE PERFIL DE CONTABILIDADE E FISCAL
                {
                    var empresa = ddlEmpresa.SelectedItem.Text.Trim();

                    InserirFaturamento(empresa, data);
                    InserirImposto(empresa, data);

                    InserirItemApuracao(empresa, data, 4, 23, true, "('1102', '1113', '1117', '1118', '1121', '1403', '2102', '2113', '2117', '2118', '2121', '2403')");
                    InserirItemApuracao(empresa, data, 4, 24, true, "('1101', '1111', '1116', '1120', '1122', '1124', '1125', '1401', '1933', '2101', '2111', '2116', '2120', '2122', '2124', '2125', '2401')");
                    InserirItemApuracao(empresa, data, 4, 37, false, "('5201', '5202', '5410', '5411', '6201', '6202', '6410', '6411')");
                    InserirItemApuracao(empresa, data, 4, 26, true, "('1252', '1253', '1257', '2252', '2253', '2257')");
                    InserirItemApuracao(empresa, data, 4, 31, true, "('1201', '1202', '1203', '1204', '1410', '1411', '2201', '2202', '2203', '2204', '2410', '2411')");
                    InserirItemApuracao(empresa, data, 4, 32, true, "('1206', '1352', '1353', '2206', '2352', '2353')");

                    InserirOutrasReceitasOpe(empresa, data);

                    InserirRegimeMonoFasico(empresa, data);

                    InserirItemCTAPF(empresa, data, 1);
                    InserirItemCTAPF(empresa, data, 2);
                    InserirItemCTAPF(empresa, data, 3);
                    InserirItemCTAPF(empresa, data, 4);
                    InserirItemCTAPF(empresa, data, 5);

                    HabilitarCampos(true);
                }

                ddlEmpresa.Enabled = false;
                ddlAno.Enabled = false;
                ddlMes.Enabled = false;
                btSair.Visible = true;

                tValorTotal = 0;
                CarregarItem(1, ddlReceitaItem);
                CarregarApuracao(1, gvReceita);

                tValorTotal = 0;
                CarregarItem(2, ddlExclusaoItem);
                CarregarApuracao(2, gvExclusao);

                tValorTotal = 0;
                CarregarItem(3, ddlIsencaoItem);
                CarregarApuracao(3, gvIsencao);

                tValorTotal = 0;
                CarregarItem(4, ddlCreditoItem);
                CarregarApuracao(4, gvCredito);
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message + "\n" + ex.StackTrace;
            }
        }
        protected void btSair_Click(object sender, EventArgs e)
        {
            hidCTBApuracaoEmpresa.Value = "";
            divApuracao.Visible = false;

            ddlEmpresa.Enabled = true;
            ddlAno.Enabled = true;
            ddlMes.Enabled = true;
            btSair.Visible = false;
        }

        private void CarregarApuracao(int ctbApuracaoItem, GridView gvApuracao)
        {
            var apuracao = contabilController.ObterApuracaoEmpresaItem(Convert.ToInt32(hidCTBApuracaoEmpresa.Value), ctbApuracaoItem);

            gvApuracao.DataSource = apuracao;
            gvApuracao.DataBind();
        }
        protected void gvApuracao_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    CTB_APURACAO_EMPRESA_ITEM apuracaoImp = e.Row.DataItem as CTB_APURACAO_EMPRESA_ITEM;

                    if (apuracaoImp != null)
                    {
                        Literal _litSinal = e.Row.FindControl("litSinal") as Literal;
                        if (_litSinal != null)
                        {
                            if (apuracaoImp.CTB_APURACAO_ITEM1.CTB_APURACAO_TIPO1.CODIGO == 1)
                                _litSinal.Text = "(+)";
                            else
                                _litSinal.Text = "(-)";
                        }

                        Literal _litItem = e.Row.FindControl("litItem") as Literal;
                        if (_litItem != null)
                            _litItem.Text = apuracaoImp.CTB_APURACAO_ITEM1.ITEM;

                        Literal _litValor = e.Row.FindControl("litValor") as Literal;
                        if (_litValor != null)
                            _litValor.Text = "R$  " + apuracaoImp.VALOR.ToString("###,###,###,##0.00");

                        tValorTotal += apuracaoImp.VALOR;

                        ImageButton _btExcluir = e.Row.FindControl("btExcluir") as ImageButton;
                        if (_btExcluir != null)
                            _btExcluir.CommandArgument = apuracaoImp.CODIGO.ToString();

                    }
                }
            }
        }
        protected void gvApuracao_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = ((GridView)sender).FooterRow;
            if (footer != null)
            {
                footer.Cells[2].Text = "Total";
                footer.Cells[3].Text = "R$  " + tValorTotal.ToString("###,###,###,##0.00");
            }
        }

        #endregion

        #region "RECEITAS"
        protected void btIncluirReceita_Click(object sender, EventArgs e)
        {
            try
            {
                labErroReceita.Text = "";

                if (!ValidarCampos(labReceita, ddlReceitaItem, null))
                {
                    labErroReceita.Text = "Selecione o Item de Receita.";
                    return;
                }
                if (!ValidarCampos(labReceitaValor, txtReceitaValor, null))
                {
                    labErroReceita.Text = "Informe o Valor de Receita.";
                    return;
                }

                if (!ValidarItemDuplicado(Convert.ToInt32(hidCTBApuracaoEmpresa.Value), 1, Convert.ToInt32(ddlReceitaItem.SelectedValue)))
                {
                    labErroReceita.Text = "Este Item já foi Inserido.";
                    return;
                }

                CTB_APURACAO_EMPRESA_ITEM apuracaoEmpresaItem = new CTB_APURACAO_EMPRESA_ITEM();
                apuracaoEmpresaItem.CTB_APURACAO_EMPRESA = Convert.ToInt32(hidCTBApuracaoEmpresa.Value);
                apuracaoEmpresaItem.CTB_APURACAO_ITEM = Convert.ToInt32(ddlReceitaItem.SelectedValue);
                apuracaoEmpresaItem.VALOR = Convert.ToDecimal(txtReceitaValor.Text);
                apuracaoEmpresaItem.DATA_INCLUSAO = DateTime.Now;
                apuracaoEmpresaItem.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

                contabilController.InserirApuracaoEmpresaItem(apuracaoEmpresaItem);
                tValorTotal = 0;
                CarregarApuracao(1, gvReceita);

                ddlReceitaItem.SelectedValue = "0";
                txtReceitaValor.Text = "";
            }
            catch (Exception ex)
            {
                labErroReceita.Text = ex.Message;
            }
        }

        private void InserirFaturamento(string empresa, DateTime dataFim)
        {
            decimal valorFatOperacional = 0;

            if (ValidarItemDuplicado(Convert.ToInt32(hidCTBApuracaoEmpresa.Value), 1, 1))
            {
                var fo = contabilController.ObterFaturamentoDesoneracao(empresa, Convert.ToDateTime((dataFim.ToString("yyyy-MM") + "-01")), dataFim, "");
                if (fo != null && fo.Count() > 0)
                {
                    valorFatOperacional = fo.Sum(p => p.FAT_OPERACIONAL);

                    CTB_APURACAO_EMPRESA_ITEM apuracaoEmpresaItem = new CTB_APURACAO_EMPRESA_ITEM();
                    apuracaoEmpresaItem.CTB_APURACAO_EMPRESA = Convert.ToInt32(hidCTBApuracaoEmpresa.Value);
                    apuracaoEmpresaItem.CTB_APURACAO_ITEM = 1;
                    apuracaoEmpresaItem.VALOR = valorFatOperacional;
                    apuracaoEmpresaItem.DATA_INCLUSAO = DateTime.Now;
                    apuracaoEmpresaItem.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

                    contabilController.InserirApuracaoEmpresaItem(apuracaoEmpresaItem);
                }
            }
        }
        private void InserirImposto(string empresa, DateTime dataFim)
        {
            decimal valorImpostoICMSOperacional = 0;
            int codigoApuracaoItem = 0;

            empresa = empresa.Trim();
            if (empresa == "HANDBOOK STORE CONFECCOES LTDA")
                codigoApuracaoItem = 39;
            if (empresa == "HB GROUP CONFECCOES LTDA")
                codigoApuracaoItem = 40;
            if (empresa == "HBF IMPORTADORA LTDA")
                codigoApuracaoItem = 41;
            if (empresa == "MEGA BITE CONFECÇÕES LTDA")
                codigoApuracaoItem = 42;
            if (empresa == "TAGZY CONFECCOES LTDA")
                codigoApuracaoItem = 43;
            if (empresa == "YUGZY CONFECCOES LTDA")
                codigoApuracaoItem = 44;
            if (empresa == "GABAZEITU COMERCIO DE CONFECCOES EIRELI - EPP")
                codigoApuracaoItem = 45;
            if (empresa == "C-MAX CLOTHES AND HAPPINESS CONFECÇÕES LTDA")
                codigoApuracaoItem = 46;

            if (ValidarItemDuplicado(Convert.ToInt32(hidCTBApuracaoEmpresa.Value), 2, codigoApuracaoItem))
            {
                var fo = contabilController.ObterImpostoFatICMS(empresa, Convert.ToDateTime((dataFim.ToString("yyyy-MM") + "-01")), dataFim);
                if (fo != null && fo.Count() > 0)
                {
                    valorImpostoICMSOperacional = Convert.ToDecimal(fo.Sum(p => p.VALOR));


                    if (codigoApuracaoItem > 0)
                    {
                        CTB_APURACAO_EMPRESA_ITEM apuracaoEmpresaItem = new CTB_APURACAO_EMPRESA_ITEM();
                        apuracaoEmpresaItem.CTB_APURACAO_EMPRESA = Convert.ToInt32(hidCTBApuracaoEmpresa.Value);
                        apuracaoEmpresaItem.CTB_APURACAO_ITEM = codigoApuracaoItem;
                        apuracaoEmpresaItem.VALOR = valorImpostoICMSOperacional;
                        apuracaoEmpresaItem.DATA_INCLUSAO = DateTime.Now;
                        apuracaoEmpresaItem.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

                        contabilController.InserirApuracaoEmpresaItem(apuracaoEmpresaItem);
                    }
                }
            }
        }
        private void InserirItemApuracao(string empresa, DateTime dataFim, int codigoApuracaoTipo, int codigoApuracaoItem, bool entrada, string inSQLCFOP)
        {
            decimal valorBaseImposto = 0;

            empresa = empresa.Trim();

            if (ValidarItemDuplicado(Convert.ToInt32(hidCTBApuracaoEmpresa.Value), codigoApuracaoTipo, codigoApuracaoItem))
            {
                var fo = contabilController.ObterBaseImpostoCFOP(empresa, Convert.ToDateTime((dataFim.ToString("yyyy-MM") + "-01")), dataFim, entrada, inSQLCFOP);
                if (fo != null && fo.Count() > 0)
                {
                    valorBaseImposto = Convert.ToDecimal(fo.Sum(p => p.BASE_IMPOSTO));

                    if (codigoApuracaoItem == 37)
                        valorBaseImposto = (valorBaseImposto * (-1));

                    CTB_APURACAO_EMPRESA_ITEM apuracaoEmpresaItem = new CTB_APURACAO_EMPRESA_ITEM();
                    apuracaoEmpresaItem.CTB_APURACAO_EMPRESA = Convert.ToInt32(hidCTBApuracaoEmpresa.Value);
                    apuracaoEmpresaItem.CTB_APURACAO_ITEM = codigoApuracaoItem;
                    apuracaoEmpresaItem.VALOR = valorBaseImposto;
                    apuracaoEmpresaItem.DATA_INCLUSAO = DateTime.Now;
                    apuracaoEmpresaItem.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

                    contabilController.InserirApuracaoEmpresaItem(apuracaoEmpresaItem);

                }
            }
        }
        private void InserirOutrasReceitasOpe(string empresa, DateTime dataFim)
        {
            decimal valorNaoOperacional = 0;
            int codigoApuracaoItem = 8;

            if (ValidarItemDuplicado(Convert.ToInt32(hidCTBApuracaoEmpresa.Value), 1, codigoApuracaoItem))
            {
                var fo = contabilController.ObterFaturamentoDesoneracao(empresa, Convert.ToDateTime((dataFim.ToString("yyyy-MM") + "-01")), dataFim, "");
                if (fo != null && fo.Count() > 0)
                {
                    valorNaoOperacional = Convert.ToDecimal(fo.Sum(p => p.FAT_N_OPERACIONAL));

                    if (codigoApuracaoItem > 0)
                    {
                        CTB_APURACAO_EMPRESA_ITEM apuracaoEmpresaItem = new CTB_APURACAO_EMPRESA_ITEM();
                        apuracaoEmpresaItem.CTB_APURACAO_EMPRESA = Convert.ToInt32(hidCTBApuracaoEmpresa.Value);
                        apuracaoEmpresaItem.CTB_APURACAO_ITEM = codigoApuracaoItem;
                        apuracaoEmpresaItem.VALOR = valorNaoOperacional;
                        apuracaoEmpresaItem.DATA_INCLUSAO = DateTime.Now;
                        apuracaoEmpresaItem.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

                        contabilController.InserirApuracaoEmpresaItem(apuracaoEmpresaItem);
                    }
                }
            }
        }
        private void InserirRegimeMonoFasico(string empresa, DateTime dataFim)
        {
            decimal valorRegimeMonofasico = 0;
            int codigoApuracaoItem = 12;

            if (ValidarItemDuplicado(Convert.ToInt32(hidCTBApuracaoEmpresa.Value), 2, codigoApuracaoItem))
            {
                var fo = contabilController.ObterRegimoMonofasicoValor(empresa, Convert.ToDateTime((dataFim.ToString("yyyy-MM") + "-01")), dataFim);
                if (fo != null)
                {
                    valorRegimeMonofasico = Convert.ToDecimal(fo.VALOR_CONTABIL);

                    if (codigoApuracaoItem > 0)
                    {
                        CTB_APURACAO_EMPRESA_ITEM apuracaoEmpresaItem = new CTB_APURACAO_EMPRESA_ITEM();
                        apuracaoEmpresaItem.CTB_APURACAO_EMPRESA = Convert.ToInt32(hidCTBApuracaoEmpresa.Value);
                        apuracaoEmpresaItem.CTB_APURACAO_ITEM = codigoApuracaoItem;
                        apuracaoEmpresaItem.VALOR = valorRegimeMonofasico;
                        apuracaoEmpresaItem.DATA_INCLUSAO = DateTime.Now;
                        apuracaoEmpresaItem.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

                        contabilController.InserirApuracaoEmpresaItem(apuracaoEmpresaItem);
                    }
                }
            }
        }
        private void InserirItemCTAPF(string empresa, DateTime dataFim, int item)
        {
            decimal valor = 0;
            int codigoApuracaoItem = 0;

            if (item == 1)
                codigoApuracaoItem = 6;
            else if (item == 2)
                codigoApuracaoItem = 38;
            else if (item == 3)
                codigoApuracaoItem = 4;
            else if (item == 4)
                codigoApuracaoItem = 2;
            else if (item == 5)
                codigoApuracaoItem = 3;

            if (ValidarItemDuplicado(Convert.ToInt32(hidCTBApuracaoEmpresa.Value), 1, codigoApuracaoItem))
            {
                var fo = contabilController.ObterValorCTAPF(empresa, Convert.ToDateTime((dataFim.ToString("yyyy-MM") + "-01")), dataFim, item);
                if (fo != null)
                {
                    valor = Convert.ToDecimal(fo.VALOR);

                    if (valor > 0)
                    {
                        CTB_APURACAO_EMPRESA_ITEM apuracaoEmpresaItem = new CTB_APURACAO_EMPRESA_ITEM();
                        apuracaoEmpresaItem.CTB_APURACAO_EMPRESA = Convert.ToInt32(hidCTBApuracaoEmpresa.Value);
                        apuracaoEmpresaItem.CTB_APURACAO_ITEM = codigoApuracaoItem;
                        apuracaoEmpresaItem.VALOR = valor;
                        apuracaoEmpresaItem.DATA_INCLUSAO = DateTime.Now;
                        apuracaoEmpresaItem.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

                        contabilController.InserirApuracaoEmpresaItem(apuracaoEmpresaItem);
                    }
                }
            }
        }
        #endregion

        #region "EXCLUSAO"
        protected void btIncluirExclusao_Click(object sender, EventArgs e)
        {
            try
            {
                labErroExclusao.Text = "";

                if (!ValidarCampos(labExclusao, ddlExclusaoItem, null))
                {
                    labErroExclusao.Text = "Selecione o Item de Exclusão.";
                    return;
                }
                if (!ValidarCampos(labExclusaoValor, txtExclusaoValor, null))
                {
                    labErroExclusao.Text = "Informe o Valor de Exclusão.";
                    return;
                }

                if (!ValidarItemDuplicado(Convert.ToInt32(hidCTBApuracaoEmpresa.Value), 2, Convert.ToInt32(ddlExclusaoItem.SelectedValue)))
                {
                    labErroExclusao.Text = "Este Item já foi Inserido.";
                    return;
                }

                CTB_APURACAO_EMPRESA_ITEM apuracaoEmpresaItem = new CTB_APURACAO_EMPRESA_ITEM();
                apuracaoEmpresaItem.CTB_APURACAO_EMPRESA = Convert.ToInt32(hidCTBApuracaoEmpresa.Value);
                apuracaoEmpresaItem.CTB_APURACAO_ITEM = Convert.ToInt32(ddlExclusaoItem.SelectedValue);
                apuracaoEmpresaItem.VALOR = Convert.ToDecimal(txtExclusaoValor.Text);
                apuracaoEmpresaItem.DATA_INCLUSAO = DateTime.Now;
                apuracaoEmpresaItem.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

                contabilController.InserirApuracaoEmpresaItem(apuracaoEmpresaItem);
                tValorTotal = 0;
                CarregarApuracao(2, gvExclusao);

                ddlExclusaoItem.SelectedValue = "0";
                txtExclusaoValor.Text = "";
            }
            catch (Exception ex)
            {
                labErroExclusao.Text = ex.Message;
            }
        }
        #endregion

        #region "ISENÇÃO"
        protected void btIncluirIsencao_Click(object sender, EventArgs e)
        {
            try
            {
                labErroIsencao.Text = "";

                if (!ValidarCampos(labIsencao, ddlIsencaoItem, null))
                {
                    labErroIsencao.Text = "Selecione o Item de Isenção.";
                    return;
                }
                if (!ValidarCampos(labIsencaoValor, txtIsencaoValor, null))
                {
                    labErroIsencao.Text = "Informe o Valor de Isenção.";
                    return;
                }

                if (!ValidarItemDuplicado(Convert.ToInt32(hidCTBApuracaoEmpresa.Value), 3, Convert.ToInt32(ddlIsencaoItem.SelectedValue)))
                {
                    labErroIsencao.Text = "Este Item já foi Inserido.";
                    return;
                }

                CTB_APURACAO_EMPRESA_ITEM apuracaoEmpresaItem = new CTB_APURACAO_EMPRESA_ITEM();
                apuracaoEmpresaItem.CTB_APURACAO_EMPRESA = Convert.ToInt32(hidCTBApuracaoEmpresa.Value);
                apuracaoEmpresaItem.CTB_APURACAO_ITEM = Convert.ToInt32(ddlIsencaoItem.SelectedValue);
                apuracaoEmpresaItem.VALOR = Convert.ToDecimal(txtIsencaoValor.Text);
                apuracaoEmpresaItem.DATA_INCLUSAO = DateTime.Now;
                apuracaoEmpresaItem.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

                contabilController.InserirApuracaoEmpresaItem(apuracaoEmpresaItem);
                tValorTotal = 0;
                CarregarApuracao(3, gvIsencao);

                ddlIsencaoItem.SelectedValue = "0";
                txtIsencaoValor.Text = "";
            }
            catch (Exception ex)
            {
                labErroIsencao.Text = ex.Message;
            }
        }
        #endregion

        #region "CRÉDITO"
        protected void btIncluirCredito_Click(object sender, EventArgs e)
        {
            try
            {
                labErroCredito.Text = "";

                if (!ValidarCampos(labCredito, ddlCreditoItem, null))
                {
                    labErroCredito.Text = "Selecione o Item de Crédito.";
                    return;
                }
                if (!ValidarCampos(labCreditoValor, txtCreditoValor, null))
                {
                    labErroCredito.Text = "Informe o Valor de Crédito.";
                    return;
                }

                if (!ValidarItemDuplicado(Convert.ToInt32(hidCTBApuracaoEmpresa.Value), 4, Convert.ToInt32(ddlCreditoItem.SelectedValue)))
                {
                    labErroCredito.Text = "Este Item já foi Inserido.";
                    return;
                }

                CTB_APURACAO_EMPRESA_ITEM apuracaoEmpresaItem = new CTB_APURACAO_EMPRESA_ITEM();
                apuracaoEmpresaItem.CTB_APURACAO_EMPRESA = Convert.ToInt32(hidCTBApuracaoEmpresa.Value);
                apuracaoEmpresaItem.CTB_APURACAO_ITEM = Convert.ToInt32(ddlCreditoItem.SelectedValue);
                apuracaoEmpresaItem.VALOR = Convert.ToDecimal(txtCreditoValor.Text);
                apuracaoEmpresaItem.DATA_INCLUSAO = DateTime.Now;
                apuracaoEmpresaItem.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

                contabilController.InserirApuracaoEmpresaItem(apuracaoEmpresaItem);
                tValorTotal = 0;
                CarregarApuracao(4, gvCredito);

                ddlCreditoItem.SelectedValue = "0";
                txtCreditoValor.Text = "";
            }
            catch (Exception ex)
            {
                labErroCredito.Text = ex.Message;
            }
        }
        #endregion

        protected void btExcluir_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton bt = (ImageButton)sender;
            if (bt != null)
            {
                try
                {
                    int codigoEmpresaItem = Convert.ToInt32(bt.CommandArgument);
                    contabilController.ExcluirApuracaoEmpresaItem(codigoEmpresaItem);

                    tValorTotal = 0;
                    CarregarApuracao(1, gvReceita);
                    tValorTotal = 0;
                    CarregarApuracao(2, gvExclusao);
                    tValorTotal = 0;
                    CarregarApuracao(3, gvIsencao);
                    tValorTotal = 0;
                    CarregarApuracao(4, gvCredito);
                }
                catch (Exception ex)
                {
                }

            }

        }
        private bool ValidarCampos(Label _labCampo, object _objCampo, Type _type)
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            if (_objCampo.GetType() == typeof(DropDownList))
            {
                _labCampo.ForeColor = _OK;
                if (((DropDownList)_objCampo).SelectedValue.Trim() == "0")
                {
                    _labCampo.ForeColor = _notOK;
                    retorno = false;
                }
            }

            if (_objCampo.GetType() == typeof(TextBox))
            {
                _labCampo.ForeColor = _OK;
                if (((TextBox)_objCampo).Text.Trim() == "")
                {
                    _labCampo.ForeColor = _notOK;
                    retorno = false;
                }
            }

            return retorno;
        }
        private bool ValidarItemDuplicado(int codigoApuracaoEmpresa, int codigoApuracaoTipo, int codigoItem)
        {
            bool retorno = true;

            var v = contabilController.ObterApuracaoEmpresaItem(codigoApuracaoEmpresa, codigoApuracaoTipo).Where(p => p.CTB_APURACAO_ITEM == codigoItem);
            if (v != null && v.Count() > 0)
                retorno = false;

            return retorno;
        }

        protected void btCalcular_Click(object sender, EventArgs e)
        {
            try
            {
                labErroCalcular.Text = "";

                if (ddlImposto.SelectedValue.Trim() == "Selecione")
                {
                    labErroCalcular.Text = "Selecione o Imposto para o cálculo.";
                    return;
                }

                string codigoEmpresaItem = hidCTBApuracaoEmpresa.Value;
                string tipoImposto = ddlImposto.SelectedValue.Trim();
                string _url = "";
                //Abrir pop-up
                _url = "fnAbrirTelaCadastroMaior('fisc_apuracao_imposto_det.aspx?e=" + codigoEmpresaItem + "&i=" + tipoImposto + "');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _url, true);
            }
            catch (Exception ex)
            {
                labErroCalcular.Text = ex.Message;
            }
        }

    }
}
