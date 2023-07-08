using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using Excel = Microsoft.Office.Interop.Excel;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.IO;
using System.Collections;

namespace Relatorios
{
    public partial class admloj_retirada_mercadoria_carr_retirada_visual : System.Web.UI.Page
    {
        DesenvolvimentoController desenvController = new DesenvolvimentoController();
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {

                if (Request.QueryString["ct"] == null || Request.QueryString["ct"] == "")
                    return;

                CarregarItens(Request.QueryString["ct"].ToString());
            }
        }

        #region "PRODUTO"
        private void CarregarItens(string codigoTransferencia)
        {
            var listaSol = baseController.ObterProdutoTransferenciaQtde(Convert.ToInt32(codigoTransferencia), "", "", "", 2);

            gvProduto.DataSource = listaSol;
            gvProduto.DataBind();
        }
        protected void gvProduto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_PRODUTOTRANSF_QTDEResult produtoSol = e.Row.DataItem as SP_OBTER_PRODUTOTRANSF_QTDEResult;

                    if (produtoSol != null)
                    {

                        var foto1 = "/Fotos/" + produtoSol.PRODUTO.Trim() + produtoSol.COR.Trim() + ".png";
                        var foto2 = "/Fotos/" + produtoSol.PRODUTO.Trim() + produtoSol.COR.Trim() + ".jpg";

                        ImageButton _imgProduto = e.Row.FindControl("imgProduto") as ImageButton;
                        if (File.Exists(Server.MapPath(foto1)))
                            _imgProduto.ImageUrl = foto1;
                        else if (File.Exists(Server.MapPath(foto2)))
                            _imgProduto.ImageUrl = foto2;
                        else
                            _imgProduto.ImageUrl = "/Fotos/sem_foto.png";

                        Label labCor = e.Row.FindControl("labCor") as Label;
                        labCor.Text = prodController.ObterCoresBasicas(produtoSol.COR).DESC_COR;

                    }
                }
            }
        }


        #endregion




    }
}