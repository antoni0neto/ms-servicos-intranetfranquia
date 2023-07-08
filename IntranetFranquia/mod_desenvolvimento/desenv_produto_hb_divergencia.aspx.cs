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

namespace Relatorios
{
    public partial class desenv_produto_hb_divergencia : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        int coluna = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregarColecoes();
            }
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {

            labErro.Text = "";
            try
            {
                if (ddlColecoes.SelectedValue == "")
                {
                    labErro.Text = "Selecione uma Coleção.";
                    return;
                }

                RecarregarProdutoHB();

                Session["COLECAO"] = ddlColecoes.SelectedValue;
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }
        protected void gvProdutoHB_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    VW_OBTER_HB_MODELO_POR_PEDIDO _produto = e.Row.DataItem as VW_OBTER_HB_MODELO_POR_PEDIDO;

                    coluna += 1;
                    if (_produto != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = coluna.ToString();
                    }
                }
            }
        }

        #region "DADOS INICIAIS"
        private void CarregarColecoes()
        {
            BaseController _base = new BaseController();
            List<COLECOE> _colecoes = _base.BuscaColecoes();
            if (_colecoes != null)
            {
                _colecoes.Insert(0, new COLECOE { COLECAO = "", DESC_COLECAO = "Selecione" });
                ddlColecoes.DataSource = _colecoes;
                ddlColecoes.DataBind();

                if (Session["COLECAO"] != null)
                    ddlColecoes.SelectedValue = Session["COLECAO"].ToString();
            }
        }
        private void RecarregarProdutoHB()
        {
            //Obter lista de produtos/hb por Pedido
            List<VW_OBTER_HB_MODELO_POR_PEDIDO> _produtoHB = new List<VW_OBTER_HB_MODELO_POR_PEDIDO>();
            _produtoHB = desenvController.ObterHBModeloPorPedido().Where(p => p.COLECAO.Trim() == ddlColecoes.SelectedItem.Text.Trim().ToUpper()).ToList();

            //HB
            if (txtHB.Text.Trim() != "")
                _produtoHB = _produtoHB.Where(p => p.HB == Convert.ToInt32(txtHB.Text)).ToList();

            //MODELO
            if (txtModelo.Text.Trim() != "")
                _produtoHB = _produtoHB.Where(p => p.MODELO.Contains(txtModelo.Text.Trim().ToUpper())).ToList();

            //NOME
            if (txtNome.Text.Trim() != "")
                _produtoHB = _produtoHB.Where(p => p.NOME.Contains(txtNome.Text.Trim().ToUpper())).ToList();

            //NUMERO DO PEDIDO
            if (txtNumeroPedido.Text.Trim() != "")
                _produtoHB = _produtoHB.Where(p => p.NUMERO_PEDIDO == Convert.ToInt32(txtNumeroPedido.Text)).ToList();

            //MOSTRUARIO
            if (ddlMostruario.SelectedValue != "")
            {
                if (ddlMostruario.SelectedValue != "V")
                    _produtoHB = _produtoHB.Where(p => p.MOSTRUARIO == ddlMostruario.SelectedValue).ToList();
                else
                    _produtoHB = _produtoHB.Where(p => p.MOSTRUARIO == "").ToList();
            }

            //STATUS
            if (ddlStatus.SelectedValue != "")
                _produtoHB = _produtoHB.Where(p => p.STATUS == ddlStatus.SelectedValue).ToList();

            gvProdutoHB.DataSource = _produtoHB.OrderBy(o => o.NUMERO_PEDIDO);
            gvProdutoHB.DataBind();
        }
        #endregion
    }
}
