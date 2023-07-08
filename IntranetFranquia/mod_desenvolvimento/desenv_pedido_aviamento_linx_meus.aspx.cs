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
    public partial class desenv_pedido_aviamento_linx_meus : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {

            }
        }

        private void CarregarMeusPedidos(string pedido)
        {
            var meusPedidos = desenvController.ObterCarrinhoMaterialCab().Where(p => p.DATA_PEDIDO != null);

            if (pedido != "")
                meusPedidos = meusPedidos.Where(p => p.PEDIDO.ToLower().Contains(pedido.ToLower())).ToList();

            if (ddlAberto.SelectedValue != "")
            {
                if (ddlAberto.SelectedValue == "S")
                    meusPedidos = meusPedidos.Where(p => p.DATA_FECHAMENTO == null).ToList();
                else
                    meusPedidos = meusPedidos.Where(p => p.DATA_FECHAMENTO != null).ToList();
            }

            gvMeusPedidos.DataSource = meusPedidos;
            gvMeusPedidos.DataBind();

            if (meusPedidos == null || meusPedidos.Count() <= 0)
                labErro.Text = "Nenhum pedido encontrado.";
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

                        Literal litDataPedido = e.Row.FindControl("litDataPedido") as Literal;
                        litDataPedido.Text = (meuPedido.DATA_PEDIDO == null) ? "" : Convert.ToDateTime(meuPedido.DATA_PEDIDO).ToString("dd/MM/yyyy");

                        Literal litDataFechamento = e.Row.FindControl("litDataFechamento") as Literal;
                        litDataFechamento.Text = (meuPedido.DATA_FECHAMENTO == null) ? "" : Convert.ToDateTime(meuPedido.DATA_FECHAMENTO).ToString("dd/MM/yyyy");

                        Button btnAbrirPedido = e.Row.FindControl("btnAbrirPedido") as Button;
                        btnAbrirPedido.CommandArgument = meuPedido.CODIGO.ToString();

                        Button btnFecharPedido = e.Row.FindControl("btnFecharPedido") as Button;
                        btnFecharPedido.CommandArgument = meuPedido.CODIGO.ToString();

                        GridView gvCarrinhoLinxCab = e.Row.FindControl("gvCarrinhoLinxCab") as GridView;
                        if (gvCarrinhoLinxCab != null)
                        {
                            var carrinhoLinxCab = desenvController.ObterCarrinhoMaterialLinxCabPorCarrinhoCab(Convert.ToInt32(meuPedido.CODIGO));
                            gvCarrinhoLinxCab.DataSource = carrinhoLinxCab;
                            gvCarrinhoLinxCab.DataBind();
                        }

                        if (meuPedido.DATA_FECHAMENTO != null)
                        {
                            btnAbrirPedido.Enabled = false;
                            btnFecharPedido.Enabled = false;
                        }
                    }
                }
            }
        }
        protected void gvCarrinhoLinxCab_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    DESENV_MATERIAL_CARRINHO_LINX_CAB meuPedido = e.Row.DataItem as DESENV_MATERIAL_CARRINHO_LINX_CAB;

                    if (meuPedido != null)
                    {

                        Literal litDataAbertura = e.Row.FindControl("litDataAbertura") as Literal;
                        litDataAbertura.Text = (meuPedido.DATA_ABERTURA == null) ? "" : Convert.ToDateTime(meuPedido.DATA_ABERTURA).ToString("dd/MM/yyyy");

                        Literal litDataFechamento = e.Row.FindControl("litDataFechamento") as Literal;
                        litDataFechamento.Text = (meuPedido.DATA_FECHAMENTO == null) ? "" : Convert.ToDateTime(meuPedido.DATA_FECHAMENTO).ToString("dd/MM/yyyy");

                        Button btnVisualizarCarrinho = e.Row.FindControl("btnVisualizarCarrinho") as Button;
                        btnVisualizarCarrinho.CommandArgument = meuPedido.CODIGO.ToString();
                        btnVisualizarCarrinho.Enabled = false;
                        if (meuPedido.DATA_FECHAMENTO != null)
                            btnVisualizarCarrinho.Enabled = true;

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
                Response.Redirect(("desenv_pedido_aviamento_linx.aspx?p=" + codigoCarrinhoCab), "_blank", "");
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                CarregarMeusPedidos(txtPedido.Text.Trim().ToUpper());

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        protected void btnFecharPedido_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                labErro.Text = "";
                Button bt = (Button)sender;
                var codigoCarrinhoCab = bt.CommandArgument;

                var carrinhoCab = desenvController.ObterCarrinhoMaterialCab(Convert.ToInt32(codigoCarrinhoCab));
                if (carrinhoCab != null)
                {
                    carrinhoCab.DATA_FECHAMENTO = DateTime.Now;
                    desenvController.AtualizarCarrinhoMaterialCab(carrinhoCab);
                }

                CarregarMeusPedidos(txtPedido.Text.Trim().ToUpper());
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        protected void btnVisualizarCarrinho_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                Button bt = (Button)sender;
                var codigoCarrinhoLinxCab = bt.CommandArgument;
                var carrinhoCab = desenvController.ObterCarrinhoMaterialLinxCab(Convert.ToInt32(codigoCarrinhoLinxCab));
                var codigoCarrinhoCab = carrinhoCab.DESENV_MATERIAL_CARRINHO_CAB.ToString();

                //abrir pedido
                Response.Redirect(("desenv_pedido_aviamento_linx.aspx?p=" + codigoCarrinhoCab + "&l=" + codigoCarrinhoLinxCab), "_blank", "");
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }



    }

}
