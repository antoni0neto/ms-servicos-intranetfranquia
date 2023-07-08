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
    public partial class desenv_pedido_aviamento_hb_saq_liberado : System.Web.UI.Page
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
                _colecoes.Insert(0, new COLECOE { COLECAO = "", DESC_COLECAO = "" });

                ddlColecoes.DataSource = _colecoes;
                ddlColecoes.DataBind();
            }
        }

        #endregion

        #region "MATERIAL"
        private void CarregarMaterialHBLib(string colecao, string hb, string produto, string nome)
        {

            int hbNum = 0;
            if (hb != "")
                hbNum = Convert.ToInt32(hb);

            var materialPedido = ObterMaterialHBLib(colecao, hbNum, produto, nome);

            gvMaterialLib.DataSource = materialPedido;
            gvMaterialLib.DataBind();

        }
        protected void gvMaterialLib_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_MATERIAL_HB_LIBERACAOResult matPedido = e.Row.DataItem as SP_OBTER_MATERIAL_HB_LIBERACAOResult;

                    if (matPedido != null)
                    {
                        Literal litDataAviamentoLib = e.Row.FindControl("litDataAviamentoLib") as Literal;
                        litDataAviamentoLib.Text = (matPedido.AVIAMENTO_LIB == null) ? "-" : Convert.ToDateTime(matPedido.AVIAMENTO_LIB).ToString("dd/MM/yyyy HH:mm");

                        Button btnBaixar = e.Row.FindControl("btnBaixar") as Button;
                        btnBaixar.CommandArgument = matPedido.COD_PROD_AVIAMENTO.ToString();

                        if (matPedido.AVIAMENTO_LIB != null)
                            btnBaixar.Enabled = false;
                    }
                }
            }
        }

        #endregion

        private List<SP_OBTER_MATERIAL_HB_LIBERACAOResult> ObterMaterialHBLib(string colecao, int hb, string produto, string nome)
        {
            var materialPedido = desenvController.ObterMaterialHBLib(colecao, hb, produto, nome);

            if (ddlLiberado.SelectedValue != "")
            {
                if (ddlLiberado.SelectedValue == "S")
                    materialPedido = materialPedido.Where(p => p.AVIAMENTO_LIB != null).ToList();
                else
                    materialPedido = materialPedido.Where(p => p.AVIAMENTO_LIB == null).ToList();
            }

            return materialPedido;
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labMsg.Text = "";



                CarregarMaterialHBLib(ddlColecoes.SelectedValue.Trim(), txtHB.Text.Trim(), txtProduto.Text.Trim(), txtNome.Text.Trim().ToUpper());
            }
            catch (Exception ex)
            {
                labMsg.Text = ex.Message;
            }

        }

        protected void btnBaixar_Click(object sender, EventArgs e)
        {
            try
            {
                Button btnBaixar = (Button)sender;

                string codigoProdHBAviamento = btnBaixar.CommandArgument;
                //atualizar pedido

                var aviamentoHB = prodController.ObterAviamentoHBCodigo(Convert.ToInt32(codigoProdHBAviamento));
                if (aviamentoHB != null)
                {
                    aviamentoHB.AVIAMENTO_LIBERADO = DateTime.Now;
                    prodController.AtualizarAviamentoHB(aviamentoHB);
                }

                CarregarMaterialHBLib(ddlColecoes.SelectedValue.Trim(), txtHB.Text.Trim(), txtProduto.Text.Trim(), txtNome.Text.Trim().ToUpper());
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}

