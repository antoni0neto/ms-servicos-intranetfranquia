using DAL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Relatorios
{
    public partial class ecom_rel_config_con : System.Web.UI.Page
    {
        EcomController ecomController = new EcomController();
        BaseController baseController = new BaseController();
        ProducaoController prodController = new ProducaoController();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                hrefVoltar.HRef = "ecom_menu.aspx";

                CarregarGrupo();
                CarregarGriffe();

            }

            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarGrupo()
        {
            var grupo = prodController.ObterGrupoProduto("");
            if (grupo != null)
            {
                grupo.Insert(0, new SP_OBTER_GRUPOResult { GRUPO_PRODUTO = "" });
                ddlGrupo.DataSource = grupo;
                ddlGrupo.DataBind();
            }
        }
        private void CarregarGriffe()
        {
            var griffe = baseController.BuscaGriffes();

            if (griffe != null)
            {
                griffe.Insert(0, new PRODUTOS_GRIFFE { GRIFFE = "" });
                ddlGriffe.DataSource = griffe;
                ddlGriffe.DataBind();
            }
        }

        #endregion


        private List<SP_OBTER_ECOM_REL_CONFIGResult> ObterEcomRelConfig()
        {
            return ecomController.ObterEcomRelConfig(ddlGriffe.SelectedValue.Trim(), ddlGrupo.SelectedValue.Trim(), Convert.ToInt32(ddlTipo.SelectedValue), txtNome.Text.Trim().ToUpper());
        }
        private void CarregarConfig()
        {
            var config = ObterEcomRelConfig();
            gvConfig.DataSource = config;
            gvConfig.DataBind();
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            CarregarConfig();
        }

        protected void gvConfig_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_ECOM_REL_CONFIGResult config = e.Row.DataItem as SP_OBTER_ECOM_REL_CONFIGResult;

                    Button btAlterar = e.Row.FindControl("btAlterar") as Button;
                    btAlterar.CommandArgument = config.CODIGO.ToString();

                    Button btExcluir = e.Row.FindControl("btExcluir") as Button;
                    btExcluir.CommandArgument = config.CODIGO.ToString();

                    GridView gvCampos = e.Row.FindControl("gvCampos") as GridView;
                    CarregarConfigCampos(config.CODIGO, gvCampos);
                }
            }
        }
        protected void gvConfig_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<SP_OBTER_ECOM_REL_CONFIGResult> produtos = ObterEcomRelConfig();

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            produtos = produtos.OrderBy(e.SortExpression + sortDirection);
            gvConfig.DataSource = produtos;
            gvConfig.DataBind();
        }
        protected void gvConfig_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = gvConfig.FooterRow;
            if (_footer != null)
            {
            }
        }

        private void CarregarConfigCampos(int codigoConfig, GridView gvCampos)
        {
            var configCampos = ecomController.ObterEcomRelConfigCampoPorRelConfig(codigoConfig);
            gvCampos.DataSource = configCampos;
            gvCampos.DataBind();
        }
        protected void gvCampos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    ECOM_REL_CONFIG_CAMPO configCampo = e.Row.DataItem as ECOM_REL_CONFIG_CAMPO;

                    Literal litCampo = e.Row.FindControl("litCampo") as Literal;
                    litCampo.Text = configCampo.ECOM_REL_CONFIG_FILTRO1.CAMPO_WHERE;

                    Literal litOperacao = e.Row.FindControl("litOperacao") as Literal;
                    litOperacao.Text = configCampo.ECOM_REL_CONFIG_OPERACOE.OPERACAO;

                    ListBox lboxValores = e.Row.FindControl("lboxValores") as ListBox;
                    CarregarListaValores(configCampo.CODIGO, lboxValores);

                }
            }
        }
        protected void gvCampos_DataBound(object sender, EventArgs e)
        {
        }
        private void CarregarListaValores(int codigoConfigCampo, ListBox lboxValores)
        {
            var valoresCampos = ecomController.ObterEcomRelConfigCampoValorPorConfigCampo(codigoConfigCampo);

            // limpar lista
            lboxValores.Items.Clear();

            ListItem item = null;
            foreach (var val in valoresCampos)
            {
                item = lboxValores.Items.FindByValue(val.CODIGO.ToString());
                if (item == null)
                {
                    item = new ListItem();
                    item.Value = val.CODIGO.ToString();
                    item.Text = val.VALOR_DESC;
                    lboxValores.Items.Add(item);
                }
            }
        }

        protected void btExcluir_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;

            try
            {
                labErro.Text = "";

                var codigoConfig = Convert.ToInt32(bt.CommandArgument);
                ecomController.ExcluirEcomRelConfig(codigoConfig);

                CarregarConfig();

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        protected void btAlterar_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;

            try
            {
                labErro.Text = "";
                var codigoConfig = Convert.ToInt32(bt.CommandArgument);
                Response.Redirect("ecom_rel_config_cad.aspx?gg=&gp=&coc=" + codigoConfig.ToString(), "_blank", "");

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }



    }
}
