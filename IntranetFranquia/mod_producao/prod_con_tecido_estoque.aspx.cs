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
using Relatorios.mod_desenvolvimento.relatorios;

namespace Relatorios
{
    public partial class prod_con_tecido_estoque : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();

        decimal qtdeEntrada = 0;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                CarregarGrupos();

                string tela = Request.QueryString["t"].ToString();
                if (tela != "1" && tela != "2")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                if (tela == "1")
                    hrefVoltar.HRef = "../mod_desenvolvimento/desenv_menu.aspx";

                if (tela == "2")
                    hrefVoltar.HRef = "prod_menu.aspx";

            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#tabs').tabs(); });", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$('#tabs').tabs('option', 'active', '" + ((hidTabSelected.Value == "") ? "0" : hidTabSelected.Value) + "');", true);

            //Evitar duplo clique no botão
            ibtPesquisar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(ibtPesquisar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarGrupos()
        {
            List<MATERIAIS_GRUPO> _matGrupo = desenvController.ObterMaterialGrupo();
            if (_matGrupo != null)
            {
                _matGrupo.Insert(0, new MATERIAIS_GRUPO { CODIGO_GRUPO = "", GRUPO = "Selecione" });
                ddlMaterialGrupo.DataSource = _matGrupo;
                ddlMaterialGrupo.DataBind();
            }
        }
        protected void ddlMaterialGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                CarregarSubGrupos(ddlMaterialGrupo.SelectedItem.Text.Trim());
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        private void CarregarSubGrupos(string grupo)
        {
            List<MATERIAIS_SUBGRUPO> _matSubGrupo = null;
            _matSubGrupo = desenvController.ObterMaterialSubGrupo().Where(p => p.GRUPO.Trim() == grupo.Trim()).ToList();

            if (_matSubGrupo != null)
            {
                _matSubGrupo = _matSubGrupo.OrderBy(p => p.SUBGRUPO.Trim()).ToList();
                _matSubGrupo.Insert(0, new MATERIAIS_SUBGRUPO { CODIGO_SUBGRUPO = "", SUBGRUPO = "Selecione" });

                ddlMaterialSubGrupo.DataSource = _matSubGrupo;
                ddlMaterialSubGrupo.DataBind();
            }
        }
        protected void ddlMaterialSubGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                CarregarCores(ddlMaterialGrupo.SelectedItem.Text.Trim(), ddlMaterialSubGrupo.SelectedItem.Text.Trim());
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }

        }
        private void CarregarCores(string grupo, string subGrupo)
        {
            //Obter cores de pedidos
            var _corPedido = desenvController.ObterPedido1000().Where(p => p.GRUPO.Trim() == grupo.Trim() &&
                                            p.SUBGRUPO.Trim() == subGrupo.Trim() &&
                                            p.COR != null && p.COR.Trim() != "").ToList();

            var _coresBasicas = ObterCoresBasicas();

            //Depois filtrar cores para carregar os filtros
            _coresBasicas = _coresBasicas.Where(p => _corPedido.Any(x => x.COR.Trim() == p.COR.Trim())).ToList();
            _coresBasicas.Insert(0, new CORES_BASICA { COR = "", DESC_COR = "" });

            ddlCor.DataSource = _coresBasicas;
            ddlCor.DataBind();

            if (_coresBasicas.Count == 2)
            {
                ddlCor.SelectedIndex = 1;
                ddlCor_SelectedIndexChanged(null, null);
            }
        }
        protected void ddlCor_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                CarregarCorFornecedor(ddlMaterialGrupo.SelectedItem.Text.Trim(), ddlMaterialSubGrupo.SelectedItem.Text.Trim(), ddlCor.SelectedValue.Trim());
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }

        }
        private void CarregarCorFornecedor(string grupo, string subGrupo, string cor)
        {
            //Obter cores do fornecedor dos pedidos
            List<DESENV_PEDIDO> _corFornecedorPedido = new List<DESENV_PEDIDO>();
            _corFornecedorPedido = desenvController.ObterPedido1000().Where(p => p.COR_FORNECEDOR != null
                                                    && p.COR_FORNECEDOR.Trim() != ""
                                                    && p.GRUPO.Trim() == grupo
                                                    && p.SUBGRUPO.Trim() == subGrupo
                                                    && p.COR.Trim() == cor).ToList();

            _corFornecedorPedido = _corFornecedorPedido.GroupBy(p => new { COR_FORNECEDOR = p.COR_FORNECEDOR.Trim() }).Select(k =>
                new DESENV_PEDIDO { COR_FORNECEDOR = k.Key.COR_FORNECEDOR }).OrderBy(p => p.COR_FORNECEDOR).ToList();

            _corFornecedorPedido.Insert(0, new DESENV_PEDIDO { COR_FORNECEDOR = "" });

            ddlCorFornecedor.DataSource = _corFornecedorPedido;
            ddlCorFornecedor.DataBind();

            if (_corFornecedorPedido.Count == 2)
            {
                ddlCorFornecedor.SelectedIndex = 1;
                ddlCorFornecedor_SelectedIndexChanged(null, null);
            }
        }
        protected void ddlCorFornecedor_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                CarregarFornecedores(ddlMaterialGrupo.SelectedItem.Text.Trim(), ddlMaterialSubGrupo.SelectedItem.Text.Trim(), ddlCor.SelectedValue.Trim(), ddlCorFornecedor.SelectedValue.Trim());
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }

        }
        private void CarregarFornecedores(string grupo, string subGrupo, string cor, string corFornecedor)
        {
            //Obter Fornecedores
            var _fornecedores = desenvController.ObterPedido1000().Where(p => p.GRUPO.Trim() == grupo.Trim() &&
                                            p.SUBGRUPO.Trim() == subGrupo.Trim() &&
                                            p.COR.Trim() == cor.Trim() &&
                                            p.COR_FORNECEDOR == corFornecedor.Trim()).ToList();


            _fornecedores = _fornecedores.GroupBy(p => new { FORNECEDOR = p.FORNECEDOR.Trim() }).Select(k =>
                new DESENV_PEDIDO { FORNECEDOR = k.Key.FORNECEDOR }).OrderBy(p => p.FORNECEDOR).ToList();

            _fornecedores.Insert(0, new DESENV_PEDIDO { FORNECEDOR = "" });
            ddlFornecedor.DataSource = _fornecedores;
            ddlFornecedor.DataBind();

        }

        private List<CORES_BASICA> ObterCoresBasicas()
        {
            List<CORES_BASICA> _cores = new List<CORES_BASICA>();
            _cores = new ProducaoController().ObterCoresBasicas();
            return _cores;
        }

        private void LimparCampos()
        {
            txtPedidoLeitura.Text = "";
            txtGrupo.Text = "";
            txtSubGrupo.Text = "";
            txtCor.Text = "";
            txtCorFornecedor.Text = "";
            txtFornecedor.Text = "";
            txtPreco.Text = "";
            txtMedida.Text = "";

            gvComposicao.DataSource = new List<MATERIAL_COMPOSICAO>();
            gvComposicao.DataBind();

            imgFotoTecido.ImageUrl = "";

            gvEntradas.DataSource = new List<DESENV_PEDIDO_QTDE>();
            gvEntradas.DataBind();

            txtQtdeEstoque.Text = "";
            txtProduto.Text = "";
            txtProdutoNome.Text = "";
            txtPecas.Text = "";
            txtFolhas.Text = "";
            txtGasto.Text = "";
            txtEstoque.Text = "";

            gvHB.DataSource = new List<SP_OBTER_HB_PEDIDOResult>();
            gvHB.DataBind();
        }
        #endregion

        #region "AÇÕES DA TELA"
        private void MoverAba(string vAba)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$('#tabs').tabs('option', 'active', '" + vAba + "');", true);
            hidTabSelected.Value = vAba;
        }
        #endregion

        protected void ibtPesquisar_Click(object sender, EventArgs e)
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            DESENV_PEDIDO pedido = null;
            try
            {
                labErro.Text = "";
                LimparCampos();
                labGrupo.ForeColor = _OK;
                labSubGrupo.ForeColor = _OK;
                labCor.ForeColor = _OK;
                labCorFornecedor.ForeColor = _OK;
                labFornecedor.ForeColor = _OK;

                if (txtPedidoNumero.Text.Trim() == "")
                {
                    if (txtDescricaoMaterial.Text.Trim() == "")
                    {
                        if (!ValidarCampos())
                        {
                            labErro.Text = "Preencha corretamento os campos em Vermelho.";
                            return;
                        }

                        var pedidos = desenvController.ObterPedido1000(ddlMaterialGrupo.SelectedValue, ddlMaterialSubGrupo.SelectedValue);

                        if (ddlCor.SelectedValue != "" && ddlCor.SelectedValue != "Selecione")
                            pedidos = pedidos.Where(p => p.COR.Contains(ddlCor.SelectedValue.Trim())).ToList();
                        if (ddlCorFornecedor.SelectedValue != "" && ddlCorFornecedor.SelectedValue != "Selecione")
                            pedidos = pedidos.Where(p => p.COR_FORNECEDOR.Contains(ddlCorFornecedor.SelectedValue.Trim())).ToList();
                        if (ddlFornecedor.SelectedValue != "" && ddlFornecedor.SelectedValue != "Selecione")
                            pedidos = pedidos.Where(p => p.FORNECEDOR.Contains(ddlFornecedor.SelectedValue.Trim())).ToList();

                        if (pedidos != null && pedidos.Count() > 0)
                        {
                            if (pedidos.Count() > 1)
                            {
                                gvPedidos.DataSource = pedidos;
                                gvPedidos.DataBind();

                                MoverAba("1");
                                return;
                            }
                            else
                            {
                                txtPedidoNumero.Text = pedidos[0].NUMERO_PEDIDO.ToString();
                                pedido = pedidos[0];
                            }
                        }
                        else
                        {
                            MoverAba("0");
                            labErro.Text = "Nenhum Pedido Encontrado. Refaça sua pesquisa.";
                            return;
                        }
                    }
                    else
                    {
                        var pedidos = desenvController.ObterPedido1000(txtDescricaoMaterial.Text.Trim());
                        if (pedidos != null && pedidos.Count() > 0)
                        {
                            if (pedidos.Count() > 1)
                            {
                                gvPedidos.DataSource = pedidos;
                                gvPedidos.DataBind();

                                MoverAba("1");
                                return;
                            }
                            else
                            {
                                txtPedidoNumero.Text = pedidos[0].NUMERO_PEDIDO.ToString();
                                pedido = pedidos[0];
                            }
                        }
                        else
                        {
                            MoverAba("0");
                            labErro.Text = "Nenhum Pedido Encontrado. Refaça sua pesquisa.";
                            return;
                        }
                    }
                }
                else
                {

                    if (txtPedidoNumero.Text.Trim() != "" && Convert.ToInt32(txtPedidoNumero.Text.Trim()) < 1000)
                    {
                        labErro.Text = "Informe um número de pedido acima de 1000.";
                        return;
                    }


                    pedido = desenvController.ObterPedidoNumero(Convert.ToInt32(txtPedidoNumero.Text));
                    if (pedido == null)
                    {
                        labErro.Text = "Nenhum Pedido Encontrado. Refaça sua pesquisa.";
                        return;
                    }

                    if (pedido.STATUS == 'E')
                    {
                        labErro.Text = "Pedido excluído. Refaça sua pesquisa.";
                        return;
                    }

                }

                CarregarCartelinha(pedido);
                MoverAba("0");
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
        protected void ibtImprimirNovo_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                ImprimirCartelinha(null);
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        #region "CARTELINHA"
        private void CarregarCartelinha(DESENV_PEDIDO pedido)
        {
            if (pedido != null)
            {
                //CABECALHO
                txtPedidoLeitura.Text = pedido.NUMERO_PEDIDO.ToString();
                txtFornecedor.Text = pedido.FORNECEDOR;
                txtGrupo.Text = pedido.GRUPO;
                txtSubGrupo.Text = pedido.SUBGRUPO;
                txtCor.Text = prodController.ObterCoresBasicas(pedido.COR).DESC_COR;
                txtCorFornecedor.Text = pedido.COR_FORNECEDOR;
                txtPreco.Text = pedido.VALOR.ToString();
                txtMedida.Text = pedido.UNIDADE_MEDIDA1.DESCRICAO + "s";

                var material = desenvController.ObterMaterialCorV2(pedido.GRUPO.Trim(), pedido.SUBGRUPO.Trim(), "")
                    .Where(p => p.COR_MATERIAL.Trim() == pedido.COR.Trim() && p.REFER_FABRICANTE.Trim() == pedido.COR_FORNECEDOR.Trim()).ToList();

                var materialFoto = desenvController.ObterMaterialFotoIntranet(material[0].MATERIAL.Substring(0, 10));
                if (materialFoto != null)
                {
                    imgFotoTecido.ImageUrl = materialFoto.FOTO;
                }

                if (material != null && material.Count() > 0)
                {
                    var composicao = desenvController.ObterMaterialComposicao(material[0].MATERIAL.Substring(0, 10));
                    gvComposicao.DataSource = composicao;
                    gvComposicao.DataBind();
                }

                CarregarEntradas(pedido);

                cbEstConferido.Checked = false;
                txtDataEstConferido.Text = "";
                if (pedido.ESTOQUE_CONFERIDO != null)
                {
                    txtDataEstConferido.Text = Convert.ToDateTime(pedido.ESTOQUE_CONFERIDO).ToString("dd/MM/yyyy");
                    cbEstConferido.Checked = true;
                }

                CarregarHB(pedido);
            }
        }
        #endregion

        #region "ENTRADAS"
        private void CarregarEntradas(DESENV_PEDIDO pedido)
        {
            //ENTRADAS E SAIDAS
            var entradas = desenvController.ObterPedidoQtdePedido(pedido.CODIGO);
            gvEntradas.DataSource = entradas;
            gvEntradas.DataBind();
        }
        protected void gvEntradas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    DESENV_PEDIDO_QTDE _pedidoQtde = e.Row.DataItem as DESENV_PEDIDO_QTDE;

                    if (_pedidoQtde != null)
                    {
                        Literal _litNF = e.Row.FindControl("litNF") as Literal;
                        if (_litNF != null)
                            _litNF.Text = _pedidoQtde.NOTA_FISCAL;

                        Literal _litDataEntrada = e.Row.FindControl("litDataEntrada") as Literal;
                        if (_litDataEntrada != null)
                            _litDataEntrada.Text = _pedidoQtde.DATA.ToString("dd/MM/yyyy");

                        Literal _litQtde = e.Row.FindControl("litQtde") as Literal;
                        if (_litQtde != null)
                        {
                            _litQtde.Text = _pedidoQtde.QTDE.ToString("###,###,###,##0.00");
                            qtdeEntrada += _pedidoQtde.QTDE;
                        }

                        Literal _litMedida = e.Row.FindControl("litMedida") as Literal;
                        if (_litMedida != null)
                            _litMedida.Text = _pedidoQtde.DESENV_PEDIDO1.UNIDADE_MEDIDA1.DESCRICAO + "s";

                        ImageButton _ibtImprimir = e.Row.FindControl("ibtImprimir") as ImageButton;
                        if (_ibtImprimir != null)
                            _ibtImprimir.CommandArgument = _pedidoQtde.CODIGO.ToString();

                    }
                }
            }
        }
        protected void gvEntradas_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvEntradas.FooterRow;
            if (footer != null)
            {
                footer.Cells[3].Text = qtdeEntrada.ToString("###,###,###,##0.00");
                hidQtdeEntrada.Value = qtdeEntrada.ToString();
            }
        }


        protected void ibtImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                ImageButton b = (ImageButton)sender;
                if (b != null)
                {
                    int codigo = 0;
                    codigo = Convert.ToInt32(b.CommandArgument);
                    var desenvQtdePedido = desenvController.ObterPedidoQtde(codigo);

                    ImprimirCartelinha(desenvQtdePedido);
                }
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }


        #endregion

        #region "PEDIDOS"
        protected void gvPedidos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    DESENV_PEDIDO _pedido = e.Row.DataItem as DESENV_PEDIDO;

                    if (_pedido != null)
                    {
                        Label _labCor = e.Row.FindControl("labCor") as Label;
                        if (_labCor != null)
                        {
                            var _cor = prodController.ObterCoresBasicas(_pedido.COR);
                            _labCor.Text = (_cor == null) ? _pedido.COR : _cor.DESC_COR;
                        }

                        ImageButton _btPesquisar = e.Row.FindControl("ibtPesquisar") as ImageButton;
                        if (_btPesquisar != null)
                            _btPesquisar.CommandArgument = _pedido.NUMERO_PEDIDO.ToString();

                    }
                }
            }
        }
        protected void ibtPesquisarPedido_Click(object sender, EventArgs e)
        {
            try
            {
                ImageButton bt = (ImageButton)sender;
                if (bt != null)
                {
                    txtPedidoNumero.Text = "";

                    int numeroPedido = Convert.ToInt32(bt.CommandArgument);
                    txtPedidoNumero.Text = numeroPedido.ToString();

                    var pedido = desenvController.ObterPedidoNumero(numeroPedido);
                    if (pedido == null)
                    {
                        labErro.Text = "Nenhum Pedido Encontrado. Refaça sua pesquisa.";
                        return;
                    }

                    if (pedido.STATUS == 'E')
                    {
                        labErro.Text = "Pedido excluído. Refaça sua pesquisa.";
                        return;
                    }
                    CarregarCartelinha(pedido);

                    MoverAba("0");
                }
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }

        }
        #endregion

        #region "HB"
        private List<SP_OBTER_HB_PEDIDOResult> ObterEstoqueCartelinha(DESENV_PEDIDO pedido)
        {
            return prodController.ObterEstoqueCartelinha(pedido.NUMERO_PEDIDO, "", "", "", "");
        }
        private void CarregarHB(DESENV_PEDIDO pedido)
        {
            gvHB.DataSource = ObterEstoqueCartelinha(pedido);
            gvHB.DataBind();
        }
        protected void gvHB_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_HB_PEDIDOResult _hb = e.Row.DataItem as SP_OBTER_HB_PEDIDOResult;

                    if (_hb != null)
                    {
                        Literal _litData = e.Row.FindControl("litData") as Literal;
                        if (_litData != null)
                            _litData.Text = Convert.ToDateTime(_hb.DATA).ToString("dd/MM/yyyy");

                        //qtdeEntrada
                        Literal _litEstoque = e.Row.FindControl("litEstoque") as Literal;
                        if (_litEstoque != null)
                        {
                            //qtdeEntrada preenchida no loop do grid Entrada
                            var gasto = (_hb.CORTE_SIMULACAO == "S") ? 0.00M : Convert.ToDecimal(_hb.GASTO);
                            qtdeEntrada = (qtdeEntrada - gasto);
                            _litEstoque.Text = qtdeEntrada.ToString("###,###,###,##0.000");
                        }

                        ImageButton btMarcarSimulado = e.Row.FindControl("btMarcarSimulado") as ImageButton;
                        ImageButton _btExcluir = e.Row.FindControl("btExcluir") as ImageButton;

                        if (_hb.HB.Trim() != "")
                        {
                            _btExcluir.Visible = false;
                            btMarcarSimulado.CommandArgument = _hb.CODIGO.ToString();
                        }
                        else
                        {
                            _btExcluir.CommandArgument = _hb.CODIGO.ToString();
                            e.Row.BackColor = Color.OldLace;
                        }

                        if (_hb.SIMULACAO == "S" || _hb.CORTE_SIMULACAO == "S")
                        {
                            e.Row.ForeColor = Color.HotPink;
                        }

                        if (_hb.CORTE_SIMULACAO == "S")
                            btMarcarSimulado.ImageUrl = "~/Image/up_disabled.png";
                        else
                            btMarcarSimulado.ImageUrl = "~/Image/down.png";

                    }
                }
            }
        }
        protected void gvHB_DataBound(object sender, EventArgs e)
        {
            hidQtdeEstoque.Value = qtdeEntrada.ToString();
            txtQtdeEstoque.Text = qtdeEntrada.ToString();
        }
        protected void gvHB_Sorting(object sender, GridViewSortEventArgs e)
        {
            var pedido = desenvController.ObterPedidoNumero(Convert.ToInt32(txtPedidoLeitura.Text));
            if (pedido != null)
            {
                //Obter qtde inicial de entrada
                qtdeEntrada = Convert.ToDecimal(hidQtdeEntrada.Value);

                IEnumerable<SP_OBTER_HB_PEDIDOResult> _hb = ObterEstoqueCartelinha(pedido);

                string sortExpression = e.SortExpression;
                SortDirection sort;

                Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

                if (sort == SortDirection.Ascending)
                    Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
                else
                    Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

                string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

                _hb = _hb.OrderBy(e.SortExpression + sortDirection);
                gvHB.DataSource = _hb;
                gvHB.DataBind();
            }
        }
        protected void txtProdutoFooter_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (txt != null)
            {
                labErroHB.Text = "";
                try
                {
                    GridViewRow row = (GridViewRow)txt.NamingContainer;
                    if (row != null)
                    {
                        Literal _litNome = row.FindControl("litNomeFooter") as Literal;
                        string produtoOuPedido = txt.Text.Trim();
                        _litNome.Text = ObterNomeProdutoOuPedido(produtoOuPedido);
                    }
                }
                catch (Exception ex)
                {
                    labErroHB.Text = ex.Message;
                }
            }
        }
        protected void CalcularSobraEstoqueRodape(object sender, EventArgs e)
        {
            try
            {
                labErroHB.Text = "";

                TextBox _txt = (TextBox)sender;
                if (_txt != null)
                {
                    GridViewRow row = (GridViewRow)_txt.NamingContainer;
                    if (row != null)
                    {
                        TextBox _txtPecasFooter = row.FindControl("txtPecasFooter") as TextBox;
                        TextBox _txtFolhasFooter = row.FindControl("txtFolhasFooter") as TextBox;
                        TextBox _txtGastoFooter = row.FindControl("txtGastoFooter") as TextBox;
                        Literal _litEstoqueFooter = row.FindControl("litEstoqueFooter") as Literal;

                        decimal qtdeEstoqueFinal = 0;
                        decimal? pecas = null;
                        decimal? folhas = null;
                        decimal? gasto = null;
                        decimal qtdeEstoque = Convert.ToDecimal(hidQtdeEstoque.Value);
                        if (_txtPecasFooter.Text.Trim() != "")
                            pecas = Convert.ToInt32(_txtPecasFooter.Text.Trim());
                        if (_txtFolhasFooter.Text.Trim() != "")
                            folhas = Convert.ToDecimal(_txtFolhasFooter.Text.Trim());
                        if (_txtGastoFooter.Text.Trim() != "")
                            gasto = Convert.ToDecimal(_txtGastoFooter.Text.Trim());

                        if (pecas != null && folhas != null)
                        {
                            gasto = pecas * folhas;
                            _txtGastoFooter.Text = Convert.ToDecimal(gasto).ToString("###,###,##0.000");
                        }
                        else if (pecas != null && gasto != null)
                        {
                            folhas = gasto / pecas;
                            _txtFolhasFooter.Text = Convert.ToDecimal(folhas).ToString("###,###,##0.000");
                        }
                        else if (folhas != null && gasto != null)
                        {
                            pecas = gasto / folhas;
                            _txtPecasFooter.Text = Convert.ToInt32(pecas).ToString();
                        }

                        if (_txtGastoFooter.Text.Trim() != "")
                        {
                            qtdeEstoqueFinal = qtdeEstoque - Convert.ToDecimal(gasto);
                            _litEstoqueFooter.Text = qtdeEstoqueFinal.ToString("###,###,##0.000");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                labErroHB.Text = ex.Message;
            }
        }
        protected void txtProduto_TextChanged(object sender, EventArgs e)
        {
            try
            {
                labErroSimulacao.Text = "";
                txtProdutoNome.Text = "";

                string produtoOuPedido = txtProduto.Text.Trim();
                txtProdutoNome.Text = ObterNomeProdutoOuPedido(produtoOuPedido);
            }
            catch (Exception ex)
            {
                labErroSimulacao.Text = ex.Message;
            }
        }
        private string ObterNomeProdutoOuPedido(string produtoOuPedido)
        {
            var _produto = baseController.BuscaProduto(produtoOuPedido);
            if (_produto != null)
            {
                return _produto.DESC_PRODUTO.Trim();
            }
            else
            {
                if (produtoOuPedido != "")
                {
                    var _pedido = desenvController.ObterPedidoNumero(Convert.ToInt32(produtoOuPedido));
                    if (_pedido != null)
                    {
                        return _pedido.SUBGRUPO.Trim();
                    }
                }
            }

            return "";
        }
        protected void CalcularSobraEstoque(object sender, EventArgs e)
        {
            try
            {
                labErroSimulacao.Text = "";

                decimal qtdeEstoqueFinal = 0;
                decimal? pecas = null;
                decimal? folhas = null;
                decimal? gasto = null;
                decimal qtdeEstoque = Convert.ToDecimal(hidQtdeEstoque.Value);
                if (txtPecas.Text.Trim() != "")
                    pecas = Convert.ToInt32(txtPecas.Text.Trim());
                if (txtFolhas.Text.Trim() != "")
                    folhas = Convert.ToDecimal(txtFolhas.Text.Trim());
                if (txtGasto.Text.Trim() != "")
                    gasto = Convert.ToDecimal(txtGasto.Text.Trim());

                if (pecas != null && folhas != null)
                {
                    gasto = pecas * folhas;
                    txtGasto.Text = Convert.ToDecimal(gasto).ToString("###,###,##0.000");
                }
                else if (pecas != null && gasto != null)
                {
                    folhas = gasto / pecas;
                    txtFolhas.Text = Convert.ToDecimal(folhas).ToString("###,###,##0.000");
                }
                else if (folhas != null && gasto != null)
                {
                    pecas = gasto / folhas;
                    txtPecas.Text = Convert.ToInt32(pecas).ToString();
                }

                if (txtGasto.Text.Trim() != "")
                {
                    qtdeEstoqueFinal = qtdeEstoque - Convert.ToDecimal(gasto);
                    txtEstoque.Text = qtdeEstoqueFinal.ToString("###,###,##0.000");
                }
            }
            catch (Exception ex)
            {
                labErroSimulacao.Text = ex.Message;
            }
        }

        protected void btIncluirSimulacao_Click(object sender, EventArgs e)
        {

            try
            {
                labErroSimulacao.Text = "";

                if (txtProduto.Text.Trim() == "" || txtProdutoNome.Text.Trim() == "")
                {
                    labErroSimulacao.Text = "Informe o Produto ou Pedido.";
                    return;
                }
                if (txtPecas.Text.Trim() == "")
                {
                    labErroSimulacao.Text = "Informe a Quantidade de Peças.";
                    return;
                }
                if (txtFolhas.Text.Trim() == "")
                {
                    labErroSimulacao.Text = "Informe o Gasto por folha.";
                    return;
                }

                PROD_HB_TECIDO_SIMULACAO _tecidoSim = new PROD_HB_TECIDO_SIMULACAO();
                _tecidoSim.NUMERO_PEDIDO = Convert.ToInt32(txtPedidoLeitura.Text);
                _tecidoSim.PRODUTO = txtProduto.Text.Trim();
                _tecidoSim.GRADE_TOTAL = Convert.ToInt32(txtPecas.Text.Trim());
                _tecidoSim.GASTO_FOLHA = Convert.ToDecimal(txtFolhas.Text.Trim());
                _tecidoSim.DATA_INCLUSAO = DateTime.Now;

                prodController.InserirTecidoEstoqueSimulado(_tecidoSim);

                txtProduto.Text = "";
                txtProdutoNome.Text = "";
                txtPecas.Text = "";
                txtFolhas.Text = "";
                txtGasto.Text = "";
                txtEstoque.Text = "";

                ibtPesquisar_Click(null, null);

            }
            catch (Exception ex)
            {
                labErroSimulacao.Text = ex.Message;
            }

        }
        protected void btIncluir_Click(object sender, EventArgs e)
        {
            try
            {
                labErroHB.Text = "";

                ImageButton _ibt = (ImageButton)sender;
                if (_ibt != null)
                {
                    GridViewRow row = (GridViewRow)_ibt.NamingContainer;
                    if (row != null)
                    {
                        TextBox _txtProdutoFooter = row.FindControl("txtProdutoFooter") as TextBox;
                        Literal _litNomeFooter = row.FindControl("litNomeFooter") as Literal;
                        TextBox _txtPecasFooter = row.FindControl("txtPecasFooter") as TextBox;
                        TextBox _txtFolhasFooter = row.FindControl("txtFolhasFooter") as TextBox;
                        TextBox _txtGastoFooter = row.FindControl("txtGastoFooter") as TextBox;
                        Literal _litEstoqueFooter = row.FindControl("litEstoqueFooter") as Literal;

                        if (_txtProdutoFooter.Text.Trim() == "" || _litNomeFooter.Text.Trim() == "")
                        {
                            labErroHB.Text = "Informe o Produto ou Pedido.";
                            return;
                        }
                        if (_txtPecasFooter.Text.Trim() == "")
                        {
                            labErroHB.Text = "Informe a quantidade de Peças.";
                            return;
                        }
                        if (_txtFolhasFooter.Text.Trim() == "")
                        {
                            labErroHB.Text = "Informe o Gasto por folha.";
                            return;
                        }

                        PROD_HB_TECIDO_SIMULACAO _tecidoSim = new PROD_HB_TECIDO_SIMULACAO();
                        _tecidoSim.NUMERO_PEDIDO = Convert.ToInt32(txtPedidoLeitura.Text);
                        _tecidoSim.PRODUTO = _txtProdutoFooter.Text.Trim();
                        _tecidoSim.GRADE_TOTAL = Convert.ToInt32(_txtPecasFooter.Text.Trim());
                        _tecidoSim.GASTO_FOLHA = Convert.ToDecimal(_txtFolhasFooter.Text.Trim());
                        _tecidoSim.DATA_INCLUSAO = DateTime.Now;

                        prodController.InserirTecidoEstoqueSimulado(_tecidoSim);

                        _txtProdutoFooter.Text = "";
                        _litNomeFooter.Text = "";
                        _txtPecasFooter.Text = "";
                        _txtFolhasFooter.Text = "";
                        _txtGastoFooter.Text = "";
                        _litEstoqueFooter.Text = "";

                        ibtPesquisar_Click(null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                labErroHB.Text = ex.Message;
            }

        }
        protected void btExcluir_Click(object sender, EventArgs e)
        {
            try
            {
                labErroHB.Text = "";

                ImageButton _ibt = (ImageButton)sender;
                if (_ibt != null)
                {
                    if (_ibt.CommandArgument.Trim() != "")
                    {
                        int codigoSimulacao = Convert.ToInt32(_ibt.CommandArgument);
                        prodController.ExcluirTecidoEstoqueSimulado(codigoSimulacao);
                        ibtPesquisar_Click(null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                labErroHB.Text = ex.Message;
            }

        }
        #endregion

        #region "VALIDACAO"
        private bool ValidarCampos()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            labGrupo.ForeColor = _OK;
            if (ddlMaterialGrupo.SelectedValue.Trim() == "Selecione" || ddlMaterialGrupo.SelectedValue.Trim() == "")
            {
                labGrupo.ForeColor = _notOK;
                retorno = false;
            }

            labSubGrupo.ForeColor = _OK;
            if (ddlMaterialSubGrupo.SelectedValue.Trim() == "Selecione" || ddlMaterialSubGrupo.SelectedValue.Trim() == "")
            {
                labSubGrupo.ForeColor = _notOK;
                retorno = false;
            }

            return retorno;
        }
        protected void ibtLimpar_Click(object sender, EventArgs e)
        {
            txtPedidoNumero.Text = "";
            ddlMaterialGrupo.SelectedValue = "Selecione";
            ddlMaterialSubGrupo.SelectedValue = "Selecione";
            ddlCor.SelectedValue = "";
            ddlCorFornecedor.SelectedValue = "";
            ddlFornecedor.SelectedValue = "";
            txtDescricaoMaterial.Text = "";

            txtPedidoLeitura.Text = "";
            txtFornecedor.Text = "";
            txtGrupo.Text = "";
            txtSubGrupo.Text = "";
            txtCor.Text = "";
            txtCorFornecedor.Text = "";
            txtPreco.Text = "";
            txtMedida.Text = "";

            gvComposicao.DataSource = new List<MATERIAL_COMPOSICAO>();
            gvComposicao.DataBind();

            imgFotoTecido.ImageUrl = "";

            gvEntradas.DataSource = new List<DESENV_PEDIDO_QTDE>();
            gvEntradas.DataBind();

            gvHB.DataSource = new List<SP_OBTER_HB_PEDIDOResult>();
            gvHB.DataBind();
            hidQtdeEstoque.Value = "";

            txtDataEstConferido.Text = "";
            cbEstConferido.Checked = false;

            gvPedidos.DataSource = new List<DESENV_PEDIDO>();
            gvPedidos.DataBind();

            MoverAba("0");
        }
        #endregion

        #region "IMPRESSAO"
        private void ImprimirCartelinha(DESENV_PEDIDO_QTDE desenvPedidoQtde)
        {
            StreamWriter wr = null;
            try
            {
                labErro.Text = "";

                string nomeArquivo = "ENT_MAT_CARTE" + ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO + ".html";
                wr = new StreamWriter(Server.MapPath("") + "\\html\\" + nomeArquivo);
                wr.Write(Cartelinha.MontarCartelinha(desenvPedidoQtde));
                wr.Flush();

                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "AbrirPocket('" + nomeArquivo + "')", true);

            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
            finally
            {
                wr.Close();
            }

        }
        #endregion

        protected void cbEstConferido_CheckedChanged(object sender, EventArgs e)
        {

            if (txtPedidoNumero.Text.Trim() != "")
            {
                var numPedido = Convert.ToInt32(txtPedidoNumero.Text);

                var desenvPedido = desenvController.ObterPedidoNumero(numPedido);
                if (desenvPedido != null)
                {
                    if (cbEstConferido.Checked)
                    {
                        desenvPedido.ESTOQUE_CONFERIDO = DateTime.Now;
                        txtDataEstConferido.Text = Convert.ToDateTime(desenvPedido.ESTOQUE_CONFERIDO).ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        desenvPedido.ESTOQUE_CONFERIDO = null;
                        txtDataEstConferido.Text = "";
                    }

                    desenvController.AtualizarPedido(desenvPedido);

                }
            }

        }

        protected void btMarcarSimulado_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                labErroHB.Text = "";

                ImageButton bt = (ImageButton)sender;
                if (bt.CommandArgument != "")
                {
                    var codigoHB = Convert.ToInt32(bt.CommandArgument);
                    var prodHB = prodController.ObterHB(codigoHB);
                    if (prodHB != null)
                    {
                        if (prodHB.CORTE_SIMULACAO == 'S')
                            prodHB.CORTE_SIMULACAO = 'N';
                        else
                            prodHB.CORTE_SIMULACAO = 'S';

                        prodController.AtualizarHB(prodHB);
                        ibtPesquisar_Click(null, null);
                    }
                }

            }
            catch (Exception ex)
            {
                labErroHB.Text = ex.Message;
            }
        }

    }
}
