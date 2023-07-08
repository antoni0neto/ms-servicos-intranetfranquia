using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.DataVisualization.Charting;
using DAL;
using System.IO;

namespace Relatorios
{
    public partial class admfis_verificar_nota_defeito_produto : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                int codigoNotaRetirada = 0;

                if (Request.QueryString["c"] == null || Request.QueryString["c"] == "" || Session["USUARIO"] == null)
                    Response.Redirect("admfis_menu.aspx");

                codigoNotaRetirada = Convert.ToInt32(Request.QueryString["c"].ToString());

                CarregarProdutos(codigoNotaRetirada);

                hidCodigoNotaRetirada.Value = codigoNotaRetirada.ToString();

                var usu = new UsuarioController().BuscaNotaRetirada(codigoNotaRetirada);
                if (usu != null)
                {
                    if (usu.NUMERO_NOTA_HBF != "0" || usu.NUMERO_NOTA_LUGZI != "0")
                        txtCDLugzy.Text = (usu.NUMERO_NOTA_LUGZI != "0") ? usu.NUMERO_NOTA_LUGZI : ((usu.NUMERO_NOTA_HBF != "0") ? usu.NUMERO_NOTA_HBF : "");

                    if (usu.NUMERO_NOTA_LUGZI_TRAN != "0")
                        txtCDLugzyTransito.Text = usu.NUMERO_NOTA_LUGZI_TRAN;

                    if (usu.NUMERO_NOTA_CMAX != "0")
                        txtCmaxNova.Text = usu.NUMERO_NOTA_CMAX;

                    if (usu.NUMERO_NOTA_CMAX_MOSTR != "0")
                        txtCmaxNovaMostruario.Text = usu.NUMERO_NOTA_CMAX_MOSTR;

                    if (usu.NUMERO_NOTA_CMAX_TRAN != "0")
                        txtCmaxNovaTransito.Text = usu.NUMERO_NOTA_CMAX_TRAN;

                    if (usu.NUMERO_NOTA_LUCIANA_TRAN != "0")
                        txtCDLucianaTransito.Text = usu.NUMERO_NOTA_LUCIANA_TRAN;

                    if (usu.NUMERO_NOTA_MOSTRUARIO != "0")
                        txtCDMostruario.Text = usu.NUMERO_NOTA_MOSTRUARIO;

                    if (usu.NUMERO_NOTA_TAGZY != "0")
                        txtCDTagzy.Text = usu.NUMERO_NOTA_TAGZY;

                }
            }
        }

        private List<SP_OBTER_PRODUTO_NF_RET_DEFResult> ObterProdutoNotaRetirada(int codigoNotaRetirada)
        {
            var produtos = baseController.ObterProdutoNotaRetirada(codigoNotaRetirada);

            return produtos;
        }
        private void CarregarProdutos(int codigoNotaRetirada)
        {
            gvNotaRetiradaProduto.DataSource = ObterProdutoNotaRetirada(codigoNotaRetirada);
            gvNotaRetiradaProduto.DataBind();

        }
        protected void gvNotaRetiradaProduto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_PRODUTO_NF_RET_DEFResult nfRet = e.Row.DataItem as SP_OBTER_PRODUTO_NF_RET_DEFResult;

                    if (nfRet != null)
                    {

                    }
                }
            }
        }
        protected void gvNotaRetiradaProduto_DataBound(object sender, EventArgs e)
        {
        }

        protected void btExcel_Click(object sender, EventArgs e)
        {
            try
            {
                var nomeArquivo = "";

                nomeArquivo = "NF_DEFEITO_" + DateTime.Today.ToString("dd-MM-yyyy") + ".xls";

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", nomeArquivo));
                Response.ContentType = "application/ms-excel";
                //Abaixo codifica os caracteres para o alfabeto latino
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("Windows-1252");
                Response.Charset = "ISO-8859-1";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gvNotaRetiradaProduto.AllowPaging = false;
                gvNotaRetiradaProduto.PageSize = 1000;
                gvNotaRetiradaProduto.DataSource = ObterProdutoNotaRetirada(Convert.ToInt32(hidCodigoNotaRetirada.Value));
                gvNotaRetiradaProduto.DataBind();

                gvNotaRetiradaProduto.HeaderRow.Style.Add("background-color", "#FFFFFF");

                for (int i = 0; i < gvNotaRetiradaProduto.HeaderRow.Cells.Count; i++)
                {
                    gvNotaRetiradaProduto.HeaderRow.Cells[i].Style.Add("background-color", "#CCCCCC");
                    gvNotaRetiradaProduto.HeaderRow.Cells[i].Style.Add("color", "#333333");
                }

                for (int x = 0; x < gvNotaRetiradaProduto.Rows.Count; x++)
                {
                    gvNotaRetiradaProduto.Rows[x].Cells[3].Style.Add("mso-number-format", "\\@");
                }

                gvNotaRetiradaProduto.RenderControl(htw);
                Response.Write(sw.ToString());
                Response.End();

            }
            catch (Exception ex)
            {
            }

        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }

    }
}
