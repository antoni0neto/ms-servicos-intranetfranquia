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
    public partial class ecom_parametros_api : System.Web.UI.Page
    {
        EcomController ecomController = new EcomController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                hrefVoltar.HRef = "ecom_menu.aspx";

                CarregarDadosIniciais();
                CarregarParametrosAPI();

                ibtSalvar.Visible = false;
                ibtCancelar.Visible = false;

            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#tabs').tabs(); });", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$('#tabs').tabs('option', 'active', '" + ((hidTabSelected.Value == "") ? "0" : hidTabSelected.Value) + "');", true);
        }

        private void CarregarParametrosAPI()
        {
            var p = ecomController.ObterParametroAPI();
            if (p != null)
            {
                ddlFilial.SelectedValue = p.CODIGO_FILIAL;
                ddlTransportadora.SelectedValue = p.TRANSPORTADORA;
                txtContaContabilCliente.Text = p.CTB_CONTA_CONTABIL;

                ddlColecao.SelectedValue = p.COLECAO;
                ddlTabelaPreco.SelectedValue = p.CODIGO_TAB_PRECO;
                ddlRepresentante.SelectedValue = p.REPRESENTANTE;

                ddlTipoVenda.SelectedValue = p.TIPO_VENDA;
                ddlTipoDocumento.SelectedValue = p.TIPO_DOCUMENTO.ToString();
                ddlAprovacao.SelectedValue = p.APROVACAO;

                txtContaContabilICR.Text = p.CONTA_PORTADORA_ICR;
                txtContaContabilLTA.Text = p.CONTA_PORTADORA_LTA;
                txtContaContabilIAC.Text = p.CONTA_PORTADORA_IAC;

                txtCentroCustoICR.Text = p.RATEIO_CENTRO_CUSTO_ICR;
                txtCentroCustoLTA.Text = p.RATEIO_CENTRO_CUSTO_LTA;
                txtCentroCustoIAC.Text = p.RATEIO_CENTRO_CUSTO_IAC;

                cbConfiguraEmail.Checked = (p.CONFIGURA_EMAIL == "S") ? true : false;
                txtRemetente.Text = p.EMAIL_REMET;
                txtUsuario.Text = p.USER_AUTH;
                txtSenha.Text = p.PASS_AUTH;
                txtSMTP.Text = p.SMPT;
                txtPorta.Text = p.PORT;

                txtValorPedidoAprovacao.Text = p.VALOR_PEDIDO_APROVACAO.ToString();
            }
            HabilitarCampos(false);
        }

        #region "AÇÕES DA TELA"
        protected void ibtEditar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                ibtEditar.Visible = false;
                ibtSalvar.Visible = true;
                ibtCancelar.Visible = true;

                HabilitarCampos(true);
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void ibtSalvar_Click(object sender, EventArgs e)
        {
            ContabilidadeController contabilController = new ContabilidadeController();

            //Validar campos obrigatórios
            try
            {
                labErro.Text = "";

                if (!ValidarCampos())
                {
                    labErro.Text = "Preencha corretamente os campos em Vermelho";
                    return;
                }

                var ctbCliente = contabilController.ObterConta(txtContaContabilCliente.Text.Trim());
                if (ctbCliente == null)
                {
                    labErro.Text = "Conta Contábil do Cliente não existe no LINX.";
                    return;
                }

                var ctbICR = contabilController.ObterConta(txtContaContabilICR.Text.Trim());
                if (ctbICR == null)
                {
                    labErro.Text = "Conta Contábil ICR não existe no LINX.";
                    return;
                }

                var ctbLTA = contabilController.ObterConta(txtContaContabilLTA.Text.Trim());
                if (ctbLTA == null)
                {
                    labErro.Text = "Conta Contábil LTA não existe no LINX.";
                    return;
                }

                var ctbIAC = contabilController.ObterConta(txtContaContabilIAC.Text.Trim());
                if (ctbIAC == null)
                {
                    labErro.Text = "Conta Contábil IAC não existe no LINX.";
                    return;
                }

                var p = ecomController.ObterParametroAPI();
                if (p != null)
                {
                    p.CODIGO_FILIAL = ddlFilial.SelectedValue;
                    p.FILIAL = ddlFilial.SelectedItem.Text.Trim();
                    p.TRANSPORTADORA = ddlTransportadora.SelectedValue;
                    p.CONTA_CONTABIL = null;
                    p.CTB_CONTA_CONTABIL = txtContaContabilCliente.Text.Trim();

                    p.COLECAO = ddlColecao.SelectedValue;
                    p.CODIGO_TAB_PRECO = ddlTabelaPreco.SelectedValue;
                    p.REPRESENTANTE = ddlRepresentante.SelectedValue;

                    p.TIPO_VENDA = ddlTipoVenda.SelectedValue;
                    p.TIPO_DOCUMENTO = Convert.ToInt32(ddlTipoDocumento.SelectedValue);
                    p.APROVACAO = ddlAprovacao.SelectedValue;

                    p.CONTA_PORTADORA_ICR = txtContaContabilICR.Text.Trim();
                    p.CONTA_PORTADORA_LTA = txtContaContabilLTA.Text.Trim();
                    p.CONTA_PORTADORA_IAC = txtContaContabilIAC.Text.Trim();

                    p.RATEIO_CENTRO_CUSTO_ICR = txtCentroCustoICR.Text.Trim();
                    p.RATEIO_CENTRO_CUSTO_LTA = txtCentroCustoLTA.Text.Trim();
                    p.RATEIO_CENTRO_CUSTO_IAC = txtCentroCustoIAC.Text.Trim();

                    p.EMAIL_REMET = txtRemetente.Text.ToLower();
                    p.USER_AUTH = txtUsuario.Text.ToLower();
                    p.PASS_AUTH = txtSenha.Text;
                    p.SMPT = txtSMTP.Text;
                    p.PORT = txtPorta.Text;
                    p.CONFIGURA_EMAIL = (cbConfiguraEmail.Checked) ? "S" : "N";

                    p.VALOR_PEDIDO_APROVACAO = Convert.ToDecimal(txtValorPedidoAprovacao.Text);

                    ecomController.AtualizarParametroAPI(p);

                    ibtSalvar.Visible = false;
                    ibtCancelar.Visible = false;
                    ibtEditar.Visible = true;

                    CarregarParametrosAPI();
                }

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
                CarregarParametrosAPI();

                //controle de visibilidade dos botões da tela
                ibtSalvar.Visible = false;
                ibtCancelar.Visible = false;
                ibtEditar.Visible = true;

                ValidarCampos();
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        private void MoverAba(string vAba)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$('#tabs').tabs('option', 'active', '" + vAba + "');", true);
            hidTabSelected.Value = vAba;
        }
        #endregion

        #region "DADOS INICIAIS"
        private void CarregarDadosIniciais()
        {
            CarregarFilial();
            CarregarTransportadora();

            CarregarColecao();
            CarregarTabelaPreco();

            CarregarTipoVenda();
            CarregarTipoDocumento();


        }
        private void CarregarFilial()
        {
            var filial = baseController.ListarFiliais();
            ddlFilial.DataSource = filial;
            ddlFilial.DataBind();
        }
        private void CarregarTransportadora()
        {
            var tra = baseController.ObterTransportadoras();
            ddlTransportadora.DataSource = tra;
            ddlTransportadora.DataBind();
        }
        private void CarregarColecao()
        {
            var col = baseController.BuscaColecoes();
            ddlColecao.DataSource = col;
            ddlColecao.DataBind();
        }
        private void CarregarTabelaPreco()
        {
            var tab = baseController.ObterTabelaPreco();
            ddlTabelaPreco.DataSource = tab;
            ddlTabelaPreco.DataBind();
        }
        private void CarregarTipoVenda()
        {
            var tipo = baseController.ObterVendaTipos();
            ddlTipoVenda.DataSource = tipo;
            ddlTipoVenda.DataBind();
        }
        private void CarregarTipoDocumento()
        {
            var tipo = baseController.ObterDocumentoTipos();
            ddlTipoDocumento.DataSource = tipo;
            ddlTipoDocumento.DataBind();
        }

        #endregion

        #region "VALIDACAO"
        private bool ValidarCampos()
        {
            int contador = 0;

            ValidarCampos(labFilial, ddlFilial, ref contador);
            ValidarCampos(labTransportadora, ddlTransportadora, ref contador);
            ValidarCampos(labContaContabilCliente, txtContaContabilCliente, ref contador);

            ValidarCampos(labColecao, ddlColecao, ref contador);
            ValidarCampos(labTabelaPreco, ddlTabelaPreco, ref contador);
            ValidarCampos(labRepresentante, ddlRepresentante, ref contador);

            ValidarCampos(labTipo, ddlTipoVenda, ref contador);
            ValidarCampos(labTipoDocumento, ddlTipoDocumento, ref contador);
            ValidarCampos(labAprovacao, ddlAprovacao, ref contador);

            ValidarCampos(labContaContabilICR, txtContaContabilICR, ref contador);
            ValidarCampos(labContaContabilLTA, txtContaContabilLTA, ref contador);
            ValidarCampos(labContaContabilIAC, txtContaContabilIAC, ref contador);

            ValidarCampos(labCentroCustoICR, txtCentroCustoICR, ref contador);
            ValidarCampos(labCentroCustoLTA, txtCentroCustoLTA, ref contador);
            ValidarCampos(labCentroCustoIAC, txtCentroCustoIAC, ref contador);

            ValidarCampos(labValorPedidoAprovacao, txtValorPedidoAprovacao, ref contador);

            if (cbConfiguraEmail.Checked)
            {
                ValidarCampos(labEmailRemetente, txtRemetente, ref contador);
                ValidarCampos(labUsuario, txtUsuario, ref contador);
                ValidarCampos(labSenha, txtSenha, ref contador);
                ValidarCampos(labSMTP, txtSMTP, ref contador);
                ValidarCampos(labPorta, txtPorta, ref contador);
            }

            return (contador == 0) ? true : false;
        }
        private void ValidarCampos(Label l, object o, ref int contador)
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;

            if (o.GetType() == typeof(TextBox))
            {
                l.ForeColor = _OK;
                if (((TextBox)o).Text.Trim() == "")
                {
                    l.ForeColor = _notOK;
                    contador += 1;
                }
            }
            if (o.GetType() == typeof(DropDownList))
            {
                l.ForeColor = _OK;
                if (((DropDownList)o).SelectedValue.Trim() == "")
                {
                    l.ForeColor = _notOK;
                    contador += 1;
                }
            }

        }
        private void HabilitarCampos(bool enable)
        {
            ddlFilial.Enabled = enable;
            ddlTransportadora.Enabled = enable;
            txtContaContabilCliente.Enabled = enable;

            ddlColecao.Enabled = enable;
            ddlTabelaPreco.Enabled = enable;
            ddlRepresentante.Enabled = enable;

            ddlTipoVenda.Enabled = enable;
            ddlTipoDocumento.Enabled = enable;
            ddlAprovacao.Enabled = enable;

            txtContaContabilICR.Enabled = enable;
            txtContaContabilLTA.Enabled = enable;
            txtContaContabilIAC.Enabled = enable;

            txtCentroCustoICR.Enabled = enable;
            txtCentroCustoLTA.Enabled = enable;
            txtCentroCustoIAC.Enabled = enable;

            txtValorPedidoAprovacao.Enabled = enable;

        }
        #endregion

        protected void cbConfiguraEmail_CheckedChanged(object sender, EventArgs e)
        {
            txtRemetente.Enabled = cbConfiguraEmail.Checked;
            txtUsuario.Enabled = cbConfiguraEmail.Checked;
            txtSenha.Enabled = cbConfiguraEmail.Checked;
            txtSMTP.Enabled = cbConfiguraEmail.Checked;
            txtPorta.Enabled = cbConfiguraEmail.Checked;
        }

    }
}