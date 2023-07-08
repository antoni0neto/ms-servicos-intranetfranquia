﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.DataVisualization.Charting;
using DAL;

namespace Relatorios
{
    public partial class VendasAtacadoPorGrupo : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();

        public double vlTotalOriginal;
        public double vlTotalCancelado;
        public double vlTotalReal;
        public double vlTotalEntregue;
        public double vlTotalEntregar;

        public int qtTotalOriginal;
        public int qtTotalCancelado;
        public int qtTotalReal;
        public int qtTotalEntregue;
        public int qtTotalEntregar;
        public int qtTotalEstoque;
        public int qtRealJanelaAnterior;
        public int qtEntregueJanelaAnterior;
        public int qtTotalRealJanelaAnterior;
        public int qtTotalEntregueJanelaAnterior;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregaDropDownListColecao();
                CarregaDropDownListGriffe();

                USUARIO usuario = null;

                if (Session["USUARIO"] == null)
                    Response.Redirect("~/Login.aspx");
                else
                    usuario = (USUARIO)Session["USUARIO"];

                if (usuario.CODIGO_PERFIL == 1)
                {
                    txtDataCorte.Enabled = true;
                }

                DATAATACADO dataAtacado = baseController.BuscaDataCorteAtacado();

                if (dataAtacado == null)
                    txtDataCorte.Text = "";
                else
                    txtDataCorte.Text = dataAtacado.DATA;
            }
        }

        private void CarregaDropDownListColecao()
        {
            ddlColecao.DataSource = baseController.BuscaColecoes();
            ddlColecao.DataBind();
        }

        protected void ddlColecao_DataBound(object sender, EventArgs e)
        {
            ddlColecao.Items.Add(new ListItem("Selecione", "0"));
            ddlColecao.SelectedValue = "0";
        }

        private void CarregaDropDownListGriffe()
        {
            ddlGriffe.DataSource = baseController.BuscaGriffes();
            ddlGriffe.DataBind();
        }

        protected void ddlGriffe_DataBound(object sender, EventArgs e)
        {
            ddlGriffe.Items.Add(new ListItem("Selecione", "0"));
            ddlGriffe.SelectedValue = "0";
        }

        private void CarregaGridViewJanela1(string data, string tipoProduto, string colecao, string griffe)
        {
            GridViewJanela1.DataSource = baseController.BuscaVendasAtacadoGriffe(data, tipoProduto, colecao, griffe);
            GridViewJanela1.DataBind();
        }

        private void CarregaGridViewJanela2(string data, string tipoProduto, string colecao, string griffe)
        {
            GridViewJanela2.DataSource = baseController.BuscaVendasAtacadoGriffe_2(data, tipoProduto, colecao, griffe);
            GridViewJanela2.DataBind();
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            if (ddlColecao.SelectedValue.ToString().Equals("0") ||
                ddlColecao.SelectedValue.ToString().Equals("")  || 
                ddlGriffe.SelectedValue.ToString().Equals("0")  || 
                ddlGriffe.SelectedValue.ToString().Equals("")   ||
                ddlTipoProduto.SelectedValue.ToString().Equals("Selecione"))
                 return;

            if (txtDataCorte.Text.Equals(""))
                CarregaGridViewJanela1(baseController.AjustaData(DateTime.Now.ToString()), ddlTipoProduto.SelectedValue, ddlColecao.SelectedValue, ddlGriffe.SelectedItem.ToString());
            else
            {
                CarregaGridViewJanela1(txtDataCorte.Text, ddlTipoProduto.SelectedValue, ddlColecao.SelectedValue, ddlGriffe.SelectedItem.ToString());
                CarregaGridViewJanela2(txtDataCorte.Text, ddlTipoProduto.SelectedValue, ddlColecao.SelectedValue, ddlGriffe.SelectedItem.ToString());

                DATAATACADO dataAtacado = new DATAATACADO();

                dataAtacado.DATA = txtDataCorte.Text;

                baseController.GravarDataCorte(dataAtacado);
            }
        }
        /*
        protected void GridViewJanela1_DataBound(object sender, EventArgs e)
        {
            vlTotalOriginal = 0;
            vlTotalCancelado = 0;
            vlTotalReal = 0;
            vlTotalEntregue = 0;
            vlTotalEntregar = 0;

            qtTotalOriginal = 0;
            qtTotalCancelado = 0;
            qtTotalReal = 0;
            qtTotalEntregue = 0;
            qtTotalEntregar = 0;
            qtTotalEstoque = 0;

            GridViewRow footer = GridViewJanela1.FooterRow;

            foreach (GridViewRow item in GridViewJanela1.Rows)
            {
                vlTotalOriginal += Convert.ToDouble(item.Cells[5].Text);
                vlTotalEntregue += Convert.ToDouble(item.Cells[12].Text);
                vlTotalEntregar += Convert.ToDouble(item.Cells[15].Text);
                vlTotalCancelado += Convert.ToDouble(item.Cells[7].Text);
                vlTotalReal += vlTotalOriginal - vlTotalCancelado;

                qtTotalOriginal += Convert.ToInt32(item.Cells[4].Text);
                qtTotalEntregue += Convert.ToInt32(item.Cells[11].Text);
                qtTotalEntregar += Convert.ToInt32(item.Cells[14].Text);
                qtTotalCancelado += Convert.ToInt32(item.Cells[6].Text);
                qtTotalEstoque += Convert.ToInt32(item.Cells[17].Text);
                qtTotalReal += qtTotalOriginal - qtTotalCancelado;
            }

            if (footer != null)
            {
                footer.Cells[1].Text = "Totais";
                footer.Cells[4].Text = (qtTotalOriginal).ToString("###,###,###");
                footer.Cells[5].Text = (vlTotalOriginal).ToString("###,###,##0.00");
                footer.Cells[6].Text = (qtTotalCancelado).ToString("###,###,###");
                footer.Cells[7].Text = (vlTotalCancelado).ToString("###,###,##0.00");
                footer.Cells[8].Text = ((Convert.ToDouble(qtTotalCancelado) / Convert.ToDouble(qtTotalOriginal)) * 100).ToString("N2") + "%";
                footer.Cells[9].Text = (qtTotalOriginal - qtTotalCancelado).ToString("###,###,###");
                footer.Cells[10].Text = (vlTotalOriginal - vlTotalCancelado).ToString("###,###,##0.00");
                footer.Cells[11].Text = (qtTotalEntregue).ToString("###,###,###");
                footer.Cells[12].Text = (vlTotalEntregue).ToString("###,###,##0.00");
                footer.Cells[13].Text = ((Convert.ToDouble(qtTotalEntregue) / Convert.ToDouble(qtTotalReal)) * 100).ToString("N2") + "%";
                footer.Cells[14].Text = (qtTotalEntregar).ToString("###,###,###");
                footer.Cells[15].Text = (vlTotalEntregar).ToString("###,###,##0.00");
                footer.Cells[16].Text = ((Convert.ToDouble(qtTotalEntregar) / Convert.ToDouble(qtTotalReal)) * 100).ToString("N2") + "%";
                footer.Cells[17].Text = (qtTotalEstoque).ToString("###,###,###");
                footer.Cells[18].Text = ((Convert.ToDouble(qtTotalEntregue) / ((Convert.ToDouble(qtTotalOriginal) - Convert.ToDouble(qtTotalCancelado)) + Convert.ToDouble(qtTotalEstoque))) * 100).ToString("N2") + "%";
            }
        }

        protected void GridViewJanela2_DataBound(object sender, EventArgs e)
        {        
            vlTotalOriginal = 0;
            vlTotalCancelado = 0;
            vlTotalReal = 0;
            vlTotalEntregue = 0;
            vlTotalEntregar = 0;

            qtTotalOriginal = 0;
            qtTotalCancelado = 0;
            qtTotalReal = 0;
            qtTotalEntregue = 0;
            qtTotalEntregar = 0;
            qtTotalEstoque = 0;
            qtRealJanelaAnterior = 0;
            qtEntregueJanelaAnterior = 0;
            qtTotalRealJanelaAnterior = 0;
            qtTotalEntregueJanelaAnterior = 0;

            GridViewRow footer = GridViewJanela2.FooterRow;

            foreach (GridViewRow item in GridViewJanela2.Rows)
            {
                vlTotalOriginal += Convert.ToDouble(item.Cells[5].Text);
                vlTotalEntregue += Convert.ToDouble(item.Cells[12].Text);
                vlTotalEntregar += Convert.ToDouble(item.Cells[15].Text);
                vlTotalCancelado += Convert.ToDouble(item.Cells[7].Text);
                vlTotalReal += vlTotalOriginal - vlTotalCancelado;

                qtTotalOriginal += Convert.ToInt32(item.Cells[4].Text);
                qtTotalEntregue += Convert.ToInt32(item.Cells[11].Text);
                qtTotalEntregar += Convert.ToInt32(item.Cells[14].Text);
                qtTotalCancelado += Convert.ToInt32(item.Cells[6].Text);
                qtTotalEstoque += Convert.ToInt32(item.Cells[17].Text);
                qtTotalReal += qtTotalOriginal - qtTotalCancelado;
            }

            if (footer != null)
            {
                footer.Cells[1].Text = "Totais";
                footer.Cells[4].Text = (qtTotalOriginal).ToString("###,###,###");
                footer.Cells[5].Text = (vlTotalOriginal).ToString("###,###,##0.00");
                footer.Cells[6].Text = (qtTotalCancelado).ToString("###,###,###");
                footer.Cells[7].Text = (vlTotalCancelado).ToString("###,###,##0.00");
                footer.Cells[8].Text = ((Convert.ToDouble(qtTotalCancelado) / Convert.ToDouble(qtTotalOriginal)) * 100).ToString("N2") + "%";
                footer.Cells[9].Text = (qtTotalOriginal - qtTotalCancelado).ToString("###,###,###");
                footer.Cells[10].Text = (vlTotalOriginal - vlTotalCancelado).ToString("###,###,##0.00");
                footer.Cells[11].Text = (qtTotalEntregue).ToString("###,###,###");
                footer.Cells[12].Text = (vlTotalEntregue).ToString("###,###,##0.00");
                footer.Cells[13].Text = ((Convert.ToDouble(qtTotalEntregue) / Convert.ToDouble(qtTotalReal)) * 100).ToString("N2") + "%";
                footer.Cells[14].Text = (qtTotalEntregar).ToString("###,###,###");
                footer.Cells[15].Text = (vlTotalEntregar).ToString("###,###,##0.00");
                footer.Cells[16].Text = ((Convert.ToDouble(qtTotalEntregar) / Convert.ToDouble(qtTotalReal)) * 100).ToString("N2") + "%";
                footer.Cells[17].Text = (qtTotalEstoque).ToString("###,###,###");
                footer.Cells[18].Text = ((Convert.ToDouble(qtTotalEntregue) / ((qtTotalRealJanelaAnterior + Convert.ToDouble(qtTotalEstoque)) - qtEntregueJanelaAnterior)) * 100).ToString("N2") + "%";
            }
        }
        */
        protected void GridViewJanela1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Sp_Vendas_Atacado_GriffeResult sp_Vendas_AtacadoResult = e.Row.DataItem as Sp_Vendas_Atacado_GriffeResult;

            if (sp_Vendas_AtacadoResult != null)
            {
                Literal literalColecao = e.Row.FindControl("LiteralColecao") as Literal;

                if (literalColecao != null)
                    literalColecao.Text = ddlColecao.SelectedValue;

                Literal literalOrigem = e.Row.FindControl("LiteralOrigem") as Literal;

                if (literalOrigem != null)
                    literalOrigem.Text = ddlTipoProduto.SelectedValue;

                Literal literalGriffe = e.Row.FindControl("LiteralGriffe") as Literal;

                if (literalGriffe != null)
                    literalGriffe.Text = ddlGriffe.SelectedValue;

                Literal literalTotalQtdeOriginal = e.Row.FindControl("LiteralTotalQtdeOriginal") as Literal;

                if (literalTotalQtdeOriginal != null)
                    literalTotalQtdeOriginal.Text = (Convert.ToDouble(sp_Vendas_AtacadoResult.TOTAL_QTDE_ORIGINAL)).ToString("###,###,###");

                Literal literalTotalValorOriginal = e.Row.FindControl("LiteralTotalValorOriginal") as Literal;

                if (literalTotalValorOriginal != null)
                    literalTotalValorOriginal.Text = (Convert.ToDouble(sp_Vendas_AtacadoResult.TOTAL_VALOR_ORIGINAL)).ToString("###,###,##0.00");

                Literal literalTotalQtdeCancelada = e.Row.FindControl("LiteralTotalQtdeCancelada") as Literal;

                if (literalTotalQtdeCancelada != null)
                    literalTotalQtdeCancelada.Text = (Convert.ToDouble(sp_Vendas_AtacadoResult.TOTAL_QTDE_CANCELADA)).ToString("###,###,###");

                Literal literalTotalValorCancelado = e.Row.FindControl("LiteralTotalValorCancelado") as Literal;

                if (literalTotalValorCancelado != null)
                    literalTotalValorCancelado.Text = (Convert.ToDouble(sp_Vendas_AtacadoResult.TOTAL_VALOR_CANCELADO)).ToString("###,###,##0.00");

                Literal literalPercCancelado = e.Row.FindControl("LiteralPercCancelado") as Literal;

                if (literalPercCancelado != null)
                    literalPercCancelado.Text = ((Convert.ToDouble(sp_Vendas_AtacadoResult.TOTAL_QTDE_CANCELADA) / Convert.ToDouble(sp_Vendas_AtacadoResult.TOTAL_QTDE_ORIGINAL)) * 100).ToString("N2") + "%";

                Literal literalQtReal = e.Row.FindControl("LiteralQtReal") as Literal;

                if (literalQtReal != null)
                    literalQtReal.Text = (Convert.ToInt32(sp_Vendas_AtacadoResult.TOTAL_QTDE_ORIGINAL) - Convert.ToInt32(sp_Vendas_AtacadoResult.TOTAL_QTDE_CANCELADA)).ToString("###,###,###");

                Literal literalVlReal = e.Row.FindControl("LiteralVlReal") as Literal;

                if (literalVlReal != null)
                    literalVlReal.Text = (Convert.ToDouble(sp_Vendas_AtacadoResult.TOTAL_VALOR_ORIGINAL) - Convert.ToDouble(sp_Vendas_AtacadoResult.TOTAL_VALOR_CANCELADO)).ToString("###,###,##0.00");

                Literal literalTotalQtdeEntregue = e.Row.FindControl("LiteralTotalQtdeEntregue") as Literal;

                if (literalTotalQtdeEntregue != null)
                    literalTotalQtdeEntregue.Text = (Convert.ToInt32(sp_Vendas_AtacadoResult.TOTAL_QTDE_ENTREGUE)).ToString("###,###,###");

                Literal literalTotalValorEntregue = e.Row.FindControl("LiteralTotalValorEntregue") as Literal;

                if (literalTotalValorEntregue != null)
                    literalTotalValorEntregue.Text = (Convert.ToInt32(sp_Vendas_AtacadoResult.TOTAL_VALOR_ENTREGUE)).ToString("###,###,##0.00");

                Literal literalPercEntregue = e.Row.FindControl("LiteralPercEntregue") as Literal;

                if (literalPercEntregue != null)
                    literalPercEntregue.Text = ((Convert.ToDouble(sp_Vendas_AtacadoResult.TOTAL_QTDE_ENTREGUE) / (Convert.ToDouble(sp_Vendas_AtacadoResult.TOTAL_QTDE_ORIGINAL) - Convert.ToDouble(sp_Vendas_AtacadoResult.TOTAL_QTDE_CANCELADA))) * 100).ToString("N2") + "%";

                Literal literalTotalQtdeEntregar = e.Row.FindControl("LiteralTotalQtdeEntregar") as Literal;

                if (literalTotalQtdeEntregar != null)
                    literalTotalQtdeEntregar.Text = (Convert.ToInt32(sp_Vendas_AtacadoResult.TOTAL_QTDE_ENTREGAR)).ToString("###,###,###");

                Literal literalTotalValorEntregar = e.Row.FindControl("LiteralTotalValorEntregar") as Literal;

                if (literalTotalValorEntregar != null)
                    literalTotalValorEntregar.Text = (Convert.ToInt32(sp_Vendas_AtacadoResult.TOTAL_VALOR_ENTREGAR)).ToString("###,###,##0.00");

                Literal literalEstoque = e.Row.FindControl("LiteralEstoque") as Literal;

                if (literalEstoque != null)
                    literalEstoque.Text = (Convert.ToInt32(sp_Vendas_AtacadoResult.ESTOQUE_HBF)).ToString("###,###,###");

                Literal literalPercEntregar = e.Row.FindControl("LiteralPercEntregar") as Literal;

                if (literalPercEntregar != null)
                    literalPercEntregar.Text = ((Convert.ToDouble(sp_Vendas_AtacadoResult.TOTAL_QTDE_ENTREGAR) / (Convert.ToDouble(sp_Vendas_AtacadoResult.TOTAL_QTDE_ORIGINAL) - Convert.ToDouble(sp_Vendas_AtacadoResult.TOTAL_QTDE_CANCELADA))) * 100).ToString("N2") + "%";

                Literal literalPercRetorno = e.Row.FindControl("LiteralPercRetorno") as Literal;

                if (literalPercRetorno != null)
                    literalPercRetorno.Text = ((Convert.ToDouble(sp_Vendas_AtacadoResult.TOTAL_QTDE_ENTREGUE) / ((Convert.ToDouble(sp_Vendas_AtacadoResult.TOTAL_QTDE_ORIGINAL) - Convert.ToDouble(sp_Vendas_AtacadoResult.TOTAL_QTDE_CANCELADA)) + Convert.ToDouble(sp_Vendas_AtacadoResult.ESTOQUE_HBF))) * 100).ToString("N2") + "%";
            }
        }

        protected void GridViewJanela2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Sp_Vendas_Atacado_Griffe_2Result sp_Vendas_AtacadoResult = e.Row.DataItem as Sp_Vendas_Atacado_Griffe_2Result;

            if (sp_Vendas_AtacadoResult != null)
            {
                Literal literalColecao = e.Row.FindControl("LiteralColecao") as Literal;

                if (literalColecao != null)
                    literalColecao.Text = ddlColecao.SelectedValue;

                Literal literalOrigem = e.Row.FindControl("LiteralOrigem") as Literal;

                if (literalOrigem != null)
                    literalOrigem.Text = ddlTipoProduto.SelectedValue;

                Literal literalGriffe = e.Row.FindControl("LiteralGriffe") as Literal;

                if (literalGriffe != null)
                    literalGriffe.Text = ddlGriffe.SelectedValue;

                Literal literalTotalQtdeOriginal = e.Row.FindControl("LiteralTotalQtdeOriginal") as Literal;

                if (literalTotalQtdeOriginal != null)
                    literalTotalQtdeOriginal.Text = (Convert.ToDouble(sp_Vendas_AtacadoResult.TOTAL_QTDE_ORIGINAL)).ToString("###,###,###");

                Literal literalTotalValorOriginal = e.Row.FindControl("LiteralTotalValorOriginal") as Literal;

                if (literalTotalValorOriginal != null)
                    literalTotalValorOriginal.Text = (Convert.ToDouble(sp_Vendas_AtacadoResult.TOTAL_VALOR_ORIGINAL)).ToString("###,###,##0.00");

                Literal literalTotalQtdeCancelada = e.Row.FindControl("LiteralTotalQtdeCancelada") as Literal;

                if (literalTotalQtdeCancelada != null)
                    literalTotalQtdeCancelada.Text = (Convert.ToDouble(sp_Vendas_AtacadoResult.TOTAL_QTDE_CANCELADA)).ToString("###,###,###");

                Literal literalTotalValorCancelado = e.Row.FindControl("LiteralTotalValorCancelado") as Literal;

                if (literalTotalValorCancelado != null)
                    literalTotalValorCancelado.Text = (Convert.ToDouble(sp_Vendas_AtacadoResult.TOTAL_VALOR_CANCELADO)).ToString("###,###,##0.00");

                Literal literalPercCancelado = e.Row.FindControl("LiteralPercCancelado") as Literal;

                if (literalPercCancelado != null)
                    literalPercCancelado.Text = ((Convert.ToDouble(sp_Vendas_AtacadoResult.TOTAL_QTDE_CANCELADA) / Convert.ToDouble(sp_Vendas_AtacadoResult.TOTAL_QTDE_ORIGINAL)) * 100).ToString("N2") + "%";

                Literal literalQtReal = e.Row.FindControl("LiteralQtReal") as Literal;

                if (literalQtReal != null)
                    literalQtReal.Text = (Convert.ToInt32(sp_Vendas_AtacadoResult.TOTAL_QTDE_ORIGINAL) - Convert.ToInt32(sp_Vendas_AtacadoResult.TOTAL_QTDE_CANCELADA)).ToString("###,###,###");

                Literal literalVlReal = e.Row.FindControl("LiteralVlReal") as Literal;

                if (literalVlReal != null)
                    literalVlReal.Text = (Convert.ToDouble(sp_Vendas_AtacadoResult.TOTAL_VALOR_ORIGINAL) - Convert.ToDouble(sp_Vendas_AtacadoResult.TOTAL_VALOR_CANCELADO)).ToString("###,###,##0.00");

                Literal literalTotalQtdeEntregue = e.Row.FindControl("LiteralTotalQtdeEntregue") as Literal;

                if (literalTotalQtdeEntregue != null)
                    literalTotalQtdeEntregue.Text = (Convert.ToInt32(sp_Vendas_AtacadoResult.TOTAL_QTDE_ENTREGUE)).ToString("###,###,###");

                Literal literalTotalValorEntregue = e.Row.FindControl("LiteralTotalValorEntregue") as Literal;

                if (literalTotalValorEntregue != null)
                    literalTotalValorEntregue.Text = (Convert.ToInt32(sp_Vendas_AtacadoResult.TOTAL_VALOR_ENTREGUE)).ToString("###,###,##0.00");

                Literal literalPercEntregue = e.Row.FindControl("LiteralPercEntregue") as Literal;

                if (literalPercEntregue != null)
                    literalPercEntregue.Text = ((Convert.ToDouble(sp_Vendas_AtacadoResult.TOTAL_QTDE_ENTREGUE) / (Convert.ToDouble(sp_Vendas_AtacadoResult.TOTAL_QTDE_ORIGINAL) - Convert.ToDouble(sp_Vendas_AtacadoResult.TOTAL_QTDE_CANCELADA))) * 100).ToString("N2") + "%";

                Literal literalTotalQtdeEntregar = e.Row.FindControl("LiteralTotalQtdeEntregar") as Literal;

                if (literalTotalQtdeEntregar != null)
                    literalTotalQtdeEntregar.Text = (Convert.ToInt32(sp_Vendas_AtacadoResult.TOTAL_QTDE_ENTREGAR)).ToString("###,###,###");

                Literal literalTotalValorEntregar = e.Row.FindControl("LiteralTotalValorEntregar") as Literal;

                if (literalTotalValorEntregar != null)
                    literalTotalValorEntregar.Text = (Convert.ToInt32(sp_Vendas_AtacadoResult.TOTAL_VALOR_ENTREGAR)).ToString("###,###,##0.00");

                Literal literalEstoque = e.Row.FindControl("LiteralEstoque") as Literal;

                if (literalEstoque != null)
                    literalEstoque.Text = (Convert.ToInt32(sp_Vendas_AtacadoResult.ESTOQUE_HBF)).ToString("###,###,###");

                Literal literalPercEntregar = e.Row.FindControl("LiteralPercEntregar") as Literal;

                if (literalPercEntregar != null)
                    literalPercEntregar.Text = ((Convert.ToDouble(sp_Vendas_AtacadoResult.TOTAL_QTDE_ENTREGAR) / (Convert.ToDouble(sp_Vendas_AtacadoResult.TOTAL_QTDE_ORIGINAL) - Convert.ToDouble(sp_Vendas_AtacadoResult.TOTAL_QTDE_CANCELADA))) * 100).ToString("N2") + "%";

                Literal literalPercRetorno = e.Row.FindControl("LiteralPercRetorno") as Literal;

                if (literalPercRetorno != null)
                {
                    Sp_Vendas_Atacado_GrupoResult vendasAtacado = baseController.BuscaVendaAtacadoGrupoAnterior(txtDataCorte.Text, ddlTipoProduto.SelectedValue, ddlColecao.SelectedValue, ddlGriffe.SelectedItem.ToString(), sp_Vendas_AtacadoResult.GRUPO_PRODUTO);

                    qtRealJanelaAnterior = vendasAtacado.TOTAL_QTDE_ORIGINAL - vendasAtacado.TOTAL_QTDE_CANCELADA;
                    qtEntregueJanelaAnterior = vendasAtacado.TOTAL_QTDE_ENTREGUE;

                    literalPercRetorno.Text = ((Convert.ToDouble(sp_Vendas_AtacadoResult.TOTAL_QTDE_ENTREGUE) / ((qtRealJanelaAnterior + Convert.ToDouble(sp_Vendas_AtacadoResult.ESTOQUE_HBF)) - qtEntregueJanelaAnterior)) * 100).ToString("N2") + "%";

                    qtTotalRealJanelaAnterior += qtRealJanelaAnterior;
                    qtTotalEntregueJanelaAnterior += qtEntregueJanelaAnterior;
                }
            }
        }
    }
}
