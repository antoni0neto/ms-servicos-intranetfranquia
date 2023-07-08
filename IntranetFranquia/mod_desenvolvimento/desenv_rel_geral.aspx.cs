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

namespace Relatorios
{
    public partial class desenv_rel_geral : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();
        List<SP_OBTER_DESENV_REL_GERALResult> _geral = new List<SP_OBTER_DESENV_REL_GERALResult>();

        //gvGeral
        int coluna = 0;
        int g_qtdeInicial = 0;
        int g_qtdeLib = 0;
        int g_gradeReal = 0;
        decimal g_preco = 0;

        int g_gradeRealFem = 0;
        int g_gradeRealMasc = 0;
        decimal g_precoTotalFem = 0;
        decimal g_precoTotalMasc = 0;


        //gvResumo
        int colunaResumo = 0;
        int g_qtdeSKU = 0;
        int g_qtdePeca = 0;
        decimal g_valorResumo = 0;

        const string PRODUCAO_VAREJO = "REL_GERAL_PRODUCAO_VAREJO";
        const string PRODUCAO_ATACADO = "REL_GERAL_PRODUCAO_ATACADO";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["t"] == null || Request.QueryString["t"] == "" || Session["USUARIO"] == null)
                    Response.Redirect("Login.aspx");

                string tipo = Request.QueryString["t"].ToString();

                if (tipo != "V" && tipo != "A")
                    Response.Redirect("Login.aspx");

                hidTipo.Value = tipo;

                if (tipo == "A")
                {
                    labTitulo.Text = "Produção Brasil Atacado";
                    labSubTitulo.Text = labTitulo.Text;
                }
                else
                {
                    labTitulo.Text = "Produção Brasil Varejo";
                    labSubTitulo.Text = labTitulo.Text;
                }

                CarregarColecoes();
                ddlStatusResumo.Visible = false;

                if (hidTipo.Value == "A")
                    Session[PRODUCAO_ATACADO] = null;
                else
                    Session[PRODUCAO_VAREJO] = null;

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
                    CarregarGriffe();
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
            char atacado = (hidTipo.Value == "A") ? 'S' : 'N';

            col = ddlColecoesBuscar.SelectedValue.Trim();

            _geral = desenvController.ObterRelGeral(col, atacado, status);

            if (hidTipo.Value == "A")
                Session[PRODUCAO_ATACADO] = _geral;
            else
                Session[PRODUCAO_VAREJO] = _geral;

            _geral = _geral.OrderBy(i => i.GRUPO).ThenBy(j => j.STATUS).ToList();

            gvGeral.Columns[8].SortExpression = (hidTipo.Value == "A") ? "QTDE_ATACADO" : "VAREJO";
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

                        //QTDE INICIAL
                        g_qtdeInicial += (hidTipo.Value == "A") ? Convert.ToInt32(_geral.QTDE_ATACADO) : Convert.ToInt32(_geral.VAREJO);
                        g_qtdeLib += (hidTipo.Value == "A") ? Convert.ToInt32(_geral.QTDE_ATACADO_LIB) : Convert.ToInt32(_geral.QTDE_VAREJO_LIB);

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
                            _litPreco.Text = "R$ " + ((hidTipo.Value == "A") ? Convert.ToDecimal(_geral.PRECO_ATACADO) : Convert.ToDecimal(_geral.PRECO)).ToString("###,###,###,###,###0.00");
                            //Somatorio
                            g_preco += (hidTipo.Value == "A") ? Convert.ToDecimal(_geral.PRECO_ATACADO) : Convert.ToDecimal(_geral.PRECO);
                        }

                        int grade = 0;

                        //Soma total da grade para compor o total corretamente //LEANDRO
                        if (_geral.STATUS == "1" || _geral.STATUS == "3" || _geral.STATUS == "4")//ABERTO//PLANEJADO//EM ESTOQUE
                        {
                            if (_geral.GRIFFE == "FEMININO")
                                g_gradeRealFem += (hidTipo.Value == "A") ? Convert.ToInt32(_geral.QTDE_ATACADO) : Convert.ToInt32(_geral.VAREJO);
                            else if (_geral.GRIFFE == "MASCULINO")
                                g_gradeRealMasc += (hidTipo.Value == "A") ? Convert.ToInt32(_geral.QTDE_ATACADO) : Convert.ToInt32(_geral.VAREJO);
                            grade = (hidTipo.Value == "A") ? Convert.ToInt32(_geral.QTDE_ATACADO) : Convert.ToInt32(_geral.VAREJO);
                        }
                        else if (_geral.STATUS == "2")//CORTADO
                        {
                            if (_geral.GRIFFE == "FEMININO")
                                g_gradeRealFem += Convert.ToInt32(_geral.GRADE_REAL);
                            else if (_geral.GRIFFE == "MASCULINO")
                                g_gradeRealMasc += Convert.ToInt32(_geral.GRADE_REAL);
                            grade = (Convert.ToInt32(_geral.GRADE_REAL) <= 0) ? ((hidTipo.Value == "A") ? Convert.ToInt32(_geral.QTDE_ATACADO) : Convert.ToInt32(_geral.VAREJO)) : Convert.ToInt32(_geral.GRADE_REAL);
                        }
                        else if (_geral.STATUS == "5")//A CORTAR
                        {
                            if (_geral.GRIFFE == "FEMININO")
                                g_gradeRealFem += (hidTipo.Value == "A") ? Convert.ToInt32(_geral.QTDE_ATACADO_LIB) : Convert.ToInt32(_geral.QTDE_VAREJO_LIB);
                            else if (_geral.GRIFFE == "MASCULINO")
                                g_gradeRealMasc += (hidTipo.Value == "A") ? Convert.ToInt32(_geral.QTDE_ATACADO_LIB) : Convert.ToInt32(_geral.QTDE_VAREJO_LIB);
                            grade = (hidTipo.Value == "A") ? Convert.ToInt32(_geral.QTDE_ATACADO_LIB) : Convert.ToInt32(_geral.QTDE_VAREJO_LIB);
                        }

                        Literal _litValorTotal = e.Row.FindControl("litValorTotal") as Literal;
                        if (_litValorTotal != null)
                        {

                            _litValorTotal.Text = "R$ " + Convert.ToDecimal((grade * ((hidTipo.Value == "A") ? Convert.ToDecimal(_geral.PRECO_ATACADO) : Convert.ToDecimal(_geral.PRECO)))).ToString("###,###,###,###,###0.00");

                            //Somatorio
                            if (_geral.GRIFFE == "FEMININO")
                                g_precoTotalFem += Convert.ToDecimal(_litValorTotal.Text.Replace("R$ ", ""));
                            else if (_geral.GRIFFE == "MASCULINO")
                                g_precoTotalMasc += Convert.ToDecimal(_litValorTotal.Text.Replace("R$ ", ""));

                        }
                        //DESCRICAO STATUS
                        Literal _litStatus = e.Row.FindControl("litStatus") as Literal;
                        if (_litStatus != null)
                            _litStatus.Text = _geral.STATUS_DESC;

                        Literal _litQtdePlanejada = e.Row.FindControl("litQtdePlanejada") as Literal;
                        if (_litQtdePlanejada != null)
                            _litQtdePlanejada.Text = (hidTipo.Value == "A") ? _geral.QTDE_ATACADO.ToString() : _geral.VAREJO.ToString();

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
                _footer.Cells[8].Text = g_qtdeInicial.ToString();
                _footer.Cells[8].HorizontalAlign = HorizontalAlign.Center;
                //COLUNA Qtde Liberada
                _footer.Cells[9].Text = g_qtdeLib.ToString();
                _footer.Cells[9].HorizontalAlign = HorizontalAlign.Center;
                //COLUNA Grade Real
                _footer.Cells[10].Text = g_gradeReal.ToString();
                _footer.Cells[10].HorizontalAlign = HorizontalAlign.Center;

                //COLUNA Valor Total
                _footer.Cells[12].Text = "R$ " + (g_precoTotalFem + g_precoTotalMasc).ToString("###,###,###,###,###0.00");
                _footer.Cells[12].HorizontalAlign = HorizontalAlign.Right;


                decimal valorNacionalFem = 0;
                decimal valorNacionalMasc = 0;
                //Carregar valor final
                if (labTotalNacionalFem.Text != "")
                {
                    valorNacionalFem = Convert.ToDecimal(labTotalNacionalFem.Text.Replace("R$ ", ""));
                    labValorFinalFem.Text = "R$ " + (valorNacionalFem - g_precoTotalFem).ToString("###,###,###,###,###0.00");
                }

                //Carregar valor final
                if (labTotalNacionalMasc.Text != "")
                {

                    valorNacionalMasc = Convert.ToDecimal(labTotalNacionalMasc.Text.Replace("R$ ", ""));
                    labValorFinalMasc.Text = "R$ " + (valorNacionalMasc - g_precoTotalMasc).ToString("###,###,###,###,###0.00");
                }

                //Carregar valor final
                if (labTotalNacional.Text != "")
                {
                    decimal valorNacional = 0;
                    valorNacional = Convert.ToDecimal(labTotalNacional.Text.Replace("R$ ", ""));
                    labValorFinal.Text = "R$ " + ((valorNacionalFem + valorNacionalMasc) - (g_precoTotalFem + g_precoTotalMasc)).ToString("###,###,###,###,###0.00");
                }

                labValorTotalFiltroFem.Text = "R$ " + (g_precoTotalFem).ToString("###,###,###,###,###0.00");
                labQtdeTotalFiltroFem.Text = (g_gradeRealFem).ToString();

                labValorTotalFiltroMasc.Text = "R$ " + (g_precoTotalMasc).ToString("###,###,###,###,###0.00");
                labQtdeTotalFiltroMasc.Text = (g_gradeRealMasc).ToString();

                labValorTotalFiltro.Text = "R$ " + (g_precoTotalFem + g_precoTotalMasc).ToString("###,###,###,###,###0.00");
                labQtdeTotalFiltro.Text = (g_gradeRealFem + g_gradeRealMasc).ToString();
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
        private void CarregarGriffe()
        {
            List<SP_OBTER_DESENV_REL_GERALResult> _geralGriffe = new List<SP_OBTER_DESENV_REL_GERALResult>();

            var _griffe = _geral.Select(s => s.GRIFFE).Distinct().ToList();
            foreach (var item in _griffe)
            {
                if (item.Trim() != "")
                    _geralGriffe.Add(new SP_OBTER_DESENV_REL_GERALResult { GRIFFE = item.Trim() });
            }

            _geralGriffe = _geralGriffe.OrderBy(p => p.GRIFFE).ToList();
            _geralGriffe.Insert(0, new SP_OBTER_DESENV_REL_GERALResult { GRIFFE = "" });
            ddlGriffe.DataSource = _geralGriffe;
            ddlGriffe.DataBind();
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
        { FiltroGeral(); }
        protected void ddlGrupo_SelectedIndexChanged(object sender, EventArgs e)
        { FiltroGeral(); }
        protected void ddlGriffe_SelectedIndexChanged(object sender, EventArgs e)
        { FiltroGeral(); }
        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        { FiltroGeral(); }
        protected void gvGeral_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<SP_OBTER_DESENV_REL_GERALResult> geralProducao = null;


            if (hidTipo.Value == "A")
            {
                if (Session[PRODUCAO_ATACADO] != null)
                    geralProducao = ((IEnumerable<SP_OBTER_DESENV_REL_GERALResult>)Session[PRODUCAO_ATACADO]);
            }
            else
            {
                if (Session[PRODUCAO_VAREJO] != null)
                    geralProducao = ((IEnumerable<SP_OBTER_DESENV_REL_GERALResult>)Session[PRODUCAO_VAREJO]);
            }

            geralProducao = Filtrar(geralProducao);

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            geralProducao = geralProducao.OrderBy(e.SortExpression + sortDirection);
            gvGeral.DataSource = geralProducao;
            gvGeral.DataBind();

        }
        private void FiltroGeral()
        {
            if (hidTipo.Value == "A")
            {
                if (Session[PRODUCAO_ATACADO] != null)
                    _geral.AddRange((List<SP_OBTER_DESENV_REL_GERALResult>)Session[PRODUCAO_ATACADO]);
            }
            else
            {
                if (Session[PRODUCAO_VAREJO] != null)
                    _geral.AddRange((List<SP_OBTER_DESENV_REL_GERALResult>)Session[PRODUCAO_VAREJO]);
            }

            gvGeral.DataSource = Filtrar(_geral);
            gvGeral.DataBind();
        }

        private IEnumerable<SP_OBTER_DESENV_REL_GERALResult> Filtrar(IEnumerable<SP_OBTER_DESENV_REL_GERALResult> _geral)
        {
            string sOrigem = "";
            string sGrupo = "";
            string sGriffe = "";
            string sStatus = "";

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

            if (ddlGriffe.SelectedValue.Trim() != "")
            {
                sGriffe = ddlGriffe.SelectedValue.Trim();
                _geral = _geral.Where(p => p.GRIFFE.Trim().ToUpper() == sGriffe.Trim().ToUpper()).ToList();
            }

            if (ddlStatus.SelectedValue.Trim() != "0" && ddlStatus.SelectedValue.Trim() != "")
            {
                sStatus = ddlStatus.SelectedItem.Text.Trim();
                _geral = _geral.Where(p => p.STATUS_DESC.Trim().ToUpper() == sStatus.Trim().ToUpper()).ToList();
            }

            return _geral;
        }

        #endregion

        #region "BUDGET"
        private void CarregarBudget()
        {
            List<DESENV_BUDGET> _budget = new List<DESENV_BUDGET>();
            decimal budgetNacFem = 0, budgetNacMasc = 0;
            decimal budgetImpFem = 0, budgetImpMasc = 0;
            decimal budgetViradoFem = 0, budgetViradoMasc = 0;
            decimal budgetVoltaAtacadoFem = 0, budgetVoltaAtacadoMasc = 0;

            char atacado = (hidTipo.Value == "A") ? 'A' : 'V';


            _budget = desenvController.ObterBudget(ddlColecoesBuscar.SelectedValue.Trim()).Where(p => p.TIPO_VENDA == atacado).ToList();

            foreach (DESENV_BUDGET bud in _budget)
            {
                if (bud.TIPO.ToUpper().Contains("NACIONAL"))
                {
                    if (bud.GRIFFE == "FEMININO")
                    {
                        budgetNacFem = bud.BUDGET;
                        labTotalNacionalFem.Text = "R$ " + budgetNacFem.ToString("###,###,###,##0.00");
                    }
                    else if (bud.GRIFFE == "MASCULINO")
                    {
                        budgetNacMasc = bud.BUDGET;
                        labTotalNacionalMasc.Text = "R$ " + budgetNacMasc.ToString("###,###,###,##0.00");
                    }
                }
                if (bud.TIPO.ToUpper().Contains("IMPORTADO"))
                {
                    if (bud.GRIFFE == "FEMININO")
                    {
                        budgetImpFem = bud.BUDGET;
                        labTotalImportacaoFem.Text = "R$ " + budgetImpFem.ToString("###,###,###,##0.00");
                    }
                    else if (bud.GRIFFE == "MASCULINO")
                    {
                        budgetImpMasc = bud.BUDGET;
                        labTotalImportacaoMasc.Text = "R$ " + budgetImpMasc.ToString("###,###,###,##0.00");
                    }
                }
                if (bud.TIPO.ToUpper().Contains("VIRADO"))
                {
                    if (bud.GRIFFE == "FEMININO")
                    {
                        budgetViradoFem = bud.BUDGET;
                        labTotalViradoFem.Text = "R$ " + budgetViradoFem.ToString("###,###,###,##0.00");
                    }
                    else if (bud.GRIFFE == "MASCULINO")
                    {
                        budgetViradoMasc = bud.BUDGET;
                        labTotalViradoMasc.Text = "R$ " + budgetViradoMasc.ToString("###,###,###,##0.00");
                    }
                }
                if (bud.TIPO.ToUpper().Contains("ATACADO"))
                {
                    if (bud.GRIFFE == "FEMININO")
                    {
                        budgetVoltaAtacadoFem = bud.BUDGET;
                        labTotalVoltaAtacadoFem.Text = "R$ " + budgetVoltaAtacadoFem.ToString("###,###,###,##0.00");
                    }
                    else if (bud.GRIFFE == "MASCULINO")
                    {
                        budgetVoltaAtacadoMasc = bud.BUDGET;
                        labTotalVoltaAtacadoMasc.Text = "R$ " + budgetVoltaAtacadoMasc.ToString("###,###,###,##0.00");
                    }
                }
            }

            labTotalNacional.Text = "R$ " + (budgetNacFem + budgetNacMasc).ToString("###,###,###,##0.00");
            labTotalImportacao.Text = "R$ " + (budgetImpFem + budgetImpMasc).ToString("###,###,###,##0.00");
            labTotalVirado.Text = "R$ " + (budgetViradoFem + budgetViradoMasc).ToString("###,###,###,##0.00");
            labTotalVoltaAtacado.Text = "R$ " + (budgetVoltaAtacadoFem + budgetVoltaAtacadoMasc).ToString("###,###,###,##0.00");

            labTotalBudgetFem.Text = "R$ " + (budgetNacFem + budgetImpFem + budgetViradoFem + budgetVoltaAtacadoFem).ToString("###,###,###,##0.00");
            labTotalBudgetMasc.Text = "R$ " + (budgetNacMasc + budgetImpMasc + budgetViradoMasc + budgetVoltaAtacadoMasc).ToString("###,###,###,##0.00");
            labTotalBudget.Text = "R$ " + (
                (budgetNacFem + budgetNacMasc) +
                (budgetImpFem + budgetImpMasc) +
                (budgetViradoFem + budgetViradoMasc) +
                (budgetVoltaAtacadoFem + budgetVoltaAtacadoMasc)).ToString("###,###,###,##0.00");

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
            List<SP_OBTER_DESENV_REL_RESUMOResult> _lstResumo = new List<SP_OBTER_DESENV_REL_RESUMOResult>();
            char? cStatus = null;
            char atacado = (hidTipo.Value == "A") ? 'S' : 'N';

            if (status != "" && status != "0")
                cStatus = Convert.ToChar(status);

            _lstResumo = desenvController.ObterRelatorioResumo(ddlColecoesBuscar.SelectedValue.Trim(), atacado, cStatus);
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

