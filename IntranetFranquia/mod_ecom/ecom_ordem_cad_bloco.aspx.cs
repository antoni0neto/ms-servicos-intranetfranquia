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
    public partial class ecom_ordem_cad_bloco : System.Web.UI.Page
    {
        EcomController ecomController = new EcomController();

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
                    hrefVoltar.HRef = "../mod_ecom/ecom_menu.aspx";

                CarregarCategoriaMag();
                CarregarFiltroCategoriaMag();

                //Controle de Controles
                btCancelar.Visible = false;
            }
        }


        private void CarregarFiltroCategoriaMag()
        {
            var y = ecomController.ObterMagentoGrupoProdutoBloco();

            y.Insert(0, new ECOM_GRUPO_PRODUTO { CODIGO = 0, GRUPO = "" });
            ddlCategoriaMagFiltro.DataSource = y.OrderBy(p => p.GRUPO);
            ddlCategoriaMagFiltro.DataBind();
        }

        public List<ECOM_BLOCO_PRODUTO> ObterBloco(int ecomGrupoProduto)
        {
            var blocos = ecomController.ObterBloco();

            if (ecomGrupoProduto > 0)
                blocos = blocos.Where(p => p.ECOM_GRUPO_PRODUTO == ecomGrupoProduto).ToList();

            return blocos;
        }

        #region "INICIALIZAR DADOS"
        private void CarregarCategoriaMag()
        {
            var catMag = ecomController.ObterMagentoGrupoProdutoBloco();
            catMag.Insert(0, new ECOM_GRUPO_PRODUTO { CODIGO = 0, GRUPO = "Selecione" });

            ddlCategoriaMag.DataSource = catMag;
            ddlCategoriaMag.DataBind();

        }
        private void CarregarBlocos(int ecomGrupoProduto)
        {
            try
            {
                labErro.Text = "";

                var b = ObterBloco(ecomGrupoProduto);

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
            labBloco.ForeColor = Color.Black;
            labOrdem.ForeColor = Color.Black;
            btCancelar.Visible = false;

            ddlCategoriaMag.SelectedValue = "0";
            txtBloco.Text = "";
            txtOrdem.Text = "";

            CarregarBlocos(Convert.ToInt32(ddlCategoriaMagFiltro.SelectedValue));
        }
        #endregion

        #region "CRUD"
        private void Incluir(ECOM_BLOCO_PRODUTO bloco)
        {
            ecomController.InserirBloco(bloco);
        }
        private void Editar(ECOM_BLOCO_PRODUTO bloco)
        {
            ecomController.AtualizarBloco(bloco);
        }
        private void Excluir(int _codigo)
        {
            ecomController.ExcluirBloco(_codigo);
        }
        #endregion

        #region "AÇÕES"
        protected void btIncluir_Click(object sender, EventArgs e)
        {
            bool _Inclusao = true;

            labErro.Text = "";
            if (ddlCategoriaMag.SelectedValue.Trim() == "Selecione" || ddlCategoriaMag.SelectedValue.Trim() == "0")
            {
                labErro.Text = "Selecione a Categoria Magento.";
                return;
            }

            if (txtBloco.Text.Trim() == "")
            {
                labErro.Text = "Informe o nome do Bloco.";
                return;
            }

            if (txtOrdem.Text.Trim() == "")
            {
                labErro.Text = "Informe a ordem do Bloco.";
                return;
            }

            try
            {
                if (hidCodigo.Value != "0" && hidCodigo.Value != "")
                    _Inclusao = false;

                ECOM_BLOCO_PRODUTO _novo = new ECOM_BLOCO_PRODUTO();
                _novo.ECOM_ORDEM_DATA = ecomController.ObterUltimaDataOrdem().CODIGO;
                _novo.ECOM_GRUPO_PRODUTO = Convert.ToInt32(ddlCategoriaMag.SelectedValue);
                _novo.BLOCO = txtBloco.Text.Trim().ToUpper();
                _novo.ORDEM = Convert.ToInt32(txtOrdem.Text.Trim());
                _novo.DATA_INCLUSAO = DateTime.Now;

                if (_Inclusao)
                {
                    Incluir(_novo);
                }
                else
                {
                    _novo.CODIGO = Convert.ToInt32(hidCodigo.Value);
                    Editar(_novo);
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
                    ECOM_BLOCO_PRODUTO bloco = ecomController.ObterBloco(Convert.ToInt32(b.CommandArgument));
                    if (bloco != null)
                    {

                        hidCodigo.Value = bloco.CODIGO.ToString();

                        ddlCategoriaMag.SelectedValue = bloco.ECOM_GRUPO_PRODUTO.ToString();
                        txtBloco.Text = bloco.BLOCO;
                        txtOrdem.Text = bloco.ORDEM.ToString();

                        labCategoria.ForeColor = Color.Red;
                        labBloco.ForeColor = Color.Red;
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
                        Excluir(Convert.ToInt32(b.CommandArgument));
                        RecarregarTela();
                    }
                    catch (Exception)
                    {
                        labErro.Text = "O Bloco não pode ser excluído. Este bloco possui produto cadastrado.";
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
                    ECOM_BLOCO_PRODUTO bloco = e.Row.DataItem as ECOM_BLOCO_PRODUTO;

                    if (bloco != null)
                    {

                        Literal litCatMagento = e.Row.FindControl("litCatMagento") as Literal;
                        litCatMagento.Text = bloco.ECOM_GRUPO_PRODUTO1.GRUPO;

                        Literal litOrdem = e.Row.FindControl("litOrdem") as Literal;
                        litOrdem.Text = bloco.ORDEM.ToString();

                        Literal litTot4 = e.Row.FindControl("litTot4") as Literal;
                        if (litTot4 != null)
                            litTot4.Text = ecomController.ObterEcomProdutoBlocoTOT4(bloco.CODIGO).TOT_PRODUTO4.ToString();

                        Button _btAlerar = e.Row.FindControl("btAlterar") as Button;
                        if (_btAlerar != null)
                            _btAlerar.CommandArgument = bloco.CODIGO.ToString();

                        Button _btExcluir = e.Row.FindControl("btExcluir") as Button;
                        if (_btExcluir != null)
                            _btExcluir.CommandArgument = bloco.CODIGO.ToString();
                    }
                }
            }
        }

        [WebMethod]
        public static string SalvarOrdem(string[] blocos)
        {
            EcomController ecomController = new EcomController();

            int ordem = 1;
            foreach (var b in blocos)
            {
                var bloco = ecomController.ObterBloco(Convert.ToInt32(b));
                if (bloco != null)
                {
                    bloco.ORDEM = ordem;
                    ecomController.AtualizarBloco(bloco);

                    ordem += 1;
                }
            }

            return "";
        }

        protected void btFiltrar_Click(object sender, EventArgs e)
        {
            CarregarBlocos(Convert.ToInt32(ddlCategoriaMagFiltro.SelectedValue));
        }
    }

}
