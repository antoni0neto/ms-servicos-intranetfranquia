using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq.Expressions;
using System.Linq.Dynamic;

namespace Relatorios.mod_desenvolvimento
{
    public partial class desenv_produto_liberacao : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();
        NotificacaoController notController = new NotificacaoController();

        int colunaProduto = 0;
        //PRODUTOS
        decimal qtdeEstoque = 0;
        decimal consumoLiberada = 0;
        decimal consumoReservada = 0;
        decimal consumoSaldo = 0;

        int qtdeTotalOriginal = 0;
        int qtdeTotalLiberada = 0;
        decimal qtdeTotalConsumo = 0;

        List<DESENV_PRODUTO_FICTEC> g_FichaTecnica = new List<DESENV_PRODUTO_FICTEC>();
        List<DESENV_PEDIDO> g_Pedido = new List<DESENV_PEDIDO>();

        List<DESENV_PEDIDO> g_PedidoFiltro = new List<DESENV_PEDIDO>();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                CarregarGrupos();
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#tabs').tabs(); });", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$('#tabs').tabs('option', 'active', '" + ((hidTabSelected.Value == "") ? "0" : hidTabSelected.Value) + "');", true);
        }

        #region "DADOS INICIAIS"
        private void CarregarListasAuxiliares()
        {
            g_FichaTecnica = desenvController.ObterFichaTecnica();
            g_Pedido = desenvController.ObterPedido1000().Where(p => p.STATUS != 'E').ToList();
        }
        private void CarregarGrupos()
        {
            List<MATERIAIS_GRUPO> _matGrupo = desenvController.ObterMaterialGrupo();
            if (_matGrupo != null)
            {
                _matGrupo.Insert(0, new MATERIAIS_GRUPO { CODIGO_GRUPO = "", GRUPO = "Selecione" });
                ddlMaterialGrupo.DataSource = _matGrupo;
                ddlMaterialGrupo.DataBind();

                //ddlMaterialGrupo.SelectedValue = "04";
                //ddlMaterialGrupo_SelectedIndexChanged(ddlMaterialGrupo, null);
            }
        }
        protected void ddlMaterialGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            if (ddl.ID == "ddlMaterialGrupo")
                CarregarSubGrupos(ddlMaterialGrupo.SelectedItem.Text.Trim());
        }
        private void CarregarSubGrupos(string grupo)
        {
            CarregarListasAuxiliares();

            //Obter cores do fornecedor da ficha tecnica
            var _subGrupo = g_FichaTecnica;
            _subGrupo = _subGrupo.Where(p => p.GRUPO.Trim() == grupo.Trim() &&
                                            p.SUBGRUPO != null && p.SUBGRUPO.Trim() != "").ToList();

            var cf1 = _subGrupo.GroupBy(p => new { SUBGRUPO = p.SUBGRUPO.Trim() }).Select(k => new DESENV_PRODUTO_FICTEC { SUBGRUPO = k.Key.SUBGRUPO }).ToList();

            //Obter cores do fornecedor dos pedidos
            var _subGrupoPedido = g_Pedido;
            _subGrupoPedido = _subGrupoPedido.Where(p => p.GRUPO.Trim() == grupo.Trim() &&
                                                    p.SUBGRUPO != null && p.SUBGRUPO.Trim() != "").ToList();
            var cf2 = _subGrupoPedido.GroupBy(p => new { SUBGRUPO = p.SUBGRUPO.Trim() }).Select(k => new DESENV_PRODUTO_FICTEC { SUBGRUPO = k.Key.SUBGRUPO }).ToList();

            //AUXILIAR para juntar as duas lista
            List<DESENV_PRODUTO_FICTEC> cF = new List<DESENV_PRODUTO_FICTEC>();
            cF.AddRange(cf1);
            cF.AddRange(cf2);

            //Separar duplicatas
            cF = cF.GroupBy(p => new { SUBGRUPO = p.SUBGRUPO.Trim() }).Select(k => new DESENV_PRODUTO_FICTEC { SUBGRUPO = k.Key.SUBGRUPO }).OrderBy(p => p.SUBGRUPO).ToList();

            cF.Insert(0, new DESENV_PRODUTO_FICTEC { SUBGRUPO = "Selecione" });
            ddlMaterialSubGrupo.DataSource = cF;
            ddlMaterialSubGrupo.DataBind();

        }
        protected void ddlMaterialSubGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            if (ddl.ID == "ddlMaterialSubGrupo")
                CarregarCores(ddlMaterialGrupo.SelectedItem.Text.Trim(), ddlMaterialSubGrupo.SelectedValue.Trim());
        }
        private void CarregarCores(string grupo, string subGrupo)
        {
            CarregarListasAuxiliares();

            //Obter cores do fornecedor da ficha tecnica
            var _cor = g_FichaTecnica;
            _cor = _cor.Where(p => p.GRUPO.Trim() == grupo.Trim() &&
                                    p.SUBGRUPO.Trim() == subGrupo.Trim() &&
                                    p.PROD_DETALHE == null &&
                                    p.COR_MATERIAL != null && p.COR_MATERIAL.Trim() != "").ToList();
            var cf1 = _cor.GroupBy(p => new { COR_MATERIAL = p.COR_MATERIAL.Trim() }).Select(k => new DESENV_PRODUTO_FICTEC { COR_MATERIAL = k.Key.COR_MATERIAL }).ToList();

            //Obter cores do fornecedor dos pedidos
            /*var _corPedido = g_Pedido;
            _corPedido = _corPedido.Where(p => p.GRUPO.Trim() == grupo.Trim() &&
                                            p.SUBGRUPO.Trim() == subGrupo.Trim() &&
                                            p.COR != null && p.COR.Trim() != "").ToList();
            var cf2 = _corPedido.GroupBy(p => new { COR = p.COR.Trim() }).Select(k => new DESENV_PRODUTO_FICTEC { COR_MATERIAL = k.Key.COR }).ToList();*/

            //AUXILIAR para juntar as duas lista
            List<DESENV_PRODUTO_FICTEC> cF = new List<DESENV_PRODUTO_FICTEC>();
            cF.AddRange(cf1);
            //cF.AddRange(cf2);

            //Separar duplicatas
            cF = cF.GroupBy(p => new { COR_MATERIAL = p.COR_MATERIAL.Trim() }).Select(k => new DESENV_PRODUTO_FICTEC { COR_MATERIAL = k.Key.COR_MATERIAL }).ToList();

            var _coresBasicas = ObterCoresBasicas();
            _coresBasicas = _coresBasicas.Where(p => cF.Any(x => x.COR_MATERIAL.Trim() == p.COR.Trim())).ToList();

            _coresBasicas.Insert(0, new CORES_BASICA { COR = "", DESC_COR = "Selecione" });
            ddlCor.DataSource = _coresBasicas;
            ddlCor.DataBind();

            if (_coresBasicas.Count == 2)
            {
                ddlCor.SelectedIndex = 1;
                ddlCor_SelectedIndexChanged(ddlCor, null);
            }

        }
        protected void ddlCor_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            if (ddl.ID == "ddlCor")
                CarregarCorFornecedor(ddlMaterialGrupo.SelectedItem.Text.Trim(), ddlMaterialSubGrupo.SelectedValue.Trim(), ddlCor.SelectedValue.Trim());
        }
        private void CarregarCorFornecedor(string grupo, string subGrupo, string cor)
        {
            CarregarListasAuxiliares();

            //Obter cores do fornecedor da ficha tecnica
            var _corFornecedor = g_FichaTecnica;
            _corFornecedor = _corFornecedor.Where(p => p.GRUPO.Trim() == grupo.Trim() &&
                                                        p.SUBGRUPO.Trim() == subGrupo.Trim() &&
                                                        p.COR_MATERIAL.Trim() == cor.Trim() &&
                                                        p.PROD_DETALHE == null &&
                                                        p.COR_FORNECEDOR != null && p.COR_FORNECEDOR.Trim() != "").ToList();
            var cf1 = _corFornecedor.GroupBy(p => new { COR_FORNECEDOR = p.COR_FORNECEDOR.Trim() }).Select(k => new DESENV_PRODUTO_FICTEC { COR_FORNECEDOR = k.Key.COR_FORNECEDOR }).ToList();

            //Obter cores do fornecedor dos pedidos
            /*var _corFornecedorPedido = g_Pedido;
            _corFornecedorPedido = _corFornecedorPedido.Where(p => p.GRUPO.Trim() == grupo.Trim() &&
                                                                p.SUBGRUPO.Trim() == subGrupo.Trim() &&
                                                                p.COR.Trim() == cor.Trim() &&
                                                                p.COR_FORNECEDOR != null && p.COR_FORNECEDOR.Trim() != "").ToList();
            var cf2 = _corFornecedorPedido.GroupBy(p => new { COR_FORNECEDOR = p.COR_FORNECEDOR.Trim() }).Select(k => new DESENV_PRODUTO_FICTEC { COR_FORNECEDOR = k.Key.COR_FORNECEDOR }).ToList();*/

            //AUXILIAR para juntar as duas lista
            List<DESENV_PRODUTO_FICTEC> cF = new List<DESENV_PRODUTO_FICTEC>();
            cF.AddRange(cf1);
            //cF.AddRange(cf2);

            //Separar duplicatas
            cF = cF.GroupBy(p => new { COR_FORNECEDOR = p.COR_FORNECEDOR.Trim() }).Select(k => new DESENV_PRODUTO_FICTEC { COR_FORNECEDOR = k.Key.COR_FORNECEDOR }).OrderBy(p => p.COR_FORNECEDOR).ToList();

            cF.Insert(0, new DESENV_PRODUTO_FICTEC { COR_FORNECEDOR = "Selecione" });
            ddlCorFornecedor.DataSource = cF;
            ddlCorFornecedor.DataBind();

            if (cF.Count == 2)
                ddlCorFornecedor.SelectedIndex = 1;

        }
        private List<CORES_BASICA> ObterCoresBasicas()
        {
            List<CORES_BASICA> _cores = new List<CORES_BASICA>();
            _cores = prodController.ObterCoresBasicas();
            return _cores;
        }

        private void CarregarFiltros(List<SP_OBTER_LIBERACAO_PRODUTOResult> _produtoLiberacao)
        {
            var _modelo = _produtoLiberacao.GroupBy(p => new { MODELO = p.MODELO }).Select(x => new SP_OBTER_LIBERACAO_PRODUTOResult { MODELO = x.Key.MODELO }).ToList();
            ddlModeloFiltro.DataSource = _modelo;
            ddlModeloFiltro.DataBind();
            ddlModeloFiltro.Items.Insert(0, new ListItem { Value = "", Text = "" });

            var _nome = _produtoLiberacao.GroupBy(p => new { DESC_PRODUTO = p.DESC_PRODUTO }).Select(x => new SP_OBTER_LIBERACAO_PRODUTOResult { DESC_PRODUTO = x.Key.DESC_PRODUTO }).ToList();
            ddlNomeFiltro.DataSource = _nome;
            ddlNomeFiltro.DataBind();
            ddlNomeFiltro.Items.Insert(0, new ListItem { Value = "", Text = "" });
        }
        #endregion

        #region "AÇÕES DA TELA"
        private void MoverAba(string vAba)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$('#tabs').tabs('option', 'active', '" + vAba + "');", true);
            hidTabSelected.Value = vAba;
        }
        #endregion

        #region "PRODUTOS"
        private List<SP_OBTER_LIBERACAO_PRODUTOResult> ObterLiberacaoProduto()
        {
            List<SP_OBTER_LIBERACAO_PRODUTOResult> _liberacaoProduto = new List<SP_OBTER_LIBERACAO_PRODUTOResult>();

            _liberacaoProduto = desenvController.ObterLiberacaoProduto(
                "",
                "",
                ddlMaterialGrupo.SelectedItem.Text.Trim(),
                ddlMaterialSubGrupo.SelectedItem.Text.Trim(),
                ddlCor.SelectedValue.Trim(),
                ddlCorFornecedor.SelectedValue.Trim(),
                "");

            return _liberacaoProduto;
        }
        private void CarregarProdutos(string modelo, string nome, string status, bool filtro)
        {
            var _liberacaoProduto = ObterLiberacaoProduto();

            var _liberacaoProdutoGrupo = _liberacaoProduto.GroupBy(p => new
            {
                DESENV_PRODUTO = p.DESENV_PRODUTO,
                MODELO = p.MODELO,
                COR = p.COR,
                GRUPO = p.GRUPO,
                SUBGRUPO = p.SUBGRUPO,
                COR_MATERIAL = p.COR_MATERIAL,
                DESC_COR = p.DESC_COR,
                COR_FORNECEDOR = p.COR_FORNECEDOR,
                TIPO = p.TIPO,
                LIBERADO = p.LIBERADO,
                DESC_PRODUTO = p.DESC_PRODUTO,
                STATUS = p.STATUS,
                QTDE_ORIGINAL = p.QTDE_ORIGINAL,
                FOTO = p.FOTO,
                FOTO2 = p.FOTO2,
                CODIGO_PRODUTO = p.CODIGO_PRODUTO,
                COLECAO = p.COLECAO
            }).Select(x => new SP_OBTER_LIBERACAO_PRODUTOResult
            {
                DESENV_PRODUTO = x.Key.DESENV_PRODUTO,
                MODELO = x.Key.MODELO,
                COR = x.Key.COR,
                GRUPO = x.Key.GRUPO,
                SUBGRUPO = x.Key.SUBGRUPO,
                COR_MATERIAL = x.Key.COR_MATERIAL,
                DESC_COR = x.Key.DESC_COR,
                COR_FORNECEDOR = x.Key.COR_FORNECEDOR,
                TIPO = x.Key.TIPO,
                QTDE_LIBERADA = x.Sum(g => g.QTDE_LIBERADA),
                CONSUMO = x.Sum(g => g.CONSUMO),
                LIBERADO = x.Key.LIBERADO,
                DESC_PRODUTO = x.Key.DESC_PRODUTO,
                STATUS = x.Key.STATUS,
                QTDE_ORIGINAL = x.Key.QTDE_ORIGINAL,
                FOTO = x.Key.FOTO,
                FOTO2 = x.Key.FOTO2,
                CODIGO_PRODUTO = x.Key.CODIGO_PRODUTO,
                COLECAO = x.Key.COLECAO
            }).ToList();

            if (ddlTipo.SelectedValue != "")
                _liberacaoProdutoGrupo = _liberacaoProdutoGrupo.Where(p => p.TIPO.Substring(0, 1) == ddlTipo.SelectedValue).ToList();

            if (modelo != "")
                _liberacaoProdutoGrupo = _liberacaoProdutoGrupo.Where(p => p.MODELO == modelo).ToList();

            if (nome != "")
                _liberacaoProdutoGrupo = _liberacaoProdutoGrupo.Where(p => p.DESC_PRODUTO == nome).ToList();

            if (status != "")
            {
                if (status == "V") //VAZIO
                    _liberacaoProdutoGrupo = _liberacaoProdutoGrupo.Where(p => p.STATUS == null).ToList();
                else
                    _liberacaoProdutoGrupo = _liberacaoProdutoGrupo.Where(p => p.STATUS.ToString() == status).ToList();
            }

            if (_liberacaoProdutoGrupo == null || _liberacaoProdutoGrupo.Count() <= 0)
                labErro.Text = "Nenhum Produto encontrado. Refaça sua pesquisa.";

            if (!filtro)
                CarregarFiltros(_liberacaoProduto);

            gvProdutos.DataSource = _liberacaoProdutoGrupo;
            gvProdutos.DataBind();

            //Carregar qtdes status
            CarregarQtdeTecido(_liberacaoProduto);
        }
        protected void gvProdutos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_LIBERACAO_PRODUTOResult _liberacaoProduto = e.Row.DataItem as SP_OBTER_LIBERACAO_PRODUTOResult;

                    colunaProduto += 1;
                    if (_liberacaoProduto != null)
                    {

                        Literal _litColuna = e.Row.FindControl("litColuna") as Literal;
                        if (_litColuna != null)
                            _litColuna.Text = colunaProduto.ToString();

                        if (_liberacaoProduto.DESENV_PRODUTO > 0)
                        {

                            TextBox _txtQtde = e.Row.FindControl("txtQtde") as TextBox;
                            if (_txtQtde != null)
                                _txtQtde.Text = _liberacaoProduto.QTDE_LIBERADA.ToString();

                            ImageButton _btLiberarProduto = e.Row.FindControl("btLiberarProduto") as ImageButton;
                            DropDownList _ddlLiberacao = e.Row.FindControl("ddlLiberacao") as DropDownList;
                            if (_ddlLiberacao != null)
                            {
                                if (_liberacaoProduto.STATUS != null)
                                {
                                    _ddlLiberacao.SelectedValue = _liberacaoProduto.STATUS.ToString();
                                    if (_liberacaoProduto.STATUS == 'L' || _liberacaoProduto.STATUS == 'S')
                                    {
                                        _btLiberarProduto.Visible = false;
                                        _ddlLiberacao.Enabled = false;
                                        _txtQtde.Enabled = false;
                                    }
                                }
                            }

                            HiddenField _hidQtdeAtual = e.Row.FindControl("hidQtdeAtual") as HiddenField;
                            if (_hidQtdeAtual != null)
                                _hidQtdeAtual.Value = _liberacaoProduto.QTDE_LIBERADA.ToString();

                            HiddenField _hidStatusAtual = e.Row.FindControl("hidStatusAtual") as HiddenField;
                            if (_hidStatusAtual != null)
                                _hidStatusAtual.Value = _liberacaoProduto.STATUS.ToString();

                            //SOMATORIO
                            qtdeTotalLiberada += Convert.ToInt32(_liberacaoProduto.QTDE_LIBERADA);
                            qtdeTotalOriginal += Convert.ToInt32(_liberacaoProduto.QTDE_ORIGINAL);
                            qtdeTotalConsumo += Convert.ToDecimal(_liberacaoProduto.CONSUMO);

                            Label _labQtdeVendaAtacado = e.Row.FindControl("labQtdeVendaAtacado") as Label;
                            if (_labQtdeVendaAtacado != null)
                            {
                                var qtdeVendaAtacado = desenvController.ObterVendaAtacadoProduto(_liberacaoProduto.COLECAO, _liberacaoProduto.MODELO, _liberacaoProduto.COR);
                                if (qtdeVendaAtacado != null && qtdeVendaAtacado.Count() > 0)
                                    _labQtdeVendaAtacado.Text = qtdeVendaAtacado.Sum(p => p.QTDE_ATACADO).Value.ToString();
                                else
                                    _labQtdeVendaAtacado.Text = "0";

                            }

                            //Popular GRID VIEW FILHO
                            if (_liberacaoProduto.FOTO != null && _liberacaoProduto.FOTO.Trim() != "")
                            {
                                GridView gvFoto = e.Row.FindControl("gvFoto") as GridView;
                                if (gvFoto != null)
                                {
                                    List<DESENV_PRODUTO> _fotoProduto = new List<DESENV_PRODUTO>();
                                    _fotoProduto.Add(new DESENV_PRODUTO { CODIGO = _liberacaoProduto.CODIGO_PRODUTO, FOTO = _liberacaoProduto.FOTO, FOTO2 = _liberacaoProduto.FOTO2 });
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
        }
        protected void gvProdutos_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvProdutos.FooterRow;
            if (footer != null)
            {
                footer.Cells[2].Text = "Total";
                footer.Cells[7].Text = qtdeTotalConsumo.ToString("###,###,###,##0.000");
                footer.Cells[9].Text = qtdeTotalOriginal.ToString("###,###,###,##0");
                footer.Cells[10].Text = qtdeTotalLiberada.ToString("###,###,###,##0");
            }
        }
        protected void gvProdutos_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<SP_OBTER_LIBERACAO_PRODUTOResult> listaLiberacaoProduto = ObterLiberacaoProduto();

            if (ddlTipo.SelectedValue != "")
                listaLiberacaoProduto = listaLiberacaoProduto.Where(p => p.TIPO.Substring(0, 1) == ddlTipo.SelectedValue).ToList();

            if (ddlModeloFiltro.SelectedValue.Trim() != "")
                listaLiberacaoProduto = listaLiberacaoProduto.Where(p => p.MODELO.Trim() == ddlModeloFiltro.SelectedValue.Trim()).ToList();

            if (ddlNomeFiltro.SelectedValue.Trim() != "")
                listaLiberacaoProduto = listaLiberacaoProduto.Where(p => p.DESC_PRODUTO.Trim() == ddlNomeFiltro.SelectedValue.Trim()).ToList();

            if (ddlStatusFiltro.SelectedValue.Trim() != "")
            {
                if (ddlStatusFiltro.SelectedValue.Trim() == "V") //VAZIO
                    listaLiberacaoProduto = listaLiberacaoProduto.Where(p => p.STATUS == null).ToList();
                else
                    listaLiberacaoProduto = listaLiberacaoProduto.Where(p => p.STATUS.ToString() == ddlStatusFiltro.SelectedValue.Trim()).ToList();
            }

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            listaLiberacaoProduto = listaLiberacaoProduto.OrderBy(e.SortExpression + sortDirection);
            gvProdutos.DataSource = listaLiberacaoProduto;
            gvProdutos.DataBind();
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
        protected void ibtPesquisar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (!ValidarCampos())
                {
                    labErro.Text = "Preencha corretamento os campos em Vermelho.";
                    return;
                }

                //Limpar cabecalho
                Utils.WebControls.GetBoundFieldIndexByName(gvProdutos, " - >>");
                Utils.WebControls.GetBoundFieldIndexByName(gvProdutos, " - <<");

                CarregarProdutos("", "", "", false);
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void ddlFiltro_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                CarregarProdutos(ddlModeloFiltro.SelectedValue, ddlNomeFiltro.SelectedValue, ddlStatusFiltro.SelectedValue, true);
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void btLiberarProduto_Click(object sender, EventArgs e)
        {
            try
            {
                ImageButton _bt = (ImageButton)sender;
                if (_bt != null)
                {
                    labErro.Text = "";
                    GridViewRow row = (GridViewRow)_bt.NamingContainer;
                    if (row != null)
                    {

                        string tipo = "";
                        string statusAtual = "";
                        string codigoProduto = "";
                        int qtdeAtual = 0;
                        int qtdeNova = 0;

                        codigoProduto = gvProdutos.DataKeys[row.RowIndex].Value.ToString();
                        tipo = ((Label)row.FindControl("labTipo")).Text.Substring(0, 1);
                        statusAtual = ((HiddenField)row.FindControl("hidStatusAtual")).Value.ToString();
                        qtdeAtual = Convert.ToInt32(((HiddenField)row.FindControl("hidQtdeAtual")).Value.ToString());
                        qtdeNova = Convert.ToInt32(((TextBox)row.FindControl("txtQtde")).Text);

                        DropDownList _ddlLiberacao = row.FindControl("ddlLiberacao") as DropDownList;

                        /*if (qtdeNova > qtdeAtual)
                        {
                            labErro.Text = "Liberação de Produtos não pode ter a quantidade maior que a Planejada.";
                            return;
                        }*/

                        List<DESENV_PRODUTO_PRODUCAO> _produtoProducao = desenvController.ObterProdutoProducao(Convert.ToInt32(codigoProduto)).Where(p => p.TIPO.ToString() == tipo &&
                                                                                                                                                        ((p.STATUS == null) ? "" : p.STATUS.ToString()) == statusAtual).ToList();
                        if (_produtoProducao != null && _produtoProducao.Count > 0)
                        {

                            DateTime? dtLiberacao = null;
                            int? iUsuario = null;
                            char? statusNovo = null;

                            if (_ddlLiberacao.SelectedValue != "")
                            {
                                statusNovo = Convert.ToChar(_ddlLiberacao.SelectedValue);
                                /*
                                //validar se existe tecido em estoque para reservar ou liberar
                                Label _labConsumo = row.FindControl("labConsumo") as Label;
                                decimal _consumoAtual = 0;
                                decimal _consumoNovo = 0;
                                if (_labConsumo != null && _labConsumo.Text.Trim() != "")
                                {
                                    _consumoAtual = Convert.ToDecimal(_labConsumo.Text);//Obter Consumo Atual
                                    _consumoNovo = ((Convert.ToDecimal(_labConsumo.Text) / qtdeAtual) * qtdeNova); //Consumo Novo
                                }

                                decimal _saldo = 0;
                                if (labQtdeSaldo.Text.Trim() != "")
                                    _saldo = Convert.ToDecimal(labQtdeSaldo.Text.Trim()) + _consumoAtual;

                                if (_consumoNovo > _saldo)
                                {
                                    if ((statusNovo == 'L' || statusNovo == 'P') && (statusAtual == "" || statusAtual == "R"))
                                    {
                                        labErro.Text = "Você não possui este Tecido em Estoque para Liberar ou Reservar o Produto.";
                                        _ddlLiberacao.SelectedValue = statusAtual;
                                        return;
                                    }
                                }*/

                            }
                            else
                                statusNovo = null;

                            //Controle de qtde da produção conforme status
                            int qtdeDiferenca = qtdeNova - qtdeAtual;

                            if (qtdeDiferenca != 0)
                            {
                                if (statusNovo == 'L' || statusNovo == 'R')
                                {
                                    AtualizarQtdeProducao(codigoProduto, tipo, statusNovo.ToString(), qtdeDiferenca);
                                }
                                else if (statusNovo == 'P')
                                {
                                    AtualizarQtdeProducao(codigoProduto, tipo, statusAtual, qtdeDiferenca * (-1));
                                    AtualizarQtdeProducao(codigoProduto, tipo, "L", qtdeDiferenca);
                                    statusNovo = 'L';

                                    dtLiberacao = DateTime.Now;
                                    iUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                                }
                            }

                            if (_ddlLiberacao.SelectedValue == "L")
                            {
                                dtLiberacao = DateTime.Now;
                                iUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

                                //ATUALIZAR TRIPA SE O STATUS FOR LIBERADO
                                var _produto = desenvController.ObterProduto(Convert.ToInt32(codigoProduto));
                                int qtdeTotalTipoProduto = ObterQtdeTotalTipoProduto(_produto, tipo);
                                if (tipo == "M")
                                {
                                    _produto.QTDE_MOSTRUARIO = qtdeTotalTipoProduto;
                                }
                                if (tipo == "V")
                                {
                                    _produto.QTDE = qtdeTotalTipoProduto;
                                    if (_produto.QTDE <= 0)
                                        _produto.CORTE_VAREJO = "";
                                }
                                if (tipo == "A")
                                {
                                    _produto.QTDE_ATACADO = qtdeTotalTipoProduto;
                                    if (_produto.QTDE_ATACADO <= 0)
                                        _produto.CORTE_ATACADO = "";
                                }

                                desenvController.AtualizarProduto(_produto);
                            }

                            //Atualizar principal - produto e tipo
                            DESENV_PRODUTO_PRODUCAO pAux = null;
                            foreach (var pp in _produtoProducao)
                            {
                                pAux = new DESENV_PRODUTO_PRODUCAO();
                                pAux = desenvController.ObterProdutoProducaoCodigo(pp.CODIGO);
                                pAux.STATUS = statusNovo;
                                pAux.DATA_LIBERACAO = dtLiberacao;
                                pAux.USUARIO_LIBERACAO = iUsuario;
                                desenvController.AtualizarProdutoProducao(pAux);
                            }

                            //recarregar grid
                            CarregarProdutos(ddlModeloFiltro.SelectedValue, ddlNomeFiltro.SelectedValue, ddlStatusFiltro.SelectedValue, true);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        private void CarregarQtdeTecido(List<SP_OBTER_LIBERACAO_PRODUTOResult> _liberacaoProduto)
        {
            labQtdeEstoque.Text = "0,000";
            labQtdeLiberado.Text = "0,000";
            labQtdeReservada.Text = "0,000";
            labQtdeSaldo.Text = "0,000";

            var _saldoEstoque = new List<SP_OBTER_MATERIAL_SALDOResult>();
            _saldoEstoque = desenvController.ObterMaterialSaldo(ddlMaterialGrupo.SelectedItem.Text.Trim(), ddlMaterialSubGrupo.SelectedItem.Text.Trim(), ddlCor.SelectedValue.Trim(), ddlCorFornecedor.SelectedValue.Trim(), "", 1);
            qtdeEstoque = _saldoEstoque.Sum(p => p.QTDE_ESTOQUE);

            labQtdeEstoque.Text = qtdeEstoque.ToString("###,###,###,##0.000");

            consumoLiberada = _liberacaoProduto.Where(p => (p.STATUS == 'L' || p.STATUS == 'P') && p.PROD_HB == null).Sum(x => x.CONSUMO).Value;
            consumoReservada = _liberacaoProduto.Where(p => p.STATUS == 'R').Sum(x => x.CONSUMO).Value;

            labQtdeLiberado.Text = consumoLiberada.ToString("###,###,###,##0.000");
            labQtdeReservada.Text = consumoReservada.ToString("###,###,###,##0.000");

            consumoSaldo = ((qtdeEstoque) - (consumoLiberada + consumoReservada));
            labQtdeSaldo.Text = consumoSaldo.ToString("###,###,###,##0.000");
        }
        private void AtualizarQtdeProducao(string codigoProduto, string tipo, string status, int qtdeDiferenca)
        {
            var _produto = desenvController.ObterProduto(Convert.ToInt32(codigoProduto));
            //ZERAR QTDE PARA NAO ATUALIZAR
            _produto.QTDE_MOSTRUARIO = 0;
            _produto.QTDE = 0;
            _produto.QTDE_ATACADO = 0;
            if (tipo == "M")
                _produto.QTDE_MOSTRUARIO = qtdeDiferenca;
            if (tipo == "V")
                _produto.QTDE = qtdeDiferenca;
            if (tipo == "A")
                _produto.QTDE_ATACADO = qtdeDiferenca;

            InserirQtdeProdutoProducao(_produto, true, status);
        }
        private void InserirQtdeProdutoProducao(DESENV_PRODUTO _produto, bool _edit, string status)
        {
            List<DESENV_PRODUTO_FICTEC> _fichaTecnica = null;
            _fichaTecnica = desenvController.ObterFichaTecnica(_produto.CODIGO);

            DESENV_PRODUTO_PRODUCAO _produtoProducao = null;
            int codigoProdutoProducao = 0;
            int usuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

            string msg = "ALTERADA";
            if (!_edit)
                msg = "EXCLUÍDA";

            foreach (DESENV_PRODUTO_FICTEC ft in _fichaTecnica)
            {
                if (_produto.QTDE_MOSTRUARIO != 0)
                {
                    _produtoProducao = new DESENV_PRODUTO_PRODUCAO();
                    _produtoProducao.DESENV_PRODUTO = ft.DESENV_PRODUTO;
                    _produtoProducao.PROD_DETALHE = ft.PROD_DETALHE;
                    _produtoProducao.TIPO = 'M';
                    _produtoProducao.QTDE = (_produto.QTDE_MOSTRUARIO == null) ? 0 : Convert.ToInt32(_produto.QTDE_MOSTRUARIO);
                    _produtoProducao.DATA_INCLUSAO = DateTime.Now;
                    _produtoProducao.USUARIO_INCLUSAO = usuario;
                    if (status != "")
                    {
                        _produtoProducao.STATUS = Convert.ToChar(status);
                        if (status == "L")
                        {
                            _produtoProducao.USUARIO_LIBERACAO = usuario;
                            _produtoProducao.DATA_LIBERACAO = DateTime.Now;
                        }
                    }
                    codigoProdutoProducao = desenvController.InserirProdutoProducao(_produtoProducao);
                    //Gerar Notificacao de Aprovacao e necessidade de preencher FICHA TECNICA
                    notificacao_envio.GerarNotificacao(ft.CODIGO, "DESENV_PRODUTO_FICTEC", 2, ("PRODUTO: " + _produto.MODELO + " - QTDE: " + _produtoProducao.QTDE.ToString() + " PEÇAS MOSTRUÁRIO - " + msg));
                }

                if (_produto.QTDE != 0)
                {
                    _produtoProducao = new DESENV_PRODUTO_PRODUCAO();
                    _produtoProducao.DESENV_PRODUTO = ft.DESENV_PRODUTO;
                    _produtoProducao.PROD_DETALHE = ft.PROD_DETALHE;
                    _produtoProducao.TIPO = 'V';
                    _produtoProducao.QTDE = Convert.ToInt32(_produto.QTDE);
                    _produtoProducao.DATA_INCLUSAO = DateTime.Now;
                    _produtoProducao.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                    if (status != "")
                    {
                        _produtoProducao.STATUS = Convert.ToChar(status);
                        if (status == "L")
                        {
                            _produtoProducao.USUARIO_LIBERACAO = usuario;
                            _produtoProducao.DATA_LIBERACAO = DateTime.Now;
                        }
                    }
                    codigoProdutoProducao = desenvController.InserirProdutoProducao(_produtoProducao);
                    //Gerar Notificacao de Aprovacao e necessidade de preencher FICHA TECNICA
                    notificacao_envio.GerarNotificacao(ft.CODIGO, "DESENV_PRODUTO_FICTEC", 2, ("PRODUTO: " + _produto.MODELO + " - QTDE: " + _produtoProducao.QTDE.ToString() + " PEÇAS VAREJO - " + msg));
                }

                if (_produto.QTDE_ATACADO != 0)
                {
                    _produtoProducao = new DESENV_PRODUTO_PRODUCAO();
                    _produtoProducao.DESENV_PRODUTO = ft.DESENV_PRODUTO;
                    _produtoProducao.PROD_DETALHE = ft.PROD_DETALHE;
                    _produtoProducao.TIPO = 'A';
                    _produtoProducao.QTDE = Convert.ToInt32(_produto.QTDE_ATACADO);
                    _produtoProducao.DATA_INCLUSAO = DateTime.Now;
                    _produtoProducao.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                    if (status != "")
                    {
                        _produtoProducao.STATUS = Convert.ToChar(status);
                        if (status == "L")
                        {
                            _produtoProducao.USUARIO_LIBERACAO = usuario;
                            _produtoProducao.DATA_LIBERACAO = DateTime.Now;
                        }
                    }
                    codigoProdutoProducao = desenvController.InserirProdutoProducao(_produtoProducao);
                    //Gerar Notificacao de Aprovacao e necessidade de preencher FICHA TECNICA
                    notificacao_envio.GerarNotificacao(ft.CODIGO, "DESENV_PRODUTO_FICTEC", 2, ("PRODUTO: " + _produto.MODELO + " - QTDE: " + _produtoProducao.QTDE.ToString() + " PEÇAS ATACADO - " + msg));
                }
            }
        }
        private int ObterQtdeTotalTipoProduto(DESENV_PRODUTO _produto, string tipo)
        {
            int _qtde = 0;

            var _produtoProducao = desenvController.ObterProdutoProducao(_produto.CODIGO).Where(p => p.PROD_DETALHE == null && p.TIPO.ToString() == tipo);
            if (_produtoProducao != null && _produtoProducao.Count() > 0)
                _qtde = _produtoProducao.Sum(x => x.QTDE);

            return _qtde;
        }
        private bool ValidarQtdeProducao(int desenv_produto, int qtde, string tipo, char? status)
        {
            bool retorno = true;

            var _produtoProducao = desenvController.ObterProdutoProducao(desenv_produto);
            if (_produtoProducao != null && _produtoProducao.Count() > 0)
            {
                var _producaoQtde = _produtoProducao.Where(p => p.TIPO.ToString() == tipo && p.PROD_DETALHE == null && p.STATUS == status).ToList().Sum(x => x.QTDE);
                if (_producaoQtde > 0)
                    if (qtde < _producaoQtde)
                        retorno = false;
            }

            return retorno;
        }
        #endregion

        #region "VALIDACAO"
        private bool ValidarCampos()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            labGrupo.ForeColor = _OK;
            if (ddlMaterialGrupo.SelectedValue.Trim() == "")
            {
                labGrupo.ForeColor = _notOK;
                retorno = false;
            }

            labSubGrupo.ForeColor = _OK;
            if (ddlMaterialSubGrupo.SelectedValue.Trim() == "")
            {
                labSubGrupo.ForeColor = _notOK;
                retorno = false;
            }

            labCor.ForeColor = _OK;
            if (ddlCor.SelectedValue.Trim() == "")
            {
                labCor.ForeColor = _notOK;
                retorno = false;
            }

            labCorFornecedor.ForeColor = _OK;
            if (ddlCorFornecedor.SelectedValue.Trim() == "Selecione")
            {
                labCorFornecedor.ForeColor = _notOK;
                retorno = false;
            }

            return retorno;
        }
        private void HabilitarCampos(bool enable)
        {

        }
        private void LimparCampos(bool _cabecalho)
        {
            labErro.Text = "";

        }
        #endregion
    }
}
