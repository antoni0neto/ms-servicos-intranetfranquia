using DAL;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Collections;

namespace Relatorios
{
    public partial class ecom_pedido_mag_histbol_cli : System.Web.UI.Page
    {
        EcomController ecomController = new EcomController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {

                string pedidoExterno = "";

                if (Request.QueryString["p"] == null || Request.QueryString["p"] == "" ||
                    Session["USUARIO"] == null
                    )
                    Response.Redirect("ecom_menu.aspx");

                pedidoExterno = Request.QueryString["p"].ToString();
                hidPedidoExterno.Value = pedidoExterno;

                CarregarPedidoHistorico(pedidoExterno);
                CarregarStatusCliente();

                var pedidoMag = ecomController.ObterPedidoMag(pedidoExterno);
                if (pedidoMag != null)
                {
                    txtPedidoExterno.Text = pedidoExterno;
                    txtNomeCliente.Text = pedidoMag.CLIENTE;
                }

            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#tabs').tabs(); });", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$('#tabs').tabs('option', 'active', '" + ((hidTabSelected.Value == "") ? "0" : hidTabSelected.Value) + "');", true);

            btAdicionar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btAdicionar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void MoverAba(string vAba)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$('#tabs').tabs('option', 'active', '" + vAba + "');", true);
            hidTabSelected.Value = vAba;
        }
        private void CarregarStatusCliente()
        {
            var statusMagCliente = ecomController.ObterEcomPedidoMagClienteHistStatus();
            statusMagCliente.Insert(0, new ECOM_PEDIDO_CLIENTE_HIST_STA { CODIGO = 0, MOTIVO = "Selecione" });
            ddlMotivo.DataSource = statusMagCliente;
            ddlMotivo.DataBind();
        }
        #endregion

        private void CarregarPedidoHistorico(string pedidoExterno)
        {
            var hist = ecomController.ObterEcomPedidoMagClienteHist(pedidoExterno);

            gvPedidoHist.DataSource = hist;
            gvPedidoHist.DataBind();

            if (hist == null || hist.Count() <= 0)
                MoverAba("1");
        }
        protected void gvPedidoHist_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    ECOM_PEDIDO_CLIENTE_HIST pedido = e.Row.DataItem as ECOM_PEDIDO_CLIENTE_HIST;

                    Literal litData = e.Row.FindControl("litData") as Literal;
                    litData.Text = pedido.DATA_CRIACAO.ToString("dd/MM/yyyy HH:mm");

                    Literal litUsuario = e.Row.FindControl("litUsuario") as Literal;
                    litUsuario.Text = baseController.BuscaUsuario(pedido.USUARIO).NOME_USUARIO;

                    Literal litStatus = e.Row.FindControl("litStatus") as Literal;
                    litStatus.Text = pedido.ECOM_PEDIDO_CLIENTE_HIST_STA1.MOTIVO;

                }
            }
            if (e.Row.DataItemIndex != -1)
            {
                e.Row.Attributes.Add("onMouseover", "this.style.background='#dddddd';this.style.cursor='hand'");
                e.Row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(e.Row.BackColor) + "';this.style.cursor='hand'");
            }
        }
        protected void gvPedidoHist_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = gvPedidoHist.FooterRow;
            if (_footer != null)
            {
            }
        }

        protected void btAdicionar_Click(object sender, EventArgs e)
        {
            try
            {

                MoverAba("1");

                if (ddlMotivo.SelectedValue == "0")
                {
                    labErro.Text = "Selecione o motivo.";
                    return;
                }
                if (txtComentario.Text == "")
                {
                    labErro.Text = "Informe o comentário.";
                    return;
                }

                var cliHist = new ECOM_PEDIDO_CLIENTE_HIST();

                cliHist.PEDIDO_EXTERNO = hidPedidoExterno.Value;
                cliHist.COMENTARIO = txtComentario.Text;
                cliHist.ECOM_PEDIDO_CLIENTE_HIST_STA = Convert.ToInt32(ddlMotivo.SelectedValue);
                cliHist.DATA_CRIACAO = DateTime.Now;
                cliHist.USUARIO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

                ecomController.InserirEcomPedidoMagClienteHist(cliHist);

                txtComentario.Text = "";
                ddlMotivo.SelectedValue = "0";

                CarregarPedidoHistorico(cliHist.PEDIDO_EXTERNO);
                MoverAba("0");

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }


    }
}
