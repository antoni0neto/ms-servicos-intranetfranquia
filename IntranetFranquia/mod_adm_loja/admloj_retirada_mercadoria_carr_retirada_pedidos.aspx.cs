using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.DataVisualization.Charting;
using DAL;

namespace Relatorios
{
    public partial class admloj_retirada_mercadoria_carr_retirada_pedidos : System.Web.UI.Page
    {
        UsuarioController usuController = new UsuarioController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregarFilial();
            }
        }

        private void CarregarFilial()
        {
            if (Session["USUARIO"] == null)
                Response.Redirect("~/Login.aspx");
            else
            {
                USUARIO usuario = (USUARIO)Session["USUARIO"];

                List<FILIAI> lstFilial = new List<FILIAI>();
                lstFilial = baseController.BuscaFiliais(usuario).Where(p => p.TIPO_FILIAL.Trim() == "LOJA").ToList();

                if (lstFilial.Count > 0)
                {
                    lstFilial.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "" });
                    ddlFilial.DataSource = lstFilial;
                    ddlFilial.DataBind();

                    if (lstFilial.Count == 2)
                    {
                        ddlFilial.SelectedIndex = 1;
                        ddlFilial.Enabled = false;
                    }
                }
            }
        }

        private List<PRODUTO_TRANSFERENCIA> ObterProdutoTransferenciaPorArquivo()
        {
            var trans = baseController.ObterProdutoTransferenciaPorArquivo();

            if (ddlFilial.SelectedValue != "")
                trans = trans.Where(p => p.CODIGO_FILIAL.Trim() == ddlFilial.SelectedValue.Trim()).ToList();

            if (ddlPedidoAberto.SelectedValue != "")
            {
                if (ddlPedidoAberto.SelectedValue == "S")
                    trans = trans.Where(p => p.DATA_ENVIO == null).ToList();
                else
                    trans = trans.Where(p => p.DATA_ENVIO != null).ToList();
            }

            return trans;
        }
        private void CarregarProdutosTrans()
        {
            gvPedidoTransferencia.DataSource = ObterProdutoTransferenciaPorArquivo();
            gvPedidoTransferencia.DataBind();
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                CarregarProdutosTrans();
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        protected void gvPedidoTransferencia_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    PRODUTO_TRANSFERENCIA trans = e.Row.DataItem as PRODUTO_TRANSFERENCIA;

                    if (trans != null)
                    {

                        Literal litDataSol = e.Row.FindControl("litDataSol") as Literal;
                        litDataSol.Text = Convert.ToDateTime(trans.DATA_INCLUSAO).ToString("dd/MM/yyyy");

                        Literal litFilial = e.Row.FindControl("litFilial") as Literal;
                        litFilial.Text = baseController.BuscaFilialCodigo(Convert.ToInt32(trans.CODIGO_FILIAL)).FILIAL.Trim();

                        Literal litDataEnvio = e.Row.FindControl("litDataEnvio") as Literal;
                        litDataEnvio.Text = (trans.DATA_ENVIO == null) ? "-" : Convert.ToDateTime(trans.DATA_ENVIO).ToString("dd/MM/yyyy");

                        Button btAbrir = e.Row.FindControl("btAbrir") as Button;
                        btAbrir.CommandArgument = trans.CODIGO.ToString();

                        Button btVisualizar = e.Row.FindControl("btVisualizar") as Button;
                        btVisualizar.CommandArgument = trans.CODIGO.ToString();

                        Button btExcluir = e.Row.FindControl("btExcluir") as Button;
                        btExcluir.CommandArgument = trans.CODIGO.ToString();
                        if (trans.DATA_ENVIO != null || !ddlFilial.Enabled)
                            btExcluir.Visible = false;


                    }
                }
            }
        }
        protected void gvPedidoTransferencia_DataBound(object sender, EventArgs e)
        {
        }

        protected void btAbrir_Click(object sender, EventArgs e)
        {
            try
            {
                Button bt = (Button)sender;
                if (bt != null)
                {
                    string codigoTrans = bt.CommandArgument.ToString();
                    Response.Redirect(("admloj_retirada_mercadoria.aspx?sol=1&ct=" + codigoTrans), "_blank", "");
                }
            }
            catch (Exception)
            {
            }
        }

        protected void btExcluir_Click(object sender, EventArgs e)
        {
            try
            {
                Button bt = (Button)sender;
                if (bt != null)
                {
                    var codigoTrans = bt.CommandArgument;

                    //excluir produtos da sol
                    var produtoSol = baseController.ObterProdutoTransferenciaSol(codigoTrans);
                    foreach (var l in produtoSol)
                        baseController.ExcluirProdutoTransferenciaSol(l.CODIGO);

                    var produtoItem = baseController.ObterProdutoTransferenciaItem(codigoTrans);
                    foreach (var l in produtoItem)
                        baseController.ExcluirProdutoTransferenciaItem(l.CODIGO);

                    baseController.ExcluirProdutoTransferencia(Convert.ToInt32(codigoTrans));
                }

                CarregarProdutosTrans();
            }
            catch (Exception)
            {
            }
        }

        protected void btVisualizar_Click(object sender, EventArgs e)
        {
            try
            {
                Button bt = (Button)sender;
                if (bt != null)
                {
                    string codigoTrans = bt.CommandArgument.ToString();
                    Response.Redirect(("admloj_retirada_mercadoria_carr_retirada_visual.aspx?ct=" + codigoTrans), "_blank", "");
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
