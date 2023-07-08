using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using System.Text;
using System.IO;
using System.Globalization;
using System.Drawing;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using System.Collections;
using Relatorios.mod_ecom.mag;


namespace Relatorios
{
    public partial class ecom_alt_produto_categoria : System.Web.UI.Page
    {
        ProducaoController prodController = new ProducaoController();
        EcomController eController = new EcomController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregarOutlet();
                CarregarColecoes();
                CarregarGrupo();
                CarregarGriffe();

                dialogPai.Visible = false;
                CarregarJQuery();
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
            btEnviarMagento.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btEnviarMagento, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarOutlet()
        {
            var outlet = eController.ObterMagentoGrupoProduto().Where(p => p.CODIGO_PAI == 3 || p.CODIGO_PAI == 4).ToList();

            outlet.Insert(0, new ECOM_GRUPO_PRODUTO { CODIGO = 0, GRUPO = "Selecione" });
            ddlOutlet.DataSource = outlet;
            ddlOutlet.DataBind();
        }
        private void CarregarColecoes()
        {
            List<COLECOE> _colecoes = baseController.BuscaColecoes();
            if (_colecoes != null)
            {
                _colecoes.Insert(0, new COLECOE { COLECAO = "", DESC_COLECAO = "" });
                ddlColecao.DataSource = _colecoes;
                ddlColecao.DataBind();
            }
        }
        private void CarregarGrupo()
        {
            List<SP_OBTER_GRUPOResult> _grupo = prodController.ObterGrupoProduto("");

            if (_grupo != null)
            {
                _grupo.Insert(0, new SP_OBTER_GRUPOResult { GRUPO_PRODUTO = "" });
                ddlGrupo.DataSource = _grupo;
                ddlGrupo.DataBind();
            }
        }
        private void CarregarGriffe()
        {
            List<PRODUTOS_GRIFFE> griffe = baseController.BuscaGriffes();

            if (griffe != null)
            {
                griffe.Insert(0, new PRODUTOS_GRIFFE { GRIFFE = "" });
                ddlGriffe.DataSource = griffe;
                ddlGriffe.DataBind();
            }
        }

        private void CarregarJQuery()
        {
            //GRID CLIENTE
            if (gvProduto.Rows.Count > 0)
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionP').accordion({ collapsible: true, heightStyle: 'content' });});", true);
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#accordionP').accordion({ collapsible: true, heightStyle: 'content', active: false });});", true);
        }
        #endregion

        private List<SP_OBTER_ECOM_PRODUTO_MAGENTOResult> ObterProduto()
        {
            var produtos = eController.ObterProdutoMagento(ddlColecao.SelectedValue, ddlGrupo.SelectedValue, Convert.ToInt32(ddlOutlet.SelectedValue), txtProduto.Text.Trim(), ddlGriffe.SelectedValue, null);

            return produtos;
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (ddlOutlet.SelectedValue == "0")
                {
                    labErro.Text = "Selecione o Grupo/Outlet do Produto...";
                    return;
                }

                gvProduto.DataSource = ObterProduto();
                gvProduto.DataBind();

                CarregarJQuery();
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        #region "GRID PRODUTO"
        protected void gvProduto_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_ECOM_PRODUTO_MAGENTOResult prod = e.Row.DataItem as SP_OBTER_ECOM_PRODUTO_MAGENTOResult;

                    System.Web.UI.WebControls.Image _imgProduto = e.Row.FindControl("imgProduto") as System.Web.UI.WebControls.Image;
                    _imgProduto.ImageUrl = (prod.FOTO_FRENTE_CAB == null) ? "" : prod.FOTO_FRENTE_CAB;

                    Literal litPrecoTL = e.Row.FindControl("litPrecoTL") as Literal;
                    litPrecoTL.Text = "R$ " + prod.PRECO_TL.ToString();

                }
            }
        }
        protected void gvProduto_DataBound(object sender, EventArgs e)
        {
        }

        #endregion

        private bool TemProdutoSelecionado()
        {
            bool ok = false;
            foreach (GridViewRow row in gvProduto.Rows)
            {
                CheckBox cb = row.FindControl("cbEnviarProduto") as CheckBox;
                if (cb.Checked)
                {
                    ok = true;
                    break;
                }
            }
            return ok;
        }
        protected void btEnviarMagento_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                labErroMagento.Text = "";

                if (!TemProdutoSelecionado())
                {
                    CarregarJQuery();

                    labErroMagento.Text = "Selecione pelo menos um produto.";
                    return;
                }

                USUARIO usuario = (USUARIO)Session["USUARIO"];
                string mapPath = Server.MapPath("..");

                Magento mag = new Magento(usuario.CODIGO_USUARIO, mapPath);

                List<string> produtosAsso = new List<string>();
                string produtoLinx = "";
                decimal precoTL = 0;
                //string msg = "";
                //string header = "Aviso";
                int codigoCategoriaDe = 0;
                int codigoCategariaPara = 0;

                //obter De
                codigoCategoriaDe = Convert.ToInt32(ddlOutlet.SelectedValue);

                var cCatPara = eController.ObterMagentoGrupoProdutoDePara(codigoCategoriaDe);
                if (cCatPara == null)
                {
                    labErroMagento.Text = "Categoria de Outlet não foi encontrada. Entre em contato com TI.";
                    return;
                }

                codigoCategariaPara = Convert.ToInt32(cCatPara.CODIGO_OUTLET);

                foreach (GridViewRow row in gvProduto.Rows)
                {
                    CheckBox cb = row.FindControl("cbEnviarProduto") as CheckBox;
                    if (cb.Checked)
                    {

                        TextBox _txtRelevancia = row.FindControl("txtRelevancia") as TextBox;
                        string posicao = _txtRelevancia.Text.Trim();

                        if (posicao == "")
                            posicao = "99";

                        produtoLinx = gvProduto.DataKeys[row.RowIndex].Values[0].ToString();
                        precoTL = Convert.ToDecimal(gvProduto.DataKeys[row.RowIndex].Values[1].ToString());

                        mag.AlterarGrupoOutlet(produtoLinx, codigoCategoriaDe, codigoCategariaPara, precoTL, DateTime.Today, null, posicao);
                    }
                }


                labErroMagento.Text = "OK";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#dialog').dialog({ autoOpen: true, position: { at: 'center top'}, height: 250, width: 395, modal: true, close: function (event, ui) { window.open('ecom_menu.aspx', '_self'); } }); });", true);
                dialogPai.Visible = true;

                CarregarJQuery();
            }
            catch (Exception ex)
            {
                labErroMagento.Text = ex.Message;
            }
        }



    }
}

