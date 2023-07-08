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
    public partial class ecom_rel_config_entrada_saida : System.Web.UI.Page
    {
        EcomController ecomController = new EcomController();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                hrefVoltar.HRef = "ecom_menu.aspx";

                CarregarRegras();
                CarregarRegrasRel();

            }

            btSalvar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btSalvar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarRegras()
        {
            var configEntrada = ecomController.ObterEcomRelConfigPorTipo(1);
            var configSaida = ecomController.ObterEcomRelConfigPorTipo(2);

            configEntrada.Insert(0, new ECOM_REL_CONFIG { CODIGO = 0, NOME = "Selecione" });
            ddlRegraEntrada.DataSource = configEntrada;
            ddlRegraEntrada.DataBind();

            configSaida.Insert(0, new ECOM_REL_CONFIG { CODIGO = 0, NOME = "Selecione" });
            ddlRegraSaida.DataSource = configSaida;
            ddlRegraSaida.DataBind();
        }

        #endregion

        private List<ECOM_REL_CONFIG_DEPARA> ObterEcomRelConfigDEPARA()
        {
            return ecomController.ObterEcomRelConfigDEPARA();
        }
        private void CarregarRegrasRel()
        {
            var regras = ObterEcomRelConfigDEPARA();

            IEnumerable<ECOM_REL_CONFIG_DEPARA> regrasAux = (IEnumerable<ECOM_REL_CONFIG_DEPARA>)regras;
            regrasAux = regras.OrderBy(hidCampo.Value + "" + hidOrdem.Value);

            gvConfig.DataSource = regrasAux;
            gvConfig.DataBind();
        }

        protected void gvConfig_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    ECOM_REL_CONFIG_DEPARA config = e.Row.DataItem as ECOM_REL_CONFIG_DEPARA;

                    Literal litEntrada = e.Row.FindControl("litEntrada") as Literal;
                    litEntrada.Text = config.ECOM_REL_CONFIG.NOME;

                    Literal litSaida = e.Row.FindControl("litSaida") as Literal;
                    litSaida.Text = config.ECOM_REL_CONFIG1.NOME;

                    Button btExcluir = e.Row.FindControl("btExcluir") as Button;
                    btExcluir.CommandArgument = config.CODIGO.ToString();
                }
            }
        }
        protected void gvConfig_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = gvConfig.FooterRow;
            if (_footer != null)
            {
            }
        }

        protected void btExcluir_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;

            try
            {
                labErro.Text = "";

                var codigo = Convert.ToInt32(bt.CommandArgument);
                ecomController.ExcluirEcomRelConfigDEPARA(codigo);

                CarregarRegrasRel();

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        protected void btSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (ddlRegraEntrada.SelectedValue == "0")
                {
                    labErro.Text = "Selecione a Regra de Entrada";
                    return;
                }

                if (ddlRegraSaida.SelectedValue == "0")
                {
                    labErro.Text = "Selecione a Regra de Saída";
                    return;
                }

                if (txtTopQtde.Text == "" || Convert.ToInt32(txtTopQtde.Text) < 1)
                {
                    labErro.Text = "Informe a quantidade de produtos da regra. Mínimo 1.";
                    return;
                }

                //validara se existe
                var configDE = Convert.ToInt32(ddlRegraEntrada.SelectedValue);
                var configPARA = Convert.ToInt32(ddlRegraSaida.SelectedValue);
                var regra = ecomController.ObterEcomRelConfigDEPARARel(configDE, configPARA);
                if (regra != null)
                {
                    labErro.Text = "Estas regras já foram relacionadas.";
                    return;
                }

                var configDEPARA = new ECOM_REL_CONFIG_DEPARA();
                configDEPARA.ECOM_REL_CONFIG_DE = configDE;
                configDEPARA.ECOM_REL_CONFIG_PARA = configPARA;
                configDEPARA.DATA_INCLUSAO = DateTime.Now;
                configDEPARA.QTDE_PRODUTO = Convert.ToInt32(txtTopQtde.Text);
                ecomController.InserirEcomRelConfigDEPARA(configDEPARA);

                CarregarRegrasRel();

                ddlRegraEntrada.SelectedValue = "0";
                ddlRegraSaida.SelectedValue = "0";
                txtTopQtde.Text = "";

                labErro.Text = "Regras relacionadas com sucesso.";

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        protected void gvConfig_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<ECOM_REL_CONFIG_DEPARA> produtos = ObterEcomRelConfigDEPARA();

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            hidOrdem.Value = sortDirection;
            hidCampo.Value = e.SortExpression;

            produtos = produtos.OrderBy(e.SortExpression + sortDirection);
            gvConfig.DataSource = produtos;
            gvConfig.DataBind();

        }

    }
}
