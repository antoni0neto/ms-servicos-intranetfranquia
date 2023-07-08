using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Drawing;
using System.Web.UI.HtmlControls;
using System.Drawing.Drawing2D;
using DAL;
using System.Text;
using System.Data.OleDb;

namespace Relatorios
{
    public partial class desenv_pedido_aviamento_meus : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                CarregarColecoes();
                CarregarPedidoPerfil();
                CarregarMeusPedidos();
            }
        }

        private void CarregarColecoes()
        {
            var colecoes = baseController.BuscaColecoes();
            colecoes.Insert(0, new COLECOE { COLECAO = "", DESC_COLECAO = "Selecione" });

            ddlColecao.DataSource = colecoes;
            ddlColecao.DataBind();

            if (Session["COLECAO"] != null)
            {
                ddlColecao.SelectedValue = Session["COLECAO"].ToString();
            }

        }
        private void CarregarPedidoPerfil()
        {
            var perfil = desenvController.ObterCarrinhoMaterialPerfil();
            perfil.Insert(0, new DESENV_MATERIAL_PEDIDO_PERFIL { CODIGO = 0, PERFIL = "Selecione" });
            ddlTipoCompra.DataSource = perfil;
            ddlTipoCompra.DataBind();
        }

        private void CarregarMeusPedidos()
        {

            if (Session["USUARIO"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            var meusPedidos = desenvController.ObterCarrinhoMaterialCab();

            if (txtPedidoFiltro.Text.Trim() != "")
                meusPedidos = meusPedidos.Where(p => p.PEDIDO.ToLower().Contains(txtPedidoFiltro.Text.Trim().ToLower())).ToList();

            if (ddlMeusFiltro.SelectedValue != "")
            {
                var codigoUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

                if (ddlMeusFiltro.SelectedValue == "S")
                    meusPedidos = meusPedidos.Where(p => p.USUARIO == codigoUsuario).ToList();
                else
                    meusPedidos = meusPedidos.Where(p => p.USUARIO != codigoUsuario).ToList();
            }

            gvMeusPedidos.DataSource = meusPedidos;
            gvMeusPedidos.DataBind();

        }

        protected void gvMeusPedidos_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    DESENV_MATERIAL_CARRINHO_CAB meuPedido = e.Row.DataItem as DESENV_MATERIAL_CARRINHO_CAB;

                    if (meuPedido != null)
                    {
                        Literal litPedido = e.Row.FindControl("litPedido") as Literal;
                        litPedido.Text = meuPedido.PEDIDO;

                        Literal litTipoCompra = e.Row.FindControl("litTipoCompra") as Literal;
                        litTipoCompra.Text = meuPedido.DESENV_MATERIAL_PEDIDO_PERFIL1.PERFIL;

                        Literal litDataAbertura = e.Row.FindControl("litDataAbertura") as Literal;
                        litDataAbertura.Text = (meuPedido.DATA_ABERTURA == null) ? "" : Convert.ToDateTime(meuPedido.DATA_ABERTURA).ToString("dd/MM/yyyy");

                        Literal litDataPedido = e.Row.FindControl("litDataPedido") as Literal;
                        litDataPedido.Text = (meuPedido.DATA_PEDIDO == null) ? "-" : Convert.ToDateTime(meuPedido.DATA_PEDIDO).ToString("dd/MM/yyyy");

                        Literal litDataFechamento = e.Row.FindControl("litDataFechamento") as Literal;
                        litDataFechamento.Text = (meuPedido.DATA_FECHAMENTO == null) ? "-" : Convert.ToDateTime(meuPedido.DATA_FECHAMENTO).ToString("dd/MM/yyyy");

                        Button btnAbrirPedido = e.Row.FindControl("btnAbrirPedido") as Button;
                        btnAbrirPedido.CommandArgument = meuPedido.CODIGO.ToString();

                        Button btnExcluir = e.Row.FindControl("btnExcluir") as Button;
                        btnExcluir.CommandArgument = meuPedido.CODIGO.ToString();

                        if (meuPedido.DATA_PEDIDO != null)
                        {
                            btnExcluir.Enabled = false;
                        }
                    }
                }
            }
        }

        protected void btnAbrirPedido_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                Button bt = (Button)sender;
                var codigoCarrinhoCab = bt.CommandArgument;

                //abrir pedido
                Response.Redirect(("desenv_pedido_aviamento.aspx?p=" + codigoCarrinhoCab), "_blank", "");
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        protected void btnExcluir_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                Button btn = (Button)sender;

                string codigoCarrinhoCab = btn.CommandArgument;

                desenvController.ExcluirCarrinhoMaterialEstoquePorCab(Convert.ToInt32(codigoCarrinhoCab));
                desenvController.ExcluirCarrinhoMaterialPorCab(Convert.ToInt32(codigoCarrinhoCab));
                desenvController.ExcluirCarrinhoMaterialCab(Convert.ToInt32(codigoCarrinhoCab));

                CarregarMeusPedidos();

            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void btnCriarPedido_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                var colecao = ddlColecao.SelectedValue.Trim();
                var pedido = txtPedido.Text.Trim().ToUpper();
                if (colecao == "")
                {
                    labErro.Text = "Selecione a Coleção.";
                    return;
                }
                if (pedido == "")
                {
                    labErro.Text = "Informe o Pedido.";
                    return;
                }
                if (ddlTipoCompra.SelectedValue == "0")
                {
                    labErro.Text = "Selecione o Tipo de Compra.";
                    return;
                }

                var pedidoExiste = desenvController.ObterCarrinhoMaterialCabPorPedido(pedido);
                if (pedidoExiste != null)
                {
                    labErro.Text = "Este pedido já existe.";
                    return;
                }

                var carrinhoCab = new DESENV_MATERIAL_CARRINHO_CAB();
                carrinhoCab.COLECAO = colecao;
                carrinhoCab.PEDIDO = pedido;
                carrinhoCab.USUARIO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                carrinhoCab.DATA_ABERTURA = DateTime.Now;
                carrinhoCab.DESENV_MATERIAL_PEDIDO_PERFIL = Convert.ToInt32(ddlTipoCompra.SelectedValue);
                desenvController.InserirCarrinhoMaterialCab(carrinhoCab);

                CarregarMeusPedidos();

                txtPedido.Text = "";
                ddlTipoCompra.SelectedValue = "0";
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }

        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            CarregarMeusPedidos();
        }

    }

}
