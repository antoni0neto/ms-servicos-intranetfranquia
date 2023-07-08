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
using System.Linq.Expressions;
using System.Linq.Dynamic;
using System.Globalization;


namespace Relatorios
{
    public partial class desenv_painel_resultado_produtoconsol : System.Web.UI.Page
    {
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();
        LojaController lojaController = new LojaController();
        DesenvolvimentoController desenvController = new DesenvolvimentoController();

        const string PAINEL_RESULTADO_PRODUTO = "PAINEL_RESULTADO_PRODUTO_CONSOL";

        //decimal gPrecoTS = 0;
        //decimal gPrecoTO = 0;
        decimal gMkpIni = 0;

        int gQtdeTotal = 0;
        int gQtdeTO = 0;
        decimal gPorcTO = 0;
        int gQtdeMD = 0;
        decimal gPorcMD = 0;
        int gQtdeVM = 0;
        decimal gPorcVM = 0;

        decimal gValVenda = 0;
        decimal gLucro = 0;
        decimal gMkpParc = 0;
        int gQtdeEstoque = 0;
        decimal gPorcEstoque = 0;
        decimal gValEstoque = 0;
        decimal gMkpFim = 0;
        decimal gLucroFinal = 0;

        decimal gCustoTotal = 0;
        decimal totLinha = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataInicial.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtDataFinal.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);


            if (!Page.IsPostBack)
            {
                string tela = Request.QueryString["t"].ToString();
                if (tela != "1" && tela != "2" && tela != "3")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                if (tela == "1")
                    hrefVoltar.HRef = "desenv_menu.aspx";
                else if (tela == "2")
                    hrefVoltar.HRef = "../mod_acomp_mensal/acomp_menu.aspx";

                CarregarColecoes();
                CarregarGrupo();
                CarregarGriffe();
                CarregarSigned();

            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        private List<SP_OBTER_PAINEL_RESULTADO_PRODUTO_CONSOLResult> ObterPainelResultadoProduto()
        {

            var colecaoLinxCon = "";
            if (lstColecao.GetSelectedIndices().Count() > 0)
            {
                foreach (var v in lstColecao.GetSelectedIndices())
                {
                    var item = lstColecao.Items[v].Value.Trim() + ",";
                    colecaoLinxCon = colecaoLinxCon + item;
                }

                colecaoLinxCon = colecaoLinxCon + ",";
                colecaoLinxCon = colecaoLinxCon.Replace(",,", "");
            }

            var griffeCon = "";
            if (lstGriffe.GetSelectedIndices().Count() > 0)
            {
                foreach (var v in lstGriffe.GetSelectedIndices())
                {
                    var item = lstGriffe.Items[v].Value.Trim() + ",";
                    griffeCon = griffeCon + item;
                }

                griffeCon = griffeCon + ",";
                griffeCon = griffeCon.Replace(",,", "");
            }

            DateTime dataIni = new DateTime(1900, 1, 1);
            DateTime dataFim = new DateTime(1900, 1, 1);
            if (txtDataInicial.Text.Trim() != "")
                DateTime.TryParse(txtDataInicial.Text.Trim(), out dataIni);
            if (txtDataFinal.Text.Trim() != "")
                DateTime.TryParse(txtDataFinal.Text.Trim(), out dataFim);

            var resProduto = lojaController.ObterPainelResultadoProdutoConsolidado(dataIni, dataFim, colecaoLinxCon, ddlColecaoOrigem.SelectedValue, griffeCon, ddlGrupo.SelectedValue, ddlTipo.SelectedValue);

            if (ddlComVenda.SelectedValue != "")
            {
                if (ddlComVenda.SelectedValue == "S")
                    resProduto = resProduto.Where(p => p.QTDE_VALEMERCADORIA > 0 || p.QTDE_VENDA_MARKDOWN > 0 || p.QTDE_VENDA_TO > 0).ToList();
                else
                    resProduto = resProduto.Where(p => p.QTDE_VALEMERCADORIA <= 0 && p.QTDE_VENDA_MARKDOWN <= 0 && p.QTDE_VENDA_TO <= 0).ToList();
            }

            if (ddlComEstoque.SelectedValue != "")
            {
                if (ddlComEstoque.SelectedValue == "S")
                    resProduto = resProduto.Where(p => p.QTDE_ESTOQUE > 0).ToList();
                else
                    resProduto = resProduto.Where(p => p.QTDE_ESTOQUE <= 0).ToList();
            }

            if (ddlSigned.SelectedValue != "")
            {
                if (ddlSigned.SelectedValue != "VAZIO")
                    resProduto = resProduto.Where(p => p.SIGNED_NOME == ddlSigned.SelectedValue.Trim()).ToList();
                else
                    resProduto = resProduto.Where(p => p.SIGNED_NOME == null || p.SIGNED_NOME.Trim() == "").ToList();
            }

            return resProduto;
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                var resultadoProduto = ObterPainelResultadoProduto();

                Utils.WebControls.GetBoundFieldIndexByName(gvResultadoProduto, " - >>");
                Utils.WebControls.GetBoundFieldIndexByName(gvResultadoProduto, " - <<");

                gvResultadoProduto.DataSource = resultadoProduto;
                gvResultadoProduto.DataBind();

                Session[PAINEL_RESULTADO_PRODUTO] = resultadoProduto;
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }

        protected void gvResultadoProduto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_PAINEL_RESULTADO_PRODUTO_CONSOLResult res = e.Row.DataItem as SP_OBTER_PAINEL_RESULTADO_PRODUTO_CONSOLResult;

                    Label labMKPIni = e.Row.FindControl("labMKPIni") as Label;
                    labMKPIni.Text = Convert.ToDecimal(res.MKP_INICIAL).ToString("##0.00");

                    Label labPorcTO = e.Row.FindControl("labPorcTO") as Label;
                    labPorcTO.Text = Convert.ToDecimal(res.PORC_VENDA_TO).ToString("##0.00") + "%";

                    Label labPorcMarkdown = e.Row.FindControl("labPorcMarkdown") as Label;
                    labPorcMarkdown.Text = Convert.ToDecimal(res.PORC_VENDA_MARKDOWN).ToString("##0.00") + "%";

                    Label labPorcVM = e.Row.FindControl("labPorcVM") as Label;
                    labPorcVM.Text = Convert.ToDecimal(res.PORC_VALEMERCADORIA).ToString("##0.00") + "%";

                    Label labLucro = e.Row.FindControl("labLucro") as Label;
                    labLucro.Text = "R$ " + Convert.ToDecimal(res.LUCRO).ToString("###,###,##0.00");

                    Label labMKPParc = e.Row.FindControl("labMKPParc") as Label;
                    labMKPParc.Text = Convert.ToDecimal(res.MKP_PARCIAL).ToString("##0.00");

                    Label labPorcEstoque = e.Row.FindControl("labPorcEstoque") as Label;
                    labPorcEstoque.Text = Convert.ToDecimal(res.PORC_ESTOQUE).ToString("##0.00") + "%";

                    Label labValEstoque = e.Row.FindControl("labValEstoque") as Label;
                    labValEstoque.Text = "R$ " + Convert.ToDecimal(res.VALOR_ESTOQUE).ToString("###,###,##0.00");

                    Label labCustoTotal = e.Row.FindControl("labCustoTotal") as Label;
                    labCustoTotal.Text = "R$ " + Convert.ToDecimal(res.CUSTO_TOTAL).ToString("###,###,##0.00");

                    Label labQtdeProdutoVendido = e.Row.FindControl("labQtdeProdutoVendido") as Label;
                    labQtdeProdutoVendido.Text = (res.QTDE_VENDA_TO + res.QTDE_VENDA_MARKDOWN + res.QTDE_VALEMERCADORIA).ToString();

                    Label labVendaAtual = e.Row.FindControl("labVendaAtual") as Label;
                    labVendaAtual.Text = "R$ " + Convert.ToDecimal(res.VALOR_VENDA).ToString("###,###,##0.00");

                    Label labMKPFinal = e.Row.FindControl("labMKPFinal") as Label;
                    labMKPFinal.Text = Convert.ToDecimal(res.MKP_FINAL).ToString("##0.00");

                    Label labLucroFinal = e.Row.FindControl("labLucroFinal") as Label;
                    labLucroFinal.Text = "R$ " + Convert.ToDecimal(res.LUCRO_FINAL).ToString("###,###,##0.00");


                    gQtdeTotal += Convert.ToInt32(res.QTDE_TOTAL);
                    gQtdeTO += Convert.ToInt32(res.QTDE_VENDA_TO);
                    gQtdeMD += Convert.ToInt32(res.QTDE_VENDA_MARKDOWN);
                    gQtdeVM += Convert.ToInt32(res.QTDE_VALEMERCADORIA);

                    gValVenda += Convert.ToDecimal(res.VALOR_VENDA);
                    gLucro += Convert.ToDecimal(res.LUCRO);

                    gQtdeEstoque += Convert.ToInt32(res.QTDE_ESTOQUE);
                    gValEstoque += Convert.ToDecimal(res.VALOR_ESTOQUE);

                    gLucroFinal += Convert.ToDecimal(res.LUCRO_FINAL);

                    gCustoTotal += Convert.ToDecimal(res.CUSTO_TOTAL);

                    totLinha += 1;

                }
            }

            if (e.Row.DataItemIndex != -1)
            {
                e.Row.Attributes.Add("onMouseover", "this.style.background='#dddddd';this.style.cursor='hand'");
                e.Row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(e.Row.BackColor) + "';this.style.cursor='hand'");
            }
        }
        protected void gvResultadoProduto_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = gvResultadoProduto.FooterRow;
            if (_footer != null)
            {
                _footer.Cells[1].Text = "Total";
                _footer.Cells[1].HorizontalAlign = HorizontalAlign.Left;

                //if (gPrecoTS > 0)
                //    gMkpIni = (gPrecoTO * 1.00M / gPrecoTS * 1.00M);
                //_footer.Cells[4].Text = "Mkp Inicial: " + gMkpIni.ToString("##0.00");

                _footer.Cells[5].Text = "Qtde Total: " + gQtdeTotal.ToString();

                _footer.Cells[6].Text = "Qtde Venda TO: " + gQtdeTO.ToString();
                if (gQtdeTotal > 0)
                    gPorcTO = (gQtdeTO * 1.00M / gQtdeTotal * 1.00M) * 100.00M;
                _footer.Cells[7].Text = "% Venda TO: " + gPorcTO.ToString("##0.00") + "%";

                _footer.Cells[8].Text = "Qtde Venda Mdo: " + gQtdeMD.ToString();
                if (gQtdeTotal > 0)
                    gPorcMD = (gQtdeMD * 1.00M / gQtdeTotal * 1.00M) * 100.00M;
                _footer.Cells[9].Text = "% Venda Mdo: " + gPorcMD.ToString("##0.00") + "%";

                _footer.Cells[10].Text = "Qtde Venda VM: " + gQtdeVM.ToString();
                if (gQtdeTotal > 0)
                    gPorcVM = (gQtdeVM * 1.00M / gQtdeTotal * 1.00M) * 100.00M;
                _footer.Cells[11].Text = "% Venda Mdo: " + gPorcVM.ToString("##0.00") + "%";

                _footer.Cells[12].Text = "Lucro: R$ " + gLucro.ToString("###,###,##0.00");

                //if (totLinha > 0)
                //{
                //    if ((((gQtdeTO + gQtdeMD + gQtdeVM) * (gPrecoTS * 1.00M / totLinha)) * 1.00M) > 0)
                //        gMkpParc = (gValVenda * 1.00M) / (((gQtdeTO + gQtdeMD + gQtdeVM) * (gPrecoTS * 1.00M / totLinha)) * 1.00M);
                //}
                //_footer.Cells[13].Text = "MKP Parcial: " + gMkpParc.ToString("##0.00");

                _footer.Cells[14].Text = "Qtde Estoque: " + gQtdeEstoque.ToString();
                if (gQtdeTotal > 0)
                    gPorcEstoque = (gQtdeEstoque * 1.00M / gQtdeTotal * 1.00M) * 100.00M;
                _footer.Cells[15].Text = "% Estoque: " + gPorcEstoque.ToString("##0.00") + "%";

                _footer.Cells[16].Text = "Valor Estoque: " + "R$ " + gValEstoque.ToString("###,###,##0.00");

                _footer.Cells[17].Text = "Custo Total: " + "R$" + gCustoTotal.ToString("###,###,##0.00");

                _footer.Cells[18].Text = "Qtde Vendido: " + ((gQtdeTO + gQtdeMD + gQtdeVM)).ToString();

                _footer.Cells[19].Text = "Valor Venda: " + "R$" + gValVenda.ToString("###,###,##0.00");

                //if (totLinha > 0)
                //{
                //    if ((gQtdeTotal * gPrecoTS) > 0)
                //        gMkpFim = ((gValVenda + gValEstoque) * 1.00M) / ((gQtdeTotal * (gPrecoTS * 1.00M / totLinha)) * 1.00M);
                //}
                //_footer.Cells[20].Text = "Mkp Final: " + gMkpFim.ToString("##0.00");

                _footer.Cells[21].Text = "Lucro Final: " + "R$" + gLucroFinal.ToString("###,###,##0.00");

            }

        }
        protected void gvResultadoProduto_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<SP_OBTER_PAINEL_RESULTADO_PRODUTO_CONSOLResult> resultadoProduto;

            if (Session[PAINEL_RESULTADO_PRODUTO] != null)
                resultadoProduto = (IEnumerable<SP_OBTER_PAINEL_RESULTADO_PRODUTO_CONSOLResult>)Session[PAINEL_RESULTADO_PRODUTO];
            else
                resultadoProduto = ObterPainelResultadoProduto();

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            resultadoProduto = resultadoProduto.OrderBy(e.SortExpression + sortDirection);
            gvResultadoProduto.DataSource = resultadoProduto;
            gvResultadoProduto.DataBind();

        }

        #region "DADOS INICIAIS"
        private void CarregarColecoes()
        {
            var colecoes = baseController.BuscaColecoes();
            if (colecoes != null)
            {
                colecoes.Insert(0, new COLECOE { COLECAO = "", DESC_COLECAO = "" });
                lstColecao.DataSource = colecoes;
                lstColecao.DataBind();

                ddlColecaoOrigem.DataSource = colecoes;
                ddlColecaoOrigem.DataBind();
            }
        }
        private void CarregarGrupo()
        {
            var _grupo = prodController.ObterGrupoProduto("");

            if (_grupo != null)
            {
                _grupo.Insert(0, new SP_OBTER_GRUPOResult { GRUPO_PRODUTO = "" });
                ddlGrupo.DataSource = _grupo;
                ddlGrupo.DataBind();
            }
        }
        private void CarregarGriffe()
        {
            var griffe = baseController.BuscaGriffes();

            if (griffe != null)
            {
                griffe.Insert(0, new PRODUTOS_GRIFFE { GRIFFE = "" });
                lstGriffe.DataSource = griffe;
                lstGriffe.DataBind();
            }
        }
        private void CarregarSigned()
        {
            List<DESENV_PRODUTO> signedFiltro = new List<DESENV_PRODUTO>();

            var _signed = desenvController.ObterDesenvolvimentoColecao("").Where(p => p.SIGNED_NOME != null && p.SIGNED_NOME.Trim() != "").Select(s => s.SIGNED_NOME).Distinct().ToList();
            foreach (var item in _signed)
                if (item != "")
                    signedFiltro.Add(new DESENV_PRODUTO { SIGNED_NOME = item });

            signedFiltro = signedFiltro.OrderBy(p => p.SIGNED_NOME).ToList();
            signedFiltro.Insert(0, new DESENV_PRODUTO { SIGNED_NOME = "" });
            signedFiltro.Add(new DESENV_PRODUTO { SIGNED_NOME = "VAZIO" });
            ddlSigned.DataSource = signedFiltro;
            ddlSigned.DataBind();
        }


        #endregion


    }
}
