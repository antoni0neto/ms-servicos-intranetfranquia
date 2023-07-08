using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Drawing;
using DAL;

namespace Relatorios.mod_desenvolvimento
{
    public partial class desenv_acessorio_pedido : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();

        List<SP_OBTER_DESENV_ACESS_SEM_PEDIDOResult> g_AcessorioFiltro = new List<SP_OBTER_DESENV_ACESS_SEM_PEDIDOResult>();
        AcessoriosSemPedidoFiltro g_Filtro = new AcessoriosSemPedidoFiltro();

        private Boolean validaFiltro()
        {
            Boolean bRetorno = false;
            DateTime resultado;

            DateTime? dataInicio = null;
            DateTime? dataFim = null;

            if (txtDtPrazoInicio.Text.Trim() != "")
            {
                if (!DateTime.TryParse(this.txtDtPrazoInicio.Text.Trim(), out resultado))
                {
                    panMensagem.Visible = true;
                    txtMensagemErro.Text = "Data de Previsão de Entrega (Início) inválida.";
                    return bRetorno;
                }
                else
                {
                    dataInicio = Convert.ToDateTime(txtDtPrazoInicio.Text);
                }
            }

            if (txtDtPrazoFim.Text.Trim() != "")
            {
                if (!DateTime.TryParse(this.txtDtPrazoFim.Text.Trim(), out resultado))
                {
                    panMensagem.Visible = true;
                    txtMensagemErro.Text = "Data de Previsão de Entrega (Fim) inválida.";
                    return bRetorno;
                }
                else
                {
                    dataFim = Convert.ToDateTime(txtDtPrazoFim.Text);
                }
            }

            g_Filtro.dtPrazoInicio = dataInicio;
            g_Filtro.dtPrazoFim = dataFim;
            g_Filtro.Colecao = ddlColecoesFiltro.SelectedValue;
            g_Filtro.Origem = ddlOrigemFiltro.SelectedValue;
            g_Filtro.Grupo = ddlGrupoFiltro.SelectedValue;
            g_Filtro.Griffe = ddlGriffeFiltro.SelectedValue;
            g_Filtro.Cor = ddlCorFiltro.SelectedValue;
            g_Filtro.CorFornecedor = ddlCorFornecedorFiltro.SelectedValue;
            g_Filtro.Fornecedor = ddlFornecedorFiltro.SelectedValue;
            g_Filtro.Produto = txtProduto.Text;

            bRetorno = true;

            return bRetorno;
        }

        private List<SP_OBTER_GRUPOResult> RetornarGrupo()
        {
            return (prodController.ObterGrupoProduto("02"));
        }
        private List<DESENV_PRODUTO_ORIGEM> RetornarOrigem()
        {
            return (desenvController.ObterProdutoOrigem().Where(i => i.COLECAO.Trim() == ddlColecoesFiltro.SelectedValue.Trim() && i.STATUS == 'A').ToList());
        }
        private List<CORES_BASICA> RetornarCorLinx()
        {
            List<CORES_BASICA> _coresBasicas = new List<CORES_BASICA>();
            _coresBasicas = prodController.ObterCoresBasicas().ToList();
            _coresBasicas = _coresBasicas.GroupBy(p => new { COR = p.COR.Trim(), DESC_COR = p.DESC_COR.Trim() }).Select(x => new CORES_BASICA { COR = x.Key.COR.Trim(), DESC_COR = x.Key.DESC_COR.Trim() }).ToList();
            return (_coresBasicas);
        }
        private List<PROD_FORNECEDOR> RetornarFornecedor()
        {
            List<PROD_FORNECEDOR> _fornecedores = prodController.ObterFornecedor().ToList();
            _fornecedores.Insert(0, new PROD_FORNECEDOR { FORNECEDOR = "", STATUS = 'S' });
            return _fornecedores.Where(p => (p.STATUS == 'A' && p.TIPO == 'C') || p.STATUS == 'S').GroupBy(x => new { FORNECEDOR = x.FORNECEDOR.Trim() }).Select(f => new PROD_FORNECEDOR { FORNECEDOR = f.Key.FORNECEDOR.Trim() }).ToList();
        }
        private void CarregarColecoes()
        {
            BaseController _base = new BaseController();
            List<COLECOE> _colecoes = _base.BuscaColecoes();
            if (_colecoes != null)
            {
                _colecoes.Insert(0, new COLECOE { COLECAO = "0", DESC_COLECAO = "Selecione" });

                ddlColecoesFiltro.DataSource = _colecoes;
                ddlColecoesFiltro.DataBind();

                if (Session["COLECAO"] != null)
                {
                    ddlColecoesFiltro.SelectedValue = Session["COLECAO"].ToString();
                    CarregarOrigem(Session["COLECAO"].ToString().Trim());
                    ddlColecoesFiltro_SelectedIndexChanged(null, null);
                }
            }
        }
        private void CarregarGrupo()
        {
            List<SP_OBTER_GRUPOResult> _grupo = RetornarGrupo();
            if (_grupo != null)
            {
                _grupo.Insert(0, new SP_OBTER_GRUPOResult { GRUPO_PRODUTO = "" });
                ddlGrupoFiltro.DataSource = _grupo;
                ddlGrupoFiltro.DataBind();
            }
        }
        private void CarregarOrigem(string col)
        {
            List<DESENV_PRODUTO_ORIGEM> _origem = RetornarOrigem();
            _origem = _origem.Where(p => p.COLECAO.Trim() == col.Trim()).OrderBy(i => i.DESCRICAO).ToList();
            if (_origem != null)
            {
                _origem.Insert(0, new DESENV_PRODUTO_ORIGEM { CODIGO = 0, DESCRICAO = "" });
                ddlOrigemFiltro.DataSource = _origem;
                ddlOrigemFiltro.DataBind();

                if (_origem.Count == 2)
                    ddlOrigemFiltro.SelectedValue = _origem[1].CODIGO.ToString();

            }
        }
        private void CarregarGriffe()
        {
            List<PRODUTOS_GRIFFE> griffe = (new BaseController().BuscaGriffes());

            if (griffe != null)
            {
                griffe.Insert(0, new PRODUTOS_GRIFFE { GRIFFE = "" });
                ddlGriffeFiltro.DataSource = griffe;
                ddlGriffeFiltro.DataBind();
            }
        }
        private void CarregarCores()
        {
            List<CORES_BASICA> _cores = RetornarCorLinx();
            if (_cores != null)
            {
                _cores.Insert(0, new CORES_BASICA { COR = "", DESC_COR = "" });

                ddlCorFiltro.DataSource = _cores;
                ddlCorFiltro.DataBind();
            }
        }
        private void CarregarFornecedores()
        {
            List<PROD_FORNECEDOR> _fornecedores = RetornarFornecedor();

            if (_fornecedores != null)
            {
                ddlFornecedorFiltro.DataSource = _fornecedores;
                ddlFornecedorFiltro.DataBind();
            }
        }
        private void CarregarCorFornecedor()
        {
            List<DESENV_ACESSORIO> corFornecedorFiltro = new List<DESENV_ACESSORIO>();

            var _corFornecedor = g_AcessorioFiltro.Where(p => p.COR_FORNECEDOR != null).Select(s => s.COR_FORNECEDOR.Trim()).Distinct().ToList();
            foreach (var item in _corFornecedor)
                if (item.Trim() != "")
                    corFornecedorFiltro.Add(new DESENV_ACESSORIO { COR_FORNECEDOR = item.Trim() });

            corFornecedorFiltro = corFornecedorFiltro.OrderBy(p => p.COR_FORNECEDOR).ToList();
            corFornecedorFiltro.Insert(0, new DESENV_ACESSORIO { COR_FORNECEDOR = "" });
            ddlCorFornecedorFiltro.DataSource = corFornecedorFiltro;
            ddlCorFornecedorFiltro.DataBind();
        }

        private void marcarRow(GridViewRow dgItem)
        {
            dgItem.BorderWidth = 2;
            dgItem.BorderColor = Color.DarkBlue;
        }

        private void desmarcarRow(Int32 iLinha)
        {
            foreach (GridViewRow gvItem in gvAcessorio.Rows)
            {
                if (gvItem.RowIndex != iLinha)
                {
                    gvItem.BorderWidth = 0;
                    gvItem.BorderColor = Color.Empty;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["USUARIO"] == null)
                    Response.Redirect("desenv_menu.aspx");

                //Valida queryString
                if (Request.QueryString["t"] == null || Request.QueryString["t"] == "")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                string tela = Request.QueryString["t"].ToString();
                if (tela != "1" && tela != "2")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                if (tela == "1")
                    hrefVoltar.HRef = "desenv_menu.aspx";

                if (tela == "2")
                    hrefVoltar.HRef = "../mod_producao/prod_menu.aspx";

                //Carregar combos
                CarregarColecoes();
                CarregarGrupo();
                CarregarGriffe();
                CarregarFornecedores();
                CarregarCores();
                RecarregarAcessorioColecao("");
                CarregarCorFornecedor();

                //Foco no coleção
                ddlColecoesFiltro.Focus();
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
            Page.MaintainScrollPositionOnPostBack = true;
        }

        protected void ddlColecoesFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            string colecao = ddlColecoesFiltro.SelectedValue.Trim();
            if (colecao != "" && colecao != "0")
            {
                CarregarOrigem(colecao);
            }
        }

        protected void btLimpar_Click(object sender, EventArgs e)
        {
            try
            {
                txtDtPrazoInicio.Text = "";
                txtDtPrazoFim.Text = "";

                ddlColecoesFiltro.SelectedValue = "0";
                ddlOrigemFiltro.SelectedValue = "0";

                ddlGrupoFiltro.SelectedValue = "";
                txtProduto.Text = "";
                ddlGriffeFiltro.SelectedValue = "";
                ddlFornecedorFiltro.SelectedValue = "";

                ddlCorFiltro.SelectedValue = "";
                ddlCorFornecedorFiltro.SelectedValue = "";

                labSalvar.Text = "";

                pnlTripa.Visible = false;
            }
            catch (Exception ex)
            {
                panMensagem.Visible = true;
                txtMensagemErro.Text = "ERRO: " + ex.Message + "\n" + ex.StackTrace;
            }
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (validaFiltro() == true)
                {
                    RecarregarAcessorioColecao(g_Filtro.Colecao);

                    RecarregarAcessorioFiltro(g_Filtro);

                    carregaGvAcessorios();

                    pnlTripa.Visible = true;
                }
            }
            catch (Exception ex)
            {
                panMensagem.Visible = true;
                txtMensagemErro.Text = "ERRO: " + ex.Message + "\n" + ex.StackTrace;
            }
        }

        private void RecarregarAcessorioColecao(String colecao)
        {
            //Obter lista de acessorios
            List<SP_OBTER_DESENV_ACESS_SEM_PEDIDOResult> _lstAcessorio = new List<SP_OBTER_DESENV_ACESS_SEM_PEDIDOResult>();
            _lstAcessorio = desenvController.ObterDesenvAcessSemPedido(colecao);

            //Obter tudo
            g_AcessorioFiltro = new List<SP_OBTER_DESENV_ACESS_SEM_PEDIDOResult>();
            g_AcessorioFiltro.AddRange(_lstAcessorio);
        }

        private void RecarregarAcessorioFiltro(AcessoriosSemPedidoFiltro filtro)
        {
            //FILTRO DATA ENTREGA INICIO E FIM
            if (filtro.dtPrazoInicio == null && filtro.dtPrazoFim == null)
            {
                //g_AcessorioFiltro = g_AcessorioFiltro;
            }
            else if (filtro.dtPrazoInicio != null && filtro.dtPrazoFim == null)
            {
                g_AcessorioFiltro = g_AcessorioFiltro.Where(p => p.DATA_PREVISAO_ENTREGA.Trim() == "" | DateTime.Parse(p.DATA_PREVISAO_ENTREGA) >= filtro.dtPrazoInicio).ToList();
            }
            else if (filtro.dtPrazoInicio == null && filtro.dtPrazoFim != null)
            {
                g_AcessorioFiltro = g_AcessorioFiltro.Where(p => p.DATA_PREVISAO_ENTREGA.Trim() == "" | DateTime.Parse(p.DATA_PREVISAO_ENTREGA) <= filtro.dtPrazoFim).ToList();
            }
            else
            {
                g_AcessorioFiltro = g_AcessorioFiltro.Where(p => p.DATA_PREVISAO_ENTREGA.Trim() != "").ToList();
                g_AcessorioFiltro = g_AcessorioFiltro.Where(p => DateTime.Parse(p.DATA_PREVISAO_ENTREGA) >= filtro.dtPrazoInicio & DateTime.Parse(p.DATA_PREVISAO_ENTREGA) <= filtro.dtPrazoFim).ToList();
            }
            //FILTRO PRODUTO
            if (filtro.Produto.Trim() != "0" && filtro.Produto.Trim() != "")
                g_AcessorioFiltro = g_AcessorioFiltro.Where(p => p.PRODUTO.Trim().ToUpper() == filtro.Produto.Trim().ToUpper()).ToList();

            //FILTRO GRIFFE
            if (filtro.Griffe.Trim() != "0" && filtro.Griffe.Trim() != "")
                g_AcessorioFiltro = g_AcessorioFiltro.Where(p => ((p.GRIFFE == null) ? "" : p.GRIFFE.ToUpper().Trim()) == filtro.Griffe.ToUpper().Trim()).ToList();

            //FILTRO ORIGEM
            if (filtro.Origem.Trim() != "0" && filtro.Origem.Trim() != "")
                g_AcessorioFiltro = g_AcessorioFiltro.Where(p => p.DESENV_PRODUTO_ORIGEM.ToString() == filtro.Origem).ToList();

            //FILTRO GRUPO
            if (filtro.Grupo.Trim() != "0" && filtro.Grupo.Trim() != "")
                g_AcessorioFiltro = g_AcessorioFiltro.Where(p => ((p.GRUPO == null) ? "" : p.GRUPO.Trim()) == filtro.Grupo.Trim()).ToList();

            //FILTRO COR
            if (filtro.Cor.Trim() != "0" && filtro.Cor.Trim() != "")
                g_AcessorioFiltro = g_AcessorioFiltro.Where(p => p.COR.ToUpper().Trim() == filtro.Cor.ToUpper().Trim()).ToList();

            //FILTRO FORNECEDOR
            if (filtro.Fornecedor.Trim() != "0" && filtro.Fornecedor.Trim() != "")
                g_AcessorioFiltro = g_AcessorioFiltro.Where(p => ((p.FORNECEDOR == null) ? "" : p.FORNECEDOR.ToUpper().Trim()) == filtro.Fornecedor.ToUpper().Trim()).ToList();

            //FILTRO COR FORNECEDOR
            if (filtro.CorFornecedor.Trim() != "0" && filtro.CorFornecedor.Trim() != "")
                g_AcessorioFiltro = g_AcessorioFiltro.Where(p => ((p.COR_FORNECEDOR == null) ? "" : p.COR_FORNECEDOR.ToUpper().Trim()) == filtro.CorFornecedor.ToUpper().Trim()).ToList();

            Session["AcessoriosS"] = g_AcessorioFiltro;
        }

        private void carregaGvAcessorios()
        {
            gvAcessorio.DataSource = g_AcessorioFiltro;
            gvAcessorio.DataBind();
        }

        protected void btnFecharMensagem_Click(object sender, EventArgs e)
        {
            txtMensagemErro.Text = "";
            panMensagem.Visible = false;
        }

        protected void CalendarDataInicio_SelectionChanged(object sender, EventArgs e)
        {
            txtDtPrazoInicio.Text = CalendarDataInicio.SelectedDate.ToString("dd/MM/yyyy");
        }

        protected void CalendarDataFim_SelectionChanged(object sender, EventArgs e)
        {
            txtDtPrazoFim.Text = CalendarDataFim.SelectedDate.ToString("dd/MM/yyyy");
        }

        protected void chkTodos_CheckedChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvAcessorio.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("chkGerarPedido");

                if (chk != null)
                {
                    chk.Checked = (sender as CheckBox).Checked;
                }
            }
        }

        protected void gvAcessorio_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.DataItemIndex != -1)
                {
                    LinkButton lnk = e.Row.FindControl("lnkSel") as LinkButton;
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(lnk, ""));
                    e.Row.Attributes.Add("onMouseover", "this.style.background='#dddddd';this.style.cursor='hand'");
                    e.Row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(e.Row.BackColor) + "';this.style.cursor='hand'");
                }

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (e.Row.DataItem != null)
                    {
                        SP_OBTER_DESENV_ACESS_SEM_PEDIDOResult _acessorio = e.Row.DataItem as SP_OBTER_DESENV_ACESS_SEM_PEDIDOResult;

                        if (_acessorio != null)
                        {
                            Literal _litNome = e.Row.FindControl("litNome") as Literal;
                            if (_litNome != null)
                                _litNome.Text = _acessorio.NOME.Trim();

                            Label _labPrevisaoEntrega = e.Row.FindControl("labPrevisaoEntrega") as Label;
                            if (_labPrevisaoEntrega != null)
                                _labPrevisaoEntrega.Text = (_acessorio.DATA_PREVISAO_ENTREGA == null || _acessorio.DATA_PREVISAO_ENTREGA.Trim() == "") ? "" : (Convert.ToDateTime(_acessorio.DATA_PREVISAO_ENTREGA).ToString("dd/MM/yyyy"));

                            Label _labPreco = e.Row.FindControl("labPreco") as Label;
                            if (_labPreco != null)
                                _labPreco.Text = (_acessorio.PRECO == 0) ? "" : ("R$ " + Convert.ToDecimal(_acessorio.PRECO).ToString(""));

                            Label _labCusto = e.Row.FindControl("labCusto") as Label;
                            if (_labCusto != null)
                                _labCusto.Text = (_acessorio.CUSTO == 0) ? "" : ("R$ " + Convert.ToDecimal(_acessorio.CUSTO).ToString(""));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                panMensagem.Visible = true;
                txtMensagemErro.Text = "ERRO: " + ex.Message + "\n" + ex.StackTrace;
            }
        }

        protected void gvAcessorio_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                //if (validaFiltro() == true)
                //{
                //    RecarregarAcessorioColecao(g_Filtro.Colecao);

                //    RecarregarAcessorioFiltro(g_Filtro);
                //}

                g_AcessorioFiltro = (List<SP_OBTER_DESENV_ACESS_SEM_PEDIDOResult>)Session["AcessoriosS"];

                string sortExpression = e.SortExpression;
                SortDirection sort;

                Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

                if (sort == SortDirection.Ascending)
                    Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
                else
                    Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

                string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

                if (sortDirection == " DESC ")
                {
                    sort = SortDirection.Descending;
                }

                if (sortExpression == "PRODUTO")
                {
                    g_AcessorioFiltro = g_AcessorioFiltro.OrderBy(m => m.PRODUTO + sortDirection).ToList();
                }
                else if (sortExpression == "GRUPO")
                {
                    g_AcessorioFiltro = g_AcessorioFiltro.OrderBy(m => m.GRUPO + sortDirection).ToList();
                }
                else if (sortExpression == "NOME")
                {
                    g_AcessorioFiltro = g_AcessorioFiltro.OrderBy(m => m.NOME + sortDirection).ToList();
                }
                else if (sortExpression == "DESCRICAO_ORIGEM")
                {
                    g_AcessorioFiltro = g_AcessorioFiltro.OrderBy(m => m.DESCRICAO_ORIGEM + sortDirection).ToList();
                }
                else if (sortExpression == "GRIFFE")
                {
                    g_AcessorioFiltro = g_AcessorioFiltro.OrderBy(m => m.GRIFFE + sortDirection).ToList();
                }
                else if (sortExpression == "DESC_COR")
                {
                    g_AcessorioFiltro = g_AcessorioFiltro.OrderBy(m => m.DESC_COR + sortDirection).ToList();
                }
                else if (sortExpression == "COR_FORNECEDOR")
                {
                    g_AcessorioFiltro = g_AcessorioFiltro.OrderBy(m => m.COR_FORNECEDOR + sortDirection).ToList();
                }
                else if (sortExpression == "FORNECEDOR")
                {
                    g_AcessorioFiltro = g_AcessorioFiltro.OrderBy(m => m.FORNECEDOR + sortDirection).ToList();
                }
                else if (sortExpression == "DATA_PREVISAO_ENTREGA")
                {
                    g_AcessorioFiltro = g_AcessorioFiltro.OrderBy(m => m.DATA_PREVISAO_ENTREGA + sortDirection).ToList();
                }
                else if (sortExpression == "PRECO")
                {
                    g_AcessorioFiltro = g_AcessorioFiltro.OrderBy(m => m.PRECO + sortDirection).ToList();
                }
                else if (sortExpression == "CUSTO")
                {
                    g_AcessorioFiltro = g_AcessorioFiltro.OrderBy(m => m.CUSTO + sortDirection).ToList();
                }
                else if (sortExpression == "QTDE")
                {
                    g_AcessorioFiltro = g_AcessorioFiltro.OrderBy(m => m.QTDE + sortDirection).ToList();
                }
                else if (sortExpression == "DESCRICAO_SUGERIDA")
                {
                    g_AcessorioFiltro = g_AcessorioFiltro.OrderBy(m => m.DESCRICAO_SUGERIDA + sortDirection).ToList();
                }

                carregaGvAcessorios();
            }
            catch (Exception ex)
            {
                panMensagem.Visible = true;
                txtMensagemErro.Text = "ERRO:5 " + ex.Message + "\n" + ex.StackTrace;
            }
        }

        private string GerarPedidoCompraAcessorio(DESENV_ACESSORIO_PEDIDO pedidoAcessorio)
        {
            //string pedido = "";
            //string aprovadoPor = "";

            //aprovadoPor = "INTRANET-" + ((USUARIO)Session["USUARIO"]).NOME_USUARIO;

            //var pedidoCompra = desenvController.GerarPedidoCompraAcessorio(
            //    pedidoAcessorio.DESENV_ACESSORIO1.PRODUTO,
            //    pedidoAcessorio.DESENV_ACESSORIO1.COR,
            //    pedidoAcessorio.DESENV_ACESSORIO1.FORNECEDOR,
            //    pedidoAcessorio.QTDE_ORIGINAL,
            //    aprovadoPor,
            //    pedidoAcessorio.DATA_PREV_ENTREGA
            //    );

            //if (pedidoCompra != null)
            //    pedido = pedidoCompra.NUMERO_PEDIDO;

            //return pedido;
            return "";
        }

        protected void btnGerarPedido_Click(object sender, EventArgs e)
        {

            panMensagem.Visible = true;
            // tata solicitou a nao geracao de pedidos 31/01/2017
            txtMensagemErro.Text = "Pedidos não serão gerados.";


            //StringBuilder sbErro = new StringBuilder();
            //int codigoAcessorio;
            //bool validaPedido;
            //bool temErro = false;
            //System.Drawing.Color _OK = System.Drawing.Color.Gray;
            //System.Drawing.Color _notOK = System.Drawing.Color.Red;

            //try
            //{
            //    foreach (GridViewRow row in gvAcessorio.Rows)
            //    {
            //        HiddenField _hidCodigo = (HiddenField)row.FindControl("hidCodigo");
            //        Literal _litProduto = (Literal)row.FindControl("litProduto");
            //        Literal _litGrupo = (Literal)row.FindControl("litGrupo");
            //        Literal _litNome = (Literal)row.FindControl("litNome");
            //        Literal _litOrigem = (Literal)row.FindControl("litOrigem");
            //        Literal _litGriffe = (Literal)row.FindControl("litGriffe");
            //        Literal _litCor = (Literal)row.FindControl("litCor");
            //        Literal _litCorFornecedor = (Literal)row.FindControl("litCorFornecedor");
            //        Literal _litFornecedor = (Literal)row.FindControl("litFornecedor");
            //        Literal _litPrevisaoEntrega = (Literal)row.FindControl("litPrevisaoEntrega");
            //        Literal _litPreco = (Literal)row.FindControl("litPreco");
            //        Literal _litCusto = (Literal)row.FindControl("litCusto");
            //        Literal _litQuantidade = (Literal)row.FindControl("litQuantidade");
            //        Literal _litDescricaoSugerida = (Literal)row.FindControl("litDescricaoSugerida");
            //        Button _btnErros = (Button)row.FindControl("btnErros");
            //        HiddenField _hidErros = (HiddenField)row.FindControl("hidErros");

            //        CheckBox chk = (CheckBox)row.FindControl("chkGerarPedido");

            //        if (chk != null)
            //        {
            //            if (chk.Checked == true)
            //            {
            //                codigoAcessorio = 0;
            //                validaPedido = true;

            //                sbErro.Clear();

            //                if (_litProduto.Text.Trim() == "")
            //                {
            //                    sbErro.Append("- Produto em branco.\n");
            //                    validaPedido = false;
            //                    temErro = true;
            //                }

            //                if (_litCor.Text.Trim() == "")
            //                {
            //                    sbErro.Append("- Cor em branco.\n");
            //                    validaPedido = false;
            //                    temErro = true;
            //                }

            //                if (_litQuantidade.Text.Trim() == "")
            //                {
            //                    sbErro.Append("- Quantidade em branco.\n");
            //                    validaPedido = false;
            //                    temErro = true;
            //                }

            //                if (_litQuantidade.Text.Trim() == "0")
            //                {
            //                    sbErro.Append("- Quantidade deve ser maior que zero.\n");
            //                    validaPedido = false;
            //                    temErro = true;
            //                }

            //                if (_litPreco.Text.Trim() == "")
            //                {
            //                    sbErro.Append("- Preço em branco.\n");
            //                    validaPedido = false;
            //                    temErro = true;
            //                }

            //                if (_litPreco.Text.Trim() == "0")
            //                {
            //                    sbErro.Append("- Preço deve ser maior que zero.\n");
            //                    validaPedido = false;
            //                    temErro = true;
            //                }

            //                if (_litPrevisaoEntrega.Text.Trim() == "")
            //                {
            //                    sbErro.Append("- Previsão de Entrega em branco.\n");
            //                    validaPedido = false;
            //                    temErro = true;
            //                }


            //                if (validaPedido == false)
            //                {
            //                    row.BackColor = _notOK;
            //                    _hidErros.Value = sbErro.ToString();
            //                    _btnErros.Visible = true;
            //                    chk.Checked = false;
            //                }
            //                else
            //                {
            //                    codigoAcessorio = Convert.ToInt32(_hidCodigo.Value);

            //                    DESENV_ACESSORIO _acessorio = null;
            //                    _acessorio = desenvController.ObterAcessorio(codigoAcessorio);

            //                    string pedido = "";
            //                    if (validaPedido)
            //                    {

            //                        //validar produto cadastrado
            //                        var produtoCor = new BaseController().BuscaProdutoCor(_acessorio.PRODUTO, _acessorio.COR);
            //                        if (produtoCor != null)
            //                        {
            //                            DESENV_ACESSORIO_PEDIDO pedidoAcessorio = new DESENV_ACESSORIO_PEDIDO();
            //                            pedidoAcessorio.DESENV_ACESSORIO = codigoAcessorio;
            //                            pedidoAcessorio.PRECO = Convert.ToDecimal(_litPreco.Text.Trim());
            //                            pedidoAcessorio.QTDE_ORIGINAL = Convert.ToInt32(_litQuantidade.Text.Trim());
            //                            pedidoAcessorio.QTDE_O1 = 0;
            //                            pedidoAcessorio.QTDE_O2 = 0;
            //                            pedidoAcessorio.QTDE_O3 = 0;
            //                            pedidoAcessorio.QTDE_O4 = 0;
            //                            pedidoAcessorio.QTDE_O5 = 0;
            //                            pedidoAcessorio.QTDE_O6 = 0;
            //                            pedidoAcessorio.QTDE_O7 = 0;
            //                            pedidoAcessorio.QTDE_O8 = 0;
            //                            pedidoAcessorio.QTDE_O9 = 0;
            //                            pedidoAcessorio.QTDE_O10 = 0;
            //                            pedidoAcessorio.DATA_PEDIDO = DateTime.Now;
            //                            pedidoAcessorio.USUARIO_PEDIDO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
            //                            pedidoAcessorio.DATA_PREV_ENTREGA = Convert.ToDateTime(_litPrevisaoEntrega.Text.Trim());
            //                            desenvController.InserirAcessorioPedido(pedidoAcessorio);

            //                            //GERAR PEDIDO DE COMPRA NO LINX
            //                            pedido = GerarPedidoCompraAcessorio(pedidoAcessorio);

            //                            pedidoAcessorio.PEDIDO = pedido;
            //                            desenvController.AtualizarAcessorioPedido(pedidoAcessorio);
            //                        }
            //                        else
            //                        {
            //                            sbErro.Append("- Erro ao gerar PEDIDO DE COMPRA. PRODUTO ou COR não cadastrado.");
            //                        }
            //                    }

            //                    if (sbErro.ToString().Trim() == "")
            //                    {
            //                        row.BackColor = _OK;
            //                        chk.Visible = false;
            //                        _hidErros.Value = "";
            //                        _btnErros.Visible = false;
            //                    }
            //                    else
            //                    {
            //                        row.BackColor = _notOK;
            //                        _hidErros.Value = sbErro.ToString();
            //                        _btnErros.Visible = true;
            //                        chk.Checked = false;
            //                    }
            //                }
            //            }
            //        }
            //    }

            //    sbErro.Clear();
            //    if (temErro == true)
            //    {
            //        sbErro.Append("Nem todos os pedidos foram gerados.");
            //    }
            //    else
            //    {
            //        sbErro.Append("Pedidos gerados com Sucesso!!");
            //    }

            //    panMensagem.Visible = true;
            //    txtMensagemErro.Text = sbErro.ToString();
            //}
            //catch (Exception ex)
            //{
            //    panMensagem.Visible = true;
            //    txtMensagemErro.Text = "ERRO: " + ex.Message + "\n" + ex.StackTrace;
            //}
        }

        protected void gvAcessorio_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandArgument.ToString().Trim() == "")
                { return; }

                int resultado;
                if (!int.TryParse(e.CommandArgument.ToString(), out resultado))
                { return; }

                Int32 iIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow dgItem = gvAcessorio.Rows[iIndex];

                if (e.CommandName == "Ver_Erros")
                {
                    HiddenField _hidErros = (HiddenField)(dgItem.FindControl("hidErros"));

                    panMensagem.Visible = true;
                    txtMensagemErro.Text = _hidErros.Value.ToString();
                }
            }
            catch (Exception ex)
            {
                panMensagem.Visible = true;
                txtMensagemErro.Text = "ERRO: " + ex.Message + "\n" + ex.StackTrace;
            }
        }

        protected void chkGerarPedido_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox _chk = (CheckBox)sender;
            GridViewRow rowgrid = (GridViewRow)_chk.NamingContainer;
            desmarcarRow(-1);
            marcarRow(rowgrid);
        }
    }

    public class AcessoriosSemPedidoFiltro
    {
        public DateTime? dtPrazoInicio { get; set; }
        public DateTime? dtPrazoFim { get; set; }
        public string Colecao { get; set; }
        public string Origem { get; set; }
        public string Grupo { get; set; }
        public string Produto { get; set; }
        public string Griffe { get; set; }
        public string Fornecedor { get; set; }
        public string Cor { get; set; }
        public string CorFornecedor { get; set; }
    }
}