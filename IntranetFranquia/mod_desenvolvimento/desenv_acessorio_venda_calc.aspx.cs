using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Relatorios
{
    public partial class desenv_acessorio_venda_calc : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();

        int totEstoque = 0;
        decimal qtdeVenda = 0;

        decimal valorVenda = 0;
        decimal custoVenda = 0;
        decimal lucroVenda = 0;

        decimal valValorizacao = 0;
        decimal custoEstoque = 0;
        decimal lucroEstoque = 0;

        decimal lucroTotal = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataInicial.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataFinal.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);

            if (!Page.IsPostBack)
            {
                CarregarColecoes();
                CarregarFornecedores();
                CarregarGrupo();
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        private List<SP_OBTER_VENDA_CALCADOSResult> ObterVendaCalcados()
        {
            DateTime? dataIni = null;
            DateTime? dataFim = null;

            if (txtDataInicial.Text.Trim() != "")
                dataIni = Convert.ToDateTime(txtDataInicial.Text.Trim());

            if (txtDataFinal.Text.Trim() != "")
                dataFim = Convert.ToDateTime(txtDataFinal.Text.Trim());

            var vendaCalc = desenvController.ObterVendaCalcados(ddlColecoes.SelectedValue.Trim(), ddlGrupo.SelectedValue.Trim(), txtProduto.Text.Trim(), ddlFornecedor.SelectedValue.Trim(), dataIni, dataFim);
            return vendaCalc;
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                gvVendaCalcado.DataSource = ObterVendaCalcados();
                gvVendaCalcado.DataBind();
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }

        protected void gvVendaCalcado_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_VENDA_CALCADOSResult calc = e.Row.DataItem as SP_OBTER_VENDA_CALCADOSResult;

                    if (calc != null)
                    {
                        System.Web.UI.WebControls.Image imgFotoPeca = e.Row.FindControl("imgFotoPeca") as System.Web.UI.WebControls.Image;
                        if (imgFotoPeca != null)
                            imgFotoPeca.ImageUrl = calc.FOTO;

                        Label labValorVenda = e.Row.FindControl("labValorVenda") as Label;
                        labValorVenda.Text = "R$" + Convert.ToDecimal(calc.VALOR_VENDA_PERIODO).ToString("###,###,###,##0.00");

                        Label labCustoVenda = e.Row.FindControl("labCustoVenda") as Label;
                        labCustoVenda.Text = "R$" + Convert.ToDecimal(calc.CUSTO_VENDA).ToString("###,###,###,##0.00");

                        Label labLucroVenda = e.Row.FindControl("labLucroVenda") as Label;
                        labLucroVenda.Text = "R$" + Convert.ToDecimal(calc.LUCRO_VENDA).ToString("###,###,###,##0.00");

                        Label labValorizacao = e.Row.FindControl("labValorizacao") as Label;
                        labValorizacao.Text = "R$" + Convert.ToDecimal(calc.VALORIZACAO).ToString("###,###,###,##0.00");

                        Label labCustoEstoque = e.Row.FindControl("labCustoEstoque") as Label;
                        labCustoEstoque.Text = "R$" + Convert.ToDecimal(calc.CUSTO_ESTOQUE).ToString("###,###,###,##0.00");

                        Label labLucroEstoque = e.Row.FindControl("labLucroEstoque") as Label;
                        labLucroEstoque.Text = "R$" + Convert.ToDecimal(calc.LUCRO_ESTOQUE).ToString("###,###,###,##0.00");

                        Label labLucroTotal = e.Row.FindControl("labLucroTotal") as Label;
                        labLucroTotal.Text = "R$" + Convert.ToDecimal(calc.LUCRO_TOTAL).ToString("###,###,###,##0.00");

                        totEstoque += Convert.ToInt32(calc.ESTOQUE);
                        qtdeVenda += Convert.ToInt32(calc.QTDE_VENDIDA_PERIODO);

                        valorVenda += Convert.ToDecimal(calc.VALOR_VENDA_PERIODO);
                        custoVenda += Convert.ToDecimal(calc.CUSTO_VENDA);
                        lucroVenda += Convert.ToDecimal(calc.LUCRO_VENDA);

                        valValorizacao += Convert.ToDecimal(calc.VALORIZACAO);
                        custoEstoque += Convert.ToDecimal(calc.CUSTO_ESTOQUE);
                        lucroEstoque += Convert.ToDecimal(calc.LUCRO_ESTOQUE);

                        lucroTotal += Convert.ToDecimal(calc.LUCRO_TOTAL);

                        //Popular GRID VIEW FILHO

                        DateTime? dataIni = null;
                        DateTime? dataFim = null;
                        if (txtDataInicial.Text.Trim() != "")
                            dataIni = Convert.ToDateTime(txtDataInicial.Text.Trim());

                        if (txtDataFinal.Text.Trim() != "")
                            dataFim = Convert.ToDateTime(txtDataFinal.Text.Trim());

                        var vendaCalcadoMes = desenvController.ObterVendaCalcadosPorMes(calc.PRODUTO, calc.COR, dataIni, dataFim);
                        if (vendaCalcadoMes != null && vendaCalcadoMes.Count() > 0)
                        {
                            GridView gvHistMensal = e.Row.FindControl("gvHistMensal") as GridView;
                            CarregarVendaCalcadosPorMes(gvHistMensal, vendaCalcadoMes);
                        }
                        else
                        {
                            System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                            if (img != null)
                                img.Visible = false;
                        }
                    }
                }
            }
        }
        protected void gvVendaCalcado_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvVendaCalcado.FooterRow;
            if (footer != null)
            {
                footer.Cells[2].Text = "Total";
                footer.Cells[2].HorizontalAlign = HorizontalAlign.Left;

                footer.Cells[7].Text = totEstoque.ToString();
                footer.Cells[8].Text = qtdeVenda.ToString();

                footer.Cells[9].Text = "R$" + valorVenda.ToString("###,###,###,##0.00");
                footer.Cells[10].Text = "R$" + custoVenda.ToString("###,###,###,##0.00");
                footer.Cells[11].Text = "R$" + lucroVenda.ToString("###,###,###,##0.00");

                footer.Cells[12].Text = "R$" + valValorizacao.ToString("###,###,###,##0.00");
                footer.Cells[13].Text = "R$" + custoEstoque.ToString("###,###,###,##0.00");
                footer.Cells[14].Text = "R$" + lucroEstoque.ToString("###,###,###,##0.00");

                footer.Cells[15].Text = "R$" + lucroTotal.ToString("###,###,###,##0.00");
            }
        }
        protected void gvVendaCalcado_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<SP_OBTER_VENDA_CALCADOSResult> vendaCalc = ObterVendaCalcados();

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            vendaCalc = vendaCalc.OrderBy(e.SortExpression + sortDirection);

            gvVendaCalcado.DataSource = vendaCalc;
            gvVendaCalcado.DataBind();
        }

        private void CarregarVendaCalcadosPorMes(GridView gvHistMensal, List<SP_OBTER_VENDA_CALCADOS_PORMESResult> vendaCalcadoMes)
        {
            if (gvHistMensal.Columns.Count > 0)
                gvHistMensal.Columns.Clear();

            DataTable dt = new DataTable();

            Color backColor = Color.White;
            bool bColor = true;
            var b = new BoundField();
            foreach (var v in vendaCalcadoMes)
            {
                b = new BoundField();
                b.DataField = v.ANOMES;
                b.HeaderText = v.ANOMES;
                b.HeaderStyle.Width = Unit.Pixel(220);
                b.ItemStyle.Width = Unit.Pixel(220);
                b.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                gvHistMensal.Columns.Add(b);
                dt.Columns.Add(new DataColumn(v.ANOMES, typeof(string)));
                b.ItemStyle.BackColor = backColor;

                if (bColor)
                {
                    backColor = Color.Snow;
                    bColor = false;
                }
                else
                {
                    backColor = Color.White;
                    bColor = true;
                }
            }

            b = new BoundField();
            b.DataField = "Total";
            b.HeaderText = "Total";
            b.HeaderStyle.Width = Unit.Pixel(220);
            b.ItemStyle.Width = Unit.Pixel(220);
            b.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            gvHistMensal.Columns.Add(b);
            dt.Columns.Add(new DataColumn("Total", typeof(string)));
            b.ItemStyle.BackColor = backColor;

            int j = 0;
            int totVen = 0;
            DataRow dr = dt.NewRow();
            foreach (var vcm in vendaCalcadoMes)
            {
                dr[j] = (vcm != null && vcm.QTDE_VENDA > 0) ? Convert.ToInt32(vcm.QTDE_VENDA).ToString() : "0";
                j += 1;

                totVen += Convert.ToInt32(vcm.QTDE_VENDA);
            }
            //adidicona coluna total
            dr[j] = totVen.ToString();

            //Adiciona a linha no datatable
            dt.Rows.Add(dr);
            gvHistMensal.DataSource = dt;
            gvHistMensal.DataBind();
        }
        protected void gvHistMensal_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    PROD_HB _hb = e.Row.DataItem as PROD_HB;
                    if (_hb != null)
                    {
                    }
                }
            }
        }

        #region "DADOS INICIAIS"
        private void CarregarColecoes()
        {
            var _colecoes = baseController.BuscaColecoes();
            if (_colecoes != null)
            {
                _colecoes.Insert(0, new COLECOE { COLECAO = "", DESC_COLECAO = "" });
                ddlColecoes.DataSource = _colecoes;
                ddlColecoes.DataBind();
            }
        }
        private void CarregarFornecedores()
        {

            var _fornecedores = prodController.ObterFornecedor();
            _fornecedores.Insert(0, new PROD_FORNECEDOR { FORNECEDOR = "", STATUS = 'S' });
            _fornecedores = _fornecedores.Where(p => (p.STATUS == 'A' && p.TIPO == 'C') || p.STATUS == 'S').GroupBy(x => new { FORNECEDOR = x.FORNECEDOR.Trim() }).Select(f => new PROD_FORNECEDOR { FORNECEDOR = f.Key.FORNECEDOR.Trim() }).ToList();

            if (_fornecedores != null)
            {
                ddlFornecedor.DataSource = _fornecedores;
                ddlFornecedor.DataBind();
            }

        }
        private void CarregarGrupo()
        {
            var _grupo = prodController.ObterGrupoProduto("02");
            if (_grupo != null)
            {
                _grupo.Insert(0, new SP_OBTER_GRUPOResult { GRUPO_PRODUTO = "" });
                ddlGrupo.DataSource = _grupo;
                ddlGrupo.DataBind();
            }
        }

        #endregion

    }
}
