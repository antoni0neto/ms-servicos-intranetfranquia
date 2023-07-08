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
    public partial class desenv_rel_produto_liberacao : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        int coluna = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregarColecoes();
                CarregarOrigem("");
                CarregarGrupo();
            }
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {

            labErro.Text = "";
            try
            {
                if (ddlColecoes.SelectedValue == "")
                {
                    labErro.Text = "Selecione uma Coleção.";
                    return;
                }

                if (ddlOrigem.SelectedValue == "0" && ddlGrupo.SelectedValue == "" && txtModelo.Text.Trim() == "" && txtNome.Text.Trim() == "")
                {
                    labErro.Text = "Informe pelo menos um filtro.";
                    return;
                }

                RecarregarProduto();

                Session["COLECAO"] = ddlColecoes.SelectedValue;
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
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

                        Button _btOK = e.Row.FindControl("btOK") as Button;
                        if (_btOK != null)
                            _btOK.CommandArgument = _produto.CODIGO.ToString();

                        GridView gvFoto = e.Row.FindControl("gvFoto") as GridView;
                        if (gvFoto != null)
                        {
                            List<DESENV_PRODUTO> _fotoProduto = new List<DESENV_PRODUTO>();
                            _fotoProduto.Add(new DESENV_PRODUTO { FOTO = _produto.FOTO, FOTO2 = _produto.FOTO2 });
                            gvFoto.DataSource = _fotoProduto;
                            gvFoto.DataBind();
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
                    DESENV_PRODUTO _foto = e.Row.DataItem as DESENV_PRODUTO;
                    if (_foto != null)
                    {
                        System.Web.UI.WebControls.Image _imgFotoProduto = e.Row.FindControl("imgFotoProduto") as System.Web.UI.WebControls.Image;
                        if (_imgFotoProduto != null)
                            _imgFotoProduto.ImageUrl = _foto.FOTO;
                    }
                }
            }
        }

        protected void btOK_Click(object sender, EventArgs e)
        {
            int codigo = 0;

            try
            {
                Button b = (Button)sender;

                if (b != null)
                {
                    //Obter Codigo
                    codigo = Convert.ToInt32(b.CommandArgument);

                }
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
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
        protected void ddlColecoes_SelectedIndexChanged(object sender, EventArgs e)
        {
            string colecao = ddlColecoes.SelectedValue.Trim();
            if (colecao != "" && colecao != "0")
            {
                CarregarOrigem(colecao);
            }
        }
        private void CarregarGrupo()
        {
            List<SP_OBTER_GRUPOResult> _grupo = new ProducaoController().ObterGrupoProduto("01");

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

        private void RecarregarProduto()
        {
            //Obter lista de produtos
            List<SP_OBTER_HB_PRODUTO_RELACIONADOResult> _produto = null;
            int? hb = null;

            _produto = desenvController.ObterProdutoHBRelacionado(ddlColecoes.SelectedValue.Trim(), hb, txtModelo.Text.Trim().ToUpper(), txtNome.Text.Trim().ToUpper());

            gvProduto.DataSource = _produto;
            gvProduto.DataBind();
        }
        #endregion
    }
}
