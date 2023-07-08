using DAL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq.Dynamic;
using System.Text;
using System.IO;

namespace Relatorios
{
    public partial class desenv_pedido_aviamento_faltv3 : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                CarregarColecoes();
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private void CarregarColecoes()
        {
            var _colecoes = baseController.BuscaColecoes();
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
            if (ddlColecoes.SelectedValue == "")
                Session["COLECAO"] = null;
            else
                Session["COLECAO"] = ddlColecoes.SelectedValue;

        }

        #endregion

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labMsg.Text = "";

                if (ddlColecoes.SelectedValue == "")
                {
                    labMsg.Text = "Selecione a Coleção";
                    return;
                }

                CarregarMaterialHBFaltante(ddlColecoes.SelectedValue.Trim(), txtHB.Text.Trim(), txtProduto.Text.Trim(), "", "");
            }
            catch (Exception ex)
            {
                labMsg.Text = ex.Message;
            }

        }
        private void CarregarMaterialHBFaltante(string colecao, string hb, string produto, string grupoMaterial, string subGrupoMaterial)
        {

            var materialPedido = ObterMaterialHBFaltante(colecao, hb, produto, grupoMaterial, subGrupoMaterial);

            gvMaterialFalt.DataSource = materialPedido;
            gvMaterialFalt.DataBind();

        }
        private List<SP_OBTER_MATERIAL_HB_FALTV2Result> ObterMaterialHBFaltante(string colecao, string hb, string produto, string grupoMaterial, string subGrupoMaterial)
        {
            var materialPedido = desenvController.ObterMaterialHBFaltanteV2(colecao, hb, produto, grupoMaterial, subGrupoMaterial);

            if (ddlFaltaCompra.SelectedValue != "")
                materialPedido = materialPedido.Where(p => p.FALTA_COMPRA == ddlFaltaCompra.SelectedValue).ToList();

            return materialPedido;
        }
        protected void gvMaterialFalt_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_MATERIAL_HB_FALTV2Result matPedido = e.Row.DataItem as SP_OBTER_MATERIAL_HB_FALTV2Result;

                    if (matPedido != null)
                    {
                    }
                }
            }
        }


    }
}

