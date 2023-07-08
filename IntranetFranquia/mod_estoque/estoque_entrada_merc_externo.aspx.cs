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
    public partial class estoque_entrada_merc_externo : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();

        const string PROD_ACAB_PEDCOMPRA_EXTERNO = "PROD_ACAB_PEDCOMPRA_EXTERNO";

        int qtdeIntra = 0;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                CarregarColecoes();

                CarregarFabricante();
                CarregarFilial();

                Session[PROD_ACAB_PEDCOMPRA_EXTERNO] = null;
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
            btBaixar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBaixar, null) + ";");

        }

        #region "DADOS INICIAIS"
        private void CarregarColecoes()
        {
            var colecoes = baseController.BuscaColecoes();

            colecoes.Insert(0, new COLECOE { COLECAO = "", DESC_COLECAO = "" });

            ddlColecoes.DataSource = colecoes;
            ddlColecoes.DataBind();
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
            if (Session["USUARIO"] == null)
                Response.Redirect("~/Login.aspx");
            else
            {
                USUARIO usuario = (USUARIO)Session["USUARIO"];

                var filiais = baseController.BuscaFiliais(usuario);

                filiais.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "Selecione" });

                ddlFilial.DataSource = filiais;
                ddlFilial.DataBind();

                if (filiais.Count() == 2)
                    ddlFilial.SelectedIndex = 1;
            }
        }

        #endregion

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labMsg.Text = "";
                labErroBaixa.Text = "";
                btBaixar.Enabled = true;

                if (ddlFilial.SelectedValue == "")
                {
                    labMsg.Text = "Selecione a Filial...";
                    return;
                }

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

            var produtoAcabado = desenvController.ObterProdutoAcabadoProdutoPrePedidoV10(ddlColecoes.SelectedValue.Trim(), 0, "", txtProduto.Text.Trim(), ddlFilial.SelectedItem.Text.Trim(), ddlFabricante.SelectedValue.Trim())
                .Where(p => p.DATA_EXCLUSAO == null).ToList();

            if (ddlProdutoRecebido.SelectedValue != "")
            {
                if (ddlProdutoRecebido.SelectedValue == "S")
                    produtoAcabado = produtoAcabado.Where(p => p.RECEBIMENTO != null).ToList();
                else
                    produtoAcabado = produtoAcabado.Where(p => p.RECEBIMENTO == null).ToList();
            }

            return produtoAcabado;
        }

        private void CarregarProdutoAcabado()
        {
            var produtoAcabado = ObterProdutoAcabadoProdutoPrePedido();

            gvProdutoAcabado.DataSource = produtoAcabado;
            gvProdutoAcabado.DataBind();

            Session[PROD_ACAB_PEDCOMPRA_EXTERNO] = produtoAcabado;
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

                        Literal litDataPrevEntrega = e.Row.FindControl("litDataPrevEntrega") as Literal;
                        litDataPrevEntrega.Text = Convert.ToDateTime(produtoAcabado.LIMITE_ENTREGA).ToString("dd/MM/yyyy");

                        Literal litRecebimento = e.Row.FindControl("litRecebimento") as Literal;
                        litRecebimento.Text = (produtoAcabado.RECEBIMENTO == null) ? "-" : Convert.ToDateTime(produtoAcabado.RECEBIMENTO).ToString("dd/MM/yyyy");

                        TextBox txtNFEntrada = e.Row.FindControl("txtNFEntrada") as TextBox;
                        txtNFEntrada.Text = produtoAcabado.NF_ENTRADA;
                        if (produtoAcabado.NF_ENTRADA != "")
                            txtNFEntrada.Enabled = false;

                        qtdeIntra += produtoAcabado.QTDE_INTRA;

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

            }
        }
        protected void gvProdutoAcabado_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (Session[PROD_ACAB_PEDCOMPRA_EXTERNO] != null)
            {
                IEnumerable<SP_OBTER_PRODUTO_ACABADO_PRODUTO_V10Result> produtoAcabado = (IEnumerable<SP_OBTER_PRODUTO_ACABADO_PRODUTO_V10Result>)Session[PROD_ACAB_PEDCOMPRA_EXTERNO];

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

        #endregion

        protected void btBaixar_Click(object sender, EventArgs e)
        {
            try
            {
                labMsg.Text = "";
                labErroBaixa.Text = "";

                if (txtRecebidoPor.Text.Trim() == "")
                {
                    labErroBaixa.Text = "Informe quem recebeu os produtos...";
                    return;
                }

                foreach (GridViewRow row in gvProdutoAcabado.Rows)
                {
                    var pedido = gvProdutoAcabado.DataKeys[row.RowIndex][0].ToString();
                    var produto = gvProdutoAcabado.DataKeys[row.RowIndex][1].ToString();
                    var cor = gvProdutoAcabado.DataKeys[row.RowIndex][2].ToString();
                    var entrega = Convert.ToDateTime(gvProdutoAcabado.DataKeys[row.RowIndex][3].ToString());

                    var prePedido = desenvController.ObterComprasProdutoPrePedido(pedido, produto, cor, entrega);
                    if (prePedido != null)
                    {
                        if (prePedido.RECEBIMENTO == null)
                        {
                            var txtNFEntrada = ((TextBox)row.FindControl("txtNFEntrada"));
                            var nfEntrada = txtNFEntrada.Text.Trim().ToUpper();

                            if (nfEntrada != "")
                            {
                                prePedido.NF_ENTRADA = nfEntrada;
                                prePedido.RECEBIMENTO = DateTime.Now;
                                prePedido.RECEBIDO_POR = txtRecebidoPor.Text.Trim().ToUpper();
                                desenvController.AtualizarComprasProdutoPrePedido(prePedido);

                                txtNFEntrada.Enabled = false;
                            }
                        }
                    }
                }


                btBaixar.Enabled = false;
                labErroBaixa.Text = "Produtos recebidos com sucesso.";

            }
            catch (Exception ex)
            {
                labErroBaixa.Text = ex.Message;
            }
        }
    }
}

