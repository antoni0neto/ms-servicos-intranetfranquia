using System;
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
    public partial class BuscaPorNomeCliente : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        
        public int totalQtdeOriginal;
        public double totalValorOriginal;
        public int totalQtdeCancelada;
        public double totalValorCancelado;
        public int totalQtdeEntregue;
        public double totalValorEntregue;
        public int totalQtdeEmbalada;
        public int totalQtdeAEntregar;
        public double totalValorAEntregar;
        public int totalQtdeDevolvida;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregaDropDownListColecao();

                USUARIO usuario = null;

                if (Session["USUARIO"] == null)
                    Response.Redirect("~/Login.aspx");
                else
                    usuario = (USUARIO)Session["USUARIO"];
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

        private void CarregaGridViewAtacado(string colecao, string nomeCliente)
        {
            GridViewAtacado.DataSource = baseController.BuscaNomeGestaoAtacado(colecao, nomeCliente);
            GridViewAtacado.DataBind();
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            if (ddlColecao.SelectedValue.ToString().Equals("0") || 
                ddlColecao.SelectedValue.ToString().Equals("")  ||
                txtNomeCliente.Text.Equals(""))
                 return;

            CarregaGridViewAtacado(ddlColecao.SelectedValue, txtNomeCliente.Text);

        }

        protected void GridViewAtacado_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Sp_Nome_Gestao_AtacadoResult sp_Gestao_AtacadoResult = e.Row.DataItem as Sp_Nome_Gestao_AtacadoResult;

            if (sp_Gestao_AtacadoResult != null)
            {
                Literal literalColecao = e.Row.FindControl("LiteralColecao") as Literal;

                if (literalColecao != null)
                    literalColecao.Text = ddlColecao.SelectedValue;

                Literal literalQtdeOriginal = e.Row.FindControl("LiteralQtdeOriginal") as Literal;

                if (literalQtdeOriginal != null)
                    literalQtdeOriginal.Text = (Convert.ToDouble(sp_Gestao_AtacadoResult.QTDE_ORIGINAL)).ToString("###,###,###");

                Literal literalValorOriginal = e.Row.FindControl("LiteralValorOriginal") as Literal;

                if (literalValorOriginal != null)
                    literalValorOriginal.Text = (Convert.ToDouble(sp_Gestao_AtacadoResult.VALOR_ORIGINAL)).ToString("###,###,##0.00");

                Literal literalQtdeCancelada = e.Row.FindControl("LiteralQtdeCancelada") as Literal;

                if (literalQtdeCancelada != null)
                    literalQtdeCancelada.Text = (Convert.ToDouble(sp_Gestao_AtacadoResult.QTDE_CANCELADA)).ToString("###,###,###");

                Literal literalValorCancelado = e.Row.FindControl("LiteralValorCancelado") as Literal;

                if (literalValorCancelado != null)
                    literalValorCancelado.Text = (Convert.ToDouble(sp_Gestao_AtacadoResult.VALOR_CANCELADO)).ToString("###,###,##0.00");
                        
                Literal literalQtdeEntregue = e.Row.FindControl("LiteralQtdeEntregue") as Literal;

                if (literalQtdeEntregue != null)
                    literalQtdeEntregue.Text = (Convert.ToInt32(sp_Gestao_AtacadoResult.QTDE_ENTREGUE)).ToString("###,###,###");

                Literal literalValorEntregue = e.Row.FindControl("LiteralValorEntregue") as Literal;

                if (literalValorEntregue != null)
                    literalValorEntregue.Text = (Convert.ToInt32(sp_Gestao_AtacadoResult.VALOR_ENTREGUE)).ToString("###,###,##0.00");

                Literal literalQtdeAEntregar = e.Row.FindControl("LiteralQtdeAEntregar") as Literal;

                if (literalQtdeAEntregar != null)
                    literalQtdeAEntregar.Text = (Convert.ToInt32(sp_Gestao_AtacadoResult.QTDE_ENTREGAR)).ToString("###,###,###");

                Literal literalValorAEntregar = e.Row.FindControl("LiteralValorAEntregar") as Literal;

                if (literalValorAEntregar != null)
                    literalValorAEntregar.Text = (Convert.ToInt32(sp_Gestao_AtacadoResult.VALOR_ENTREGAR)).ToString("###,###,##0.00");

                Literal literalQtdeEmbalada = e.Row.FindControl("LiteralQtdeEmbalada") as Literal;

                if (literalQtdeEmbalada != null)
                    literalQtdeEmbalada.Text = (Convert.ToInt32(sp_Gestao_AtacadoResult.QTDE_EMBALADA)).ToString("###,###,###");

                Literal literalValorEmbalado = e.Row.FindControl("LiteralValorEmbalado") as Literal;

                if (literalValorEmbalado != null)
                    literalValorEmbalado.Text = (Convert.ToInt32(sp_Gestao_AtacadoResult.VALOR_EMBALADO)).ToString("###,###,##0.00");

                Literal literalQtdeDevolvida = e.Row.FindControl("LiteralQtdeDevolvida") as Literal;

                if (literalQtdeDevolvida != null)
                    literalQtdeDevolvida.Text = (Convert.ToInt32(sp_Gestao_AtacadoResult.QTDE_DEVOLVIDA)).ToString("###,###,###");

                Literal literalValorDevolvido = e.Row.FindControl("LiteralValorDevolvido") as Literal;

                if (literalValorDevolvido != null)
                    literalValorDevolvido.Text = (Convert.ToInt32(sp_Gestao_AtacadoResult.VALOR_DEVOLVIDO)).ToString("###,###,##0.00");

                Literal literalStatus = e.Row.FindControl("LiteralStatus") as Literal;

                if (literalStatus != null)
                {
                    if (sp_Gestao_AtacadoResult.QTDE_ORIGINAL > 0)
                    {
                        if (sp_Gestao_AtacadoResult.STATUS_PEDIDO.Contains("EM ESTUDO") &
                            sp_Gestao_AtacadoResult.CREDITO.Contains("SEM CREDITO"))
                        {
                            if ((Convert.ToDecimal(sp_Gestao_AtacadoResult.QTDE_CANCELADA) / Convert.ToDecimal(sp_Gestao_AtacadoResult.QTDE_ORIGINAL) * 100) > 20)
                                literalStatus.Text = "CANCELADO";
                            else
                                literalStatus.Text = "NOVO";
                        }

                        if (sp_Gestao_AtacadoResult.STATUS_PEDIDO.Contains("EM ESTUDO") &
                            sp_Gestao_AtacadoResult.CREDITO.Contains("LIBERADO"))
                        {
                            if ((Convert.ToDecimal(sp_Gestao_AtacadoResult.QTDE_CANCELADA) / Convert.ToDecimal(sp_Gestao_AtacadoResult.QTDE_ORIGINAL) * 100) > 20)
                                literalStatus.Text = "CANCELADO";
                            else
                                literalStatus.Text = "CONFERIR";
                        }
                        if (sp_Gestao_AtacadoResult.STATUS_PEDIDO.Contains("APROVADO") &
                            sp_Gestao_AtacadoResult.CREDITO.Contains("SEM CREDITO"))
                        {
                            if ((Convert.ToDecimal(sp_Gestao_AtacadoResult.QTDE_CANCELADA) / Convert.ToDecimal(sp_Gestao_AtacadoResult.QTDE_ORIGINAL) * 100) > 20)
                                literalStatus.Text = "CANCELADO";
                            else
                                literalStatus.Text = "FAT. PARCIAL";
                        }
                        if (sp_Gestao_AtacadoResult.STATUS_PEDIDO.Contains("APROVADO") &
                            sp_Gestao_AtacadoResult.CREDITO.Contains("LIBERADO"))
                        {
                            if ((Convert.ToDecimal(sp_Gestao_AtacadoResult.QTDE_CANCELADA) / Convert.ToDecimal(sp_Gestao_AtacadoResult.QTDE_ORIGINAL) * 100) > 20)
                                literalStatus.Text = "CANCELADO";
                            else
                                literalStatus.Text = "FATURAMENTO";
                        }
                    }
                }

                totalQtdeOriginal += Convert.ToInt32(sp_Gestao_AtacadoResult.QTDE_ORIGINAL);
                totalValorOriginal += Convert.ToDouble(sp_Gestao_AtacadoResult.VALOR_ORIGINAL);
                totalQtdeCancelada += Convert.ToInt32(sp_Gestao_AtacadoResult.QTDE_CANCELADA);
                totalValorCancelado += Convert.ToDouble(sp_Gestao_AtacadoResult.VALOR_CANCELADO);
                totalQtdeEntregue += Convert.ToInt32(sp_Gestao_AtacadoResult.QTDE_ENTREGUE);
                totalValorEntregue += Convert.ToDouble(sp_Gestao_AtacadoResult.VALOR_ENTREGUE);
                totalQtdeEmbalada += Convert.ToInt32(sp_Gestao_AtacadoResult.QTDE_EMBALADA);
                totalQtdeAEntregar += Convert.ToInt32(sp_Gestao_AtacadoResult.QTDE_ENTREGAR);
                totalValorAEntregar += Convert.ToDouble(sp_Gestao_AtacadoResult.VALOR_ENTREGAR);
                totalQtdeDevolvida += Convert.ToInt32(sp_Gestao_AtacadoResult.QTDE_DEVOLVIDA);
            }
        }

        protected void GridViewAtacado_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = GridViewAtacado.FooterRow;

            foreach (GridViewRow item in GridViewAtacado.Rows)
            {
                Literal literalStatus = item.FindControl("LiteralStatus") as Literal;

                if (literalStatus.Text.Contains("NOVO"))
                    item.Cells[19].ForeColor = System.Drawing.Color.Red;

                if (literalStatus.Text.Contains("CANCELADO"))
                    item.Cells[19].ForeColor = System.Drawing.Color.Pink;

                if (literalStatus.Text.Contains("CONFERIR"))
                    item.Cells[19].ForeColor = System.Drawing.Color.Green;

                if (literalStatus.Text.Contains("FAT. PARCIAL"))
                    item.Cells[19].ForeColor = System.Drawing.Color.Blue;

                if (literalStatus.Text.Contains("FATURAMENTO"))
                    item.Cells[19].ForeColor = System.Drawing.Color.Orange;
            }

            if (footer != null)
            {
                footer.Cells[0].Text = "Totais";
                footer.Cells[0].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[9].Text = totalQtdeOriginal.ToString("###,###,###");
                footer.Cells[9].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[9].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[10].Text = totalValorOriginal.ToString("###,###,##0.00");
                footer.Cells[10].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[11].Text = totalQtdeCancelada.ToString("###,###,###");
                footer.Cells[11].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[12].Text = totalValorCancelado.ToString("###,###,##0.00");
                footer.Cells[12].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[13].Text = totalQtdeEntregue.ToString("###,###,###");
                footer.Cells[13].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[14].Text = totalValorEntregue.ToString("###,###,##0.00");
                footer.Cells[14].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[15].Text = totalQtdeEmbalada.ToString("###,###,###");
                footer.Cells[15].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[16].Text = totalQtdeAEntregar.ToString("###,###,###");
                footer.Cells[16].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[17].Text = totalValorAEntregar.ToString("###,###,##0.00");
                footer.Cells[17].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[18].Text = totalQtdeDevolvida.ToString("###,###,###");
                footer.Cells[18].BackColor = System.Drawing.Color.PeachPuff;
            }
        }
    }
}
