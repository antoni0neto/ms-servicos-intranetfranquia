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
    public partial class desenv_produto_exc : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        int coluna = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregarColecoes();
                CarregarGrupo();
            }
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            labBuscar.Text = "";
            if (ddlColecoesBuscar.SelectedValue == "" || ddlColecoesBuscar.SelectedValue == "0")
            {
                labBuscar.Text = "Selecione a Coleção.";
                return;
            }

            if (ddlGrupo.SelectedValue.Trim() == "Selecione" || ddlGrupo.SelectedValue.Trim() == "")
            {
                labBuscar.Text = "Selecione o Grupo.";
                return;
            }

            Session["COLECAO"] = ddlColecoesBuscar.SelectedValue;

            try
            {
                RecarregarProduto();
            }
            catch (Exception ex)
            {
                labBuscar.Text = "ERRO: " + ex.Message;
            }
        }

        protected void gvProduto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    DESENV_PRODUTO _produto = e.Row.DataItem as DESENV_PRODUTO;

                    coluna += 1;
                    if (_produto != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = coluna.ToString();

                        Literal _colecao = e.Row.FindControl("litColecao") as Literal;
                        if (_colecao != null)
                            _colecao.Text = (new BaseController().BuscaColecaoAtual(_produto.COLECAO)).DESC_COLECAO;

                        Button _btExcluir = e.Row.FindControl("btProdutoExcluir") as Button;
                        if (_btExcluir != null)
                        {
                            _btExcluir.CommandArgument = _produto.CODIGO.ToString();
                        }
                    }
                }
            }
        }
        protected void btProdutoExcluir_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            labBuscar.Text = "";
            if (b != null)
            {
                try
                {
                    DESENV_PRODUTO _produto = new DESENV_PRODUTO();
                    _produto.CODIGO = Convert.ToInt32(b.CommandArgument);

                    if (desenvController.ObterProdutoPedidoProduto(_produto.CODIGO) != null)
                    {
                        labBuscar.Text = "Não será possível excluir este Produto. Produto possui Produto relacionado.";
                    }
                    else
                    {
                        _produto.STATUS = 'E'; //Excluido
                        desenvController.ExcluirProduto(_produto);
                        RecarregarProduto();
                    }
                }
                catch (Exception ex)
                {
                    labBuscar.Text = "ERRO (btProdutoExcluir_Click). \n\n" + ex.Message;
                }
            }
        }

        #region "DADOS INICIAIS"
        private void CarregarGrupo()
        {
            List<SP_OBTER_GRUPOResult> _grupo = (new ProducaoController().ObterGrupoProduto("01"));

            if (_grupo != null)
            {
                _grupo.Insert(0, new SP_OBTER_GRUPOResult { GRUPO_PRODUTO = "Selecione" });
                ddlGrupo.DataSource = _grupo;
                ddlGrupo.DataBind();
            }
        }
        private void CarregarColecoes()
        {
            BaseController _base = new BaseController();
            List<COLECOE> _colecoes = _base.BuscaColecoes();
            if (_colecoes != null)
            {
                _colecoes.Insert(0, new COLECOE { COLECAO = "0", DESC_COLECAO = "Selecione" });
                ddlColecoesBuscar.DataSource = _colecoes;
                ddlColecoesBuscar.DataBind();

                if (Session["COLECAO"] != null)
                    ddlColecoesBuscar.SelectedValue = Session["COLECAO"].ToString();
            }
        }
        private void RecarregarProduto()
        {
            labBuscar.Text = "";

            //Obter lista de produtos para exclusão
            List<DESENV_PRODUTO> _lstProduto = new List<DESENV_PRODUTO>();
            _lstProduto = desenvController.ObterProduto(ddlColecoesBuscar.SelectedValue).Where(p => p.STATUS == 'A').ToList();
            _lstProduto = _lstProduto.Where(i => i.GRUPO.Trim() == ddlGrupo.SelectedValue.Trim()).ToList();

            if (_lstProduto.Count <= 0)
                labBuscar.Text = "Nenhum produto encontrado. Refaça sua pesquisa.";

            gvProduto.DataSource = _lstProduto;
            gvProduto.DataBind();
        }
        #endregion
    }
}
