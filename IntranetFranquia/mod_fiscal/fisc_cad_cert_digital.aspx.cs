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
    public partial class fisc_cad_cert_digital : System.Web.UI.Page
    {
        ContabilidadeController contController = new ContabilidadeController();
        BaseController baseController = new BaseController();

        const string cAbaTodos = "1";

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataIni.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataFim.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);

            if (!Page.IsPostBack)
            {

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
                HabilitarCampos(true);

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
                string codigoCertificado = hidCodigoCertificado.Value;


                if (codigoCertificado != "")
                {
                    var _certificado = contController.ObterCertificadoDigital(Convert.ToInt32(codigoCertificado));
                    if (_certificado != null)
                    {

                        CarregarCertificado(_certificado);

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
                    CarregarCertificadoTodos();
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

                int codigoCertificado = 0;
                CTB_CERTIFICADO_DIG certificado = null;
                if (hidCodigoCertificado.Value != "")
                {
                    codigoCertificado = Convert.ToInt32(hidCodigoCertificado.Value);
                    certificado = contController.ObterCertificadoDigital(codigoCertificado);
                }
                if (certificado == null)
                    certificado = new CTB_CERTIFICADO_DIG();

                certificado.TITULAR = txtTitular.Text.Trim().ToUpper();
                certificado.CNPJ_CPF = txtCPF.Text.Trim();
                certificado.RESP_ASSINATURA = txtRespAssinatura.Text.Trim().ToUpper();
                certificado.VIGENCIA_DE = Convert.ToDateTime(txtDataIni.Text.Trim());
                certificado.VIGENCIA_ATE = Convert.ToDateTime(txtDataFim.Text.Trim());
                certificado.RESP_COMPRA = ddlRespCompra.SelectedValue.ToUpper();
                certificado.CERTIFICADO = ddlCertificado.SelectedValue.ToUpper();
                certificado.TIPO = ddlTipo.SelectedValue.ToUpper();
                certificado.CERTIFICADORA = ddlCertificadora.SelectedValue.ToUpper();

                if (hidAcao.Value == "I")
                {
                    certificado.DATA_INCLUSAO = DateTime.Now;
                    certificado.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                    codigoCertificado = contController.InserirCertificadoDigital(certificado);
                }
                else
                {
                    certificado.DATA_ALTERACAO = DateTime.Now;
                    certificado.USUARIO_ALTERACAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                    contController.AtualizarCertificadoDigital(certificado);
                }


                hidCodigoCertificado.Value = codigoCertificado.ToString();
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
                labTitular.ForeColor = _OK;
                labCPF.ForeColor = _OK;
                labRespAssinatura.ForeColor = _OK;
                labDataIni.ForeColor = _OK;
                labDataFim.ForeColor = _OK;
                labRespCompra.ForeColor = _OK;
                labCertificado.ForeColor = _OK;
                labTipo.ForeColor = _OK;
                labCertificadora.ForeColor = _OK;


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
                if (hidCodigoCertificado.Value == "")
                {
                    labErro.Text = "Certificado Digital não informado. Refaça sua pesquisa.";
                }
                else
                {
                    int codigoCert = Convert.ToInt32(hidCodigoCertificado.Value);
                    var c = contController.ObterCertificadoDigital(codigoCert);
                    c.DATA_EXCLUSAO = DateTime.Now;
                    c.USUARIO_EXCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

                    contController.AtualizarCertificadoDigital(c);

                    ibtLimpar_Click(null, null);
                    MoverAba("0");
                }
            }
            catch (Exception ex)
            {
                labErro.Text = "Não será possível excluir este Certificado. Ocorreu um erro inesperado.\n\n" + ex.Message;
            }
        }

        private void MoverAba(string vAba)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$('#tabs').tabs('option', 'active', '" + vAba + "');", true);
            hidTabSelected.Value = vAba;
        }
        private void CarregarCertificado(CTB_CERTIFICADO_DIG certificadoDig)
        {
            txtTitular.Text = certificadoDig.TITULAR;
            txtCPF.Text = certificadoDig.CNPJ_CPF;

            txtRespAssinatura.Text = certificadoDig.RESP_ASSINATURA;
            txtDataIni.Text = certificadoDig.VIGENCIA_DE.ToString("dd/MM/yyyy");
            txtDataFim.Text = certificadoDig.VIGENCIA_ATE.ToString("dd/MM/yyyy");

            ddlRespCompra.SelectedValue = certificadoDig.RESP_COMPRA;
            ddlCertificado.SelectedValue = certificadoDig.CERTIFICADO;
            ddlTipo.SelectedValue = certificadoDig.TIPO;
            ddlCertificadora.SelectedValue = certificadoDig.CERTIFICADORA;

            hidCodigoCertificado.Value = certificadoDig.CODIGO.ToString();

        }
        #endregion

        #region "TODOS"
        private void CarregarCertificadoTodos()
        {
            var certTodos = contController.ObterCertificadoDigital();

            //FILTROS
            if (txtTitular.Text.Trim() != "")
                certTodos = certTodos.Where(p => p.TITULAR.Contains(txtTitular.Text.Trim().ToUpper())).ToList();

            if (txtCPF.Text.Trim() != "")
                certTodos = certTodos.Where(p => p.CNPJ_CPF.Contains(txtCPF.Text.Trim().ToUpper())).ToList();

            if (txtRespAssinatura.Text.Trim() != "")
                certTodos = certTodos.Where(p => p.RESP_ASSINATURA.Contains(txtRespAssinatura.Text.Trim().ToUpper())).ToList();

            if (txtDataIni.Text.Trim() != "")
                certTodos = certTodos.Where(p => p.VIGENCIA_DE >= Convert.ToDateTime(txtDataIni.Text)).ToList();

            if (txtDataFim.Text.Trim() != "")
                certTodos = certTodos.Where(p => p.VIGENCIA_ATE <= Convert.ToDateTime(txtDataFim.Text)).ToList();

            if (ddlRespCompra.SelectedValue.Trim() != "")
                certTodos = certTodos.Where(p => p.RESP_COMPRA.Contains(ddlRespCompra.SelectedValue)).ToList();

            if (ddlCertificado.SelectedValue.Trim() != "")
                certTodos = certTodos.Where(p => p.CERTIFICADO.Contains(ddlCertificado.SelectedValue)).ToList();

            if (ddlTipo.SelectedValue.Trim() != "")
                certTodos = certTodos.Where(p => p.TIPO.Contains(ddlTipo.SelectedValue)).ToList();

            if (ddlCertificadora.SelectedValue.Trim() != "")
                certTodos = certTodos.Where(p => p.CERTIFICADORA.Contains(ddlCertificadora.SelectedValue)).ToList();

            gvTodos.DataSource = certTodos;
            gvTodos.DataBind();
        }
        protected void gvTodos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    CTB_CERTIFICADO_DIG certif = e.Row.DataItem as CTB_CERTIFICADO_DIG;

                    if (certif != null)
                    {

                        ImageButton _ibtPesquisar = e.Row.FindControl("btPesquisar") as ImageButton;
                        if (_ibtPesquisar != null)
                            _ibtPesquisar.CommandArgument = certif.CODIGO.ToString();

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
                    string codigoCertificado = ibt.CommandArgument;
                    hidCodigoCertificado.Value = codigoCertificado;

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

            labTitular.ForeColor = _OK;
            if (txtTitular.Text.Trim() == "")
            {
                labTitular.ForeColor = _notOK;
                retorno = false;
            }

            labCPF.ForeColor = _OK;
            if (txtCPF.Text.Trim() == "")
            {
                labCPF.ForeColor = _notOK;
                retorno = false;
            }

            labRespAssinatura.ForeColor = _OK;
            if (txtRespAssinatura.Text.Trim() == "")
            {
                labRespAssinatura.ForeColor = _notOK;
                retorno = false;
            }

            labDataIni.ForeColor = _OK;
            if (txtDataIni.Text.Trim() == "")
            {
                labDataIni.ForeColor = _notOK;
                retorno = false;
            }

            labDataFim.ForeColor = _OK;
            if (txtDataFim.Text.Trim() == "")
            {
                labDataFim.ForeColor = _notOK;
                retorno = false;
            }

            labRespCompra.ForeColor = _OK;
            if (ddlRespCompra.SelectedValue == "")
            {
                labRespCompra.ForeColor = _notOK;
                retorno = false;
            }

            labCertificado.ForeColor = _OK;
            if (ddlCertificado.SelectedValue == "")
            {
                labCertificado.ForeColor = _notOK;
                retorno = false;
            }

            labTipo.ForeColor = _OK;
            if (ddlTipo.SelectedValue == "")
            {
                labTipo.ForeColor = _notOK;
                retorno = false;
            }

            labCertificadora.ForeColor = _OK;
            if (ddlCertificadora.SelectedValue == "")
            {
                labCertificadora.ForeColor = _notOK;
                retorno = false;
            }

            return retorno;
        }
        private void HabilitarCampos(bool enable)
        {
            txtTitular.Enabled = enable;
            txtCPF.Enabled = enable;
            txtRespAssinatura.Enabled = enable;
            txtDataIni.Enabled = enable;
            txtDataFim.Enabled = enable;
            ddlRespCompra.Enabled = enable;
            ddlCertificado.Enabled = enable;
            ddlTipo.Enabled = enable;
            ddlCertificadora.Enabled = enable;

        }
        private void LimparCampos()
        {
            hidCodigoCertificado.Value = "";

            txtTitular.Text = "";
            txtCPF.Text = "";
            txtRespAssinatura.Text = "";
            txtDataIni.Text = "";
            txtDataFim.Text = "";

            ddlRespCompra.SelectedValue = "";
            ddlCertificado.SelectedValue = "";
            ddlTipo.SelectedValue = "";
            ddlCertificadora.SelectedValue = "";


        }
        #endregion


    }
}