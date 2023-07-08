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
    public partial class admfis_verificar_nota_defeito : System.Web.UI.Page
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

                List<FILIAI> filiais = baseController.BuscaFiliais(usuario);

                if (filiais != null)
                {

                    filiais.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "Selecione" });

                    ddlFilial.DataSource = filiais;
                    ddlFilial.DataBind();
                }
            }
        }


        protected void btNotaRetirada_Click(object sender, EventArgs e)
        {

            try
            {
                labErro.Text = "";

                if (ddlFilial.SelectedValue == "")
                {
                    labErro.Text = "Selecione uma Filial.";
                    return;
                }

                var nR = baseController.BuscaNotasRetiradaFechada(Convert.ToInt32(ddlFilial.SelectedValue));

                if (ddlNotaBaixada.SelectedValue != "")
                {
                    if (ddlNotaBaixada.SelectedValue == "S")
                        nR = nR.Where(p => p.BAIXADO == true).ToList();
                    else
                        nR = nR.Where(p => p.BAIXADO == false).ToList();
                }

                gvNotaRetirada.DataSource = nR;
                gvNotaRetirada.DataBind();
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void gvNotaRetirada_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    NOTA_RETIRADA notaRetirada = e.Row.DataItem as NOTA_RETIRADA;

                    if (notaRetirada != null)
                    {

                        Literal litEmissao = e.Row.FindControl("litEmissao") as Literal;
                        litEmissao.Text = Convert.ToDateTime(notaRetirada.DATA_NOTA_CMAX).ToString("dd/MM/yyyy");

                        Literal litCDLugzy = e.Row.FindControl("litCDLugzy") as Literal;
                        litCDLugzy.Text = (notaRetirada.NUMERO_NOTA_LUGZI != "0") ? notaRetirada.NUMERO_NOTA_LUGZI : ((notaRetirada.NUMERO_NOTA_HBF != "0") ? notaRetirada.NUMERO_NOTA_HBF : "");

                        Literal litCDLugzyTran = e.Row.FindControl("litCDLugzyTran") as Literal;
                        litCDLugzyTran.Text = ((notaRetirada.NUMERO_NOTA_LUGZI_TRAN != "0") ? notaRetirada.NUMERO_NOTA_LUGZI_TRAN : "");

                        Literal litCmaxNova = e.Row.FindControl("litCmaxNova") as Literal;
                        litCmaxNova.Text = ((notaRetirada.NUMERO_NOTA_CMAX != "0") ? notaRetirada.NUMERO_NOTA_CMAX : "");

                        Literal litCmaxNovaMostruario = e.Row.FindControl("litCmaxNovaMostruario") as Literal;
                        litCmaxNovaMostruario.Text = ((notaRetirada.NUMERO_NOTA_CMAX_MOSTR != "0") ? notaRetirada.NUMERO_NOTA_CMAX_MOSTR : "");

                        Literal litCmaxNovaTran = e.Row.FindControl("litCmaxNovaTran") as Literal;
                        litCmaxNovaTran.Text = ((notaRetirada.NUMERO_NOTA_CMAX_TRAN != "0") ? notaRetirada.NUMERO_NOTA_CMAX_TRAN : "");

                        Literal litCDLucianaTran = e.Row.FindControl("litCDLucianaTran") as Literal;
                        litCDLucianaTran.Text = ((notaRetirada.NUMERO_NOTA_LUCIANA_TRAN != "0") ? notaRetirada.NUMERO_NOTA_LUCIANA_TRAN : "");

                        Literal litCDMostruario = e.Row.FindControl("litCDMostruario") as Literal;
                        litCDMostruario.Text = ((notaRetirada.NUMERO_NOTA_MOSTRUARIO != "0") ? notaRetirada.NUMERO_NOTA_MOSTRUARIO : "");

                        Literal litCDTagzy = e.Row.FindControl("litCDTagzy") as Literal;
                        litCDTagzy.Text = ((notaRetirada.NUMERO_NOTA_TAGZY != "0") ? notaRetirada.NUMERO_NOTA_TAGZY : "");

                        Button _btBaixar = e.Row.FindControl("btBaixar") as Button;
                        if (_btBaixar != null)
                        {
                            _btBaixar.Enabled = true;
                            if (Convert.ToBoolean(notaRetirada.BAIXADO))
                            {
                                _btBaixar.Enabled = false;
                                _btBaixar.Text = "Baixado";
                            }

                            _btBaixar.CommandArgument = notaRetirada.CODIGO_NOTA_RETIRADA.ToString();
                        }

                        Button _btAbrir = e.Row.FindControl("btAbrir") as Button;
                        if (_btAbrir != null)
                        {
                            _btAbrir.CommandArgument = notaRetirada.CODIGO_NOTA_RETIRADA.ToString();
                        }
                    }
                }
            }
        }
        protected void gvNotaRetirada_DataBound(object sender, EventArgs e)
        {
        }

        protected void btBaixar_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;
            if (bt != null)
            {
                string codigoNotaRetirada = bt.CommandArgument.ToString();

                var notaRet = baseController.BuscaNotaRetirada(Convert.ToInt32(codigoNotaRetirada));
                if (notaRet != null)
                {
                    notaRet.BAIXADO = true;
                    usuController.AtualizaNotaRetirada(notaRet);

                    bt.Enabled = false;
                }
            }
        }

        protected void btAbrir_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;
            if (bt != null)
            {
                string codigoNotaRetirada = bt.CommandArgument.ToString();

                Response.Redirect(("admfis_verificar_nota_defeito_produto.aspx?c=" + codigoNotaRetirada), "_blank", "");
            }
        }
    }
}
