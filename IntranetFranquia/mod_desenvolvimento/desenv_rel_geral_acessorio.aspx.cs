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

namespace Relatorios
{
    public partial class desenv_rel_geral_acessorio : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();
        List<SP_OBTER_DESENV_REL_ACESSResult> _geral = new List<SP_OBTER_DESENV_REL_ACESSResult>();

        //gvGeral
        int g_qtdePlanejada = 0;
        int g_qtdeAEntregar = 0;
        int g_qtdeEntregue = 0;
        int g_qtdeTotal = 0;
        decimal g_preco = 0;
        decimal g_precoTotal = 0;

        //gvResumo
        int g_qtdeSKU = 0;
        int g_qtdeAcessorio = 0;
        decimal g_valorResumo = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregarColecoes();
                ddlStatusResumo.Visible = false;
                Session["REL_GERAL_ACESS"] = null;
                ViewState["sortDirection"] = true;
            }

            //Evitar duplo clique no botão
            btFiltroStatus.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btFiltroStatus, null) + ";");
        }

        protected void btFiltroStatus_Click(object sender, EventArgs e)
        {
            int _count = 0;
            try
            {
                labStatus.Text = "";
                labValorFinal.Text = "R$ 0,00";
                if (ddlColecoesBuscar.SelectedValue.Trim() == "0")
                {
                    labStatus.Text = "Selecione a Coleção.";
                    return;
                }

                CarregarBudget();
                _count = RecarregarRelatorioGeral();
                if (_count > 0)
                {
                    RecarregarRelatorioResumo("");
                    ddlStatusResumo.Visible = true;
                    CarregarStatus(ddlColecoesBuscar.SelectedValue.Trim());
                    CarregarOrigem();
                    CarregarGrupo();
                    CarregarPedido();
                    pnlRelatorio.Visible = true;
                }
                else
                {
                    labStatus.Text = "Planejamento da coleção de acessório não encontrado.";
                    pnlRelatorio.Visible = false;
                    ddlStatusResumo.Visible = false;
                }

                Session["COLECAO"] = ddlColecoesBuscar.SelectedValue;
            }
            catch (Exception ex)
            {
                labStatus.Text = "ERRO: " + ex.Message;
            }
        }
        private int RecarregarRelatorioGeral()
        {
            string col = "";

            col = ddlColecoesBuscar.SelectedValue.Trim();
            _geral = desenvController.ObterRelColecaoAcessorio(col);

            Session["REL_GERAL_ACESS"] = _geral;

            _geral = _geral.OrderBy(i => i.GRUPO).ThenBy(j => j.STATUS).ToList();
            gvGeral.DataSource = _geral;
            gvGeral.DataBind();

            return _geral.Count;
        }
        protected void gvGeral_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_DESENV_REL_ACESSResult _geral = e.Row.DataItem as SP_OBTER_DESENV_REL_ACESSResult;
                    if (_geral != null)
                    {

                        g_qtdePlanejada += _geral.QTDE_PLANEJADA;
                        g_qtdeAEntregar += _geral.QTDE_ENTREGAR;
                        g_qtdeEntregue += Convert.ToInt32(_geral.QTDE_ENTREGUE);

                        Literal _litPreco = e.Row.FindControl("litPreco") as Literal;
                        if (_litPreco != null)
                        {
                            _litPreco.Text = "R$ " + Convert.ToDecimal(_geral.PRECO).ToString("###,###,###,###,###0.00");
                            //Somatorio
                            g_preco += Convert.ToDecimal(_geral.PRECO);
                        }

                        int qtdeTotalLinha = 0;
                        if (_geral.STATUS == 5)
                        {
                            g_qtdeTotal += Convert.ToInt32(_geral.QTDE_ENTREGUE);
                            qtdeTotalLinha = Convert.ToInt32(_geral.QTDE_ENTREGUE);
                        }
                        else if (_geral.STATUS == 2 || _geral.STATUS == 3)
                        {
                            g_qtdeTotal += Convert.ToInt32(_geral.QTDE_ENTREGUE) + _geral.QTDE_ENTREGAR;
                            qtdeTotalLinha = Convert.ToInt32(_geral.QTDE_ENTREGUE) + _geral.QTDE_ENTREGAR;
                        }
                        else
                        {
                            qtdeTotalLinha = 0;
                        }

                        Literal _litValorTotal = e.Row.FindControl("litValorTotal") as Literal;
                        if (_litValorTotal != null)
                        {
                            _litValorTotal.Text = "R$ " + Convert.ToDecimal((qtdeTotalLinha * Convert.ToDecimal(_geral.PRECO))).ToString("###,###,###,###,###0.00");

                            //Somatorio
                            g_precoTotal += Convert.ToDecimal(_litValorTotal.Text.Replace("R$ ", ""));
                        }
                        //DESCRICAO STATUS
                        Literal _litStatus = e.Row.FindControl("litStatus") as Literal;
                        if (_litStatus != null)
                            _litStatus.Text = _geral.STATUS_DESC;

                        //Popular GRID VIEW FILHO
                        if (_geral.FOTO1 != null && _geral.FOTO1.Trim() != "")
                        {
                            GridView gvFoto = e.Row.FindControl("gvFoto") as GridView;
                            if (gvFoto != null)
                            {
                                List<DESENV_ACESSORIO> _fotoAcessorio = new List<DESENV_ACESSORIO>();
                                _fotoAcessorio.Add(new DESENV_ACESSORIO { CODIGO = _geral.CODIGO, FOTO1 = _geral.FOTO1, FOTO2 = _geral.FOTO2 });
                                gvFoto.DataSource = _fotoAcessorio;
                                gvFoto.DataBind();
                            }
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
        protected void gvFoto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    DESENV_ACESSORIO _acessorio = e.Row.DataItem as DESENV_ACESSORIO;
                    if (_acessorio != null)
                    {
                        System.Web.UI.WebControls.Image _imgFotoPeca = e.Row.FindControl("imgFotoPeca") as System.Web.UI.WebControls.Image;
                        if (_imgFotoPeca != null)
                            _imgFotoPeca.ImageUrl = _acessorio.FOTO1;

                        System.Web.UI.WebControls.Image _imgFotoPeca2 = e.Row.FindControl("imgFotoPeca2") as System.Web.UI.WebControls.Image;
                        if (_imgFotoPeca2 != null)
                            _imgFotoPeca2.ImageUrl = _acessorio.FOTO2;
                    }
                }
            }
        }

        protected void gvGeral_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = gvGeral.FooterRow;
            if (_footer != null)
            {
                _footer.Cells[2].Text = "Total";
                _footer.Cells[2].HorizontalAlign = HorizontalAlign.Left;

                //COLUNA  Qtde Planejada
                _footer.Cells[8].Text = g_qtdePlanejada.ToString();
                _footer.Cells[8].HorizontalAlign = HorizontalAlign.Center;
                //COLUNA Qtde Liberada
                _footer.Cells[9].Text = g_qtdeAEntregar.ToString();
                _footer.Cells[9].HorizontalAlign = HorizontalAlign.Center;
                //COLUNA Grade Real
                _footer.Cells[10].Text = g_qtdeEntregue.ToString();
                _footer.Cells[10].HorizontalAlign = HorizontalAlign.Center;
                //COLUNA Preço
                _footer.Cells[11].Text = "R$ " + g_preco.ToString("###,###,###,###,###0.00");
                _footer.Cells[11].HorizontalAlign = HorizontalAlign.Right;
                //COLUNA Valor Total
                _footer.Cells[12].Text = "R$ " + g_precoTotal.ToString("###,###,###,###,###0.00");
                _footer.Cells[12].HorizontalAlign = HorizontalAlign.Right;

                //Carregar valor final
                if (labTotalNacional.Text != "")
                {
                    decimal valorNacional = 0;
                    valorNacional = Convert.ToDecimal(labTotalNacional.Text.Replace("R$ ", ""));
                    labValorFinal.Text = "R$ " + (valorNacional - g_precoTotal).ToString("###,###,###,###,###0.00");
                }

                labValorTotalFiltro.Text = "R$ " + g_precoTotal.ToString("###,###,###,###,###0.00");
                labQtdeTotalFiltro.Text = g_qtdeTotal.ToString();
            }
        }

        #region "DADOS INICIAIS"
        private void CarregarColecoes()
        {
            List<COLECOE> _colecoes = baseController.BuscaColecoes();
            if (_colecoes != null)
            {
                _colecoes.Insert(0, new COLECOE { COLECAO = "0", DESC_COLECAO = "Selecione" });
                ddlColecoesBuscar.DataSource = _colecoes;
                ddlColecoesBuscar.DataBind();

                if (Session["COLECAO"] != null)
                    ddlColecoesBuscar.SelectedValue = Session["COLECAO"].ToString();
            }
        }
        private void CarregarGrupo()
        {
            List<SP_OBTER_GRUPOResult> _grupo = prodController.ObterGrupoProduto("02");
            if (_grupo != null)
            {
                _grupo = _grupo.Where(i => _geral.Any(g => g.GRUPO.Trim() == i.GRUPO_PRODUTO.Trim())).ToList();
                _grupo.Insert(0, new SP_OBTER_GRUPOResult { GRUPO_PRODUTO = "" });
                ddlGrupo.DataSource = _grupo;
                ddlGrupo.DataBind();
            }
        }
        private void CarregarOrigem()
        {
            List<DESENV_PRODUTO_ORIGEM> _origem = desenvController.ObterProdutoOrigem(ddlColecoesBuscar.SelectedValue);
            if (_origem != null)
            {
                _origem = _origem.Where(i => _geral.Any(g => g.DESCRICAO_ORIGEM.Trim().ToUpper() == i.DESCRICAO.Trim().ToUpper())).ToList();
                _origem.Insert(0, new DESENV_PRODUTO_ORIGEM { CODIGO = 0, DESCRICAO = "" });
                ddlOrigem.DataSource = _origem;
                ddlOrigem.DataBind();
            }
        }
        private void CarregarPedido()
        {
            List<SP_OBTER_DESENV_REL_ACESSResult> _geralPedido = new List<SP_OBTER_DESENV_REL_ACESSResult>();

            var _pedido = _geral.Select(s => s.PEDIDO).Distinct().ToList();
            foreach (var item in _pedido)
            {
                if (item.Trim() != "")
                    _geralPedido.Add(new SP_OBTER_DESENV_REL_ACESSResult { PEDIDO = item.Trim() });
            }

            _geralPedido = _geralPedido.OrderBy(p => Convert.ToInt32(p.PEDIDO)).ToList();
            _geralPedido.Insert(0, new SP_OBTER_DESENV_REL_ACESSResult { PEDIDO = "" });
            ddlNumeroPedido.DataSource = _geralPedido;
            ddlNumeroPedido.DataBind();
        }
        private void CarregarStatus(string col)
        {
            List<SP_OBTER_DESENV_REL_ACESSResult> _geralStatus = new List<SP_OBTER_DESENV_REL_ACESSResult>();

            var _status = _geral.Select(s => s.STATUS_DESC).Distinct().ToList();
            foreach (var item in _status)
            {
                if (item.Trim() != "")
                    _geralStatus.Add(new SP_OBTER_DESENV_REL_ACESSResult { STATUS_DESC = item.Trim() });
            }

            _geralStatus = _geralStatus.OrderBy(p => p.STATUS_DESC).ToList();
            _geralStatus.Insert(0, new SP_OBTER_DESENV_REL_ACESSResult { STATUS_DESC = "" });
            ddlStatus.DataSource = _geralStatus;
            ddlStatus.DataBind();

            ddlStatusResumo.DataSource = _geralStatus;
            ddlStatusResumo.DataBind();
        }

        #endregion

        #region "FILTROS"
        protected void ddlOrigem_SelectedIndexChanged(object sender, EventArgs e)
        { FiltroGeral("ORIGEM", (bool)ViewState["sortDirection"], true); }
        protected void ddlGrupo_SelectedIndexChanged(object sender, EventArgs e)
        { FiltroGeral("GRUPO", (bool)ViewState["sortDirection"], true); }
        protected void ddlNumeroPedido_SelectedIndexChanged(object sender, EventArgs e)
        { FiltroGeral("NUMEROPEDIDO", (bool)ViewState["sortDirection"], true); }
        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        { FiltroGeral("STATUS", (bool)ViewState["sortDirection"], true); }
        protected void gvGeral_Sorting(object sender, GridViewSortEventArgs e)
        {
            bool order;

            if ((bool)ViewState["sortDirection"])
                ViewState["sortDirection"] = false;
            else
                ViewState["sortDirection"] = true;

            order = (bool)ViewState["sortDirection"];

            GridView gv = (GridView)sender;
            if (gv != null)
            {
                gv.Columns[2].HeaderText = "Origem";
                gv.Columns[3].HeaderText = "Grupo";
                gv.Columns[4].HeaderText = "Produto";
                gv.Columns[5].HeaderText = "Nome";
                gv.Columns[6].HeaderText = "Cor";
                gv.Columns[7].HeaderText = "Pedido";
                gv.Columns[13].HeaderText = "Status";

                if (e.SortExpression == "ORIGEM") gv.Columns[2].HeaderText = gv.Columns[2].HeaderText + " " + ((order) ? " - >>" : " - <<");
                if (e.SortExpression == "GRUPO") gv.Columns[3].HeaderText = gv.Columns[3].HeaderText + " " + ((order) ? " - >>" : " - <<");
                if (e.SortExpression == "PRODUTO") gv.Columns[4].HeaderText = gv.Columns[4].HeaderText + " " + ((order) ? " - >>" : " - <<");
                if (e.SortExpression == "NOME") gv.Columns[5].HeaderText = gv.Columns[5].HeaderText + " " + ((order) ? " - >>" : " - <<");
                if (e.SortExpression == "COR") gv.Columns[6].HeaderText = gv.Columns[6].HeaderText + " " + ((order) ? " - >>" : " - <<");
                if (e.SortExpression == "PEDIDO") gv.Columns[7].HeaderText = gv.Columns[7].HeaderText + " " + ((order) ? " - >>" : " - <<");
                if (e.SortExpression == "STATUS") gv.Columns[13].HeaderText = gv.Columns[13].HeaderText + " " + ((order) ? " - >>" : " - <<");
            }

            FiltroGeral(e.SortExpression, order, false);
        }
        private void FiltroGeral(string pCampo, bool pAsc, bool pHeader)
        {
            string sOrigem = "";
            string sGrupo = "";
            string sNumeroPedido = "";
            string sStatus = "";

            if (Session["REL_GERAL_ACESS"] != null)
            {
                _geral.AddRange((List<SP_OBTER_DESENV_REL_ACESSResult>)Session["REL_GERAL_ACESS"]);

                //FILTROS DOS  COMBOS
                if (ddlOrigem.SelectedValue.Trim() != "0" && ddlOrigem.SelectedValue.Trim() != "")
                {
                    sOrigem = ddlOrigem.SelectedItem.Text;
                    _geral = _geral.Where(p => p.DESCRICAO_ORIGEM.Trim().ToUpper() == sOrigem.Trim().ToUpper()).ToList();
                }

                if (ddlGrupo.SelectedValue.Trim() != "0" && ddlGrupo.SelectedValue.Trim() != "")
                {
                    sGrupo = ddlGrupo.SelectedValue.Trim();
                    _geral = _geral.Where(p => p.GRUPO.Trim().ToUpper() == sGrupo.Trim().ToUpper()).ToList();
                }

                if (ddlNumeroPedido.SelectedValue.Trim() != "0" && ddlNumeroPedido.SelectedValue.Trim() != "")
                {
                    sNumeroPedido = ddlNumeroPedido.SelectedValue.Trim();
                    _geral = _geral.Where(p => p.PEDIDO.Trim().ToUpper() == sNumeroPedido.Trim().ToUpper()).ToList();
                }

                if (ddlStatus.SelectedValue.Trim() != "0" && ddlStatus.SelectedValue.Trim() != "")
                {
                    sStatus = ddlStatus.SelectedItem.Text.Trim();
                    _geral = _geral.Where(p => p.STATUS_DESC.Trim().ToUpper() == sStatus.Trim().ToUpper()).ToList();
                }

                //ORDENAÇÃO
                if (pAsc)
                {
                    if (pCampo == "ORIGEM") _geral = _geral.OrderBy(p => p.DESCRICAO_ORIGEM).ToList();
                    if (pCampo == "GRUPO") _geral = _geral.OrderBy(p => p.GRUPO).ToList();
                    if (pCampo == "STATUS") _geral = _geral.OrderBy(p => p.STATUS_DESC).ToList();
                    if (pCampo == "PRODUTO") _geral = _geral.OrderBy(p => p.PRODUTO).ToList();
                    if (pCampo == "NOME") _geral = _geral.OrderBy(p => p.NOME).ToList();
                    if (pCampo == "DESC_COR") _geral = _geral.OrderBy(p => p.DESC_COR).ToList();
                    if (pCampo == "PEDIDO") _geral = _geral.OrderBy(p => (p.PEDIDO.Trim() == "") ? 0 : Convert.ToInt32(p.PEDIDO)).ToList();
                }
                else
                {
                    if (pCampo == "ORIGEM") _geral = _geral.OrderByDescending(p => p.DESCRICAO_ORIGEM).ToList();
                    if (pCampo == "GRUPO") _geral = _geral.OrderByDescending(p => p.GRUPO).ToList();
                    if (pCampo == "STATUS") _geral = _geral.OrderByDescending(p => p.STATUS_DESC).ToList();
                    if (pCampo == "PRODUTO") _geral = _geral.OrderByDescending(p => p.PRODUTO).ToList();
                    if (pCampo == "NOME") _geral = _geral.OrderByDescending(p => p.NOME).ToList();
                    if (pCampo == "DESC_COR") _geral = _geral.OrderByDescending(p => p.DESC_COR).ToList();
                    if (pCampo == "PEDIDO") _geral = _geral.OrderByDescending(p => (p.PEDIDO.Trim() == "") ? 0 : Convert.ToInt32(p.PEDIDO)).ToList();
                }

                if (pHeader)
                {
                    gvGeral.Columns[2].HeaderText = "Origem";
                    gvGeral.Columns[3].HeaderText = "Grupo";
                    gvGeral.Columns[4].HeaderText = "Produto";
                    gvGeral.Columns[5].HeaderText = "Nome";
                    gvGeral.Columns[6].HeaderText = "Cor";
                    gvGeral.Columns[7].HeaderText = "Pedido";
                    gvGeral.Columns[13].HeaderText = "Status";
                }
                gvGeral.DataSource = _geral;
                gvGeral.DataBind();
            }
            else
            {
                RecarregarRelatorioGeral();
            }

        }

        #endregion

        #region "BUDGET"
        private void CarregarBudget()
        {
            List<DESENV_BUDGET> _budget = new List<DESENV_BUDGET>();
            decimal budgetNac = 0;
            decimal budgetImp = 0;
            decimal budgetVirado = 0;
            decimal budgetVoltaAtacado = 0;

            _budget = desenvController.ObterBudget(ddlColecoesBuscar.SelectedValue.Trim()).Where(p => p.TIPO_VENDA == 'C').ToList();

            foreach (DESENV_BUDGET bud in _budget)
            {
                if (bud.TIPO.ToUpper().Contains("NACIONAL"))
                {
                    budgetNac = bud.BUDGET;
                    labTotalNacional.Text = "R$ " + budgetNac.ToString("###,###,###,##0.00");
                }
                if (bud.TIPO.ToUpper().Contains("IMPORTADO"))
                {
                    budgetImp = bud.BUDGET;
                    labTotalImportacao.Text = "R$ " + budgetImp.ToString("###,###,###,##0.00");
                }
                if (bud.TIPO.ToUpper().Contains("VIRADO"))
                {
                    budgetVirado = bud.BUDGET;
                    labTotalVirado.Text = "R$ " + budgetVirado.ToString("###,###,###,##0.00");
                }
                if (bud.TIPO.ToUpper().Contains("ATACADO"))
                {
                    budgetVoltaAtacado = bud.BUDGET;
                    labTotalVoltaAtacado.Text = "R$ " + budgetVoltaAtacado.ToString("###,###,###,##0.00");
                }
            }
            labTotalBudget.Text = "R$ " + (budgetNac + budgetImp + budgetVirado + budgetVoltaAtacado).ToString("###,###,###,##0.00");
        }
        #endregion

        #region "RESUMO"
        protected void gvResumo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_DESENV_REL_ACESSRESUMOResult geralResumo = e.Row.DataItem as SP_OBTER_DESENV_REL_ACESSRESUMOResult;

                    if (geralResumo != null)
                    {
                        Literal _litValorTotal = e.Row.FindControl("litValorTotal") as Literal;
                        if (_litValorTotal != null)
                            _litValorTotal.Text = "R$ " + geralResumo.VALOR.ToString("###,###,###,###,###0.00");

                        g_qtdeSKU += geralResumo.SKU;
                        g_qtdeAcessorio += geralResumo.QTDE;
                        g_valorResumo += geralResumo.VALOR;
                    }
                }
            }
        }
        protected void gvResumo_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = gvResumo.FooterRow;
            if (_footer != null)
            {
                _footer.Cells[1].Text = "Total";
                _footer.Cells[1].HorizontalAlign = HorizontalAlign.Left;

                //COLUNA  Qtde SKU
                _footer.Cells[2].Text = g_qtdeSKU.ToString();
                _footer.Cells[2].HorizontalAlign = HorizontalAlign.Right;
                //COLUNA Quantidade de Acessórios
                _footer.Cells[3].Text = g_qtdeAcessorio.ToString();
                _footer.Cells[3].HorizontalAlign = HorizontalAlign.Right;
                //COLUNA Valor Total (Valorizado)
                _footer.Cells[4].Text = "R$ " + g_valorResumo.ToString("###,###,###,###,###0.00");
                _footer.Cells[4].HorizontalAlign = HorizontalAlign.Right;
            }
        }
        private void RecarregarRelatorioResumo(string status)
        {
            List<SP_OBTER_DESENV_REL_ACESSRESUMOResult> _lstResumo = new List<SP_OBTER_DESENV_REL_ACESSRESUMOResult>();

            _lstResumo = desenvController.ObterRelColecaoAcessorioResumo(ddlColecoesBuscar.SelectedValue.Trim(), status);
            gvResumo.DataSource = _lstResumo;
            gvResumo.DataBind();
        }
        protected void ddlStatusResumo_SelectedIndexChanged(object sender, EventArgs e)
        {
            RecarregarRelatorioResumo(ddlStatusResumo.SelectedValue);
        }
        #endregion

    }
}

