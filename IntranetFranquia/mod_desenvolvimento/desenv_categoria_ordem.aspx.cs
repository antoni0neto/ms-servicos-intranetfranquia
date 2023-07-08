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
using System.Data.OleDb;
using System.Web.Services;

namespace Relatorios
{
    public partial class desenv_categoria_ordem : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                //Valida queryString
                if (Request.QueryString["t"] == null || Request.QueryString["t"] == "")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                string tela = Request.QueryString["t"].ToString();
                if (tela != "1" && tela != "2" && tela != "3" && tela != "4")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                if (tela == "1")
                    hrefVoltar.HRef = "../mod_desenvolvimento/desenv_menu.aspx";

                CarregarColecoes();
                CarregarGriffe();


                //Controle de Controles
                btCancelar.Visible = false;
            }
        }

        private void CarregarColecoes()
        {
            var colecoes = baseController.BuscaColecoes();

            colecoes.Insert(0, new COLECOE { COLECAO = "", DESC_COLECAO = "Selecione" });

            ddlColecao.DataSource = colecoes;
            ddlColecao.DataBind();

            ddlColecaoFiltro.DataSource = colecoes;
            ddlColecaoFiltro.DataBind();
        }
        private void CarregarGriffe()
        {
            var griffes = baseController.BuscaGriffes();
            griffes.Insert(0, new PRODUTOS_GRIFFE { GRIFFE = "Selecione" });

            ddlGriffe.DataSource = griffes;
            ddlGriffe.DataBind();

            ddlGriffeFiltro.DataSource = griffes;
            ddlGriffeFiltro.DataBind();
        }

        public List<DESENV_CATEGORIA> ObterCategoria(string colecao, string griffe)
        {
            var categorias = desenvController.ObterCategoria(colecao, griffe);
            return categorias;
        }

        #region "INICIALIZAR DADOS"
        private void CarregarCategorias(string colecao, string griffe)
        {
            try
            {
                labErro.Text = "";

                var b = ObterCategoria(colecao, griffe);

                gvBloco.DataSource = b;
                gvBloco.DataBind();

                if (b == null || b.Count() <= 0)
                    labErro.Text = "Nenhum registro encontrado.";

                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { sortTableBloco(); });", true);

            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO (gvBloco): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
            }

        }
        private void RecarregarTela()
        {
            //Recarregar valores e fontes
            hidCodigo.Value = "";
            labCategoria.ForeColor = Color.Black;
            labColecao.ForeColor = Color.Black;
            labGriffe.ForeColor = Color.Black;
            labOrdem.ForeColor = Color.Black;
            btCancelar.Visible = false;

            ddlColecao.SelectedValue = "";
            ddlGriffe.SelectedValue = "Selecione";
            txtCategoria.Text = "";
            txtOrdem.Text = "";

            CarregarCategorias(ddlColecaoFiltro.SelectedValue, ddlGriffeFiltro.SelectedValue);
        }
        #endregion

        #region "AÇÕES"
        protected void btIncluir_Click(object sender, EventArgs e)
        {
            var inc = true;

            labErro.Text = "";
            if (ddlColecao.SelectedValue.Trim() == "")
            {
                labErro.Text = "Selecione a Coleção.";
                return;
            }
            if (ddlGriffe.SelectedValue.Trim() == "Selecione")
            {
                labErro.Text = "Selecione a Griffe.";
                return;
            }

            if (txtCategoria.Text.Trim() == "")
            {
                labErro.Text = "Informe o nome da Categoria.";
                return;
            }

            if (txtOrdem.Text.Trim() == "")
            {
                labErro.Text = "Informe a ordem da Categoria.";
                return;
            }

            try
            {
                if (hidCodigo.Value != "0" && hidCodigo.Value != "")
                    inc = false;

                var novo = new DESENV_CATEGORIA();
                novo.COLECAO = ddlColecao.SelectedValue.Trim();
                novo.GRIFFE = ddlGriffe.SelectedValue.Trim();
                novo.NOME = txtCategoria.Text.Trim().ToUpper();
                novo.ORDEM = Convert.ToInt32(txtOrdem.Text.Trim());


                if (inc)
                {
                    desenvController.InserirCategoria(novo);
                }
                else
                {
                    novo.CODIGO = Convert.ToInt32(hidCodigo.Value);
                    desenvController.AtualizarCategoria(novo);
                }

                RecarregarTela();
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO (btIncluir_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
            }
        }
        protected void btAlterar_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            if (b != null)
            {
                try
                {
                    DESENV_CATEGORIA categoria = desenvController.ObterCategoria(Convert.ToInt32(b.CommandArgument));
                    if (categoria != null)
                    {

                        hidCodigo.Value = categoria.CODIGO.ToString();

                        ddlColecao.SelectedValue = baseController.BuscaColecaoAtual(categoria.COLECAO).COLECAO;
                        ddlGriffe.SelectedValue = baseController.BuscaGriffes().Where(p => p.GRIFFE.Trim() == categoria.GRIFFE.Trim()).SingleOrDefault().GRIFFE;
                        txtCategoria.Text = categoria.NOME;
                        txtOrdem.Text = categoria.ORDEM.ToString();

                        labCategoria.ForeColor = Color.Red;
                        labColecao.ForeColor = Color.Red;
                        labGriffe.ForeColor = Color.Red;
                        labOrdem.ForeColor = Color.Red;
                        btCancelar.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    labErro.Text = "ERRO (btAlterar_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
                }
            }
        }
        protected void btExcluir_Click(object sender, EventArgs e)
        {

            Button b = (Button)sender;
            if (b != null)
            {
                labErro.Text = "";
                try
                {
                    try
                    {
                        desenvController.ExcluirCategoria(Convert.ToInt32(b.CommandArgument));
                        RecarregarTela();
                    }
                    catch (Exception)
                    {
                        labErro.Text = "A categoria não pode ser excluída. Esta categoria possui produto cadastrado.";
                    }

                }
                catch (Exception ex)
                {
                    labErro.Text = "ERRO (btExcluir_Click): ENVIE ESTA TELA PARA O TI.\n\n" + ex.Message;
                }
            }
        }
        protected void btCancelar_Click(object sender, EventArgs e)
        {
            RecarregarTela();
        }
        #endregion

        protected void gvBloco_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    DESENV_CATEGORIA cat = e.Row.DataItem as DESENV_CATEGORIA;

                    if (cat != null)
                    {

                        Literal litColecao = e.Row.FindControl("litColecao") as Literal;
                        litColecao.Text = baseController.BuscaColecaoAtual(cat.COLECAO).DESC_COLECAO.Trim();

                        Literal litGriffe = e.Row.FindControl("litGriffe") as Literal;
                        litGriffe.Text = cat.GRIFFE;

                        Literal litOrdem = e.Row.FindControl("litOrdem") as Literal;
                        litOrdem.Text = cat.ORDEM.ToString();

                        Button _btAlerar = e.Row.FindControl("btAlterar") as Button;
                        if (_btAlerar != null)
                            _btAlerar.CommandArgument = cat.CODIGO.ToString();

                        Button _btExcluir = e.Row.FindControl("btExcluir") as Button;
                        if (_btExcluir != null)
                            _btExcluir.CommandArgument = cat.CODIGO.ToString();
                    }
                }
            }
        }

        [WebMethod]
        public static string SalvarOrdem(string[] categorias)
        {
            DesenvolvimentoController desenvController = new DesenvolvimentoController();

            int ordem = 1;
            foreach (var b in categorias)
            {
                var cat = desenvController.ObterCategoria(Convert.ToInt32(b));
                if (cat != null)
                {
                    cat.ORDEM = ordem;
                    desenvController.AtualizarCategoria(cat);

                    ordem += 1;
                }
            }

            return "";
        }

        protected void btFiltrar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                if (ddlColecaoFiltro.SelectedValue != "" && ddlGriffeFiltro.SelectedValue != "Selecione")
                {
                    CarregarCategorias(ddlColecaoFiltro.SelectedValue.Trim(), ddlGriffeFiltro.SelectedValue.Trim());
                }
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
    }

}
