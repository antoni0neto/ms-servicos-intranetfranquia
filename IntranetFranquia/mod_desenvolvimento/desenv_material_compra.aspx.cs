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
using System.Text;
using System.Text.RegularExpressions;

namespace Relatorios.mod_desenvolvimento
{
    public partial class desenv_material_compra : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();
        NotificacaoController notController = new NotificacaoController();

        int colunaNotificacao = 0, colunaPedido = 0, colunaEntrada = 0, colunaProduto = 0, colunaEstoque = 0, colunaPedidoProduto = 0, colunaPedidoNota = 0;
        //PRODUTOS
        decimal qtdeTotal = 0;

        //ENTRADAS, RESERVAS E PEDIDOS
        decimal qtdeEntradaIni, qtdeEntradaEntregue, qtdePedidoIni, qtdePedidoEntregue = 0;
        decimal qtdePedidoProdutoIni, qtdePedidoProdutoEntregue = 0;
        decimal qtdePedidoProdutoFalt, qtdeEntradaFalt = 0;

        //ESTOQUE
        decimal qtdeEstoque = 0, qtdeEstoqueEntregue = 0, qtdeEstoqueConsumida = 0;

        decimal qtdeSubPedido = 0;

        List<DESENV_PEDIDO> g_PedidoFiltro = new List<DESENV_PEDIDO>();

        protected void Page_Load(object sender, EventArgs e)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtPedidoData.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtPedidoPrevEntrega.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtPedidoDataReserva.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);

            if (!Page.IsPostBack)
            {
                CarregarColecoes();
                CarregarGrupos();
                CarregarSubGrupos("");
                //CarregarCores("", "");
                CarregarCorFornecedor("", "", "ddlCorFornecedor");
                CarregarFornecedores();
                CarregarFormaPgto();
                CarregarUnidadeMedida();
                //CarregarNotificacao();

                ddlCondicaoPgto_SelectedIndexChanged(null, null);
                chkEmail_CheckedChanged(chkEmail, null);

                btAbrirSubPedido.Visible = false;

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

                rdbFiltroTexto_CheckedChanged(rdbFiltroDDL, null);

            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#tabs').tabs(); });", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$('#tabs').tabs('option', 'active', '" + ((hidTabSelected.Value == "") ? "0" : hidTabSelected.Value) + "');", true);

            btSalvarPedido.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btSalvarPedido, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarColecoes()
        {
            BaseController _base = new BaseController();
            List<COLECOE> _colecoes = _base.BuscaColecoes();
            if (_colecoes != null)
            {
                _colecoes.Insert(0, new COLECOE { COLECAO = "", DESC_COLECAO = "" });
                ddlColecoes.DataSource = _colecoes;
                ddlColecoes.DataBind();

                //if (Session["COLECAO"] != null)
                //  ddlColecoes.SelectedValue = Session["COLECAO"].ToString();
            }
        }
        private void CarregarFormaPgto()
        {
            List<FORMA_PGTO> _formaPGTO = (new BaseController().ObterFormaPgto());

            _formaPGTO.Insert(0, new FORMA_PGTO { CONDICAO_PGTO = "", DESC_COND_PGTO = "" });

            if (_formaPGTO != null)
            {
                ddlCondicaoPgto.DataSource = _formaPGTO;
                ddlCondicaoPgto.DataBind();
            }
        }
        private void CarregarCores(string grupo, string subGrupo)
        {
            //Obter cores do fornecedor da ficha tecnica
            var _cor = desenvController.ObterFichaTecnica().Where(p => p.GRUPO.Trim() == grupo.Trim() &&
                                    p.SUBGRUPO.Trim() == subGrupo.Trim() &&
                                    p.COR_MATERIAL != null && p.COR_MATERIAL.Trim() != "").ToList();
            var cf1 = _cor.GroupBy(p => new { COR_MATERIAL = p.COR_MATERIAL.Trim() }).Select(k => new DESENV_PRODUTO_FICTEC { COR_MATERIAL = k.Key.COR_MATERIAL }).ToList();

            //Obter cores do fornecedor dos pedidos
            var _corPedido = desenvController.ObterPedido1000().Where(p => p.GRUPO.Trim() == grupo.Trim() &&
                                            p.SUBGRUPO.Trim() == subGrupo.Trim() &&
                                            p.COR != null && p.COR.Trim() != "").ToList();
            var cf2 = _corPedido.GroupBy(p => new { COR = p.COR.Trim() }).Select(k => new DESENV_PRODUTO_FICTEC { COR_MATERIAL = k.Key.COR }).ToList();

            //AUXILIAR para juntar as duas lista
            List<DESENV_PRODUTO_FICTEC> cF = new List<DESENV_PRODUTO_FICTEC>();
            cF.AddRange(cf1);
            cF.AddRange(cf2);

            //Separar duplicatas
            cF = cF.GroupBy(p => new { COR_MATERIAL = p.COR_MATERIAL.Trim() }).Select(k => new DESENV_PRODUTO_FICTEC { COR_MATERIAL = k.Key.COR_MATERIAL }).ToList();

            var _coresBasicas = ObterCoresBasicas();

            //Carregar cores do pedido 
            var _coresPedido = _coresBasicas;
            _coresPedido.Insert(0, new CORES_BASICA { COR = "", DESC_COR = "" });
            ddlCorPedido.DataSource = _coresPedido;
            ddlCorPedido.DataBind();

            //Depois filtrar cores para carregar os filtros
            _coresBasicas = _coresBasicas.Where(p => cF.Any(x => x.COR_MATERIAL.Trim() == p.COR.Trim())).ToList();
            _coresBasicas.Insert(0, new CORES_BASICA { COR = "", DESC_COR = "" });

            ddlCor.DataSource = _coresBasicas;
            ddlCor.DataBind();

            if (_coresBasicas.Count == 2)
                ddlCor.SelectedIndex = 1;

        }
        private void CarregarCoresPedido(string grupo, string subGrupo)
        {
            List<CORES_BASICA> coresBasicas = new List<CORES_BASICA>();

            List<MATERIAI> filtroMaterial = new List<MATERIAI>();
            filtroMaterial = desenvController.ObterMaterial().Where(p => p.GRUPO.Trim() == grupo.Trim() && p.SUBGRUPO.Trim() == subGrupo.Trim()).ToList();

            List<MATERIAIS_CORE> materialCores = desenvController.ObterMaterialCor();
            materialCores = materialCores.Where(i => filtroMaterial.Any(g => g.MATERIAL.Trim() == i.MATERIAL.Trim())).OrderBy(p => p.DESC_COR_MATERIAL).ToList();

            CORES_BASICA _corBasica = null;
            foreach (MATERIAIS_CORE c in materialCores)
            {
                _corBasica = new CORES_BASICA();
                _corBasica.COR = c.COR_MATERIAL;
                _corBasica.DESC_COR = c.DESC_COR_MATERIAL.Trim();
                coresBasicas.Add(_corBasica);
            }

            coresBasicas = coresBasicas.GroupBy(p => new { COR = p.COR.Trim(), DESC_COR = p.DESC_COR.Trim() }).Select(k => new CORES_BASICA { COR = k.Key.COR, DESC_COR = k.Key.DESC_COR }).ToList();

            coresBasicas = coresBasicas.OrderBy(p => p.DESC_COR.Trim()).ToList();
            coresBasicas.Insert(0, new CORES_BASICA { COR = "", DESC_COR = "" });

            ddlCorPedido.DataSource = coresBasicas;
            ddlCorPedido.DataBind();

            if (coresBasicas.Count == 2)
                ddlCorPedido.SelectedIndex = 1;

        }
        private void CarregarFornecedores()
        {
            List<PROD_FORNECEDOR> _fornecedor = prodController.ObterFornecedor().Where(p => p.STATUS == 'A' && p.TIPO != 'S').ToList();
            if (_fornecedor != null)
            {
                var _fornecedorAux = _fornecedor.Where(p => p.FORNECEDOR != null).Select(s => s.FORNECEDOR.Trim()).Distinct().ToList();

                _fornecedor = new List<PROD_FORNECEDOR>();
                foreach (var item in _fornecedorAux)
                    if (item.Trim() != "")
                        _fornecedor.Add(new PROD_FORNECEDOR { FORNECEDOR = item.Trim() });

                _fornecedor.OrderBy(p => p.FORNECEDOR);
                _fornecedor.Insert(0, new PROD_FORNECEDOR { FORNECEDOR = "" });
                ddlFornecedor.DataSource = _fornecedor;
                ddlFornecedor.DataBind();

                ddlPedidoFornecedor.DataSource = _fornecedor;
                ddlPedidoFornecedor.DataBind();

            }
        }
        private void CarregarDetalhes()
        {
            List<PROD_DETALHE> detalhes = prodController.ObterDetalhes().OrderBy(p => p.DESCRICAO).ToList();

            detalhes.Insert(0, new PROD_DETALHE { CODIGO = 99, DESCRICAO = "PRINCIPAL" });
            //ddlDetalhe.DataSource = detalhes;
            //ddlDetalhe.DataBind();
        }
        private void CarregarGrupos()
        {
            List<MATERIAIS_GRUPO> _matGrupo = desenvController.ObterMaterialGrupo();
            if (_matGrupo != null)
            {
                _matGrupo.Insert(0, new MATERIAIS_GRUPO { CODIGO_GRUPO = "", GRUPO = "" });
                ddlMaterialGrupo.DataSource = _matGrupo;
                ddlMaterialGrupo.DataBind();

                ddlMaterialGrupoPedido.DataSource = _matGrupo;
                ddlMaterialGrupoPedido.DataBind();
            }
        }
        protected void ddlMaterialGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                DropDownList ddl = (DropDownList)sender;
                if (ddl.ID == "ddlMaterialGrupo")
                {
                    hidLimpaFiltro.Value = "";
                    CarregarSubGrupos(ddlMaterialGrupo.SelectedItem.Text.Trim());
                }
                else
                    CarregarSubGrupos(ddlMaterialGrupoPedido.SelectedItem.Text.Trim());

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        private void CarregarSubGrupos(string grupo)
        {
            List<MATERIAIS_SUBGRUPO> _matSubGrupo = null;
            if (grupo.Trim() != "")
                _matSubGrupo = desenvController.ObterMaterialSubGrupo().Where(p => p.GRUPO.Trim() == grupo.Trim()).ToList();
            else
                _matSubGrupo = desenvController.ObterMaterialSubGrupo();

            if (_matSubGrupo != null)
            {
                _matSubGrupo = _matSubGrupo.OrderBy(p => p.SUBGRUPO.Trim()).ToList();
                _matSubGrupo.Insert(0, new MATERIAIS_SUBGRUPO { CODIGO_SUBGRUPO = "", SUBGRUPO = "" });
                if (hidLimpaFiltro.Value == "")
                {
                    ddlMaterialSubGrupo.DataSource = _matSubGrupo;
                    ddlMaterialSubGrupo.DataBind();
                }
                ddlMaterialSubGrupoPedido.DataSource = _matSubGrupo;
                ddlMaterialSubGrupoPedido.DataBind();

            }
        }
        protected void ddlMaterialSubGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                DropDownList ddl = (DropDownList)sender;
                if (ddl.ID == "ddlMaterialSubGrupo")
                {
                    CarregarCores(ddlMaterialGrupo.SelectedItem.Text.Trim(), ddlMaterialSubGrupo.SelectedItem.Text.Trim());
                    CarregarCorFornecedor(ddlMaterialGrupo.SelectedItem.Text.Trim(), ddlMaterialSubGrupo.SelectedItem.Text.Trim(), "ddlCorFornecedor");
                }
                else
                {
                    CarregarCoresPedido(ddlMaterialGrupoPedido.SelectedItem.Text.Trim(), ddlMaterialSubGrupoPedido.SelectedItem.Text.Trim());
                    CarregarCorFornecedor(ddlMaterialGrupoPedido.SelectedItem.Text.Trim(), ddlMaterialSubGrupoPedido.SelectedItem.Text.Trim(), "ddlCorFornecedorPedido");
                }
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }

        }
        private void CarregarCorFornecedor(string grupo, string subGrupo, string objeto)
        {

            if (objeto == "ddlCorFornecedor")
            {
                //Obter cores do fornecedor da ficha tecnica
                List<DESENV_PRODUTO_FICTEC> _corFornecedor = new List<DESENV_PRODUTO_FICTEC>();
                _corFornecedor = desenvController.ObterFichaTecnica().Where(p => p.COR_FORNECEDOR != null && p.COR_FORNECEDOR.Trim() != "" && p.GRUPO.Trim() == grupo && p.SUBGRUPO.Trim() == subGrupo).ToList();
                var cf1 = _corFornecedor.GroupBy(p => new { COR_FORNECEDOR = p.COR_FORNECEDOR.Trim() }).Select(k => new DESENV_PRODUTO_FICTEC { COR_FORNECEDOR = k.Key.COR_FORNECEDOR }).ToList();

                //Obter cores do fornecedor dos pedidos
                List<DESENV_PEDIDO> _corFornecedorPedido = new List<DESENV_PEDIDO>();
                _corFornecedorPedido = desenvController.ObterPedido1000().Where(p => p.COR_FORNECEDOR != null && p.COR_FORNECEDOR.Trim() != "" && p.GRUPO.Trim() == grupo && p.SUBGRUPO.Trim() == subGrupo).ToList();
                var cf2 = _corFornecedorPedido.GroupBy(p => new { COR_FORNECEDOR = p.COR_FORNECEDOR.Trim() }).Select(k => new DESENV_PRODUTO_FICTEC { COR_FORNECEDOR = k.Key.COR_FORNECEDOR }).ToList();

                //AUXILIAR para juntar as duas lista
                List<DESENV_PRODUTO_FICTEC> cF = new List<DESENV_PRODUTO_FICTEC>();
                cF.AddRange(cf1);
                cF.AddRange(cf2);

                //Separar duplicatas
                cF = cF.GroupBy(p => new { COR_FORNECEDOR = p.COR_FORNECEDOR.Trim() }).Select(k => new DESENV_PRODUTO_FICTEC { COR_FORNECEDOR = k.Key.COR_FORNECEDOR }).OrderBy(p => p.COR_FORNECEDOR).ToList();

                cF.Insert(0, new DESENV_PRODUTO_FICTEC { COR_FORNECEDOR = "" });
                if (hidLimpaFiltro.Value == "")
                {
                    ddlCorFornecedor.DataSource = cF;
                    ddlCorFornecedor.DataBind();
                }
            }
            else
            {
                List<MATERIAI> filtroMaterial = new List<MATERIAI>();
                filtroMaterial = desenvController.ObterMaterial().Where(p => p.GRUPO.Trim() == grupo.Trim() && p.SUBGRUPO.Trim() == subGrupo.Trim()).ToList();

                List<MATERIAIS_CORE> materialCores = desenvController.ObterMaterialCor();
                materialCores = materialCores.Where(i => filtroMaterial.Any(g => g.MATERIAL.Trim() == i.MATERIAL.Trim())).OrderBy(p => p.DESC_COR_MATERIAL).ToList();

                materialCores.Insert(0, new MATERIAIS_CORE { REFER_FABRICANTE = "" });
                ddlCorFornecedorPedido.DataSource = materialCores.OrderBy(p => p.REFER_FABRICANTE);
                ddlCorFornecedorPedido.DataBind();
            }

        }
        private void CarregarUnidadeMedida()
        {
            List<UNIDADE_MEDIDA> _unidadeMedida = (new ProducaoController().ObterUnidadeMedida());

            _unidadeMedida.Add(new UNIDADE_MEDIDA { CODIGO = 0, DESCRICAO = "", STATUS = 'A' });

            _unidadeMedida = _unidadeMedida.OrderBy(l => l.CODIGO).ToList();

            if (_unidadeMedida != null)
            {
                ddlUnidade.DataSource = _unidadeMedida;
                ddlUnidade.DataBind();
            }
        }

        private List<CORES_BASICA> ObterCoresBasicas()
        {
            List<CORES_BASICA> _cores = new List<CORES_BASICA>();
            _cores = prodController.ObterCoresBasicas();
            return _cores;
        }
        #endregion

        #region "AÇÕES DA TELA"
        private void MoverAba(string vAba)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$('#tabs').tabs('option', 'active', '" + vAba + "');", true);
            hidTabSelected.Value = vAba;
        }
        protected void rdbFiltroTexto_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rdb = (RadioButton)sender;
            if (rdb != null)
            {
                if (rdb.ID == "rdbFiltroDDL")
                {
                    txtGrupoFiltro.Enabled = false;
                    txtGrupoFiltro.Text = "";
                    txtSubGrupoFiltro.Enabled = false;
                    txtSubGrupoFiltro.Text = "";

                    ddlMaterialGrupo.Enabled = true;
                    ddlMaterialSubGrupo.Enabled = true;
                    ddlCor.Enabled = true;
                    ddlCorFornecedor.Enabled = true;
                    ddlFornecedor.Enabled = true;
                }
                else
                {
                    ddlMaterialGrupo.Enabled = false;
                    ddlMaterialGrupo.SelectedValue = "";
                    ddlMaterialSubGrupo.Enabled = false;
                    ddlMaterialSubGrupo.SelectedValue = "";
                    ddlCor.Enabled = false;
                    ddlCor.SelectedValue = "";
                    ddlCorFornecedor.Enabled = false;
                    ddlCorFornecedor.SelectedValue = "";
                    ddlFornecedor.Enabled = false;
                    ddlFornecedor.SelectedValue = "";

                    txtGrupoFiltro.Enabled = true;
                    txtSubGrupoFiltro.Enabled = true;
                }

            }
        }
        #endregion

        #region "PRODUTOS"
        private void CarregarProdutos()
        {
            List<SP_OBTER_MATERIAL_PRODUTOResult> _materialProduto = new List<SP_OBTER_MATERIAL_PRODUTOResult>();

            string grupo = "";
            string subGrupo = "";
            string cor = "";
            string corFornecedor = "";
            string fornecedor = "";

            if (rdbFiltroDDL.Checked)
            {
                grupo = ddlMaterialGrupo.SelectedItem.Text;
                subGrupo = ddlMaterialSubGrupo.SelectedItem.Text;
                cor = ddlCor.SelectedValue;
                corFornecedor = ddlCorFornecedor.SelectedValue;
                fornecedor = ddlFornecedor.SelectedValue;
            }
            else
            {
                grupo = txtGrupoFiltro.Text.Trim().ToUpper();
                subGrupo = txtSubGrupoFiltro.Text.Trim().ToUpper();
                cor = "";
                corFornecedor = "";
                fornecedor = "";
            }

            _materialProduto = desenvController.ObterMaterialProduto(
                txtModelo.Text,
                "",
                txtNome.Text,
                grupo,
                subGrupo,
                fornecedor,
                cor,
                corFornecedor);

            //Soh recarrega filtros quando clicar em pesquisar
            /*if (ddlCorFornecedorFiltro.SelectedValue.Trim() == "" && ddlTipoFiltro.SelectedValue.Trim() == "")
                CarregarFiltros(_materialProduto);
            else
            {
                if (ddlCorFornecedorFiltro.SelectedValue != "")
                    _materialProduto = _materialProduto.Where(p => p.COR_FORNECEDOR.Trim() == ddlCorFornecedorFiltro.SelectedValue.Trim()).ToList();

                if (ddlTipoFiltro.SelectedValue != "")
                    _materialProduto = _materialProduto.Where(p => p.TIPO.Trim() == ddlTipoFiltro.SelectedValue.Trim()).ToList();
            }*/

            labErro.Text = "";
            btGerarPedido.Visible = false;
            if (_materialProduto != null && _materialProduto.Count > 0)
                btGerarPedido.Visible = true;
            else
                labErro.Text = "Nenhum Produto encontrado. Refaça sua pesquisa.";

            gvProdutos.DataSource = _materialProduto.OrderBy(p => p.GRUPO).ThenBy(k => k.SUBGRUPO).ThenBy(k => k.DESC_COR_MATERIAL).ThenBy(k => k.COR_FORNECEDOR).ThenBy(k => k.MODELO);
            gvProdutos.DataBind();
        }
        protected void gvProdutos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_MATERIAL_PRODUTOResult _produto = e.Row.DataItem as SP_OBTER_MATERIAL_PRODUTOResult;

                    colunaProduto += 1;
                    if (_produto != null)
                    {

                        Literal _litColuna = e.Row.FindControl("litColuna") as Literal;
                        if (_litColuna != null)
                            _litColuna.Text = colunaProduto.ToString();

                        if (_produto.CODIGO > 0)
                        {

                            Label _labFornecedor = e.Row.FindControl("labFornecedor") as Label;
                            if (_labFornecedor != null)
                                _labFornecedor.Text = (_produto.FORNECEDOR == null) ? "" : _produto.FORNECEDOR.Trim().Substring(0, ((_produto.FORNECEDOR.Trim().Length < 18) ? (_produto.FORNECEDOR.Trim().Length) : 17));

                            Label _labQtde = e.Row.FindControl("labQtde") as Label;
                            if (_labQtde != null)
                            {
                                _labQtde.Text = (_produto.QTDE * _produto.CONSUMO).ToString();
                                if (_produto.QTDE < 0)
                                    _labQtde.ForeColor = Color.Red;

                                qtdeTotal += (_produto.QTDE * _produto.CONSUMO);
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
                footer.Cells[1].Text = "Total";
                footer.Cells[10].Text = qtdeTotal.ToString();
            }
        }

        protected void gvPedidoProduto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    DESENV_PEDIDO _pedido = e.Row.DataItem as DESENV_PEDIDO;

                    colunaPedidoProduto += 1;
                    if (_pedido != null)
                    {
                        Literal _litColuna = e.Row.FindControl("litColuna") as Literal;
                        if (_litColuna != null)
                            _litColuna.Text = colunaPedidoProduto.ToString();

                        if (_pedido.CODIGO > 0)
                        {
                            Label _labNumeroPedido = e.Row.FindControl("labNumeroPedido") as Label;
                            if (_labNumeroPedido != null)
                                _labNumeroPedido.Text = _pedido.NUMERO_PEDIDO.ToString();

                            Label _labFornecedor = e.Row.FindControl("labFornecedor") as Label;
                            if (_labFornecedor != null)
                                _labFornecedor.Text = (_pedido.FORNECEDOR == null) ? "" : _pedido.FORNECEDOR.Trim().Substring(0, ((_pedido.FORNECEDOR.Trim().Length < 18) ? (_pedido.FORNECEDOR.Trim().Length) : 17));

                            Label _labGrupo = e.Row.FindControl("labGrupo") as Label;
                            if (_labGrupo != null)
                                _labGrupo.Text = _pedido.GRUPO;

                            Label _labSubGrupo = e.Row.FindControl("labSubGrupo") as Label;
                            if (_labSubGrupo != null)
                                _labSubGrupo.Text = _pedido.SUBGRUPO;

                            Label _labCor = e.Row.FindControl("labCor") as Label;
                            if (_labCor != null)
                            {
                                var _cor = prodController.ObterCoresBasicas(_pedido.COR);
                                _labCor.Text = (_cor == null) ? _pedido.COR : _cor.DESC_COR;
                            }

                            Label _labCorFornecedor = e.Row.FindControl("labCorFornecedor") as Label;
                            if (_labCorFornecedor != null)
                                _labCorFornecedor.Text = _pedido.COR_FORNECEDOR;

                            Label _labQtde = e.Row.FindControl("labQtde") as Label;
                            if (_labQtde != null)
                            {
                                _labQtde.Text = _pedido.QTDE.ToString("###,###,###,##0.000");
                                qtdePedidoProdutoIni += _pedido.QTDE;
                            }

                            Label _labQtdeEntregue = e.Row.FindControl("labQtdeEntregue") as Label;
                            if (_labQtdeEntregue != null)
                            {
                                List<DESENV_PEDIDO_QTDE> _lstPedidoQtde = desenvController.ObterPedidoQtdePedido(_pedido.CODIGO);
                                var _lstTotal = _lstPedidoQtde.GroupBy(g => g.DESENV_PEDIDO).Select(i => new { QTDE = i.Sum(w => w.QTDE) }).SingleOrDefault();
                                if (_lstTotal != null)
                                {
                                    _labQtdeEntregue.Text = Convert.ToDecimal(_lstTotal.QTDE).ToString("######0.000");
                                    qtdePedidoProdutoEntregue += Convert.ToDecimal(_lstTotal.QTDE);
                                }
                                else
                                    _labQtdeEntregue.Text = "0";
                            }

                            Label _labStatus = e.Row.FindControl("labStatus") as Label;
                            if (_labStatus != null)
                            {
                                string status = "SEM STATUS";
                                if (_pedido.STATUS == 'A') status = "AGUARDANDO";
                                else if (_pedido.STATUS == 'B') status = "BAIXADO";
                                else if (_pedido.STATUS == 'R') status = "RESERVADO";
                                else if (_pedido.STATUS == 'E') status = "EXCLUÍDO";

                                _labStatus.Text = status;
                            }

                        }
                    }
                }
            }

        }
        protected void gvPedidoProduto_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvPedidoProduto.FooterRow;
            if (footer != null)
            {
                footer.Cells[1].Text = "Total";
                footer.Cells[7].Text = qtdePedidoProdutoIni.ToString();
                footer.Cells[8].Text = qtdePedidoProdutoEntregue.ToString();
            }
        }

        protected void btConfirmarCompra_Click(object sender, EventArgs e)
        {
            ImageButton iButton = (ImageButton)sender;
            if (iButton != null)
            {
                try
                {
                    int codigoProdutoProducao = Convert.ToInt32(iButton.CommandArgument);
                    if (codigoProdutoProducao > 0)
                    {
                        DESENV_PRODUTO_PRODUCAO _produtoProducao = null;
                        _produtoProducao = desenvController.ObterProdutoProducaoCodigo(codigoProdutoProducao);

                        if (_produtoProducao != null)
                        {
                            _produtoProducao.DATA_EXCLUSAO = DateTime.Now;

                            desenvController.AtualizarProdutoProducao(_produtoProducao);
                        }

                        CarregarProdutos();
                    }
                }
                catch (Exception ex)
                {
                    labErro.Text = ex.Message;
                }
            }
        }
        protected void btGerarPedido_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                labErroGerarPedido.Text = "";
                //VALIDACOES
                string msg = "";
                decimal qtde = 0;

                qtde = ValidarGeracaoPedido(out msg);
                if (qtde <= 0)
                {
                    labErro.Text = msg;
                    labErroGerarPedido.Text = msg;
                    return;
                }

                hidLimpaFiltro.Value = "N";

                txtPedidoNumero.Text = ObterUltimoPedido().ToString();
                txtPedidoData.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtQtde.Text = qtde.ToString();
                ddlColecoes.SelectedValue = "20    ";

                ddlMaterialGrupoPedido.SelectedValue = desenvController.ObterMaterialGrupo(hidMaterialGrupoPedido.Value).CODIGO_GRUPO;
                ddlMaterialGrupo_SelectedIndexChanged(ddlMaterialGrupoPedido, null);
                ddlMaterialSubGrupoPedido.SelectedValue = desenvController.ObterMaterialSubGrupo(hidMaterialGrupoPedido.Value, hidMaterialSubGrupoPedido.Value).CODIGO_SUBGRUPO;
                ddlMaterialSubGrupo_SelectedIndexChanged(ddlMaterialSubGrupoPedido, null);
                ddlCorPedido.SelectedValue = prodController.ObterCoresBasicas().Where(p => p.DESC_COR.Trim() == hidCorPedido.Value.Trim()).Take(1).SingleOrDefault().COR;

                CarregarCorFornecedor(ddlMaterialGrupoPedido.SelectedItem.Text, ddlMaterialSubGrupoPedido.SelectedItem.Text, "ddlCorFornecedorPedido");
                ddlCorFornecedorPedido.SelectedValue = hidCorFornecedorPedido.Value;


                if (ddlFornecedor.SelectedValue.Trim() != "")
                {
                    ddlPedidoFornecedor.SelectedValue = ddlFornecedor.SelectedValue;
                }
                else if (hidFornecedorPedido.Value != "")
                {
                    //valida se existe...
                    var f = prodController.ObterFornecedor().Where(p => p.FORNECEDOR.Trim().Contains(hidFornecedorPedido.Value)).First();
                    if (f != null)
                        ddlPedidoFornecedor.SelectedValue = f.FORNECEDOR.Trim();
                }
                else
                    ddlPedidoFornecedor.SelectedValue = "";

                MoverAba("1");

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void chkMarcar_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            if (cb != null)
            {
                try
                {
                    decimal qtdeSelecionada = 0;
                    GridViewRow row = (GridViewRow)cb.NamingContainer;
                    if (row != null)
                    {
                        qtdeSelecionada = (labQtdeSelecionada.Text == "") ? 0 : Convert.ToDecimal(labQtdeSelecionada.Text);
                        Label _labQtde = row.FindControl("labQtde") as Label;
                        if (_labQtde != null)
                        {
                            if (cb.Checked)
                                qtdeSelecionada += (_labQtde.Text == "") ? 0 : Convert.ToDecimal(_labQtde.Text);
                            else
                                qtdeSelecionada -= (_labQtde.Text == "") ? 0 : Convert.ToDecimal(_labQtde.Text);
                        }

                        labQtdeSelecionada.Text = qtdeSelecionada.ToString();

                        Label _labFornecedor = row.FindControl("labFornecedor") as Label;
                        hidFornecedorPedido.Value = "";
                        if (_labFornecedor != null)
                            hidFornecedorPedido.Value = _labFornecedor.Text;
                    }
                }
                catch (Exception ex)
                {
                    labErro.Text = ex.Message;
                }
            }
        }
        protected void chkProdutoTodos_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;

            if (cb.Checked)
                labMarcarTodos.Text = "Desmarcar Todos";
            else
                labMarcarTodos.Text = "Marcar Todos";

            //Des/Marcar todos
            foreach (GridViewRow item in gvProdutos.Rows)
            {
                CheckBox chkMarcar = item.FindControl("chkMarcar") as CheckBox;
                if (cb.Checked)
                {
                    if (!chkMarcar.Checked)
                    {
                        chkMarcar.Checked = cb.Checked;
                        chkMarcar_CheckedChanged(chkMarcar, null);
                    }
                }
                else
                {
                    if (chkMarcar.Checked)
                    {
                        chkMarcar.Checked = cb.Checked;
                        chkMarcar_CheckedChanged(chkMarcar, null);
                    }
                }
            }
        }
        protected void ibtPesquisar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                labErroGerarPedido.Text = "";
                hidLimpaFiltro.Value = "";

                if (rdbFiltroDDL.Checked)
                {
                    if (ddlMaterialGrupo.SelectedValue.Trim() == "")
                    {
                        labErro.Text = "Selecione o Grupo.";
                        return;
                    }

                    if (ddlMaterialSubGrupo.SelectedValue.Trim() == "")
                    {
                        labErro.Text = "Selecione o SubGrupo.";
                        return;
                    }

                    if (ddlCor.SelectedValue.Trim() == "" && ddlCorFornecedor.SelectedValue.Trim() == "")
                    {
                        labErro.Text = "Selecione a Cor e/ou a Descrição do Fornecedor.";
                        return;
                    }
                }
                else
                {
                    if (txtGrupoFiltro.Text.Trim() == "" || txtGrupoFiltro.Text.Trim().Length < 3)
                    {
                        labErro.Text = "Informe o Grupo.";
                        return;
                    }
                    if (txtSubGrupoFiltro.Text.Trim() == "" || txtSubGrupoFiltro.Text.Trim().Length < 3)
                    {
                        labErro.Text = "Informe o SubGrupo.";
                        return;
                    }
                }

                CarregarPedidos(null, gvPedidoProduto, "T");
                CarregarProdutos();

                labQtdeSelecionada.Text = "0";

                labQtdeTotalPedido.Text = qtdePedidoProdutoIni.ToString("###,###,###,###,##0.000");
                labQtdeTotalProduto.Text = qtdeTotal.ToString("###,###,###,###,##0.000");
                decimal dNecessidadeCompra = (qtdePedidoProdutoIni - qtdeTotal);
                if (dNecessidadeCompra >= 0)
                {
                    labQtdeSobra.Text = (dNecessidadeCompra).ToString("###,###,###,###,##0.000");
                    labQtdeNecessidadeCompra.Text = "0,000";
                }
                else
                {
                    labQtdeNecessidadeCompra.Text = (dNecessidadeCompra * (-1)).ToString("###,###,###,###,##0.000");
                    labQtdeSobra.Text = "0,000";
                }

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void btEditarProduto_Click(object sender, EventArgs e)
        {
            ImageButton iBt = (ImageButton)sender;
            if (iBt != null)
            {
                labErro.Text = "";

                GridViewRow row = (GridViewRow)iBt.NamingContainer;
                if (row != null)
                {
                    try
                    {


                    }
                    catch (Exception ex)
                    {
                        labErro.Text = ex.Message;
                    }
                }
            }
        }
        protected void btSairProduto_Click(object sender, EventArgs e)
        {
            ImageButton iBt = (ImageButton)sender;
            if (iBt != null)
            {
                labErro.Text = "";

                GridViewRow row = (GridViewRow)iBt.NamingContainer;
                if (row != null)
                {
                    try
                    {

                    }
                    catch (Exception ex)
                    {
                        labErro.Text = ex.Message;
                    }
                }
            }
        }
        protected void ddlFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            labQtdeSelecionada.Text = "0";
            CarregarProdutos();
        }

        private void CarregarFiltros(List<SP_OBTER_MATERIAL_PRODUTOResult> _materialProduto)
        {
            /*
            List<SP_OBTER_MATERIAL_PRODUTOResult> _filtroDescFornecedor = new List<SP_OBTER_MATERIAL_PRODUTOResult>();
            var _descFornecedor = _materialProduto.Where(p => p.COR_FORNECEDOR != null).Select(s => s.COR_FORNECEDOR.Trim()).Distinct().ToList();
            foreach (var item in _descFornecedor)
                if (item.Trim() != "")
                    _filtroDescFornecedor.Add(new SP_OBTER_MATERIAL_PRODUTOResult { COR_FORNECEDOR = item.Trim() });

            _filtroDescFornecedor.Insert(0, new SP_OBTER_MATERIAL_PRODUTOResult { COR_FORNECEDOR = "" });
            ddlCorFornecedorFiltro.DataSource = _filtroDescFornecedor.OrderBy(p => p.COR_FORNECEDOR);
            ddlCorFornecedorFiltro.DataBind();

            List<SP_OBTER_MATERIAL_PRODUTOResult> _filtroTipo = new List<SP_OBTER_MATERIAL_PRODUTOResult>();
            var _tipo = _materialProduto.Where(p => p.TIPO != null).Select(s => s.TIPO.Trim()).Distinct().ToList();
            foreach (var item in _tipo)
                if (item.Trim() != "")
                    _filtroTipo.Add(new SP_OBTER_MATERIAL_PRODUTOResult { TIPO = item.Trim() });

            _filtroTipo.Insert(0, new SP_OBTER_MATERIAL_PRODUTOResult { TIPO = "" });
            ddlTipoFiltro.DataSource = _filtroTipo.OrderBy(p => p.TIPO);
            ddlTipoFiltro.DataBind();*/
        }
        private int ObterUltimoPedido()
        {
            return desenvController.ObterUltimoPedido();
        }
        #endregion

        #region "PEDIDO"
        private List<DESENV_PEDIDO> ObterPedidos(string status)
        {
            List<DESENV_PEDIDO> _pedidos = new List<DESENV_PEDIDO>();

            if (status == "")
                _pedidos = desenvController.ObterPedido1000();
            else if (status == "T")//TODOS MENOS OS EXCLUIDOS
                _pedidos = desenvController.ObterPedido1000().Where(p => p.STATUS != 'E').ToList();
            else
                _pedidos = desenvController.ObterPedido1000().Where(p => p.STATUS == Convert.ToChar(status)).ToList();

            return _pedidos;
        }
        private void CarregarPedidos(List<DESENV_PEDIDO> _pedidos, GridView gv, string status)
        {
            if (_pedidos == null || _pedidos.Count <= 0)
            {
                _pedidos = new List<DESENV_PEDIDO>();
                _pedidos = ObterPedidos(status);

                string grupo = "";
                string subGrupo = "";
                string cor = "";
                List<string> lstCores = new List<string>();
                string corFornecedor = "";
                string fornecedor = "";

                if (rdbFiltroDDL.Checked)
                {
                    grupo = ddlMaterialGrupo.SelectedItem.Text.Trim();
                    subGrupo = ddlMaterialSubGrupo.SelectedItem.Text.Trim();
                    if (ddlCor.SelectedValue.Trim() != "")
                        lstCores.Add(ddlCor.SelectedValue.Trim());
                    corFornecedor = ddlCorFornecedor.SelectedValue.Trim();
                    fornecedor = ddlFornecedor.SelectedValue.Trim();
                }
                else
                {
                    grupo = txtGrupoFiltro.Text.Trim().ToUpper();
                    subGrupo = txtSubGrupoFiltro.Text.Trim().ToUpper();
                    cor = "";
                    corFornecedor = "";
                    fornecedor = "";
                }

                if (fornecedor != "")
                    _pedidos = _pedidos.Where(p => p.FORNECEDOR != null && p.FORNECEDOR.Trim().Contains(fornecedor)).ToList();

                if (grupo != "")
                    _pedidos = _pedidos.Where(p => p.GRUPO != null && p.GRUPO.Trim().Contains(grupo)).ToList();

                if (subGrupo != "")
                    _pedidos = _pedidos.Where(p => p.SUBGRUPO != null && p.SUBGRUPO.Trim().Contains(subGrupo)).ToList();

                if (lstCores.Count() > 0)
                    _pedidos = _pedidos.Where(p => p.COR != null && lstCores.Contains(p.COR.Trim())).ToList();

                if (corFornecedor != "")
                    _pedidos = _pedidos.Where(p => p.COR_FORNECEDOR != null && p.COR_FORNECEDOR.Trim().Contains(corFornecedor)).ToList();
            }

            if (_pedidos == null || _pedidos.Count <= 0)
                _pedidos.Add(new DESENV_PEDIDO { CODIGO = 0, NUMERO_PEDIDO = 0 });

            gv.DataSource = _pedidos;
            gv.DataBind();
        }
        protected void gvPedidos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    DESENV_PEDIDO _pedido = e.Row.DataItem as DESENV_PEDIDO;

                    colunaPedido += 1;
                    if (_pedido != null)
                    {
                        ImageButton _btEditarPedido = e.Row.FindControl("btEditarPedido") as ImageButton;
                        ImageButton _btExcluirPedido = e.Row.FindControl("btExcluirPedido") as ImageButton;

                        Literal _litColuna = e.Row.FindControl("litColuna") as Literal;
                        if (_litColuna != null)
                            _litColuna.Text = colunaPedido.ToString();

                        if (_pedido.CODIGO > 0)
                        {
                            Label _labNumeroPedido = e.Row.FindControl("labNumeroPedido") as Label;
                            if (_labNumeroPedido != null)
                                _labNumeroPedido.Text = _pedido.NUMERO_PEDIDO.ToString();

                            Label _labDataPedido = e.Row.FindControl("labDataPedido") as Label;
                            if (_labDataPedido != null)
                                _labDataPedido.Text = _pedido.DATA_PEDIDO.ToString("dd/MM/yyyy");

                            Label _labPrevisaoEntrega = e.Row.FindControl("labPrevisaoEntrega") as Label;
                            if (_labPrevisaoEntrega != null)
                                if (_pedido.DATA_ENTREGA_PREV != null)
                                    _labPrevisaoEntrega.Text = Convert.ToDateTime(_pedido.DATA_ENTREGA_PREV).ToString("dd/MM/yyyy");

                            Label _labFornecedor = e.Row.FindControl("labFornecedor") as Label;
                            if (_labFornecedor != null)
                                _labFornecedor.Text = (_pedido.FORNECEDOR == null) ? "" : _pedido.FORNECEDOR.Trim().Substring(0, ((_pedido.FORNECEDOR.Trim().Length < 18) ? (_pedido.FORNECEDOR.Trim().Length) : 17));

                            Label _labSubGrupo = e.Row.FindControl("labSubGrupo") as Label;
                            if (_labSubGrupo != null)
                            {
                                string materialCodigo = "";
                                var material = desenvController.ObterMaterial(_pedido.GRUPO, _pedido.SUBGRUPO.Trim());
                                if (material != null)
                                {
                                    var materialCor = desenvController.ObterMaterialCor().Where(p => p.COR_MATERIAL.Trim() == _pedido.COR.Trim() && p.REFER_FABRICANTE.Trim() == _pedido.COR_FORNECEDOR.Trim() && material.Any(x => x.MATERIAL == p.MATERIAL)).FirstOrDefault();
                                    if (materialCor != null)
                                        materialCodigo = materialCor.MATERIAL.Trim();
                                }

                                _labSubGrupo.Text = ((materialCodigo == "") ? "" : (materialCodigo + " - ")) + _pedido.SUBGRUPO;
                            }

                            Label _labCor = e.Row.FindControl("labCor") as Label;
                            if (_labCor != null)
                            {
                                var _cor = prodController.ObterCoresBasicas(_pedido.COR);
                                _labCor.Text = (_cor == null) ? _pedido.COR : (_pedido.COR + " - " + _cor.DESC_COR);
                            }

                            Label _labCorFornecedor = e.Row.FindControl("labCorFornecedor") as Label;
                            if (_labCorFornecedor != null)
                                _labCorFornecedor.Text = _pedido.COR_FORNECEDOR;

                            Label _labQtde = e.Row.FindControl("labQtde") as Label;
                            if (_labQtde != null)
                            {
                                _labQtde.Text = _pedido.QTDE.ToString("###,###,###,##0.000");
                                qtdePedidoIni += _pedido.QTDE;
                            }

                            Label _labQtdeEntregue = e.Row.FindControl("labQtdeEntregue") as Label;
                            var totEntregue = 0.00M;
                            if (_labQtdeEntregue != null)
                            {
                                List<DESENV_PEDIDO_QTDE> _lstPedidoQtde = desenvController.ObterPedidoQtdePedido(_pedido.CODIGO);
                                var _lstTotal = _lstPedidoQtde.GroupBy(g => g.DESENV_PEDIDO).Select(i => new { QTDE = i.Sum(w => w.QTDE) }).SingleOrDefault();
                                if (_lstTotal != null)
                                {
                                    _labQtdeEntregue.Text = Convert.ToDecimal(_lstTotal.QTDE).ToString("###,###,##0.000");
                                    totEntregue = Convert.ToDecimal(_lstTotal.QTDE);
                                    qtdePedidoEntregue += Convert.ToDecimal(_lstTotal.QTDE);
                                }
                                else
                                    _labQtdeEntregue.Text = "0";
                            }

                            Label labQtdeFalt = e.Row.FindControl("labQtdeFalt") as Label;
                            labQtdeFalt.Text = (_pedido.QTDE - totEntregue).ToString();
                            qtdePedidoProdutoFalt += (_pedido.QTDE - totEntregue);

                            Label _labStatus = e.Row.FindControl("labStatus") as Label;
                            if (_labStatus != null)
                            {
                                string status = "SEM STATUS";
                                if (_pedido.STATUS == 'A') status = "AGUARDANDO";
                                else if (_pedido.STATUS == 'B') status = "BAIXADO";
                                else if (_pedido.STATUS == 'R') status = "RESERVADO";
                                else if (_pedido.STATUS == 'E') status = "EXCLUÍDO";

                                _labStatus.Text = status;
                            }

                            if (_pedido.STATUS != 'E')
                            {
                                _btExcluirPedido.Visible = true;
                                _btExcluirPedido.CommandArgument = _pedido.CODIGO.ToString();
                            }
                            _btEditarPedido.Visible = true;

                        }
                    }
                }
            }
        }
        protected void gvPedidos_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvPedidos.FooterRow;
            if (footer != null)
            {
                footer.Cells[2].Text = "Total";
                footer.Cells[9].Text = qtdePedidoIni.ToString();
                footer.Cells[10].Text = qtdePedidoEntregue.ToString();
                footer.Cells[11].Text = qtdePedidoProdutoFalt.ToString();
            }
        }
        protected void btIncluirPedido_Click(object sender, EventArgs e)
        {
            ImageButton bt = (ImageButton)sender;
            if (bt != null)
            {
                GridViewRow row = (GridViewRow)bt.NamingContainer;
                if (row != null)
                {
                    labErro.Text = "";
                    try
                    {

                        btAbrirSubPedido.Visible = false;
                        btCancelarPedido_Click(null, null);
                        hidLimpaFiltro.Value = "";
                        ddlStatus.SelectedValue = "A"; //AGUARDANDO
                        int numeroPedido = desenvController.ObterUltimoPedido();
                        txtPedidoNumero.Text = numeroPedido.ToString();
                        MoverAba("1");
                    }
                    catch (Exception ex)
                    {
                        labErro.Text = "ERRO (btIncluirPedido_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
                    }
                }
            }
        }
        protected void btEditarPedido_Click(object sender, EventArgs e)
        {
            ImageButton bt = (ImageButton)sender;

            if (bt != null)
            {
                GridViewRow row = (GridViewRow)bt.NamingContainer;
                if (row != null)
                {
                    try
                    {

                        hidLimpaFiltro.Value = "N";
                        btAbrirSubPedido.Visible = false;

                        GridView gv = null;
                        if (bt.ID == "btEditarPedido")
                            gv = gvPedidos;
                        else if (bt.ID == "btEditarPedidoEntrada")
                            gv = gvPedidosEntrada;

                        int codigoPedido = 0;
                        codigoPedido = Convert.ToInt32(gv.DataKeys[row.RowIndex].Value.ToString());

                        hidCodigoPedido.Value = codigoPedido.ToString();

                        var _pedido = desenvController.ObterPedido(codigoPedido);
                        if (_pedido != null)
                        {
                            txtPedidoNumero.Text = _pedido.NUMERO_PEDIDO.ToString();
                            ddlColecoes.SelectedValue = (new BaseController().BuscaColecaoAtual(_pedido.COLECAO)).COLECAO;

                            var f = prodController.ObterFornecedor().Where(p => p.FORNECEDOR.Trim() == _pedido.FORNECEDOR.Trim());
                            if (f != null && f.Count() > 0)
                            {
                                ddlPedidoFornecedor.SelectedValue = f.Take(1).SingleOrDefault().FORNECEDOR.Trim();
                                ddlPedidoFornecedor_SelectedIndexChanged(ddlPedidoFornecedor, null);
                            }

                            var g = desenvController.ObterMaterialGrupo(_pedido.GRUPO);
                            if (g != null)
                            {
                                ddlMaterialGrupoPedido.SelectedValue = g.CODIGO_GRUPO;
                                ddlMaterialGrupo_SelectedIndexChanged(ddlMaterialGrupoPedido, null);
                            }

                            var s = desenvController.ObterMaterialSubGrupo(_pedido.GRUPO, _pedido.SUBGRUPO);
                            if (s != null)
                                ddlMaterialSubGrupoPedido.SelectedValue = s.CODIGO_SUBGRUPO;

                            if (_pedido.STATUS != 'E')
                                ddlStatus.SelectedValue = _pedido.STATUS.ToString();
                            else
                                ddlStatus.SelectedValue = "";

                            CarregarCores(ddlMaterialGrupoPedido.SelectedItem.Text.Trim(), ddlMaterialSubGrupoPedido.SelectedItem.Text.Trim());

                            var c = prodController.ObterCoresBasicas(_pedido.COR);
                            if (c != null)
                                ddlCorPedido.SelectedValue = c.COR;

                            CarregarCorFornecedor(_pedido.GRUPO, _pedido.SUBGRUPO, "ddlCorFornecedorPedido");
                            ddlCorFornecedorPedido.SelectedValue = _pedido.COR_FORNECEDOR;
                            txtQtde.Text = _pedido.QTDE.ToString();
                            txtQtde.Enabled = false;
                            txtPreco.Text = _pedido.VALOR.ToString();
                            ddlCondicaoPgto.SelectedValue = _pedido.CONDICAO_PGTO;
                            ddlCondicaoPgto_SelectedIndexChanged(null, null);
                            txtCondicaoPgtoOutro.Text = _pedido.CONDICAO_PGTO_OUTRO;
                            txtPedidoData.Text = _pedido.DATA_PEDIDO.ToString("dd/MM/yyyy");
                            txtPedidoPrevEntrega.Text = (_pedido.DATA_ENTREGA_PREV == null) ? "" : Convert.ToDateTime(_pedido.DATA_ENTREGA_PREV).ToString("dd/MM/yyyy");
                            txtPedidoDataReserva.Text = (_pedido.DATA_RESERVA == null) ? "" : Convert.ToDateTime(_pedido.DATA_RESERVA).ToString("dd/MM/yyyy");
                            ddlUnidade.SelectedValue = (_pedido.UNIDADE_MEDIDA == null) ? "0" : _pedido.UNIDADE_MEDIDA.ToString();

                            CarregarSubPedidos(_pedido.CODIGO);

                        }

                        btAbrirSubPedido.Visible = true;

                        MoverAba("1");
                    }
                    catch (Exception ex)
                    {
                        labErro.Text = "ERRO (btEditarPedido_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
                    }
                }
            }

        }

        protected void btExcluirPedido_Click(object sender, EventArgs e)
        {
            ImageButton bt = (ImageButton)sender;
            if (bt != null)
            {
                GridViewRow row = (GridViewRow)bt.NamingContainer;
                if (row != null)
                {
                    labErro.Text = "";
                    try
                    {

                        GridView gv = null;
                        if (bt.ID == "btExcluirPedido")
                            gv = gvPedidos;

                        int codigoPedido = 0;
                        codigoPedido = Convert.ToInt32(gv.DataKeys[row.RowIndex].Value.ToString());

                        DESENV_PEDIDO _pedido = desenvController.ObterPedido(codigoPedido);
                        if (_pedido != null)
                        {
                            //VALIDAR EXCLUSAO DE PEDIDO
                            //VERIFICA SE JA TEM QTDE ENTREGUE
                            List<DESENV_PEDIDO_QTDE> _listaPedidoQtde = desenvController.ObterPedidoQtdePedido(codigoPedido);
                            if (_listaPedidoQtde != null && _listaPedidoQtde.Select(p => p.QTDE).Sum() > 0)
                            {
                                labErro.Text = "Não será possível excluir este Pedido. Pedido tem entrega realizada.";
                                return;
                            }

                            _pedido.STATUS = 'E';
                            desenvController.ExcluirPedido(_pedido);


                        }

                    }
                    catch (Exception ex)
                    {
                        labErro.Text = "ERRO (btExcluirPedido_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
                    }
                }
            }
        }
        protected void btSalvarPedido_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                if (!ValidarCampos())
                {
                    labErro.Text = "Preencha corretamente os campos em Vermelho.";
                    return;
                }

                hidLimpaFiltro.Value = "";

                DESENV_PEDIDO _pedido = new DESENV_PEDIDO();
                _pedido.NUMERO_PEDIDO = Convert.ToInt32(txtPedidoNumero.Text);
                _pedido.COLECAO = ddlColecoes.SelectedValue;
                _pedido.FORNECEDOR = ddlPedidoFornecedor.SelectedValue;
                _pedido.GRUPO = ddlMaterialGrupoPedido.SelectedItem.Text.Trim();
                _pedido.SUBGRUPO = ddlMaterialSubGrupoPedido.SelectedItem.Text.Trim();
                _pedido.STATUS = Convert.ToChar(ddlStatus.SelectedValue);

                _pedido.COR = ddlCorPedido.SelectedValue;
                _pedido.COR_FORNECEDOR = ddlCorFornecedorPedido.SelectedValue.Trim().ToUpper();

                _pedido.QTDE = Convert.ToDecimal(txtQtde.Text);
                _pedido.VALOR = Convert.ToDecimal(txtPreco.Text);
                _pedido.CONDICAO_PGTO = ddlCondicaoPgto.SelectedValue;
                _pedido.CONDICAO_PGTO_OUTRO = txtCondicaoPgtoOutro.Text.Trim();

                _pedido.DATA_PEDIDO = Convert.ToDateTime(txtPedidoData.Text);
                if (txtPedidoPrevEntrega.Text.Trim() != "")
                    _pedido.DATA_ENTREGA_PREV = Convert.ToDateTime(txtPedidoPrevEntrega.Text);
                if (txtPedidoDataReserva.Text.Trim() != "")
                    _pedido.DATA_RESERVA = Convert.ToDateTime(txtPedidoDataReserva.Text);

                _pedido.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                _pedido.DATA_INCLUSAO = DateTime.Now;
                _pedido.UNIDADE_MEDIDA = Convert.ToInt32(ddlUnidade.SelectedValue);

                if (hidCodigoPedido.Value == "" || hidCodigoPedido.Value == "0")
                {
                    int codigoPedido = 0;
                    codigoPedido = desenvController.InserirPedido(_pedido);

                    // inserir subpedido
                    DESENV_PEDIDO_SUB _pedidoSub = new DESENV_PEDIDO_SUB();
                    _pedidoSub.DESENV_PEDIDO = _pedido.CODIGO;
                    _pedidoSub.FORNECEDOR = _pedido.FORNECEDOR;
                    _pedidoSub.QTDE = _pedido.QTDE;
                    _pedidoSub.VALOR = _pedido.VALOR;
                    _pedidoSub.DATA_PEDIDO = _pedido.DATA_PEDIDO;
                    _pedidoSub.DATA_ENTREGA_PREV = _pedido.DATA_ENTREGA_PREV;
                    _pedidoSub.EMAIL = txtCorpoEmail.Text.Trim();
                    _pedidoSub.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                    _pedidoSub.DATA_INCLUSAO = DateTime.Now;

                    desenvController.InserirPedidoSub(_pedidoSub);

                    //CarregarSubPedidos(_pedido.CODIGO);
                }
                else
                {
                    _pedido.CODIGO = Convert.ToInt32(hidCodigoPedido.Value);
                    desenvController.AtualizarPedido(_pedido);
                    //Obter pedido atualizado
                    _pedido = desenvController.ObterPedido(Convert.ToInt32(hidCodigoPedido.Value));
                }

                //LimparCampos(false);
                txtPedidoPrevEntrega.Text = "";
                txtPedidoDataReserva.Text = "";
                txtQtde.Text = "";
                hidCodigoPedido.Value = "";

                int numeroPedido = desenvController.ObterUltimoPedido();
                txtPedidoNumero.Text = numeroPedido.ToString();

                labErro.Text = "Pedido cadastrado com sucesso.";

                //Enviar E-mail
                if (chkEmail.Checked)
                    EnviarEmail(_pedido);

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void btCancelarPedido_Click(object sender, EventArgs e)
        {
            hidLimpaFiltro.Value = "";
            System.Drawing.Color _OK = System.Drawing.Color.Gray;

            labNumeroPedido.ForeColor = _OK;
            labColecao.ForeColor = _OK;
            labFornecedorPedido.ForeColor = _OK;
            labMaterialGrupoPedido.ForeColor = _OK;
            labMaterialSubGrupoPedido.ForeColor = _OK;
            labStatus.ForeColor = _OK;
            labCorPedido.ForeColor = _OK;
            labCorFornecedorPedido.ForeColor = _OK;
            labQuantidade.ForeColor = _OK;
            labPreco.ForeColor = _OK;
            labCondicaoPgto.ForeColor = _OK;
            labDataPedido.ForeColor = _OK;
            labDataPrevisaoEntrega.ForeColor = _OK;
            labDataReserva.ForeColor = _OK;
            labUnidadeMedida.ForeColor = _OK;

            LimparCampos(false);

            txtQtde.Enabled = true;
            //MoverAba("0");
        }
        protected void ibtPedidoNovo_Click(object sender, EventArgs e)
        {
            try
            {
                btAbrirSubPedido.Visible = false;
                hidCodigoPedido.Value = "";
                labErro.Text = "";
                hidLimpaFiltro.Value = "";
                btCancelarPedido_Click(null, null);
                int numeroPedido = desenvController.ObterUltimoPedido();
                txtPedidoNumero.Text = numeroPedido.ToString();
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void txtPedidoNumero_TextChanged(object sender, EventArgs e)
        {
            TextBox _txt = (TextBox)sender;
            if (_txt != null && _txt.Text.Trim() != "")
                ValidarNumeroPedido(Convert.ToInt32(_txt.Text));
        }
        protected void ddlCondicaoPgto_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCondicaoPgtoOutro.Text = "";
            if (ddlCondicaoPgto.SelectedValue.Trim().ToUpper() == "##")
                txtCondicaoPgtoOutro.Visible = true;
            else
                txtCondicaoPgtoOutro.Visible = false;
        }
        protected void ddlPedidoFornecedor_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtEmail.Text = "";
            var email = prodController.ObterFornecedor().Where(p => p.FORNECEDOR.Trim() == ddlPedidoFornecedor.SelectedValue.Trim() && p.EMAIL != null && p.EMAIL.Trim() != "").ToList();
            if (email != null && email.Count() > 0)
            {
                txtEmail.Text = email[0].EMAIL;
            }
        }
        protected void ibtPesquisarTodos_Click(object sender, EventArgs e)
        {
            List<DESENV_PEDIDO> _pedidos = new List<DESENV_PEDIDO>();

            try
            {
                labErro.Text = "";
                if (txtPedidoNumeroTodos.Text.Trim() != "" && Convert.ToInt32(txtPedidoNumeroTodos.Text.Trim()) < 1000)
                {
                    labErro.Text = "Número do pedido deve ser acima de 1000.";
                    return;
                }


                if (txtPedidoNumeroTodos.Text.Trim() != "")
                {
                    var pedido = desenvController.ObterPedidoNumero(Convert.ToInt32(txtPedidoNumeroTodos.Text));
                    if (pedido != null)
                    {
                        _pedidos.Add(pedido);
                    }
                    else
                    {
                        labErro.Text = "Pedido não foi encontrado. Refaça sua pesquisa.";
                        return;
                    }
                }

                CarregarPedidos(_pedidos, gvPedidos, "");

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void btAbrirSubPedido_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            string msg = "";
            string _url = "";
            if (b != null)
            {
                try
                {
                    labErro.Text = "";

                    if (hidCodigoPedido.Value == "")
                    {
                        labErro.Text = "Pedido não existe.";
                        return;
                    }

                    string numeroPedido = txtPedidoNumero.Text;

                    //Abrir pop-up
                    _url = "fnAbrirTelaCadastroMaior('desenv_material_compra_subpedido.aspx?p=" + numeroPedido + "');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _url, true);
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                }
            }

        }
        #endregion

        #region "EMAIL"
        protected void chkEmail_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            if (chk != null)
            {
                txtCorpoEmail.Enabled = chk.Checked;
                txtCorpoEmail.Text = "";
                txtEmail.Enabled = chk.Checked;
                txtEmail.Text = "";
            }
        }
        private void EnviarEmail(DESENV_PEDIDO _pedido)
        {
            string assuntoEmail = "";

            assuntoEmail = "HBF - Pedido de Compra: " + _pedido.NUMERO_PEDIDO.ToString().Trim() + " - " + _pedido.SUBGRUPO + " ";
            assuntoEmail = assuntoEmail + _pedido.COR_FORNECEDOR;

            email_envio email = new email_envio();
            email.ASSUNTO = assuntoEmail;
            email.REMETENTE = (USUARIO)Session["USUARIO"];
            email.MENSAGEM = MontarCorpoEmail(_pedido, txtCorpoEmail.Text);

            List<string> destinatario = new List<string>();
            var usuarioEmail = new UsuarioController().ObterEmailUsuarioTela(3, 1).Where(p => p.EMAIL != null && p.EMAIL.Trim() != "").ToList();
            foreach (var usu in usuarioEmail)
                if (usu != null)
                    destinatario.Add(usu.EMAIL);

            if (txtEmail.Text.Trim() != "")
            {
                string[] sEmail = txtEmail.Text.Trim().Split(';');
                if (sEmail != null && sEmail.Count() > 0)
                    foreach (string s in sEmail)
                        destinatario.Add(s);
            }

            email.DESTINATARIOS = destinatario;

            if (destinatario.Count > 0)
                email.EnviarEmail();
        }
        private string MontarCorpoEmail(DESENV_PEDIDO _pedido, string corpoEmail)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("");

            sb.Append("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            sb.Append("<html>");
            sb.Append("<head>");
            sb.Append("    <title>COMPRA DE MATERIAL</title>");
            sb.Append("    <meta charset='UTF-8'>");
            sb.Append("</head>");
            sb.Append("<body>");
            sb.Append("    <div style='color: black; font-size: 10.2pt; font-weight: 700; font-family: Arial, sans-serif;");
            sb.Append("        background: white; white-space: nowrap;'>");
            sb.Append("        <span style='font-family:Calibri; font-size:medium;'>Compra de Material</span>");
            sb.Append("        <br />");
            sb.Append("        <br />");
            sb.Append("        <div id='divMaterial' align='left'>");
            sb.Append("            <table border='0' cellpadding='0' cellspacing='0' style='width: 1000pt; padding: 0px;");
            sb.Append("                color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
            sb.Append("                background: white; white-space: nowrap;'>");
            sb.Append("                <tr>");
            sb.Append("                    <td style='line-height: 20px;'>");
            sb.Append("                        <table border='0' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
            sb.Append("                            width: 750pt'>");
            sb.Append("                            <tr style='text-align: left;'>");
            sb.Append("                                <td style='width: 180px; font-family:Calibri;'>");
            sb.Append("                                    Número:");
            sb.Append("                                </td>");
            sb.Append("                                <td style='font-family:Calibri;'>");
            sb.Append("                                    " + _pedido.NUMERO_PEDIDO);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Fornecedor:");
            sb.Append("                                </td>");
            sb.Append("                                <td style='font-family:Calibri;'>");
            sb.Append("                                    " + _pedido.FORNECEDOR);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Material:");
            sb.Append("                                </td>");
            sb.Append("                                <td style='font-family:Calibri;'>");
            sb.Append("                                    " + _pedido.SUBGRUPO);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Cor Fornecedor:");
            sb.Append("                                </td>");
            sb.Append("                                <td style='font-family:Calibri;'>");
            sb.Append("                                    " + _pedido.COR_FORNECEDOR);
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Quantidade:");
            sb.Append("                                </td>");
            sb.Append("                                <td style='font-family:Calibri;'>");
            sb.Append("                                    " + _pedido.QTDE.ToString("###,###,###,##0.000") + " " + ((_pedido.UNIDADE_MEDIDA1 == null) ? "" : (_pedido.UNIDADE_MEDIDA1.DESCRICAO + "s")));
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Preço:");
            sb.Append("                                </td>");
            sb.Append("                                <td style='font-family:Calibri;'>");
            sb.Append("                                    R$ " + _pedido.VALOR.ToString("###,###,###,##0.00"));
            sb.Append("                                </td>");
            sb.Append("                            </tr>");
            sb.Append("                            <tr>");
            sb.Append("                                <td>");
            sb.Append("                                    Data do Pedido:");
            sb.Append("                                </td>");
            sb.Append("                                <td style='font-family:Calibri;'>");
            sb.Append("                                    " + _pedido.DATA_PEDIDO.ToString("dd/MM/yyyy"));
            sb.Append("                                </td>");
            sb.Append("                            </tr>");

            List<FORMA_PGTO> _formaPGTO = (new BaseController().ObterFormaPgto());
            if (_formaPGTO != null && _formaPGTO.Count > 0)
            {
                var fp = _formaPGTO.Where(p => p.CONDICAO_PGTO.Trim() == _pedido.CONDICAO_PGTO.Trim()).SingleOrDefault();
                if (fp != null)
                {
                    sb.Append("                            <tr>");
                    sb.Append("                                <td>");
                    sb.Append("                                    Condição de Pagamento:");
                    sb.Append("                                </td>");
                    sb.Append("                                <td style='font-family:Calibri;'>");
                    sb.Append("                                    " + ((_pedido.CONDICAO_PGTO.Trim() == "##") ? "" : fp.DESC_COND_PGTO.Trim()) + " " + _pedido.CONDICAO_PGTO_OUTRO.Trim());
                    sb.Append("                                </td>");
                    sb.Append("                            </tr>");
                }
            }

            if (_pedido.DATA_ENTREGA_PREV != null)
            {
                sb.Append("                            <tr>");
                sb.Append("                                <td>");
                sb.Append("                                    Previsão de Entrega:");
                sb.Append("                                </td>");
                sb.Append("                                <td style='font-family:Calibri;'>");
                sb.Append("                                    " + Convert.ToDateTime(_pedido.DATA_ENTREGA_PREV).ToString("dd/MM/yyyy"));
                sb.Append("                                </td>");
                sb.Append("                            </tr>");
            }
            if (_pedido.DATA_RESERVA != null)
            {
                sb.Append("                            <tr>");
                sb.Append("                                <td>");
                sb.Append("                                    Data de Reserva:");
                sb.Append("                                </td>");
                sb.Append("                                <td style='font-family:Calibri;'>");
                sb.Append("                                    " + Convert.ToDateTime(_pedido.DATA_RESERVA).ToString("dd/MM/yyyy"));
                sb.Append("                                </td>");
                sb.Append("                            </tr>");
            }
            sb.Append("                        </table>");
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("                <tr>");
            sb.Append("                    <td>");
            sb.Append("                        &nbsp;");
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("                <tr>");
            sb.Append("                    <td>");
            sb.Append("                        " + corpoEmail.Replace("\n", "<br>"));
            sb.Append("                    </td>");
            sb.Append("                </tr>");
            sb.Append("            </table>");
            sb.Append("        </div>");
            sb.Append("        <br />");
            sb.Append("        <br />");
            sb.Append("        <span>Enviado por: " + ((USUARIO)Session["USUARIO"]).NOME_USUARIO + "</span>");
            sb.Append("    </div>");
            sb.Append("</body>");
            sb.Append("</html>");

            return sb.ToString();
        }
        #endregion

        #region "ENTRADAS"
        protected void gvPedidosEntrada_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    DESENV_PEDIDO _pedido = e.Row.DataItem as DESENV_PEDIDO;

                    colunaEntrada += 1;
                    if (_pedido != null)
                    {

                        Literal _litColuna = e.Row.FindControl("litColuna") as Literal;
                        if (_litColuna != null)
                            _litColuna.Text = colunaEntrada.ToString();

                        if (_pedido.CODIGO > 0)
                        {
                            Label _labNumeroPedido = e.Row.FindControl("labNumeroPedido") as Label;
                            if (_labNumeroPedido != null)
                                _labNumeroPedido.Text = _pedido.NUMERO_PEDIDO.ToString();

                            Label _labDataPedido = e.Row.FindControl("labDataPedido") as Label;
                            if (_labDataPedido != null)
                                _labDataPedido.Text = _pedido.DATA_PEDIDO.ToString("dd/MM/yyyy");

                            Label _labPrevisaoEntrega = e.Row.FindControl("labPrevisaoEntrega") as Label;
                            if (_labPrevisaoEntrega != null)
                                if (_pedido.DATA_ENTREGA_PREV != null)
                                    _labPrevisaoEntrega.Text = Convert.ToDateTime(_pedido.DATA_ENTREGA_PREV).ToString("dd/MM/yyyy");

                            Label _labFornecedor = e.Row.FindControl("labFornecedor") as Label;
                            if (_labFornecedor != null)
                                _labFornecedor.Text = (_pedido.FORNECEDOR == null) ? "" : _pedido.FORNECEDOR.Trim().Substring(0, ((_pedido.FORNECEDOR.Trim().Length < 18) ? (_pedido.FORNECEDOR.Trim().Length) : 17));

                            Label _labSubGrupo = e.Row.FindControl("labSubGrupo") as Label;
                            if (_labSubGrupo != null)
                            {
                                string materialCodigo = "";
                                var material = desenvController.ObterMaterial(_pedido.GRUPO, _pedido.SUBGRUPO.Trim());
                                if (material != null)
                                {
                                    var materialCor = desenvController.ObterMaterialCor().Where(p => p.COR_MATERIAL.Trim() == _pedido.COR.Trim() && p.REFER_FABRICANTE.Trim() == _pedido.COR_FORNECEDOR.Trim() && material.Any(x => x.MATERIAL == p.MATERIAL)).FirstOrDefault();
                                    if (materialCor != null)
                                        materialCodigo = materialCor.MATERIAL.Trim();
                                }

                                _labSubGrupo.Text = ((materialCodigo == "") ? "" : (materialCodigo + " - ")) + _pedido.SUBGRUPO;
                            }

                            Label _labCor = e.Row.FindControl("labCor") as Label;
                            if (_labCor != null)
                            {
                                var _cor = prodController.ObterCoresBasicas(_pedido.COR);
                                _labCor.Text = (_cor == null) ? _pedido.COR : (_pedido.COR + " - " + _cor.DESC_COR);
                            }

                            Label _labCorFornecedor = e.Row.FindControl("labCorFornecedor") as Label;
                            if (_labCorFornecedor != null)
                                _labCorFornecedor.Text = _pedido.COR_FORNECEDOR;

                            Label _labQtde = e.Row.FindControl("labQtde") as Label;
                            if (_labQtde != null)
                            {
                                _labQtde.Text = _pedido.QTDE.ToString("###,###,###,##0.000");
                                qtdeEntradaIni += _pedido.QTDE;
                            }

                            Label _labQtdeEntregue = e.Row.FindControl("labQtdeEntregue") as Label;
                            var totEntregue = 0.00M;
                            if (_labQtdeEntregue != null)
                            {
                                List<DESENV_PEDIDO_QTDE> _lstPedidoQtde = desenvController.ObterPedidoQtdePedido(_pedido.CODIGO);
                                var _lstTotal = _lstPedidoQtde.GroupBy(g => g.DESENV_PEDIDO).Select(i => new { QTDE = i.Sum(w => w.QTDE) }).SingleOrDefault();
                                if (_lstTotal != null)
                                {
                                    _labQtdeEntregue.Text = Convert.ToDecimal(_lstTotal.QTDE).ToString("###,###,###,##0.000");
                                    totEntregue = Convert.ToDecimal(_lstTotal.QTDE);
                                    qtdeEntradaEntregue += Convert.ToDecimal(_lstTotal.QTDE);
                                }
                                else
                                    _labQtdeEntregue.Text = "0";
                            }

                            Label labQtdeFalt = e.Row.FindControl("labQtdeFalt") as Label;
                            labQtdeFalt.Text = (_pedido.QTDE - totEntregue).ToString();
                            qtdeEntradaFalt += (_pedido.QTDE - totEntregue);

                            //TextBox t = e.Row.FindControl("txtDataEntrega") as TextBox;
                            //if (t != null)
                            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + t.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy', maxDate: new Date() });});", true);

                            ImageButton _btEntrarQtde = e.Row.FindControl("btEntrarQtde") as ImageButton;
                            if (_btEntrarQtde != null)
                                _btEntrarQtde.CommandArgument = _pedido.CODIGO.ToString();
                        }
                    }
                }
            }
        }
        protected void gvPedidosEntrada_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvPedidosEntrada.FooterRow;
            if (footer != null)
            {
                footer.Cells[2].Text = "Total";
                footer.Cells[8].Text = qtdeEntradaIni.ToString();
                footer.Cells[9].Text = qtdeEntradaEntregue.ToString();
                footer.Cells[10].Text = qtdeEntradaFalt.ToString();

            }
        }
        protected void btEntrarQtde_Click(object sender, EventArgs e)
        {
            ImageButton b = (ImageButton)sender;
            string msg = "";
            string _url = "";
            if (b != null)
            {
                try
                {
                    labErro.Text = "";
                    string codigoPedido = b.CommandArgument;

                    //Abrir pop-up
                    _url = "fnAbrirTelaCadastroMaior('desenv_material_compra_entrada.aspx?p=" + codigoPedido + "');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _url, true);
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                }
            }


            //string msg = "";
            //DateTime _databaixa = DateTime.Now;
            //decimal _qtdeentregue = 0;

            //try
            //{
            //    labErro.Text = "";
            //    ImageButton b = (ImageButton)sender;
            //    if (b != null)
            //    {
            //        labErro.Text = "";
            //        int codigoPedido = Convert.ToInt32(b.CommandArgument);
            //        GridViewRow _row = (GridViewRow)b.NamingContainer;
            //        if (_row != null)
            //        {
            //            TextBox _txtDataEntrega = _row.FindControl("txtDataEntrega") as TextBox;
            //            if (_txtDataEntrega.Text == "" || !DateTime.TryParse(_txtDataEntrega.Text, out _databaixa))
            //                msg = "Data inválida. Informe uma Data.";

            //            TextBox _txtQtdeEntrega = _row.FindControl("txtQtdeEntregue") as TextBox;
            //            if (_txtQtdeEntrega.Text == "" || !Decimal.TryParse(_txtQtdeEntrega.Text, out _qtdeentregue) || _qtdeentregue == 0)
            //                msg = "Quantidade inválida. Informe uma Quantidade.";

            //            TextBox _txtNotaFiscal = _row.FindControl("txtNotaFiscal") as TextBox;
            //            if (_txtNotaFiscal.Text.Trim() == "")
            //                msg = "Informe a Nota Fiscal.";

            //            if (msg != "")
            //            {
            //                labErro.Text = msg;
            //            }
            //            else
            //            {
            //                //Inserir QTDE
            //                DESENV_PEDIDO_QTDE _qtdePedido = new DESENV_PEDIDO_QTDE();
            //                _qtdePedido.DESENV_PEDIDO = codigoPedido;
            //                _qtdePedido.QTDE = _qtdeentregue;
            //                _qtdePedido.DATA = _databaixa;
            //                _qtdePedido.NOTA_FISCAL = _txtNotaFiscal.Text.Trim();
            //                _qtdePedido.USUARIO_INCLUSAO = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
            //                desenvController.InserirPedidoQtde(_qtdePedido);
            //            }

            //            ibtPesquisarEntrada_Click(null, null);
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    labErro.Text = ex.Message;
            //}
        }
        protected void btBaixarPedido_Click(object sender, EventArgs e)
        {
            ImageButton _bt = (ImageButton)sender;
            if (_bt != null)
            {
                GridViewRow row = (GridViewRow)_bt.NamingContainer;
                if (row != null)
                {

                    GridView gv = null;
                    string statusPedido = "";
                    if (_bt.ID == "btBaixarPedidoEntrada")
                    {
                        gv = gvPedidosEntrada;
                        statusPedido = "A";
                    }

                    string codigoPedido = gv.DataKeys[row.RowIndex].Value.ToString();

                    if (codigoPedido != "")
                    {
                        var _pedido = desenvController.ObterPedido(Convert.ToInt32(codigoPedido));
                        if (_pedido != null)
                        {
                            if (statusPedido == "A")
                                _pedido.STATUS = 'B';
                            else if (statusPedido == "R")
                                _pedido.STATUS = 'A';
                            desenvController.AtualizarPedido(_pedido);
                            CarregarPedidos(null, gv, statusPedido);
                        }
                    }
                }
            }
        }
        protected void ibtPesquisarEntrada_Click(object sender, EventArgs e)
        {
            List<DESENV_PEDIDO> _pedidos = new List<DESENV_PEDIDO>();

            try
            {
                labErro.Text = "";
                if (txtPedidoNumeroEntrada.Text.Trim() != "" && Convert.ToInt32(txtPedidoNumeroEntrada.Text.Trim()) < 1000)
                {
                    labErro.Text = "Número do pedido deve ser acima de 1000.";
                    return;
                }


                if (txtPedidoNumeroEntrada.Text.Trim() != "")
                {
                    var pedido = desenvController.ObterPedidoNumero(Convert.ToInt32(txtPedidoNumeroEntrada.Text));
                    if (pedido != null)
                    {
                        if (pedido.STATUS != 'A')
                        {
                            labErro.Text = "Este pedido não está aguardando Entradas.";
                            return;
                        }
                        _pedidos.Add(pedido);
                    }
                    else
                    {
                        labErro.Text = "Pedido não foi encontrado. Refaça sua pesquisa.";
                        return;
                    }
                }

                CarregarPedidos(_pedidos, gvPedidosEntrada, "A");
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        #endregion

        #region "ESTOQUE"
        private void CarregarEstoque()
        {
            List<SP_OBTER_MATERIAL_SALDOResult> _saldoEstoque = new List<SP_OBTER_MATERIAL_SALDOResult>();

            _saldoEstoque = desenvController.ObterMaterialSaldo(ddlMaterialGrupo.SelectedItem.Text.Trim(), ddlMaterialSubGrupo.SelectedItem.Text.Trim(), ddlCor.SelectedValue.Trim(), ddlCorFornecedor.SelectedValue.Trim(), ddlFornecedor.SelectedValue.Trim(), 1);

            labErroEstoque.Text = "";
            if (_saldoEstoque == null || _saldoEstoque.Count <= 0)
                labErroEstoque.Text = "Nenhum Material encontrado. Refaça sua pesquisa.";

            gvEstoque.DataSource = _saldoEstoque;
            gvEstoque.DataBind();
        }
        protected void gvEstoque_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_MATERIAL_SALDOResult _saldo = e.Row.DataItem as SP_OBTER_MATERIAL_SALDOResult;

                    colunaEstoque += 1;
                    if (_saldo != null)
                    {

                        Literal _litColuna = e.Row.FindControl("litColuna") as Literal;
                        if (_litColuna != null)
                            _litColuna.Text = colunaEstoque.ToString();

                        Label _labGrupo = e.Row.FindControl("labGrupo") as Label;
                        if (_labGrupo != null)
                        {
                            if (_saldo.GRUPO.Trim() != "")
                                _labGrupo.Text = _saldo.GRUPO;
                            else
                                _labGrupo.Visible = false;
                        }

                        Label _labSubGrupo = e.Row.FindControl("labSubGrupo") as Label;
                        if (_labSubGrupo != null)
                        {
                            if (_saldo.SUBGRUPO.Trim() != "")
                                _labSubGrupo.Text = _saldo.SUBGRUPO;
                            else
                                _labSubGrupo.Visible = false;
                        }

                        Label _labCor = e.Row.FindControl("labCor") as Label;
                        if (_labCor != null)
                        {
                            if (_saldo.COR.Trim() != "")
                            {
                                var _cor = prodController.ObterCoresBasicas(_saldo.COR);
                                _labCor.Text = (_cor == null) ? _saldo.COR : _cor.DESC_COR;
                            }
                            else
                                _labCor.Visible = false;
                        }

                        Label _labCorFornecedor = e.Row.FindControl("labCorFornecedor") as Label;
                        if (_labCorFornecedor != null)
                        {
                            if (_saldo.COR_FORNECEDOR.Trim() != "")
                                _labCorFornecedor.Text = _saldo.COR_FORNECEDOR;
                            else
                                _labCorFornecedor.Visible = false;
                        }

                        Label _labFornecedor = e.Row.FindControl("labFornecedor") as Label;
                        if (_labFornecedor != null)
                        {
                            if (_saldo.FORNECEDOR.Trim() != "")
                                _labFornecedor.Text = _saldo.FORNECEDOR;
                            else
                                _labFornecedor.Visible = false;
                        }

                        Label _labQtdeEntregue = e.Row.FindControl("labQtdeEntregue") as Label;
                        if (_labQtdeEntregue != null)
                        {
                            _labQtdeEntregue.Text = _saldo.QTDE_ENTREGUE.ToString("###,###,###,##0.000");
                            qtdeEstoqueEntregue += _saldo.QTDE_ENTREGUE;
                        }

                        Label _labQtdeConsumida = e.Row.FindControl("labQtdeConsumida") as Label;
                        if (_labQtdeConsumida != null)
                        {
                            _labQtdeConsumida.Text = _saldo.QTDE_CONSUMIDA.ToString("###,###,###,##0.000");
                            qtdeEstoqueConsumida += _saldo.QTDE_CONSUMIDA;
                        }

                        Label _labQtde = e.Row.FindControl("labQtde") as Label;
                        if (_labQtde != null)
                        {
                            _labQtde.Text = _saldo.QTDE_ESTOQUE.ToString("###,###,###,##0.000");
                            qtdeEstoque += _saldo.QTDE_ESTOQUE;
                        }
                    }
                }
            }
        }
        protected void gvEstoque_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvEstoque.FooterRow;
            if (footer != null)
            {
                footer.Cells[1].Text = "Total";
                footer.Cells[1].HorizontalAlign = HorizontalAlign.Left;
                footer.Cells[6].Text = qtdeEstoqueEntregue.ToString("###,###,###,##0.000");
                footer.Cells[7].Text = qtdeEstoqueConsumida.ToString("###,###,###,##0.000");
                footer.Cells[8].Text = qtdeEstoque.ToString("###,###,###,##0.000");
            }
        }
        protected void ibtPesquisarEstoque_Click(object sender, EventArgs e)
        {
            labErroEstoque.Text = "";
            labErro.Text = "";
            try
            {

                if (ddlMaterialGrupo.SelectedValue.Trim() == "" || ddlMaterialSubGrupo.SelectedValue.Trim() == "")
                {
                    labErroEstoque.Text = "Selecione o Grupo e SubGrupo para pesquisar o Estoque.";
                    return;
                }

                CarregarEstoque();
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        #endregion

        #region "NOTAS"
        private List<SP_OBTER_PEDIDOS_ENTRADAResult> ObterRelPedidoEntrada()
        {
            List<SP_OBTER_PEDIDOS_ENTRADAResult> _pedidosNota = new List<SP_OBTER_PEDIDOS_ENTRADAResult>();

            int? numeroPedido = null;
            if (txtPedidoNumeroNota.Text.Trim() != "")
                numeroPedido = Convert.ToInt32(txtPedidoNumeroNota.Text.Trim());

            string grupo = "";
            string subGrupo = "";
            string cor = "";
            List<string> lstCores = new List<string>();
            string corFornecedor = "";
            string fornecedor = "";

            if (rdbFiltroDDL.Checked)
            {
                grupo = ddlMaterialGrupo.SelectedItem.Text.Trim();
                subGrupo = ddlMaterialSubGrupo.SelectedItem.Text.Trim();
                cor = (ddlCor.SelectedItem == null) ? "" : ddlCor.SelectedItem.Text.Trim().ToUpper();
                corFornecedor = ddlCorFornecedor.SelectedValue.Trim();
                fornecedor = ddlFornecedor.SelectedValue.Trim();
            }
            else
            {
                grupo = txtGrupoFiltro.Text.Trim().ToUpper();
                subGrupo = txtSubGrupoFiltro.Text.Trim().ToUpper();
                cor = "";
                corFornecedor = "";
                fornecedor = "";
            }

            _pedidosNota = desenvController.ObterRelPedidosEntrada(numeroPedido, txtNotaFiscal.Text.Trim(), grupo, subGrupo, cor, corFornecedor, fornecedor);

            return _pedidosNota;
        }
        private void CarregarNotasPedido()
        {
            gvPedidosNota.DataSource = ObterRelPedidoEntrada();
            gvPedidosNota.DataBind();
        }
        protected void gvPedidosNota_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_PEDIDOS_ENTRADAResult _pedidoNota = e.Row.DataItem as SP_OBTER_PEDIDOS_ENTRADAResult;

                    colunaPedidoNota += 1;
                    if (_pedidoNota != null)
                    {

                        Literal _litColuna = e.Row.FindControl("litColuna") as Literal;
                        if (_litColuna != null)
                            _litColuna.Text = colunaPedidoNota.ToString();

                        Label _labDataRecebimento = e.Row.FindControl("labDataRecebimento") as Label;
                        if (_labDataRecebimento != null)
                            _labDataRecebimento.Text = _pedidoNota.DATA_RECEBIMENTO.ToString("dd/MM/yyyy");

                        Label _labCor = e.Row.FindControl("labCor") as Label;
                        if (_labCor != null)
                            _labCor.Text = _pedidoNota.COR + " - " + _pedidoNota.DESC_COR;

                        ImageButton btExcluir = e.Row.FindControl("btExcluir") as ImageButton;
                        btExcluir.CommandArgument = _pedidoNota.CODIGO.ToString();

                        if (_pedidoNota.DATA_IMPRESSAO != null)
                            e.Row.ForeColor = Color.Red;
                    }
                }
            }
        }
        protected void gvPedidosNota_DataBound(object sender, EventArgs e)
        {
        }
        protected void ibtPesquisarNotas_Click(object sender, EventArgs e)
        {

            try
            {
                labErro.Text = "";

                if (txtPedidoNumeroNota.Text.Trim() != "" && Convert.ToInt32(txtPedidoNumeroNota.Text.Trim()) < 1000)
                {
                    labErro.Text = "Número do pedido deve ser acima de 1000.";
                    return;
                }

                CarregarNotasPedido();
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        #region "RELATORIO"
        protected void btImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                GerarRelatorio(ObterRelPedidoEntrada());
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        private void GerarRelatorio(List<SP_OBTER_PEDIDOS_ENTRADAResult> _pedidosEntrada)
        {
            StreamWriter wr = null;
            try
            {
                string nomeArquivo = "PEDIDOS_ENTRADA_" + ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO + ".html";
                wr = new StreamWriter(Server.MapPath("") + "\\html\\" + nomeArquivo);
                wr.Write(MontarRelatorioHTML(_pedidosEntrada));
                wr.Flush();

                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "AbrirImpressao('" + nomeArquivo + "')", true);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                wr.Close();
            }
        }
        private StringBuilder MontarRelatorioHTML(List<SP_OBTER_PEDIDOS_ENTRADAResult> _pedidosEntrada)
        {
            StringBuilder _texto = new StringBuilder();

            _texto = MontarCabecalho(_texto);
            _texto = MontarProdutoACortar(_texto, _pedidosEntrada);
            _texto = MontarRodape(_texto);

            return _texto;
        }
        private StringBuilder MontarCabecalho(StringBuilder _texto)
        {
            _texto.Append("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            _texto.Append(" <html>");
            _texto.Append("     <head>");
            _texto.Append("         <title>Notas Fiscais Pedido</title>   ");
            _texto.Append("         <meta charset='UTF-8'>          ");
            _texto.Append("         <style type='text/css'>");
            _texto.Append("             @media print");
            _texto.Append("             {");
            _texto.Append("                 .tdback");
            _texto.Append("                 {");
            _texto.Append("                     background-color: WindowFrame !important;");
            _texto.Append("                     -webkit-print-color-adjust: exact;");
            _texto.Append("                 }");
            _texto.Append("             }");
            _texto.Append("         </style>");
            _texto.Append("     </head>");
            _texto.Append("");
            _texto.Append("<body onLoad='window.print();'>");
            _texto.Append("    <div id='fichaNotaPedido' align='center' style='border: 0px solid #000;'>");
            _texto.Append("        <br />");
            _texto.Append("        <br />");
            _texto.Append("        <div align='center' style='border: 2px solid #000; background-color: transparent;");
            _texto.Append("            width: 517pt;'>");
            _texto.Append("            <h4>");
            _texto.Append("                NOTAS FISCAIS DE PEDIDO - 15/01/2015");
            _texto.Append("            </h4>");

            return _texto;
        }
        private StringBuilder MontarRodape(StringBuilder _texto)
        {
            _texto.Append("            <p>&nbsp;</p>");
            _texto.Append("        </div>");
            _texto.Append("    </div>");
            _texto.Append("</body>");
            _texto.Append("</html>");
            return _texto;
        }
        private StringBuilder MontarProdutoACortar(StringBuilder _texto, List<SP_OBTER_PEDIDOS_ENTRADAResult> _pedidosEntrada)
        {

            if (_pedidosEntrada.Count > 0)
            {
                _texto.Append("            <table border='0' cellpadding='0' cellspacing='0' style='width: 500pt; padding: 0px;");
                _texto.Append("                            color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
                _texto.Append("                            background: white; white-space: nowrap;'>");
                _texto.Append("                <tr style='line-height: 20px;'>");
                _texto.Append("                    <td class='tdback' style='border: 1px solid #000; border-right: none; text-align: center;");
                _texto.Append("                                    background-color: WindowFrame !important; width: 90px;'>");
                _texto.Append("                        NOTA FISCAL");
                _texto.Append("                    </td>");
                _texto.Append("                    <td class='tdback' style='border: 1px solid #000; border-right: none; text-align: left;");
                _texto.Append("                                    background-color: WindowFrame !important;'>");
                _texto.Append("                        FORNECEDOR");
                _texto.Append("                    </td>");
                _texto.Append("                    <td class='tdback' style='border: 1px solid #000; border-right: none; text-align: center;");
                _texto.Append("                                    background-color: WindowFrame !important; width: 75px;'>");
                _texto.Append("                        MATERIAL");
                _texto.Append("                    </td>");
                _texto.Append("                    <td class='tdback' style='border: 1px solid #000; border-right: none; text-align: left; background-color: WindowFrame !important; width: 120px;'>");
                _texto.Append("                        COR");
                _texto.Append("                    </td>");
                _texto.Append("                    <td class='tdback' style='border: 1px solid #000; border-right: none; text-align: center; background-color: WindowFrame !important; width: 75px;'>");
                _texto.Append("                        DATA");
                _texto.Append("                    </td>");
                _texto.Append("                    <td class='tdback' style='border: 1px solid #000; border-right: none; text-align: center; background-color: WindowFrame !important; width: 95px;'>");
                _texto.Append("                        QTDE ENTREGUE");
                _texto.Append("                    </td>");
                _texto.Append("                    <td class='tdback' style='border: 1px solid #000; text-align: center; background-color: WindowFrame !important; width: 75px;'>");
                _texto.Append("                        No. PEDIDO");
                _texto.Append("                    </td>");
                _texto.Append("                </tr>");

                foreach (SP_OBTER_PEDIDOS_ENTRADAResult ppe in _pedidosEntrada)
                {
                    var pedidoQtde = desenvController.ObterPedidoQtde(ppe.CODIGO);
                    if (pedidoQtde != null)
                    {
                        pedidoQtde.DATA_IMPRESSAO = DateTime.Now;
                        desenvController.AtualizarPedidoQtde(pedidoQtde);
                    }

                    _texto.Append("                <tr class='trback' style='line-height: 20px;'>");
                    _texto.Append("                    <td style='border: 1px solid #000; border-right: none; border-top: none; text-align: center;'>");
                    _texto.Append("                        " + ppe.NOTA_FISCAL);
                    _texto.Append("                    </td>");
                    _texto.Append("                    <td style='border: 1px solid #000; border-right: none; border-top: none; text-align: left;'>");
                    _texto.Append("                        " + ppe.FORNECEDOR);
                    _texto.Append("                    </td>");
                    _texto.Append("                    <td style='border: 1px solid #000; border-right: none; border-top: none; text-align: center;'>");
                    _texto.Append("                        " + ppe.MATERIAL);
                    _texto.Append("                    </td>");
                    _texto.Append("                    <td style='border: 1px solid #000; border-right: none; border-top: none; text-align: left;'>");
                    _texto.Append("                        " + ppe.DESC_COR);
                    _texto.Append("                    </td>");
                    _texto.Append("                    <td style='border: 1px solid #000; border-right: none; border-top: none; text-align: center;'>");
                    _texto.Append("                        " + ppe.DATA_RECEBIMENTO.ToString("dd/MM/yyyy"));
                    _texto.Append("                    </td>");
                    _texto.Append("                    <td style='border: 1px solid #000; border-right: none; border-top: none; text-align: center;'>");
                    _texto.Append("                        " + ppe.QTDE);
                    _texto.Append("                    </td>");
                    _texto.Append("                    <td style='border: 1px solid #000; border-top: none; text-align: center;'>");
                    _texto.Append("                        " + ppe.NUMERO_PEDIDO);
                    _texto.Append("                    </td>");
                    _texto.Append("                </tr>");
                }
                _texto.Append("            </table>");

            }
            return _texto;
        }
        #endregion

        #endregion

        #region "VALIDACAO"
        private bool ValidarNumeroPedido(int numeroPedido)
        {
            labNumeroPedido.ForeColor = Color.Gray;
            labNumeroPedido.ToolTip = "";
            labErro.Text = "";
            if (desenvController.ObterPedidoNumero(numeroPedido) != null)
            {
                txtPedidoNumero.Text = "";
                txtPedidoNumero.Focus();
                labNumeroPedido.ForeColor = Color.Red;
                labNumeroPedido.ToolTip = "Número do Pedido já existe. Informe um número novo.";
                labErro.Text = "Número do Pedido já existe. Informe um número novo.";
                return false;
            }

            return true;
        }
        private decimal ValidarGeracaoPedido(out string msg)
        {
            msg = "";

            decimal qtde = 0;
            string produto = "";
            string produtoAux = "";
            foreach (GridViewRow row in gvProdutos.Rows)
            {
                Label _labQtde = row.FindControl("labQtde") as Label;
                Label _labGrupo = row.FindControl("labGrupo") as Label;
                Label _labSubGrupo = row.FindControl("labSubGrupo") as Label;
                Label _labCor = row.FindControl("labCor") as Label;
                Label _labCorFornecedor = row.FindControl("labCorFornecedor") as Label;

                CheckBox chkQtde = row.FindControl("chkMarcar") as CheckBox;
                if (chkQtde != null && chkQtde.Checked)
                {
                    if (_labQtde != null)
                    {
                        decimal qtdeAux = 0;
                        if (decimal.TryParse(_labQtde.Text, out qtdeAux))
                            qtde += qtdeAux;
                    }
                }

                if (_labGrupo != null && _labSubGrupo != null && _labCor != null && _labCorFornecedor != null)
                {
                    produtoAux = _labGrupo.Text.Trim() + _labSubGrupo.Text.Trim() + _labCor.Text.Trim() + _labCorFornecedor.Text.Trim();
                    if (produto == "")
                    {
                        produto = produtoAux;
                        hidMaterialGrupoPedido.Value = _labGrupo.Text;
                        hidMaterialSubGrupoPedido.Value = _labSubGrupo.Text;
                        hidCorPedido.Value = _labCor.Text;
                        hidCorFornecedorPedido.Value = _labCorFornecedor.Text;
                    }
                    else if (produto == produtoAux)
                    {
                        produto = produtoAux;
                        hidMaterialGrupoPedido.Value = _labGrupo.Text;
                        hidMaterialSubGrupoPedido.Value = _labSubGrupo.Text;
                        hidCorPedido.Value = _labCor.Text;
                        hidCorFornecedorPedido.Value = _labCorFornecedor.Text;
                    }
                    else
                    {
                        produto = "";
                        hidMaterialGrupoPedido.Value = "";
                        hidMaterialSubGrupoPedido.Value = "";
                        hidCorPedido.Value = "";
                        hidCorFornecedorPedido.Value = "";
                        break;
                    }
                }
            }

            if (produto.Trim() == "")
            {
                msg = "Não será possível gerar o Pedido. Foram selecionados Materiais diferentes.";
                return 0;
            }

            if (qtde <= 0)
            {
                qtde = Convert.ToDecimal(labQtdeNecessidadeCompra.Text);
                if (qtde <= 0)
                {
                    msg = "Não será possível gerar o Pedido. Quantidade deve ser maior que Zero.";
                    return 0;
                }
            }

            return qtde;
        }
        private bool ValidarCampos()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            labNumeroPedido.ForeColor = _OK;
            if (txtPedidoNumero.Text.Trim() == "")
            {
                labNumeroPedido.ForeColor = _notOK;
                retorno = false;
            }

            labColecao.ForeColor = _OK;
            if (ddlColecoes.SelectedValue.Trim() == "")
            {
                labColecao.ForeColor = _notOK;
                retorno = false;
            }

            labFornecedorPedido.ForeColor = _OK;
            if (ddlPedidoFornecedor.SelectedValue.Trim() == "")
            {
                labFornecedorPedido.ForeColor = _notOK;
                retorno = false;
            }

            labMaterialGrupoPedido.ForeColor = _OK;
            if (ddlMaterialGrupoPedido.SelectedValue.Trim() == "")
            {
                labMaterialGrupoPedido.ForeColor = _notOK;
                retorno = false;
            }

            labMaterialSubGrupoPedido.ForeColor = _OK;
            if (ddlMaterialSubGrupoPedido.SelectedValue.Trim() == "")
            {
                labMaterialSubGrupoPedido.ForeColor = _notOK;
                retorno = false;
            }

            labStatus.ForeColor = _OK;
            if (ddlStatus.SelectedValue.Trim() == "")
            {
                labStatus.ForeColor = _notOK;
                retorno = false;
            }

            labCorPedido.ForeColor = _OK;
            if (ddlCorPedido.SelectedValue.Trim() == "")
            {
                labCorPedido.ForeColor = _notOK;
                retorno = false;
            }

            labCorFornecedorPedido.ForeColor = _OK;
            if (ddlCorFornecedorPedido.SelectedValue.Trim() == "")
            {
                labCorFornecedorPedido.ForeColor = _notOK;
                retorno = false;
            }

            labQuantidade.ForeColor = _OK;
            decimal qtdeVal = 0;
            if (txtQtde.Text.Trim() == "" || !Decimal.TryParse(txtQtde.Text, out qtdeVal))
            {
                labQuantidade.ForeColor = _notOK;
                retorno = false;
            }

            labPreco.ForeColor = _OK;
            if (txtPreco.Text.Trim() == "")
            {
                labPreco.ForeColor = _notOK;
                retorno = false;
            }

            labCondicaoPgto.ForeColor = _OK;
            if (ddlCondicaoPgto.SelectedValue.Trim() == "")
            {
                labCondicaoPgto.ForeColor = _notOK;
                retorno = false;
            }

            labDataPedido.ForeColor = _OK;
            if (txtPedidoData.Text.Trim() == "")
            {
                labDataPedido.ForeColor = _notOK;
                retorno = false;
            }

            labDataReserva.ForeColor = _OK;
            labDataPrevisaoEntrega.ForeColor = _OK;
            if (ddlStatus.SelectedValue == "A" || ddlStatus.SelectedValue == "B")
                if (txtPedidoPrevEntrega.Text.Trim() == "")
                {
                    labDataPrevisaoEntrega.ForeColor = _notOK;
                    retorno = false;
                }
            if (ddlStatus.SelectedValue == "R")
                if (txtPedidoDataReserva.Text.Trim() == "")
                {
                    labDataReserva.ForeColor = _notOK;
                    retorno = false;
                }

            labUnidadeMedida.ForeColor = _OK;
            if (ddlUnidade.SelectedValue.Trim() == "" || ddlUnidade.SelectedValue.Trim() == "0")
            {
                labUnidadeMedida.ForeColor = _notOK;
                retorno = false;
            }

            labEmail.ForeColor = _OK;
            labCorpoEmail.ForeColor = _OK;
            if (chkEmail.Checked)
            {
                if (txtCorpoEmail.Text.Trim() == "")
                {
                    labCorpoEmail.ForeColor = _notOK;
                    retorno = false;
                }

                if (txtEmail.Text.Trim() == "")
                {
                    labEmail.ForeColor = _notOK;
                    retorno = false;
                }

                if (txtEmail.Text.Trim().Contains(","))
                    txtEmail.Text = txtEmail.Text.Trim().Replace(",", ";");

                if (!Utils.WebControls.ValidarEmail(txtEmail.Text.Trim()))
                {
                    labEmail.ForeColor = _notOK;
                    retorno = false;
                }
            }

            return retorno;
        }
        private void HabilitarCampos(bool enable)
        {

            //PEDIDOS
            txtPedidoNumero.Enabled = enable;
            ddlColecoes.Enabled = enable;
            ddlFornecedor.Enabled = enable;
            ddlMaterialGrupoPedido.Enabled = enable;
            ddlMaterialSubGrupoPedido.Enabled = enable;
            ddlStatus.Enabled = enable;
            ddlCorPedido.Enabled = enable;
            ddlCorFornecedorPedido.Enabled = enable;
            txtQtde.Enabled = enable;
            txtPreco.Enabled = enable;
            ddlCondicaoPgto.Enabled = enable;
            txtPedidoData.Enabled = enable;
            txtPedidoPrevEntrega.Enabled = enable;
            txtPedidoDataReserva.Enabled = enable;
            btSalvarPedido.Visible = enable;
            btCancelarPedido.Visible = enable;

            foreach (GridViewRow row in gvPedidos.Rows)
                if (row != null)
                {
                    string status = "";
                    Label _labStatus = row.FindControl("labStatus") as Label;
                    if (_labStatus != null)
                        status = _labStatus.Text;


                    ImageButton _btEditarPedido = row.FindControl("btEditarPedido") as ImageButton;
                    if (_btEditarPedido != null)
                        _btEditarPedido.Visible = enable;
                    ImageButton _btExcluirPedido = row.FindControl("btExcluirPedido") as ImageButton;
                    if (_btExcluirPedido != null)
                    {
                        if (status.Trim() != "EXCLUÍDO")
                            _btExcluirPedido.Visible = enable;
                    }
                }

            GridViewRow footer = gvPedidos.FooterRow;
            if (footer != null)
            {
                ImageButton _btIncluirPedido = footer.FindControl("btIncluirPedido") as ImageButton;
                if (_btIncluirPedido != null)
                    _btIncluirPedido.Visible = enable;
            }

        }
        private void LimparCampos(bool _cabecalho)
        {
            labErro.Text = "";
            hidCodigoPedido.Value = "";

            txtPedidoNumero.Text = "";
            ddlPedidoFornecedor.SelectedValue = "";
            ddlMaterialGrupoPedido.SelectedValue = "";
            ddlMaterialSubGrupoPedido.SelectedValue = "";
            ddlCorPedido.SelectedValue = "";
            ddlCorFornecedorPedido.Text = "";
            txtQtde.Text = "";
            txtPreco.Text = "";
            ddlCondicaoPgto.SelectedValue = "";
            txtCondicaoPgtoOutro.Text = "";
            txtPedidoData.Text = "";
            txtPedidoPrevEntrega.Text = "";
            txtPedidoDataReserva.Text = "";
            ddlUnidade.SelectedValue = "0";
            txtCorpoEmail.Text = "";
            txtEmail.Text = "";
            gvSubPedido.DataSource = new List<DESENV_PEDIDO_SUB>();
            gvSubPedido.DataBind();

            if (_cabecalho)
            {
                ddlMaterialGrupo.SelectedValue = "";
                ddlMaterialGrupo_SelectedIndexChanged(ddlMaterialGrupo, null);
                ddlMaterialSubGrupo.SelectedValue = "";
                ddlFornecedor.SelectedValue = "";
                ddlCor.SelectedValue = "";
                ddlCorFornecedor.SelectedValue = "";

                CarregarFornecedores();
                CarregarCores(ddlMaterialGrupo.SelectedItem.Text.Trim(), ddlMaterialSubGrupo.SelectedItem.Text.Trim());
            }

        }
        #endregion

        protected void btExcluir_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                labErro.Text = "";

                ImageButton bt = (ImageButton)sender;

                string codigo = bt.CommandArgument;

                desenvController.ExcluirPedidoQtde(Convert.ToInt32(codigo));
                CarregarNotasPedido();
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        #region "SUBPEDIDO"
        private void CarregarSubPedidos(int codigoDesenvPedido)
        {
            var subPedido = desenvController.ObterPedidoSubPorDesenvPedido(codigoDesenvPedido);

            gvSubPedido.DataSource = subPedido;
            gvSubPedido.DataBind();
        }
        protected void gvSubPedido_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    DESENV_PEDIDO_SUB subPedido = e.Row.DataItem as DESENV_PEDIDO_SUB;

                    Label labNumeroSubPedido = e.Row.FindControl("labNumeroSubPedido") as Label;
                    labNumeroSubPedido.Text = subPedido.DESENV_PEDIDO1.NUMERO_PEDIDO.ToString() + "-" + subPedido.CODIGO.ToString();

                    Label labFornecedor = e.Row.FindControl("labFornecedor") as Label;
                    labFornecedor.Text = subPedido.FORNECEDOR;

                    Label labQtde = e.Row.FindControl("labQtde") as Label;
                    labQtde.Text = subPedido.QTDE.ToString();

                    Label labCusto = e.Row.FindControl("labCusto") as Label;
                    labCusto.Text = subPedido.VALOR.ToString();

                    Label labDataPedido = e.Row.FindControl("labDataPedido") as Label;
                    labDataPedido.Text = Convert.ToDateTime(subPedido.DATA_PEDIDO).ToString("dd/MM/yyyy");

                    Label labDataPedidoPrevisao = e.Row.FindControl("labDataPedidoPrevisao") as Label;
                    labDataPedidoPrevisao.Text = (subPedido.DATA_ENTREGA_PREV == null) ? "" : Convert.ToDateTime(subPedido.DATA_ENTREGA_PREV).ToString("dd/MM/yyyy");

                    ImageButton btExcluirSubPedido = e.Row.FindControl("btExcluirSubPedido") as ImageButton;
                    btExcluirSubPedido.CommandArgument = subPedido.CODIGO.ToString();


                    qtdeSubPedido += Convert.ToDecimal(subPedido.QTDE);
                }
            }
        }
        protected void gvSubPedido_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvSubPedido.FooterRow;
            if (footer != null)
            {
                footer.Cells[1].Text = "Total";

                footer.Cells[3].Text = qtdeSubPedido.ToString();
                txtQtde.Text = qtdeSubPedido.ToString();
            }
        }
        protected void btExcluirSubPedido_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton bt = (ImageButton)sender;

                string codigoSubPedido = bt.CommandArgument;

                var subPedido = desenvController.ObterPedidoSub(Convert.ToInt32(codigoSubPedido));
                if (subPedido == null)
                {
                    labErro.Text = "SubPedido não foi encontrado.";
                    return;
                }

                var pedido = desenvController.ObterPedido(subPedido.DESENV_PEDIDO);
                if (pedido == null)
                {
                    labErro.Text = "Pedido principal não foi encontrado.";
                    return;
                }

                pedido.QTDE = pedido.QTDE - Convert.ToDecimal(subPedido.QTDE);

                desenvController.ExcluirPedidoSub(Convert.ToInt32(codigoSubPedido));
                desenvController.AtualizarPedido(pedido);

                txtQtde.Text = pedido.QTDE.ToString();

                CarregarSubPedidos(Convert.ToInt32(hidCodigoPedido.Value));

            }
            catch (Exception)
            {

                throw;
            }
        }
        protected void btAtualizarSubPedido_Click(object sender, EventArgs e)
        {
            if (hidCodigoPedido.Value != "")
            {
                CarregarSubPedidos(Convert.ToInt32(hidCodigoPedido.Value));
            }
        }
        #endregion



    }
}
