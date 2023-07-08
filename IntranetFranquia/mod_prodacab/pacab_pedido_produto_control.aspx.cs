using DAL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq.Dynamic;
using System.Text;
using System.IO;
using System.Web.UI.HtmlControls;

namespace Relatorios
{
    public partial class pacab_pedido_produto_control : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();

        const string PROD_ACAB_PEDCOMPRA_PROD = "PROD_ACAB_PEDCOMPRA_CONTROL";

        int qtdeIntra = 0;
        int qtdeLinx = 0;

        int qtdeCar = 0;
        decimal valCar = 0;
        decimal valorTotPgto = 0;


        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                CarregarColecoes();
                CarregarGrupo();

                CarregarFabricante();
                CarregarFilial();
                CarregarGriffe();

                CarregarCarrinhoLinx();

                Session[PROD_ACAB_PEDCOMPRA_PROD] = null;
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        #region "DADOS INICIAIS"
        protected void ddlColecoes_SelectedIndexChanged(object sender, EventArgs e)
        {
            string colecao = ddlColecoes.SelectedValue.Trim();
            if (colecao != "" && colecao != "0")
            {
                Session["COLECAO"] = ddlColecoes.SelectedValue;
                CarregarOrigem(colecao);
            }

        }
        private void CarregarColecoes()
        {
            BaseController _base = new BaseController();
            List<COLECOE> _colecoes = _base.BuscaColecoes();
            if (_colecoes != null)
            {
                _colecoes.Insert(0, new COLECOE { COLECAO = "", DESC_COLECAO = "" });

                ddlColecoes.DataSource = _colecoes;
                ddlColecoes.DataBind();

                if (Session["COLECAO"] != null)
                {
                    ddlColecoes.SelectedValue = Session["COLECAO"].ToString();
                    CarregarOrigem(Session["COLECAO"].ToString().Trim());
                    ddlColecoes_SelectedIndexChanged(null, null);
                }
            }
        }
        private void CarregarGrupo()
        {
            List<SP_OBTER_GRUPOResult> _grupo = (prodController.ObterGrupoProduto(""));
            if (_grupo != null)
            {
                _grupo.Insert(0, new SP_OBTER_GRUPOResult { GRUPO_PRODUTO = "" });
                ddlGrupo.DataSource = _grupo;
                ddlGrupo.DataBind();
            }
        }
        private void CarregarOrigem(string col)
        {
            List<DESENV_PRODUTO_ORIGEM> _origem = desenvController.ObterProdutoOrigem().Where(i => i.STATUS == 'A').ToList();
            _origem = _origem.Where(p => p.COLECAO.Trim() == col.Trim()).OrderBy(i => i.DESCRICAO).ToList();
            if (_origem != null)
            {
                _origem.Insert(0, new DESENV_PRODUTO_ORIGEM { CODIGO = 0, DESCRICAO = "" });
                ddlOrigem.DataSource = _origem;
                ddlOrigem.DataBind();

                if (_origem.Count == 2)
                    ddlOrigem.SelectedValue = _origem[1].CODIGO.ToString();

            }
        }
        private void CarregarFabricante()
        {
            var _fornecedores = prodController.ObterFornecedor().ToList();
            _fornecedores.Insert(0, new PROD_FORNECEDOR { FORNECEDOR = "", STATUS = 'S' });
            _fornecedores.Where(p => ((p.STATUS == 'A' && p.TIPO == 'C') || p.STATUS == 'S')).GroupBy(x => new { FORNECEDOR = x.FORNECEDOR.Trim() }).Select(f => new PROD_FORNECEDOR { FORNECEDOR = f.Key.FORNECEDOR.Trim() }).ToList();

            if (_fornecedores != null)
            {
                ddlFabricante.DataSource = _fornecedores;
                ddlFabricante.DataBind();
            }
        }
        private void CarregarFilial()
        {
            List<FILIAI> filial = new List<FILIAI>();
            List<FILIAIS_DE_PARA> filialDePara = new List<FILIAIS_DE_PARA>();

            filial = baseController.BuscaFiliais();

            filialDePara = baseController.BuscaFilialDePara();
            filial = filial.Where(i => !filialDePara.Any(g => g.DE == i.COD_FILIAL)).ToList();
            if (filial != null)
            {
                filial.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "" });
                filial.Insert(1, new FILIAI { COD_FILIAL = "1055", FILIAL = "C-MAX (NOVA)" });
                filial.Insert(2, new FILIAI { COD_FILIAL = "000029", FILIAL = "CD - LUGZY               " });
                filial.Insert(3, new FILIAI { COD_FILIAL = "000041", FILIAL = "CD MOSTRUARIO            " });
                filial.Insert(4, new FILIAI { COD_FILIAL = "1029", FILIAL = "ATACADO HANDBOOK         " });
                filial.Insert(5, new FILIAI { COD_FILIAL = "1054", FILIAL = "HANDBOOK ONLINE          " });

                ddlFilial.DataSource = filial;
                ddlFilial.DataBind();

                ddlFilial.Enabled = true;
                if (filial.Count() == 2)
                {
                    ddlFilial.SelectedIndex = 1;
                    ddlFilial.Enabled = false;
                }
            }
        }
        private void CarregarGriffe()
        {
            var griffe = (baseController.BuscaGriffes());

            if (griffe != null)
            {
                griffe.Insert(0, new PRODUTOS_GRIFFE { GRIFFE = "" });
                lstGriffe.DataSource = griffe;
                lstGriffe.DataBind();
            }
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
                        produtoCores.Insert(0, new PRODUTO_CORE { COR_PRODUTO = "", DESC_COR_PRODUTO = "" });

                        ddlCor.DataSource = produtoCores;
                        ddlCor.DataBind();
                    }
                    else
                    {
                        ddlCor.DataSource = new List<PRODUTO_CORE>();
                        ddlCor.DataBind();
                    }
                }
                else
                {
                    ddlCor.DataSource = new List<PRODUTO_CORE>();
                    ddlCor.DataBind();
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labMsg.Text = "";

                if (ddlFilial.SelectedValue == "" && ddlFabricante.SelectedValue == "" && ddlColecoes.SelectedValue == "")
                {
                    labMsg.Text = "Selecione pelo menos a Filial, Fornecedor ou Coleção...";
                    return;
                }

                //if (ddlColecoes.SelectedValue == "")
                //{
                //    labMsg.Text = "Selecione a Coleção.";
                //    return;
                //}

                CarregarProdutoAcabado();
            }
            catch (Exception ex)
            {
                labMsg.Text = ex.Message;
            }

        }

        #region "PRODUTO"
        private List<SP_OBTER_PRODUTO_ACABADO_PRODUTO_V10Result> ObterProdutoAcabadoProdutoPrePedido()
        {
            var desenvOrigem = 0;
            if (ddlOrigem.SelectedValue != "0" && ddlOrigem.SelectedValue != "")
                desenvOrigem = Convert.ToInt32(ddlOrigem.SelectedValue);

            var produtoAcabado = desenvController.ObterProdutoAcabadoProdutoPrePedidoV10(ddlColecoes.SelectedValue.Trim(), desenvOrigem, ddlGrupo.SelectedValue.Trim(), txtProduto.Text.Trim(), ddlFilial.SelectedItem.Text.Trim(), ddlFabricante.SelectedValue.Trim());

            //if (ddlGriffe.SelectedValue.Trim() != "")
            //    produtoAcabado = produtoAcabado.Where(p => p.GRIFFE.Trim() == ddlGriffe.SelectedValue.Trim()).ToList();

            //Filtrar por griffe
            var griffeCon = "";
            if (lstGriffe.GetSelectedIndices().Count() > 0)
            {
                foreach (var v in lstGriffe.GetSelectedIndices())
                {
                    var itemList = lstGriffe.Items[v].Value.Trim() + ",";
                    griffeCon = griffeCon + itemList;
                }

                griffeCon = griffeCon + ",";
                griffeCon = griffeCon.Replace(",,", "");
            }
            if (griffeCon != "")
                produtoAcabado = produtoAcabado.Where(p => p.GRIFFE != null && griffeCon.Contains(p.GRIFFE.Trim())).ToList();

            if (ddlCor.SelectedValue != "")
                produtoAcabado = produtoAcabado.Where(p => p.COR.Trim() == ddlCor.SelectedValue.Trim()).ToList();

            if (txtPedidoIntra.Text.Trim() != "")
                produtoAcabado = produtoAcabado.Where(p => p.PEDIDO_INTRA.Trim().ToLower() == txtPedidoIntra.Text.Trim().ToLower()).ToList();

            if (ddlPedidoAberto.SelectedValue != "")
            {
                if (ddlPedidoAberto.SelectedValue == "S")
                    produtoAcabado = produtoAcabado.Where(p => p.PEDIDO_LINX == null || p.PEDIDO_LINX.Trim() == "").ToList();
                else
                    produtoAcabado = produtoAcabado.Where(p => p.PEDIDO_LINX != null && p.PEDIDO_LINX.Trim() != "").ToList();
            }

            if (ddlAtrasado.SelectedValue != "")
            {
                if (ddlAtrasado.SelectedValue == "S")
                    produtoAcabado = produtoAcabado.Where(p => p.ENTREGA < DateTime.Now.Date).ToList();
                else
                    produtoAcabado = produtoAcabado.Where(p => p.ENTREGA >= DateTime.Now.Date).ToList();
            }

            if (ddlExcluido.SelectedValue != "")
            {
                if (ddlExcluido.SelectedValue == "S")
                    produtoAcabado = produtoAcabado.Where(p => p.DATA_EXCLUSAO != null).ToList();
                else
                    produtoAcabado = produtoAcabado.Where(p => p.DATA_EXCLUSAO == null).ToList();
            }

            if (ddlItensOK.SelectedValue != "")
            {
                if (ddlItensOK.SelectedValue == "S")
                {
                    produtoAcabado = produtoAcabado.Where(p => p.DATA_BAIXA_AVIAMENTO != null && p.DATA_BAIXA_TAG != null && p.DATA_BAIXA_ETI_BARRA != null && p.DATA_BAIXA_ETI_COMP != null).ToList();
                }
                else
                {
                    produtoAcabado = produtoAcabado.Where(p => p.DATA_BAIXA_AVIAMENTO == null || p.DATA_BAIXA_TAG == null || p.DATA_BAIXA_ETI_BARRA == null || p.DATA_BAIXA_ETI_COMP == null).ToList();
                }
            }


            if (ddlTipo.SelectedValue != "")
                produtoAcabado = produtoAcabado.Where(p => p.TIPO == ddlTipo.SelectedValue.Trim()).ToList();


            if (ddlPagamento.SelectedValue != "")
            {
                //<asp:ListItem Value="1" Text="Em Aberto"></asp:ListItem>
                //<asp:ListItem Value="2" Text="Pago"></asp:ListItem>
                //<asp:ListItem Value="3" Text="Sem Cadastro"></asp:ListItem>
                if (ddlPagamento.SelectedValue == "1")
                    produtoAcabado = produtoAcabado.Where(p => p.QTDE_PARCELA > 0 && p.QTDE_PARCELA_PAGA != p.QTDE_PARCELA).ToList();
                else if (ddlPagamento.SelectedValue == "2")
                    produtoAcabado = produtoAcabado.Where(p => p.QTDE_PARCELA > 0 && p.QTDE_PARCELA_PAGA == p.QTDE_PARCELA).ToList();
                else if (ddlPagamento.SelectedValue == "3")
                    produtoAcabado = produtoAcabado.Where(p => p.QTDE_PARCELA <= 0 && p.QTDE_PARCELA_PAGA <= 0).ToList();

            }

            return produtoAcabado;
        }

        private void CarregarProdutoAcabado()
        {
            var produtoAcabado = ObterProdutoAcabadoProdutoPrePedido();

            gvProdutoAcabado.DataSource = produtoAcabado;
            gvProdutoAcabado.DataBind();

            Session[PROD_ACAB_PEDCOMPRA_PROD] = produtoAcabado;
        }
        protected void gvProdutoAcabado_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_PRODUTO_ACABADO_PRODUTO_V10Result produtoAcabado = e.Row.DataItem as SP_OBTER_PRODUTO_ACABADO_PRODUTO_V10Result;

                    if (produtoAcabado != null)
                    {
                        var hoje = DateTime.Now.Date;
                        var codigoUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

                        var produtoNoCarrinho = desenvController.ObterCarrinhoLinxProduto(produtoAcabado.PRODUTO, produtoAcabado.COR, codigoUsuario);
                        if (produtoNoCarrinho != null)
                            e.Row.BackColor = Color.PaleGreen;

                        var acessorioNoCarrinho = desenvController.ObterCarrinhoLinxAcessorio(produtoAcabado.PRODUTO, produtoAcabado.COR, codigoUsuario);
                        if (acessorioNoCarrinho != null)
                            e.Row.BackColor = Color.PaleGreen;

                        ImageButton imgProduto = e.Row.FindControl("imgProduto") as ImageButton;
                        imgProduto.ImageUrl = produtoAcabado.FOTO1;
                        imgProduto.CommandArgument = produtoAcabado.CODIGO_PRODUTO.ToString() + "|" + produtoAcabado.TIPO.Trim();

                        ImageButton btImprimir = e.Row.FindControl("btImprimir") as ImageButton;
                        btImprimir.CommandArgument = produtoAcabado.PEDIDO_INTRA.Trim() + "|" + produtoAcabado.PRODUTO.Trim() + "|" + produtoAcabado.COR.Trim() + "|" + produtoAcabado.TIPO + "|" + produtoAcabado.ENTREGA.ToString("yyyy-MM-dd");

                        Literal litDataEntrega = e.Row.FindControl("litDataEntrega") as Literal;
                        litDataEntrega.Text = Convert.ToDateTime(produtoAcabado.ENTREGA).ToString("dd/MM/yyyy");

                        Literal litDataPrevEntrega = e.Row.FindControl("litDataPrevEntrega") as Literal;
                        litDataPrevEntrega.Text = Convert.ToDateTime(produtoAcabado.LIMITE_ENTREGA).ToString("dd/MM/yyyy");

                        ImageButton imgExc = e.Row.FindControl("imgExc") as ImageButton;
                        imgExc.CommandArgument = produtoAcabado.PEDIDO_INTRA.Trim() + "|" + produtoAcabado.PRODUTO.Trim() + "|" + produtoAcabado.COR.Trim() + "|" + produtoAcabado.TIPO + "|" + produtoAcabado.ENTREGA.ToString("yyyy-MM-dd");

                        ImageButton imgPagto = e.Row.FindControl("imgPagto") as ImageButton;
                        imgPagto.CommandArgument = produtoAcabado.PEDIDO_INTRA.Trim() + "|" + produtoAcabado.PRODUTO.Trim() + "|" + produtoAcabado.COR.Trim() + "|" + produtoAcabado.TIPO + "|" + produtoAcabado.ENTREGA.ToString("yyyy-MM-dd");

                        if (produtoAcabado.ENTREGA < hoje && produtoAcabado.QTDE_LINX <= 0)
                            e.Row.BackColor = Color.LightGoldenrodYellow;

                        if (produtoAcabado.ENTREGA < hoje && produtoAcabado.ENTREGA != produtoAcabado.LIMITE_ENTREGA && produtoAcabado.QTDE_LINX <= 0)
                            e.Row.BackColor = Color.Pink;

                        Button btAdicionarCarrinho = e.Row.FindControl("btAdicionarCarrinho") as Button;
                        if (produtoAcabado.PEDIDO_LINX != "")
                        {
                            imgExc.Visible = false;
                            btAdicionarCarrinho.Visible = false;
                        }

                        if (produtoAcabado.DATA_EXCLUSAO != null)
                        {
                            e.Row.ForeColor = Color.IndianRed;
                            imgExc.Visible = false;
                            btAdicionarCarrinho.Visible = false;
                        }

                        var dataBloqueio = Convert.ToDateTime("2000-01-01");
                        CheckBox cbComposicao = e.Row.FindControl("cbComposicao") as CheckBox;
                        Literal lit1 = e.Row.FindControl("lit1") as Literal;
                        if (produtoAcabado.DATA_BAIXA_ETI_COMP != null)
                        {
                            cbComposicao.Checked = true;
                            cbComposicao.Enabled = false;

                            if (produtoAcabado.DATA_BAIXA_ETI_COMP.Value.Date == dataBloqueio)
                            {
                                cbComposicao.Visible = false;
                                lit1.Visible = false;
                            }

                        }

                        CheckBox cbPreco = e.Row.FindControl("cbPreco") as CheckBox;
                        Literal lit2 = e.Row.FindControl("lit2") as Literal;
                        if (produtoAcabado.DATA_BAIXA_ETI_BARRA != null)
                        {
                            cbPreco.Checked = true;
                            cbPreco.Enabled = false;
                            if (produtoAcabado.DATA_BAIXA_ETI_BARRA.Value.Date == dataBloqueio)
                            {
                                cbPreco.Visible = false;
                                lit2.Visible = false;
                            }
                        }

                        CheckBox cbTAG = e.Row.FindControl("cbTAG") as CheckBox;
                        Literal lit3 = e.Row.FindControl("lit3") as Literal;
                        if (produtoAcabado.DATA_BAIXA_TAG != null)
                        {
                            cbTAG.Checked = true;
                            cbTAG.Enabled = false;
                            if (produtoAcabado.DATA_BAIXA_TAG.Value.Date == dataBloqueio)
                            {
                                cbTAG.Visible = false;
                                lit3.Visible = false;
                            }
                        }
                        CheckBox cbAviamento = e.Row.FindControl("cbAviamento") as CheckBox;
                        Literal lit4 = e.Row.FindControl("lit4") as Literal;
                        if (produtoAcabado.DATA_BAIXA_AVIAMENTO != null)
                        {
                            cbAviamento.Checked = true;
                            cbAviamento.Enabled = false;
                            if (produtoAcabado.DATA_BAIXA_AVIAMENTO.Value.Date == dataBloqueio)
                            {
                                cbAviamento.Visible = false;
                                lit4.Visible = false;
                            }
                        }

                        TextBox txtObs = e.Row.FindControl("txtObs") as TextBox;
                        txtObs.Text = produtoAcabado.OBS;

                        Literal litValorTotal = e.Row.FindControl("litValorTotal") as Literal;
                        var valTotal = Convert.ToDecimal((produtoAcabado.QTDE_INTRA * produtoAcabado.CUSTO) * 1.00M);
                        litValorTotal.Text = "R$" + valTotal.ToString("###,###,###,##0.00");
                        CriarPagto(produtoAcabado, e.Row);

                        qtdeIntra += produtoAcabado.QTDE_INTRA;
                        qtdeLinx += produtoAcabado.QTDE_LINX;
                        valorTotPgto += valTotal;
                    }
                }
            }

            if (e.Row.DataItemIndex != -1)
            {
                e.Row.Attributes.Add("onMouseover", "this.style.background='#dddddd';this.style.cursor='hand'");
                e.Row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(e.Row.BackColor) + "';this.style.cursor='hand'");
            }
        }
        protected void gvProdutoAcabado_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvProdutoAcabado.FooterRow;
            if (footer != null)
            {
                footer.Cells[2].Text = "Total";

                footer.Cells[12].Text = qtdeIntra.ToString();
                footer.Cells[14].Text = qtdeLinx.ToString();

                footer.Cells[18].Text = valorTotPgto.ToString("###,###,###,##0.00");

            }
        }
        protected void gvProdutoAcabado_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (Session[PROD_ACAB_PEDCOMPRA_PROD] != null)
            {
                IEnumerable<SP_OBTER_PRODUTO_ACABADO_PRODUTO_V10Result> produtoAcabado = (IEnumerable<SP_OBTER_PRODUTO_ACABADO_PRODUTO_V10Result>)Session[PROD_ACAB_PEDCOMPRA_PROD];

                string sortExpression = e.SortExpression;
                SortDirection sort;

                Utils.WebControls.GridViewSortDirection(gvProdutoAcabado, e, out sort);

                if (sort == SortDirection.Ascending)
                    Utils.WebControls.GetBoundFieldIndexByName(gvProdutoAcabado, e.SortExpression, " - >>");
                else
                    Utils.WebControls.GetBoundFieldIndexByName(gvProdutoAcabado, e.SortExpression, " - <<");

                string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

                produtoAcabado = produtoAcabado.OrderBy(e.SortExpression + sortDirection);
                gvProdutoAcabado.DataSource = produtoAcabado;
                gvProdutoAcabado.DataBind();
            }
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

        private void CriarPagto(SP_OBTER_PRODUTO_ACABADO_PRODUTO_V10Result produtoAcabado, GridViewRow row)
        {
            var vermelho = "#B22222";
            var verde = "green";

            var pgto = desenvController.ObterComprasProdutoPrePedidoPagto(produtoAcabado.PEDIDO_INTRA, produtoAcabado.PRODUTO, produtoAcabado.COR, produtoAcabado.ENTREGA);

            Literal litpgto = null;
            var i = 1;
            foreach (var p in pgto)
            {
                var valor = ((p.VALOR <= 0) ? (p.PORC.ToString("##0.00") + "%") : ("R$ " + p.VALOR.ToString("###,###,##0.00")));

                litpgto = row.FindControl("litParc" + i.ToString()) as Literal;

                litpgto.Text = litpgto.Text + " <font color='" + ((p.DATA_BAIXA == null) ? vermelho : verde) + "'>" + valor + " - " + p.DATA_PAGAMENTO.ToString("dd/MM/yyyy") + "<br />" + ((p.DATA_BAIXA == null) ? "Pendente" : "Pago") + "</font> ";
                litpgto.Text = litpgto.Text + " <br /> ";

                i = i + 1;
            }

            for (var ii = i; ii < 10; ii++)
            {
                litpgto = row.FindControl("litParc" + ii.ToString()) as Literal;
                if (litpgto != null)
                    litpgto.Visible = false;
            }

        }

        protected void imgPagto_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton imgPagto = (ImageButton)sender;
                var imgs = imgPagto.CommandArgument.Split('|');
                var pedidoIntra = imgs[0];
                var produto = imgs[1];
                var corProduto = imgs[2];
                var tipo = imgs[3];
                var entrega = Convert.ToDateTime(imgs[4]);


                //Abrir pop-up
                var _url = "fnAbrirTelaCadastro('pacab_pedido_produto_linx_pgto.aspx?ped=" + pedidoIntra + "&prod=" + produto + "&cor=" + corProduto + "&tip=" + tipo + "&en=" + entrega.ToString("yyyy-MM-dd") + "');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _url, true);


            }
            catch (Exception)
            {

            }

        }

        protected void btAdicionarCarrinho_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["USUARIO"] == null)
                {
                    Response.Redirect("~/login.aspx");
                    return;
                }

                labPedido.Text = "";
                labMsg.Text = "";

                Button bt = (Button)(sender);

                GridViewRow row = (GridViewRow)bt.NamingContainer;

                string pedido = gvProdutoAcabado.DataKeys[row.RowIndex][0].ToString();
                string produto = gvProdutoAcabado.DataKeys[row.RowIndex][1].ToString();
                string cor = gvProdutoAcabado.DataKeys[row.RowIndex][2].ToString();
                DateTime entrega = Convert.ToDateTime(gvProdutoAcabado.DataKeys[row.RowIndex][3].ToString());
                string tipo = gvProdutoAcabado.DataKeys[row.RowIndex][4].ToString();

                int codigoUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

                if (tipo == "01") //PEÇAS
                {
                    var produtoNoCarrinho = desenvController.ObterCarrinhoLinxProduto(produto, cor, codigoUsuario);
                    if (produtoNoCarrinho != null)
                    {
                        labPedido.Text = "Produto já foi adicionado.";
                        return;
                    }

                    var produtoTemPedido = desenvController.ObterComprasProdutoPrePedido(pedido, produto, cor, entrega);
                    if (produtoTemPedido != null && produtoTemPedido.PEDIDO_LINX != null)
                    {
                        labPedido.Text = "Produto já possui pedido gerado no LINX: " + produtoTemPedido.PEDIDO_LINX;
                        return;
                    }

                    var carrinho = new DESENV_PRODUTO_CARRINHO_LINX
                    {
                        PEDIDO = pedido,
                        PRODUTO = produto,
                        COR_PRODUTO = cor,
                        ENTREGA = entrega,
                        USUARIO = codigoUsuario,
                    };
                    desenvController.InserirCarrinhoLinxProduto(carrinho);
                }
                else
                {
                    var acessorioNoCarrinho = desenvController.ObterCarrinhoLinxAcessorio(produto, cor, codigoUsuario);
                    if (acessorioNoCarrinho != null)
                    {
                        labPedido.Text = "Acessório já foi adicionado.";
                        return;
                    }

                    var acessorioTemPedido = desenvController.ObterComprasProdutoPrePedido(pedido, produto, cor, entrega);
                    if (acessorioTemPedido != null && acessorioTemPedido.PEDIDO_LINX != null)
                    {
                        labPedido.Text = "Acessório já possui pedido gerado no LINX: " + acessorioTemPedido.PEDIDO_LINX;
                        return;
                    }

                    var carrinho = new DESENV_ACESSORIO_CARRINHO_LINX
                    {
                        PEDIDO = pedido,
                        PRODUTO = produto,
                        COR_PRODUTO = cor,
                        ENTREGA = entrega,
                        USUARIO = codigoUsuario,
                    };
                    desenvController.InserirCarrinhoLinxAcessorio(carrinho);
                }

                row.BackColor = Color.PaleGreen;
                row.Attributes.Add("onMouseover", "this.style.background='#dddddd';this.style.cursor='hand'");
                row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(row.BackColor) + "';this.style.cursor='hand'");

                CarregarCarrinhoLinx();

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        protected void imgExc_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton imgExc = (ImageButton)sender;
                var imgs = imgExc.CommandArgument.Split('|');
                var pedidoIntra = imgs[0];
                var produto = imgs[1];
                var corProduto = imgs[2];
                var tipo = imgs[3];
                var entrega = Convert.ToDateTime(imgs[4]);

                var produtoPrePedido = desenvController.ObterComprasProdutoPrePedido(pedidoIntra, produto, corProduto, entrega);
                if (produtoPrePedido != null)
                {
                    produtoPrePedido.DATA_EXCLUSAO = DateTime.Now;
                    desenvController.AtualizarComprasProdutoPrePedido(produtoPrePedido);
                }

                CarregarProdutoAcabado();
            }
            catch (Exception)
            {

            }
        }
        protected void btAdicionarTodos_Click(object sender, EventArgs e)
        {

            try
            {
                if (Session["USUARIO"] == null)
                {
                    Response.Redirect("~/login.aspx");
                    return;
                }

                if (ddlFilial.SelectedValue == "" && ddlFabricante.SelectedValue == "")
                {
                    labMsg.Text = "Selecione a Filial e/ou o Fornecedor...";
                    return;
                }

                labPedido.Text = "";
                labMsg.Text = "";

                var produtos = ObterProdutoAcabadoProdutoPrePedido();
                foreach (var p in produtos)
                {
                    string pedido = p.PEDIDO_INTRA;
                    string produto = p.PRODUTO;
                    string cor = p.COR;
                    DateTime entrega = Convert.ToDateTime(p.ENTREGA);
                    string tipo = p.TIPO;

                    int codigoUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

                    if (tipo == "01")
                    {
                        var produtoNoCarrinho = desenvController.ObterCarrinhoLinxProduto(produto, cor, codigoUsuario);
                        if (produtoNoCarrinho != null)
                        {
                            //labPedido.Text = "Produto já foi adicionado.";
                            continue;
                        }

                        var produtoTemPedido = desenvController.ObterComprasProdutoPrePedido(pedido, produto, cor, entrega);
                        if (produtoTemPedido != null && produtoTemPedido.PEDIDO_LINX != null)
                        {
                            //labPedido.Text = "Produto já possui pedido gerado no LINX: " + acessorioTemPedido.PEDIDO_LINX;
                            continue;
                        }

                        var carrinho = new DESENV_PRODUTO_CARRINHO_LINX
                        {
                            PEDIDO = pedido,
                            PRODUTO = produto,
                            COR_PRODUTO = cor,
                            ENTREGA = entrega,
                            USUARIO = codigoUsuario,
                        };
                        desenvController.InserirCarrinhoLinxProduto(carrinho);
                    }
                    else
                    {
                        var acessorioNoCarrinho = desenvController.ObterCarrinhoLinxAcessorio(produto, cor, codigoUsuario);
                        if (acessorioNoCarrinho != null)
                        {
                            //labPedido.Text = "Produto já foi adicionado.";
                            continue;
                        }

                        var acessorioTemPedido = desenvController.ObterComprasProdutoPrePedido(pedido, produto, cor, entrega);
                        if (acessorioTemPedido != null && acessorioTemPedido.PEDIDO_LINX != null)
                        {
                            //labPedido.Text = "Produto já possui pedido gerado no LINX: " + acessorioTemPedido.PEDIDO_LINX;
                            continue;
                        }

                        var carrinho = new DESENV_ACESSORIO_CARRINHO_LINX
                        {
                            PEDIDO = pedido,
                            PRODUTO = produto,
                            COR_PRODUTO = cor,
                            ENTREGA = entrega,
                            USUARIO = codigoUsuario,
                        };
                        desenvController.InserirCarrinhoLinxAcessorio(carrinho);
                    }

                }

                CarregarCarrinhoLinx();

            }
            catch (Exception)
            {

                throw;
            }

        }

        protected void btImprimir_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton b = (ImageButton)sender;
            string msg = "";

            if (b != null)
            {
                try
                {

                    var val = b.CommandArgument.Split('|');
                    var pedidoIntra = val[0];
                    var produto = val[1];
                    var corProduto = val[2];
                    var tipo = val[3];
                    var entrega = Convert.ToDateTime(val[4]);

                    var produtoPrePedido = desenvController.ObterComprasProdutoPrePedido(pedidoIntra, produto, corProduto, entrega);

                    GerarRelatorio(produtoPrePedido);

                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                }
            }
        }

        protected void cbItens_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox cb = (CheckBox)sender;
                GridViewRow row = (GridViewRow)cb.NamingContainer;

                var pedido = gvProdutoAcabado.DataKeys[row.RowIndex][0].ToString();
                var produto = gvProdutoAcabado.DataKeys[row.RowIndex][1].ToString();
                var cor = gvProdutoAcabado.DataKeys[row.RowIndex][2].ToString();
                var entrega = Convert.ToDateTime(gvProdutoAcabado.DataKeys[row.RowIndex][3].ToString());

                var prePedido = desenvController.ObterComprasProdutoPrePedido(pedido, produto, cor, entrega);
                if (prePedido != null)
                {
                    var codigoUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

                    if (cb.ID.ToLower().Contains("composicao"))
                    {
                        prePedido.DATA_BAIXA_ETI_COMP = DateTime.Now;
                        prePedido.USUARIO_BAIXA_ETI_COMP = codigoUsuario;
                    }
                    else if (cb.ID.ToLower().Contains("preco"))
                    {
                        prePedido.DATA_BAIXA_ETI_BARRA = DateTime.Now;
                        prePedido.USUARIO_BAIXA_ETI_BARRA = codigoUsuario;
                    }
                    else if (cb.ID.ToLower().Contains("tag"))
                    {
                        prePedido.DATA_BAIXA_TAG = DateTime.Now;
                        prePedido.USUARIO_BAIXA_TAG = codigoUsuario;
                    }
                    else if (cb.ID.ToLower().Contains("aviamento"))
                    {
                        prePedido.DATA_BAIXA_AVIAMENTO = DateTime.Now;
                        prePedido.USUARIO_BAIXA_AVIAMENTO = codigoUsuario;
                    }

                    desenvController.AtualizarComprasProdutoPrePedido(prePedido);
                    cb.Enabled = false;
                }


            }
            catch (Exception)
            {
            }
        }
        protected void txtObs_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox txt = (TextBox)sender;
                GridViewRow row = (GridViewRow)txt.NamingContainer;

                var pedido = gvProdutoAcabado.DataKeys[row.RowIndex][0].ToString();
                var produto = gvProdutoAcabado.DataKeys[row.RowIndex][1].ToString();
                var cor = gvProdutoAcabado.DataKeys[row.RowIndex][2].ToString();
                var entrega = Convert.ToDateTime(gvProdutoAcabado.DataKeys[row.RowIndex][3].ToString());

                var prePedido = desenvController.ObterComprasProdutoPrePedido(pedido, produto, cor, entrega);
                if (prePedido != null)
                {
                    prePedido.OBS = txt.Text;
                    desenvController.AtualizarComprasProdutoPrePedido(prePedido);
                }


            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region "CARRINHO"
        private void CarregarCarrinhoLinx()
        {
            var codigoUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

            var carrinho = desenvController.ObterComprasCarrinhoLinx(codigoUsuario);
            gvCarrinho.DataSource = carrinho;
            gvCarrinho.DataBind();
        }
        protected void gvCarrinho_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_PRODUTO_ACABADO_CARLINXResult carrinho = e.Row.DataItem as SP_OBTER_PRODUTO_ACABADO_CARLINXResult;

                    if (carrinho != null)
                    {

                        Literal litProduto = e.Row.FindControl("litProduto") as Literal;
                        litProduto.Text = carrinho.PRODUTO;

                        Literal litNome = e.Row.FindControl("litNome") as Literal;
                        litNome.Text = carrinho.DESC_PRODUTO;

                        Literal litCor = e.Row.FindControl("litCor") as Literal;
                        litCor.Text = carrinho.DESC_COR;

                        Literal litFornecedor = e.Row.FindControl("litFornecedor") as Literal;
                        litFornecedor.Text = carrinho.FORNECEDOR;

                        Literal litCusto = e.Row.FindControl("litCusto") as Literal;
                        litCusto.Text = (carrinho.CUSTO == null) ? "" : ("R$ " + carrinho.CUSTO.ToString());

                        TextBox txtCustoNota = e.Row.FindControl("txtCustoNota") as TextBox;
                        txtCustoNota.Text = (carrinho.CUSTO_NOTA == null) ? "" : (carrinho.CUSTO_NOTA.ToString());

                        var pPrePedido = desenvController.ObterComprasProdutoPrePedido(carrinho.PEDIDO, carrinho.PRODUTO, carrinho.COR_PRODUTO, carrinho.ENTREGA);
                        Literal litGradeTotal = e.Row.FindControl("litGradeTotal") as Literal;
                        litGradeTotal.Text = pPrePedido.QTDE_ORIGINAL.ToString();

                        Literal litValTotal = e.Row.FindControl("litValTotal") as Literal;
                        var custo = Convert.ToDecimal((carrinho.CUSTO_NOTA != null) ? carrinho.CUSTO_NOTA : ((carrinho.CUSTO != null) ? carrinho.CUSTO : 0.00M));
                        var valor = Convert.ToDecimal(pPrePedido.QTDE_ORIGINAL) * custo;
                        litValTotal.Text = "R$ " + (valor).ToString("###,###,###,##0.00");

                        ImageButton _btAdicionarGrade = e.Row.FindControl("btAdicionarGrade") as ImageButton;
                        if (_btAdicionarGrade != null)
                            _btAdicionarGrade.CommandArgument = carrinho.CODIGO.ToString() + "|" + carrinho.TIPO;

                        ImageButton _btExcluirCarrinho = e.Row.FindControl("btExcluirItemCarrinho") as ImageButton;
                        if (_btExcluirCarrinho != null)
                            _btExcluirCarrinho.CommandArgument = carrinho.CODIGO.ToString() + "|" + carrinho.TIPO;

                        qtdeCar += pPrePedido.QTDE_ORIGINAL;
                        valCar += valor;

                    }
                }
            }

            if (e.Row.DataItemIndex != -1)
            {
                e.Row.Attributes.Add("onMouseover", "this.style.background='#dddddd';this.style.cursor='hand'");
                e.Row.Attributes.Add("onMouseout", "this.style.background='" + System.Drawing.ColorTranslator.ToHtml(e.Row.BackColor) + "';this.style.cursor='hand'");
            }
        }
        protected void gvCarrinho_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvCarrinho.FooterRow;
            if (footer != null)
            {
                footer.Cells[1].Text = "Total";

                footer.Cells[7].Text = qtdeCar.ToString();
                footer.Cells[8].Text = "R$ " + valCar.ToString("###,###,##0.00");
            }
        }
        protected void btExcluirCarrinho_Click(object sender, EventArgs e)
        {
            try
            {
                var codigoUsuario = ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;
                desenvController.ExcluirCarrinhoLinxProdutoPorUsuario(codigoUsuario);
                desenvController.ExcluirCarrinhoLinxAcessorioPorUsuario(codigoUsuario);
                CarregarCarrinhoLinx();
            }
            catch (Exception)
            {
            }
        }
        protected void btAtualizarCarrinho_Click(object sender, EventArgs e)
        {
            try
            {
                labPedido.Text = "";
                labMsg.Text = "";
                CarregarCarrinhoLinx();
            }
            catch (Exception)
            {

                throw;
            }
        }
        protected void txtCustoNota_TextChanged(object sender, EventArgs e)
        {
            try
            {

                TextBox txt = (TextBox)sender;
                GridViewRow row = (GridViewRow)txt.NamingContainer;

                string produto = gvCarrinho.DataKeys[row.RowIndex][1].ToString();
                string tipo = gvCarrinho.DataKeys[row.RowIndex][3].ToString();
                if (tipo == "01")
                {
                    var produtos = desenvController.ObterProdutoPorProduto(produto);
                    foreach (var p in produtos)
                    {
                        var pp = desenvController.ObterProduto(p.CODIGO);
                        pp.CUSTO_NOTA = Convert.ToDecimal(txt.Text.Trim());
                        desenvController.AtualizarProduto(pp);
                    }
                }
                else
                {
                    var acessorios = desenvController.ObterAcessorioPorProduto(produto);
                    foreach (var p in acessorios)
                    {
                        var pp = desenvController.ObterAcessorio(p.CODIGO);
                        pp.CUSTO_NOTA = Convert.ToDecimal(txt.Text.Trim());
                        desenvController.AtualizarAcessorio(pp);
                    }
                }

                CarregarCarrinhoLinx();
            }
            catch (Exception)
            {
            }

        }

        protected void btExcluirItemCarrinho_Click(object sender, EventArgs e)
        {
            try
            {
                ImageButton bt = (ImageButton)(sender);

                var vals = bt.CommandArgument.Split('|');

                var codigoCarrinho = Convert.ToInt32(vals[0].ToString());
                var tipo = vals[1];

                if (tipo == "01")
                    desenvController.ExcluirCarrinhoLinxProdutoPorCodigo(codigoCarrinho);
                else
                    desenvController.ExcluirCarrinhoLinxAcessorioPorCodigo(codigoCarrinho);

                CarregarCarrinhoLinx();
            }
            catch (Exception)
            {
            }
        }
        protected void btAdicionarGrade_Click(object sender, EventArgs e)
        {
            string msg = "";
            string _url = "";

            try
            {
                ImageButton bt = (ImageButton)(sender);

                var vals = bt.CommandArgument.Split('|');

                var codigoCarrinho = Convert.ToInt32(vals[0].ToString());
                var tipo = vals[1];

                if (tipo == "01")
                {
                    //Abrir pop-up
                    _url = "fnAbrirTelaCadastroMaior('pacab_pedido_produto_grade_linxv2.aspx?c=" + codigoCarrinho.ToString() + "');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _url, true);
                }
                else
                {
                    //Abrir pop-up
                    _url = "fnAbrirTelaCadastroMaior('pacab_pedido_acessorio_grade_linx.aspx?c=" + codigoCarrinho.ToString() + "');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _url, true);
                }

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
            }

        }

        protected void btGerarPedido_Click(object sender, EventArgs e)
        {

            Button b = (Button)sender;
            string msg = "";
            string _url = "";
            if (b != null)
            {
                try
                {
                    if (gvCarrinho.Rows.Count > 0)
                    {
                        //Abrir pop-up
                        _url = "fnAbrirTelaCadastroMaior('pacab_pedido_produto_linx_gerarv10.aspx');";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _url, true);
                    }

                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: 'ERRO', life: 5000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);
                }
            }
        }

        #endregion

        #region "RELATORIO"
        private void GerarRelatorio(COMPRAS_PRODUTO_PREPEDIDO compraProdutoPedido)
        {
            StreamWriter wr = null;
            try
            {
                string nomeArquivo = "PROD_ACAB_" + compraProdutoPedido.PEDIDO_LINX + "-" + compraProdutoPedido.PRODUTO + "-" + compraProdutoPedido.ENTREGA.ToString("ddMMyyyy") + "_" + ((USUARIO)Session["USUARIO"]).CODIGO_USUARIO + ".html";
                wr = new StreamWriter(Server.MapPath("") + "\\html\\" + nomeArquivo);
                wr.Write(MontarRelatorioHTML(compraProdutoPedido));
                wr.Flush();

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "AbrirImpressao('" + nomeArquivo + "')", true);
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
        private StringBuilder MontarCabecalho(StringBuilder _texto)
        {
            _texto.Append("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            _texto.Append(" <html>");
            _texto.Append("     <head>");
            _texto.Append("         <title>Produto Acabado - Ficha</title>   ");
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


            return _texto;
        }
        private StringBuilder MontarRodape(StringBuilder _texto)
        {
            _texto.Append("</body>");
            _texto.Append("</html>");

            return _texto;
        }
        private StringBuilder MontarRelatorioHTML(COMPRAS_PRODUTO_PREPEDIDO compraProdutoPedido)
        {
            StringBuilder _texto = new StringBuilder();

            _texto = MontarCabecalho(_texto);
            _texto = MontarLogisticaProdutoAcabado(_texto, compraProdutoPedido);
            _texto = MontarRodape(_texto);

            return _texto;
        }
        public StringBuilder MontarLogisticaProdutoAcabado(StringBuilder _texto, COMPRAS_PRODUTO_PREPEDIDO compraProdutoPedido)
        {
            ProducaoController prodController = new ProducaoController();
            BaseController baseController = new BaseController();

            int contLinha = 0;
            int tLinhaVarejo = 7;
            int tLinhaAtacado = 3;
            int tLinhaControleQ = 6;

            var desenvProduto = new DESENV_PRODUTO();
            var modelo = compraProdutoPedido.PRODUTO.Trim();
            var cor = compraProdutoPedido.COR_PRODUTO.Trim();

            if (compraProdutoPedido.COD_CATEGORIA == "01")
            {
                desenvProduto = desenvController.ObterProdutoCor(compraProdutoPedido.PRODUTO, compraProdutoPedido.COR_PRODUTO).OrderByDescending(p => p.DATA_INCLUSAO).FirstOrDefault();
                if (desenvProduto != null)
                {
                    modelo = desenvProduto.MODELO.Trim();
                    cor = desenvProduto.COR.Trim();
                }
            }
            else
            {
                var desenvAcessorio = desenvController.ObterAcessorio(compraProdutoPedido.PRODUTO, compraProdutoPedido.COR_PRODUTO);
                if (desenvAcessorio != null)
                {
                    modelo = desenvAcessorio.PRODUTO;
                    cor = desenvAcessorio.COR.Trim();

                    desenvProduto.COLECAO = desenvAcessorio.COLECAO;
                    desenvProduto.MODELO = modelo;
                    desenvProduto.COR = cor;
                    desenvProduto.FORNECEDOR = desenvAcessorio.FORNECEDOR;
                    desenvProduto.FOTO = desenvAcessorio.FOTO1;
                    desenvProduto.FOTO2 = desenvAcessorio.FOTO2;

                    desenvProduto.GRIFFE = desenvAcessorio.GRIFFE;
                    desenvProduto.GRUPO = desenvAcessorio.GRUPO;
                    desenvProduto.DESC_MODELO = desenvAcessorio.DESCRICAO_SUGERIDA;

                }
            }

            string pcVarejo = "-";
            string pcAtacado = "-";
            string pcMostruario = "-";

            string dataPedidoVarejo = "-";
            string dataPedidoAtacado = "-";
            string dataPedidoMostruario = "-";

            string dataEntregaVarejo = "-";
            string dataEntregaAtacado = "-";
            string dataEntregaMostruario = "-";

            string custo = "";
            string valorTotal = "";
            string fornecedor = desenvProduto.FORNECEDOR.Trim();

            int qtdeTotal = 0;

            SP_OBTER_PRODUTO_COMPRA_INTRANET_TAMResult atacadoQtde = null;
            SP_OBTER_PRODUTO_COMPRA_INTRANET_TAMResult varejoQtde = null;
            SP_OBTER_PRODUTO_COMPRA_INTRANET_TAMResult mostruarioQtde = null;

            char mostruario = (ddlFilial.SelectedItem.Text.Trim().ToLower().Contains("mostruario")) ? 'S' : 'N';

            var pedidoLinx = desenvController.ObterProdutoAcabadoIntranet(compraProdutoPedido.PEDIDO, compraProdutoPedido.PRODUTO, compraProdutoPedido.COR_PRODUTO, compraProdutoPedido.ENTREGA, mostruario);
            var obs = "-";
            if (pedidoLinx != null && pedidoLinx.Count() > 0)
            {
                var m = pedidoLinx.Where(p => p.FILIAL.Trim().Contains("CD MOSTRUARIO")).LastOrDefault();
                if (m != null)
                {
                    pcMostruario = m.PEDIDO;
                    dataPedidoMostruario = m.EMISSAO.ToString("dd/MM/yyyy");
                    dataEntregaMostruario = (m.LIMITE_ENTREGA == null) ? "-" : Convert.ToDateTime(m.LIMITE_ENTREGA).ToString("dd/MM/yyyy");
                    custo = (m.CUSTO1 == null) ? "-" : Convert.ToDecimal(m.CUSTO1).ToString("###,###,##0.00");
                    fornecedor = m.FORNECEDOR;
                    obs = m.OBS;
                }

                var a = pedidoLinx.Where(p => p.FILIAL.Trim().Contains("ATACADO HANDBOOK")).LastOrDefault();
                if (a != null)
                {
                    pcAtacado = a.PEDIDO;
                    dataPedidoAtacado = a.EMISSAO.ToString("dd/MM/yyyy");
                    dataEntregaAtacado = (a.LIMITE_ENTREGA == null) ? "-" : Convert.ToDateTime(a.LIMITE_ENTREGA).ToString("dd/MM/yyyy");
                    custo = (a.CUSTO1 == null) ? "-" : Convert.ToDecimal(a.CUSTO1).ToString("###,###,##0.00");
                    fornecedor = a.FORNECEDOR;
                    obs = a.OBS;
                }

                var v = pedidoLinx.Where(p => p.FILIAL.Trim().Contains("CD - LUGZY")).LastOrDefault();
                if (v != null)
                {
                    pcVarejo = v.PEDIDO;
                    dataPedidoVarejo = v.EMISSAO.ToString("dd/MM/yyyy");
                    dataEntregaVarejo = (v.LIMITE_ENTREGA == null) ? "-" : Convert.ToDateTime(v.LIMITE_ENTREGA).ToString("dd/MM/yyyy");
                    custo = (v.CUSTO1 == null) ? "-" : Convert.ToDecimal(v.CUSTO1).ToString("###,###,##0.00");
                    fornecedor = v.FORNECEDOR;
                    obs = v.OBS;
                }

                atacadoQtde = desenvController.ObterProdutoAcabadoIntranetTamanho(pcAtacado, modelo, cor, compraProdutoPedido.ENTREGA);
                varejoQtde = desenvController.ObterProdutoAcabadoIntranetTamanho(pcVarejo, modelo, cor, compraProdutoPedido.ENTREGA);
                mostruarioQtde = desenvController.ObterProdutoAcabadoIntranetTamanho(pcMostruario, modelo, cor, compraProdutoPedido.ENTREGA);

                qtdeTotal = Convert.ToInt32(((atacadoQtde != null) ? atacadoQtde.QTDE_ORIGINAL : 0) + ((varejoQtde != null) ? varejoQtde.QTDE_ORIGINAL : 0) + ((mostruarioQtde != null) ? mostruarioQtde.QTDE_ORIGINAL : 0));

                valorTotal = "R$ " + Convert.ToDecimal((((((atacadoQtde != null) ? atacadoQtde.QTDE_ORIGINAL : 0) + ((varejoQtde != null) ? varejoQtde.QTDE_ORIGINAL : 0) + ((mostruarioQtde != null) ? mostruarioQtde.QTDE_ORIGINAL : 0)))) * Convert.ToDecimal(((custo == "-") ? "0" : custo))).ToString("###,###,###,##0.00");

            }

            _texto.AppendLine(" <br />");
            _texto.AppendLine("    <span>LOGÍSTICA - PRODUTO ACABADO</span>");
            _texto.AppendLine("<div id='divLog' align='center'>");
            _texto.AppendLine("        <table border='0' cellpadding='0' cellspacing='0' style='width: 517pt; padding: 0px;");
            _texto.AppendLine("            color: black; font-size: 9.0pt; font-weight: 700; font-family: Arial, sans-serif;");
            _texto.AppendLine("            background: white; white-space: nowrap;'>");
            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td style='line-height: 20px;'>");
            _texto.AppendLine("                    <table border='0' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
            _texto.AppendLine("                        width: 517pt'>");
            _texto.AppendLine("                        <tr style='text-align: left; border-top: 2px solid #000; border-bottom: none;'>");
            _texto.AppendLine("                            <td rowspan='7' style='border: 1px solid #000;'>");
            _texto.AppendLine("                                <img alt='Foto Peça' width='90' height='120' src='..\\.." + desenvProduto.FOTO.Replace("~", "").Replace("/", "\\") + "' />");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 184px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;" + ((mostruario == 'S') ? "MOSTRUÁRIO" : "PRODUÇÃO"));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 200px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;COLEÇÃO: " + baseController.BuscaColecaoAtual(desenvProduto.COLECAO).DESC_COLECAO.Trim());
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td colspan='2' style='border-right: 1px solid #000; border-bottom: 1px dotted #FFF;'>");
            _texto.AppendLine("                                &nbsp;PRODUTO ACABADO");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                        <tr style='border: 1px solid #000;'>");
            _texto.AppendLine("                            <td style='border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;QTDE: " + qtdeTotal.ToString());
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;NOME: " + desenvProduto.GRUPO + " " + desenvProduto.DESC_MODELO.Replace(desenvProduto.GRUPO, ""));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td colspan='2' style='border-right: 1px solid #000; border-bottom: 1px dotted #FFF;'>");
            _texto.AppendLine("                                &nbsp;GRIFFE: " + desenvProduto.GRIFFE);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                        <tr style='border: 1px solid #000;'>");
            _texto.AppendLine("                            <td style='border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;PRODUTO: " + modelo);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;COR: " + cor + " - " + prodController.ObterCoresBasicas(cor).DESC_COR.Trim());
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;CUSTO: " + custo);
            _texto.AppendLine("                            </td>");

            decimal preco = 0;
            var precoTO = baseController.BuscaPrecoOriginalProduto(modelo);
            if (precoTO != null && precoTO.PRECO1 > 0)
                preco = Convert.ToDecimal(precoTO.PRECO1);
            else
            {
                var precoTL = baseController.BuscaPrecoLojaProduto(modelo);
                if (precoTL != null && precoTL.PRECO1 > 0)
                    preco = Convert.ToDecimal(precoTL.PRECO1);
            }


            _texto.AppendLine("                            <td style='border-right: 1px solid #000; border-bottom: 1px dotted #FFF;'>");
            _texto.AppendLine("                                 &nbsp;VENDA: R$ " + preco.ToString("###,###,###,##0.00"));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");

            _texto.AppendLine("                        <tr style='border: 1px solid #000;'>");
            _texto.AppendLine("                            <td style='border-left: 1px solid #000; border-right: 1px solid #000;' colspan='2'>");
            _texto.AppendLine("                                &nbsp;FORNECEDOR: " + desenvProduto.FORNECEDOR.Trim());
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; border-bottom: 1px dotted #FFF;' colspan='2'>");
            _texto.AppendLine("                                &nbsp;Total: " + valorTotal);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");

            _texto.AppendLine("                        <tr style='border: 1px solid #000;'>");
            _texto.AppendLine("                            <td style='border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;P.C. Mostruário: " + pcMostruario);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;Data Pedido: " + dataPedidoMostruario);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; border-bottom: 1px dotted #FFF;' colspan='2'>");
            _texto.AppendLine("                                &nbsp;Entrega: " + dataEntregaMostruario);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("");
            _texto.AppendLine("                        <tr style='border: 1px solid #000;'>");
            _texto.AppendLine("                            <td style='border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;P.C. Atacado: " + pcAtacado);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;Data Pedido: " + dataPedidoAtacado);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; border-bottom: 1px dotted #FFF;' colspan='2'>");
            _texto.AppendLine("                                &nbsp;Entrega: " + dataEntregaAtacado);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("");
            _texto.AppendLine("                        <tr style='border: 1px solid #000;'>");
            _texto.AppendLine("                            <td style='border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;P.C. Varejo: " + pcVarejo);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;Data Pedido: " + dataPedidoVarejo);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; border-bottom: 1px dotted #FFF;' colspan='2'>");
            _texto.AppendLine("                                &nbsp;Entrega: " + dataEntregaVarejo);
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");


            _texto.AppendLine("                    </table>");
            _texto.AppendLine("                    <table border='0' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
            _texto.AppendLine("                        width: 517pt'>");
            _texto.AppendLine("                        <tr>");
            _texto.AppendLine("                            <td colspan='4'>");
            _texto.AppendLine("                                <table border='0' cellpadding='0' cellspacing='0' width='689' style='border-collapse: collapse; width: 517pt;'>");
            _texto.AppendLine("                                    <tr style='text-align: center; border-top: 1px solid #000;'>");
            _texto.AppendLine("                                        <td style=' text-align: left; border-right: 1px solid #000; border-left: 1px solid #000;'>");
            _texto.AppendLine("                                            &nbsp;");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                            -");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                            " + ((varejoQtde != null) ? varejoQtde.TAMANHO_1 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_1 : "EXP")));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                            " + ((varejoQtde != null) ? varejoQtde.TAMANHO_2 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_2 : "XP")));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                            " + ((varejoQtde != null) ? varejoQtde.TAMANHO_3 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_3 : "PP")));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                            " + ((varejoQtde != null) ? varejoQtde.TAMANHO_4 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_4 : "P")));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                            " + ((varejoQtde != null) ? varejoQtde.TAMANHO_5 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_5 : "M")));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                            " + ((varejoQtde != null) ? varejoQtde.TAMANHO_6 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_6 : "G")));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                            " + ((varejoQtde != null) ? varejoQtde.TAMANHO_7 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_7 : "GG")));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                            TOTAL");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
            _texto.AppendLine("                                        <td style='text-align: left; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            &nbsp;MOSTRUÁRIO");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            ");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + ((mostruarioQtde != null) ? mostruarioQtde.CO1.ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + ((mostruarioQtde != null) ? mostruarioQtde.CO2.ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + ((mostruarioQtde != null) ? mostruarioQtde.CO3.ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + ((mostruarioQtde != null) ? mostruarioQtde.CO4.ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + ((mostruarioQtde != null) ? mostruarioQtde.CO5.ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + ((mostruarioQtde != null) ? mostruarioQtde.CO6.ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + ((mostruarioQtde != null) ? mostruarioQtde.CO7.ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td>");
            _texto.AppendLine("                                            " + ((mostruarioQtde != null) ? (mostruarioQtde.CO1 + mostruarioQtde.CO2 + mostruarioQtde.CO3 + mostruarioQtde.CO4 + mostruarioQtde.CO5 + mostruarioQtde.CO6 + mostruarioQtde.CO7).ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
            _texto.AppendLine("                                        <td style='text-align: left; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            &nbsp;VAREJO");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            ");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + ((varejoQtde != null) ? varejoQtde.CO1.ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + ((varejoQtde != null) ? varejoQtde.CO2.ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + ((varejoQtde != null) ? varejoQtde.CO3.ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + ((varejoQtde != null) ? varejoQtde.CO4.ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + ((varejoQtde != null) ? varejoQtde.CO5.ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + ((varejoQtde != null) ? varejoQtde.CO6.ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + ((varejoQtde != null) ? varejoQtde.CO7.ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td>");
            _texto.AppendLine("                                            " + ((varejoQtde != null) ? (varejoQtde.CO1 + varejoQtde.CO2 + varejoQtde.CO3 + varejoQtde.CO4 + varejoQtde.CO5 + varejoQtde.CO6 + varejoQtde.CO7).ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
            _texto.AppendLine("                                        <td style='text-align: left; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            &nbsp;ATACADO");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            ");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + ((atacadoQtde != null) ? atacadoQtde.CO1.ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + ((atacadoQtde != null) ? atacadoQtde.CO2.ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + ((atacadoQtde != null) ? atacadoQtde.CO3.ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + ((atacadoQtde != null) ? atacadoQtde.CO4.ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + ((atacadoQtde != null) ? atacadoQtde.CO5.ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + ((atacadoQtde != null) ? atacadoQtde.CO6.ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + ((atacadoQtde != null) ? atacadoQtde.CO7.ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td>");
            _texto.AppendLine("                                            " + ((atacadoQtde != null) ? (atacadoQtde.CO1 + atacadoQtde.CO2 + atacadoQtde.CO3 + atacadoQtde.CO4 + atacadoQtde.CO5 + atacadoQtde.CO6 + atacadoQtde.CO7).ToString() : ""));
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                    <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
            _texto.AppendLine("                                        <td style='text-align: left; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            &nbsp;TOTAL");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            ");
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + (((varejoQtde != null) ? varejoQtde.CO1 : 0) + ((atacadoQtde != null) ? atacadoQtde.CO1 : 0) + ((mostruarioQtde != null) ? mostruarioQtde.CO1 : 0)).ToString());
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + (((varejoQtde != null) ? varejoQtde.CO2 : 0) + ((atacadoQtde != null) ? atacadoQtde.CO2 : 0) + ((mostruarioQtde != null) ? mostruarioQtde.CO2 : 0)).ToString());
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + (((varejoQtde != null) ? varejoQtde.CO3 : 0) + ((atacadoQtde != null) ? atacadoQtde.CO3 : 0) + ((mostruarioQtde != null) ? mostruarioQtde.CO3 : 0)).ToString());
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + (((varejoQtde != null) ? varejoQtde.CO4 : 0) + ((atacadoQtde != null) ? atacadoQtde.CO4 : 0) + ((mostruarioQtde != null) ? mostruarioQtde.CO4 : 0)).ToString());
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + (((varejoQtde != null) ? varejoQtde.CO5 : 0) + ((atacadoQtde != null) ? atacadoQtde.CO5 : 0) + ((mostruarioQtde != null) ? mostruarioQtde.CO5 : 0)).ToString());
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + (((varejoQtde != null) ? varejoQtde.CO6 : 0) + ((atacadoQtde != null) ? atacadoQtde.CO6 : 0) + ((mostruarioQtde != null) ? mostruarioQtde.CO6 : 0)).ToString());
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                            " + (((varejoQtde != null) ? varejoQtde.CO7 : 0) + ((atacadoQtde != null) ? atacadoQtde.CO7 : 0) + ((mostruarioQtde != null) ? mostruarioQtde.CO7 : 0)).ToString());
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                        <td>");
            _texto.AppendLine("                                            " +
                                        (((varejoQtde != null) ? (varejoQtde.CO1 + varejoQtde.CO2 + varejoQtde.CO3 + varejoQtde.CO4 + varejoQtde.CO5 + varejoQtde.CO6 + varejoQtde.CO7) : 0)
                                        +
                                        ((mostruarioQtde != null) ? (mostruarioQtde.CO1 + mostruarioQtde.CO2 + mostruarioQtde.CO3 + mostruarioQtde.CO4 + mostruarioQtde.CO5 + mostruarioQtde.CO6 + mostruarioQtde.CO7) : 0)
                                        +
                                        ((atacadoQtde != null) ? (atacadoQtde.CO1 + atacadoQtde.CO2 + atacadoQtde.CO3 + atacadoQtde.CO4 + atacadoQtde.CO5 + atacadoQtde.CO6 + atacadoQtde.CO7) : 0)).ToString()

                );
            _texto.AppendLine("                                        </td>");
            _texto.AppendLine("                                    </tr>");
            _texto.AppendLine("                                </table>");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                    </table>");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");
            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td style='border: 1px solid #FFF;'>");
            _texto.AppendLine("                    &nbsp;");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");

            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td style='border: 1px solid #FFF;'>");
            _texto.AppendLine("                    &nbsp;");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");
            //VAREJO
            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td>");
            _texto.AppendLine("                    <table border='0' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
            _texto.AppendLine("                        width: 100%'>");
            _texto.AppendLine("                        <tr style='text-align: center; border-top: 1px solid #000;'>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; border-left: 1px solid #000;'>");
            _texto.AppendLine("                                <span style='font-size: 11px;'>ESTOQUE VAREJO</span>");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                -");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_1 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_1 : "EXP")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_2 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_2 : "XP")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_3 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_3 : "PP")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_4 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_4 : "P")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_5 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_5 : "M")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_6 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_6 : "G")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_7 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_7 : "GG")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                TOTAL");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            //INICIO LOOP ESTOQUE VAREJO
            for (contLinha = 1; contLinha <= tLinhaVarejo; contLinha++)
            {
                _texto.AppendLine("                        <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                        </tr>");
            }
            // FIM LOOP ESTOQUE VAREJO

            _texto.AppendLine("                        <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                TOTAL FINAL");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                    </table>");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");
            _texto.AppendLine("            <tr style='border: 1px solid #FFF;'>");
            _texto.AppendLine("                <td style='border: 1px solid #FFF;'>");
            _texto.AppendLine("                    &nbsp;");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");

            //ATACADO
            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td>");
            _texto.AppendLine("                    <table border='0' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
            _texto.AppendLine("                        width: 100%'>");
            _texto.AppendLine("                        <tr style='text-align: center; border-top: 1px solid #000;'>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; border-left: 1px solid #000;'>");
            _texto.AppendLine("                                <span style='font-size: 11px;'>ESTOQUE ATACADO</span>");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                ");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_1 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_1 : "EXP")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_2 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_2 : "XP")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_3 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_3 : "PP")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_4 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_4 : "P")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_5 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_5 : "M")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_6 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_6 : "G")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_7 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_7 : "GG")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                TOTAL");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            //INICIO LOOP ESTOQUE ATACADO
            for (contLinha = 1; contLinha <= tLinhaAtacado; contLinha++)
            {
                _texto.AppendLine("                        <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                        </tr>");
            }
            // FIM LOOP ESTOQUE ATACADO

            _texto.AppendLine("                        <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                TOTAL FINAL");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                    </table>");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");
            _texto.AppendLine("            <tr style='border: 1px solid #FFF;'>");
            _texto.AppendLine("                <td style='border: 1px solid #FFF;'>");
            _texto.AppendLine("                    &nbsp;");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");

            //CONTROLE DE QUALIDADE
            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td>");
            _texto.AppendLine("                    <table border='0' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
            _texto.AppendLine("                        width: 100%'>");
            _texto.AppendLine("                        <tr style='text-align: center; border-top: 1px solid #000;'>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000; border-left: 1px solid #000;'>");
            _texto.AppendLine("                                <span style='font-size: 11px;'>CONTR QUALIDADE</span>");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                ");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_1 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_1 : "EXP")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_2 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_2 : "XP")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_3 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_3 : "PP")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_4 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_4 : "P")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_5 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_5 : "M")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_6 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_6 : "G")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_7 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_7 : "GG")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                TOTAL");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            //INICIO LOOP CONTROLE DE QUALIDADE
            for (contLinha = 1; contLinha <= tLinhaControleQ; contLinha++)
            {
                _texto.AppendLine("                        <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                            <td>");
                _texto.AppendLine("                                &nbsp;");
                _texto.AppendLine("                            </td>");
                _texto.AppendLine("                        </tr>");
            }
            // FIM LOOP CONTROLE DE QUALIDADE

            _texto.AppendLine("                        <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                TOTAL FINAL");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                    </table>");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");
            _texto.AppendLine("            <tr style='border: 1px solid #FFF;'>");
            _texto.AppendLine("                <td style='border: 1px solid #FFF;'>");
            _texto.AppendLine("                    &nbsp;");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");

            //RODAPE
            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td>");
            _texto.AppendLine("                    <table border='0' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
            _texto.AppendLine("                        width: 100%'>");
            _texto.AppendLine("                        <tr style='text-align: center; line-height: 18px;'>");
            _texto.AppendLine("                            <td style='border: 1px solid #000;'>");
            _texto.AppendLine("                                PILOTO");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border: 1px solid #000;'>");
            _texto.AppendLine("                                MOSTRUÁRIO");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td colspan='8' style='text-align:left;'>");
            _texto.AppendLine("                                SCS - 2ª QUALIDADE");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                        <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
            _texto.AppendLine("                            <td style='width: 120px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 120px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-top: 1px solid #fff; border-bottom: 1px solid #fff;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_1 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_1 : "EXP")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_2 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_2 : "XP")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_3 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_3 : "PP")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_4 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_4 : "P")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_5 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_5 : "M")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_6 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_6 : "G")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000; '>");
            _texto.AppendLine("                                " + ((varejoQtde != null) ? varejoQtde.TAMANHO_7 : ((atacadoQtde != null) ? atacadoQtde.TAMANHO_7 : "GG")));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                TOTAL");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                        <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
            _texto.AppendLine("                            <td style='width: 120px; border-top: 1px solid #fff; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 120px; border-top: 1px solid #fff; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-top: 1px solid #fff; border-bottom: 1px solid #fff;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                        <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
            _texto.AppendLine("                            <td style='border: 1px solid #000;'>");
            _texto.AppendLine("                                PERDA");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border: 1px solid #000;'>");
            _texto.AppendLine("                                AMOSTRA");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-top: 1px solid #fff; border-bottom: 1px solid #fff;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                        <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
            _texto.AppendLine("                            <td style='width: 120px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 120px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-top: 1px solid #fff; border-bottom: 1px solid #fff;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                        <tr style='border: 1px solid #000; text-align: center; line-height: 18px;'>");
            _texto.AppendLine("                            <td style='width: 120px; border-top: 1px solid #fff; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 120px; border-top: 1px solid #fff; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='border-top: 1px solid #fff; border-bottom: 1px solid #fff;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                            <td style='width: 62px; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                &nbsp;");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                    </table>");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");
            _texto.AppendLine("            <tr style='border: 1px solid #FFF;'>");
            _texto.AppendLine("                <td style='border: 1px solid #FFF;'>");
            _texto.AppendLine("                    &nbsp;");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");
            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td style='line-height: 12px;'>");
            _texto.AppendLine("                    <table border='0' cellpadding='0' cellspacing='0' style='border-collapse: collapse;");
            _texto.AppendLine("                        width: 100%;'>");
            _texto.AppendLine("                        <tr style='border-top: 1px solid #000;'>");
            _texto.AppendLine("                            <td style='width: 100%; border-left: 1px solid #000; border-right: 1px solid #000;'>");
            _texto.AppendLine("                                OBSERVAÇÕES:");
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                        <tr style='border: 1px solid #000; border-top: 1px solid #FFF; line-height: 15px;'>");
            _texto.AppendLine("                            <td style='text-align: center;'>");
            _texto.AppendLine("                                " + obs.Replace("\r", "<br/>").Replace("\n", "<br/>"));
            _texto.AppendLine("                            </td>");
            _texto.AppendLine("                        </tr>");
            _texto.AppendLine("                    </table>");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");
            _texto.AppendLine("            <tr>");
            _texto.AppendLine("                <td>");
            _texto.AppendLine("                    &nbsp;");
            _texto.AppendLine("                </td>");
            _texto.AppendLine("            </tr>");
            _texto.AppendLine("        </table>");
            _texto.AppendLine("</div>");

            return _texto;
        }
        #endregion


    }
}

