using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Relatorios
{
    public partial class fisc_cad_obrigacao : System.Web.UI.Page
    {
        ContabilidadeController contController = new ContabilidadeController();
        BaseController baseController = new BaseController();

        const string cAbaTodos = "1";

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataFim.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);

            if (!Page.IsPostBack)
            {

                CarregarEsfera();
                CarregarRegimeTributacao();
                CarregarUF();
                ddlPeriodo_SelectedIndexChanged(ddlPeriodo, null);


                ibtSalvar.Visible = false;
                ibtCancelar.Visible = false;
                ibtEditar.Visible = false;
                ibtExcluir.Visible = false;
                ibtLimpar.Visible = false;

            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#tabs').tabs(); });", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$('#tabs').tabs('option', 'active', '" + ((hidTabSelected.Value == "") ? "0" : hidTabSelected.Value) + "');", true);
        }

        #region "DADOS INICIAIS"
        private void CarregarEsfera()
        {
            var esfera = contController.ObterEsfera();

            esfera.Insert(0, new CTB_ESFERA { CODIGO = 0, DESCRICAO = "Selecione" });
            ddlEsfera.DataSource = esfera;
            ddlEsfera.DataBind();
        }
        private void CarregarUF()
        {
            var filial = baseController.ObterFilialCadastroCLIFOR();
            var filialUF = filial.Select(p => p.UF).Distinct().ToList().OrderBy(p => p);

            ddlUF.DataSource = filialUF;
            ddlUF.DataBind();
            ddlUF.Items.Insert(0, new ListItem { Value = "", Text = "Selecione" });
        }
        private void CarregarRegimeTributacao()
        {
            var empresa = new BaseController().ObterEmpresa().Select(p => p.DESC_TRIBUTACAO).Distinct().ToList();

            empresa.Insert(0, "Selecione");
            ddlRegimeTributacao.DataSource = empresa;
            ddlRegimeTributacao.DataBind();
        }
        protected void ddlPeriodo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string periodo = ddlPeriodo.SelectedValue;

            if (periodo == "")
            {
                ddlDiaUtil.SelectedValue = "";
                ddlDiaUtil.Enabled = false;

                txtDataIni.Text = "";
                txtDataIni.Enabled = false;

                txtDataFim.Text = "";
                txtDataFim.Enabled = false;
            }
            else if (periodo == "M") //MENSAL
            {
                ddlDiaUtil.SelectedValue = "";
                ddlDiaUtil.Enabled = true;

                labDataIni.Text = "Dia";
                txtDataIni.Text = "";
                txtDataIni.Enabled = true;
                txtDataIni.MaxLength = 2;

                txtDataFim.Text = "";
                txtDataFim.Enabled = false;
            }
            else if (periodo == "A") //ANUAL
            {
                ddlDiaUtil.SelectedValue = "N";
                ddlDiaUtil.Enabled = false;

                labDataIni.Text = "Data Inicial";
                txtDataIni.Text = "";
                txtDataIni.Enabled = true;
                txtDataIni.MaxLength = 10;
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataIni.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);

                txtDataFim.Text = "";
                txtDataFim.Enabled = true;
                txtDataFim.MaxLength = 10;
            }

        }

        #endregion

        #region "AÇÕES DA TELA"
        protected void ibtIncluir_Click(object sender, EventArgs e)
        {
            //controle de visibilidade dos botões da tela
            ibtIncluir.Visible = false;
            ibtEditar.Visible = false;
            ibtExcluir.Visible = false;
            ibtPesquisar.Visible = false;
            ibtLimpar.Visible = false;

            //Controle de campos
            HabilitarCampos(true);
            LimparCampos();

            hidAcao.Value = "I";
            ibtSalvar.Visible = true;
            ibtCancelar.Visible = true;

            MoverAba("0");
        }
        protected void ibtEditar_Click(object sender, EventArgs e)
        {
            try
            {
                //controle de visibilidade dos botões da tela
                ibtIncluir.Visible = false;
                ibtEditar.Visible = false;
                ibtExcluir.Visible = false;
                ibtPesquisar.Visible = false;
                ibtLimpar.Visible = false;

                ibtSalvar.Visible = true;
                ibtCancelar.Visible = true;

                labErro.Text = "";
                hidAcao.Value = "A";

                //verifica se pode atualizar
                var obrigacaoValida = contController.ObterObrigacaoFiscalFilial().Where(p => p.CTB_OBRIGACAO == Convert.ToInt32(hidCodigoObrigacao.Value));
                if (obrigacaoValida == null || obrigacaoValida.Count() <= 0)
                {
                    HabilitarCampos(true);

                    //Controle de campos depois de habilitar todos
                    if (ddlPeriodo.SelectedValue == "M")
                    {
                        txtDataFim.Enabled = false;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataIni.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);
                        txtDataFim.Enabled = true;
                        ddlDiaUtil.Enabled = false;
                    }
                }
                else
                {
                    ddlStatus.Enabled = true;
                }

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void ibtPesquisar_Click(object sender, EventArgs e)
        {

            labErro.Text = "";
            try
            {
                string codigoObrigacao = hidCodigoObrigacao.Value;


                if (codigoObrigacao != "")
                {
                    var _obrigacao = contController.ObterObrigacaoFiscal(Convert.ToInt32(codigoObrigacao));
                    if (_obrigacao != null)
                    {

                        CarregarObrigacao(_obrigacao);

                        ibtIncluir.Visible = true;
                        ibtPesquisar.Visible = false;
                        ibtSalvar.Visible = false;
                        ibtCancelar.Visible = false;
                        ibtEditar.Visible = true;
                        ibtLimpar.Visible = true;
                        ibtExcluir.Visible = true;
                        hidAcao.Value = "C";

                        HabilitarCampos(false);

                        MoverAba("0");

                    }
                }
                else
                {


                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$('#tabs').tabs('option', 'active', '" + cAbaTodos + "');", true);
                    hidTabSelected.Value = cAbaTodos;
                    CarregarObrigacaoTodos();
                }

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void ibtLimpar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                ibtPesquisar.Visible = true;
                ibtEditar.Visible = false;
                ibtLimpar.Visible = false;
                ibtExcluir.Visible = false;
                hidAcao.Value = "";

                HabilitarCampos(true);
                LimparCampos();

                MoverAba("0");
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void ibtSalvar_Click(object sender, EventArgs e)
        {
            //Validar campos obrigatórios
            try
            {

                labErro.Text = "";

                if (!ValidarCampos())
                {
                    labErro.Text = "Preencha corretamente os campos em Vermelho";
                    return;
                }

                int codigoObrigacao = 0;
                CTB_OBRIGACAO obrigacao = null;
                if (hidCodigoObrigacao.Value != "")
                {
                    codigoObrigacao = Convert.ToInt32(hidCodigoObrigacao.Value);
                    obrigacao = contController.ObterObrigacaoFiscal(codigoObrigacao);
                }
                if (obrigacao == null)
                    obrigacao = new CTB_OBRIGACAO();

                obrigacao.DESCRICAO = txtObrigacao.Text.Trim().ToUpper();
                obrigacao.CTB_ESFERA = Convert.ToInt32(ddlEsfera.SelectedValue);
                obrigacao.REGIME_TRIBUTACAO = ddlRegimeTributacao.SelectedValue.ToUpper();
                obrigacao.PERIODO = Convert.ToChar(ddlPeriodo.SelectedValue);
                obrigacao.DIA_UTIL = (ddlDiaUtil.SelectedValue == "S") ? true : false;
                if (obrigacao.PERIODO == 'M')
                {
                    obrigacao.DIA = Convert.ToInt32(txtDataIni.Text);
                    obrigacao.DATA_INI = null;
                    obrigacao.DATA_FIM = null;
                }
                if (obrigacao.PERIODO == 'A')
                {
                    obrigacao.DIA = null;
                    obrigacao.DATA_INI = Convert.ToDateTime(txtDataIni.Text);
                    obrigacao.DATA_FIM = Convert.ToDateTime(txtDataFim.Text);
                }
                obrigacao.UF = ddlUF.SelectedValue;
                obrigacao.UTILIZAR_VALOR = (ddlUtilizaValor.SelectedValue == "S") ? true : false;
                obrigacao.STATUS = Convert.ToChar(ddlStatus.SelectedValue);
                obrigacao.FUNDAMENTACAO = txtFundamentacao.Text;
                obrigacao.OBSERVACAO = txtObservacao.Text;
                obrigacao.DATA_INCLUSAO = DateTime.Now;
                obrigacao.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

                if (txtInsEstadual.Text.Trim() != "")
                    obrigacao.INSCRICAO_ESTADUAL = txtInsEstadual.Text;

                if (hidAcao.Value == "I")
                    codigoObrigacao = contController.InserirObrigacaoFiscal(obrigacao);
                else
                    contController.AtualizarObrigacaoFiscal(obrigacao);


                hidCodigoObrigacao.Value = codigoObrigacao.ToString();
                //Entrar em modo de CONSULTA
                ibtPesquisar_Click(null, null);
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void ibtCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                //if (!ValidarCampos() && hidAcao.Value == "A")
                //{
                //    labErro.Text = "Preencha corretamente os campos em Vermelho";
                //    return;
                //}

                //controle de visibilidade dos botões da tela
                ibtIncluir.Visible = true;
                ibtSalvar.Visible = false;
                ibtCancelar.Visible = false;

                System.Drawing.Color _OK = System.Drawing.Color.Gray;
                labObrigacao.ForeColor = _OK;
                labEsfera.ForeColor = _OK;
                labRegimeTributacao.ForeColor = _OK;
                labPeriodo.ForeColor = _OK;
                labDiaUtil.ForeColor = _OK;
                labDataIni.ForeColor = _OK;
                labDataFim.ForeColor = _OK;
                labUF.ForeColor = _OK;
                labUtilizaValor.ForeColor = _OK;
                labFundamentacao.ForeColor = _OK;
                labObservacao.ForeColor = _OK;
                labStatus.ForeColor = _OK;

                if (hidAcao.Value == "I")
                {
                    ibtLimpar_Click(null, null);

                    ibtPesquisar.Visible = true;

                }

                if (hidAcao.Value == "A")
                {
                    ibtPesquisar_Click(null, null);
                    ibtPesquisar.Visible = false;
                    ibtExcluir.Visible = true;
                    ibtEditar.Visible = true;
                    ibtLimpar.Visible = true;
                }

                hidAcao.Value = "";
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void ibtExcluir_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                if (hidCodigoObrigacao.Value == "")
                {
                    labErro.Text = "Obrigação não informada. Refaça sua pesquisa.";
                }
                else
                {
                    contController.ExcluirObrigacaoFiscal(Convert.ToInt32(hidCodigoObrigacao.Value));

                    ibtLimpar_Click(null, null);
                    MoverAba("0");
                }
            }
            catch (Exception ex)
            {
                labErro.Text = "Não será possível excluir esta Obrigação. Obrigação já foi utilizada.";
            }
        }

        private void MoverAba(string vAba)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$('#tabs').tabs('option', 'active', '" + vAba + "');", true);
            hidTabSelected.Value = vAba;
        }
        private void CarregarObrigacao(CTB_OBRIGACAO obrigacao)
        {
            txtObrigacao.Text = obrigacao.DESCRICAO;
            ddlEsfera.SelectedValue = obrigacao.CTB_ESFERA.ToString();
            ddlRegimeTributacao.SelectedValue = "Selecione";
            if (obrigacao.REGIME_TRIBUTACAO != null)
                ddlRegimeTributacao.SelectedValue = obrigacao.REGIME_TRIBUTACAO.ToUpper();
            ddlDiaUtil.SelectedValue = (obrigacao.DIA_UTIL) ? "S" : "N";
            ddlPeriodo.SelectedValue = obrigacao.PERIODO.ToString();
            //ddlPeriodo_SelectedIndexChanged(ddlPeriodo, null);
            if (obrigacao.PERIODO == 'M')
                txtDataIni.Text = obrigacao.DIA.ToString();
            if (obrigacao.PERIODO == 'A')
            {
                txtDataIni.Text = Convert.ToDateTime(obrigacao.DATA_INI).ToString("dd/MM/yyyy");
                txtDataFim.Text = Convert.ToDateTime(obrigacao.DATA_FIM).ToString("dd/MM/yyyy");
            }
            ddlUF.SelectedValue = obrigacao.UF;
            ddlUtilizaValor.SelectedValue = (obrigacao.UTILIZAR_VALOR) ? "S" : "N";
            txtInsEstadual.Text = obrigacao.INSCRICAO_ESTADUAL;
            txtFundamentacao.Text = obrigacao.FUNDAMENTACAO;
            txtObservacao.Text = obrigacao.OBSERVACAO;
            ddlStatus.SelectedValue = obrigacao.STATUS.ToString();

        }
        #endregion

        #region "TODOS"
        private void CarregarObrigacaoTodos()
        {
            var obrigacaoTodos = contController.ObterObrigacaoFiscal();

            //FILTROS
            if (txtObrigacao.Text.Trim() != "")
                obrigacaoTodos = obrigacaoTodos.Where(p => p.DESCRICAO.Contains(txtObrigacao.Text.Trim().ToUpper())).ToList();

            if (ddlEsfera.SelectedValue != "0")
                obrigacaoTodos = obrigacaoTodos.Where(p => p.CTB_ESFERA == Convert.ToInt32(ddlEsfera.SelectedValue)).ToList();

            if (ddlRegimeTributacao.Text.Trim() != "Selecione")
                obrigacaoTodos = obrigacaoTodos.Where(p => p.REGIME_TRIBUTACAO.Contains(ddlRegimeTributacao.SelectedValue.Trim().ToUpper())).ToList();

            if (ddlPeriodo.SelectedValue != "")
                obrigacaoTodos = obrigacaoTodos.Where(p => p.PERIODO == Convert.ToChar(ddlPeriodo.SelectedValue)).ToList();

            if (ddlDiaUtil.SelectedValue != "")
                obrigacaoTodos = obrigacaoTodos.Where(p => p.DIA_UTIL == ((ddlDiaUtil.SelectedValue == "S") ? true : false)).ToList();

            if (ddlUF.SelectedValue != "")
                obrigacaoTodos = obrigacaoTodos.Where(p => p.UF == ddlUF.SelectedValue).ToList();

            if (ddlUtilizaValor.SelectedValue != "")
                obrigacaoTodos = obrigacaoTodos.Where(p => p.UTILIZAR_VALOR == ((ddlUtilizaValor.SelectedValue == "S") ? true : false)).ToList();

            if (ddlStatus.SelectedValue != "")
                obrigacaoTodos = obrigacaoTodos.Where(p => p.STATUS == Convert.ToChar(ddlStatus.SelectedValue)).ToList();

            gvTodos.DataSource = obrigacaoTodos;
            gvTodos.DataBind();
        }
        protected void gvTodos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    CTB_OBRIGACAO obrigacao = e.Row.DataItem as CTB_OBRIGACAO;

                    if (obrigacao != null)
                    {

                        Literal _litEsfera = e.Row.FindControl("litEsfera") as Literal;
                        if (_litEsfera != null)
                            _litEsfera.Text = obrigacao.CTB_ESFERA1.DESCRICAO;

                        Literal _litPeriodo = e.Row.FindControl("litPeriodo") as Literal;
                        if (_litPeriodo != null)
                            _litPeriodo.Text = (obrigacao.PERIODO == 'A') ? "ANUAL" : "MENSAL";

                        Literal _litDia = e.Row.FindControl("litDia") as Literal;
                        if (_litDia != null)
                        {
                            if (obrigacao.PERIODO == 'A') // ANUAL
                            {
                                _litDia.Text = Convert.ToDateTime(obrigacao.DATA_INI).ToString("dd/MM/yyyy") + " - " + Convert.ToDateTime(obrigacao.DATA_FIM).ToString("dd/MM/yyyy");
                            }
                            else
                            {
                                _litDia.Text = obrigacao.DIA.ToString();
                            }
                        }

                        Literal _litStatus = e.Row.FindControl("litStatus") as Literal;
                        if (_litStatus != null)
                            _litStatus.Text = (obrigacao.STATUS == 'A') ? "Ativo" : "Inativo";

                        ImageButton _ibtPesquisar = e.Row.FindControl("btPesquisar") as ImageButton;
                        if (_ibtPesquisar != null)
                            _ibtPesquisar.CommandArgument = obrigacao.CODIGO.ToString();

                    }
                }
            }
        }
        protected void btPesquisarTodos_Click(object sender, EventArgs e)
        {
            ImageButton ibt = (ImageButton)sender;
            if (ibt != null)
            {
                try
                {
                    string codigoObrigacao = ibt.CommandArgument;
                    hidCodigoObrigacao.Value = codigoObrigacao;

                    ibtPesquisar_Click(null, null);

                    MoverAba("0");
                }
                catch (Exception ex)
                {
                    labErro.Text = ex.Message;
                }
            }
        }
        #endregion

        #region "VALIDACAO"
        private bool ValidarCampos()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            labObrigacao.ForeColor = _OK;
            if (txtObrigacao.Text.Trim() == "")
            {
                labObrigacao.ForeColor = _notOK;
                retorno = false;
            }

            labEsfera.ForeColor = _OK;
            if (ddlEsfera.SelectedValue == "0")
            {
                labEsfera.ForeColor = _notOK;
                retorno = false;
            }

            labRegimeTributacao.ForeColor = _OK;
            if (ddlRegimeTributacao.SelectedValue == "Selecione")
            {
                labRegimeTributacao.ForeColor = _notOK;
                retorno = false;
            }

            labPeriodo.ForeColor = _OK;
            if (ddlPeriodo.SelectedValue == "")
            {
                labPeriodo.ForeColor = _notOK;
                retorno = false;
            }

            labDiaUtil.ForeColor = _OK;
            if (ddlDiaUtil.SelectedValue == "")
            {
                labDiaUtil.ForeColor = _notOK;
                retorno = false;
            }

            labDataIni.ForeColor = _OK;
            if (txtDataIni.Text.Trim() == "")
            {
                labDataIni.ForeColor = _notOK;
                retorno = false;
            }

            labDataFim.ForeColor = _OK;
            if (txtDataFim.Text.Trim() == "" && ddlPeriodo.SelectedValue == "A")
            {
                labDataFim.ForeColor = _notOK;
                retorno = false;
            }

            labUF.ForeColor = _OK;
            if (ddlUF.SelectedValue == "")
            {
                labUF.ForeColor = _notOK;
                retorno = false;
            }

            labUtilizaValor.ForeColor = _OK;
            if (ddlUtilizaValor.SelectedValue == "")
            {
                labUtilizaValor.ForeColor = _notOK;
                retorno = false;
            }

            labStatus.ForeColor = _OK;
            if (ddlStatus.SelectedValue == "")
            {
                labStatus.ForeColor = _notOK;
                retorno = false;
            }

            return retorno;
        }
        private void HabilitarCampos(bool enable)
        {
            txtObrigacao.Enabled = enable;
            ddlEsfera.Enabled = enable;
            ddlRegimeTributacao.Enabled = enable;
            ddlPeriodo.Enabled = enable;
            ddlDiaUtil.Enabled = enable;
            txtDataIni.Enabled = enable;
            txtDataFim.Enabled = enable;
            ddlUF.Enabled = enable;
            ddlUtilizaValor.Enabled = enable;
            txtInsEstadual.Enabled = enable;
            txtFundamentacao.Enabled = enable;
            txtObservacao.Enabled = enable;
            ddlStatus.Enabled = enable;

        }
        private void LimparCampos()
        {
            hidCodigoObrigacao.Value = "";

            txtObrigacao.Text = "";
            ddlEsfera.SelectedValue = "0";
            ddlRegimeTributacao.SelectedValue = "Selecione";
            ddlPeriodo.SelectedValue = "";
            ddlPeriodo_SelectedIndexChanged(ddlPeriodo, null);

            ddlUF.SelectedValue = "";
            ddlUtilizaValor.SelectedValue = "";
            txtInsEstadual.Text = "";
            txtFundamentacao.Text = "";
            txtObservacao.Text = "";
            ddlStatus.SelectedValue = "";
        }
        #endregion


    }
}