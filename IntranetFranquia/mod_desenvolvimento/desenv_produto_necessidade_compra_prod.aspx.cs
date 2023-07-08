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

namespace Relatorios
{
    public partial class desenv_produto_necessidade_compra_prod : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();

        List<SP_OBTER_MATERIAL_PRODUTOResult> lstMaterialProduto = null;
        List<MaterialEntity> lstPedidosTotais = new List<MaterialEntity>();
        List<MaterialEntity> lstProdutosTotais = new List<MaterialEntity>();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                CarregarColecoes();
                CarregarCores();
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#tabs').tabs(); });", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$('#tabs').tabs('option', 'active', '" + ((hidTabSelected.Value == "") ? "0" : hidTabSelected.Value) + "');", true);

            //Evitar duplo clique no botão
            ibtPesquisar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(ibtPesquisar, null) + ";");
            btAdicionar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btAdicionar, null) + ";");

        }

        #region "DADOS INICIAIS"
        private void CarregarColecoes()
        {
            BaseController _base = new BaseController();
            List<COLECOE> _colecoes = _base.BuscaColecoes();
            if (_colecoes != null)
            {
                _colecoes.Insert(0, new COLECOE { COLECAO = "", DESC_COLECAO = "Selecione" });
                ddlColecoes.DataSource = _colecoes;
                ddlColecoes.DataBind();

                if (Session["COLECAO"] != null)
                    ddlColecoes.SelectedValue = Session["COLECAO"].ToString();
            }
        }
        private void CarregarCores()
        {
            var _coresBasicas = ObterCoresBasicas();

            _coresBasicas.Insert(0, new CORES_BASICA { COR = "", DESC_COR = "" });
            ddlCor.DataSource = _coresBasicas;
            ddlCor.DataBind();

            if (_coresBasicas.Count == 2)
                ddlCor.SelectedIndex = 1;

        }
        private List<CORES_BASICA> ObterCoresBasicas()
        {
            List<CORES_BASICA> _cores = new List<CORES_BASICA>();
            _cores = new ProducaoController().ObterCoresBasicas();
            return _cores;
        }
        #endregion

        #region "AÇÕES DA TELA"
        private void MoverAba(string vAba)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$('#tabs').tabs('option', 'active', '" + vAba + "');", true);
            hidTabSelected.Value = vAba;
        }
        #endregion

        #region "MATERIAIS"
        private List<SP_OBTER_MATERIAL_PRODUTOResult> ObterNecessidadeCompraMaterial(string colecao, string produto, string nome, string cor)
        {
            List<SP_OBTER_MATERIAL_PRODUTOResult> _materialProduto = new List<SP_OBTER_MATERIAL_PRODUTOResult>();

            _materialProduto = desenvController.ObterMaterialProduto(
                produto,
                cor,
                nome,
                "",
                "",
                "",
                "",
                "").Where(p => p.COLECAO.Trim() == colecao.Trim()).ToList();

            return _materialProduto;
        }
        private void CarregarMateriais()
        {
            lstMaterialProduto = new List<SP_OBTER_MATERIAL_PRODUTOResult>();

            DESENV_PRODUTO produto = null;
            foreach (ListItem item in lstModeloSelecionado.Items)
            {
                if (item != null)
                {
                    produto = desenvController.ObterProduto(Convert.ToInt32(item.Value));
                    if (produto != null)
                        lstMaterialProduto.AddRange(ObterNecessidadeCompraMaterial(produto.COLECAO, produto.MODELO, "", produto.COR));
                }
            }

            if (lstMaterialProduto == null || lstMaterialProduto.Count() <= 0)
                labErro.Text = "Nenhum Material encontrado para os Produtos selecionados. Refaça sua pesquisa.";

            gvProdutos.DataSource = lstMaterialProduto.OrderBy(p => p.GRUPO).ThenBy(k => k.SUBGRUPO).ThenBy(k => k.DESC_COR_MATERIAL).ThenBy(k => k.COR_FORNECEDOR).ThenBy(k => k.MODELO);
            gvProdutos.DataBind();
        }
        protected void gvProdutos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_MATERIAL_PRODUTOResult _produto = e.Row.DataItem as SP_OBTER_MATERIAL_PRODUTOResult;

                    if (_produto != null)
                    {

                        if (_produto.CODIGO > 0)
                        {

                            Label _labFornecedor = e.Row.FindControl("labFornecedor") as Label;
                            if (_labFornecedor != null)
                                _labFornecedor.Text = (_produto.FORNECEDOR == null) ? "" : _produto.FORNECEDOR.Trim().Substring(0, ((_produto.FORNECEDOR.Trim().Length < 18) ? (_produto.FORNECEDOR.Trim().Length) : 17));

                            decimal qtde = 0;
                            Label _labQtde = e.Row.FindControl("labQtde") as Label;
                            if (_labQtde != null)
                            {
                                qtde = (_produto.QTDE * _produto.CONSUMO);
                                _labQtde.Text = qtde.ToString();
                                if (_produto.QTDE < 0)
                                    _labQtde.ForeColor = Color.Red;
                            }

                            lstProdutosTotais.Add(new MaterialEntity
                            {
                                GRUPO = _produto.GRUPO,
                                SUBGRUPO = _produto.SUBGRUPO,
                                COR = _produto.COR_MATERIAL,
                                COR_FORNECEDOR = _produto.COR_FORNECEDOR,
                                QTDE = qtde
                            });

                        }
                    }
                }
            }
        }
        private List<MaterialEntity> AgruparProduto(List<MaterialEntity> _lstProdutosTotais)
        {
            var totalProduto = _lstProdutosTotais.GroupBy(p => new
            {
                GRUPO = p.GRUPO.Trim(),
                SUBGRUPO = p.SUBGRUPO.Trim(),
                COR = p.COR.Trim(),
                COR_FORNECEDOR = p.COR_FORNECEDOR.Trim()
            }).Select(x => new MaterialEntity
            {
                GRUPO = x.Key.GRUPO,
                SUBGRUPO = x.Key.SUBGRUPO,
                COR = x.Key.COR,
                COR_FORNECEDOR = x.Key.COR_FORNECEDOR,
                QTDE = x.Sum(o => o.QTDE)
            }).ToList();

            return totalProduto;
        }
        protected void gvProdutos_DataBound(object sender, EventArgs e)
        {
            if (lstProdutosTotais != null && lstProdutosTotais.Count() > 0)
            {
                gvProdutoTotais.DataSource = AgruparProduto(lstProdutosTotais).OrderBy(p => p.GRUPO).ThenBy(p => p.SUBGRUPO).ThenBy(p => p.COR).ThenBy(p => p.COR_FORNECEDOR);
                gvProdutoTotais.DataBind();
            }
        }
        protected void gvProdutoTotais_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    MaterialEntity _produto = e.Row.DataItem as MaterialEntity;

                    if (_produto != null)
                    {
                        Label _labGrupo = e.Row.FindControl("labGrupo") as Label;
                        if (_labGrupo != null)
                            _labGrupo.Text = _produto.GRUPO;

                        Label _labSubGrupo = e.Row.FindControl("labSubGrupo") as Label;
                        if (_labSubGrupo != null)
                            _labSubGrupo.Text = _produto.SUBGRUPO;

                        Label _labCor = e.Row.FindControl("labCor") as Label;
                        if (_labCor != null)
                        {
                            var _cor = prodController.ObterCoresBasicas(_produto.COR);
                            _labCor.Text = (_cor == null) ? _produto.COR : _cor.DESC_COR;
                        }

                        Label _labCorFornecedor = e.Row.FindControl("labCorFornecedor") as Label;
                        if (_labCorFornecedor != null)
                            _labCorFornecedor.Text = _produto.COR_FORNECEDOR;

                        Label _labQtde = e.Row.FindControl("labQtde") as Label;
                        if (_labQtde != null)
                            _labQtde.Text = _produto.QTDE.ToString("###,###,###,##0.000");
                    }
                }
            }
        }

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
        private void CarregarPedidos(List<SP_OBTER_MATERIAL_PRODUTOResult> _materialProduto)
        {
            List<DESENV_PEDIDO> lstPedidos = new List<DESENV_PEDIDO>();
            lstPedidos = ObterPedidos("T");

            lstPedidos = lstPedidos.Where(p => _materialProduto.Any(x =>
                                                                        x.GRUPO.Trim() == p.GRUPO.Trim() &&
                                                                        x.SUBGRUPO.Trim() == p.SUBGRUPO.Trim() &&
                                                                        x.COR_MATERIAL.Trim() == p.COR.Trim() &&
                                                                        x.COR_FORNECEDOR.Trim() == p.COR_FORNECEDOR.Trim()
                                                                        )).ToList();

            if (lstPedidos == null || lstPedidos.Count <= 0)
                lstPedidos.Add(new DESENV_PEDIDO { CODIGO = 0, NUMERO_PEDIDO = 0 });

            gvPedidoProduto.DataSource = lstPedidos.OrderBy(p => p.GRUPO).ThenBy(p => p.SUBGRUPO).ThenBy(p => p.COR).ThenBy(p => p.COR_FORNECEDOR); ;
            gvPedidoProduto.DataBind();
        }
        protected void gvPedidoProduto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    DESENV_PEDIDO _pedido = e.Row.DataItem as DESENV_PEDIDO;

                    if (_pedido != null)
                    {
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
                                _labQtde.Text = _pedido.QTDE.ToString("###,###,###,##0.000");

                            Label _labQtdeEntregue = e.Row.FindControl("labQtdeEntregue") as Label;
                            decimal qtdeEntregue = 0;
                            if (_labQtdeEntregue != null)
                            {
                                List<DESENV_PEDIDO_QTDE> _lstPedidoQtde = desenvController.ObterPedidoQtdePedido(_pedido.CODIGO);
                                var vEntregue = _lstPedidoQtde.GroupBy(g => g.DESENV_PEDIDO).Select(i => new { QTDE = i.Sum(w => w.QTDE) }).SingleOrDefault();
                                if (vEntregue != null)
                                {
                                    qtdeEntregue = vEntregue.QTDE;
                                    _labQtdeEntregue.Text = qtdeEntregue.ToString("######0.000");
                                }
                                else
                                {
                                    _labQtdeEntregue.Text = (0).ToString("######0.000");
                                }
                            }

                            lstPedidosTotais.Add(new MaterialEntity
                            {
                                GRUPO = _pedido.GRUPO,
                                SUBGRUPO = _pedido.SUBGRUPO,
                                COR = _pedido.COR,
                                COR_FORNECEDOR = _pedido.COR_FORNECEDOR,
                                QTDE = _pedido.QTDE,
                                QTDE_ENTREGUE = qtdeEntregue
                            });

                        }
                    }
                }
            }

        }
        private List<MaterialEntity> AgruparPedido(List<MaterialEntity> _lstPedidosTotais)
        {
            var totalPedido = _lstPedidosTotais.GroupBy(p => new
            {
                GRUPO = p.GRUPO.Trim(),
                SUBGRUPO = p.SUBGRUPO.Trim(),
                COR = p.COR.Trim(),
                COR_FORNECEDOR = p.COR_FORNECEDOR.Trim()
            }).Select(x => new MaterialEntity
            {
                GRUPO = x.Key.GRUPO,
                SUBGRUPO = x.Key.SUBGRUPO,
                COR = x.Key.COR,
                COR_FORNECEDOR = x.Key.COR_FORNECEDOR,
                QTDE = x.Sum(o => o.QTDE),
                QTDE_ENTREGUE = x.Sum(o => o.QTDE_ENTREGUE)
            }).ToList();

            return totalPedido;
        }
        protected void gvPedidoProduto_DataBound(object sender, EventArgs e)
        {
            if (lstPedidosTotais != null && lstPedidosTotais.Count() > 0)
            {
                gvPedidoTotais.DataSource = AgruparPedido(lstPedidosTotais).OrderBy(p => p.GRUPO).ThenBy(p => p.SUBGRUPO).ThenBy(p => p.COR).ThenBy(p => p.COR_FORNECEDOR);
                gvPedidoTotais.DataBind();
            }
        }
        protected void gvPedidoTotais_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    MaterialEntity _pedido = e.Row.DataItem as MaterialEntity;

                    if (_pedido != null)
                    {
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
                            _labQtde.Text = _pedido.QTDE.ToString("###,###,###,##0.000");

                        Label _labQtdeEntregue = e.Row.FindControl("labQtdeEntregue") as Label;
                        if (_labQtdeEntregue != null)
                            _labQtdeEntregue.Text = _pedido.QTDE_ENTREGUE.ToString("###,###,###,##0.000");
                    }
                }
            }
        }

        private void CarregarNecessidadeCompra(List<MaterialEntity> _pedidos, List<MaterialEntity> _produtos)
        {
            List<MaterialEntity> lstNecessidadeCompra = new List<MaterialEntity>();

            var _lstProdutos = AgruparProduto(_produtos);
            var _lstPedidos = AgruparPedido(_pedidos);

            decimal qtdePedido = 0;
            decimal qtdeProduto = 0;
            decimal qtdeNecessidadeCompra = 0;
            decimal qtdeSobra = 0;
            decimal qtdeCompra = 0;

            //string cor = "";

            foreach (MaterialEntity m in _lstProdutos)
            {
                var ped = _lstPedidos.Where(p => p.GRUPO.Trim() == m.GRUPO.Trim() &&
                                                          p.SUBGRUPO.Trim() == m.SUBGRUPO.Trim() &&
                                                          p.COR.Trim() == m.COR.Trim() &&
                                                          p.COR_FORNECEDOR.Trim() == m.COR_FORNECEDOR.Trim());
                if (ped != null && ped.Count() > 0)
                    qtdePedido = ped.Sum(p => p.QTDE);

                //cor = prodController.ObterCoresBasicas(m.COR).DESC_COR.Trim();

                var prd = desenvController.ObterMaterialProduto("", "", "", m.GRUPO, m.SUBGRUPO, "", m.COR, m.COR_FORNECEDOR);
                if (prd != null && prd.Count() > 0)
                    qtdeProduto = prd.Sum(p => p.QTDE * p.CONSUMO);

                qtdeNecessidadeCompra = (qtdePedido - qtdeProduto);

                if (qtdeNecessidadeCompra >= 0)
                {
                    qtdeSobra = qtdeNecessidadeCompra;
                    qtdeCompra = 0;
                }
                else
                {
                    qtdeSobra = 0;
                    qtdeCompra = (qtdeNecessidadeCompra * (-1));
                }

                lstNecessidadeCompra.Add(new MaterialEntity
                {
                    GRUPO = m.GRUPO,
                    SUBGRUPO = m.SUBGRUPO,
                    COR = m.COR,
                    COR_FORNECEDOR = m.COR_FORNECEDOR,
                    QTDE_PRODUTO = qtdeProduto,
                    QTDE_PEDIDO = qtdePedido,
                    QTDE_NECESSIDADE = qtdeCompra,
                    QTDE_SOBRA = qtdeSobra
                });

                qtdePedido = 0;
                qtdeProduto = 0;
                qtdeNecessidadeCompra = 0;
                qtdeSobra = 0;
                qtdeCompra = 0;
            }

            gvNecessidadeCompra.DataSource = lstNecessidadeCompra;
            gvNecessidadeCompra.DataBind();
        }
        protected void gvNecessidadeCompra_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    MaterialEntity _material = e.Row.DataItem as MaterialEntity;

                    if (_material != null)
                    {
                        Label _labGrupo = e.Row.FindControl("labGrupo") as Label;
                        if (_labGrupo != null)
                            _labGrupo.Text = _material.GRUPO;

                        Label _labSubGrupo = e.Row.FindControl("labSubGrupo") as Label;
                        if (_labSubGrupo != null)
                            _labSubGrupo.Text = _material.SUBGRUPO;

                        Label _labCor = e.Row.FindControl("labCor") as Label;
                        if (_labCor != null)
                        {
                            var _cor = prodController.ObterCoresBasicas(_material.COR);
                            _labCor.Text = (_cor == null) ? _material.COR : _cor.DESC_COR;
                        }

                        Label _labCorFornecedor = e.Row.FindControl("labCorFornecedor") as Label;
                        if (_labCorFornecedor != null)
                            _labCorFornecedor.Text = _material.COR_FORNECEDOR;

                        Label _labQtdePedido = e.Row.FindControl("labQtdePedido") as Label;
                        if (_labQtdePedido != null)
                            _labQtdePedido.Text = _material.QTDE_PEDIDO.ToString("###,###,##0.000");

                        Label _labQtdeProduto = e.Row.FindControl("labQtdeProduto") as Label;
                        if (_labQtdeProduto != null)
                            _labQtdeProduto.Text = _material.QTDE_PRODUTO.ToString("###,###,##0.000");

                        Label _labNecessidadeCompra = e.Row.FindControl("labNecessidadeCompra") as Label;
                        if (_labNecessidadeCompra != null)
                        {
                            _labNecessidadeCompra.Text = _material.QTDE_NECESSIDADE.ToString("###,###,##0.000");

                            if (_material.QTDE_NECESSIDADE > 0)
                                _labNecessidadeCompra.ForeColor = Color.Red;
                        }

                        Label _labQtdeSobra = e.Row.FindControl("labQtdeSobra") as Label;
                        if (_labQtdeSobra != null)
                            _labQtdeSobra.Text = _material.QTDE_SOBRA.ToString("###,###,##0.000");

                    }
                }
            }

        }

        protected void ibtPesquisar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (lstModeloSelecionado.Items.Count <= 0)
                {
                    labErro.Text = "Adicione produtos na Lista para pesquisar.";
                    return;
                }

                CarregarMateriais();
                CarregarPedidos(lstMaterialProduto);
                CarregarNecessidadeCompra(lstPedidosTotais, lstProdutosTotais);
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        #endregion

        #region "CONTROLES LISTA"
        protected void btAdicionar_Click(object sender, EventArgs e)
        {
            try
            {

                labErro.Text = "";

                if (ddlColecoes.SelectedValue.Trim() == "")
                {
                    labErro.Text = "Selecione a Coleção.";
                    return;
                }

                if (txtProduto.Text.Trim() == "" && txtNome.Text.Trim() == "")
                {
                    labErro.Text = "Informe o Produto e/ou Nome.";
                    return;
                }

                if (ddlCor.SelectedValue.Trim() == "" || ddlCor.SelectedValue.Trim() == "0")
                {
                    labErro.Text = "Selecione a Cor do Produto.";
                    return;
                }

                List<DESENV_PRODUTO> listaProdutos = new List<DESENV_PRODUTO>();

                //Obter apenas produtos que tenham modelos e estão ativos
                listaProdutos = desenvController.ObterProduto().Where(p => p.MODELO.Trim() != ""
                    && p.GRUPO.Trim() != ""
                    && p.STATUS == 'A'
                    //&& p.PRODUTO_ACABADO == 'N'
                    ).ToList();

                //Filtrar por Colecoes
                if (ddlColecoes.SelectedValue.Trim() != "0" && ddlColecoes.SelectedValue.Trim() != "")
                    listaProdutos = listaProdutos.Where(p => p.COLECAO.Trim() == ddlColecoes.SelectedValue.Trim()).ToList();

                //Filtrar por Modelo
                if (txtProduto.Text.Trim() != "")
                    listaProdutos = listaProdutos.Where(p => p.MODELO.Trim().ToUpper().Contains(txtProduto.Text.Trim().ToUpper())).ToList();

                //Filtrar por nome
                if (txtNome.Text.Trim() != "")
                {
                    var produtoFiltro = new BaseController().BuscaProdutosDescricao(txtNome.Text.ToUpper().Trim());
                    listaProdutos = listaProdutos.Where(p => produtoFiltro.Any(x => x.PRODUTO1.Trim() == p.MODELO.Trim())).ToList();
                }

                //Filtrar por Cores
                if (ddlCor.SelectedValue.Trim() != "0" && ddlCor.SelectedValue.Trim() != "")
                    listaProdutos = listaProdutos.Where(p => p.COR.Trim() == ddlCor.SelectedValue.Trim()).ToList();

                if (listaProdutos == null || listaProdutos.Count() <= 0)
                {
                    labErro.Text = "Nenhum Produto encontrado. Refaça sua pesquisa.";
                    return;
                }

                //VALIDACAO DE FICHA TECNICA
                foreach (DESENV_PRODUTO produto in listaProdutos)
                {
                    if (produto.DATA_FICHATECNICA == null)
                    {
                        labErro.Text = "Ficha Técnica não Aprovada.";
                        return;
                    }

                    var fichaTecnica = desenvController.ObterFichaTecnica(produto.CODIGO);
                    if (fichaTecnica == null || fichaTecnica.Count() <= 0)
                    {
                        labErro.Text = "Produto não possui Ficha Técnica.";
                        return;
                    }
                }

                ListItem item = null;
                string _nome = "";
                foreach (DESENV_PRODUTO produto in listaProdutos)
                {
                    item = lstModeloSelecionado.Items.FindByValue(produto.CODIGO.ToString());
                    if (item == null)
                    {
                        item = new ListItem();
                        item.Value = produto.CODIGO.ToString();
                        var produtoLinx = baseController.BuscaProduto(produto.MODELO);
                        if (produtoLinx != null)
                            _nome = produtoLinx.DESC_PRODUTO.Trim();

                        var cor = prodController.ObterCoresBasicas(produto.COR).DESC_COR.Trim();

                        item.Text = produto.MODELO.Trim() + ((_nome == "") ? "" : (" - " + _nome)) + " - " + cor;
                        lstModeloSelecionado.Items.Add(item);
                        _nome = "";
                    }
                }
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void btExcluir_Click(object sender, EventArgs e)
        {
            foreach (int indexSelected in lstModeloSelecionado.GetSelectedIndices())
            {
                lstModeloSelecionado.Items.RemoveAt(indexSelected);
            }
        }
        protected void btLimpar_Click(object sender, EventArgs e)
        {
            lstModeloSelecionado.Items.Clear();
        }
        #endregion

        #region "VALIDACAO"
        private bool ValidarCampos()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            return retorno;
        }
        private void LimparCampos(bool _cabecalho)
        {
            labErro.Text = "";
        }
        #endregion
    }
}