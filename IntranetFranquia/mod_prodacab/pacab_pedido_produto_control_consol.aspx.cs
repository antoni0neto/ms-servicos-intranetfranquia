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
    public partial class pacab_pedido_produto_control_consol : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();

        const string PROD_ACAB_PEDCOMPRA_PROD = "PROD_ACAB_PEDCOMPRA_CONTROL_CONSOL";

        int qtdeIntra = 0;
        int qtdeLinx = 0;


        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                CarregarColecoes();
                CarregarGrupo();

                CarregarFabricante();
                CarregarFilial();
                CarregarGriffe();

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

            // VAMOS AGRUPAR!!!!!!

            var produtoAcabadoConsol = produtoAcabado.GroupBy(p => new
            {
                PRODUTO = p.PRODUTO,
                DESC_PRODUTO = p.DESC_PRODUTO,
                COR = p.COR,
                DESC_COR_PRODUTO = p.DESC_COR_PRODUTO,
                GRIFFE = p.GRIFFE,
                FORNECEDOR = p.FORNECEDOR,
                TIPO = p.TIPO
            }).Select(x => new SP_OBTER_PRODUTO_ACABADO_PRODUTO_V10Result
            {
                PRODUTO = x.Key.PRODUTO,
                DESC_PRODUTO = x.Key.DESC_PRODUTO,
                COR = x.Key.COR,
                DESC_COR_PRODUTO = x.Key.DESC_COR_PRODUTO,
                GRIFFE = x.Key.GRIFFE,
                FORNECEDOR = x.Key.FORNECEDOR,
                TIPO = x.Key.TIPO,
                QTDE_INTRA = x.Sum(p => p.QTDE_INTRA),
                QTDE_LINX = x.Sum(p => p.QTDE_LINX),
                CODIGO_PRODUTO = x.Max(p => p.CODIGO_PRODUTO),
                FOTO1 = x.Max(p => p.FOTO1),
                CUSTO = x.Max(p => p.CUSTO),
                PRECO = x.Max(p => p.PRECO)
            }).ToList();


            return produtoAcabadoConsol;
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

                        ImageButton imgProduto = e.Row.FindControl("imgProduto") as ImageButton;
                        imgProduto.ImageUrl = produtoAcabado.FOTO1;
                        imgProduto.CommandArgument = produtoAcabado.CODIGO_PRODUTO.ToString() + "|" + produtoAcabado.TIPO.Trim();

                        qtdeIntra += produtoAcabado.QTDE_INTRA;
                        qtdeLinx += produtoAcabado.QTDE_LINX;

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
                footer.Cells[1].Text = "Total";

                footer.Cells[9].Text = qtdeIntra.ToString();
                footer.Cells[10].Text = qtdeLinx.ToString();


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


        #endregion


    }
}

