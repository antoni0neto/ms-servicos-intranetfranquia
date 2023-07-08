using DAL;
using MySql.Data.MySqlClient;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Relatorios
{
    public partial class ecom_produto_cat_atul : System.Web.UI.Page
    {
        EcomController ecomController = new EcomController();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                hrefVoltar.HRef = "ecom_menu.aspx";
                CarregarCategoriaMag();
            }

            btAtualizarMagento.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btAtualizarMagento, null) + ";");
        }

        private void CarregarCategoriaMag()
        {
            var catMag = ecomController.ObterMagentoGrupoProdutoAberto();
            catMag.Insert(0, new ECOM_GRUPO_PRODUTO { CODIGO = 0, GRUPO = "Selecione" });

            ddlCategoriaMag.DataSource = catMag;
            ddlCategoriaMag.DataBind();
        }

        protected void btAtualizarMagento_Click(object sender, EventArgs e)
        {
            try
            {
                labMsg.Text = "";

                if (ddlCategoriaMag.SelectedValue == "0")
                {
                    labMsg.Text = "Selecione a Categoria Magento.";
                    return;
                }

                EcomMySqlController mysqlController = new EcomMySqlController();

                var produtoCats = ecomController.ObterProdutoCategoriaPorCategoria(Convert.ToInt32(ddlCategoriaMag.SelectedValue));

                foreach (var p in produtoCats)
                {
                    mysqlController.InserirProdutoCategoria(p.ECOM_PRODUTO1.ID_PRODUTO_MAG.ToString(), p.ECOM_GRUPO_PRODUTO.ToString(), "1");
                }

                mysqlController.FecharConexao();

                labMsg.Text = "Produtos adicionados as categorias com sucesso.";


            }
            catch (Exception ex)
            {
                labMsg.Text = ex.Message + ex.InnerException + ex.StackTrace;
            }
        }

    }
}
