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
    public partial class desenv_pedido_aviamento_falt : System.Web.UI.Page
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
        private void CarregarMaterialHBFaltante(string colecao, string hb, string produto, string nome)
        {

            int hbNum = 0;
            if (hb != "")
                hbNum = Convert.ToInt32(hb);

            var materialPedido = ObterMaterialHBFaltante(colecao, hbNum, produto, nome);

            gvMaterialFalt.DataSource = materialPedido;
            gvMaterialFalt.DataBind();

        }
        protected void gvMaterialFalt_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_MATERIAL_HB_FALTResult matPedido = e.Row.DataItem as SP_OBTER_MATERIAL_HB_FALTResult;

                    if (matPedido != null)
                    {
                        Literal litDataBaixa = e.Row.FindControl("litDataBaixa") as Literal;
                        litDataBaixa.Text = (matPedido.DATA_BAIXA == null) ? "-" : Convert.ToDateTime(matPedido.DATA_BAIXA).ToString("dd/MM/yyyy HH:mm");

                        Button btnBaixar = e.Row.FindControl("btnBaixar") as Button;
                        btnBaixar.CommandArgument = matPedido.COD_MAT_FALTA.ToString();

                        if (matPedido.DATA_BAIXA != null)
                            btnBaixar.Enabled = false;
                    }
                }
            }
        }

        #endregion

        private List<SP_OBTER_MATERIAL_HB_FALTResult> ObterMaterialHBFaltante(string colecao, int hb, string produto, string nome)
        {
            var materialPedido = desenvController.ObterMaterialHBFaltante(colecao, hb, produto, nome);

            if (ddlPedidoRealizado.SelectedValue != "")
            {
                if (ddlPedidoRealizado.SelectedValue == "S")
                    materialPedido = materialPedido.Where(p => p.DATA_BAIXA != null).ToList();
                else
                    materialPedido = materialPedido.Where(p => p.DATA_BAIXA == null).ToList();
            }

            return materialPedido;
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labMsg.Text = "";



                CarregarMaterialHBFaltante(ddlColecoes.SelectedValue.Trim(), txtHB.Text.Trim(), txtProduto.Text.Trim(), txtNome.Text.Trim().ToUpper());
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

                string codigoMatFalta = btnBaixar.CommandArgument;
                //atualizar pedido

                var matfaltante = desenvController.ObterMaterialPedidoFaltante(Convert.ToInt32(codigoMatFalta));
                if (matfaltante != null)
                {
                    matfaltante.DATA_BAIXA = DateTime.Now;
                    desenvController.AtualizarMaterialPedidoFaltante(matfaltante);
                }

                CarregarMaterialHBFaltante(ddlColecoes.SelectedValue.Trim(), txtHB.Text.Trim(), txtProduto.Text.Trim(), txtNome.Text.Trim().ToUpper());
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}

