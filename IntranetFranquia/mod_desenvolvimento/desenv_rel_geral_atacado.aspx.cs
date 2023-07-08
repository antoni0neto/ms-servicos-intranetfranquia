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
    public partial class desenv_rel_geral_atacado : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();
        List<SP_OBTER_DESENV_REL_GERALResult> _geral = new List<SP_OBTER_DESENV_REL_GERALResult>();

        //gvGeral
        int coluna = 0;
        int g_qtdeAtacadoInicial = 0;
        int g_qtdeAtacadoLib = 0;
        int g_gradeReal = 0;
        int g_gradeAtacadoReal = 0;
        decimal g_preco = 0;
        decimal g_precoTotal = 0;
        //gvResumo
        int colunaResumo = 0;
        int g_qtdeSKU = 0;
        int g_qtdePeca = 0;
        decimal g_valorResumo = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregarColecoes();
                ddlStatusResumo.Visible = false;
                Session["REL_GERAL_ATACADO"] = null;
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
                    labStatus.Text = "Planejamento da coleção não encontrado.";
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
            char? status = null;

            col = ddlColecoesBuscar.SelectedValue.Trim();

            _geral = desenvController.ObterRelGeral(col, 'S', status);
            Session["REL_GERAL_ATACADO"] = _geral;
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
                    SP_OBTER_DESENV_REL_GERALResult _geral = e.Row.DataItem as SP_OBTER_DESENV_REL_GERALResult;
                    coluna += 1;
                    if (_geral != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = coluna.ToString();

                        //QTDE ATACADO INICIAL
                        g_qtdeAtacadoInicial += Convert.ToInt32(_geral.QTDE_ATACADO);
                        g_qtdeAtacadoLib += Convert.ToInt32(_geral.QTDE_ATACADO_LIB);

                        Literal _litGradeReal = e.Row.FindControl("litGradeReal") as Literal;
                        if (_litGradeReal != null)
                        {
                            _litGradeReal.Text = _geral.GRADE_REAL.ToString();
                            //Somatorio 
                            g_gradeReal += Convert.ToInt32(_geral.GRADE_REAL);
                        }

                        Literal _litPreco = e.Row.FindControl("litPreco") as Literal;
                        if (_litPreco != null)
                        {
                            _litPreco.Text = "R$ " + Convert.ToDecimal(_geral.PRECO_ATACADO).ToString("###,###,###,###,###0.00");
                            //Somatorio
                            g_preco += Convert.ToDecimal(_geral.PRECO_ATACADO);
                        }

                        int grade = 0;

                        //Soma total da grade para compor o total corretamente //LEANDRO
                        if (_geral.STATUS == "1" || _geral.STATUS == "3" || _geral.STATUS == "4")//ABERTO//PLANEJADO//EM ESTOQUE
                        {
                            g_gradeAtacadoReal += Convert.ToInt32(_geral.QTDE_ATACADO);
                            grade = Convert.ToInt32(_geral.QTDE_ATACADO);
                        }
                        else if (_geral.STATUS == "2")//CORTADO
                        {
                            g_gradeAtacadoReal += Convert.ToInt32(_geral.GRADE_REAL);
                            grade = Convert.ToInt32(_geral.GRADE_REAL);
                        }
                        else if (_geral.STATUS == "5")//A CORTAR
                        {
                            g_gradeAtacadoReal += Convert.ToInt32(_geral.QTDE_ATACADO_LIB);
                            grade = Convert.ToInt32(_geral.QTDE_ATACADO_LIB);
                        }

                        Literal _litValorTotal = e.Row.FindControl("litValorTotal") as Literal;
                        if (_litValorTotal != null)
                        {

                            _litValorTotal.Text = "R$ " + Convert.ToDecimal((grade * Convert.ToDecimal(_geral.PRECO_ATACADO))).ToString("###,###,###,###,###0.00");

                            //Somatorio
                            g_precoTotal += Convert.ToDecimal(_litValorTotal.Text.Replace("R$ ", ""));
                        }
                        //DESCRICAO STATUS
                        Literal _litStatus = e.Row.FindControl("litStatus") as Literal;
                        if (_litStatus != null)
                            _litStatus.Text = _geral.STATUS_DESC;

                        //Popular GRID VIEW FILHO
                        if (_geral.FOTO != null && _geral.FOTO.Trim() != "")
                        {
                            GridView gvFoto = e.Row.FindControl("gvFoto") as GridView;
                            if (gvFoto != null)
                            {
                                List<DESENV_PRODUTO> _fotoProduto = new List<DESENV_PRODUTO>();
                                _fotoProduto.Add(new DESENV_PRODUTO { CODIGO = _geral.COD_PRODUTO, FOTO = _geral.FOTO, FOTO2 = _geral.FOTO2 });
                                gvFoto.DataSource = _fotoProduto;
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
                    DESENV_PRODUTO _produto = e.Row.DataItem as DESENV_PRODUTO;
                    if (_produto != null)
                    {
                        System.Web.UI.WebControls.Image _imgFotoPeca = e.Row.FindControl("imgFotoPeca") as System.Web.UI.WebControls.Image;
                        if (_imgFotoPeca != null)
                            _imgFotoPeca.ImageUrl = _produto.FOTO;

                        System.Web.UI.WebControls.Image _imgFotoPeca2 = e.Row.FindControl("imgFotoPeca2") as System.Web.UI.WebControls.Image;
                        if (_imgFotoPeca2 != null)
                            _imgFotoPeca2.ImageUrl = _produto.FOTO2;
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
                _footer.Cells[7].Text = g_qtdeAtacadoInicial.ToString();
                _footer.Cells[7].HorizontalAlign = HorizontalAlign.Center;
                //COLUNA Qtde Liberada
                _footer.Cells[8].Text = g_qtdeAtacadoLib.ToString();
                _footer.Cells[8].HorizontalAlign = HorizontalAlign.Center;
                //COLUNA Grade Real
                _footer.Cells[9].Text = g_gradeReal.ToString();
                _footer.Cells[9].HorizontalAlign = HorizontalAlign.Center;
                //COLUNA Preço
                /*_footer.Cells[10].Text = "R$ " + g_preco.ToString("###,###,###,###,###0.00");
                _footer.Cells[10].HorizontalAlign = HorizontalAlign.Right;*/
                //COLUNA Valor Total
                _footer.Cells[11].Text = "R$ " + g_precoTotal.ToString("###,###,###,###,###0.00");
                _footer.Cells[11].HorizontalAlign = HorizontalAlign.Right;

                //Carregar valor final
                if (labTotalNacional.Text != "")
                {
                    decimal valorNacional = 0;
                    valorNacional = Convert.ToDecimal(labTotalNacional.Text.Replace("R$ ", ""));
                    labValorFinal.Text = "R$ " + (valorNacional - g_precoTotal).ToString("###,###,###,###,###0.00");
                }

                labValorTotalFiltro.Text = "R$ " + g_precoTotal.ToString("###,###,###,###,###0.00");
                labQtdeTotalFiltro.Text = g_gradeAtacadoReal.ToString();
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
            List<SP_OBTER_GRUPOResult> _grupo = prodController.ObterGrupoProduto("01");
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
                _origem = _origem.Where(i => _geral.Any(g => g.ORIGEM.Trim().ToUpper() == i.DESCRICAO.Trim().ToUpper())).ToList();
                _origem.Insert(0, new DESENV_PRODUTO_ORIGEM { CODIGO = 0, DESCRICAO = "" });
                ddlOrigem.DataSource = _origem;
                ddlOrigem.DataBind();
            }
        }
        private void CarregarPedido()
        {
            List<SP_OBTER_DESENV_REL_GERALResult> _geralPedido = new List<SP_OBTER_DESENV_REL_GERALResult>();

            var _pedido = _geral.Select(s => s.NUMERO_PEDIDO).Distinct().ToList();
            foreach (var item in _pedido)
            {
                if (item.Trim() != "")
                    _geralPedido.Add(new SP_OBTER_DESENV_REL_GERALResult { NUMERO_PEDIDO = item.Trim() });
            }

            _geralPedido = _geralPedido.OrderBy(p => Convert.ToInt32(p.NUMERO_PEDIDO)).ToList();
            _geralPedido.Insert(0, new SP_OBTER_DESENV_REL_GERALResult { NUMERO_PEDIDO = "" });
            ddlNumeroPedido.DataSource = _geralPedido;
            ddlNumeroPedido.DataBind();
        }

        private void CarregarStatus(string col)
        {
            ddlStatus.Items.Clear();
            ddlStatusResumo.Items.Clear();

            ddlStatus.Items.Add(new ListItem { Value = "0", Text = "" });
            ddlStatus.Items.Add(new ListItem { Value = "1", Text = "ABERTO" });
            ddlStatus.Items.Add(new ListItem { Value = "2", Text = "CORTADO" });
            ddlStatus.Items.Add(new ListItem { Value = "3", Text = "PLANEJADO" });
            //ddlStatus.Items.Add(new ListItem { Value = "4", Text = "EM ESTOQUE" });

            ddlStatusResumo.Items.Add(new ListItem { Value = "0", Text = "" });
            ddlStatusResumo.Items.Add(new ListItem { Value = "1", Text = "ABERTO" });
            ddlStatusResumo.Items.Add(new ListItem { Value = "2", Text = "CORTADO" });
            ddlStatusResumo.Items.Add(new ListItem { Value = "3", Text = "PLANEJADO" });
            //ddlStatusResumo.Items.Add(new ListItem { Value = "4", Text = "EM ESTOQUE" });

            /*if (col.Trim() != "19")
            {
                ddlStatus.Items.Add(new ListItem { Value = "5", Text = "A CORTAR" });
                ddlStatusResumo.Items.Add(new ListItem { Value = "5", Text = "A CORTAR" });
            }*/
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
                /*
                [2] - Origem (ORIGEM)
                [3] - Grupo (GRUPO)
                [4] - Modelo (MODELO)
                [5] - Nome (NOME)
                [6] - Cor (COR)
                [8] - No. do Pedido (NUMEROPEDIDO)
                [9] - HB (HB)
                [12] - Status (STATUS)
                */

                gv.Columns[2].HeaderText = "Origem";
                gv.Columns[3].HeaderText = "Grupo";
                gv.Columns[4].HeaderText = "Modelo";
                gv.Columns[5].HeaderText = "Nome";
                gv.Columns[6].HeaderText = "Cor";
                gv.Columns[7].HeaderText = "Qtde Planejada";
                gv.Columns[9].HeaderText = "Grade Real";
                //gv.Columns[8].HeaderText = "No. do Pedido";
                //gv.Columns[9].HeaderText = "HB";
                gv.Columns[12].HeaderText = "Status";

                if (e.SortExpression == "ORIGEM") gv.Columns[2].HeaderText = gv.Columns[2].HeaderText + " " + ((order) ? " - >>" : " - <<");
                if (e.SortExpression == "GRUPO") gv.Columns[3].HeaderText = gv.Columns[3].HeaderText + " " + ((order) ? " - >>" : " - <<");
                if (e.SortExpression == "MODELO") gv.Columns[4].HeaderText = gv.Columns[4].HeaderText + " " + ((order) ? " - >>" : " - <<");
                if (e.SortExpression == "NOME") gv.Columns[5].HeaderText = gv.Columns[5].HeaderText + " " + ((order) ? " - >>" : " - <<");
                if (e.SortExpression == "COR") gv.Columns[6].HeaderText = gv.Columns[6].HeaderText + " " + ((order) ? " - >>" : " - <<");
                if (e.SortExpression == "QTDE_ATACADO") gv.Columns[7].HeaderText = gv.Columns[7].HeaderText + " " + ((order) ? " - >>" : " - <<");
                if (e.SortExpression == "GRADE_REAL") gv.Columns[9].HeaderText = gv.Columns[9].HeaderText + " " + ((order) ? " - >>" : " - <<");

                if (e.SortExpression == "STATUS") gv.Columns[12].HeaderText = gv.Columns[12].HeaderText + " " + ((order) ? " - >>" : " - <<");
            }

            FiltroGeral(e.SortExpression, order, false);
        }
        private void FiltroGeral(string pCampo, bool pAsc, bool pHeader)
        {
            string sOrigem = "";
            string sGrupo = "";
            string sNumeroPedido = "";
            string sStatus = "";

            if (Session["REL_GERAL_ATACADO"] != null)
            {
                _geral.AddRange((List<SP_OBTER_DESENV_REL_GERALResult>)Session["REL_GERAL_ATACADO"]);


                //FILTROS DOS  COMBOS
                if (ddlOrigem.SelectedValue.Trim() != "0" && ddlOrigem.SelectedValue.Trim() != "")
                {
                    sOrigem = ddlOrigem.SelectedItem.Text;
                    _geral = _geral.Where(p => p.ORIGEM.Trim().ToUpper() == sOrigem.Trim().ToUpper()).ToList();
                }

                if (ddlGrupo.SelectedValue.Trim() != "0" && ddlGrupo.SelectedValue.Trim() != "")
                {
                    sGrupo = ddlGrupo.SelectedValue.Trim();
                    _geral = _geral.Where(p => p.GRUPO.Trim().ToUpper() == sGrupo.Trim().ToUpper()).ToList();
                }

                if (ddlNumeroPedido.SelectedValue.Trim() != "0" && ddlNumeroPedido.SelectedValue.Trim() != "")
                {
                    sNumeroPedido = ddlNumeroPedido.SelectedValue.Trim();
                    _geral = _geral.Where(p => p.NUMERO_PEDIDO.Trim().ToUpper() == sNumeroPedido.Trim().ToUpper()).ToList();
                }

                if (ddlStatus.SelectedValue.Trim() != "0" && ddlStatus.SelectedValue.Trim() != "")
                {
                    sStatus = ddlStatus.SelectedItem.Text.Trim();
                    _geral = _geral.Where(p => p.STATUS_DESC.Trim().ToUpper() == sStatus.Trim().ToUpper()).ToList();
                }

                //ORDENAÇÃO
                if (pAsc)
                {
                    if (pCampo == "ORIGEM") _geral = _geral.OrderBy(p => p.ORIGEM).ToList();
                    if (pCampo == "GRUPO") _geral = _geral.OrderBy(p => p.GRUPO).ToList();
                    if (pCampo == "STATUS") _geral = _geral.OrderBy(p => p.STATUS_DESC).ToList();
                    if (pCampo == "MODELO") _geral = _geral.OrderBy(p => p.MODELO).ToList();
                    if (pCampo == "NOME") _geral = _geral.OrderBy(p => p.NOME).ToList();
                    if (pCampo == "COR") _geral = _geral.OrderBy(p => p.COR).ToList();

                    if (pCampo == "QTDE_ATACADO") _geral = _geral.OrderBy(p => p.QTDE_ATACADO).ToList();
                    if (pCampo == "GRADE_REAL") _geral = _geral.OrderBy(p => p.GRADE_REAL).ToList();

                    //if (pCampo == "HB") _geral = _geral.OrderBy(p => (p.HB.Trim() == "") ? 0 : Convert.ToInt32(p.HB)).ToList();
                    //if (pCampo == "NUMEROPEDIDO") _geral = _geral.OrderBy(p => (p.NUMERO_PEDIDO.Trim() == "") ? 0 : Convert.ToInt32(p.NUMERO_PEDIDO)).ToList();
                }
                else
                {
                    if (pCampo == "ORIGEM") _geral = _geral.OrderByDescending(p => p.ORIGEM).ToList();
                    if (pCampo == "GRUPO") _geral = _geral.OrderByDescending(p => p.GRUPO).ToList();
                    if (pCampo == "STATUS") _geral = _geral.OrderByDescending(p => p.STATUS_DESC).ToList();
                    if (pCampo == "MODELO") _geral = _geral.OrderByDescending(p => p.MODELO).ToList();
                    if (pCampo == "NOME") _geral = _geral.OrderByDescending(p => p.NOME).ToList();
                    if (pCampo == "COR") _geral = _geral.OrderByDescending(p => p.COR).ToList();

                    if (pCampo == "QTDE_ATACADO") _geral = _geral.OrderByDescending(p => p.QTDE_ATACADO).ToList();
                    if (pCampo == "GRADE_REAL") _geral = _geral.OrderByDescending(p => p.GRADE_REAL).ToList();

                    //if (pCampo == "HB") _geral = _geral.OrderByDescending(p => (p.HB.Trim() == "") ? 0 : Convert.ToInt32(p.HB)).ToList();
                    //if (pCampo == "NUMEROPEDIDO") _geral = _geral.OrderByDescending(p => (p.NUMERO_PEDIDO.Trim() == "") ? 0 : Convert.ToInt32(p.NUMERO_PEDIDO)).ToList();
                }

                if (pHeader)
                {
                    gvGeral.Columns[2].HeaderText = "Origem";
                    gvGeral.Columns[3].HeaderText = "Grupo";
                    gvGeral.Columns[4].HeaderText = "Modelo";
                    gvGeral.Columns[5].HeaderText = "Nome";
                    gvGeral.Columns[6].HeaderText = "Cor";
                    gvGeral.Columns[7].HeaderText = "Qtde Planejada";
                    gvGeral.Columns[9].HeaderText = "Grade Real";
                    gvGeral.Columns[12].HeaderText = "Status";
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

            _budget = desenvController.ObterBudget(ddlColecoesBuscar.SelectedValue.Trim()).Where(p => p.TIPO_VENDA == 'A').ToList();

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
            }
            labTotalBudget.Text = "R$ " + (budgetNac + budgetImp).ToString("###,###,###,##0.00");
        }
        #endregion

        #region "RESUMO"
        protected void gvResumo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_DESENV_REL_RESUMOResult geralResumo = e.Row.DataItem as SP_OBTER_DESENV_REL_RESUMOResult;
                    colunaResumo += 1;
                    if (geralResumo != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = colunaResumo.ToString();

                        Literal _litValorTotal = e.Row.FindControl("litValorTotal") as Literal;
                        if (_litValorTotal != null)
                            _litValorTotal.Text = "R$ " + geralResumo.VALOR.ToString("###,###,###,###,###0.00");

                        g_qtdeSKU += geralResumo.SKU;
                        g_qtdePeca += geralResumo.QTDE;
                        g_valorResumo += geralResumo.VALOR;
                        //g_valorResumoTotal += Convert.ToDecimal((geralResumo.PECA_QTDE * geralResumo.VALOR) / geralResumo.GRUPO_QTDE);
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
                //COLUNA Quantidade de Peças
                _footer.Cells[3].Text = g_qtdePeca.ToString();
                _footer.Cells[3].HorizontalAlign = HorizontalAlign.Right;
                //COLUNA Valor Total (Valorizado)
                _footer.Cells[4].Text = "R$ " + g_valorResumo.ToString("###,###,###,###,###0.00");
                _footer.Cells[4].HorizontalAlign = HorizontalAlign.Right;
            }
        }
        private void RecarregarRelatorioResumo(string status)
        {
            char? cStatus = null;
            List<SP_OBTER_DESENV_REL_RESUMOResult> _lstResumo = new List<SP_OBTER_DESENV_REL_RESUMOResult>();

            if (status != "" && status != "0")
                cStatus = Convert.ToChar(status);

            _lstResumo = desenvController.ObterRelatorioResumo(ddlColecoesBuscar.SelectedValue.Trim(), 'S', cStatus);
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
