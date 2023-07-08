using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using System.Linq.Dynamic;
using Relatorios.mod_ecomv2.mag2;

namespace Relatorios
{
    public partial class ecomv2_cad_produto_magento : System.Web.UI.Page
    {
        ProducaoController prodController = new ProducaoController();
        EcomController eController = new EcomController();
        BaseController baseController = new BaseController();

        string headerTam1 = "";
        string headerTam2 = "";
        string headerTam3 = "";
        string headerTam4 = "";
        string headerTam5 = "";
        string headerTam6 = "";
        string headerTam7 = "";
        string headerTam8 = "";
        string headerTam9 = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
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

        private List<SP_OBTER_ECOM_PRODUTO_LIBERACAOResult> ObterProduto()
        {
            var produtos = eController.ObterProdutoLinxLiberacao(ddlColecao.SelectedValue, ddlGrupo.SelectedValue, txtProduto.Text.Trim(), ddlGriffe.SelectedValue, null);

            return produtos;
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";
                labErroMagento.Text = "";

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
                    SP_OBTER_ECOM_PRODUTO_LIBERACAOResult prod = e.Row.DataItem as SP_OBTER_ECOM_PRODUTO_LIBERACAOResult;

                    System.Web.UI.WebControls.Image _imgProduto = e.Row.FindControl("imgProduto") as System.Web.UI.WebControls.Image;
                    _imgProduto.ImageUrl = (prod.FOTO_FRENTE == null) ? "" : prod.FOTO_FRENTE;

                    if (!headerTam1.Contains(prod.TAMANHO_1))
                        headerTam1 = headerTam1 + prod.TAMANHO_1 + "-";
                    if (!headerTam2.Contains(prod.TAMANHO_2))
                        headerTam2 = headerTam2 + prod.TAMANHO_2 + "-";
                    if (!headerTam3.Contains(prod.TAMANHO_3))
                        headerTam3 = headerTam3 + prod.TAMANHO_3 + "-";
                    if (!headerTam4.Contains(prod.TAMANHO_4))
                        headerTam4 = headerTam4 + prod.TAMANHO_4 + "-";
                    if (!headerTam5.Contains(prod.TAMANHO_5))
                        headerTam5 = headerTam5 + prod.TAMANHO_5 + "-";
                    if (!headerTam6.Contains(prod.TAMANHO_6))
                        headerTam6 = headerTam6 + prod.TAMANHO_6 + "-";
                    if (!headerTam7.Contains(prod.TAMANHO_7))
                        headerTam7 = headerTam7 + prod.TAMANHO_7 + "-";
                    if (!headerTam8.Contains(prod.TAMANHO_8))
                        headerTam8 = headerTam8 + prod.TAMANHO_8 + "-";
                    if (!headerTam9.Contains(prod.TAMANHO_9))
                        headerTam9 = headerTam9 + prod.TAMANHO_9 + "-";
                }
            }
        }
        protected void gvProduto_DataBound(object sender, EventArgs e)
        {
            GerarHeaderTamanho();
        }
        protected void gvProduto_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<SP_OBTER_ECOM_PRODUTO_LIBERACAOResult> produtos = ObterProduto();

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            produtos = produtos.OrderBy(e.SortExpression + sortDirection);
            gvProduto.DataSource = produtos;
            gvProduto.DataBind();

            CarregarJQuery();
            GerarHeaderTamanho();
        }

        private void GerarHeaderTamanho()
        {
            GridViewRow _header = gvProduto.HeaderRow;
            if (_header != null)
            {
                headerTam1 = headerTam1 + "-";
                headerTam1 = headerTam1.Replace("--", "");
                headerTam2 = headerTam2 + "-";
                headerTam2 = headerTam2.Replace("--", "");
                headerTam3 = headerTam3 + "-";
                headerTam3 = headerTam3.Replace("--", "");
                headerTam4 = headerTam4 + "-";
                headerTam4 = headerTam4.Replace("--", "");
                headerTam5 = headerTam5 + "-";
                headerTam5 = headerTam5.Replace("--", "");
                headerTam6 = headerTam6 + "-";
                headerTam6 = headerTam6.Replace("--", "");
                headerTam7 = headerTam7 + "-";
                headerTam7 = headerTam7.Replace("--", "");
                headerTam8 = headerTam8 + "-";
                headerTam8 = headerTam8.Replace("--", "");
                headerTam9 = headerTam9 + "-";
                headerTam9 = headerTam9.Replace("--", "");

                _header.Cells[7].Text = headerTam1 ?? "-";
                _header.Cells[8].Text = headerTam2 ?? "-";
                _header.Cells[9].Text = headerTam3 ?? "-";
                _header.Cells[10].Text = headerTam4 ?? "-";
                _header.Cells[11].Text = headerTam5 ?? "-";
                _header.Cells[12].Text = headerTam6 ?? "-";
                _header.Cells[13].Text = headerTam7 ?? "-";
                _header.Cells[14].Text = headerTam8 ?? "-";
                _header.Cells[15].Text = headerTam9 ?? "-";
            }
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

                MagentoV2 mag = new MagentoV2(usuario.CODIGO_USUARIO, mapPath);

                int codigo = 0;
                string msg = "";
                string header = "Aviso";

                foreach (GridViewRow row in gvProduto.Rows)
                {
                    CheckBox cb = row.FindControl("cbEnviarProduto") as CheckBox;
                    if (cb.Checked)
                    {
                        codigo = Convert.ToInt32(gvProduto.DataKeys[row.RowIndex].Values[0].ToString());
                        mag.SalvarProdutoEstoque(codigo);
                    }
                }

                msg = "Processo finalizado com Sucesso.";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "(function ($) { $(function () { $.jGrowl.defaults.pool = 10; $.jGrowl('" + msg + "', { header: '" + header + "', life: 10000, theme: 'manilla', speed: 'slow', position: 'top-right', animateOpen: { height: 'show', width: 'hide' }, animateClose: { height: 'show', width: 'show' } }); }); })(jQuery);", true);

                labErroMagento.Text = "OK";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "$(function () { $('#dialog').dialog({ autoOpen: true, position: { at: 'center top'}, height: 250, width: 395, modal: true, close: function (event, ui) { window.open('../mod_ecom/ecom_menu.aspx', '_self'); } }); });", true);
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

