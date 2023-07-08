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
    public partial class BuscaPorStatus : System.Web.UI.Page
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

        private void CarregaGridViewAtacado(string colecao, string status)
        {
            if (status.Equals("0"))
            {
                GridViewAtacado.DataSource = baseController.BuscaGestaoAtacado(colecao);
                GridViewAtacado.DataBind();
            }
            else
            {
                List<Sp_Gestao_AtacadoResult> listaNovo = new List<Sp_Gestao_AtacadoResult>();
                List<Sp_Gestao_AtacadoResult> listaConferir = new List<Sp_Gestao_AtacadoResult>();
                List<Sp_Gestao_AtacadoResult> listaParcial = new List<Sp_Gestao_AtacadoResult>();
                List<Sp_Gestao_AtacadoResult> listaFaturamento = new List<Sp_Gestao_AtacadoResult>();
                List<Sp_Gestao_AtacadoResult> listaCancelado = new List<Sp_Gestao_AtacadoResult>();
                List<Sp_Gestao_AtacadoResult> listaFatTotal = new List<Sp_Gestao_AtacadoResult>();
                List<Sp_Gestao_AtacadoResult> listaSemCredito = new List<Sp_Gestao_AtacadoResult>();
                List<Sp_Gestao_AtacadoResult> lista = baseController.BuscaGestaoAtacado(colecao);

                if (lista != null)
                {
                    foreach (Sp_Gestao_AtacadoResult item in lista)
                    {
                        if (item.STATUS_PEDIDO.Contains("EM ESTUDO") & item.CREDITO.Contains("SEM CREDITO"))
                        {
                            if ((Convert.ToDecimal(item.QTDE_ORIGINAL)  != Convert.ToDecimal(item.QTDE_CANCELADA)) &
                                (Convert.ToDecimal(item.VALOR_ORIGINAL) != Convert.ToDecimal(item.VALOR_CANCELADO)) &
                               ((Convert.ToDecimal(item.QTDE_ORIGINAL)  -  Convert.ToDecimal(item.QTDE_CANCELADA))  == Convert.ToDecimal(item.QTDE_ENTREGUE)) &
                               ((Convert.ToDecimal(item.VALOR_ORIGINAL) -  Convert.ToDecimal(item.VALOR_CANCELADO)) == Convert.ToDecimal(item.VALOR_ENTREGUE)))
                                listaFatTotal.Add(item);
                            else
                                if (Convert.ToDecimal(item.QTDE_ORIGINAL) > 0)
                                {
                                    if ((Convert.ToDecimal(item.QTDE_CANCELADA) / Convert.ToDecimal(item.QTDE_ORIGINAL) * 100) > 20)
                                        listaCancelado.Add(item);
                                    else
                                        listaNovo.Add(item);
                                }
                        }

                        if (item.STATUS_PEDIDO.Contains("EM ESTUDO") & item.CREDITO.Contains("LIBERADO"))
                        {
                            if ((Convert.ToDecimal(item.QTDE_ORIGINAL) != Convert.ToDecimal(item.QTDE_CANCELADA)) &
                                (Convert.ToDecimal(item.VALOR_ORIGINAL) != Convert.ToDecimal(item.VALOR_CANCELADO)) &
                               ((Convert.ToDecimal(item.QTDE_ORIGINAL) - Convert.ToDecimal(item.QTDE_CANCELADA)) == Convert.ToDecimal(item.QTDE_ENTREGUE)) &
                               ((Convert.ToDecimal(item.VALOR_ORIGINAL) - Convert.ToDecimal(item.VALOR_CANCELADO)) == Convert.ToDecimal(item.VALOR_ENTREGUE)))
                                listaFatTotal.Add(item);
                            else
                                if (Convert.ToDecimal(item.QTDE_ORIGINAL) > 0)
                                {
                                    if ((Convert.ToDecimal(item.QTDE_CANCELADA) / Convert.ToDecimal(item.QTDE_ORIGINAL) * 100) > 20)
                                        listaCancelado.Add(item);
                                    else
                                        listaConferir.Add(item);
                                }
                        }

                        if (item.STATUS_PEDIDO.Contains("REPROVADO") & item.CREDITO.Contains("LIBERADO"))
                        {
                            if ((Convert.ToDecimal(item.QTDE_ORIGINAL) != Convert.ToDecimal(item.QTDE_CANCELADA)) &
                                (Convert.ToDecimal(item.VALOR_ORIGINAL) != Convert.ToDecimal(item.VALOR_CANCELADO)) &
                               ((Convert.ToDecimal(item.QTDE_ORIGINAL) - Convert.ToDecimal(item.QTDE_CANCELADA)) == Convert.ToDecimal(item.QTDE_ENTREGUE)) &
                               ((Convert.ToDecimal(item.VALOR_ORIGINAL) - Convert.ToDecimal(item.VALOR_CANCELADO)) == Convert.ToDecimal(item.VALOR_ENTREGUE)))
                                listaFatTotal.Add(item);
                            else
                                if (Convert.ToDecimal(item.QTDE_ORIGINAL) > 0)
                                {
                                    if ((Convert.ToDecimal(item.QTDE_CANCELADA) / Convert.ToDecimal(item.QTDE_ORIGINAL) * 100) > 20)
                                        listaCancelado.Add(item);
                                    else
                                        listaConferir.Add(item);
                                }
                        }

                        if (item.STATUS_PEDIDO.Contains("REPROVADO") & item.CREDITO.Contains("SEM CREDITO"))
                        {
                            if ((Convert.ToDecimal(item.QTDE_ORIGINAL) != Convert.ToDecimal(item.QTDE_CANCELADA)) &
                                (Convert.ToDecimal(item.VALOR_ORIGINAL) != Convert.ToDecimal(item.VALOR_CANCELADO)) &
                               ((Convert.ToDecimal(item.QTDE_ORIGINAL) - Convert.ToDecimal(item.QTDE_CANCELADA)) == Convert.ToDecimal(item.QTDE_ENTREGUE)) &
                               ((Convert.ToDecimal(item.VALOR_ORIGINAL) - Convert.ToDecimal(item.VALOR_CANCELADO)) == Convert.ToDecimal(item.VALOR_ENTREGUE)))
                                listaFatTotal.Add(item);
                            else
                                if (Convert.ToDecimal(item.QTDE_ORIGINAL) > 0)
                                {
                                    if ((Convert.ToDecimal(item.QTDE_CANCELADA) / Convert.ToDecimal(item.QTDE_ORIGINAL) * 100) > 20)
                                        listaCancelado.Add(item);
                                    else
                                        listaSemCredito.Add(item);
                                }
                        }

                        if (item.STATUS_PEDIDO.Contains("APROVADO") & item.CREDITO.Contains("SEM CREDITO"))
                        {
                            if ((Convert.ToDecimal(item.QTDE_ORIGINAL) != Convert.ToDecimal(item.QTDE_CANCELADA)) &
                                (Convert.ToDecimal(item.VALOR_ORIGINAL) != Convert.ToDecimal(item.VALOR_CANCELADO)) &
                               ((Convert.ToDecimal(item.QTDE_ORIGINAL) - Convert.ToDecimal(item.QTDE_CANCELADA)) == Convert.ToDecimal(item.QTDE_ENTREGUE)) &
                               ((Convert.ToDecimal(item.VALOR_ORIGINAL) - Convert.ToDecimal(item.VALOR_CANCELADO)) == Convert.ToDecimal(item.VALOR_ENTREGUE)))
                                listaFatTotal.Add(item);
                            else
                                if (Convert.ToDecimal(item.QTDE_ORIGINAL) > 0)
                                {
                                    if ((Convert.ToDecimal(item.QTDE_CANCELADA) / Convert.ToDecimal(item.QTDE_ORIGINAL) * 100) > 20)
                                        listaCancelado.Add(item);
                                    else
                                        listaParcial.Add(item);
                                }
                        }

                        if (item.STATUS_PEDIDO.Contains("APROVADO") & item.CREDITO.Contains("LIBERADO"))
                        {
                            if ((Convert.ToDecimal(item.QTDE_ORIGINAL) != Convert.ToDecimal(item.QTDE_CANCELADA)) &
                                (Convert.ToDecimal(item.VALOR_ORIGINAL) != Convert.ToDecimal(item.VALOR_CANCELADO)) &
                               ((Convert.ToDecimal(item.QTDE_ORIGINAL) - Convert.ToDecimal(item.QTDE_CANCELADA)) == Convert.ToDecimal(item.QTDE_ENTREGUE)) &
                               ((Convert.ToDecimal(item.VALOR_ORIGINAL) - Convert.ToDecimal(item.VALOR_CANCELADO)) == Convert.ToDecimal(item.VALOR_ENTREGUE)))
                                listaFatTotal.Add(item);
                            else
                                if (Convert.ToDecimal(item.QTDE_ORIGINAL) > 0)
                                {
                                    if ((Convert.ToDecimal(item.QTDE_CANCELADA) / Convert.ToDecimal(item.QTDE_ORIGINAL) * 100) > 20)
                                        listaCancelado.Add(item);
                                    else
                                        listaFaturamento.Add(item);
                                }
                        }
                    }
                }

                if (status.Equals("1"))
                    GridViewAtacado.DataSource = listaNovo;

                if (status.Equals("2"))
                    GridViewAtacado.DataSource = listaConferir;

                if (status.Equals("3"))
                    GridViewAtacado.DataSource = listaParcial;

                if (status.Equals("4"))
                    GridViewAtacado.DataSource = listaFaturamento;

                if (status.Equals("5"))
                    GridViewAtacado.DataSource = listaCancelado;

                if (status.Equals("6"))
                    GridViewAtacado.DataSource = listaFatTotal;

                if (status.Equals("7"))
                    GridViewAtacado.DataSource = listaSemCredito;

                GridViewAtacado.DataBind();
            }
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            if (ddlColecao.SelectedValue.ToString().Equals("0")   || 
                ddlColecao.SelectedValue.ToString().Equals(""))
                 return;

            CarregaGridViewAtacado(ddlColecao.SelectedValue, ddlStatus.SelectedValue);

        }

        protected void GridViewAtacado_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Sp_Gestao_AtacadoResult sp_Gestao_AtacadoResult = e.Row.DataItem as Sp_Gestao_AtacadoResult;

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
                            if ((sp_Gestao_AtacadoResult.QTDE_ORIGINAL != sp_Gestao_AtacadoResult.QTDE_CANCELADA) &
                                (sp_Gestao_AtacadoResult.VALOR_ORIGINAL != sp_Gestao_AtacadoResult.VALOR_CANCELADO) &
                               ((sp_Gestao_AtacadoResult.QTDE_ORIGINAL - sp_Gestao_AtacadoResult.QTDE_CANCELADA) == sp_Gestao_AtacadoResult.QTDE_ENTREGUE) &
                               ((sp_Gestao_AtacadoResult.VALOR_ORIGINAL - sp_Gestao_AtacadoResult.VALOR_CANCELADO) == sp_Gestao_AtacadoResult.VALOR_ENTREGUE))
                                literalStatus.Text = "FAT. TOTAL";
                            else
                                if ((Convert.ToDecimal(sp_Gestao_AtacadoResult.QTDE_CANCELADA) / Convert.ToDecimal(sp_Gestao_AtacadoResult.QTDE_ORIGINAL) * 100) > 20)
                                    literalStatus.Text = "CANCELADO";
                                else
                                    literalStatus.Text = "NOVO";
                        }

                        if (sp_Gestao_AtacadoResult.STATUS_PEDIDO.Contains("EM ESTUDO") &
                            sp_Gestao_AtacadoResult.CREDITO.Contains("LIBERADO"))
                        {
                            if ((sp_Gestao_AtacadoResult.QTDE_ORIGINAL != sp_Gestao_AtacadoResult.QTDE_CANCELADA) &
                                (sp_Gestao_AtacadoResult.VALOR_ORIGINAL != sp_Gestao_AtacadoResult.VALOR_CANCELADO) &
                               ((sp_Gestao_AtacadoResult.QTDE_ORIGINAL - sp_Gestao_AtacadoResult.QTDE_CANCELADA) == sp_Gestao_AtacadoResult.QTDE_ENTREGUE) &
                               ((sp_Gestao_AtacadoResult.VALOR_ORIGINAL - sp_Gestao_AtacadoResult.VALOR_CANCELADO) == sp_Gestao_AtacadoResult.VALOR_ENTREGUE))
                                literalStatus.Text = "FAT. TOTAL";
                            else
                                if ((Convert.ToDecimal(sp_Gestao_AtacadoResult.QTDE_CANCELADA) / Convert.ToDecimal(sp_Gestao_AtacadoResult.QTDE_ORIGINAL) * 100) > 20)
                                    literalStatus.Text = "CANCELADO";
                                else
                                    literalStatus.Text = "CONFERIR";
                        }

                        if (sp_Gestao_AtacadoResult.STATUS_PEDIDO.Contains("REPROVADO") &
                            sp_Gestao_AtacadoResult.CREDITO.Contains("LIBERADO"))
                        {
                            if ((sp_Gestao_AtacadoResult.QTDE_ORIGINAL != sp_Gestao_AtacadoResult.QTDE_CANCELADA) &
                                (sp_Gestao_AtacadoResult.VALOR_ORIGINAL != sp_Gestao_AtacadoResult.VALOR_CANCELADO) &
                               ((sp_Gestao_AtacadoResult.QTDE_ORIGINAL - sp_Gestao_AtacadoResult.QTDE_CANCELADA) == sp_Gestao_AtacadoResult.QTDE_ENTREGUE) &
                               ((sp_Gestao_AtacadoResult.VALOR_ORIGINAL - sp_Gestao_AtacadoResult.VALOR_CANCELADO) == sp_Gestao_AtacadoResult.VALOR_ENTREGUE))
                                literalStatus.Text = "FAT. TOTAL";
                            else
                                if ((Convert.ToDecimal(sp_Gestao_AtacadoResult.QTDE_CANCELADA) / Convert.ToDecimal(sp_Gestao_AtacadoResult.QTDE_ORIGINAL) * 100) > 20)
                                    literalStatus.Text = "CANCELADO";
                                else
                                    literalStatus.Text = "CONFERIR";
                        }

                        if (sp_Gestao_AtacadoResult.STATUS_PEDIDO.Contains("REPROVADO") &
                            sp_Gestao_AtacadoResult.CREDITO.Contains("SEM CREDITO"))
                        {
                            if ((sp_Gestao_AtacadoResult.QTDE_ORIGINAL != sp_Gestao_AtacadoResult.QTDE_CANCELADA) &
                                (sp_Gestao_AtacadoResult.VALOR_ORIGINAL != sp_Gestao_AtacadoResult.VALOR_CANCELADO) &
                               ((sp_Gestao_AtacadoResult.QTDE_ORIGINAL - sp_Gestao_AtacadoResult.QTDE_CANCELADA) == sp_Gestao_AtacadoResult.QTDE_ENTREGUE) &
                               ((sp_Gestao_AtacadoResult.VALOR_ORIGINAL - sp_Gestao_AtacadoResult.VALOR_CANCELADO) == sp_Gestao_AtacadoResult.VALOR_ENTREGUE))
                                literalStatus.Text = "FAT. TOTAL";
                            else
                                if ((Convert.ToDecimal(sp_Gestao_AtacadoResult.QTDE_CANCELADA) / Convert.ToDecimal(sp_Gestao_AtacadoResult.QTDE_ORIGINAL) * 100) > 20)
                                    literalStatus.Text = "CANCELADO";
                                else
                                    literalStatus.Text = "SEM CREDITO";
                        }

                        if (sp_Gestao_AtacadoResult.STATUS_PEDIDO.Contains("APROVADO") &
                            sp_Gestao_AtacadoResult.CREDITO.Contains("SEM CREDITO"))
                        {
                            if ((sp_Gestao_AtacadoResult.QTDE_ORIGINAL != sp_Gestao_AtacadoResult.QTDE_CANCELADA) &
                                (sp_Gestao_AtacadoResult.VALOR_ORIGINAL != sp_Gestao_AtacadoResult.VALOR_CANCELADO) &
                               ((sp_Gestao_AtacadoResult.QTDE_ORIGINAL - sp_Gestao_AtacadoResult.QTDE_CANCELADA) == sp_Gestao_AtacadoResult.QTDE_ENTREGUE) &
                               ((sp_Gestao_AtacadoResult.VALOR_ORIGINAL - sp_Gestao_AtacadoResult.VALOR_CANCELADO) == sp_Gestao_AtacadoResult.VALOR_ENTREGUE))
                                literalStatus.Text = "FAT. TOTAL";
                            else
                                if ((Convert.ToDecimal(sp_Gestao_AtacadoResult.QTDE_CANCELADA) / Convert.ToDecimal(sp_Gestao_AtacadoResult.QTDE_ORIGINAL) * 100) > 20)
                                    literalStatus.Text = "CANCELADO";
                                else
                                    literalStatus.Text = "FAT. PARCIAL";
                        }

                        if (sp_Gestao_AtacadoResult.STATUS_PEDIDO.Contains("APROVADO") &
                            sp_Gestao_AtacadoResult.CREDITO.Contains("LIBERADO"))
                        {
                            if ((sp_Gestao_AtacadoResult.QTDE_ORIGINAL != sp_Gestao_AtacadoResult.QTDE_CANCELADA) &
                                (sp_Gestao_AtacadoResult.VALOR_ORIGINAL != sp_Gestao_AtacadoResult.VALOR_CANCELADO) &
                               ((sp_Gestao_AtacadoResult.QTDE_ORIGINAL - sp_Gestao_AtacadoResult.QTDE_CANCELADA) == sp_Gestao_AtacadoResult.QTDE_ENTREGUE) &
                               ((sp_Gestao_AtacadoResult.VALOR_ORIGINAL - sp_Gestao_AtacadoResult.VALOR_CANCELADO) == sp_Gestao_AtacadoResult.VALOR_ENTREGUE))
                                literalStatus.Text = "FAT. TOTAL";
                            else
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

                if (literalStatus != null)
                {
                    if (literalStatus.Text.Contains("NOVO"))
                        item.Cells[19].ForeColor = System.Drawing.Color.Red;

                    if (literalStatus.Text.Contains("CANCELADO"))
                        item.Cells[19].ForeColor = System.Drawing.Color.GreenYellow;

                    if (literalStatus.Text.Contains("CONFERIR"))
                        item.Cells[19].ForeColor = System.Drawing.Color.Green;

                    if (literalStatus.Text.Contains("FAT. PARCIAL"))
                        item.Cells[19].ForeColor = System.Drawing.Color.Blue;

                    if (literalStatus.Text.Contains("FATURAMENTO"))
                        item.Cells[19].ForeColor = System.Drawing.Color.Orange;

                    if (literalStatus.Text.Contains("FAT. TOTAL"))
                        item.Cells[19].ForeColor = System.Drawing.Color.BlueViolet;

                    if (literalStatus.Text.Contains("SEM CREDITO"))
                        item.Cells[19].ForeColor = System.Drawing.Color.Chocolate;
                }

                Literal literalQtdeAEntregar = item.FindControl("LiteralQtdeAEntregar") as Literal;
                Literal literalValorAEntregar = item.FindControl("LiteralValorAEntregar") as Literal;

                if (literalQtdeAEntregar != null & literalValorAEntregar != null)
                {
                    if (literalQtdeAEntregar.Text.Trim().Length > 0 &
                        literalValorAEntregar.Text.Trim().Equals("0,00"))
                    {
                        item.Cells[16].ForeColor = System.Drawing.Color.Red;
                        item.Cells[17].ForeColor = System.Drawing.Color.Red;
                    }
                }
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
