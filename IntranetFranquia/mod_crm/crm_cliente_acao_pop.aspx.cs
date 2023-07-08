using DAL;
using Relatorios.mod_ecom.mag;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Relatorios
{
    public partial class crm_cliente_acao_pop : System.Web.UI.Page
    {
        CRMController crmController = new CRMController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                if (Request.QueryString["cpf"] == null || Request.QueryString["cpf"] == "" ||
                    Request.QueryString["ca"] == null || Request.QueryString["ca"] == "" ||
                    Session["USUARIO"] == null
                    )
                    Response.Redirect("crm_menu.aspx");

                var cpf = Request.QueryString["cpf"].ToString();
                var codigoAcao = Convert.ToInt32(Request.QueryString["ca"].ToString());
                hidCodigoAcao.Value = codigoAcao.ToString();

                CarregarFiliais();

                var cliente = crmController.ObterClienteAcao(cpf, codigoAcao);
                txtCPF.Text = cliente.CPF;
                txtNome.Text = cliente.NOME;
                ddlFilial.SelectedValue = baseController.BuscaFilialCodigo(Convert.ToInt32(cliente.CODIGO_FILIAL)).COD_FILIAL;
                txtTelefone.Text = cliente.TELEFONE;
                txtCelular.Text = cliente.CELULAR;
                txtEndereco.Text = cliente.ENDERECO;
                txtObs.Text = cliente.OBSERVACAO;

                if (cliente.DATA_BAIXA != null)
                    ddlStatus.SelectedValue = "N";

                dialogPai.Visible = false;
            }

            //Evitar duplo clique no botão
            btSalvar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btSalvar, null) + ";");

        }

        #region "DADOS INICIAIS"
        private USUARIO ObterUsuario()
        {
            if (Session["USUARIO"] != null)
            {
                USUARIO usuario = (USUARIO)Session["USUARIO"];
                return usuario;
            }

            return null;
        }
        private void CarregarFiliais()
        {
            var filial = new List<FILIAI>();
            var filialDePara = new List<FILIAIS_DE_PARA>();

            filial = baseController.BuscaFiliais_Intermediario();
            filialDePara = baseController.BuscaFilialDePara();

            filial = filial.Where(i => !filialDePara.Any(g => g.DE == i.COD_FILIAL)).ToList();
            if (filial != null)
            {
                filial = filial.OrderBy(p => p.FILIAL).ToList();
                filial.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "Selecione" });
                ddlFilial.DataSource = filial;
                ddlFilial.DataBind();
            }
        }
        #endregion

        protected void btSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                //SALVAR
                var u = ObterUsuario();
                if (u == null)
                {
                    labErro.Text = "Favor realizar o Login novamente.";
                    return;
                }

                if (txtNome.Text == "")
                {
                    labErro.Text = "Informe o nome do cliente.";
                    return;
                }

                if (ddlFilial.SelectedValue == "")
                {
                    labErro.Text = "Selecione a Filial.";
                    return;
                }

                if (txtTelefone.Text.Trim() == "" && txtCelular.Text.Trim() == "")
                {
                    labErro.Text = "Informe pelo menos 1 telefone.";
                    return;
                }

                if (txtEndereco.Text.Trim() == "")
                {
                    labErro.Text = "Informe o endereço do Cliente.";
                    return;
                }

                var clienteAcao = crmController.ObterClienteAcao(txtCPF.Text, Convert.ToInt32(hidCodigoAcao.Value));
                clienteAcao.CPF = txtCPF.Text;
                clienteAcao.NOME = txtNome.Text.Trim().ToUpper();
                clienteAcao.CODIGO_FILIAL = ddlFilial.SelectedValue.Trim();
                clienteAcao.TELEFONE = txtTelefone.Text.Trim();
                clienteAcao.CELULAR = txtCelular.Text.Trim();
                clienteAcao.ENDERECO = txtEndereco.Text.Trim();
                clienteAcao.OBSERVACAO = txtObs.Text;
                clienteAcao.USUARIO = u.CODIGO_USUARIO;

                clienteAcao.DATA_BAIXA = null;
                if (ddlStatus.SelectedValue == "N")
                    clienteAcao.DATA_BAIXA = DateTime.Now;

                crmController.AtualizarClienteAcao(clienteAcao);

                btSalvar.Enabled = false;

                labErro.Text = "Cliente atualizado com sucesso.";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#dialog').dialog({ autoOpen: true, position: { at: 'center top'}, height: 250, width: 395, modal: true, close: function (event, ui) { window.close(); } }); });", true);
                dialogPai.Visible = true;

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
    }
}


