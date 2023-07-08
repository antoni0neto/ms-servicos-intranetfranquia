﻿using DAL;
using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Relatorios
{
    public partial class ecom_cad_produto_faturamento : System.Web.UI.Page
    {
        EcomController eController = new EcomController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregarFaturamento();
            }
        }

        #region "FATURAMENTO"
        private void CarregarFaturamento()
        {
            var fat = eController.ObterPedidoFaturamento("");

            gvFaturamento.DataSource = fat.Where(p => p.PEDIDO_ORIGEM.ToLower() == "site" && p.RETIRADA_LOJA == 'N');
            gvFaturamento.DataBind();

            gvFaturamentoLoja.DataSource = fat.Where(p => p.PEDIDO_ORIGEM.ToLower() == "site" && p.RETIRADA_LOJA == 'S');
            gvFaturamentoLoja.DataBind();

            gvFaturamentoMP.DataSource = fat.Where(p => p.PEDIDO_ORIGEM.ToLower() != "site");
            gvFaturamentoMP.DataBind();
        }
        protected void btAtualizar_Click(object sender, EventArgs e)
        {
            try
            {
                CarregarFaturamento();
            }
            catch (Exception)
            {
            }
        }
        protected void gvFaturamento_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_ECOM_PEDIDO_FATURAMENTOResult magFaturamento = e.Row.DataItem as SP_OBTER_ECOM_PEDIDO_FATURAMENTOResult;

                    if (magFaturamento != null)
                    {
                        Literal litDataPedido = e.Row.FindControl("litDataPedido") as Literal;
                        litDataPedido.Text = Convert.ToDateTime(magFaturamento.EMISSAO).ToString("dd/MM/yyyy");

                        Literal litQtdeProduto = e.Row.FindControl("litQtdeProduto") as Literal;
                        litQtdeProduto.Text = Convert.ToInt32(magFaturamento.QTDE_PRODUTO).ToString();

                        Literal litAbrirPedido = e.Row.FindControl("litAbrirPedido") as Literal;
                        litAbrirPedido.Text = CriarLinkPedido(magFaturamento.PEDIDO_EXTERNO);

                        Button btBaixar = e.Row.FindControl("btBaixar") as Button;
                        btBaixar.Enabled = true;
                        if (magFaturamento.ERRO == "")
                            btBaixar.CommandArgument = magFaturamento.PEDIDO.ToString() + "|" + magFaturamento.TIPO;
                        else
                            btBaixar.Enabled = false;
                    }
                }
            }
        }
        #endregion

        private string CriarLinkPedido(string pedidoExterno)
        {
            var link = "../mod_ecomv2/ecomv2_pedido_mag_det.aspx?p=" + pedidoExterno.Trim();
            var linkOk = "<a href=\"javascript: openwindow('" + link + "')\"><img alt='' src='../../Image/search.png' width='13px' /></a>";
            return linkOk;
        }
        protected void btBaixar_Click(object sender, EventArgs e)
        {
            try
            {
                Button bt = (Button)sender;
                if (bt != null)
                {
                    string pedidoETipo = bt.CommandArgument;

                    var pedido = pedidoETipo.Split('|')[0];
                    var tipo = pedidoETipo.Split('|')[1];

                    string url = "";
                    //Abrir pop-up
                    if (tipo != "TROCAS")
                        url = "fnAbrirTelaCadastroMaior('ecom_cad_produto_faturamento_pop.aspx?pedido=" + pedido + "');";
                    else
                        url = "fnAbrirTelaCadastroMaior('ecom_cad_produto_faturamento_pop_trocas.aspx?pedido=" + pedido + "');";

                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), url, true);

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private USUARIO ObterUsuario()
        {
            if (Session["USUARIO"] != null)
            {
                USUARIO usuario = (USUARIO)Session["USUARIO"];
                return usuario;
            }

            return null;
        }

        protected void btImprimir_Click(object sender, EventArgs e)
        {

        }

    }
}