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
    public partial class desenv_produto_hb_excluir : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        int coluna = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregarColecoes();
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

                if (txtHB.Text.Trim() == "" && txtModelo.Text.Trim() == "" && txtNome.Text.Trim() == "")
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
                    SP_OBTER_HB_PRODUTO_RELACIONADOResult _produto = e.Row.DataItem as SP_OBTER_HB_PRODUTO_RELACIONADOResult;

                    coluna += 1;
                    if (_produto != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = coluna.ToString();

                        Button _btExcluir = e.Row.FindControl("btExcluir") as Button;
                        if (_btExcluir != null)
                            _btExcluir.CommandArgument = _produto.CODIGO.ToString();

                        GridView gvFoto = e.Row.FindControl("gvFoto") as GridView;
                        if (gvFoto != null)
                        {
                            List<SP_OBTER_HB_PRODUTO_RELACIONADOResult> _fotoProduto = new List<SP_OBTER_HB_PRODUTO_RELACIONADOResult>();
                            _fotoProduto.Add(new SP_OBTER_HB_PRODUTO_RELACIONADOResult { P_FOTO = _produto.P_FOTO, HB_FOTO = _produto.HB_FOTO });
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
                    SP_OBTER_HB_PRODUTO_RELACIONADOResult _foto = e.Row.DataItem as SP_OBTER_HB_PRODUTO_RELACIONADOResult;
                    if (_foto != null)
                    {
                        System.Web.UI.WebControls.Image _imgFotoProduto = e.Row.FindControl("imgFotoProduto") as System.Web.UI.WebControls.Image;
                        if (_imgFotoProduto != null)
                            _imgFotoProduto.ImageUrl = _foto.P_FOTO;

                        System.Web.UI.WebControls.Image _imgFotoPeca = e.Row.FindControl("imgFotoPeca") as System.Web.UI.WebControls.Image;
                        if (_imgFotoPeca != null)
                            _imgFotoPeca.ImageUrl = _foto.HB_FOTO;
                    }
                }
            }
        }

        protected void btExcluir_Click(object sender, EventArgs e)
        {
            int codigo = 0;

            try
            {
                Button b = (Button)sender;

                if (b != null)
                {
                    //Obter Codigo
                    codigo = Convert.ToInt32(b.CommandArgument);

                    //Excluir relação registro na tabela
                    if (codigo > 0)
                    {
                        //EXCLUIR
                        desenvController.ExcluirProdutoHB(codigo);
                        RecarregarProduto();
                    }
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
        private void RecarregarProduto()
        {
            //Obter lista de produtos
            List<SP_OBTER_HB_PRODUTO_RELACIONADOResult> _produto = null;
            int? hb = null;

            if (txtHB.Text.Trim() != "")
                hb = Convert.ToInt32(txtHB.Text.Trim());

            _produto = desenvController.ObterProdutoHBRelacionado(ddlColecoes.SelectedValue.Trim(), hb, txtModelo.Text.Trim().ToUpper(), txtNome.Text.Trim().ToUpper());

            gvProduto.DataSource = _produto;
            gvProduto.DataBind();
        }
        #endregion
    }
}
