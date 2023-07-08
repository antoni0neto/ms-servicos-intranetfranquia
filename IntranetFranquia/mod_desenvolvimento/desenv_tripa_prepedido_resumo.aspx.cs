using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Relatorios
{
    public partial class desenv_tripa_prepedido_resumo : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();

        int skuPlanejadoAtacado = 0;
        int skuDefAtacado = 0;
        int qtdeAtacado = 0;
        decimal valorAtacado = 0;

        int skuPlanejadoVarejo = 0;
        int skuDefVarejo = 0;
        int qtdeVarejo = 0;
        decimal valorVarejo = 0;

        decimal cotaAtacado = 0;
        decimal totalAtacado = 0;
        decimal diffAtacado = 0;

        decimal cotaVarejo = 0;
        decimal totalVarejo = 0;
        decimal diffVarejo = 0;

        decimal cotaTotal = 0;
        decimal valorTotal = 0;
        decimal diffTotal = 0;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                string marca = "";
                string colecao = "";
                string griffe = "";
                string desenvOrigem = "";

                if (Request.QueryString["c"] == null || Request.QueryString["c"] == ""
                    || Request.QueryString["g"] == null || Request.QueryString["g"] == ""
                    || Request.QueryString["d"] == null || Request.QueryString["d"] == ""
                    || Request.QueryString["m"] == null || Request.QueryString["m"] == ""
                    || Session["USUARIO"] == null)
                    Response.Redirect("desenv_menu.aspx");

                colecao = Request.QueryString["c"].ToString();
                griffe = Request.QueryString["g"].ToString();
                desenvOrigem = Request.QueryString["d"].ToString();
                marca = Request.QueryString["m"].ToString();

                hidMarca.Value = marca;
                hidColecao.Value = colecao;
                hidGriffe.Value = griffe;
                hidDesenvOrigem.Value = desenvOrigem;

                CarregarResumo();
                CarregarResumoCota();
                CarregarGrupoPedido();

                if (desenvOrigem == "0")
                {
                    btIncluirGrupo.Visible = false;
                    ddlGrupoProduto.Visible = false;
                    labGrupoProduto.Visible = false;
                }

            }

        }

        #region "RESUMO"

        private void CarregarResumo()
        {
            gvPrePedido.DataSource = ObterPrePedidoResumo();
            gvPrePedido.DataBind();
        }
        private List<SP_OBTER_PREPEDIDO_RESUMOResult> ObterPrePedidoResumo()
        {

            int desenvOrigem = 0;

            desenvOrigem = Convert.ToInt32(hidDesenvOrigem.Value);
            var resumo = desenvController.ObterPrePedidoResumo(hidMarca.Value, hidColecao.Value, hidGriffe.Value, desenvOrigem);


            return resumo;
        }

        protected void gvPrePedido_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItemIndex != -1)
            {
                e.Row.Attributes.Add("onMouseover", "this.style.background='#dddddd';this.style.cursor='hand'");
                e.Row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(e.Row.BackColor) + "';this.style.cursor='hand'");
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_PREPEDIDO_RESUMOResult pre = e.Row.DataItem as SP_OBTER_PREPEDIDO_RESUMOResult;


                    Literal _litValorTotalAtacado = e.Row.FindControl("litValorTotalAtacado") as Literal;
                    _litValorTotalAtacado.Text = "R$ " + Convert.ToDecimal(pre.VALOR_TOTAL_ATACADO).ToString("###,###,###,##0.00");

                    Literal _litValorTotalVarejo = e.Row.FindControl("litValorTotalVarejo") as Literal;
                    _litValorTotalVarejo.Text = "R$ " + Convert.ToDecimal(pre.VALOR_TOTAL_VAREJO).ToString("###,###,###,##0.00");

                    skuPlanejadoAtacado += Convert.ToInt32(pre.SKU_PLAN_ATACADO);
                    skuDefAtacado += Convert.ToInt32(pre.SKU_ATACADO);
                    qtdeAtacado += Convert.ToInt32(pre.QTDE_ATACADO);
                    valorAtacado += Convert.ToDecimal(pre.VALOR_TOTAL_ATACADO);

                    skuPlanejadoVarejo += Convert.ToInt32(pre.SKU_PLAN_VAREJO);
                    skuDefVarejo += Convert.ToInt32(pre.SKU_VAREJO);
                    qtdeVarejo += Convert.ToInt32(pre.QTDE_VAREJO);
                    valorVarejo += Convert.ToDecimal(pre.VALOR_TOTAL_VAREJO);

                    TextBox txtSKUPlanejadoAtacado = e.Row.FindControl("txtSKUPlanejadoAtacado") as TextBox;
                    TextBox txtSKUPlanejadoVarejo = e.Row.FindControl("txtSKUPlanejadoVarejo") as TextBox;
                    if (hidDesenvOrigem.Value == "0")
                    {
                        txtSKUPlanejadoAtacado.Enabled = false;
                        txtSKUPlanejadoVarejo.Enabled = false;
                    }


                }
            }
        }
        protected void gvPrePedido_DataBound(object sender, EventArgs e)
        {
            var footer = gvPrePedido.FooterRow;
            if (footer != null)
            {
                footer.Cells[1].Text = "Total";

                footer.Cells[3].Text = skuPlanejadoAtacado.ToString();
                footer.Cells[4].Text = skuDefAtacado.ToString();
                footer.Cells[5].Text = (skuPlanejadoAtacado - skuDefAtacado).ToString();
                footer.Cells[6].Text = qtdeAtacado.ToString();
                footer.Cells[7].Text = valorAtacado.ToString("###,###,###,##0.00");

                footer.Cells[8].Text = skuPlanejadoVarejo.ToString();
                footer.Cells[9].Text = skuDefVarejo.ToString();
                footer.Cells[10].Text = (skuPlanejadoVarejo - skuDefVarejo).ToString();
                footer.Cells[11].Text = qtdeVarejo.ToString();
                footer.Cells[12].Text = valorVarejo.ToString("###,###,###,##0.00");

            }
        }
        protected void gvPrePedido_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<SP_OBTER_PREPEDIDO_RESUMOResult> prePedido = ObterPrePedidoResumo();

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            prePedido = prePedido.OrderBy(e.SortExpression + sortDirection);
            gvPrePedido.DataSource = prePedido.ToList();
            gvPrePedido.DataBind();

        }

        protected void txtSKUPlanejado_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox txt = (TextBox)sender;

                GridViewRow row = (GridViewRow)txt.NamingContainer;
                string grupoProduto = gvPrePedido.DataKeys[row.RowIndex].Value.ToString();

                int skuPlanejado = 0;
                if (txt.Text != "")
                    skuPlanejado = Convert.ToInt32(txt.Text);

                var preSku = desenvController.ObterPrePedidoSKU(hidMarca.Value, hidColecao.Value, grupoProduto, hidGriffe.Value, Convert.ToInt32(hidDesenvOrigem.Value));
                if (preSku != null)
                {
                    if (txt.ID.ToLower().Contains("varejo"))
                        preSku.SKU_PLAN_VAREJO = skuPlanejado;
                    else
                        preSku.SKU_PLAN_ATACADO = skuPlanejado;

                    desenvController.AtualizarPrePedidoSKU(preSku);
                }
                else
                {
                    var preSkuI = new DESENV_PREPEDIDO_SKU();
                    preSkuI.MARCA = hidMarca.Value;
                    preSkuI.COLECAO = hidColecao.Value;
                    preSkuI.GRIFFE = hidGriffe.Value;
                    preSkuI.DESENV_PRODUTO_ORIGEM = Convert.ToInt32(hidDesenvOrigem.Value);
                    preSkuI.GRUPO_PRODUTO = grupoProduto;
                    if (txt.ID.ToLower().Contains("varejo"))
                        preSkuI.SKU_PLAN_VAREJO = skuPlanejado;
                    else
                        preSkuI.SKU_PLAN_ATACADO = skuPlanejado;
                    desenvController.InserirPrePedidoSKU(preSkuI);
                }

                CarregarResumo();

            }
            catch (Exception)
            {
            }
        }

        #endregion

        #region "RESUMO COTA"

        private void CarregarResumoCota()
        {
            gvPrePedidoCota.DataSource = ObterPrePedidoResumoCota();
            gvPrePedidoCota.DataBind();
        }
        private List<SP_OBTER_PREPEDIDO_RESUMO_COTAResult> ObterPrePedidoResumoCota()
        {

            int desenvOrigem = 0;

            desenvOrigem = Convert.ToInt32(hidDesenvOrigem.Value);
            var resumoCota = desenvController.ObterPrePedidoResumoCota(hidMarca.Value, hidColecao.Value, hidGriffe.Value, 0);

            return resumoCota;
        }

        protected void gvPrePedidoCota_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItemIndex != -1)
            {
                e.Row.Attributes.Add("onMouseover", "this.style.background='#dddddd';this.style.cursor='hand'");
                e.Row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(e.Row.BackColor) + "';this.style.cursor='hand'");
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_PREPEDIDO_RESUMO_COTAResult pre = e.Row.DataItem as SP_OBTER_PREPEDIDO_RESUMO_COTAResult;


                    Literal _litCotaAtacado = e.Row.FindControl("litCotaAtacado") as Literal;
                    _litCotaAtacado.Text = "R$ " + Convert.ToDecimal(pre.COTA_ATACADO).ToString("###,###,###,##0.00");

                    Literal _litValorAtacado = e.Row.FindControl("litValorAtacado") as Literal;
                    _litValorAtacado.Text = "R$ " + Convert.ToDecimal(pre.VALOR_TOTAL_ATACADO).ToString("###,###,###,##0.00");

                    Literal _litDiffAtacado = e.Row.FindControl("litDiffAtacado") as Literal;
                    _litDiffAtacado.Text = "R$ " + Convert.ToDecimal(pre.DIFF_ATACADO).ToString("###,###,###,##0.00");

                    Literal _litCotaVarejo = e.Row.FindControl("litCotaVarejo") as Literal;
                    _litCotaVarejo.Text = "R$ " + Convert.ToDecimal(pre.COTA_VAREJO).ToString("###,###,###,##0.00");

                    Literal _litValorVarejo = e.Row.FindControl("litValorVarejo") as Literal;
                    _litValorVarejo.Text = "R$ " + Convert.ToDecimal(pre.VALOR_TOTAL_VAREJO).ToString("###,###,###,##0.00");

                    Literal _litDiffVarejo = e.Row.FindControl("litDiffVarejo") as Literal;
                    _litDiffVarejo.Text = "R$ " + Convert.ToDecimal(pre.DIFF_VAREJO).ToString("###,###,###,##0.00");

                    Literal _litTotalCota = e.Row.FindControl("litTotalCota") as Literal;
                    _litTotalCota.Text = "R$ " + Convert.ToDecimal(pre.COTA_ATACADO + pre.COTA_VAREJO).ToString("###,###,###,##0.00");

                    Literal _litTotalValor = e.Row.FindControl("litTotalValor") as Literal;
                    _litTotalValor.Text = "R$ " + Convert.ToDecimal(pre.VALOR_TOTAL_ATACADO + pre.VALOR_TOTAL_VAREJO).ToString("###,###,###,##0.00");

                    Literal _litTotalDiff = e.Row.FindControl("litTotalDiff") as Literal;
                    _litTotalDiff.Text = "R$ " + Convert.ToDecimal(pre.DIFF_ATACADO + pre.DIFF_VAREJO).ToString("###,###,###,##0.00");


                    cotaAtacado += Convert.ToDecimal(pre.COTA_ATACADO);
                    totalAtacado += Convert.ToDecimal(pre.VALOR_TOTAL_ATACADO);
                    diffAtacado += Convert.ToDecimal(pre.DIFF_ATACADO);

                    cotaVarejo += Convert.ToDecimal(pre.COTA_VAREJO);
                    totalVarejo += Convert.ToDecimal(pre.VALOR_TOTAL_VAREJO);
                    diffVarejo += Convert.ToDecimal(pre.DIFF_VAREJO);

                    cotaTotal += Convert.ToDecimal(pre.COTA_ATACADO + pre.COTA_VAREJO);
                    valorTotal += Convert.ToDecimal(pre.VALOR_TOTAL_ATACADO + pre.VALOR_TOTAL_VAREJO);
                    diffTotal += Convert.ToDecimal(pre.DIFF_ATACADO + pre.DIFF_VAREJO);


                }
            }
        }
        protected void gvPrePedidoCota_DataBound(object sender, EventArgs e)
        {
            var footer = gvPrePedidoCota.FooterRow;
            if (footer != null)
            {
                footer.Cells[1].Text = "Total";

                footer.Cells[3].Text = "R$ " + cotaAtacado.ToString("###,###,###,##0.00");
                footer.Cells[4].Text = "R$ " + totalAtacado.ToString("###,###,###,##0.00");
                footer.Cells[5].Text = "R$ " + diffAtacado.ToString("###,###,###,##0.00");

                footer.Cells[6].Text = "R$ " + cotaVarejo.ToString("###,###,###,##0.00");
                footer.Cells[7].Text = "R$ " + totalVarejo.ToString("###,###,###,##0.00");
                footer.Cells[8].Text = "R$ " + diffVarejo.ToString("###,###,###,##0.00");

                footer.Cells[9].Text = "R$ " + cotaTotal.ToString("###,###,###,##0.00");
                footer.Cells[10].Text = "R$ " + valorTotal.ToString("###,###,###,##0.00");
                footer.Cells[11].Text = "R$ " + diffTotal.ToString("###,###,###,##0.00");

            }
        }

        #endregion

        protected void btIncluirGrupo_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (ddlGrupoProduto.SelectedValue == "Selecione")
                {
                    labErro.Text = "Selecione o Grupo de Produto...";
                    return;
                }

                var preSku = desenvController.ObterPrePedidoSKU(hidMarca.Value, hidColecao.Value, ddlGrupoProduto.SelectedValue, hidGriffe.Value, Convert.ToInt32(hidDesenvOrigem.Value));
                if (preSku != null)
                {
                    labErro.Text = "Este Grupo de Produto já foi inserido...";
                    return;
                }


                var preSkuI = new DESENV_PREPEDIDO_SKU();
                preSkuI.COLECAO = hidColecao.Value;
                preSkuI.GRIFFE = hidGriffe.Value;
                preSkuI.DESENV_PRODUTO_ORIGEM = Convert.ToInt32(hidDesenvOrigem.Value);
                preSkuI.GRUPO_PRODUTO = ddlGrupoProduto.SelectedValue.Trim();
                preSkuI.SKU_PLAN_VAREJO = 0;
                preSkuI.SKU_PLAN_ATACADO = 0;
                preSkuI.MARCA = hidMarca.Value;
                desenvController.InserirPrePedidoSKU(preSkuI);

                CarregarResumo();

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        private void CarregarGrupoPedido()
        {
            var grupo = prodController.ObterGrupoProduto("01");

            grupo.Insert(0, new SP_OBTER_GRUPOResult { GRUPO_PRODUTO = "Selecione" });
            ddlGrupoProduto.DataSource = grupo;
            ddlGrupoProduto.DataBind();
        }
    }
}
