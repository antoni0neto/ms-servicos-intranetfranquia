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
    public partial class desenv_painel_resultado_produto : System.Web.UI.Page
    {
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();
        LojaController lojaController = new LojaController();
        DesenvolvimentoController desenvController = new DesenvolvimentoController();

        const string PAINEL_RESULTADO_PRODUTO = "PAINEL_RESULTADO_PRODUTO";

        decimal gPrecoTS = 0;
        decimal gPrecoTO = 0;
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
                CarregarCores();
                CarregarTecido();
                CarregarCorFornecedor();
                CarregarSigned();

            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        private List<SP_OBTER_PAINEL_RESULTADO_PRODUTOResult> ObterPainelResultadoProduto()
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

            var resProduto = lojaController.ObterPainelResultadoProduto(dataIni, dataFim, colecaoLinxCon, ddlColecaoOrigem.SelectedValue, txtProduto.Text.Trim(), griffeCon, ddlGrupo.SelectedValue, ddlTipo.SelectedValue);

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

            if (ddlFabricacao.SelectedValue != "")
            {
                if (ddlFabricacao.SelectedValue == "P")
                    resProduto = resProduto.Where(p => p.FABRICANTE == "C-MAX" || p.FABRICANTE == "HANDBOOK").ToList();
                else
                    resProduto = resProduto.Where(p => p.FABRICANTE != "C-MAX" && p.FABRICANTE != "HANDBOOK").ToList();
            }

            if (ddlTecido.SelectedValue != "")
                resProduto = resProduto.Where(p => p.TECIDO == ddlTecido.SelectedValue.Trim()).ToList();

            if (ddlCorFornecedor.SelectedValue != "")
                resProduto = resProduto.Where(p => p.COR_FORNECEDOR == ddlCorFornecedor.SelectedValue.Trim()).ToList();

            if (ddlCor.SelectedValue != "")
                resProduto = resProduto.Where(p => p.COR == ddlCor.SelectedValue.Trim()).ToList();

            if (ddlSigned.SelectedValue != "")
                resProduto = resProduto.Where(p => p.SIGNED_NOME == ddlSigned.SelectedValue.Trim()).ToList();

            if (ddlVirado.SelectedValue != "")
            {
                if (ddlVirado.SelectedValue == "S")
                    resProduto = resProduto.Where(p => p.COLECAO != p.COD_SUBCATEGORIA).ToList();
                else
                    resProduto = resProduto.Where(p => p.COLECAO == p.COD_SUBCATEGORIA).ToList();
            }

            return resProduto;
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                var resultadoProduto = ObterPainelResultadoProduto();

                var ordem = "";
                if (ddlOrdem1.SelectedValue != "")
                    ordem = ordem + ddlOrdem1.SelectedValue + ", ";

                if (ddlOrdem2.SelectedValue != "")
                    ordem = ordem + ddlOrdem2.SelectedValue + ", ";

                if (ddlOrdem3.SelectedValue != "")
                    ordem = ordem + ddlOrdem3.SelectedValue + ", ";

                if (ddlOrdem4.SelectedValue != "")
                    ordem = ordem + ddlOrdem4.SelectedValue + ", ";

                if (ordem != "")
                {
                    ordem = ordem.Trim() + ",";
                    ordem = ordem.Replace(",,", "");
                    resultadoProduto = resultadoProduto.OrderBy(ordem).ToList();
                }

                Utils.WebControls.GetBoundFieldIndexByName(gvResultadoProduto, " - >>");
                Utils.WebControls.GetBoundFieldIndexByName(gvResultadoProduto, " - <<");

                gvResultadoProduto.DataSource = resultadoProduto;
                gvResultadoProduto.DataBind();

                //gvResultadoProduto.DataSource = resultadoProduto;
                //gvResultadoProduto.DataBind();

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
                    SP_OBTER_PAINEL_RESULTADO_PRODUTOResult res = e.Row.DataItem as SP_OBTER_PAINEL_RESULTADO_PRODUTOResult;

                    ImageButton _imgProduto1 = e.Row.FindControl("imgProduto1") as ImageButton;
                    ImageButton _imgProduto2 = e.Row.FindControl("imgProduto2") as ImageButton;
                    _imgProduto1.CommandArgument = res.CODIGO_PRODUTO.ToString() + "|" + res.COD_CATEGORIA.Trim();
                    _imgProduto2.CommandArgument = _imgProduto1.CommandArgument;
                    if (File.Exists(Server.MapPath(res.FOTO1)))
                        _imgProduto1.ImageUrl = res.FOTO1;
                    else if (File.Exists(Server.MapPath(res.FOTO2)))
                        _imgProduto1.ImageUrl = res.FOTO2;
                    else if (File.Exists(Server.MapPath(res.FOTO3)))
                        _imgProduto1.ImageUrl = res.FOTO3;
                    else
                        _imgProduto1.ImageUrl = "/Fotos/sem_foto.png";
                    _imgProduto2.ImageUrl = _imgProduto1.ImageUrl;


                    Label labCustoTS = e.Row.FindControl("labCustoTS") as Label;
                    labCustoTS.Text = "R$ " + Convert.ToDecimal(res.CUSTO_TS).ToString("###,###,##0.00");

                    Label labPrecoTO = e.Row.FindControl("labPrecoTO") as Label;
                    labPrecoTO.Text = "R$ " + Convert.ToDecimal(res.PRECO_TO).ToString("###,###,##0.00");

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

                    Label labPrecoTL = e.Row.FindControl("labPrecoTL") as Label;
                    labPrecoTL.Text = "R$ " + Convert.ToDecimal(res.PRECO_TL).ToString("###,###,##0.00");

                    Label labPorcEstoque = e.Row.FindControl("labPorcEstoque") as Label;
                    labPorcEstoque.Text = Convert.ToDecimal(res.PORC_ESTOQUE).ToString("##0.00") + "%";

                    Label labValEstoque = e.Row.FindControl("labValEstoque") as Label;
                    labValEstoque.Text = "R$ " + Convert.ToDecimal(res.VALOR_ESTOQUE).ToString("###,###,##0.00");

                    Label labCustoTotal = e.Row.FindControl("labCustoTotal") as Label;
                    labCustoTotal.Text = "R$ " + Convert.ToDecimal(res.CUSTO_TOTAL).ToString("###,###,##0.00");

                    Label labVendaAtual = e.Row.FindControl("labVendaAtual") as Label;
                    labVendaAtual.Text = "R$ " + Convert.ToDecimal(res.VALOR_VENDA).ToString("###,###,##0.00");

                    Label labMKPFinal = e.Row.FindControl("labMKPFinal") as Label;
                    labMKPFinal.Text = Convert.ToDecimal(res.MKP_FINAL).ToString("##0.00");

                    Label labLucroFinal = e.Row.FindControl("labLucroFinal") as Label;
                    labLucroFinal.Text = "R$ " + Convert.ToDecimal(res.LUCRO_FINAL).ToString("###,###,##0.00");


                    gPrecoTS += Convert.ToDecimal(res.CUSTO_TS);
                    gPrecoTO += Convert.ToDecimal(res.PRECO_TO);
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

                if (gPrecoTS > 0)
                    gMkpIni = (gPrecoTO * 1.00M / gPrecoTS * 1.00M);
                _footer.Cells[9].Text = "Mkp Inicial: " + gMkpIni.ToString("##0.00");

                _footer.Cells[12].Text = "Qtde Total: " + gQtdeTotal.ToString();

                _footer.Cells[13].Text = "Qtde Venda TO: " + gQtdeTO.ToString();
                if (gQtdeTotal > 0)
                    gPorcTO = (gQtdeTO * 1.00M / gQtdeTotal * 1.00M) * 100.00M;
                _footer.Cells[14].Text = "% Venda TO: " + gPorcTO.ToString("##0.00") + "%";

                _footer.Cells[15].Text = "Qtde Venda Mdo: " + gQtdeMD.ToString();
                if (gQtdeTotal > 0)
                    gPorcMD = (gQtdeMD * 1.00M / gQtdeTotal * 1.00M) * 100.00M;
                _footer.Cells[16].Text = "% Venda Mdo: " + gPorcMD.ToString("##0.00") + "%";

                _footer.Cells[17].Text = "Qtde Venda VM: " + gQtdeVM.ToString();
                if (gQtdeTotal > 0)
                    gPorcVM = (gQtdeVM * 1.00M / gQtdeTotal * 1.00M) * 100.00M;
                _footer.Cells[18].Text = "% Venda Mdo: " + gPorcVM.ToString("##0.00") + "%";

                _footer.Cells[19].Text = "Lucro: R$ " + gLucro.ToString("###,###,##0.00");

                if (totLinha > 0)
                {
                    if ((((gQtdeTO + gQtdeMD + gQtdeVM) * (gPrecoTS * 1.00M / totLinha)) * 1.00M) > 0)
                        gMkpParc = (gValVenda * 1.00M) / (((gQtdeTO + gQtdeMD + gQtdeVM) * (gPrecoTS * 1.00M / totLinha)) * 1.00M);
                }
                _footer.Cells[20].Text = "MKP Parcial: " + gMkpParc.ToString("##0.00");

                _footer.Cells[22].Text = "Qtde Estoque: " + gQtdeEstoque.ToString();
                if (gQtdeTotal > 0)
                    gPorcEstoque = (gQtdeEstoque * 1.00M / gQtdeTotal * 1.00M) * 100.00M;
                _footer.Cells[23].Text = "% Estoque: " + gPorcEstoque.ToString("##0.00") + "%";

                _footer.Cells[24].Text = "Valor Estoque: " + "R$ " + gValEstoque.ToString("###,###,##0.00");

                if (totLinha > 0)
                {
                    if ((gQtdeTotal * gPrecoTS) > 0)
                        gMkpFim = ((gValVenda + gValEstoque) * 1.00M) / ((gQtdeTotal * (gPrecoTS * 1.00M / totLinha)) * 1.00M);
                }

                _footer.Cells[25].Text = "Custo Total: " + "R$" + gCustoTotal.ToString("###,###,##0.00");

                _footer.Cells[27].Text = "Qtde Vendido: " + ((gQtdeTO + gQtdeMD + gQtdeVM)).ToString();

                _footer.Cells[28].Text = "Valor Venda: " + "R$" + gValVenda.ToString("###,###,##0.00");

                _footer.Cells[29].Text = "Mkp Final: " + gMkpFim.ToString("##0.00");

                _footer.Cells[30].Text = "Lucro Final: " + "R$" + gLucroFinal.ToString("###,###,##0.00");

            }

        }
        protected void gvResultadoProduto_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<SP_OBTER_PAINEL_RESULTADO_PRODUTOResult> resultadoProduto;

            if (Session[PAINEL_RESULTADO_PRODUTO] != null)
                resultadoProduto = (IEnumerable<SP_OBTER_PAINEL_RESULTADO_PRODUTOResult>)Session[PAINEL_RESULTADO_PRODUTO];
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

            ddlOrdem1.SelectedValue = "";
            ddlOrdem2.SelectedValue = "";
            ddlOrdem3.SelectedValue = "";
            ddlOrdem4.SelectedValue = "";
        }

        protected void imgProduto_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton b = (ImageButton)sender;
            string msg = "";
            string _url = "";
            if (b != null)
            {
                try
                {
                    var vals = b.CommandArgument.Split('|');
                    var codigoProduto = vals[0];
                    var tipoProduto = vals[1].Trim();

                    if (tipoProduto == "01" && codigoProduto != "0")
                        _url = "fnAbrirTelaCadastroMaiorVert('../mod_desenvolvimento/desenv_tripa_view_foto.aspx?p=" + codigoProduto + "');";
                    else if (tipoProduto == "02" && codigoProduto != "0")
                        _url = "fnAbrirTelaCadastroMaiorVert('../mod_desenvolvimento/desenv_tripa_view_foto_acessorio.aspx?p=" + codigoProduto + "');";
                    else
                        return;

                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _url, true);
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                }
            }
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
        private void CarregarCores()
        {
            var cores = prodController.ObterCoresBasicas();

            if (cores != null)
            {
                cores.Insert(0, new CORES_BASICA { COR = "", DESC_COR = "" });
                ddlCor.DataSource = cores;
                ddlCor.DataBind();
            }
        }

        private void CarregarTecido()
        {
            var tecidoFiltro = new List<DESENV_PRODUTO>();

            var _tecido = desenvController.ObterDesenvolvimentoColecao("").Where(p => p.TECIDO_POCKET != null && p.STATUS == 'A');

            var _tecidoAux = _tecido.Select(s => s.TECIDO_POCKET.Trim()).Distinct().ToList();

            foreach (var item in _tecidoAux)
                if (item.Trim() != "")
                    tecidoFiltro.Add(new DESENV_PRODUTO { TECIDO_POCKET = item.Trim() });

            tecidoFiltro = tecidoFiltro.OrderBy(p => p.TECIDO_POCKET).ToList();
            tecidoFiltro.Insert(0, new DESENV_PRODUTO { TECIDO_POCKET = "" });
            ddlTecido.DataSource = tecidoFiltro;
            ddlTecido.DataBind();
        }
        private void CarregarCorFornecedor()
        {
            var corFornecedorFiltro = new List<DESENV_PRODUTO>();

            var _corFornecedor = desenvController.ObterDesenvolvimentoColecao("").Where(p => p.FORNECEDOR_COR != null && p.STATUS == 'A');

            var _corFornecedorAux = _corFornecedor.Select(s => s.FORNECEDOR_COR.Trim()).Distinct().ToList();

            foreach (var item in _corFornecedorAux)
                if (item.Trim() != "")
                    corFornecedorFiltro.Add(new DESENV_PRODUTO { FORNECEDOR_COR = item.Trim() });

            corFornecedorFiltro = corFornecedorFiltro.OrderBy(p => p.FORNECEDOR_COR).ToList();
            corFornecedorFiltro.Insert(0, new DESENV_PRODUTO { FORNECEDOR_COR = "" });
            ddlCorFornecedor.DataSource = corFornecedorFiltro;
            ddlCorFornecedor.DataBind();
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
            ddlSigned.DataSource = signedFiltro;
            ddlSigned.DataBind();
        }

        protected void txtProduto_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var codigoProduto = txtProduto.Text.Trim();

                var produto = baseController.BuscaProduto(codigoProduto);
                if (produto != null)
                {
                    var produtoCores = baseController.BuscaProdutoCores(produto.PRODUTO1);
                    if (produtoCores != null && produtoCores.Count() > 0)
                    {
                        var cores = new List<CORES_BASICA>();
                        foreach (var c in produtoCores)
                        {
                            cores.Add(new CORES_BASICA { COR = c.COR_PRODUTO, DESC_COR = c.DESC_COR_PRODUTO });
                        }

                        cores.Insert(0, new CORES_BASICA { COR = "", DESC_COR = "" });
                        ddlCor.DataSource = cores;
                        ddlCor.DataBind();
                    }
                    else
                    {
                        CarregarCores();
                    }
                }
                else
                {
                    CarregarCores();
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion


    }
}
